using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrightIdea.Models{

    public class Like
    {
        [Key]
        public int LikeId {get;set;}
        [ForeignKey("User")]
        public int UserId {get;set;}
        [ForeignKey("Post")]
        public int PostId {get;set;}
        public User Liker {get;set;}
        public Post LikedPost {get;set;}
        public DateTime CreatedAt { get;set; } = DateTime.Now;
        public DateTime UpdatedAt { get;set; } = DateTime.Now;
    }
}