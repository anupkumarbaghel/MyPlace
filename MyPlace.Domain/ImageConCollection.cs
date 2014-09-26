using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPlace.Domain
{
    public class ImageConCollection:DomainBase
    {
        public virtual string ImageUrl { get; set; }
        public virtual string Content { get; set; }
    }
}
