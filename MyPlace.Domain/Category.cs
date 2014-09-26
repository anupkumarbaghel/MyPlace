using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPlace.Domain
{
    public class Category:DomainBase
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }

        public virtual ICollection<Home> CategoryHm { get; set; }

        public Category()
        {
            CategoryHm = new HashSet<Home>();
        }
    }
}
