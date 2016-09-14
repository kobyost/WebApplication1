using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class GameTitle
    {
        public int GameTitleID { get; set; }

        [DisplayName("Title")]
        [Required]
        [RegularExpression(@"[a-zA-Z:0-9- ]+$", ErrorMessage = "Not a valid name of game")]
        public string Name { get; set; }

        [DisplayName("Developer")]
        [Required]
        [RegularExpression(@"[a-zA-Z0-9-: ]+$", ErrorMessage = "Not a valid name of company")]
        public string DevelopedBy { get; set; }

        [Required]
        [RegularExpression(@"[a-zA-Z- ]+$", ErrorMessage = "Not a valid name of genre")]
        public string Genre { get; set; }

        [Required]
        [Range(0, 10, ErrorMessage = "Please enter a rating between 0 - 10")]
        public float Rating { get; set; }

        [DisplayName("Release Date")]
        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Range(0, 300, ErrorMessage = "Please enter a price between 0 - 300")]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [RegularExpression(@"[a-zA-Z0-9- ]+$", ErrorMessage = "Not a valid name of platform")]
        public string Platform { get; set; }

        public string ImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}