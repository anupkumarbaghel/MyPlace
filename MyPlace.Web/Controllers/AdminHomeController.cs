using MyPlace.Domain;
using MyPlace.Domain.Shoping;
using MyPlace.Web.Infrastructure;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyPlace.Web.Controllers
{
    public class AdminHomeController : Controller
    {
        private MyPlaceDB db = new MyPlaceDB();

        // GET: AdminHome
        public ActionResult Index(string cardNo=null)
        {
            var model = (from row in db.Homes
                         where row.Id.ToString()==cardNo ||cardNo==null||cardNo==""
                        orderby row.CreatedOn descending
                        select row).ToList();
            return View(model);
        }

        // GET: AdminHome/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Home home = db.Homes.Find(id);
            if (home == null)
            {
                return HttpNotFound();
            }
            return View(home);
        }

        // GET: AdminHome/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminHome/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ImageUrl,Description,Type,UserID,NoViewes,State,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] Home home)
        {
            if (ModelState.IsValid)
            {
                db.Homes.Add(home);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(home);
        }

        // GET: AdminHome/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Home home = db.Homes.Find(id);
            if (home == null)
            {
                return HttpNotFound();
            }
            return View(home);
        }

        // POST: AdminHome/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ImageUrl,Description,Type,UserID,NoViewes,State,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] Home home)
        {
            if (ModelState.IsValid)
            {
                db.Entry(home).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(home);
        }

        // GET: AdminHome/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Home home = db.Homes.Find(id);
            if (home == null)
            {
                return HttpNotFound();
            }
            return View(home);
        }

        // POST: AdminHome/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Home home = db.Homes.Find(id);
            db.Homes.Remove(home);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult MasterDelete(string userid)
        {
            Home hmdelete = db.Homes.Where(e => e.UserID == userid).FirstOrDefault();
            HomeDescription hmdescDelete = db.HomeDescriptions.Where(e => e.Home.Id == hmdelete.Id).FirstOrDefault();
            Address addtodelete = db.Address.Where(e => e.HomeDescription.Id == hmdescDelete.Id).FirstOrDefault();
            Feature fetodelete = db.Feature.Where(e => e.UserRef == userid).FirstOrDefault();
            string maindir = System.IO.Path.GetDirectoryName(hmdelete.ImageUrl);
            if (fetodelete != null)
            {
                db.Entry(fetodelete).State = EntityState.Deleted;
                List<ProductCategory> pcattodelete = db.ProductCategory.Where(e => e.UserRef == userid).ToList();
                List<Product> prtodelete = new List<Product>();
                foreach (ProductCategory prcat in pcattodelete)
                {
                    db.Entry(prcat).State = EntityState.Deleted;
                    List<Product> pr = prcat.CategoryPr.ToList();
                    foreach (Product prone in pr)
                    {
                        db.Entry(prone).State = EntityState.Deleted;
                    }
                }
            }
            db.Entry(hmdelete).State = EntityState.Deleted;
            foreach (ImageConCollection concoll in hmdescDelete.HmDescImageConCollection.ToList())
            {
                db.Entry(concoll).State = EntityState.Deleted;
            }
            db.Entry(hmdescDelete).State = EntityState.Deleted;
            db.Entry(addtodelete).State = EntityState.Deleted ;
            db.SaveChanges();
            System.IO.Directory.Delete(Server.MapPath(maindir), true);
            return RedirectToAction("Index");
        }
    }
}
