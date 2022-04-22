
using NOVA.Models;
using ServiceStack;
using SqlApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace NOVA.Controllers
{
    public class LoginController : Controller
    {
        bool s = true;

        [AllowAnonymous]
        public ActionResult Login()
        {
            
            if (String.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                
                
               
                if (Request.Cookies["UserPassword"] == null || Request.Cookies["UserName"]==null)
                {

                }
                else
                {
                    ViewBag.password = Request.Cookies["UserPassword"].Value;
                    ViewBag.username = Request.Cookies["UserName"].Value;
                }

                
                
                FormsAuthentication.SignOut();
                
                return View();
            }
            return Redirect("/SecondPage/Index");

        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = "http://192.168.2.209:83/api/user/" + model.USER_NAME;
                Uri url = new Uri(apiUrl);
                WebClient client = new WebClient();
                client.Encoding = System.Text.Encoding.UTF8;
                try
                {
                    string json = client.DownloadString(url);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    var user = ser.Deserialize<User>(json);

                    //Aşağıdaki if komutu gönderilen mail ve şifre doğrultusunda kullanıcı kontrolu yapar. Eğer kullanıcı var ise login olur.
                    if (model.USER_NAME == user.USER_NAME && model.USER_PASSWORD == user.USER_PASSWORD)
                    {
                        
                        if (model.RememberMe == true)
                        {
                            HttpCookie cookieu = new HttpCookie("UserName", model.USER_NAME);
                            HttpCookie cookiep = new HttpCookie("UserPassword", model.USER_PASSWORD);

                            Response.Cookies.Add(cookieu);
                            Response.Cookies.Add(cookiep);

                        }
                        else
                        {
                            HttpCookie cookieu = new HttpCookie("UserName",null);
                            HttpCookie cookiep = new HttpCookie("UserPassword", null);
                            Response.Cookies.Add(cookieu);
                            Response.Cookies.Add(cookiep);
                        }
                        FormsAuthentication.SetAuthCookie(model.USER_NAME, true);
                        TempData["id"] = user.USER_ID;
                        TempData["name"] = user.USER_FIRSTNAME+" "+user.USER_LASTNAME;
                        TempData["username"] = user.USER_NAME;

                        var l=LogKayıt(user);
                        
                        return RedirectToAction("Index", "SecondPage");
                       
                    }

                    else
                    {
                        s = false;
                        ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
                    }
                }
                catch (Exception)
                {
                    s = false;
                }

                if (s == false)
                {
                    ViewBag.Mesaj = "Hata";
                }
                else
                {
                    ViewBag.Mesaj = null;
                }


            }
            return View(model);
        }
        public async Task<ActionResult> LogKayıt(User user)
        {


            Log log = new Log();
            log.USER_NAME = user.USER_NAME;
            log.USER_ID = user.USER_ID.ToInt();
            log.LOG_DATE = DateTime.Now.ToString();
            log.MODULE_ID = 0;
            log.PROGRAM_ID = 0;
            log.ACTIVITY_TYPE = "Log in";
            var apiUrl = "http://192.168.2.209:82/api/userlog";


            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(log), Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);



            return null;

        }
        public ActionResult LogOff()
        {
            HttpCookie nameCookie = Request.Cookies["Name"];
            HttpCookie idCookie = Request.Cookies["Id"];

            //Set the Expiry date to past date.
            nameCookie.Expires = DateTime.Now.AddDays(-1);
            idCookie.Expires = DateTime.Now.AddDays(-1);
            //Update the Cookie in Browser.
            Response.Cookies.Add(nameCookie);
            Response.Cookies.Add(idCookie);
            s = true;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }
        public ActionResult Error()
        {
            
            return View("Error");
        }

    }

}