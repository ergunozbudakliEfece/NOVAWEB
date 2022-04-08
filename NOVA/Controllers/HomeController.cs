
using NOVA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace NOVA.Controllers
{
    public class HomeController : Controller
    {      [HttpGet]
        public ActionResult Index()
        {

            ViewBag.Data = GetApiData();

            return View();
        }
        [HttpPost]
        public ActionResult Index(string username,string password)
        {
            var apiUrl = "http://192.168.2.209:83/api/user/" + username;
            Uri url = new Uri(apiUrl);
            
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
             List<User> jsonList = ser.Deserialize<List<User>>(json);
            if (jsonList.Count > 0)
            {
                if (jsonList[0].USER_NAME == username && jsonList[0].USER_PASSWORD == password)
                {
                    return RedirectToAction("Index", "SecondPage");
                }
                else
                {
                    ViewBag.Mesaj = "Kullanıcı adı veya şifre hatalı";
                    return View();
                }
            }
            else
            {
                ViewBag.Mesaj = "Kullanıcı adı veya şifre hatalı";
                return View();
            }
            
            

            
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
            return RedirectToAction("Index", "SecondPage");
        }
        

        public List<User> GetApiData()
        {

            var apiUrl = "http://192.168.2.209:83/api/user";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<User> jsonList = ser.Deserialize<List<User>>(json);
            //END

            return jsonList;
        }
    }
}