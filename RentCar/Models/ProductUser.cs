using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RentCar.Models
{
    public class ProductUser:Products
    {
        [Display(Name = "Prop")]
        [Required]
        public string UserId { get; set; }
        [ForeignKey("PropId")]

        public virtual ApplicationUser USer{ get; set; }
    }
}
