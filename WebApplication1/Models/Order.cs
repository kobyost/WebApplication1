using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        [DisplayName("Order Date")]
        public DateTime OrderDate { get; set; }

        [DisplayName("Order Price")]
        public decimal OrderPrice { get; set; }

        [Required]
        public int GameTitleID { get; set; }

        [Required]
        public int CustomerID { get; set; }


        public GameTitle GameTitle { get; set; }
        public Customer Customer { get; set; }

    }
}
