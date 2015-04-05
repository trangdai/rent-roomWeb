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
    [Authorize(Roles = "Admin")]
    public class AdvertiseController : Controller
    {
        private NhaTroEntities db = new NhaTroEntities();

        //
        // GET: /Advertise/

        public ActionResult Index()
        {
            var advertise = db.Advertise.Include(a => a.User);
            return View(advertise.ToList());
        }

        //
        // GET: /Advertise/Details/5

        public ActionResult Details(int id = 0)
        {
            Advertise advertise = db.Advertise.Find(id);
            if (advertise == null)
            {
                return HttpNotFound();
            }
            return View(advertise);
        }

        //
        // GET: /Advertise/Create

        public ActionResult Create()
        {
            ViewBag.AdminID = new SelectList(db.User, "UserID", "UserName");
            return View();
        }

        //
        // POST: /Advertise/Create

        [HttpPost]
        public ActionResult Create(Advertise advertise)
        {

            int id = 0;
            var d = (from c in db.User
                     select c).ToList();
            List<User> us = d;
            for (int i = 0; i < us.Count; i++)
            {
                if (us[i].UserName == User.Identity.Name)
                    id = us[i].UserID;

            }
            advertise.AdminID = id;
            if (ModelState.IsValid)
            {
                db.Advertise.Add(advertise);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.AdminID = new SelectList(db.User, "UserID", "UserName", advertise.AdminID);
            return View(advertise);
        }

        //
        // GET: /Advertise/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Advertise advertise = db.Advertise.Find(id);
            if (advertise == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminID = new SelectList(db.User, "UserID", "UserName", advertise.AdminID);
            return View(advertise);
        }

        //
        // POST: /Advertise/Edit/5

        [HttpPost]
        public ActionResult Edit(Advertise advertise)
        {
            if (ModelState.IsValid)
            {
                db.Entry(advertise).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdminID = new SelectList(db.User, "UserID", "UserName", advertise.AdminID);
            return View(advertise);
        }

        //
        // GET: /Advertise/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Advertise advertise = db.Advertise.Find(id);
            if (advertise == null)
            {
                return HttpNotFound();
            }
            return View(advertise);
        }

        //
        // POST: /Advertise/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Advertise advertise = db.Advertise.Find(id);
            db.Advertise.Remove(advertise);
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