using System;
using ActivityLibrary1.Entities;

namespace SportsStore.Models
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}