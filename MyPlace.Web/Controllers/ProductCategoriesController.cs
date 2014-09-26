using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyPlace.Domain.Shoping;
using MyPlace.Web.Infrastructure;
using Microsoft.AspNet.Identity;

namespace MyPlace.Web.Controllers
{
    [Authorize]
    public class ProductCategoriesController : BaseController

    {
        public string _UserID
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }

        // GET: ProductCategories
        public ActionResult Index([Bind(Exclude = "Id,UserRef,ImageUrl,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] ProductCategory productCategory)
        {
            if (ModelState.IsValid&&!string.IsNullOrEmpty(productCategory.Title))
            {
                productCategory.UserRef = _UserID;
                productCategory.CreatedBy = User.Identity.Name;
                productCategory.ModifiedBy = User.Identity.Name;
                productCategory.CreatedOn = DateTime.Now;
                productCategory.ModifiedOn = DateTime.Now;
                _db.ProductCategory.Add(productCategory);
                _db.SaveChanges();
            }
            return View(_db.ProductCategory.Where(e=>e.UserRef==_UserID).OrderByDescending(e=>e.order).ToList());
        }

        // GET: ProductCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = _db.ProductCategory.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // GET: ProductCategories/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: ProductCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Id,UserRef,ImageUrl,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] ProductCategory productCategory)
        {
            return PartialView();
        }

        // GET: ProductCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = _db.ProductCategory.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: ProductCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                ProductCategory prcat = _db.ProductCategory.Find(productCategory.Id);
                prcat.ModifiedBy = User.Identity.Name;
                prcat.ModifiedOn = DateTime.Now;
                prcat.Title = productCategory.Title;
                prcat.order = productCategory.order;
                prcat.Description = productCategory.Description;
                _db.Entry(prcat).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productCategory);
        }

        // GET: ProductCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = _db.ProductCategory.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductCategory productCategory = _db.ProductCategory.Find(id);
            _db.ProductCategory.Remove(productCategory);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

       
    }
}
