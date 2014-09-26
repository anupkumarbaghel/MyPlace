using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyPlace.Domain;

namespace MyPlace.Web.Models
{
    public class HomeViewModel
    {
        public List<Home> homes = new List<Home>(4);
    }
}