using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPlace.Domain
{
   public class Feature:DomainBase
    {
       public virtual string UserRef { get; set; }
       public virtual string Name  { get; set; }
       public virtual bool OnOf { get; set; }
    }
}
