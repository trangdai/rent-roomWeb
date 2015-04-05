using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NhaTroDBFirstVer1.Models;

namespace NhaTroDBFirstVer1.Controllers
{


    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        NhaTroEntities db = new NhaTroEntities();
        [HttpGet]//phuong thuc get
        public ActionResult Index()
        {
            NhaTroEntities db = new NhaTroEntities();
            List<User> accounts = db.User.ToList();
            ViewBag.data = accounts;
            return View("Index");
        }
        public ActionResult TroGiup()
        {
            return View(db.QLHoTroes.ToList());
        }
        public ActionResult SetSupport(int id = 0)
        {
            QLHoTro ql = db.QLHoTroes.Find(id);
            List<QLHoTro> ql1 = db.QLHoTroes.ToList();
            ql1[0].Currents = ql.QLHoTro1;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult AddSupport(FormCollection form)
        {
            QLHoTro ql = new QLHoTro();
            ql.Currents = "123";
            ql.QLHoTro1 = form.Get(0);
            db.QLHoTroes.Add(ql);
            db.SaveChanges();
            return RedirectToAction("TroGiup");

        }
    }
}
