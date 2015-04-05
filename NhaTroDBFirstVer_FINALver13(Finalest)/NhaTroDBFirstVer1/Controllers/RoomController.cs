using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NhaTroDBFirstVer1.Models;
using PagedList;
using Google.GData.Photos;
using System.Globalization;

namespace NhaTroDBFirstVer1.Controllers
{
    public class RoomController : Controller
    {
        private NhaTroEntities db = new NhaTroEntities();

        //
        // GET: /Room/

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

            ViewBag.Tin = rooms;
            return View("Index");
        }

        //
        // GET: /Room/Details/5

        public ActionResult Demobanner(int id = 0)
        {
            List<Advertise> adsList = db.Advertise.ToList();
            ViewBag.Ads = adsList;
            return View();
        }

         public ActionResult Comment(FormCollection collection)
        {
            if (User.Identity.Name == "")
            {
                return RedirectToAction("Login", "User");
            }
            int roomID = int.Parse(collection.Get(0));
            String textCmt = collection.Get(1);
            String Username = User.Identity.Name;
            DateTime time = DateTime.Now;
            Comment nx = new Comment();
            //IList<Comment> lcmt= db.Comment.Where(m => m.RoomID == roomID).ToList();
            nx.RoomID =  roomID;
            nx.UserID = (from us in db.User where us.UserName == Username select us.UserID).Single();
            nx.Time = time;
            nx.Contents = textCmt;
            db.Comment.Add(nx);
            db.SaveChanges();
            List<Comment> tempNX = db.Comment.Where(m => m.RoomID == roomID).ToList();
            Room room = db.Room.Where
                (m => m.RoomID == roomID).First();
           
            ViewBag.Comment = tempNX;
            //  return View("XemChiTietSanPham", ct);
            /// db.Configuration.ProxyCreationEnabled = false;
            // return Json(tempNX, JsonRequestBehavior.AllowGet);

            return PartialView("Comment", room);
        }

        public ActionResult Details(int id = 0)
        {
            Room room = db.Room.Find(id);
            List<Comment> tempNX = db.Comment.Where(m => m.RoomID == id).ToList();
            if (room == null)
            {
                return HttpNotFound();
            }

            ViewBag.Comment = tempNX;
            List<Advertise> ad = db.Advertise.ToList();
            ViewBag.Ad = ad;
            return View(room);
        }

        //
        // GET: /Room/Create

        public ActionResult Create()
        {
            ViewBag.RoomStatus = new SelectList(db.RoomStatus, "StatusID", "Contents");
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserName");
            return View();
        }

        //
        // POST: /Room/Create

