using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NhaTroDBFirstVer1.Models;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Web.WebPages.OAuth;

namespace NhaTroDBFirstVer1.Controllers
{
    public class UserController : Controller
    {
        private NhaTroEntities db = new NhaTroEntities();

        [Authorize(Roles = "Admin")] 
        public ActionResult Index()
        {
            return View(db.User.ToList());
        }

        [Authorize(Roles = "Admin,User,VIP")] 
        [HttpGet]
        public ActionResult Details()
        {
            User account = db.User.Single(x => x.UserName.Equals(User.Identity.Name)); 
            if (account != null)
            {
                ViewBag.Profile = account; 
            }

            return View();
        }

        public ActionResult UpdateRoles(int id = 0)
        {
            User us = db.User.Find(id);
            us.Roles = "VIP";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        
        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            return View(user);
        }

        //
        // GET: /User/Delete/5
        [Authorize(Roles = "Admin")]//Chi co Admin Moi Dung Duoc Controller nay
        public ActionResult Delete(int id = 0)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Delete/5
        [Authorize(Roles = "Admin")]//Chi co Admin Moi Dung Duoc Controller nay
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Acction su ly phuong thuc get ("thuong la cac link cac ban click vao hoac cac form truyen du lieu kieu get")
        /// </summary>
        /// <returns>sau khi su ly thi return mot trang web(nap lai web cho client)</returns>
        [HttpGet]//phuong thuc get
        public ActionResult Login()
        {
            List<Advertise> ad = db.Advertise.ToList();
            ViewBag.Ad = ad;
            return View("Login"); //nap giao dien trang logOn cho client

        }

       
        [HttpPost]//phuong thuc post 
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)//kiem tra validate cua du lieu("chi tiet thi vao Models Class:LogOnModel ma xem")
            {
                ////------------------Kiem tra tai khoan co o trong database khong?-------------------------
                ////o day minh dung phuong thuc Entities de ket noi toi database.cac ban co the dung cach khac de ket noi toi database
                NhaTroEntities db = new NhaTroEntities(); //khoi tao lop ket noi toi database
                string temp = toMD5(model.Password);
                List<User> users = db.User.Where(x => x.UserName.Equals(model.UserName) && x.Password.Equals(temp)).ToList(); //no tuong tu nhu cau lenh "Select * From Accounts Where NameAccount = @model.UserName and Password = @model.Password
                ////----------------------------------------------------------------------------------------
                if (users.Count > 0)//kiem tra xem co tim thay tai khoan do khong?
                {
                    //// Neu tim thay
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe); //goi ham nay no se goi phuong thuc GetRolesForUser(string username) o file CustomRoleProvider.cs
                    //// model.UserName o day la chuyen tai khoan nay vao de lay ra quyen cua tai khan do,model.RememberMe la co luu vao coockie hay khong? cai o vuong ma ban danh dau co luu tai khoan ay!(cai trang LogOn)
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))//doan code nay nham muc dich khi ma ban vao mot trang nao do mo khong dung quyen han no se bat ban dang nhap.neu thanh cong no se tro lai trang truoc do ma ban muon vao.
                    {
                        return Redirect(returnUrl); //tro lai trang truoc do ma ban muon vao
                    }
                    else
                    {
                        if (users[0].Roles == "Admin")
                            return RedirectToAction("Index", "Advertise");
                        else
                            return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Sai tai khoan hoac mat khau."); //neu khong tim thay tai khoan thi hien thi loi sai tai khoan
                }
            }

            // neu cac ban nhap cu phap account hoac pass no se nap lai trang LogOn va hien thi loi~
            return View("Login", model);//o day them vao model la de nap lai cac gia tri cac ban nham vao.neu khong co model thi khi nap lai trang cac du lieu o cac o account hoac pass se mat(value ="")
        }

        /// <summary>
        /// Acction su ly phuong thuc get(su ly khi ban clik vao link LogOff)
        /// </summary>
        /// <returns>Sau khi su ly xong thi nap lai mot trang web cho client</returns>
        [HttpGet]//phuong thuc get
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();//xoa du lieu quyen va tai khoan
            return RedirectToAction("Index", "Home");//nap lai trang home cho client
        }


        [HttpPost]
        public JsonResult doesUserNameExist(string UserName)
        {
            List<User> luser = db.User.ToList();
            for (int i = 0; i < luser.Count; i++)
            {
                if (UserName == luser[i].UserName)
                    return Json(false);
            }
            return Json(true);
        }

        /// <summary>
        /// Acction su ly phuong thuc get(su ly khi ban clik vao link Dang Ky)
        /// </summary>
        /// <returns>nap trang dang ky cho client</returns>
        [HttpGet]//phuong thuc get
        public ActionResult Register()
        {
            List<Advertise> ad = db.Advertise.ToList();
            ViewBag.Ad = ad;
            return View("Register");//nap giao dien trang dang ky cho client
        }

        /// <summary>
        /// Acction su ly phuong thuc get(su ly khi ban clik vao nut dang ky o form Dang Ky(trong trang dang ky))
        /// </summary>
        /// <param name="model">du lieu ma client gui len</param>
        /// <returns>khi su ly xong se nap giao dien cua mot trang web</returns>
         [CaptchaMvc.Attributes.CaptchaVerify("Captcha is not valid")]
        [HttpPost]//phuong thuc post
        public ActionResult Register(RegisterModel model)
        {
             
            if (ModelState.IsValid)//kiem tra cu phap cua du lieu.("Xem chi tiet o Models class:RegisterModel")
            {
               
                {
                    //// Neu thoa man du lieu thi kiem tra xem tai khoan nay da co trong db chua?
                    ////--------------------------thao tac voi db---------------------------
                    NhaTroEntities db = new NhaTroEntities(); //tao ket noi voi database
                   
                    List<User> users = db.User.Where(x => x.UserName.Equals(model.UserName)).ToList();//tuong dong voi cau lenh "Select * From Accounts Where Accounts = @model.UserName"
                    ////--------------------------------------------------------------------
                    if (users.Count == 0) //kiem tra xem tai khoan do da ton tai chua
                    {
                        ////neu chua ton tai thi insert vao trong database
                        ////--------------------------thao tac voi db---------------------------
                        User user = new User();//khoi tao mot doi tuong Account ("Mot dong` du lieu trong database")
                        user.UserName = model.UserName; //gan tai khoan 

                        string temp = toMD5(model.Password);//ma hoa md5 password

                        user.Password = temp.ToString(); //gan mat khau
                        //user.Password = "124567eqraja91k21k1od1219201jfcd";
                        user.PhoneNumber = model.SoDienThoai;
                        user.FullName = model.FullName;
                        user.Email = model.Email;
                        user.Roles = "User"; //gan quyen
                        //string exquery = "Insert into User (UserName,Password,PhoneNumber,FullName,Email,Roles,Address) values (@user.UserName,(select convert(nvarchar(32),HashBytes('MD5',@user.Password)),@user.PhoneNumber,@user.FullName,@user.Email,@user.Roles,null";
                        db.User.Add(user);// tuong duong voi dong lenh: "INSERT INTO Accounts([NameAccount],[Password],[Roles])VALUES(@account.NameAccount,@account.Password,@account.Roles)"
                        db.SaveChanges(); // tuong duong voi executequery
                        ////--------------------------------------------------------------------
                        FormsAuthentication.SetAuthCookie(model.UserName, false);//khi dang ky xong thi dang nhap luon nhung khong luu vao cookie
                        return RedirectToAction("DangKyThanhCong"); //nap trang Home cho client
                    }
                    else
                    {
                        ModelState.AddModelError("", "tai khoan nay da ton tai");//add them thong bao
                    }
                }
                
            }
            //// neu nhap sai cu phap thi se nap lai trang dang ky va hien thi loi~
            return View("Register", model);//nap lai trang Dang Ky cho client
        }
        public static string toMD5(string pass)
        {
            MD5CryptoServiceProvider myMD5 = new MD5CryptoServiceProvider();
            byte[] myPass = System.Text.Encoding.UTF8.GetBytes(pass);
            myPass = myMD5.ComputeHash(myPass);

            StringBuilder s = new StringBuilder();
            foreach (byte p in myPass)
            {
                s.Append(p.ToString("x").ToLower());
            }
            return s.ToString();
        }        

        [Authorize(Roles = "Admin,User,VIP")] //Chi co Admin va ThanhVien Moi Dung duoc acction nay
        [HttpGet] //phuong thuc get
        public ActionResult DoiMatKhau()
        {
            return View("DoiMatKhau"); //Nap giao dien trang DoiMatKhau cho client
        }

        [Authorize(Roles = "Admin,User,VIP")] //Chi co Admin va ThanhVien Moi Dung duoc acction nay
        [HttpPost] //phuong thuc get
        public ActionResult DoiMatKhau(ChangePasswordModel model)
        {
            if (ModelState.IsValid)//kiem tra xem mat khau doi da hop le chua
            {
                ////neu mat khau moi hop le thi ket noi voi database de kiem tra mat khau cu da dung chu
                NhaTroEntities db = new NhaTroEntities();//ket noi voi database
                string temp = toMD5(model.OldPassword);
                List<User> users =
                    db.User.Where(
                        x => x.UserName.Equals(User.Identity.Name) && x.Password.Equals(temp)).ToList();//tuong duong voi cau lenh "Select * from Accounts where NameAccount = @User.Identity.Name && Password = model.OldPassword"
                if (users.Count > 0)//neu tim thay tai khoan voi dung mat khau cau
                {
                    User account =
                        db.User.Single(
                            x => x.UserName.Equals(User.Identity.Name) && x.Password.Equals(temp));//tuong duong voi cau lenh "Select Top 1 from Accounts where NameAccount = @User.Identity.Name && Password = model.OldPassword"
                    //trong Entity muon edit du lieu trong database thi can phai tim duoc du lieu can thay doi truoc roi edit thong tin sau do save
                    account.Password = toMD5(model.NewPassword);//thay doi mat khau moi thay cho mat khau cu
                    db.SaveChanges();// luu lai du lieu thay doi .tuong duong voi cau lenh "Update Accounts Set Password = @model.NewPassword Where NameAccount= @User.Identity.Name and Password = @model.OldPassword"
                    return RedirectToAction("DoiMatKhauThanhCong");//doi mat khau thanh cong xong thi chuyen sang acction: DoiMatKhauThanhCong()
                }
                else
                {
                    ModelState.AddModelError("", "mat khau cu sai");//add them thong bao
                }
            }

            //neu du lieu doi mat khau khong dung cu phap thi nap lai trang doi mat khau va hien thi thong bao loi
            return View("DoiMatKhau", model);//nap lai trang thong tin cho client va chuyen model
        }

        /// <summary>
        /// Acction su ly phuong thuc get ("khi mot acction nao redirect sang")
        /// </summary>
        /// <returns>nap trang hien thi doi mat khau thanh cong cho client</returns>
        [Authorize(Roles = "Admin,User,VIP")] //Chi co Admin va ThanhVien Moi Dung duoc acction nay
        [HttpGet] //phuong thuc get
        public ActionResult DoiMatKhauThanhCong()
        {
            return View("DoiMatKhauThanhCong");//nap trang doi mat khau cho client
        }

        public ActionResult DangKyThanhCong()
        {
            return View("DangKyThanhCong");//nap trang doi mat khau cho client
        }
        //public ActionResult XemtinDaDang()
        //{
        //    String userName = User.Identity.Name;
        //    var d = (from u in db.User
        //             select u).ToList();
        //    List<User> us = d;
        //    int usid =0 ;
        //    for (int i = 0; i < us.Count; i++)
        //    {
        //        if (us[i].UserName == userName)
        //            usid = us[i].UserID;
        //    }
        //    var l = (from r in db.Room
        //             join s in db.User on r.UserID equals s.UserID
        //             where s.UserID == usid
        //             select r).ToList();
        //    List<Room> rm = l;
        //    return View(l);
        //}

        //Paypal
        public ActionResult NangCap()
        {
            return View("NangCap");

        }
        public ActionResult NangCapUser()
        {
            int idDH = 0;
            List<User> us = db.User.ToList();
            for (int i = 0; i < us.Count; i++)
            {
                if (us[i].UserName == User.Identity.Name)
                    idDH = us[i].UserID;
            }
            PayPal pp = new PayPal(idDH)
            {
                shipping_1 = "50.5",
                tax_cart = "0.1"
            };

            return View(pp);

        }

        public void ExternalLogin(string provider)
        {
            // just pass a provider to make it work
            OAuthWebSecurity.RequestAuthentication(provider, Url.Action("ExternalLoginCallback"));
        }

        public ActionResult ExternalLoginCallback()
        {
            var result = OAuthWebSecurity.VerifyAuthentication();

            if (result.IsSuccessful == false)
            {
                return RedirectToAction("Error", "User");
            }

            // this is just demo. Make all things you need before setting cookies
            FormsAuthentication.SetAuthCookie(result.UserName, false);

            // We need to get the url user has entered to the browser to use it in the redirect. 
            // This is just demo.
            return Redirect(Url.Action("Index", "Home"));
        }

    }
}
