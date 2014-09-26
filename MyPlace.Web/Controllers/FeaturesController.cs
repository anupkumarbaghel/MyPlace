using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPlace.Domain;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace MyPlace.Web.Controllers
{
    public class FeaturesController :BaseController
    {
        public string _UserID
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }
        // GET: Features
        public ActionResult Index()
        {
            string userid=_UserID;
            ViewBag.shonof = false;
            Feature fe = _db.Feature.Where(e => e.UserRef == userid).FirstOrDefault();
            if (fe != null)
            {
                ViewBag.Shoping = true;
                ViewBag.shonof = fe.OnOf;
            }
            else
            {
                ViewBag.Shoping = false;
            }

            int catcount = _db.ProductCategory.Where(e => e.UserRef == userid).Count();
            if (catcount == 0) ViewBag.showproduct = false; else ViewBag.showproduct = true;

            return View();
        }
        [HttpPost]
        public ActionResult Index(string feature)
        {
            string userid = _UserID;

            Feature fe = new Feature();
            fe.UserRef = _UserID;
            fe.Name = feature;
            fe.OnOf = true;
            fe.CreatedOn = DateTime.Now;
            fe.ModifiedOn = DateTime.Now;
            fe.CreatedBy = User.Identity.Name;
            fe.ModifiedBy = User.Identity.Name;
            _db.Feature.Add(fe);
            _db.SaveChanges();

            return View("FeShoapingAdded");
        }

      

    }
}