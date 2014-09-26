using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyPlace.Domain.Shoping;

namespace MyPlace.Web.Models
{
    public class ProductVM
    {
        public List<Product> p1s = new List<Product>(2);
        public List<Product> p2s = new List<Product>(2);
    }
}