using Microsoft.AspNet.Identity;
using MyPlace.Domain.NoteBook;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyPlace.Web.Controllers
{
    [Authorize]
    public class NoteBooksController : BaseController
    {
        public string _UserID
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }

        // GET: NoteBooks
        public ActionResult Index(int noteBook = 0,int note=0)
        {
            string userref = _UserID;
            IEnumerable<NoteBook> usernotebooks = _db.NoteBook.Where(e => e.UserRef == userref).ToList();
            @ViewBag.NoteBooks = usernotebooks;
            //this thing provide complete security
            if(_db.NoteBook.Find(noteBook)!=null)
            if (!usernotebooks.Contains(_db.NoteBook.Find(noteBook))) throw new Exception("Wrong Action");
           
                @ViewBag.ActiveNoteBook = noteBook;
                IEnumerable<Note> notes = _db.Note.Where(e => e.WhichNoteBook.Id == noteBook).ToList();
                @ViewBag.Notes = notes;
                @ViewBag.ActiveNote = note;

                Note selectedNote = notes.Where(e => e.Id == note).FirstOrDefault();
               
                    @ViewBag.Heading =selectedNote==null?"": selectedNote.Title;
                    @ViewBag.Content =selectedNote==null?"": selectedNote.Content;


            ////////////// Publish logic///////////////////////////////////////////////////////
                    if (selectedNote != null)
                    {
                        if (_db.PublishedNote.Where(e => e.NotePublished.Id == selectedNote.Id).FirstOrDefault() == null)
                            @ViewBag.Publish = "Make it public";
                        else
                            @ViewBag.Publish = "Make it private";
                    }
                    else
                    {
                        @ViewBag.Publish = string.Empty;
                    }
           ////////////////////////////////// Publish logic///////////////////////////////////////////////////////

                   
                    
               
          
           
            return View();
        }
      
        // GET: NoteBooks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoteBook noteBook = _db.NoteBook.Find(id);
            if (noteBook == null)
            {
                return HttpNotFound();
            }
            return View(noteBook);
        }

        // GET: NoteBooks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NoteBooks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Description,order")] NoteBook noteBook)
        {
            if (ModelState.IsValid)
            {
                noteBook.UserRef = _UserID;
                noteBook.CreatedBy = User.Identity.Name;
                noteBook.ModifiedBy = User.Identity.Name;
                noteBook.CreatedOn = DateTime.Now;
                noteBook.ModifiedOn = DateTime.Now;

                _db.NoteBook.Add(noteBook);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(noteBook);
        }

        // GET: NoteBooks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoteBook noteBook = _db.NoteBook.Find(id);
            if (noteBook == null)
            {
                return HttpNotFound();
            }
            return View(noteBook);
        }

        // POST: NoteBooks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,order")] NoteBook noteBook)
        {
            if (ModelState.IsValid)
            {
                NoteBook ntbk = _db.NoteBook.Find(noteBook.Id);
                ntbk.Title = noteBook.Title;
                ntbk.Description = noteBook.Description;
                ntbk.order = noteBook.order;
                ntbk.ModifiedOn = DateTime.Now;
                ntbk.ModifiedBy = User.Identity.Name;
          
                _db.Entry(ntbk).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(noteBook);
        }

        // GET: NoteBooks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoteBook noteBook = _db.NoteBook.Find(id);
            if (noteBook == null)
            {
                return HttpNotFound();
            }
            return View(noteBook);
        }

        // POST: NoteBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NoteBook noteBook = _db.NoteBook.Find(id);
            _db.NoteBook.Remove(noteBook);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

       
    }
}
