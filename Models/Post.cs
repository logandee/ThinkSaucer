using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BrightIdea.Models{

    public class Post
    {
        [Key]
        public int PostId {get;set;}
        [Required]
        [MinLength(5, ErrorMessage="Your idea must be at least 5 Charaters long")]
        public string Content {get;set;}
        [ForeignKey("User")]
        public int UserId {get;set;}
        public User Creator {get;set;}
        public List<Like> Likes {get;set;}
        public DateTime CreatedAt { get;set; } = DateTime.Now;
        public DateTime UpdatedAt { get;set; } = DateTime.Now;
    }
}