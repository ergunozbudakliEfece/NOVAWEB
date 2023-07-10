
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
using DocumentFormat.OpenXml.EMMA;
using Microsoft.Extensions.Logging;
using static ServiceStack.Diagnostics.Events;

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
        public async Task<ActionResult> Login(Login model)
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
                if (user.Count > 0)
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
                       
                            var userAgent = Request.UserAgent;
                            string platform = "";
                            if (userAgent.Contains("Edge"))
                            {
                                if (userAgent.Contains("Mobile"))
                                {
                                    platform = "Edge(Mobile)";
                                }
                                else
                                {
                                    platform = "Edge";
                                }
                                //Edge
                               
                            }
                            else if (userAgent.Contains("Chrome"))
                            {
                                if (userAgent.Contains("OPR"))
                                {
                                    if (userAgent.Contains("Mobile"))
                                    {
                                        platform = "Opera(Mobile)";
                                    }
                                    else
                                    {
                                        platform = "Opera";
                                    }
                                    
                                }
                                else if (userAgent.Contains("Edg"))
                                {
                                    if (userAgent.Contains("Mobile"))
                                    {
                                        platform = "Microsoft Edge(Mobile)";
                                    }
                                    else
                                    {
                                        platform = "Microsoft Edge";
                                    }
                                    
                                }
                                else
                                {
                                    if (userAgent.Contains("Mobile"))
                                    {
                                        platform = "Google Chrome(Mobile)";
                                    }
                                    else
                                    {
                                        platform = "Google Chrome";
                                    }
                                   
                                }
                                //Chrome

                            }
                            else if (userAgent.Contains("Safari"))
                            {
                                if (userAgent.Contains("Mobile"))
                                {
                                    platform = "Safari(Mobile)";
                                }
                                else
                                {
                                    platform = "Safari";
                                }
                                //Safari
                               
                            }
                            else if (userAgent.Contains("Firefox"))
                            {
                                if (userAgent.Contains("Mobile"))
                                {
                                    platform = "Firefox(Mobile)";
                                }
                                else
                                {
                                    platform = "Firefox";
                                }
                                //Firefox
                                
                            }
                            else if (userAgent.Contains("rv"))
                            {
                                if (userAgent.Contains("Mobile"))
                                {
                                    platform = "IE11(Mobile)";
                                }
                                else
                                {
                                    platform = "IE11";
                                }
                                //IE11
                               
                            }
                            else if (userAgent.Contains("MSIE"))
                            {
                                if (userAgent.Contains("Mobile"))
                                {
                                    platform = "IE6-10(Mobile)";
                                }
                                else
                                {
                                    platform = "IE6-10";
                                }
                                //IE6-10
                                
                            }
                            else if (userAgent.Contains("Other"))
                            {
                                if (userAgent.Contains("Mobile"))
                                {
                                    platform = "Opera(Mobile)";
                                }
                                else
                                {
                                    platform = "Opera";
                                }
                                //Other
                               
                            }

                            LoginModel login = new LoginModel();
                                login.USER_ID = user[0].USER_ID.ToInt();
                            login.PLATFORM = platform;
                                var apiUrlnew = "http://192.168.2.13:83/api/UserLogin";

                                var httpClientnew = new System.Net.Http.HttpClient();
                                var requestnew = new HttpRequestMessage(HttpMethod.Post, apiUrlnew)
                                {
                                    Content = new StringContent(new JavaScriptSerializer().Serialize(login), Encoding.UTF8, "application/json")
                                };
                                var response = await httpClientnew.SendAsync(requestnew);

                            string json1 = null;
                            LoginModel createdlog = null;
                            var apiUrl1 = "http://192.168.2.13:83/api/UserLogin/" + user[0].USER_ID.ToInt();
                            Uri url1 = new Uri(apiUrl1);
                            WebClient client1 = new WebClient();
                            client1.Encoding = System.Text.Encoding.UTF8;

                            json1 = client1.DownloadString(url1);
                            JavaScriptSerializer ser1 = new JavaScriptSerializer();
                            createdlog = ser1.Deserialize<LoginModel>(json1);


                            HttpCookie logid1 = new HttpCookie("LogId", createdlog.LOG_ID.ToString());
                            Response.Cookies.Add(logid1);
                        var x = GetRoleName();
                        if (x != null)
                        {
                            HttpCookie cookierolename = new HttpCookie("RoleName", x.ROLE_NAME);
                            Response.Cookies.Add(cookierolename);
                        }
                        if (Session["Name"] != null)
                        {
                            HttpCookie cookiename1 = new HttpCookie("Name", Session["Name"].ToString());
                            Response.Cookies.Add(cookiename1);
                        }



                        if (!model.defurl.IsEmpty()&&model.defurl!= "~/Login/LogOff"&& model.defurl != "~/undefined/undefined")
                        {
                            return Redirect(model.defurl);
                        }

                        return RedirectToAction("Login", "Login");



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
        public class LoginModel
        {
            public int LOG_ID { set; get; }
            public int USER_ID { set; get; }
            public DateTime LOGIN_DATE { set; get; }
            public DateTime LOGOUT_DATE { set; get; }
            public DateTime LAST_ACTIVITY_DATE { set; get; }
            public int LAST_ACTIVITY { set; get; }
            public string PLATFORM { set; get; }
        }

        public Models.Roles GetRoleName()
        {
            if (Request.Cookies["Role"] != null)
            {
                var apiUrl = "http://192.168.2.13:83/api/roles/" + Request.Cookies["Role"].Value.ToInt();
                Uri url = new Uri(apiUrl);
                WebClient client = new WebClient();
                client.Encoding = System.Text.Encoding.UTF8;

                string json1 = client.DownloadString(url);
                //END

                //JSON Parse START
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var json = ser.Deserialize<Models.Roles>(json1);

                //END

                return json;
            }
            else
            {
                return null;
            }



            //Connect API


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

            var httpClientnew = new System.Net.Http.HttpClient();
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


            var httpClient = new System.Net.Http.HttpClient();
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






            var httpClientnew = new System.Net.Http.HttpClient();
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






                var httpClientnew = new System.Net.Http.HttpClient();
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



            var apiUrl = "http://192.168.2.13:83/api/user/" + GetUserByMail(Session["Mail"].ToString()).USER_ID;


            var httpClient = new System.Net.Http.HttpClient();
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

        public async Task<ActionResult> LogOff()
        {
            LoginModel login = new LoginModel();
            login.LOG_ID = Request.Cookies["LogId"].Value.ToInt();
            login.LAST_ACTIVITY = -2;
            var apiUrlnew = "http://192.168.2.13:83/api/UserLogin";

            var httpClientnew = new System.Net.Http.HttpClient();
            var requestnew = new HttpRequestMessage(HttpMethod.Put, apiUrlnew)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(login), Encoding.UTF8, "application/json")
            };

            var responsenew = await httpClientnew.SendAsync(requestnew);
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }
        public ActionResult Error()
        {

            return View("Error");
        }

    }

}