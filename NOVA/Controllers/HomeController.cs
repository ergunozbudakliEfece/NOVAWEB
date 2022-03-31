using NetOpenX50;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using NetOpenX.Rest.Client;
using NetOpenX.Rest.Client.Model;
using NetOpenX.Rest.Client.BLL;
using NetOpenX.Rest.Client.Model.NetOpenX;
using NetOpenX.Rest.Client.Model.Enums;

namespace NOVA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            


            return View();
        }
        
        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Test()
        {
            return View("SecondPage");
        }
    }
}