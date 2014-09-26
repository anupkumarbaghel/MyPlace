using MyPlace.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPlace.Domain;
using MyPlace.Domain.Shoping;
using System.IO;
using MyPlace.Web.Helpers;
using System.Drawing;
using MyPlace.Web.Models;
using System.Data;
using Microsoft.AspNet.Identity;
using System.Data.Entity;



namespace MyPlace.Web.Controllers
{
    [Authorize]
    [ValidateInput(false)]
    public class ControlPanelController : BaseController
    {
        public string _UserID
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }
        public bool ShopingFeature
        {
            get
            {
                bool result = false;
                string userid=_UserID;
                if (_db.Feature.Where(e => e.UserRef == userid).FirstOrDefault() != null) result = true;
                return result ;
            }
        }
        public bool MaintainCard
        {
            get
            {
                bool result = false;
                string userid = _UserID;
                HomeDescription desc = _db.HomeDescriptions.Where(e => e.Home.UserID == userid).FirstOrDefault();
                if (desc != null) result = true;
                return result;
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            CpHomeVM model = new CpHomeVM();
            Home home = _db.Homes.Where(e => e.UserID ==_UserID).FirstOrDefault();
            if (home != null)
            { 
                model.ImageUrl = home.ImageUrl;
                model.Description = home.Description;
                model.Type = home.Type;
                return View(model);
            }
            return View(model);
        }

        [HttpPost][ValidateAntiForgeryToken]
        public ActionResult Index(CpHomeVM hmVm)
        {
            if (ModelState.IsValid)
            {
                Home home = _db.Homes.Where(e => e.UserID == _UserID).FirstOrDefault();
                if (home != null)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(Session["frontImagePath"])))
                    home.ImageUrl = Session["frontImagePath"].ToString();
                    home.Description = hmVm.Description;
                    home.Type = hmVm.Type;
                    home.UserID = _UserID;
                    home.ModifiedOn = DateTime.Now;
                    _db.Entry(home).State = EntityState.Modified;
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                Home hm = new Home();
                hm.ImageUrl = Session["frontImagePath"].ToString();
                hm.Description = hmVm.Description;
                hm.Type = hmVm.Type;
                hm.UserID = _UserID;
                hm.NoViewes = 1;
                hm.State = "N";
                hm.CreatedOn = DateTime.Now;
                hm.ModifiedOn = DateTime.Now;
                _db.Homes.Add(hm);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(hmVm);
        }
        [HttpGet]
        public ActionResult CpDescription()
        {
            int HomeID =(int) Session["Description_Home_ID"];
            List<ImageConCollection> imgcoll = new List<ImageConCollection>();
            for (int i = 0; i < 10; i++) imgcoll.Add(new ImageConCollection { ImageUrl = "", Content = "" });

            CpHmDesc model = new CpHmDesc();
            HomeDescription hmdesc = _db.HomeDescriptions.Where(e => e.Home.Id == HomeID).FirstOrDefault(); 
            if (hmdesc != null)
            {
                model.HeaderImageUrl = hmdesc.HeaderImageUrl;
                model.HeaderImageUrl1 = hmdesc.HeaderImageUrl1;
                model.MainHeader = hmdesc.MainHeader;
                model.NotificationHeader = hmdesc.NotificationHeader;
                model.NotificationHeaderSub = hmdesc.NotificationHeaderSub;
                model.ShopingNotification = hmdesc.ShopingNotification;
                model.VideoUrl1 = hmdesc.VideoUrl1;
                model.VideoUrl2 = hmdesc.VideoUrl2;
                model.imgdesc = hmdesc.HmDescImageConCollection.ToList();
            }
            else
            model.imgdesc = imgcoll;
            ViewBag.Shoping = ShopingFeature;
            ViewBag.MaintainCard = MaintainCard;

            model.id = HomeID;
            return View(model);
        }
        [HttpPost][ValidateAntiForgeryToken]
        public ActionResult CpDescription(CpHmDesc hmdescVM)
        {
            Home hm = _db.Homes.Find(hmdescVM.id);
            if (hm == null)
            {
                ViewBag.Shoping = false;
                return View(hmdescVM);
            }
            HomeDescription hmdescPrevious = _db.HomeDescriptions.Where(e => e.Home.Id == hmdescVM.id).FirstOrDefault();

            if (hmdescPrevious != null)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["HmDescMainImage"])))
                    hmdescPrevious.HeaderImageUrl = Session["HmDescMainImage"].ToString();

                if (!string.IsNullOrEmpty(Convert.ToString(Session["HmDescMainImage1"])))
                    hmdescPrevious.HeaderImageUrl1 = Session["HmDescMainImage1"].ToString();
          



                for (int i = 0; i < 10; i++)
                {
                    string imageurl = string.Empty;
                    if (!string.IsNullOrEmpty(Convert.ToString(Session["UploadCollImage" + i])))
                    {
                        imageurl = Session["UploadCollImage" + i].ToString(); Session["UploadCollImage" + i] = null;
                        hmdescPrevious.HmDescImageConCollection.ToList()[i].ImageUrl = imageurl;
                    }
                    string content = hmdescVM.imgdesc[i].Content;
                    hmdescPrevious.HmDescImageConCollection.ToList()[i].Content = content;  
                }



                hmdescPrevious.ModifiedOn = DateTime.Now;
                hmdescPrevious.MainHeader = hmdescVM.MainHeader;
                hmdescPrevious.NotificationHeader = hmdescVM.NotificationHeader;
                hmdescPrevious.NotificationHeaderSub = hmdescVM.NotificationHeaderSub;
                hmdescPrevious.ShopingNotification = hmdescVM.ShopingNotification;
                hmdescPrevious.VideoUrl1 = hmdescVM.VideoUrl1;
                hmdescPrevious.VideoUrl2 = hmdescVM.VideoUrl2;
                _db.Entry(hmdescPrevious).State = EntityState.Modified;
                _db.SaveChanges();
             
                return RedirectToAction("Address");
            }

            List<ImageConCollection> imgcoll = new List<ImageConCollection>();
            for (int i = 0; i < 10; i++)
            {
                string imageurl = string.Empty;
                if (!string.IsNullOrEmpty(Convert.ToString(Session["UploadCollImage" + i])))
                    imageurl = Session["UploadCollImage" + i].ToString();
                Session["UploadCollImage" + i] = null;

                imgcoll.Add(new ImageConCollection
                {
                    ImageUrl = imageurl,
                    Content = hmdescVM.imgdesc[i].Content
                    ,
                    CreatedOn = DateTime.Now
                    ,
                    ModifiedOn = DateTime.Now
                });
            }



            HomeDescription hmdesc = new HomeDescription();
            hmdesc.MainHeader = hmdescVM.MainHeader;
            if (!string.IsNullOrEmpty(Convert.ToString(Session["HmDescMainImage"])))
            hmdesc.HeaderImageUrl = Session["HmDescMainImage"].ToString();
            hmdesc.HmDescImageConCollection = imgcoll;
            hmdesc.NotificationHeader = hmdescVM.NotificationHeader;
            hmdesc.NotificationHeaderSub = hmdescVM.NotificationHeaderSub;
            hmdesc.ShopingNotification = hmdescVM.ShopingNotification;
            hmdesc.VideoUrl1 = hmdescVM.VideoUrl1;
            hmdesc.VideoUrl2 = hmdescVM.VideoUrl2;
            hmdesc.CreatedOn = DateTime.Now;
            hmdesc.ModifiedOn = DateTime.Now;
            hmdesc.Home = hm;
            _db.HomeDescriptions.Add(hmdesc);
            _db.SaveChanges();
          
            return RedirectToAction("Address");
        }
        [HttpGet]
        public ActionResult Address()
        {
            string userid = _UserID;
            var homeid = _db.Homes.Where(e => e.UserID == userid).FirstOrDefault().Id;
            int HmDescid = _db.HomeDescriptions.Where(e => e.Home.Id == homeid).First().Id;
          
         
            Address address = new Address();
            address = _db.Address.Where(e => e.HomeDescription.Id == HmDescid).FirstOrDefault();
            if (address != null)
            {
                CpAddressVM model = new CpAddressVM();
                model.Id = address.Id;
                model.AddressLine1 = address.AddressLine1;
                model.AddressLine2 = address.AddressLine2;
                model.City = address.City;
                model.State = address.State;
                model.Country = address.Country;
                model.PinCode = address.PinCode;
                model.PhoneNo = address.PhoneNo;
                model.LatitudeNLongitude = address.LatitudeNLongitude;
                return View(model);
            }
            CpAddressVM model1 = new CpAddressVM();
            return View(model1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Address(CpAddressVM addressVM)
        {
            if (ModelState.IsValid)
            {
                Address address = _db.Address.Find(addressVM.Id);
                if (address!=null)
                {
                    address.ModifiedOn = DateTime.Now;
                    address.AddressLine1 = addressVM.AddressLine1;
                    address.AddressLine2 = addressVM.AddressLine2;
                    address.City = addressVM.City;
                    address.State = addressVM.State;
                    address.Country = addressVM.Country;
                    address.PinCode = addressVM.PinCode;
                    address.PhoneNo = addressVM.PhoneNo;
                    address.LatitudeNLongitude = addressVM.LatitudeNLongitude;
                    _db.Entry(address).State = EntityState.Modified;
                    _db.SaveChanges();
                    Session["address.HomeDescription.Id"] = address.HomeDescription.Id;
                    return RedirectToAction("DescViaCp", "Home");
                }
                address = new Address();
                string userid = _UserID;
                var homeid = _db.Homes.Where(e => e.UserID == userid).FirstOrDefault().Id;
                int HmDescId = _db.HomeDescriptions.Where(e => e.Home.Id == homeid).First().Id;

               
                address.HomeDescription = _db.HomeDescriptions.Find(HmDescId);
                address.CreatedOn = DateTime.Now;
                address.ModifiedOn = DateTime.Now;
                address.AddressLine1 = addressVM.AddressLine1;
                address.AddressLine2 = addressVM.AddressLine2;
                address.City = addressVM.City;
                address.State = addressVM.State;
                address.Country = addressVM.Country;
                address.PinCode = addressVM.PinCode;
                address.PhoneNo = addressVM.PhoneNo;
                address.LatitudeNLongitude = addressVM.LatitudeNLongitude;
                _db.Address.Add(address);
                _db.SaveChanges();
                Session["address.HomeDescription.Id"] = address.HomeDescription.Id;
                return RedirectToAction("DescViaCp", "Home");
            }

            return View(addressVM);
        }

       

        public ActionResult UploadImage(HttpPostedFileBase frontImage)
        {
            string path = frontImage.ProcessMe("~/Upload/" + User.Identity.GetUserId(), 200, "FrontImage");
            Session["frontImagePath"] = path;
            return Json(path);
        }
        public ActionResult UploadImageDescMain(HttpPostedFileBase HmDescMainImage)
        {
            string path = HmDescMainImage.ProcessMe("~/Upload/" + User.Identity.GetUserId(),600, "HmDescMainImage");
            Session["HmDescMainImage"] = path;
            return Json(path);
        }
        public ActionResult UploadImageDescMain1(HttpPostedFileBase HmDescMainImage1)
        {
            string path = HmDescMainImage1.ProcessMe("~/Upload/" + User.Identity.GetUserId(), 600, "HmDescMainImage1");
            Session["HmDescMainImage1"] = path;
            return Json(path);
        }

        int _imageHeight = 300;
        public ActionResult UploadCollImage0(HttpPostedFileBase HmDescMainImage)
        {
            string i = "0";
            string path = HmDescMainImage.ProcessMe("~/Upload/" + User.Identity.GetUserId(), _imageHeight, "UploadCollImage" + i);
            Session["UploadCollImage" + i] = path;
            return Json(path);
        }
        public ActionResult UploadCollImage1(HttpPostedFileBase HmDescMainImage)
        {
            string i = "1";
            string path = HmDescMainImage.ProcessMe("~/Upload/" + User.Identity.GetUserId(), _imageHeight, "UploadCollImage" + i);
            Session["UploadCollImage"+i] = path;
            return Json(path);
        }
        public ActionResult UploadCollImage2(HttpPostedFileBase HmDescMainImage)
        {
            string i = "2";
            string path = HmDescMainImage.ProcessMe("~/Upload/" + User.Identity.GetUserId(), _imageHeight, "UploadCollImage" + i);
            Session["UploadCollImage" + i] = path;
            return Json(path);
        }
        public ActionResult UploadCollImage3(HttpPostedFileBase HmDescMainImage)
        {
            string i = "3";
            string path = HmDescMainImage.ProcessMe("~/Upload/" + User.Identity.GetUserId(), _imageHeight, "UploadCollImage" + i);
            Session["UploadCollImage" + i] = path;
            return Json(path);
        }
        public ActionResult UploadCollImage4(HttpPostedFileBase HmDescMainImage)
        {
            string i = "4";
            string path = HmDescMainImage.ProcessMe("~/Upload/" + User.Identity.GetUserId(), _imageHeight, "UploadCollImage" + i);
            Session["UploadCollImage" + i] = path;
            return Json(path);
        }
        public ActionResult UploadCollImage5(HttpPostedFileBase HmDescMainImage)
        {
            string i = "5";
            string path = HmDescMainImage.ProcessMe("~/Upload/" + User.Identity.GetUserId(), _imageHeight, "UploadCollImage" + i);
            Session["UploadCollImage" + i] = path;
            return Json(path);
        }
        public ActionResult UploadCollImage6(HttpPostedFileBase HmDescMainImage)
        {
            string i = "6";
            string path = HmDescMainImage.ProcessMe("~/Upload/" + User.Identity.GetUserId(), _imageHeight, "UploadCollImage" + i);
            Session["UploadCollImage" + i] = path;
            return Json(path);
        }
        public ActionResult UploadCollImage7(HttpPostedFileBase HmDescMainImage)
        {
            string i = "7";
            string path = HmDescMainImage.ProcessMe("~/Upload/" + User.Identity.GetUserId(), _imageHeight, "UploadCollImage" + i);
            Session["UploadCollImage" + i] = path;
            return Json(path);
        }
        public ActionResult UploadCollImage8(HttpPostedFileBase HmDescMainImage)
        {
            string i = "8";
            string path = HmDescMainImage.ProcessMe("~/Upload/" + User.Identity.GetUserId(), _imageHeight, "UploadCollImage" + i);
            Session["UploadCollImage" + i] = path;
            return Json(path);
        }
        public ActionResult UploadCollImage9(HttpPostedFileBase HmDescMainImage)
        {
            string i = "9";
            string path = HmDescMainImage.ProcessMe("~/Upload/" + User.Identity.GetUserId(), _imageHeight, "UploadCollImage" + i);
            Session["UploadCollImage" + i] = path;
            return Json(path);
        }
        
       
    }
}
