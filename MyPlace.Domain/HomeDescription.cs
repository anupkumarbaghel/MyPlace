using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyPlace.Domain
{
    public class HomeDescription:DomainBase
    {
        public virtual string MainHeader { get; set; }
        public virtual string HeaderImageUrl { get; set; }
        public virtual string HeaderImageUrl1 { get; set; }
        public virtual string NotificationHeader { get; set; }
        public virtual string NotificationHeaderSub { get; set; }
        public virtual string VideoUrl1 { get; set; }
        public virtual string VideoUrl2 { get; set; }
        public virtual string ShopingNotification { get; set; }
        public virtual ICollection<ImageConCollection> HmDescImageConCollection { get; set; }
        public virtual Home Home { get; set; }
    }
}
