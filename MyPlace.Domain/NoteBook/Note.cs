using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MyPlace.Domain.NoteBook
{
   public class Note:DomainBase
    {
        public virtual string Title { get; set; }
     
        public virtual string Content { get; set; }
        public virtual string ExtraField { get; set; }
        public virtual NoteBook WhichNoteBook { get; set; }
    }
}
