
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
using System.Management;
using System.Net.Mail;

namespace NOVA.Controllers
{
    
    public class LoginController : Controller
    {


        [AllowAnonymous]
        public ActionResult Login()
        {

            if (String.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {



                if (Request.Cookies["UserPassword"] == null || Request.Cookies["UserName"] == null)
                {

                }
                else
                {
                    ViewBag.password = Request.Cookies["UserPassword"].Value;
                    ViewBag.username = Request.Cookies["UserName"].Value;
                }



                FormsAuthentication.SignOut();
                ViewBag.Info = Session["Info"];
                return View();
            }

            return Redirect("~/Home/Index");

        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(Login model)
        {
            string username = "";
            string password = "";

            if (ModelState.IsValid)
            {
                if (model.USER_PASSWORD == null)
                {
                    password = "0";
                }
                else
                {
                    password = model.USER_PASSWORD;
                }
                if (model.USER_NAME == null)
                {
                    username = "0";
                }
                else
                {
                    username = model.USER_NAME;
                }

                string json = null;
                List<User> user = null;
                var apiUrl = "http://192.168.2.13:83/api/user/exec/" + username + "/" + password;
                Uri url = new Uri(apiUrl);
                WebClient client = new WebClient();
                client.Encoding = System.Text.Encoding.UTF8;
                try
                {

                    json = client.DownloadString(url);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    user = ser.Deserialize<List<User>>(json);
                }
                catch (Exception)
                {

                }

                //Aşağıdaki if komutu gönderilen mail ve şifre doğrultusunda kullanıcı kontrolu yapar. Eğer kullanıcı var ise login olur.
                if (user.Count >0)
                {
                    if (user[0].ACTIVE == true)
                    {

                        if (model.RememberMe == "on")
                        {
                            HttpCookie cookieu = new HttpCookie("UserName", model.USER_NAME);
                            Session["UserName"] = model.USER_NAME;
                            HttpCookie cookiep = new HttpCookie("UserPassword", model.USER_PASSWORD);
                            Response.Cookies.Add(cookieu);
                            Response.Cookies.Add(cookiep);

                        }
                        else
                        {
                            HttpCookie cookieu = new HttpCookie("UserName", model.USER_NAME);
                            HttpCookie cookiep = new HttpCookie("UserPassword", null);
                            Session["UserName"] = model.USER_NAME;
                            Response.Cookies.Add(cookieu);
                            Response.Cookies.Add(cookiep);
                        }
                        FormsAuthentication.SetAuthCookie("Log", true);

                        ViewBag.Name = user[0].USER_FIRSTNAME + " " + user[0].USER_LASTNAME;


                        HttpCookie cookiename = new HttpCookie("Name", ViewBag.Name);
                        Response.Cookies.Add(cookiename);
                        Session["Name"] = ViewBag.Name;
                        HttpCookie cookierole = new HttpCookie("Role", user[0].USER_ROLE);
                        Response.Cookies.Add(cookierole);
                        HttpCookie cookieid = new HttpCookie("Id", user[0].USER_ID);
                        Response.Cookies.Add(cookieid);
                        HttpCookie cookiemail = new HttpCookie("Mail", user[0].USER_MAIL);
                        Response.Cookies.Add(cookiemail);
                        HttpCookie cookiefn = new HttpCookie("FirstName", user[0].USER_FIRSTNAME);
                        Response.Cookies.Add(cookiefn);
                        HttpCookie cookieln = new HttpCookie("LastName", user[0].USER_LASTNAME);
                        Response.Cookies.Add(cookieln);
                        //var l=LogKayıt(user);
                        Session["UserId"] = user[0].USER_ID;
                        Session["UserIdInt"] = user[0].USER_ID.ToInt();
                        HttpCookie cookieuserid = new HttpCookie("UserId", Session["UserId"].ToString());
                        Response.Cookies.Add(cookieuserid);


                        HttpCookie cookielogin = new HttpCookie("Login", DateTime.Now.ToString());
                        Response.Cookies.Add(cookielogin);
                        HttpCookie cookiesign = new HttpCookie("SignIn", DateTime.Now.AddHours(3).ToString());
                        Response.Cookies.Add(cookiesign);

                        String s = model.USER_NAME.Substring(0, 1);
                        var userAgent = Request.UserAgent;
                        string platform = "";
                        if (userAgent.Contains("Edge"))
                        {
                            //Edge
                            platform = "Edge";
                        }
                        else if (userAgent.Contains("Chrome"))
                        {
                            if (userAgent.Contains("OPR"))
                            {
                                platform = "Opera";
                            }
                            else if (userAgent.Contains("Edg"))
                            {
                                platform = "Microsoft Edge";
                            }
                            else
                            {
                                platform = "Google Chrome";
                            }
                            //Chrome

                        }
                        else if (userAgent.Contains("Safari"))
                        {
                            //Safari
                            platform = "Safari";
                        }
                        else if (userAgent.Contains("Firefox"))
                        {
                            //Firefox
                            platform = "Firefox";
                        }
                        else if (userAgent.Contains("rv"))
                        {
                            //IE11
                            platform = "IE11";
                        }
                        else if (userAgent.Contains("MSIE"))
                        {
                            //IE6-10
                            platform = "IE6-10";
                        }
                        else if (userAgent.Contains("Other"))
                        {
                            //Other
                            platform = "Opera";
                        }
                        HttpCookie cookieplatform = new HttpCookie("Platform", platform);
                        Response.Cookies.Add(cookieplatform);
                        if (Request.Cookies["Id"].Value.ToInt() != 10050)
                        {
                            
                            
                            var session = GetSession(user[0].USER_ID.ToInt())[0];
                            if (session.ACTIVITY_TYPE == "login")
                            {
                                ViewBag.Login = "Oturumunuz başka bir bilgisayarda açık. Oturum tüm diğer cihazlarda kapatılsın mı?";
                                HttpCookie cookieoldplatform = new HttpCookie("OldPlatform", session.PLATFORM);
                                Response.Cookies.Add(cookieoldplatform);
                                return View();

                            }
                            



                        }
                        else
                        {
                            HttpCookie cookieuname = new HttpCookie("Ghost", "ghost");
                            Response.Cookies.Add(cookieuname);
                        }

                        HttpCookie cookiech = new HttpCookie("checked", "");
                        Response.Cookies.Add(cookiech);
                        


                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Mesaj = "Yetki";

                    }


                }

                else
                {
                    ViewBag.Mesaj = "Hata";
                    HttpCookie cookieu = new HttpCookie("UserName", model.USER_NAME);
                    Response.Cookies.Add(cookieu);
                }





            }
            ViewBag.username = Request.Cookies["UserName"].Value;
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult LogOffAllDevice()
        {
            SignInTBL lognew = new SignInTBL();
            lognew.USER_NAME = Request.Cookies["UserName"].Value;
            lognew.USER_ID = Request.Cookies["Id"].Value.ToInt();
            lognew.LOG_DATETIME = DateTime.Parse(Request.Cookies["SignIn"].Value);
            
            lognew.ACTIVITY_TYPE = "logout";
            if (Request.Cookies["OldPlatform"] != null)
            {
                lognew.PLATFORM = Request.Cookies["OldPlatform"].Value;
            }
            else {
                lognew.PLATFORM = "Google Chrome";
            }
            

            var apiUrlnew = "http://192.168.2.13:83/api/Log";

            var httpClientnew = new HttpClient();
            var requestnew = new HttpRequestMessage(HttpMethod.Post, apiUrlnew)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(lognew), Encoding.UTF8, "application/json")
            };

            var responsenew = httpClientnew.SendAsync(requestnew);
            HttpCookie cookiech = new HttpCookie("checked", "");
            Response.Cookies.Add(cookiech);
            return RedirectToAction("Login");
        }
        public List<SignIn> GetSession(int id)
        {


            var apiUrl = "http://192.168.2.13:83/api/log/max:" + id;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SignIn> jsonList = ser.Deserialize<List<SignIn>>(json);

            //END

            return jsonList;
        }
        public List<Log> GetUserLog(int id)
        {


            var apiUrl = "http://192.168.2.13:83/api/userlog/" + id;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Log> jsonList = ser.Deserialize<List<Log>>(json);

            //END

            return jsonList;
        }
        public async Task<ActionResult> LogKayıt(User user)
        {
            HttpCookie cookielogin = new HttpCookie("Login", DateTime.Now.ToString());
            Response.Cookies.Add(cookielogin);
            Log log = new Log();
            log.USER_NAME = user.USER_NAME;
            log.USER_ID = user.USER_ID.ToInt();
            log.ACTIVITY_START = DateTime.Now.ToString();
            log.ACTIVITY_END = null;
            log.MODULE_ID = 0;
            log.PROGRAM_ID = 0;
            log.ACTIVITY_TYPE = "Main Page";
            var apiUrl = "http://192.168.2.13:83/api/userlog";


            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(log), Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);



            return null;

        }

