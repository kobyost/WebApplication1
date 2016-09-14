using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Branch
    {
        [Key]
        public int BranchID { get; set; }

        [DisplayName("Zip Code")]
        [RegularExpression(@"^\d{7}$", ErrorMessage = "Invalid Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(15), RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Pleae enter only alphabetic characters")]
        public string City { get; set; }

        [Required]
        [StringLength(30)]
        public string Address { get; set; }


        [DisplayName("Phone Number")]
        [Required]
        [RegularExpression(@"\d{9,10}", ErrorMessage = "Pleae enter a valid phone number")]
        public string PhoneNumber { get; set; }


        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Pleae enter a valid email")]
        public string Email { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }
}


