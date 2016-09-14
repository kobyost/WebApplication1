using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [DisplayName("User Name")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Pleae enter a valid user name")]
        public string UserName { get; set; }


        [DisplayName("First Name")]
        [Required]
        [StringLength(10), RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Pleae enter only alphabetic characters")]
        public string FirstName { get; set; }


        [DisplayName("Last Name")]
        [Required]
        [StringLength(10), RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Pleae enter only alphabetic characters")]
        public string LastName { get; set; }


        [DisplayName("Phone Number")]
        [Required]
        [RegularExpression(@"\d{9,10}", ErrorMessage = "Pleae enter a valid phone number")]
        public string PhoneNumber { get; set; }


        [Required]
        [StringLength(15), RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Pleae enter only alphabetic characters")]
        public string City { get; set; }


        [Required]
        [StringLength(30)]
        public string Address { get; set; }


        [DisplayName("Zip Code")]
        [RegularExpression(@"^\d{7}$", ErrorMessage = "Invalid Zip Code")]
        public string ZipCode { get; set; }


        public DateTime BirthDate { get; set; }


        public virtual ICollection<Order> Orders { get; set; }
    }
}