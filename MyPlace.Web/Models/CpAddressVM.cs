using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPlace.Web.Models
{
    public class CpAddressVM
    {
        public virtual int Id { get; set; }
        [StringLength(150)]
        public virtual string AddressLine1 { get; set; }
        [StringLength(150)]
        public virtual string AddressLine2 { get; set; }
        [StringLength(100)]
        [Required]
        public virtual string City { get; set; }
        [StringLength(100)]
        public virtual string State { get; set; }
        [StringLength(100)]
        public virtual string PinCode { get; set; }
        [StringLength(100)]
        public virtual string Country { get; set; }
        [StringLength(15)]
        public virtual string PhoneNo { get; set; }
        [StringLength(100)]
        [Display(Name = "Latitude And Longitude", Prompt = "23.168762, 75.784346")]
        public virtual string LatitudeNLongitude { get; set; }
    }
}