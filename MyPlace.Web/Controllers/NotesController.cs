using Microsoft.AspNet.Identity;
using MyPlace.Domain.NoteBook;
using MyPlace.Web.Helpers;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyPlace.Web.Models;

namespace MyPlace.Web.Controllers
{
    [Authorize]
    public class NotesController : BaseController
    {

        public string _UserID
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }
        // GET: Notes
        public ActionResult Index()
        {
            return View(_db.Note.ToList());
        }

        // GET: Notes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = _db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Notes/Create
        public ActionResult Create(int id)
        {
            Session["notebookid"] = id;
            @ViewBag.createnotebook = _db.NoteBook.Find(id).Title;
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Content")] MyPlace.Web.Models.NoteVM notevm)
        {
            if (ModelState.IsValid)
            {
                int ntbkid = (int)Session["notebookid"];
                NoteBook ntbk = _db.NoteBook.Find(ntbkid);
              
                Note note = new Note();
                note.WhichNoteBook = ntbk;
                note.Title = notevm.Title;
                note.Content = notevm.Content;
                note.CreatedBy = User.Identity.Name;
                note.ModifiedBy = User.Identity.Name;
                note.CreatedOn = DateTime.Now;
                note.ModifiedOn = DateTime.Now;
               
                _db.Note.Add(note);
                _db.SaveChanges();



                return RedirectToAction("Index", "NoteBooks", new {noteBook=ntbkid,note=note.Id });
            }

            return View(notevm);
        }

        // GET: Notes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = _db.Note.Find(id);
            if (note.WhichNoteBook.UserRef != _UserID) throw new Exception("You r not allowed to access this.");
            @ViewBag.notebook = note.WhichNoteBook.Title;
            NoteVM notevm = new NoteVM();
            notevm.Id = note.Id;
            notevm.Title = note.Title;
            notevm.Content = note.Content;
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(notevm);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content")] NoteVM notevm)
        {
            if (ModelState.IsValid)
            {

                Note note = _db.Note.Find(notevm.Id);
                if (note.WhichNoteBook.UserRef != _UserID) throw new Exception("You r not allowed to access this.");
                var ntbkid = note.WhichNoteBook.Id;
                note.Title = notevm.Title;
                note.Content = notevm.Content;
                note.ModifiedBy = User.Identity.Name;
                note.ModifiedOn = DateTime.Now;
                _db.Entry(note).State = EntityState.Modified;
                _db.SaveChanges();
             
                return RedirectToAction("Index", "NoteBooks", new { noteBook = ntbkid, note = note.Id });
            }
            return View(notevm);
        }

        // GET: Notes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = _db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = _db.Note.Find(id);
            _db.Note.Remove(note);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UploadImage(HttpPostedFileBase frontImage)
        {
            //string path = frontImage.ProcessMe("~/Upload/" + User.Identity.GetUserId(),700,Guid.NewGuid().ToString());
            string path = frontImage.SaveIt("~/Upload/" + User.Identity.GetUserId(), Guid.NewGuid().ToString());
            Session["frontImagePathNotes"] = path;
            return Json(path);
        }
       
    }
}
