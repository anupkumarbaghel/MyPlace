using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyPlace.Domain;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyPlace.Web.Models
{
    public class NoteVM
    {
        public int Id { get; set; }
        public virtual string Title { get; set; }
        [AllowHtml]
        [UIHint("tinymce_jquery_classic")]
        public virtual string Content { get; set; }
        
    }
}