        [HttpPost]
        public ActionResult Create(Room room, FormCollection form, IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    room.StartDate = DateTime.Now;

                    //upload img
                    PicasaService picasaService = new PicasaService("Picasa");
                    // picasaService.setUserCredentials("***@gmail.com", "***");
                    picasaService.setUserCredentials("timphongtronhanh@gmail.com", "timphongtronhanh2010");
                    String AuthToken = picasaService.QueryClientLoginToken();

                    if (!String.IsNullOrEmpty(AuthToken))
                    {
                        foreach (var file in files)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                /*var fileName = Path.GetFileName(file.FileName);
                                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                                file.SaveAs(path);*/

                                String albumId = "default";
                                Uri postUri = new Uri(PicasaQuery.CreatePicasaUri("timphongtronhanh@gmail.com", albumId));
                                System.IO.Stream fileStream = file.InputStream;
                                PicasaEntry entry = (PicasaEntry)picasaService.Insert(postUri, fileStream, "image/jpeg", file.FileName);
                                room.Image = entry.Media.Content.Url;

                                fileStream.Close();
                                // ViewBag.RoomStatus = new SelectList(db.RoomStatus, "StatusID", "Content");

                            }
                        }

                    }


                    //information
                    room.Lat = Double.Parse(form.Get(9).ToString(), CultureInfo.InvariantCulture);
                    room.Long = Double.Parse(form.Get(10).ToString(), CultureInfo.InvariantCulture);
                    var userID = (from u in db.User select u).ToList();
                    int uID = 0;
                    List<User> us = userID;
                    for (int i = 0; i < us.Count; i++)
                    {
                        if (us[i].UserName == User.Identity.Name)
                            uID = us[i].UserID;
                    }
                    room.UserID = uID;
                    db.Room.Add(room);
                    db.SaveChanges();

                }
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            ViewBag.RoomStatus = new SelectList(db.RoomStatus, "StatusID", "Contents", room.RoomStatus);
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserName", room.UserID);
            //return View("Create",room);
            string temp = "Details/" + room.RoomID;
            return RedirectToAction(temp);
        }

        public ActionResult DanhSachTinDang()
        {
            var userID = (from u in db.User select u).ToList();
            int uID = 0;
            List<User> us = userID;
            for (int i = 0; i < us.Count; i++)
            {
                if (us[i].UserName == User.Identity.Name)
                    uID = us[i].UserID;
            }

            var rooms = (from r in db.Room
                         where r.UserID == uID
                         select r);
            return View(rooms.ToList());
        }
        public ActionResult SaveTin(int id = 0)
        {

            var userID = (from u in db.User select u).ToList();
            int uID = 0;
            List<User> us = userID;
            for (int i = 0; i < us.Count; i++)
            {
                if (us[i].UserName == User.Identity.Name)
                    uID = us[i].UserID;
            }
            Room r = db.Room.Find(id);
            QLTDD ql = new QLTDD();
            ql.UserID = uID;
            ql.RoomID = r.RoomID;
            db.QLTDDs.Add(ql);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        public ActionResult DanhSachTinLuu()
        {
            var userID = (from u in db.User select u).ToList();
            int uID = 0;
            List<User> us = userID;
            for (int i = 0; i < us.Count; i++)
            {
                if (us[i].UserName == User.Identity.Name)
                    uID = us[i].UserID;
            }


            var rm = (from q in db.QLTDDs
                      where q.UserID == uID
                      select q.RoomID);
            List<Int32> ql = rm.ToList();
            List<Room> rms = new List<Room>();
            for (int i = 0; i < ql.Count; i++)
            {
                int k = ql[i];
                rms.Add(db.Room.Single(x => x.RoomID.Equals(k)));

            }
            return View(rms.ToList());
        }
        public ActionResult DeleteFromQLDD(int id = 0)
        {
            var userID = (from u in db.User select u).ToList();
            int uID = 0;
            List<User> us = userID;
            for (int i = 0; i < us.Count; i++)
            {
                if (us[i].UserName == User.Identity.Name)
                    uID = us[i].UserID;
            }



            List<QLTDD> ql = db.QLTDDs.ToList();
            for (int i = 0; i < ql.Count; i++)
            {
                if (ql[i].RoomID == id && ql[i].UserID == uID)
                    db.QLTDDs.Remove(ql[i]);
            }
            db.SaveChanges();

            return RedirectToAction("DanhSachTinLuu");
        }
        //
        // GET: /Room/Edit/5

        public ActionResult Edit(int id = 0)
        {

            Room room = db.Room.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomStatus = new SelectList(db.RoomStatus, "StatusID", "Contents", room.RoomStatus1);
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserName", room.UserID);
            return View(room);
        }

        //
        // POST: /Room/Edit/5

        [HttpPost]
        public ActionResult Edit(Room room, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                room.Lat = Double.Parse(form.Get(10).ToString(), CultureInfo.InvariantCulture);
                room.Long = Double.Parse(form.Get(11).ToString(), CultureInfo.InvariantCulture);
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomStatus = new SelectList(db.RoomStatus, "StatusID", "Contents", room.RoomStatus1);
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserName", room.UserID);
            return View(room);
        }

        //
        // GET: /Room/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Room room = db.Room.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        //
        // POST: /Room/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Room.Find(id);
            db.Room.Remove(room);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        public ActionResult SearchIndex(string Quan, string giaMin, string giaMax, string SMin, string SMax, int? page)
        {
            var ListResult = new List<Room>();
            int Count;
            var GenreQry = (from d in db.Room
                            select d).OrderBy(d => d.RoomID);
            List<Room> rm = GenreQry.ToList();
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            if (Quan != null)
            {
                for (int i = 0; i < rm.Count; i++)
                {
                    if (rm[i].Address.Contains(Quan))
                    {
                        ListResult.Add(rm[i]);
                    }
                }
            }
            Count = ListResult.Count;
            if (giaMin != "" && Count > 0)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (ListResult[i].Price < float.Parse(giaMin))
                    {
                        ListResult.Remove(ListResult[i]);
                        Count--;
                        i--;
                    }
                }
            }
            Count = ListResult.Count;
            if (giaMax != "" && Count > 0)
            {

                for (int i = 0; i < Count; i++)
                {
                    if (ListResult[i].Price > float.Parse(giaMax))
                    {
                        ListResult.Remove(ListResult[i]);
                        Count--;
                        i--;
                    }
                }

            }
            Count = ListResult.Count;
            if (SMax != "" && Count > 0)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (ListResult[i].Area > float.Parse(SMax))
                    {
                        ListResult.Remove(ListResult[i]);
                        Count--;
                        i--;
                    }
                }
            }
            Count = ListResult.Count;
            if (SMin != "" && Count > 0)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (ListResult[i].Area < float.Parse(SMin))
                    {
                        ListResult.Remove(ListResult[i]);
                        Count--;
                        i--;
                    }
                }
            }
            Count = ListResult.Count;
            var dropList = new List<string>
            {
                "Quận 1" , "Quận 2","Quận 5","Quận 7","Quận 9", "Quận Thủ Đức"
            };
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            ViewBag.Quan = new SelectList(dropList);
            List<Advertise> ad = db.Advertise.ToList();
            ViewBag.Ad = ad;
            if (string.IsNullOrEmpty(Quan) && string.IsNullOrEmpty(giaMin) && string.IsNullOrEmpty(giaMax) && string.IsNullOrEmpty(SMax) && string.IsNullOrEmpty(SMin))
            {
                return View(GenreQry.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View(ListResult.ToPagedList(pageNumber, pageSize));
            }
        }

    }
}