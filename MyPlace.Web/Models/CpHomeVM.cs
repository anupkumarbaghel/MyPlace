using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPlace.Web.Models
{
    public class CpHomeVM
    {
        public string ImageUrl { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        [Required]
        public string Type { get; set; }
    }
}