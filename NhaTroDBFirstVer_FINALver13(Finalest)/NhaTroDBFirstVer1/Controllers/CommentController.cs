using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NhaTroDBFirstVer1.Models;

namespace NhaTroDBFirstVer1.Controllers
{
    public class CommentController : Controller
    {
        private NhaTroEntities db = new NhaTroEntities();

        //
        // GET: /Comment/

        public ActionResult Index()
        {
            var comment = db.Comment.Include(c => c.User);
            return View(comment.ToList());
        }

        //
        // GET: /Comment/Details/5

        public ActionResult Details(int id = 0)
        {
            Comment comment = db.Comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        //
        // GET: /Comment/Create

        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserName");
            return View();
        }

        //
        // POST: /Comment/Create

        [HttpPost]
        public ActionResult Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comment.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.User, "UserID", "UserName", comment.UserID);
            return View(comment);
        }

        //
        // GET: /Comment/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Comment comment = db.Comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserName", comment.UserID);
            return View(comment);
        }

        //
        // POST: /Comment/Edit/5

        [HttpPost]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserName", comment.UserID);
            return View(comment);
        }

        //
        // GET: /Comment/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Comment comment = db.Comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        //
        // POST: /Comment/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comment.Find(id);
            db.Comment.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}