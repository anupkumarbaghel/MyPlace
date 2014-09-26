using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPlace.Domain.NoteBook
{
   public class NoteBook:DomainBase
    {
       public NoteBook()
       {
           order = 1;
       }
        public virtual string UserRef { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual decimal order { get; set; }
        [NotMapped]
        public int NoteCount { get; set; }
    }
}
