using Newtonsoft.Json;
using NOVA.Models;
using ServiceStack;
using SqlApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace NOVA.Controllers
{
   
    [_SessionController]
    public class SecondPageController : Controller
    {
        [HandleError]
        // GET: SecondPage
        public ActionResult Index()
        {
            if (TempData["name"] == null || TempData["id"] == null) {
                if(Request.Cookies["Id"]==null || Request.Cookies["Id"].Value.Equals("10003") || Request.Cookies["Id"].Value.Equals("10005"))
                {
                    ViewBag.Id ="2";
                    ViewBag.Name = Request.Cookies["Name"].Value.ToString();
                }
                else
                {
                    ViewBag.Name = Request.Cookies["Name"].Value.ToString();
                    ViewBag.Id = Request.Cookies["Id"].Value.ToString();
                }
            }
            else
            {
                HttpCookie cookiei;
                HttpCookie cookien = new HttpCookie("Name", TempData["name"].ToString());
                HttpCookie cookieun= new HttpCookie("Name", TempData["username"].ToString());
                if (TempData["id"].ToString()=="10003" || TempData["id"].ToString() == "10005")
                {
                    cookiei = new HttpCookie("Id", "2");
                }
                else
                {
                    cookiei = new HttpCookie("Id", TempData["id"].ToString());
                }
                
                Response.Cookies.Add(cookien);
                Response.Cookies.Add(cookiei);
                Response.Cookies.Add(cookieun);
                ViewBag.Name = Request.Cookies["Name"].Value.ToString();
                ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            }
            
            ViewBag.Users = TempData["mydata"];
            ViewBag.Module_id= TempData["moduleid"];
            
            return View();
        }
        


    }

    
}
    
