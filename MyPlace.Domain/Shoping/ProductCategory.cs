using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPlace.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPlace.Domain.Shoping
{
   public class ProductCategory:MyPlace.Domain.DomainBase
    {
        public virtual string UserRef { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual decimal order { get; set; }
       [NotMapped]
        public int ProductCount  { get; set; }
       [NotMapped]
       public bool Checked { get; set; }
       public virtual ICollection<Product> CategoryPr { get; set; }

       public ProductCategory()
        {
            CategoryPr = new HashSet<Product>();
        }
    }
}
