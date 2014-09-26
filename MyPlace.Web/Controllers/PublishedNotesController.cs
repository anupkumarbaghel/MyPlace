using MyPlace.Domain.NoteBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MyPlace.Web.Controllers
{
    public class PublishedNotesController : BaseController
    {
        public string _UserID
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }
        // GET: PublishedNotes
        public ActionResult Index(int? selectedNote)
        {
            List<PublishedNotes> model = _db.PublishedNote.ToList();
            Note sn = new Note();
            if (selectedNote != null)
            {
                if (model.Where(e => e.NotePublished.Id == selectedNote).FirstOrDefault()!=null)
                sn = model.Where(e => e.NotePublished.Id == selectedNote).First().NotePublished;    
            }

            @ViewBag.SelectedNote = selectedNote;
            @ViewBag.Heading = sn.Title;
            @ViewBag.Content = sn.Content;
            return View(model);
        }
        public ActionResult PublishNote(int id)
        {
            Note note = _db.Note.Find(id);
            if (note.WhichNoteBook.UserRef != _UserID) throw new Exception("You r not allowed to access this.");
            if (_db.PublishedNote.Where(e => e.NotePublished.Id == note.Id).Count() < 1)//this will ensure only one entry is published
            {
                PublishedNotes pbnote = new PublishedNotes();
                pbnote.NotePublished = note;
                pbnote.CreatedBy = User.Identity.Name;
                pbnote.CreatedOn = DateTime.Now;
                pbnote.ModifiedBy = User.Identity.Name;
                pbnote.ModifiedOn = DateTime.Now;
                _db.PublishedNote.Add(pbnote);
                _db.SaveChanges();
            }
            else
            {
                PublishedNotes pbnote = _db.PublishedNote.Where(e => e.NotePublished.Id == note.Id).First();
                int ntbkid = pbnote.NotePublished.WhichNoteBook.Id;
                _db.PublishedNote.Remove(pbnote);
                _db.SaveChanges();
                return RedirectToAction("Index", "NoteBooks", new { noteBook = ntbkid, note = note.Id });
            }
        
           

            return View(note);
        }
    }
}