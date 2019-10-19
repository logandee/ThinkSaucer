using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BrightIdea.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace BrightIdea.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("Create")]
        public IActionResult Create(VMW Submission)
        {   
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == Submission.user.Email))
                {
                // Manually add a ModelState error to the Email field
                ModelState.AddModelError("user.Email", "Email already in use!");
                return View("Index");
                
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                Submission.user.Password = Hasher.HashPassword(Submission.user, Submission.user.Password);
                dbContext.Add(Submission.user);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserId", Submission.user.UserId);
                return RedirectToAction("bright_idea");
            }
            else
            {
                return View("Index");
            }
        } 
        [HttpPost("Login")]       
        public IActionResult Login(VMW Submission)
        {
            if(ModelState.IsValid)
            {
                User userFromDb = dbContext.Users.FirstOrDefault(u => u.Email == Submission.loginUser.Email);
                if(userFromDb == null)
                {
                    ModelState.AddModelError("loginUser.Email", "Invalide Email/Password");
                    return View("Index");
                }

                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(Submission.loginUser, userFromDb.Password, Submission.loginUser.Password);
                if(result == 0)
                {
                    ModelState.AddModelError("loginUser.Email", "Invalide Email/Password");
                    return View("Index"); 
                }
                HttpContext.Session.SetInt32("UserId", userFromDb.UserId);
                return RedirectToAction("bright_idea");
            }
            else
            {
                ModelState.AddModelError("loginUser.Email", "Invalide Email/Password");
                return View("Index");
            }
        }
        [HttpGet("bright_idea")]
        public IActionResult bright_idea()
        {
            int? LocalUserId = HttpContext.Session.GetInt32("UserId");
            if(LocalUserId == null)
            {
                return RedirectToAction("Index");
            }

            User userLoggedIn = dbContext.Users.FirstOrDefault(u => u.UserId == LocalUserId);
            VMW viewModel = new VMW();
            viewModel.user = userLoggedIn;
            viewModel.posts = dbContext.Posts.Include(p => p.Likes).ThenInclude(l => l.Liker).ToList();
            return View(viewModel);
        }
        [HttpGet]
        [Route("bright_idea/new_like/{postid}/{userid}")]
        public IActionResult new_like(int postid, int userid)
        {
            Like newLike = new Like();
            newLike.PostId = postid;
            newLike.UserId = userid;
            dbContext.Add(newLike);
            dbContext.SaveChanges();
            return RedirectToAction("bright_idea");
        }
        [HttpGet]
        [Route("bright_idea/{postid}")]
        public IActionResult ViewPost(int postid)
        {
            int? LocalUserId = HttpContext.Session.GetInt32("UserId");
            if(LocalUserId == null)
            {
                return RedirectToAction("Index");
            }

            User userLoggedIn = dbContext.Users.FirstOrDefault(u => u.UserId == LocalUserId);
            List<Like> theseLikes = dbContext.Likes.Include(l => l.Liker).Where(l => l.PostId == postid).ToList();
            Post thisPost = dbContext.Posts.FirstOrDefault(p => p.PostId == postid);
            VMW viewModel = new VMW();
            viewModel.newPost = thisPost;
            viewModel.user = userLoggedIn;
            viewModel.likes = theseLikes;
            return View(viewModel);
        }
        [HttpGet]
        [Route("bright_idea/delete/{postid}")]
        public IActionResult Remove(int postid)
        {
            int? LocalUserId = HttpContext.Session.GetInt32("UserId");
            if(LocalUserId == null)
            {
                return RedirectToAction("Index");
            }

            User userLoggedIn = dbContext.Users.FirstOrDefault(u => u.UserId == LocalUserId);
            Post thisPost = dbContext.Posts.FirstOrDefault(p => p.PostId == postid);

            if(thisPost.Creator.UserId == userLoggedIn.UserId)
            {
                dbContext.Posts.Remove(thisPost);
                dbContext.SaveChanges();
            }
            return RedirectToAction("bright_idea");
        
        }
        [HttpGet]
        [Route("bright_idea/user/{userid}")]
        public IActionResult ViewUser(int userid)
        {
            int? LocalUserId = HttpContext.Session.GetInt32("UserId");
            if(LocalUserId == null)
            {
                return RedirectToAction("Index");
            }

            User userLoggedIn = dbContext.Users.Include(u => u.CreatedPosts).Include(u => u.Likes).FirstOrDefault(u => u.UserId == userid);
            VMW viewModel = new VMW();
            viewModel.user = userLoggedIn;
            return View(viewModel);
        }

        [HttpPost("NewPost")]
        public IActionResult NewPost(VMW Submission)
        {
            if(ModelState.IsValid)
            {
                Post newPost = Submission.newPost;
                int? LocalUserId = HttpContext.Session.GetInt32("UserId");
                if(LocalUserId == null)
                {
                    return RedirectToAction("Index");
                }
                User userLoggedIn = dbContext.Users.FirstOrDefault(u => u.UserId == LocalUserId);
                newPost.UserId = userLoggedIn.UserId;
                dbContext.Add(newPost);
                dbContext.SaveChanges();
                return RedirectToAction("bright_idea");
            }
            else
            {
                return RedirectToAction("bright_idea");
            } 
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
