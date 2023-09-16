using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.Helpers;
using System.Runtime.Remoting.Contexts;
namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        hospitalEntities DB = new hospitalEntities();
        // GET: Home
        public ActionResult Index()
        {
            if (Convert.ToString(Session["usertype"]) == "admin")
            {
                return RedirectToAction("Index","Account/Index");
            }
            else
            {
                var Data = DB.tbl_slider.ToList();
                ViewBag.Title = "Health Lab";
                tbl_users TB = new tbl_users();
                int? sum = DB.tbl_users.Sum(m => m.userCount);
                int actualSum = sum ?? 0; // Handle the null value by assigning a default value (e.g., 0)
                int count = DB.tbl_users.Count();

                ViewBag.Count = count;
                ViewBag.Sum = actualSum;

                return View(Data);
            }
        }
        [HttpPost]
        public ActionResult FindData(string searchdata)
        {
            string query = "SELECT * FROM tbl_users WHERE Name = '" + searchdata + "' OR Name LIKE '" + searchdata + "%'";
            var data = DB.tbl_users.SqlQuery(query).ToList();
            return View(data);
        }
        public ActionResult UserProfile(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            else
            {
                var users = DB.tbl_users.Find(id);
                return View(users);
            }
        }
        [HttpPost]
        public ActionResult UserProfile(FormCollection fc,string password, string email)
        {
            tbl_users TB = new tbl_users();
            if (password == null)
            {
                var data = DB.tbl_users.FirstOrDefault(a => a.Email == email && a.Password == password);
                int id = int.Parse(fc["id"]);
                    TB.id = id;
                    TB.userCount = int.Parse(fc["dbcount"]);
                    TB.Email = fc["email"];
                    TB.Name = fc["name"];
                    TB.Password = fc["dbpass"];
                    TB.Phone = fc["phone"];
                    TB.Gender = fc["dbgender"];
                    TB.User_Image = fc["dbimg"];
                    TB.DOB = fc["dbdob"];
                    TB.User_Type = fc["dbusertype"];
                    TB.education = fc["dbedu"];
                    TB.location = fc["dbloc"];
                    TB.title = fc["dbtitle"];
                    TB.experience = fc["experience"];
                    TB.bio = fc["bio"];
                    TB.isPublicProfile = int.Parse(fc["dbstatus"]);
                    Session["userid"] = id;
                    DB.Entry(TB).State = System.Data.Entity.EntityState.Modified;
                    DB.SaveChanges();
                    return RedirectToAction("UserProfile/" + id);
            }
            else
            {
                var data = DB.tbl_users.FirstOrDefault(a => a.Email == email && a.Password == password);
                int id = int.Parse(fc["dbid"]);
                    TB.DOB = fc["dbdob"];
                    TB.id = id;
                    TB.userCount = int.Parse(fc["dbcount"]);
                    TB.Email = fc["email"];
                    TB.Name = fc["name"];
                    MD5 md5 = new MD5CryptoServiceProvider();
                    md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
                    byte[] result = md5.Hash;
                    StringBuilder strBuilder = new StringBuilder();
                    for (int i = 0; i < result.Length; i++)
                    {
                        strBuilder.Append(result[i].ToString("x2"));
                    }
                    string Password = strBuilder.ToString();
                    TB.Password = Password;
                    TB.Gender = fc["dbgender"];
                    TB.User_Image = fc["dbimg"];
                    TB.User_Type = fc["dbusertype"];
                    TB.Phone = fc["phone"];
                    TB.education = fc["dbedu"];
                    TB.location = fc["dbloc"];
                    TB.title = fc["dbtitle"];
                    TB.experience = fc["experience"];
                    TB.bio = fc["bio"];
                    TB.isPublicProfile = int.Parse(fc["dbstatus"]);
                    DB.Entry(TB).State = System.Data.Entity.EntityState.Modified;
                    DB.SaveChanges();
                    Session["userid"] = id;
                    return RedirectToAction("UserProfile/" + id);
            }
        }
        public ActionResult Services()
        {
            if (Convert.ToString(Session["usertype"]) == "admin")
            {
                return RedirectToAction("Index", "Account/Index");
            }
            else
            {
                var Data = DB.tbl_services.ToList();
                return View(Data);
            }
        }
        public ActionResult Appointment()
        {
            if (Convert.ToString(Session["usertype"]) == "admin")
            {
                return RedirectToAction("Index", "Account/Index");
            }
            else
            {
                return View();
            }  
        }
        [HttpPost]
        public ActionResult Appointment(FormCollection fc)
        {
                tbl_appointment TB = new tbl_appointment();
                TB.Name = fc["apname"];
                TB.Email = fc["apemail"];
                TB.Date = fc["apdate"];
                TB.Time = fc["aptime"];
                TB.Department = fc["appointmentfor"];
                DB.tbl_appointment.Add(TB);
                DB.SaveChanges();
                return RedirectToAction("Appointment"); 
        }
        public ActionResult Gallery()
        {
            if (Convert.ToString(Session["usertype"]) == "admin")
            {
                return RedirectToAction("Index", "Account/Index");
            }
            else
            {
                var Data = DB.tbl_gallery.ToList();
                return View(Data);
            }
        }
        public ActionResult Doctor()
        {
            if (Convert.ToString(Session["usertype"]) == "admin")
            {
                return RedirectToAction("Index", "Account/Index");
            }
            else
            {
                var Data = DB.tbl_doctors.ToList();
                return View(Data);
            }
        }
        public ActionResult Contact()
        {
            if (Convert.ToString(Session["usertype"]) == "admin")
            {
                return RedirectToAction("Index", "Account/Index");
            }
            else
            {
                var Data = DB.tbl_address.ToList();
                return View(Data);
            }
        }
        [HttpPost]
        public ActionResult Contact(FormCollection fc)
        {
                tbl_contact TB = new tbl_contact();
                TB.Name = fc["cname"];
                TB.Email = fc["cemail"];
                TB.Phone = fc["cnumber"];
                TB.Message = fc["cmessage"];
                DB.tbl_contact.Add(TB);
                DB.SaveChanges();
                return RedirectToAction("Contact");
        }
        [HttpPost]
        public ActionResult Subscription(FormCollection fc)
        {
            tbl_subscriptions TB = new tbl_subscriptions();
            TB.Email = fc["subemail"];
            DB.tbl_subscriptions.Add(TB);
            DB.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Forum()
        {
            if (Session["usertype"] != null)
            {
                ViewBag.Queries = DB.tbl_queries.ToList();
                ViewBag.Replies = DB.tbl_replies.ToList();

                return View();
            }
            else {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult AddQuery(FormCollection formData)
        {
            tbl_queries TB = new tbl_queries();
            TB.user_image = formData["userimg"];
            TB.name = formData["name"];
            TB.queries = formData["query"];
            DB.tbl_queries.Add(TB);
            DB.SaveChanges();
            return RedirectToAction("Forum");
        }
        [HttpPost]
        public ActionResult AddReply(FormCollection fc)
        {
            tbl_replies TB = new tbl_replies();
            TB.user_image = fc["userimg"];
            TB.q_id = int.Parse(fc["q_id"]);
            TB.name = fc["reply_name"];
            TB.replies = fc["replies"];
            DB.tbl_replies.Add(TB);
            DB.SaveChanges();
            return RedirectToAction("Forum");
        }
        public ActionResult Login()
        {
            if (Session["usertype"] == null)
            {
                return View();
            }
            else
            {
                if (Convert.ToString(Session["usertype"]) == "admin")
                {
                    return RedirectToAction("Index", "Account/Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        public ActionResult Login(FormCollection fc,string email, string password)
        {
            tbl_users TB = new tbl_users();
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }
            string Password = strBuilder.ToString();
            var admin = DB.admins.FirstOrDefault(a => a.Email == email && a.Password == Password);
            var status = DB.tbl_users.FirstOrDefault(a => a.Email == email && a.Password == Password);
            if (status != null && status.User_Type == "user")
            {
                int usercount = int.Parse(fc["usercount"]);
                status.userCount = usercount;
                DB.Entry(status).Property(a => a.userCount).IsModified = true;
                DB.SaveChanges();
                Session["userid"] = status.id;
                Session["email"] = status.Email;
                Session["usertype"] = status.User_Type;
                Session["username"] = status.Name;
                Session["userimg"] = status.User_Image;
                return RedirectToAction("Index");
            }
            else if (admin != null && admin.User_Type == "admin")
            {
                ViewBag.admin = "Admin Login Not Allowed";
                return View();
            }
            else
            {
                ViewBag.error = "Invalid Email Or Password";
                return View();
            }
        }
        public ActionResult LogUser(int? id)
        {
            tbl_users TB = new tbl_users();
            var status = DB.tbl_users.FirstOrDefault(a => a.id == id);
            status.userCount = 0;
            DB.Entry(status).Property(a => a.userCount).IsModified = true;
            DB.SaveChanges();
            return View();
        }
        public ActionResult LogOut(int? id)
        {
            tbl_users TB = new tbl_users();
            var status = DB.tbl_users.FirstOrDefault(a => a.id == id);
            status.userCount = 0;
            DB.Entry(status).Property(a => a.userCount).IsModified = true;
            DB.SaveChanges();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index");    
        }
        public ActionResult Register()
        {
            if (Session["usertype"] == null)
            { 
                return View();
            }
            else
            {
                if (Convert.ToString(Session["usertype"]) != "user")
                {
                    return RedirectToAction("Index", "Account/Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult Register(FormCollection fc, HttpPostedFileBase userimg)
        {
            tbl_users TB = new tbl_users();
            if (ModelState.IsValid)
            {
                TB.title = fc["title"];
                string name = fc["firstname"]+" "+fc["lastname"];
                TB.Name = name;
                TB.Email = fc["email"];
                // encrypted password work              
                string Password = fc["password"];
                MD5 md5 = new MD5CryptoServiceProvider();
                md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Password));
                byte[] result = md5.Hash;
                StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    strBuilder.Append(result[i].ToString("x2"));
                }
                TB.Password = strBuilder.ToString();
                TB.DOB = fc["DOB"];
                TB.education = fc["education"];
                TB.bio = fc["bio"];
                TB.experience = fc["experience"];
                TB.location = fc["location"];
                TB.Phone = fc["phone"];
                TB.Gender = fc["gender"];
                TB.User_Type = fc["usertype"];
                // image work 
                string extension = Path.GetExtension(userimg.FileName);
                string path = Path.GetFileNameWithoutExtension(userimg.FileName);
                userimg.SaveAs(@"C:\Users\Sam\source\repos\WebApp\WebApp\UserImages\" + path + extension);
                TB.User_Image = path + extension;
                DB.tbl_users.Add(TB);
                DB.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }
    }
}