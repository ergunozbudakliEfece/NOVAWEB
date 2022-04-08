using Newtonsoft.Json;
using NOVA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace NOVA.Controllers
{
    public class SecondPageController : Controller
    {
        // GET: SecondPage
        public ActionResult Index()
        {
            ViewBag.Users = GetApiData();
            
                

            return View();
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
    
