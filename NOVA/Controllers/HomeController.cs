
using Microsoft.AspNetCore.Http;
using NOVA.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace NOVA.Controllers
{
    [Microsoft.AspNetCore.Mvc.RequestSizeLimit(100000000)]
    public class HomeController : Controller
    {
        [HandleError]
        public ActionResult Maintenance()
        {
            return View();
        }
        // GET: SecondPage
        public ActionResult mutluyillar()
        {
            return View();
                }
            public ActionResult Index()
        {
           
            if (Request.Cookies["Id"] == null){
                FormsAuthentication.SignOut();
                TempData["LOG"] = "ok";
                return RedirectToAction("Login","Login");
            }
           
           
            if (Request.Cookies["Id"].Value.ToInt() != 10050)
            {
                
                    HttpCookie cookiech = new HttpCookie("checked", "checked");
                    Response.Cookies.Add(cookiech);
                    SignInTBL lognew = new SignInTBL();
                    lognew.USER_NAME = Request.Cookies["UserName"].Value;
                    lognew.USER_ID = Request.Cookies["Id"].Value.ToInt();
                    lognew.LOG_DATETIME = DateTime.Parse(Request.Cookies["SignIn"].Value);
                    lognew.ACTIVITY_TYPE = "login";
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
                   
                        var responsenew = httpClientnew.SendAsync(requestnew);
                    
                        
                        if (GetSession(Request.Cookies["Id"].Value.ToInt())[0].LOG_DATETIME != null)
                        {
                            var xm = DateTime.Parse(GetSession(Request.Cookies["Id"].Value.ToInt())[0].LOG_DATETIME);
                            var xn = DateTime.Parse(Request.Cookies["SignIn"].Value).AddHours(-3);
                            var log = GetSession(Request.Cookies["Id"].Value.ToInt())[0].ACTIVITY_TYPE;
                            if (((xm != xn) && log == "login") || log == "logout")
                            {
                           
                                    FormsAuthentication.SignOut();
                                    return RedirectToAction("Login", "Login");
                               
                           
                            
                           

                            }
                        }
                    
                    
                }
                
            


          



            ViewBag.OnlineUsers = null;
            ViewBag.OfflineUsers = null;
            var x = GetRoleName();
            if (x != null)
            {
                HttpCookie cookierolename = new HttpCookie("RoleName", x.ROLE_NAME);
                Response.Cookies.Add(cookierolename);
            }
            if (Session["Name"] != null)
            {
                HttpCookie cookiename = new HttpCookie("Name", Session["Name"].ToString());
                Response.Cookies.Add(cookiename);
            }

            ViewBag.Name = Request.Cookies["Name"].Value.ToString();
            ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            ViewBag.RoleName = Request.Cookies["RoleName"].Value.ToString();
            var yetki = GetYetki();
            var yetkiKontrolSatis = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 1).USER_AUTH;

            var yetkiKontrolStok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 2).USER_AUTH;
            var yetkiKontrolUretim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 3).USER_AUTH;
            var yetkiKontrolSatinAlma = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 4).USER_AUTH;
            var yetkiKontrolFinans = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 5).USER_AUTH;
            var yetkiKontrolMuhasebe = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 6).USER_AUTH;
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 9).USER_AUTH;
            var kullaniciayar = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 10).USER_AUTH;
            var kullaniciyetki = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 11).USER_AUTH;
            var istatistik = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 12).USER_AUTH;
            var yetkiKontrolYonetim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 8).USER_AUTH;
            var yetkiKontrolSube = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 15).USER_AUTH;
            var yetkiKontrolSevkiyat = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 17).USER_AUTH;
            var yetkiKontrolDetayliSip = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 16).USER_AUTH;
            var yetkiKontrolSiparisRaporu = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 18).USER_AUTH;
            var yetkiKontrolIstatistik = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 8).USER_AUTH;
            var yetkidetaylisatinalma = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 19).USER_AUTH;
            var yetkisaticisiparisi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 20).USER_AUTH;
            var isemrikayit= yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 27).USER_AUTH;
            var yetkianlikuretim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 26).USER_AUTH;
            var yetkifiyatlistok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 28).USER_AUTH;
            var yetkifiyatsizstok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 29).USER_AUTH;
            var ziyaretkaydi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var musteriraporu = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var musteriraporuozel = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 31).USER_AUTH;
            var ziyaretplani = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var yonetimstok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 33).USER_AUTH;
            var fiyatyonetim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 34).USER_AUTH;
            var fiyatlistesi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 35).USER_AUTH;
            var kuryetki = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 36).USER_AUTH;
            if (kuryetki != true)
            {
                ViewBag.DisplayKur = "none";
            }
            else
            {
                ViewBag.DisplayKur = "unset";
            }
            if (fiyatlistesi != true)
            {
                ViewBag.DisplayFiyatListesi = "none";
            }
            else
            {
                ViewBag.DisplayFiyatListesi = "unset";
            }
            if (fiyatyonetim != true)
            {
                ViewBag.FiyatYonetim = "none";
            }
            else
            {
                ViewBag.FiyatYonetim = "unset";
            }
            if (yonetimstok != true)
            {
                ViewBag.Stok = "none";
            }
            else
            {
                ViewBag.Stok = "unset";
            }
            if (ziyaretplani != true)
            {
                ViewBag.DisplayZiyaretPlani = "none";
            }
            else
            {
                ViewBag.DisplayZiyaretPlani = "unset";
            }
            if (musteriraporuozel != true)
            {
                ViewBag.DisplayMusteriOzel = "none";
            }
            else
            {
                ViewBag.DisplayMusteriOzel = "unset";
            }
            if ((musteriraporu == true && musteriraporuozel==false)||(Request.Cookies["Id"].Value=="10001" || Request.Cookies["Id"].Value == "10002"))
            {
                ViewBag.DisplayMusteriRaporu = "unset";
            }
            else
            {
                ViewBag.DisplayMusteriRaporu = "none";
                
            }
            if (ziyaretkaydi != true)
            {
                ViewBag.DisplayZiyaretKaydi = "none";
            }
            else
            {
                ViewBag.DisplayZiyaretKaydi = "unset";
            }
            if (yetkifiyatsizstok != true)
            {
                ViewBag.DisplayFiyatsizStok = "none";
            }
            else
            {
                ViewBag.DisplayFiyatsizStok = "unset";
            }
            if (yetkifiyatlistok != true)
            {
                ViewBag.DisplayFiyatliStok = "none";
            }
            else
            {
                ViewBag.DisplayFiyatliStok = "unset";
            }
            if (yetkianlikuretim != true)
            {
                ViewBag.DisplayAnlikUretim = "none";
            }
            else
            {
                ViewBag.DisplayAnlikUretim = "unset";
            }
            if (isemrikayit != true)
            {
                ViewBag.DisplayIsEmriKayit = "none";
            }
            else
            {
                ViewBag.DisplayIsEmriKayit = "unset";
            }
            if (yetkisaticisiparisi != true)
            {
                ViewBag.DisplaySaticiSiparisRaporu = "none";
            }
            else
            {
                ViewBag.DisplaySaticiSiparisRaporu = "unset";
            }
            if (yetkidetaylisatinalma != true)
            {
                ViewBag.DisplaySatinAlmaRaporu = "none";
            }
            else
            {
                ViewBag.DisplaySatinAlmaRaporu = "unset";
            }
            if (yetkiKontrolIstatistik != true)
            {
                ViewBag.DisplayIstatistik = "none";
            }
            else
            {
                ViewBag.DisplayIstatistik = "unset";
            }
            if (yetkiKontrolSiparisRaporu != true)
            {
                ViewBag.DisplaySiparisRaporu = "none";
            }
            else
            {
                ViewBag.DisplaySiparisRaporu = "unset";
            }
            if (yetkiKontrolSevkiyat != true)
            {
                ViewBag.DisplaySevkiyat = "none";
            }
            else
            {
                ViewBag.DisplaySevkiyat = "unset";
            }
            if (yetkiKontrolDetayliSip != true)
            {
                ViewBag.DisplaySiparis = "none";
            }
            else
            {
                ViewBag.DisplaySiparis = "unset";
            }
            if (yetkiKontrolSube != true)
            {
                ViewBag.Sube = "none";
            }
            else
            {
                ViewBag.Sube = "unset";
            }
            if (yetkiKontrolYonetim != true)
            {
                ViewBag.DisplayYonetim = "none";
            }
            else
            {
                ViewBag.DisplayYonetim = "unset";
            }
            if (yetkiKontrolUretim != true)
            {
                ViewBag.DisplayUretim = "none";
            }
            else
            {
                ViewBag.DisplayUretim = "unset";
            }
            if (yetkiKontrolSatinAlma != true)
            {
                ViewBag.DisplaySatinAlma = "none";
            }
            else
            {
                ViewBag.DisplaySatinAlma = "unset";
            }
            if (yetkiKontrolFinans != true)
            {
                ViewBag.DisplayFinans = "none";
            }
            else
            {
                ViewBag.DisplayFinans = "unset";
            }
            if (yetkiKontrolMuhasebe != true)
            {
                ViewBag.DisplayMuhasebe = "none";
            }
            else
            {
                ViewBag.DisplayMuhasebe = "unset";
            }
            if (yetkiKontrolStok != true)
            {
                ViewBag.DisplayStok = "none";
            }
            else
            {
                ViewBag.DisplayStok = "unset";
            }
            if (yetkiKontrolSatis != true)
            {
                ViewBag.DisplaySatis = "none";
            }
            else
            {
                ViewBag.DisplaySatis = "unset";
            }
            if (yetkiKontrol != true)
            {
                ViewBag.Display = "none";
            }
            else
            {
                ViewBag.Display = "unset";
            }
            if (kullaniciayar != true)
            {
                ViewBag.Display1 = "none";
            }
            else
            {
                ViewBag.Display1 = "unset";
            }
            if (kullaniciyetki != true)
            {
                ViewBag.Display2 = "none";
            }
            else
            {
                ViewBag.Display2 = "unset";
            }
            if (istatistik != true)
            {
                ViewBag.Display3 = "none";
            }
            else
            {
                ViewBag.Display3 = "unset";
            }
            var ik = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 7).USER_AUTH;
            if (ik != true)
            {
                ViewBag.Display4 = "none";
            }
            else
            {
                ViewBag.Display4 = "unset";
            }
            var ik1 = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 13).USER_AUTH;
            if (ik1 != true)
            {
                ViewBag.Display5 = "none";
            }
            else
            {
                ViewBag.Display5 = "unset";
            }
            var ik2 = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 14).USER_AUTH;
            if (ik2 != true)
            {
                ViewBag.Display6 = "none";
            }
            else
            {
                ViewBag.Display6 = "unset";
            }
            if (Session["UserId"] != null)
            {
                HttpCookie cookieid = new HttpCookie("Id", Session["UserId"].ToString());
                Response.Cookies.Add(cookieid);
            }





            List<string> offlineList = new List<string>();
            List<string> offlineday = new List<string>();
            List<string> offlinehour = new List<string>();
            List<string> offlineminute = new List<string>();

            ViewBag.OnlineUsers = GetOnlineUsers();
            ViewBag.OfflineUsers = GetOfflineUsers();
            var dates = GetOfflineDates();
            
            for (int i = 0; i < ViewBag.OfflineUsers.Count; i++)
            {
                if (dates.Where(t => t.USER_ID == ViewBag.OfflineUsers[i].USER_ID) != null)
                {
                    try
                    {
                        var lehlul=(dates.Where(t => t.USER_ID == ViewBag.OfflineUsers[i].USER_ID)).ToList();
                        
                        string day = TimeSpan.FromMilliseconds((DateTime.Now - DateTime.Parse(lehlul[0].LOG_DATETIME)).TotalMilliseconds).Days.ToString();
                        string hour = TimeSpan.FromMilliseconds((DateTime.Now - DateTime.Parse(lehlul[0].LOG_DATETIME)).TotalMilliseconds).Hours.ToString();
                        string minute = TimeSpan.FromMilliseconds((DateTime.Now - DateTime.Parse(lehlul[0].LOG_DATETIME)).TotalMilliseconds).Minutes.ToString();
                        string xs = TimeSpan.FromMilliseconds((DateTime.Now - DateTime.Parse(lehlul[0].LOG_DATETIME)).TotalMilliseconds).ToString();

                        offlineList.Add(xs);
                        offlineday.Add(day);
                        offlinehour.Add(hour);
                        offlineminute.Add(minute);
                    }
                    catch (Exception)
                    {
                        offlineList.Add(" ");
                        offlineday.Add(" ");
                        offlinehour.Add(" ");
                        offlineminute.Add(" ");
                    }

                }
                else
                {
                    offlineList.Add(" ");
                    offlineday.Add(" ");
                    offlinehour.Add(" ");
                    offlineminute.Add(" ");
                }

            }
            ViewBag.OfflineDate = offlineList;
            ViewBag.OfflineDay = offlineday;
            ViewBag.OfflineHour = offlinehour;
            ViewBag.OfflineMinute = offlineminute;
            return View();
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

        public List<User> GetYetki(int id, int inckey)
        {
            var apiUrl = "http://192.168.2.13:83/api/userwithroles/" + id + "/" + inckey;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<User> jsonList = ser.Deserialize<List<User>>(json);
            return jsonList;
        }

        public List<User> GetYetki()
        {
            var apiUrl = "http://192.168.2.13:83/api/userwithroles";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<User> jsonList = ser.Deserialize<List<User>>(json);
            return jsonList;
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                try
                {
                    var filename = Request.Cookies["Id"].Value.ToString() + ".png";
                    var path = Path.Combine(Server.MapPath("/assets/img/avatars/"), filename);
                    file.SaveAs(path);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return RedirectToAction("Index", "Profil");
        }

        public List<Online> GetOnline()
        {
            var apiUrl = "http://192.168.2.13:83/api/online/online";
            var apiUrl2 = "http://192.168.2.13:83/api/online/busy";
            //Connect API
            Uri url = new Uri(apiUrl);
            Uri url2 = new Uri(apiUrl2);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            string json2 = client.DownloadString(url2);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Online> jsonList = ser.Deserialize<List<Online>>(json);
            List<Online> jsonList2 = ser.Deserialize<List<Online>>(json2);
            jsonList.AddRange(jsonList2);
            return jsonList;
        }
        public async Task<ActionResult> StatuKayıt(Online user)
        {

            Online online = new Online();
            online.USER_ID = user.USER_ID;
            online.USER_NAME = user.USER_NAME;
            online.ACTIVITY_TYPE = user.ACTIVITY_TYPE;

            var apiUrl = "http://192.168.2.13:83/api/Online/" + user.USER_ID;


            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(online), Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);



            return null;

        }

        public string Refresh()
        {
            string check = "";

            ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            ViewBag.RoleName = Request.Cookies["RoleName"].Value.ToString();
            ViewBag.Name = Request.Cookies["Name"].Value.ToString();
            ViewBag.OnlineUsers = GetOnlineUsers();
            ViewBag.OfflineUsers = GetOfflineUsers();
            List<string> offlineList = new List<string>();
            List<string> offlineday = new List<string>();
            List<string> offlinehour = new List<string>();
            List<string> offlineminute = new List<string>();
            for (int i = 0; i < ViewBag.OfflineUsers.Count; i++)
            {
                if (GetOfflineDates(ViewBag.OfflineUsers[i].USER_ID) != null)
                {
                    try
                    {
                        var date = TimeSpan.FromMilliseconds((DateTime.Now - DateTime.Parse(GetOfflineDates(ViewBag.OfflineUsers[i].USER_ID)[0].LOG_DATETIME)).TotalMilliseconds);
                        string day = date.Days.ToString();
                        string hour = date.Hours.ToString();
                        string minute = date.Minutes.ToString();
                        string x = date.ToString();
                        offlineList.Add(x);
                        offlineday.Add(day);
                        offlinehour.Add(hour);
                        offlineminute.Add(minute);
                    }
                    catch (Exception)
                    {

                        offlineList.Add(" ");
                        offlineday.Add(" ");
                        offlinehour.Add(" ");
                        offlineminute.Add(" ");
                    }

                }
                else
                {
                    offlineList.Add(" ");
                    offlineday.Add(" ");
                    offlinehour.Add(" ");
                    offlineminute.Add(" ");

                }

            }
            ViewBag.OfflineDate = offlineList;
            ViewBag.OfflineDay = offlineday;
            ViewBag.OfflineHour = offlinehour;
            ViewBag.OfflineMinute = offlineminute;
            check = "true";
            return check;

        }
        public User GetKullaniciApiData()
        {


            var apiUrl = "http://192.168.2.13:83/api/user/" + Request.Cookies["Id"].Value.ToInt();

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            User jsonList = ser.Deserialize<User>(json);

            //END

            return jsonList;
        }
        public User GetUsersApiData(int id)
        {


            var apiUrl = "http://192.168.2.13:83/api/user/" + id;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            User jsonList = ser.Deserialize<User>(json);

            //END

            return jsonList;
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(Login pw)
        {
            string pass = GetKullaniciApiData().USER_PASSWORD;
            User user = new User();
            if(pw.PasswordChange.NEW_PASSWORD!=null&& pw.PasswordChange.NEW_PASSWORD_REPEAT != null)
            {
                if (pw.PasswordChange.OLD_PASSWORD.ToString().Equals(pass))
                {
                    if (pw.PasswordChange.NEW_PASSWORD.ToString() == pw.PasswordChange.NEW_PASSWORD_REPEAT.ToString())
                    {
                        user.USER_ID = Request.Cookies["Id"].Value;
                        user.USER_NAME = Request.Cookies["UserName"].Value.ToString();
                        user.USER_PASSWORD = pw.PasswordChange.NEW_PASSWORD;
                        user.USER_FIRSTNAME = Request.Cookies["FirstName"].Value.ToString();
                        user.USER_LASTNAME = Request.Cookies["LastName"].Value.ToString();
                        user.USER_MAIL = Request.Cookies["Mail"].Value.ToString();
                        user.USER_ROLE = Request.Cookies["Role"].Value.ToString();
                        user.ACTIVE = true;
                        var apiUrl = "http://192.168.2.13:83/api/user/" + Request.Cookies["Id"].Value.ToInt();


                        var httpClient = new HttpClient();
                        var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
                        {
                            Content = new StringContent(new JavaScriptSerializer().Serialize(user), Encoding.UTF8, "application/json")

                        };

                        var response = await httpClient.SendAsync(request);

                        string apiResponse = await response.Content.ReadAsStringAsync();


                        Session["Alert"] = "Şifre değişikliği başarılı!";
                    }
                    else
                    {
                        Session["Alert"] = "Girmiş olduğunuz şifreler aynı değil!";
                        return RedirectToAction("Index", "Profil");
                    }
                }
            }
           
            else
            {
                Session["Alert"] = "Lütfen gerekli alanları doldurduğunuzdan emin olun!";
                return RedirectToAction("Index", "Profil");
            }




            return RedirectToAction("Index", "Profil");

        }
        public List<Online> GetOnlineUsers()
        {


            var apiUrl = "http://192.168.2.13:83/api/log/login";
            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Online> jsonList = ser.Deserialize<List<Online>>(json);

            //END

            return jsonList;
        }
        public List<Online> GetOfflineUsers()
        {


            var apiUrl = "http://192.168.2.13:83/api/log/logout";
            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Online> jsonList = ser.Deserialize<List<Online>>(json);
            //END

            return jsonList;
        }
        public List<SignIn> GetOfflineDates(int id)
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
            if (jsonList[0].LOG_DATETIME == null)
            {
                jsonList = null;
            }
            return jsonList;
        }
        public List<SignIn> GetOfflineDates()
        {


            var apiUrl = "http://192.168.2.13:83/api/log";
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
            if (jsonList[0].LOG_DATETIME == null)
            {
                jsonList = null;
            }
            return jsonList;
        }


        public JsonResult SessionInfo()
        {

            if (Session["Log"] == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
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

    }


}

