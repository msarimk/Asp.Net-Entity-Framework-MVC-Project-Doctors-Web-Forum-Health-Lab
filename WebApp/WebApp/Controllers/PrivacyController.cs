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

namespace WebApp.Controllers
{
    public class PrivacyController : Controller
    {
       hospitalEntities DB = new hospitalEntities();
        // GET: Privacy
        public ActionResult Public(int? id)
        {
            if(id == null) {
                return HttpNotFound();
            }
            else
            {
                var status = DB.tbl_users.FirstOrDefault(a => a.id == id);
                int publicid = 1;
                /*            bool  = (intValue == 1);*/
                status.isPublicProfile = publicid;
                DB.Entry(status).Property(a => a.isPublicProfile).IsModified = true;
                DB.SaveChanges();
                return RedirectToAction("", "Home/UserProfile/" + id);
            }
        }

        public ActionResult Private(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            else
            {
                var status = DB.tbl_users.FirstOrDefault(a => a.id == id);
                int publicid = 0;
                /*            bool publicid = (intValue == 1);*/
                status.isPublicProfile = publicid;
                DB.Entry(status).Property(a => a.isPublicProfile).IsModified = true;
                DB.SaveChanges();
                return RedirectToAction("", "Home/UserProfile/" + id);
            }
        }
    }
}