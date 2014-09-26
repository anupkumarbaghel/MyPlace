using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyPlace.Domain;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyPlace.Web.Models
{
    public class CpHmDesc
    {
        public int id { get; set; }
        public string MainHeader { get; set; }
        public string HeaderImageUrl { get; set; }
        public string HeaderImageUrl1 { get; set; }
        public string NotificationHeader { get; set; }
        [AllowHtml]
        [UIHint("tinymce_jquery_classic_compressed")]
        public string NotificationHeaderSub { get; set; }
        public string VideoUrl1 { get; set; }
        public string VideoUrl2 { get; set; }
        public string ShopingNotification { get; set; }
        public List<ImageConCollection> imgdesc{get;set;}
    }
}