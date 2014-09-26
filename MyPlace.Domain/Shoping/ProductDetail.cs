using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPlace.Domain.Shoping
{
    public class ProductDetail:DomainBase
    {
        public virtual string ImageUrl1 { get; set; }
        public virtual string ImageUrl2 { get; set; }
        public virtual string ImageUrl3 { get; set; }
        public virtual string ImageUrl4 { get; set; }
        public virtual string VideoUrl1 { get; set; }
        public virtual string Specification { get; set; }
        public virtual string AnyDetail { get; set; }
        public Product ProductRef { get; set; }
    }
}
