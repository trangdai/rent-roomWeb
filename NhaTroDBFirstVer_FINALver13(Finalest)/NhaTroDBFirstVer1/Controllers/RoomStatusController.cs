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
    public class RoomStatusController : Controller
    {
        private NhaTroEntities db = new NhaTroEntities();

        //
        // GET: /RoomStatus/

        public ActionResult Index()
        {
            return View(db.RoomStatus.ToList());
        }

        //
        // GET: /RoomStatus/Details/5

        public ActionResult Details(int id = 0)
        {
            RoomStatus roomstatus = db.RoomStatus.Find(id);
            if (roomstatus == null)
            {
                return HttpNotFound();
            }
            return View(roomstatus);
        }

        //
        // GET: /RoomStatus/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /RoomStatus/Create

        [HttpPost]
        public ActionResult Create(RoomStatus roomstatus)
        {
            if (ModelState.IsValid)
            {
                db.RoomStatus.Add(roomstatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roomstatus);
        }

        //
        // GET: /RoomStatus/Edit/5

        public ActionResult Edit(int id = 0)
        {
            RoomStatus roomstatus = db.RoomStatus.Find(id);
            if (roomstatus == null)
            {
                return HttpNotFound();
            }
            return View(roomstatus);
        }

        //
        // POST: /RoomStatus/Edit/5

        [HttpPost]
        public ActionResult Edit(RoomStatus roomstatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomstatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roomstatus);
        }

        //
        // GET: /RoomStatus/Delete/5

        public ActionResult Delete(int id = 0)
        {
            RoomStatus roomstatus = db.RoomStatus.Find(id);
            if (roomstatus == null)
            {
                return HttpNotFound();
            }
            return View(roomstatus);
        }

        //
        // POST: /RoomStatus/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomStatus roomstatus = db.RoomStatus.Find(id);
            db.RoomStatus.Remove(roomstatus);
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