using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPlace.Web.Infrastructure;

namespace MyPlace.Web.Controllers
{
    public class BaseController : Controller
    {
        protected MyPlaceDB _db = new MyPlaceDB();
        protected override void Dispose(bool disposing)
        {
            if (_db != null)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
