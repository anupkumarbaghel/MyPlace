using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyPlace.Domain
{
    public class Home:DomainBase
    {
       
        public virtual string ImageUrl { get; set; }
        [StringLength(55)]
        public virtual string Description { get; set; }
        public virtual string Type { get; set; }
        public virtual string UserID { get; set; }
        public virtual int NoViewes { get; set; }
        public virtual string State { get; set; }
        public virtual ICollection<Category> HmCategory { get; set; }

        public Home()
        {
            HmCategory = new HashSet<Category>();
        }
    }
}
