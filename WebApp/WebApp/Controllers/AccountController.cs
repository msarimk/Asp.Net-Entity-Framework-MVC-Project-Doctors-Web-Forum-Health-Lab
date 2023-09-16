using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        hospitalEntities DB = new hospitalEntities();

        // Dashboard View
        public ActionResult Index()
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                int users_count = DB.tbl_users.Count();
                ViewBag.UserCount = users_count;
                int app_count = DB.tbl_appointment.Count();
                ViewBag.AppCount = app_count;
                int? sum = DB.tbl_users.Sum(m => m.userCount);
                int actualSum = sum ?? 0; // Handle the null value by assigning a default value (e.g., 0)
                ViewBag.ActiveSum = actualSum;
                int dr_count = DB.tbl_doctors.Count();
                ViewBag.DrCount = dr_count;

                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult UpdateProfile(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
                {
                    var users = DB.admins.Find(id);
                    return View(users);
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
        }
        [HttpPost]
        public ActionResult UpdateProfile(FormCollection fc,string email,string password)
        {
            admin admin = new admin();
            if (password == null)
            {
                var data = DB.tbl_users.FirstOrDefault(a => a.Email == email && a.Password == password);
                int id = int.Parse(fc["id"]);
                admin.id = id;
                admin.Email = fc["email"];
                admin.Name = fc["name"];
                admin.Password = fc["dbpass"];
                admin.Phone = fc["phone"];
                admin.Gender = fc["dbgender"];
                admin.User_Image = fc["dbimg"];
                admin.DOB = fc["dbdob"];
                admin.User_Type = fc["dbusertype"];
                admin.education = fc["dbedu"];
                admin.location = fc["dbloc"];
                admin.title = fc["dbtitle"];
                admin.experience = fc["experience"];
                admin.bio = fc["bio"];
                Session["userid"] = id;
                DB.Entry(admin).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Updateprofile/" + id);
            }
            else
            {
                var data = DB.tbl_users.FirstOrDefault(a => a.Email == email && a.Password == password);
                int id = int.Parse(fc["dbid"]);
                admin.DOB = fc["dbdob"];
                admin.id = id;
                admin.Email = fc["email"];
                admin.Name = fc["name"];
                MD5 md5 = new MD5CryptoServiceProvider();
                md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
                byte[] result = md5.Hash;
                StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    strBuilder.Append(result[i].ToString("x2"));
                }
                string Password = strBuilder.ToString();
                admin.Password = Password;
                admin.Gender = fc["dbgender"];
                admin.User_Image = fc["dbimg"];
                admin.User_Type = fc["dbusertype"];
                admin.Phone = fc["phone"];
                admin.education = fc["dbedu"];
                admin.location = fc["dbloc"];
                admin.title = fc["dbtitle"];
                admin.experience = fc["experience"];
                admin.bio = fc["bio"];
                DB.Entry(admin).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                Session["userid"] = id;
                return RedirectToAction("UpdateProfile/" + id);
            }
        }

        public ActionResult Users()
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                var users = DB.tbl_users.ToList();
                return View(users);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult DeleteUser(int? id)
        {
            var deletedata = DB.tbl_users.Find(id);
            if (deletedata == null)
            {
                HttpNotFound();
            }
            else
            {
                System.IO.File.Delete(@"C:\Users\Sam\source\repos\WebApp\WebApp\UserImages\" + deletedata.User_Image);
                DB.tbl_users.Remove(deletedata);
                DB.SaveChanges();
            }
                return RedirectToAction("Users");
        }

        // Slider Work
        public ActionResult Slider()
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                var data = DB.tbl_slider.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        public ActionResult AddSlider()
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                return View(); 
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult AddSlider(FormCollection fc, HttpPostedFileBase sliderimg)
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                tbl_slider TB = new tbl_slider();
                TB.Titles = fc["title"];
                TB.Sub_Titles = fc["sub-title"];
                TB.Button_Titles = fc["btn-title"];
                // Image work 
                string extension = Path.GetExtension(sliderimg.FileName);
                string path = Path.GetFileNameWithoutExtension(sliderimg.FileName);
                sliderimg.SaveAs(@"C:\Users\Sam\source\repos\WebApp\WebApp\SliderImages\" + path + extension);
                TB.Images = path + extension;
                DB.tbl_slider.Add(TB);
                DB.SaveChanges();
                return RedirectToAction("Slider");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult UpdateSlider(int? id)
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                var findData = DB.tbl_slider.Find(id);

                if (findData == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(findData);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult UpdateSlider(FormCollection fc , HttpPostedFileBase sliderimg)
        {
            tbl_slider TB = new tbl_slider();
            if (sliderimg == null) {
                TB.id = int.Parse(fc["id"]);
                TB.Titles = fc["title"];
                TB.Sub_Titles = fc["sub-title"];
                TB.Button_Titles = fc["btn-title"];
                TB.Images = fc["sliderimg1"];
                DB.Entry(TB).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Slider");
            }
            else
            {
                TB.id = int.Parse(fc["id"]);
                TB.Titles = fc["title"];
                TB.Sub_Titles = fc["sub-title"];
                TB.Button_Titles = fc["btn-title"];
                // Image work 
                string extension = Path.GetExtension(sliderimg.FileName);
                string path = Path.GetFileNameWithoutExtension(sliderimg.FileName);
                sliderimg.SaveAs(@"C:\Users\Sam\source\repos\WebApp\WebApp\SliderImages\" + path + extension);
                TB.Images = path + extension;
                DB.Entry(TB).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Slider");
            }
        }

        public ActionResult DeleteSlider(int? id)
        {
            var deletedata = DB.tbl_slider.Find(id);
            if (deletedata == null)
            {
                HttpNotFound();
            }
            else
            {
                System.IO.File.Delete(@"C:\Users\Sam\source\repos\WebApp\WebApp\SliderImages\" + deletedata.Images);
                DB.tbl_slider.Remove(deletedata);
                DB.SaveChanges();     
            }
            return RedirectToAction("Slider");
        }

        // Services Work
        public ActionResult Services()
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                var data = DB.tbl_services.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult AddServices()
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult AddServices(FormCollection fc)
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                tbl_services TB = new tbl_services();
                TB.Titles = fc["title"];
                TB.Descriptions = fc["description"];
                TB.Icons = fc["icon"];
                DB.tbl_services.Add(TB);
                DB.SaveChanges();
                return RedirectToAction("Services");
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        public ActionResult UpdateServices(int? id)
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                var findData = DB.tbl_services.Find(id);
                if (findData == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(findData);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult UpdateServices(FormCollection fc)
        {
            tbl_services TB = new tbl_services();
            TB.id = int.Parse(fc["id"]);
            TB.Titles = fc["title"];
            TB.Descriptions = fc["description"];
            TB.Icons = fc["icon"];
            DB.Entry(TB).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("Services");
        }
        public ActionResult DeleteServices(int? id)
        {
            var deletedata = DB.tbl_services.Find(id);
            if (deletedata == null)
            {
                HttpNotFound();
            }
            else
            {
                DB.tbl_services.Remove(deletedata);
                DB.SaveChanges();
            }
            return RedirectToAction("Services");
        }

        // Appointment Work
        public ActionResult Appointment()
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null) 
            {
                var data = DB.tbl_appointment.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult DeleteAppointment(int? id)
        {
            var deletedata = DB.tbl_appointment.Find(id);
            if (deletedata == null)
            {
                HttpNotFound();
            }
            else
            {
                DB.tbl_appointment.Remove(deletedata);
                DB.SaveChanges();
            }
            return RedirectToAction("Appointment");
        }

        // Gallery Work
        public ActionResult Gallery()
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                var data = DB.tbl_gallery.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult AddGallery()
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult AddGallery(FormCollection fc, HttpPostedFileBase galleryimg)
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                tbl_gallery TB = new tbl_gallery();
                TB.Titles = fc["title"];
                // image work 
                string extension = Path.GetExtension(galleryimg.FileName);
                string path = Path.GetFileNameWithoutExtension(galleryimg.FileName);
                galleryimg.SaveAs(@"C:\Users\Sam\source\repos\WebApp\WebApp\GalleryImages\" + path + extension);
                TB.Images = path + extension;
                DB.tbl_gallery.Add(TB);
                DB.SaveChanges();
                return RedirectToAction("Gallery");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult UpdateGallery(int? id)
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                var findData = DB.tbl_gallery.Find(id);
                if (findData == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(findData);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult UpdateGallery(FormCollection fc, HttpPostedFileBase galleryimg )
        {
            tbl_gallery TB = new tbl_gallery();
            if(galleryimg == null)
            {
                TB.id = int.Parse(fc["id"]);
                TB.Titles = fc["title"];
                TB.Images = fc["galleryimg1"];
                DB.Entry(TB).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Gallery");
            }
            else
            {
                TB.id = int.Parse(fc["id"]);
                TB.Titles = fc["title"];
                // image work 
                string extension = Path.GetExtension(galleryimg.FileName);
                string path = Path.GetFileNameWithoutExtension(galleryimg.FileName);
                galleryimg.SaveAs(@"C:\Users\Sam\source\repos\WebApp\WebApp\GalleryImages\" + path + extension);
                TB.Images = path + extension;
                DB.Entry(TB).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Gallery");
            }
        }
        public ActionResult DeleteGallery(int? id)
        {
            var deletedata = DB.tbl_gallery.Find(id);
            if (deletedata == null)
            {
                HttpNotFound();
            }
            else
            {
                System.IO.File.Delete(@"C:\Users\Sam\source\repos\WebApp\WebApp\SliderImages\" + deletedata.Images);
                DB.tbl_gallery.Remove(deletedata);
                DB.SaveChanges();
            }
                return RedirectToAction("Gallery");
        }

        // Doctors Work
        public ActionResult Doctors()
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                var data = DB.tbl_doctors.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult AddDoctors()
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult AddDoctors(FormCollection fc ,HttpPostedFileBase doctorimg)
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                tbl_doctors TB = new tbl_doctors();
                TB.Name = fc["name"];
                TB.Title = fc["title"];
                TB.Degree = fc["degree"];
                TB.Timing = fc["timing"];
                // image work 
                string extension = Path.GetExtension(doctorimg.FileName);
                string path = Path.GetFileNameWithoutExtension(doctorimg.FileName);
                doctorimg.SaveAs(@"C:\Users\Sam\source\repos\WebApp\WebApp\DoctorImages\" + path + extension);
                TB.Profile = path + extension;
                DB.tbl_doctors.Add(TB);
                DB.SaveChanges();
                return RedirectToAction("Doctors");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult UpdateDoctors(int? id)
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                var findData = DB.tbl_doctors.Find(id);
                if (findData == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(findData);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult UpdateDoctors(FormCollection fc , HttpPostedFileBase doctorimg )
        {
            tbl_doctors TB = new tbl_doctors();
            if (doctorimg == null)
            {
                TB.id = int.Parse(fc["id"]);
                TB.Name = fc["name"];
                TB.Title = fc["title"];
                TB.Degree = fc["degree"];
                TB.Timing = fc["timing"];
                TB.Profile = fc["doctorimg1"];
                DB.Entry(TB).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Doctors");
            }
            else
            {
                TB.id = int.Parse(fc["id"]);
                TB.Name = fc["name"];
                TB.Title = fc["title"];
                TB.Degree = fc["degree"];
                TB.Timing = fc["timing"];
                // image work 
                string extension = Path.GetExtension(doctorimg.FileName);
                string path = Path.GetFileNameWithoutExtension(doctorimg.FileName);
                doctorimg.SaveAs(@"C:\Users\Sam\source\repos\WebApp\WebApp\DoctorImages\" + path + extension);
                TB.Profile = path + extension;
                DB.Entry(TB).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Doctors");
            }
        }
        public ActionResult DeleteDoctors(int? id)
        {
            var deletedata = DB.tbl_doctors.Find(id);
            if (deletedata == null)
            {
                HttpNotFound();
            }
            else
            {
                System.IO.File.Delete(@"C:\Users\Sam\source\repos\WebApp\WebApp\SliderImages\" + deletedata.Profile);
                DB.tbl_doctors.Remove(deletedata);
                DB.SaveChanges();
            }
            return RedirectToAction("Doctors");
        }

        // Contact Work
        public ActionResult Contact()
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                var data = DB.tbl_contact.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult DeleteContact(int? id)
        {
            var deletedata = DB.tbl_contact.Find(id);
            if (deletedata == null)
            {
                HttpNotFound();
            }
            else
            {
                DB.tbl_contact.Remove(deletedata);
                DB.SaveChanges();
            }
            return RedirectToAction("Contact");
        }

        // Address Work
        public ActionResult Address()
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                var data = DB.tbl_address.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult AddAddress()
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult AddAddress(FormCollection fc)
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                tbl_address TB = new tbl_address();
                TB.Icons = fc["icon"];
                TB.Titles = fc["title"];
                TB.Address = fc["address"];
                DB.tbl_address.Add(TB);
                DB.SaveChanges();
                return RedirectToAction("Address");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult UpdateAddress(int? id)
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                var findData = DB.tbl_address.Find(id);
                if (findData == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(findData);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult UpdateAddress(FormCollection fc)
        {
                tbl_address TB = new tbl_address();
                TB.id = int.Parse(fc["id"]);
                TB.Icons = fc["icon"];
                TB.Titles = fc["title"];
                TB.Address = fc["address"];
                DB.Entry(TB).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Address");

        }
        public ActionResult DeleteAddress(int? id)
        {
            var deletedata = DB.tbl_address.Find(id);
            if (deletedata == null)
            {
                HttpNotFound();
            }
            else
            {
                DB.tbl_address.Remove(deletedata);
                DB.SaveChanges();
            }
            return RedirectToAction("Address");
        }

        // Subscription Work
        public ActionResult Subscription()
        {
            if (Convert.ToString(Session["usertype"]) == "admin" && Session["usertype"] != null)
            {
                var data = DB.tbl_subscriptions.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult DeleteSubscription(int? id)
        {
            var deletedata = DB.tbl_subscriptions.Find(id);
            if (deletedata == null)
            {
                HttpNotFound();
            }
            else
            {
                DB.tbl_subscriptions.Remove(deletedata);
                DB.SaveChanges();
            }
            return RedirectToAction("Subscription");
        }
       
        public ActionResult Login()
        {
            if (Session["usertype"] == null)
            {
                return View();
            }
            else
            {
                if (Convert.ToString(Session["usertype"]) == "user")
                {
                    return RedirectToAction("Index", "Home/Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }    
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }
            string Password = strBuilder.ToString();
            var user = DB.tbl_users.FirstOrDefault(a => a.Email == email && a.Password == Password);
            var status = DB.admins.FirstOrDefault(a => a.Email == email && a.Password == Password);
            if (status != null && status.User_Type == "admin")
                {
                    Session["userid"] = status.id;
                    Session["usertype"] = status.User_Type;
                    Session["username"] = status.Name;
                    Session["userimg"] = status.User_Image;
                    return RedirectToAction("Index");
                }
                else if (user != null && user.User_Type == "user")
                {
                    ViewBag.user = "Users Login Not Allowed";
                    return View();
                }
                else
                {
                    ViewBag.error = "Invalid Email Or Password";
                    return View();
                }
        }
        public ActionResult LogOut()
        {
                Session.Abandon();
                Session.Clear();
                return RedirectToAction("Login");
        }

    }
}
