using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AspAngularSpa.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "evgeni", DateTime.Now, DateTime.Now.AddHours(1), false, "evgeni-data");
            string token = FormsAuthentication.Encrypt(ticket);
            FormsAuthenticationTicket formsTicket = FormsAuthentication.Decrypt(token);
            if (formsTicket.Expired) 
            {
                
            }
            return View();
        }

    }
}