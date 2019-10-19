using BrightIdea.Models;
using System.Collections.Generic;
public class VMW
{
    public User user {get;set;}
    public LoginUser loginUser {get;set;}
    public Post newPost {get;set;}
    public List<Post> posts {get;set;}
    public List<Like> likes {get;set;}
}