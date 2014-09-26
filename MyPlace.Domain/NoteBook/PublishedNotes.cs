using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MyPlace.Domain.NoteBook
{
   public class PublishedNotes:DomainBase
    {
        public virtual Note NotePublished { get; set; }
    }
}
