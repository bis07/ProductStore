using System;
using System.Activities.Debugger;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ActivityLibrary1.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter first adress line")]
        [Display(Name = "Line 1")]
        public string Line1 { get; set; }
        [Display(Name = "Line 2")]
        public string Line2 { get; set; }
        [Display(Name = "Line 3")]
        public string Line3 { get; set; }
        [Required(ErrorMessage = "Please enter your city")]
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        [Required(ErrorMessage = "Please enter your country name")]
        public string Country { get; set; }
        public bool GiftWrap { get; set; }
    }
}
