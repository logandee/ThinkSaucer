using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BrightIdea.Models{

    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required]
        [MinLength(3)]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage="Cannot include numbers or sybols")]
        [Display(Name="Name")]
        public string Name { get;set; }
        [Required]
        [MinLength(3)]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage="Can only include letters and numbers")]
        [Display(Name="Alias")]
        public string Alias { get;set; }
        [EmailAddress]
        [Required]
        public string Email { get;set; }
        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters")]
        public string Password { get;set; }
        public List<Post> CreatedPosts {get;set;}
        public List<Like> Likes {get;set;}
        public DateTime CreatedAt { get;set; } = DateTime.Now;
        public DateTime UpdatedAt { get;set; } = DateTime.Now;
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm { get;set; }

    }
}