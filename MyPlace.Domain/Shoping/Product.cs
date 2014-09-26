using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPlace.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPlace.Domain.Shoping
{
   public class Product:MyPlace.Domain.DomainBase
    {
        public virtual string Title { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal Price { get; set; }
        public virtual decimal order { get; set; }
        public virtual ICollection<ProductCategory> PrCategory { get; set; }
        [NotMapped]
        public bool IsDetail { get; set; }
        public Product()
        {
            PrCategory = new HashSet<ProductCategory>();
        }
    }
}