        public List<SignIn> GetInckey(int id)
        {


            var apiUrl = "http://192.168.2.13:83/api/log/max:" + id;
            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SignIn> jsonList = ser.Deserialize<List<SignIn>>(json);
            //END
            if (jsonList.Count == 0)
            {
                jsonList = null;
            }
            return jsonList;
        }
        [AllowAnonymous]
        public ActionResult LogOut(LogClass log)
        {
            SignInTBL lognew = new SignInTBL();
            lognew.USER_NAME = log.USERNAME;
            lognew.USER_ID =log.USERID.ToInt();
            lognew.LOG_DATETIME =DateTime.Parse(log.LASTACTIVETIME).AddHours(3);
            lognew.ACTIVITY_TYPE = "logout";
            lognew.PLATFORM = log.PLATFORM;
            
           




            var apiUrlnew = "http://192.168.2.13:83/api/Log";






            var httpClientnew = new HttpClient();
            var requestnew = new HttpRequestMessage(HttpMethod.Post, apiUrlnew)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(lognew), Encoding.UTF8, "application/json")
            };

            var responsenew =  httpClientnew.SendAsync(requestnew);
            return null;
        }
        public class LogClass
        {
            public string USERID { get; set; }
            public string LASTACTIVETIME { get; set; }
            public string USERNAME { get; set; }
            public string PLATFORM { get; set; }
        }
        public async Task<ActionResult> LogOutAsync()
        {
            if (Request.Cookies["UserId"].Value.ToInt() != 10050 || Request.Cookies["Ghost"].Value != "ghost")
            {
                SignInTBL lognew = new SignInTBL();
                lognew.USER_NAME = Request.Cookies["UserName"].Value;
                lognew.USER_ID = Request.Cookies["Id"].Value.ToInt();
                lognew.LOG_DATETIME =DateTime.Now.AddHours(3);
                lognew.ACTIVITY_TYPE = "logout";
                if (Request.Cookies["Platform"] != null)
                {
                    lognew.PLATFORM = Request.Cookies["Platform"].Value;
                }
                else
                {
                    lognew.PLATFORM = "Google Chrome";
                }




                var apiUrlnew = "http://192.168.2.13:83/api/Log";






                var httpClientnew = new HttpClient();
                var requestnew = new HttpRequestMessage(HttpMethod.Post, apiUrlnew)
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(lognew), Encoding.UTF8, "application/json")
                };

                var responsenew = await httpClientnew.SendAsync(requestnew);

            }




            return null;

        }
        public User GetUserByMail(string mail)
        {
            User jsonList;

            var apiUrl = "http://192.168.2.13:83/api/user/mail:" + mail;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            try
            {
                string json = client.DownloadString(url);
                JavaScriptSerializer ser = new JavaScriptSerializer();
                jsonList = ser.Deserialize<User>(json);
            }
            catch (Exception)
            {

                jsonList = null;
            }

            //END

            //JSON Parse START


            //END

            return jsonList;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(Login pw)
        {
            User user = new User();
            if (pw.PasswordChange.CODE == (Guid)Session["Guid"])
            {
                if (pw.PasswordChange.NEW_PASSWORD.ToString() == pw.PasswordChange.NEW_PASSWORD_REPEAT.ToString())
                {
                    user.USER_ID = GetUserByMail(Session["Mail"].ToString()).USER_ID;
                    user.USER_NAME = GetUserByMail(Session["Mail"].ToString()).USER_NAME;
                    user.USER_PASSWORD = pw.PasswordChange.NEW_PASSWORD;
                    user.USER_FIRSTNAME = GetUserByMail(Session["Mail"].ToString()).USER_FIRSTNAME;
                    user.USER_LASTNAME = GetUserByMail(Session["Mail"].ToString()).USER_LASTNAME;
                    user.USER_MAIL = GetUserByMail(Session["Mail"].ToString()).USER_MAIL;
                    user.USER_ROLE = GetUserByMail(Session["Mail"].ToString()).USER_ROLE;
                    user.ACTIVE = true;
                }
                else
                {
                    Session["AlertLogin"] = "Girmiş Olduğunuz Şifreler Aynı Değil!";
                    Session["Info"] = null;
                    return RedirectToAction("Login");
                }
            }
            else
            {
                Session["AlertLogin"] = "Girdiğiniz doğrulama kodu yanlış!";
                Session["Info"] = null;
                return RedirectToAction("Login");
            }



            var apiUrl = "http://192.168.2.13:83/api/user/" + GetUserByMail(Session["Mail"].ToString()).USER_ID.ToInt();


            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(user), Encoding.UTF8, "application/json")

            };

            var response = await httpClient.SendAsync(request);

            string apiResponse = await response.Content.ReadAsStringAsync();

            Session["AlertLogin"] = "Şifre Değişikliği Başarılı";
            Session["Info"] = null;
            return RedirectToAction("Login");

        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult MailControl(Login login)
        {

            if (GetUserByMail(login.User.USER_MAIL) != null)
            {
                Session["Mail"] = login.User.USER_MAIL;
                Session["Guid"] = Guid.NewGuid();
                ViewBag.Info = "Başarılı";


                string subject = "NOVA | Doğrulama Kodu";

                string body = "Aşağıdaki doğrulama kodunu ilgili alana girerek şifrenizi değiştirebilirsiniz.</br></br><strong>Doğrulama kodu: " + Session["Guid"] + "</strong></br></br>Şifrenizi kişisel olmayan bir şifre ile değiştirmenizi ve <strong><u>hiç kimseyle paylaşmamanızı öneririz</u></strong>.";



                WebMail.SmtpServer = "192.168.2.13";
                WebMail.Send(login.User.USER_MAIL, subject, body, "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, "High", null);

            }
            else
            {
                Session["AlertLogin"] = "Bu mail adresi sistemimizde kayıtlı değil!";
            }




            return View("Login");

        }

        public ActionResult LogOff()
        {
            _ = LogOutAsync();
            Session["LoginControl"] = null;
            HttpCookie nameCookie = Request.Cookies["Name"];
            HttpCookie idCookie = Request.Cookies["Id"];
            HttpCookie idCookierolename = Request.Cookies["RoleName"];
            HttpCookie roleid = Request.Cookies["role"];


         

            //Set the Expiry date to past date.
            nameCookie.Expires = DateTime.Now.AddDays(-1);

            idCookie.Expires = DateTime.Now.AddDays(-1);
            idCookierolename.Expires = DateTime.Now.AddDays(-1);
            roleid.Expires = DateTime.Now.AddDays(-1);

            //Update the Cookie in Browser.
            Response.Cookies.Add(nameCookie);
            Response.Cookies.Add(idCookie);
            Response.Cookies.Add(idCookierolename);
            Response.Cookies.Add(roleid);
            Session["Alert"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }
        public ActionResult Error()
        {

            return View("Error");
        }

    }

}