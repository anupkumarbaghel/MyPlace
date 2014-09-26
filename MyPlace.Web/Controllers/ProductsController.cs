using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web;
using MyPlace.Domain.Shoping;
using MyPlace.Web.Infrastructure;
using MyPlace.Web.Helpers;
using Microsoft.AspNet.Identity;
using System.Web.Security;

namespace MyPlace.Web.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {

        public string _UserID
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }

        public ActionResult Index([Bind(Exclude = "CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] Product product,FormCollection col)
        {
                //this is used for creating product
                string userid = _UserID;
               string categories = string.Empty;
               if( col["CategoryRef"] !=null)categories= col["CategoryRef"].ToString();

                if (ModelState.IsValid && !string.IsNullOrEmpty(product.Title) && !string.IsNullOrEmpty(categories) )
                {
                    if (Session["ProductImage"] != null)
                        product.ImageUrl = Session["ProductImage"] as string;
                    product.CreatedOn = DateTime.Now;
                    product.ModifiedOn = DateTime.Now;
                    CheckCategory(categories);
                    product.PrCategory = GetCats(categories);
                    product.CreatedBy = User.Identity.Name;
                    _db.Product.Add(product);
                    _db.SaveChanges();
                    MoveImage(product);
                }
           
       
            //finally it list the product to page
               List<Product> model = _db.Product.Where(e => e.PrCategory.Where(u => u.UserRef == userid).Count() > 0).OrderByDescending(e=>e.order).ToList();
               foreach (Product pr in model)
               {
                   var prdet = _db.ProductDetail.Where(e => e.ProductRef.Id == pr.Id).FirstOrDefault();
                   if (prdet != null) pr.IsDetail = true;
                   else pr.IsDetail = false;
               }
               return View(model);
        }

        private List<ProductCategory> GetCats(string categories)
        {
            List<ProductCategory> result = new List<ProductCategory>();
            string[] nums = categories.Split(',');
            foreach (string num in nums)
            {
                int prCatId = Convert.ToInt32(num);
                ProductCategory prcat = _db.ProductCategory.Find(prCatId);
                result.Add(prcat);
            }
            return result;
        }

        private void CheckCategory(string categories)
        {
            try
            {
                string[] result = categories.Split(',');
                for (int i = 0; i < result.Length; i++)
                {
                    int caid = Convert.ToInt32(result[i]);
                    string useridpa = _db.ProductCategory.Find(caid).UserRef;
                    if (useridpa != _UserID) FormsAuthentication.SignOut();
                }
            }
            catch (Exception)
            {

               RedirectToAction("Index", "Home");
            }
          
        }

        private void MoveImage(Product product)
        {
            if (Session["ProductImage"] != null)
            {
                if (System.IO.File.Exists(Server.MapPath(Session["ProductImage"].ToString())))
                {
                    //ones data is saved move image to diffrent folder and also change the path as well
                    string fileCopyDir = "~/Upload/" + User.Identity.GetUserId() + "/Product/" + product.Id;
                    var dir = Server.MapPath(fileCopyDir);
                    System.IO.Directory.CreateDirectory(dir);
                    string filePath = Session["ProductImage"].ToString();
                    string fileCopyPath = fileCopyDir + "/Product.jpg";
                    System.IO.File.Move(Server.MapPath(filePath), Server.MapPath(fileCopyPath));
                    product.ImageUrl = fileCopyPath.TrimStart('~');
                    _db.Entry(product).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            }
          
        }

       

        // GET: Products/Create
        public ActionResult Create()
        {
            string userid = _UserID;
            var result = _db.ProductCategory.Where(e => e.UserRef == userid);
            ViewBag.ProductCategory = result;
            var model = new Product();
            return PartialView(model);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] Product product)
        {
            //if (ModelState.IsValid)
            //{
            //    product.ImageUrl = Session["ProductImage"] as string;
            //    _db.Product.Add(product);
            //    _db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(product);
            string userid = _UserID;
            var result = _db.ProductCategory.Where(e => e.UserRef == userid);
            ViewBag.ProductCategory = result;
            var model = new Product();
            return PartialView(model);
        }

        public ActionResult UploadProductImage(HttpPostedFileBase frontImage)
        {
            string path = frontImage.ProcessMe("~/Upload/" + User.Identity.GetUserId()+"/Product", 250, "Product");
            Session["ProductImage"] = path;
            return Json(path);
        }
        public ActionResult UploadEditProductImage(HttpPostedFileBase frontImage)
        {
            string path = frontImage.ProcessMe("~/Upload/" + User.Identity.GetUserId() + "/Product/" + Session["product.id"].ToString(), 250, "Product");
            Session["ProductEditImage"] = path;
            return Json(path);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session["product.id"] = id;
            Product product = _db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            string userid = _UserID;
            var result = _db.ProductCategory.Where(e => e.UserRef == userid);
            var chkpro = product.PrCategory;
            foreach (var pro in result)
            {
                if (chkpro.Contains(pro))
                    pro.Checked = true;
            }

            ViewBag.ProductCategory = result;
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "ImageUrl,CategoryRef,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] Product product,FormCollection col)
        {
            string userid = _UserID;
            string categories = string.Empty;
            if (col["CategoryRef"] != null) categories = col["CategoryRef"].ToString();

            if (ModelState.IsValid && !string.IsNullOrEmpty(categories))
            {
               Product productedit = _db.Product.Find(product.Id);
               productedit.Title = product.Title;
               productedit.Description = product.Description;
               productedit.Price = product.Price;
               productedit.order = product.order;
               CheckCategory(categories);
               List<ProductCategory> givenCategory = GetCats(categories);
               List<ProductCategory> rmCategories = productedit.PrCategory.ToList();

               for (int i = 0; i < rmCategories.Count(); i++) productedit.PrCategory.Remove(rmCategories[i]);
               for (int i = 0; i < givenCategory.Count(); i++) productedit.PrCategory.Add(givenCategory[i]);



                   productedit.ModifiedBy = User.Identity.Name;
               productedit.ModifiedOn = DateTime.Now;
               _db.Entry(productedit).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
          
            var result = _db.ProductCategory.Where(e => e.UserRef == userid);
            ViewBag.ProductCategory = result;
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = _db.Product.Find(id);
            _db.Product.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Details(int id)
        {
            
            Session["ProductIDForDetail"] = id;//used for image uploading path generation

            ProductDetail model = new ProductDetail();
            var prdetail = _db.ProductDetail.Where(e=>e.ProductRef.Id==id).FirstOrDefault();
            if (prdetail != null)
            {
                model = prdetail;
                Session["ProductDetailID"] = prdetail.Id;
            }
            
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Details(string Specification,string VideoUrl1)
        {
            if (ModelState.IsValid)
            {
                ProductDetail prDetail = new ProductDetail();
                if (Session["ProductDetailID"] == null)//creating
                {
                    if (Session["ProductDetailImage1"] != null)
                        prDetail.ImageUrl1 = Session["ProductDetailImage1"].ToString();
                    if (Session["ProductDetailImage2"] != null)
                        prDetail.ImageUrl2 = Session["ProductDetailImage2"].ToString();
                    if (Session["ProductDetailImage3"] != null)
                        prDetail.ImageUrl3 = Session["ProductDetailImage3"].ToString();
                    if (Session["ProductDetailImage4"] != null)
                        prDetail.ImageUrl4 = Session["ProductDetailImage4"].ToString();
                    prDetail.Specification = Specification;
                    prDetail.VideoUrl1 = VideoUrl1;
                    prDetail.ProductRef = _db.Product.Find((int)Session["ProductIDForDetail"]);
                    prDetail.CreatedOn = DateTime.Now;
                    prDetail.ModifiedOn = DateTime.Now;
                    _db.ProductDetail.Add(prDetail);
                    _db.SaveChanges();
                }
                else
                {//modifieng
                    int productdetailid = (int)Session["ProductDetailID"];
                    prDetail = _db.ProductDetail.Find(productdetailid);

                    if (Session["ProductDetailImage1"] != null)
                        prDetail.ImageUrl1 = Session["ProductDetailImage1"].ToString();
                    if (Session["ProductDetailImage2"] != null)
                        prDetail.ImageUrl2 = Session["ProductDetailImage2"].ToString();
                    if (Session["ProductDetailImage3"] != null)
                        prDetail.ImageUrl3 = Session["ProductDetailImage3"].ToString();
                    if (Session["ProductDetailImage4"] != null)
                        prDetail.ImageUrl4 = Session["ProductDetailImage4"].ToString();

                    prDetail.VideoUrl1 = VideoUrl1;
                    prDetail.Specification = Specification;
                    prDetail.ModifiedOn = DateTime.Now;
                    _db.Entry(prDetail).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            Session["ProductDetailID"] = null;
            Session["ProductDetailImage1"] = null;
            Session["ProductDetailImage2"] = null;
            Session["ProductDetailImage3"] = null;
            Session["ProductDetailImage4"] = null;

            return RedirectToAction("Index");
        }



        int _imageHeight = 400;
        public string GetImageUploadPath()
        {
            string result = string.Empty;
            result = "~/Upload/" + User.Identity.GetUserId() + "/Product/" + Session["ProductIDForDetail"].ToString();
            return result;
        }
        public ActionResult UploadCollImage1(HttpPostedFileBase HmDescMainImage)
        {
            string i = "1";
            string path = HmDescMainImage.ProcessMe(GetImageUploadPath(), _imageHeight, "ProductDetailImage" + i);
            Session["ProductDetailImage" + i] = path;
            return Json(path);
        }
        public ActionResult UploadCollImage2(HttpPostedFileBase HmDescMainImage)
        {
            string i = "2";
            string path = HmDescMainImage.ProcessMe(GetImageUploadPath(), _imageHeight, "ProductDetailImage" + i);
            Session["ProductDetailImage" + i] = path;
            return Json(path);
        }
        public ActionResult UploadCollImage3(HttpPostedFileBase HmDescMainImage)
        {
            string i = "3";
            string path = HmDescMainImage.ProcessMe(GetImageUploadPath(), _imageHeight, "ProductDetailImage" + i);
            Session["ProductDetailImage" + i] = path;
            return Json(path);
        }
        public ActionResult UploadCollImage4(HttpPostedFileBase HmDescMainImage)
        {
            string i = "4";
            string path = HmDescMainImage.ProcessMe(GetImageUploadPath(), 700, "ProductDetailImage" + i);
            Session["ProductDetailImage" + i] = path;
            return Json(path);
        }

       
    }
}
