using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NhaTroDBFirstVer1.Models;
using System.Security.Cryptography;
using System.Text;
using PagedList;
namespace NhaTroDBFirstVer1.Controllers
{
    public class HomeController : Controller
    {
        private NhaTroEntities db = new NhaTroEntities();
        /// <summary>
        /// Acction su ly phuong thuc get ("khi click vao link Home")
        /// </summary>
        /// <returns>Nap trang Home cho client</returns>
        [HttpGet] //phuong thuc get
        public ActionResult Index()
        {
            var top5User = (from r in db.Room
                            join user in db.User
                            on r.UserID equals user.UserID
                            where user.Roles == "User"
                            orderby r.RoomID descending
                            select r.RoomID).Take(5);
            var top5VIP = (from btp in db.Room
                           join user in db.User
                           on btp.UserID equals user.UserID
                           where user.Roles == "VIP"
                           orderby btp.RoomID descending
                           select btp.RoomID).Take(5);
            List<Room> rooms = new List<Room>();
            List<int> tempuser = top5User.ToList();
            List<int> tempvip = top5VIP.ToList();
            for (int i = 0; i < tempvip.Count; i++)
            {
                int k = tempvip[i];
                rooms.Add(db.Room.Single(x => x.RoomID.Equals(k)));
            }
            for (int i = 0; i < tempuser.Count; i++)
            {
                int k = tempuser[i];
                rooms.Add(db.Room.Single(x => x.RoomID.Equals(k)));
            }
            var top3 = (from b in db.Room
                        orderby b.RoomID descending
                        select b).Take(3);
            List<Room> ls = top3.ToList();
            ViewBag.Img = ls;
            ViewBag.Tin = rooms;
            List<QLHoTro> ql = db.QLHoTroes.ToList();
            ViewBag.YH = "ymsgr:sendIM?" + ql[0].Currents;
            List<Advertise> ad = db.Advertise.ToList();
            ViewBag.Ad = ad;
            return View("Index");
        }

        public ActionResult DanhSachTinDang()
        {

            return View();
        }


        [Authorize(Roles = "")] //Chi can dang nhap la dung duoc Action nay
        [HttpGet] //phuong thuc get
        public ActionResult PhanQuyen()
        {
            return View("PhanQuyen");//nap trang doi mat khau cho client
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Nhóm I.";

            return View();
        }

        //MULTILANGUGE
        public ActionResult ChangeCulture(Culture lang, string returnUrl)
        {
            if (returnUrl.Length >= 3)
                returnUrl = returnUrl.Substring(3);
            return Redirect("/" + lang.ToString() + returnUrl);
        }


    }
}
