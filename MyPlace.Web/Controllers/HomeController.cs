using MyPlace.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPlace.Domain;
using MyPlace.Domain.Shoping;
using MyPlace.Web.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;



namespace MyPlace.Web.Controllers
{
    public class HomeController : BaseController
    {
        public string _UserID
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }

        public ActionResult AdminIndex(string cardNo = null, string city = null, string product = null, string title = null, string type = null, string state = null)
        {
            string userid = _UserID;
            List<HomeViewModel> model = new List<HomeViewModel>();
            HomeViewModel model1 = new HomeViewModel();

            List<int> homeids = new List<int>();
            if (!string.IsNullOrEmpty(cardNo + city + product + title + type + state))
            {
                homeids = GetHomeIdsOnCity(cardNo, city, product, title, type, state).Select(e => e.Id).ToList();
                if (homeids.Count() == 0) @ViewBag.CountResultFound = "No Results Found";
            }


            List<Home> homeList = (from row in _db.Homes
                                   let days = DbFunctions.DiffDays(row.CreatedOn, DateTime.Now)
                                   let likes = row.NoViewes
                                   let homeorder = likes / (days == 0 ? 1 : days)
                                   where (homeids.Count() == 0 || homeids.Contains(row.Id))
                                   orderby homeorder descending
                                   select row).ToList();
            // where row.State=="o" || row.UserID==userid
            if (!string.IsNullOrEmpty(cardNo + city + product + title + type + state))
            {
                if (homeids.Count() != 0)
                    @ViewBag.CountResultFound = homeList.Count() + " Results Found";
            }
            ModelRow4Arrange(model, model1, homeList);//if not then we can't maintain the regular arrangment of card
            //not working now 
            if (Request.IsAjaxRequest())
            {
                return PartialView("_BusinessECardView", model);
            }
            return View(model);
        }
        public ActionResult CardShowHide(string cardshowhide, int cardid)
        {
            Home hm = _db.Homes.Find(cardid);
            if (cardshowhide == "show")
            {
                hm.State = "o";
            }
            if (cardshowhide == "hide")
            {
                hm.State = "h";
            }
            _db.Entry(hm).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Index(string cardNo = null, string city = null, string product = null, string title = null, string type = null)
        {
            string userid = _UserID;
            List<HomeViewModel> model = new List<HomeViewModel>();
            HomeViewModel model1 = new HomeViewModel();

            List<int> homeids = new List<int>();
            if (!string.IsNullOrEmpty(cardNo + city + product + title + type))
            {
                homeids = GetHomeIdsOnCity(cardNo, city, product, title, type).Select(e => e.Id).ToList();
                if (homeids.Count() == 0) @ViewBag.CountResultFound = "No Results Found";
            }


            List<Home> homeList = (from row in _db.Homes
                                   let days = DbFunctions.DiffDays(row.CreatedOn, DateTime.Now)
                                   let likes = row.NoViewes
                                   let homeorder = likes / (days == 0 ? 1 : days)
                                   where (row.State == "o" || row.UserID == userid) &&
                                   (homeids.Count() == 0 || homeids.Contains(row.Id))
                                   orderby homeorder descending
                                   select row).ToList();
            // where row.State=="o" || row.UserID==userid
            if (!string.IsNullOrEmpty(cardNo + city + product + title + type))
            {
                if (homeids.Count() != 0)
                    @ViewBag.CountResultFound = homeList.Count() + " Results Found";
            }
            ModelRow4Arrange(model, model1, homeList);//if not then we can't maintain the regular arrangment of card
            //not working now 
            if (Request.IsAjaxRequest())
            {
                return PartialView("_BusinessECardView", model);
            }
            ViewBag.UserReference = userid;
            return View(model);
        }

        private List<Home> GetHomeIdsOnCity(string cardNo, string city, string product, string title, string type, string state = "")
        {
            List<Home> result = new List<Home>();
            List<Home> reshomeCity = new List<Home>();
            List<Home> reshomeCardno = new List<Home>();
            List<Home> reshomeProduct = new List<Home>();
            List<Home> reshomeTitle = new List<Home>();
            List<Home> reshomeType = new List<Home>();
            List<Home> reshomeState = new List<Home>();

            if (!string.IsNullOrEmpty(cardNo))
            {

                reshomeCardno = (from row in _db.Homes
                                 where cardNo == "" || cardNo == null || row.Id.ToString() == cardNo
                                 select row).ToList();
                return reshomeCardno;
            }

            if (!string.IsNullOrEmpty(city))
            {
                List<string> rescities = EagleThreshHoldSearch(city, _db.Address.Select(e => e.City), 50);
                reshomeCity = (from row in _db.Address
                               where city == "" || city == null || rescities.Contains(row.City)
                               select row.HomeDescription.Home).ToList();
                return reshomeCity;
            }


            if (!string.IsNullOrEmpty(product))
            {
                List<string> resProducts = EagleThreshHoldSearch(product, _db.Product.Select(e => e.Title), 50);

                var respr1 = (from r in _db.Product
                              where product == "" || product == null || resProducts.Contains(r.Title)
                              select r.PrCategory).ToList();
                HashSet<string> pc = new HashSet<string>();
                foreach (ICollection<ProductCategory> r in respr1)
                {
                    foreach (ProductCategory p in r)
                    {
                        //if(!pc.Contains(p.UserRef))
                        pc.Add(p.UserRef);
                    }
                }

                reshomeProduct = (from r in _db.Homes
                                  where pc.Contains(r.UserID)
                                  select r).ToList();
                return reshomeProduct;
            }
            if (!string.IsNullOrEmpty(title))
            {
                List<string> resProducts = EagleThreshHoldSearch(title, _db.Homes.Select(e => e.Description), 40);
                reshomeTitle = (from row in _db.Homes
                                where title == "" || title == null || resProducts.Contains(row.Description)
                                select row).ToList();
                return reshomeTitle;
            }
            if (!string.IsNullOrEmpty(type))
            {
                List<string> resProducts = EagleThreshHoldSearch(type, _db.Homes.Select(e => e.Type), 40);
                reshomeType = (from row in _db.Homes
                               where type == "" || type == null || resProducts.Contains(row.Type)
                               select row).ToList();
                return reshomeType;
            }
            if (!string.IsNullOrEmpty(state))
            {
                reshomeState = (from row in _db.Homes
                                where state == "" || state == null || row.State == state
                                select row).ToList();
                return reshomeState;
            }

            return result;
        }

        private List<string> EagleThreshHoldSearch(string ToSearch, IEnumerable<string> SearchSource, int threshHoldRating = 30)
        {
            List<string> result = new List<string>();
            foreach (string item in SearchSource)
            {
                int localRating = ProvideRating(ToSearch, item);
                if (localRating >= threshHoldRating) result.Add(item);
            }
            return result;
        }

        private int ProvideRating(string ToSearch, string FromSearch)
        {
            int result = 0; int globalSumOfRating = 0;
            string[] toSearchArray = ToSearch.Split(' ');
            string[] fromSearchArray = FromSearch.Split(' ');

            foreach (string oneWord in toSearchArray)
            {
                int ratingOfOneWord = 0;
                foreach (string anotherword in fromSearchArray)
                {
                    int localRating = LetterSequenceMatchRating(oneWord.ToLower(), anotherword.ToLower());
                    ratingOfOneWord = localRating >= ratingOfOneWord ? localRating : ratingOfOneWord;//highest rating should be considered

                }
                globalSumOfRating += ratingOfOneWord;//for average we take the sum
            }
            //Sequence rating formula can also be added for more accurate search (if any one can try)
            int avgRating = (globalSumOfRating * 100) / (toSearchArray.Length * 100);
            result = avgRating;

            return result;
        }
        private int LetterSequenceMatchRating(string oneWord, string anotherWord)
        {
            int result = 0;
            if (oneWord == anotherWord) return 100;//if that waord match rating is 100 thats what we want most

            if (oneWord.Length > 1)
            {
                for (int i = oneWord.Length; i >= 2; i--)
                {
                    string onewordsublevel = oneWord.Substring(0, i);//word fall by level; moves from best to worst and try to find best match and rate them
                    if (anotherWord.Contains(onewordsublevel))
                    {
                        int total = anotherWord.Length > oneWord.Length ? anotherWord.Length : oneWord.Length;
                        int satisfied = onewordsublevel.Length;
                        int percentageSatisfied = (satisfied * 100) / total;
                        result = percentageSatisfied;
                        break;
                    }
                }
            }

            return result;
        }

        private static HomeViewModel ModelRow4Arrange(List<HomeViewModel> model, HomeViewModel model1, List<Home> homeList)
        {
            int i = 0; int totalitem = homeList.Count();
            foreach (var item in homeList)
            {
                i++;
                model1.homes.Add(item);
                int noofitems = model1.homes.Count();
                if (noofitems == 4)
                {
                    model.Add(model1);
                    model1 = new HomeViewModel();
                }
                else
                {
                    if (i == totalitem)
                    {
                        model.Add(model1);
                    }
                }
            }
            return model1;
        }

        public ActionResult Description(int id)
        {
            Home hm = _db.Homes.Find(id);
            if (User.Identity.IsAuthenticated)
            {
                if (hm.UserID == _UserID)
                {
                    Session["Description_Home_ID"] = id;
                    return RedirectToAction("CpDescription", "ControlPanel");
                }
            }
            HomeDescription model = new HomeDescription();
            HomeDescription hmdesc = _db.HomeDescriptions.Where(e => e.Home.Id == id).FirstOrDefault();
            if (hmdesc != null)
                model = hmdesc;

            Address address = _db.Address.Where(e => e.HomeDescription.Id == hmdesc.Id).FirstOrDefault();
            if (address != null)
            {
                ViewBag.Address = address;
                Session["LatitudeNLongtitude"] = address.LatitudeNLongitude + "," + hm.Description;
            }
            else ViewBag.Address = new Address();
            ///only for increasing the no. of views////
            hm.NoViewes = hm.NoViewes + 1;
            _db.Entry(hm).State = EntityState.Modified;
            _db.SaveChanges();
            ///only for increasing the no. of views////
            int homedescid = hmdesc != null ? hmdesc.Id : -1;
            ViewBag.Shoping = ShopingFeature(homedescid);
            return View(model);
        }
        public bool ShopingFeature(int id)
        {
            bool result = false;
            if (id != -1)
            {
                string userid = _db.HomeDescriptions.Find(id).Home.UserID;
                var isShopingFeature = _db.Feature.Where(e => e.UserRef == userid).FirstOrDefault();
                if (isShopingFeature != null) result = true;
            }
            return result;
        }
        public ActionResult DescViaCp()
        {
            int HmDescID = (int)Session["address.HomeDescription.Id"];
            HomeDescription model = _db.HomeDescriptions.Find(HmDescID);

            Address address = _db.Address.Where(e => e.HomeDescription.Id == HmDescID).FirstOrDefault();
            if (address != null)
            {
                ViewBag.Address = address;
                Session["LatitudeNLongtitude"] = address.LatitudeNLongitude + "," + model.Home.Description;
            }
            else ViewBag.Address = new Address();



            ViewBag.Shoping = ShopingFeature(HmDescID);
            return View("Description", model);
        }
        public ActionResult GetLonNLet()
        {
            string result = string.Empty;
            if (Session["LatitudeNLongtitude"] != null) result = Session["LatitudeNLongtitude"].ToString();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public string GetUserIDOnDescID(string DescriptionID)
        {
            try
            {
                string result = string.Empty;
                var description = _db.HomeDescriptions.Find(Convert.ToInt32(DescriptionID));
                result = description.Home.UserID;
                return result;
            }
            catch (Exception)
            {
                return "null";
            }

        }
        public ActionResult OnlineShoaping(string id, string val)
        {
            ///Category
            string ProductCategoryToShow = string.Empty;
            ViewBag.DescId = id;
            string userid = GetUserIDOnDescID(id);
            ViewBag.TopMessage = _db.Homes.Where(e => e.UserID == userid).FirstOrDefault().Description;
            IEnumerable<ProductCategory> ProductCategory = _db.ProductCategory.Where(e => e.UserRef == userid).OrderByDescending(e => e.order).ToList();
            //to check weather product belong to category or not
            foreach (var pcat in ProductCategory)
            {
                string rowid = pcat.Id.ToString();
                if (rowid == val) ProductCategoryToShow = val;

                pcat.ProductCount = pcat.CategoryPr.Count();
            }
            ViewBag.Category = ProductCategory;

            ///product
            List<ProductVM> model = new List<ProductVM>();
            if (ProductCategory.FirstOrDefault() != null)
            {
                string currentcat = ProductCategory.FirstOrDefault().Id.ToString();
                if (!string.IsNullOrEmpty(ProductCategoryToShow)) currentcat = ProductCategoryToShow;
                ViewBag.ActiveCat = currentcat;
                int currcat = Convert.ToInt32(currentcat);
                ProductCategory selectedcat = _db.ProductCategory.Find(currcat);
                ProductVM model1 = new ProductVM();
                ICollection<Product> prdts = selectedcat.CategoryPr.OrderByDescending(e => e.order).ToList();
                foreach (Product pr in prdts)
                {
                    var prdet = _db.ProductDetail.Where(e => e.ProductRef.Id == pr.Id).FirstOrDefault();
                    if (prdet != null) pr.IsDetail = true;
                    else pr.IsDetail = false;
                }
                int i = 0, totalcount = prdts.Count();
                foreach (var row in prdts)
                {
                    i++;
                    if (model1.p1s.Count() < 2)
                    {
                        model1.p1s.Add(row);
                    }
                    else
                    {
                        if (model1.p2s.Count() < 2)
                        {
                            model1.p2s.Add(row);
                            if (model1.p2s.Count() == 2)
                            {
                                model.Add(model1);
                                model1 = new ProductVM();
                            }
                        }

                    }
                    if (i == totalcount && model1.p1s.Count() != 0)
                    {
                        model.Add(model1);
                    }
                }
            }


            ///view
            return View(model);
        }
        public ActionResult ProductDetail(int id)
        {
            ProductDetail model = _db.ProductDetail.Where(e => e.ProductRef.Id == id).FirstOrDefault();
            if (model == null) return RedirectToAction("OnlineShoaping");
            return View(model);
        }




        public ActionResult DontGoOffline()
        {

            return Json("anup", JsonRequestBehavior.AllowGet);

        }

    }
}
