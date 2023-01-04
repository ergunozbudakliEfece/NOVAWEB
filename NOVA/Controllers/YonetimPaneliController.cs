﻿
//using NetOpenX50;
using NOVA.Models;
using ServiceStack;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Text;
using System.Threading.Tasks;

using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace NOVA.Controllers
{
    
    public class YonetimPaneliController : Controller
    {
        
        [HandleError]
        public ActionResult YetkiAyar()
        {
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

            // Stop Caching in Firefox
            Response.Cache.SetNoStore();
            var yetkiKontrol = GetYetki(Request.Cookies["Id"].Value.ToInt(), 11)[0];
            if (yetkiKontrol.SELECT_AUTH != true)
            {
                Session["ModulYetkiMesajı"] = "Modüle yetkiniz bulunmamaktadır";
                return RedirectToAction("Index", "Home");
            }
            if (yetkiKontrol.UPDATE_AUTH == true)
            {
                ViewBag.Update = "Yetkili";
            }
            if (yetkiKontrol.DELETE_AUTH == true)
            {
                ViewBag.Delete = "Yetkili";
            }
            if (yetkiKontrol.INSERT_AUTH == true)
            {
                ViewBag.Insert = "Yetkili";
            }



            // Stop Caching in Firefox

            Session["Alert"] = null;
            ViewBag.Page = 3;

            if (Session["Update"] != null)
            {
                ViewBag.Updated = "true";
            }



            ViewBag.Users = TempData["mydata"];
            ViewBag.Module_id = TempData["moduleid"];
            if (ViewBag.Module_id == null)
            {
                ViewBag.Module_id = 0;

            }
            if (ViewBag.Users == null)
            {
                if (TempData["mydata"] != null)
                {
                    Yetki(1);
                    ViewBag.Users = TempData["mydata"];
                }
                else
                {
                    ViewBag.Users = GetUsersApiData().OrderBy(t => t.USER_ID);
                }

            }
            var s = new SelectList(GetRoleApiData(), "ROLE_ID", "ROLE_NAME");
            ViewBag.Modules = GetModulesApiData();
            ViewBag.Roles = s;
            ViewBag.Roles1 = GetRoleApiData();
            ViewBag.RolesAuth = GetRolesAuthData();

            ViewBag.Name = Session["Name"];
            ViewBag.RoleName = Request.Cookies["RoleName"].Value.ToString();
            ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            var yetkiKontrolyonetim = GetYetki(Request.Cookies["Id"].Value.ToInt(), 9)[0];
            var kullaniciayar = GetYetki(Request.Cookies["Id"].Value.ToInt(), 10)[0];
            var kullaniciyetki = GetYetki(Request.Cookies["Id"].Value.ToInt(), 11)[0];
            var istatistik = GetYetki(Request.Cookies["Id"].Value.ToInt(), 12)[0];
            var yetkiKontrolSatis = GetYetki(Request.Cookies["Id"].Value.ToInt(), 1)[0];
            var yetkiKontrolStok = GetYetki(Request.Cookies["Id"].Value.ToInt(), 2)[0];
            var yetkiKontrolUretim = GetYetki(Request.Cookies["Id"].Value.ToInt(), 3)[0];
            var yetkiKontrolSatinAlma = GetYetki(Request.Cookies["Id"].Value.ToInt(), 4)[0];
            var yetkiKontrolFinans = GetYetki(Request.Cookies["Id"].Value.ToInt(), 5)[0];
            var yetkiKontrolMuhasebe = GetYetki(Request.Cookies["Id"].Value.ToInt(), 6)[0];
            var yetkiKontrolYonetim = GetYetki(Request.Cookies["Id"].Value.ToInt(), 8)[0];
            var yetkiKontrolSevkiyat = GetYetki(Request.Cookies["Id"].Value.ToInt(), 17)[0];
            var yetkiKontrolDetayliSip = GetYetki(Request.Cookies["Id"].Value.ToInt(), 16)[0];
            var yetkiKontrolSiparisRaporu = GetYetki(Request.Cookies["Id"].Value.ToInt(), 18)[0];
            var yetkidetaylisatinalma = GetYetki(Request.Cookies["Id"].Value.ToInt(), 19)[0];
            var yetkisaticisiparisi = GetYetki(Request.Cookies["Id"].Value.ToInt(), 20)[0];
            var yetkifiyatlistok = GetYetki(Request.Cookies["Id"].Value.ToInt(), 28)[0];
            var yetkifiyatsizstok = GetYetki(Request.Cookies["Id"].Value.ToInt(), 29)[0];
            var ziyaretkaydi = GetYetki(Request.Cookies["Id"].Value.ToInt(), 30)[0];
            
            if (ziyaretkaydi.USER_AUTH != true)
            {
                ViewBag.DisplayZiyaretKaydi = "none";
            }
            else
            {
                ViewBag.DisplayZiyaretKaydi = "unset";
            }
            if (yetkifiyatsizstok.USER_AUTH != true)
            {
                ViewBag.DisplayFiyatsizStok = "none";
            }
            else
            {
                ViewBag.DisplayFiyatsizStok = "unset";
            }
            if (yetkifiyatlistok.USER_AUTH != true)
            {
                ViewBag.DisplayFiyatliStok = "none";
            }
            else
            {
                ViewBag.DisplayFiyatliStok = "unset";
            }
            if (yetkisaticisiparisi.USER_AUTH != true)
            {
                ViewBag.DisplaySaticiSiparisRaporu = "none";
            }
            else
            {
                ViewBag.DisplaySaticiSiparisRaporu = "unset";
            }
            if (yetkidetaylisatinalma.USER_AUTH != true)
            {
                ViewBag.DisplaySatinAlmaRaporu = "none";
            }
            else
            {
                ViewBag.DisplaySatinAlmaRaporu = "unset";
            }
            if (yetkiKontrolSiparisRaporu.USER_AUTH != true)
            {
                ViewBag.DisplaySiparisRaporu = "none";
            }
            else
            {
                ViewBag.DisplaySiparisRaporu = "unset";
            }
            if (yetkiKontrolSevkiyat.USER_AUTH != true)
            {
                ViewBag.DisplaySevkiyat = "none";
            }
            else
            {
                ViewBag.DisplaySevkiyat = "unset";
            }
            if (yetkiKontrolDetayliSip.USER_AUTH != true)
            {
                ViewBag.DisplaySiparis = "none";
            }
            else
            {
                ViewBag.DisplaySiparis = "unset";
            }
            if (yetkiKontrolYonetim.USER_AUTH != true)
            {
                ViewBag.DisplayYonetim = "none";
            }
            else
            {
                ViewBag.DisplayYonetim = "unset";
            }
            if (yetkiKontrolUretim.USER_AUTH != true)
            {
                ViewBag.DisplayUretim = "none";
            }
            else
            {
                ViewBag.DisplayUretim = "unset";
            }
            if (yetkiKontrolSatinAlma.USER_AUTH != true)
            {
                ViewBag.DisplaySatinAlma = "none";
            }
            else
            {
                ViewBag.DisplaySatinAlma = "unset";
            }
            if (yetkiKontrolFinans.USER_AUTH != true)
            {
                ViewBag.DisplayFinans = "none";
            }
            else
            {
                ViewBag.DisplayFinans = "unset";
            }
            if (yetkiKontrolMuhasebe.USER_AUTH != true)
            {
                ViewBag.DisplayMuhasebe = "none";
            }
            else
            {
                ViewBag.DisplayMuhasebe = "unset";
            }
            if (yetkiKontrolStok.USER_AUTH != true)
            {
                ViewBag.DisplayStok = "none";
            }
            else
            {
                ViewBag.DisplayStok = "unset";
            }
            if (yetkiKontrolSatis.USER_AUTH != true)
            {
                ViewBag.DisplaySatis = "none";
            }
            if (yetkiKontrolyonetim.USER_AUTH != true)
            {
                ViewBag.Display = "none";
            }
            else
            {
                ViewBag.Display = "unset";
            }
            if (kullaniciayar.USER_AUTH != true)
            {
                ViewBag.Display1 = "none";
            }
            else
            {
                ViewBag.Display1 = "unset";
            }
            if (kullaniciyetki.USER_AUTH != true)
            {
                ViewBag.Display2 = "none";
            }
            else
            {
                ViewBag.Display2 = "unset";
            }
            if (istatistik.USER_AUTH != true)
            {
                ViewBag.Display3 = "none";
            }
            else
            {
                ViewBag.Display3 = "unset";
            }
            var ik = GetYetki(Request.Cookies["Id"].Value.ToInt(), 7)[0];
            if (ik.USER_AUTH != true)
            {
                ViewBag.Display4 = "none";
            }
            else
            {
                ViewBag.Display4 = "unset";
            }
            var ik1 = GetYetki(Request.Cookies["Id"].Value.ToInt(), 13)[0];
            if (ik1.USER_AUTH != true)
            {
                ViewBag.Display5 = "none";
            }
            else
            {
                ViewBag.Display5 = "unset";
            }
            var ik2 = GetYetki(Request.Cookies["Id"].Value.ToInt(), 14)[0];
            if (ik2.USER_AUTH != true)
            {
                ViewBag.Display6 = "none";
            }
            else
            {
                ViewBag.Display6 = "unset";
            }

            return View("Yetki");
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
        public ActionResult KullaniciAyar()
        {
            if (Session["Durum"] != null)
            {
                ViewBag.Durum = Session["Durum"];
            }
            else
            {
                ViewBag.Durum = null;
            }
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

            // Stop Caching in Firefox
            Response.Cache.SetNoStore();
            var yetkiKontrol = GetYetki(Request.Cookies["Id"].Value.ToInt(), 10)[0];
            if (yetkiKontrol.SELECT_AUTH != true)
            {
                Session["ModulYetkiMesajı"] = "Modüle yetkiniz bulunmamaktadır";
                return RedirectToAction("Index", "Home");
            }
            if (yetkiKontrol.UPDATE_AUTH == true)
            {
                ViewBag.Update = "Yetkili";
            }
            if (yetkiKontrol.DELETE_AUTH == true)
            {
                ViewBag.Delete = "Yetkili";
            }
            if (yetkiKontrol.INSERT_AUTH == true)
            {
                ViewBag.INSERT = "Yetkili";
            }
            Session["Alert"] = null;

            ViewBag.Page = 2;



            ViewBag.Users = GetKullaniciApiData().OrderBy(t => t.USER_ID);


            var s = new SelectList(GetRoleApiData().Select(u => new { Fullname = u.ROLE_ID + "-" + u.ROLE_NAME, USER_ROLE = u.ROLE_ID, SELECTED = u.ROLE_ID }), "USER_ROLE", "Fullname");
            ViewBag.Roles = s;
            ViewBag.Roles1 = GetRoleApiData();


            ViewBag.RoleName = Request.Cookies["RoleName"].Value.ToString();
            ViewBag.Name = Session["Name"];
            var yetkiKontrolyonetim = GetYetki(Request.Cookies["Id"].Value.ToInt(), 9)[0];
            var kullaniciayar = GetYetki(Request.Cookies["Id"].Value.ToInt(), 10)[0];
            var kullaniciyetki = GetYetki(Request.Cookies["Id"].Value.ToInt(), 11)[0];
            var istatistik = GetYetki(Request.Cookies["Id"].Value.ToInt(), 12)[0];
            var yetkiKontrolSatis = GetYetki(Request.Cookies["Id"].Value.ToInt(), 1)[0];
            var yetkiKontrolStok = GetYetki(Request.Cookies["Id"].Value.ToInt(), 2)[0];
            var yetkiKontrolUretim = GetYetki(Request.Cookies["Id"].Value.ToInt(), 3)[0];
            var yetkiKontrolSatinAlma = GetYetki(Request.Cookies["Id"].Value.ToInt(), 4)[0];
            var yetkiKontrolFinans = GetYetki(Request.Cookies["Id"].Value.ToInt(), 5)[0];
            var yetkiKontrolMuhasebe = GetYetki(Request.Cookies["Id"].Value.ToInt(), 6)[0];
            var yetkiKontrolYonetim = GetYetki(Request.Cookies["Id"].Value.ToInt(), 8)[0];
            var yetkiKontrolSevkiyat = GetYetki(Request.Cookies["Id"].Value.ToInt(), 17)[0];
            var yetkiKontrolDetayliSip = GetYetki(Request.Cookies["Id"].Value.ToInt(), 16)[0];
            var yetkiKontrolSiparisRaporu = GetYetki(Request.Cookies["Id"].Value.ToInt(), 18)[0];
            var yetkidetaylisatinalma = GetYetki(Request.Cookies["Id"].Value.ToInt(), 19)[0];
            var yetkisaticisiparisi = GetYetki(Request.Cookies["Id"].Value.ToInt(), 20)[0];
            var yetkifiyatlistok = GetYetki(Request.Cookies["Id"].Value.ToInt(), 28)[0];
            var yetkifiyatsizstok = GetYetki(Request.Cookies["Id"].Value.ToInt(), 29)[0];
            var ziyaretkaydi = GetYetki(Request.Cookies["Id"].Value.ToInt(), 30)[0];

            if (ziyaretkaydi.USER_AUTH != true)
            {
                ViewBag.DisplayZiyaretKaydi = "none";
            }
            else
            {
                ViewBag.DisplayZiyaretKaydi = "unset";
            }
            if (yetkifiyatsizstok.USER_AUTH != true)
            {
                ViewBag.DisplayFiyatsizStok = "none";
            }
            else
            {
                ViewBag.DisplayFiyatsizStok = "unset";
            }
            if (yetkifiyatlistok.USER_AUTH != true)
            {
                ViewBag.DisplayFiyatliStok = "none";
            }
            else
            {
                ViewBag.DisplayFiyatliStok = "unset";
            }
            if (yetkisaticisiparisi.USER_AUTH != true)
            {
                ViewBag.DisplaySaticiSiparisRaporu = "none";
            }
            else
            {
                ViewBag.DisplaySaticiSiparisRaporu = "unset";
            }
            if (yetkidetaylisatinalma.USER_AUTH != true)
            {
                ViewBag.DisplaySatinAlmaRaporu = "none";
            }
            else
            {
                ViewBag.DisplaySatinAlmaRaporu = "unset";
            }
            if (yetkiKontrolSiparisRaporu.USER_AUTH != true)
            {
                ViewBag.DisplaySiparisRaporu = "none";
            }
            else
            {
                ViewBag.DisplaySiparisRaporu = "unset";
            }
            if (yetkiKontrolSevkiyat.USER_AUTH != true)
            {
                ViewBag.DisplaySevkiyat = "none";
            }
            else
            {
                ViewBag.DisplaySevkiyat = "unset";
            }
            if (yetkiKontrolDetayliSip.USER_AUTH != true)
            {
                ViewBag.DisplaySiparis = "none";
            }
            else
            {
                ViewBag.DisplaySiparis = "unset";
            }
            if (yetkiKontrolYonetim.USER_AUTH != true)
            {
                ViewBag.DisplayYonetim = "none";
            }
            else
            {
                ViewBag.DisplayYonetim = "unset";
            }
            if (yetkiKontrolUretim.USER_AUTH != true)
            {
                ViewBag.DisplayUretim = "none";
            }
            else
            {
                ViewBag.DisplayUretim = "unset";
            }
            if (yetkiKontrolSatinAlma.USER_AUTH != true)
            {
                ViewBag.DisplaySatinAlma = "none";
            }
            else
            {
                ViewBag.DisplaySatinAlma = "unset";
            }
            if (yetkiKontrolFinans.USER_AUTH != true)
            {
                ViewBag.DisplayFinans = "none";
            }
            else
            {
                ViewBag.DisplayFinans = "unset";
            }
            if (yetkiKontrolMuhasebe.USER_AUTH != true)
            {
                ViewBag.DisplayMuhasebe = "none";
            }
            else
            {
                ViewBag.DisplayMuhasebe = "unset";
            }
            if (yetkiKontrolStok.USER_AUTH != true)
            {
                ViewBag.DisplayStok = "none";
            }
            else
            {
                ViewBag.DisplayStok = "unset";
            }
            if (yetkiKontrolSatis.USER_AUTH != true)
            {
                ViewBag.DisplaySatis = "none";
            }
            if (yetkiKontrolyonetim.USER_AUTH != true)
            {
                ViewBag.Display = "none";
            }
            else
            {
                ViewBag.Display = "unset";
            }
            if (kullaniciayar.USER_AUTH != true)
            {
                ViewBag.Display1 = "none";
            }
            else
            {
                ViewBag.Display1 = "unset";
            }
            if (kullaniciyetki.USER_AUTH != true)
            {
                ViewBag.Display2 = "none";
            }
            else
            {
                ViewBag.Display2 = "unset";
            }
            if (istatistik.USER_AUTH != true)
            {
                ViewBag.Display3 = "none";
            }
            else
            {
                ViewBag.Display3 = "unset";
            }
            var ik = GetYetki(Request.Cookies["Id"].Value.ToInt(), 7)[0];
            if (ik.USER_AUTH != true)
            {
                ViewBag.Display4 = "none";
            }
            else
            {
                ViewBag.Display4 = "unset";
            }
            var ik1 = GetYetki(Request.Cookies["Id"].Value.ToInt(), 13)[0];
            if (ik1.USER_AUTH != true)
            {
                ViewBag.Display5 = "none";
            }
            else
            {
                ViewBag.Display5 = "unset";
            }
            var ik2 = GetYetki(Request.Cookies["Id"].Value.ToInt(), 14)[0];
            if (ik2.USER_AUTH != true)
            {
                ViewBag.Display6 = "none";
            }
            else
            {
                ViewBag.Display6 = "unset";
            }
            ViewBag.Id = Request.Cookies["Id"].Value.ToString();



            return View("Kullanici");
        }
        public ActionResult MailFormu(string password,string mail,string username)
        {
            ViewBag.Password = password;
            ViewBag.MailAdresi = mail;
            ViewBag.UserName = username;
            return View("MailForm");
        }
        public ActionResult MailFormuToplu()
        {
            
            ViewBag.Mails = GetKullaniciApiData();
            return View("MailFormToplu");
        }
        public ActionResult Test()
        {
            ViewBag.Modules = GetModulesApiData();
            ViewBag.Users = GetUsersApiData();
            var s = new SelectList(GetRoleApiData(), "ROLE_ID", "ROLE_NAME");
            ViewBag.Roles = s;
            return View("Yedek");
        }
        public ActionResult Yetki(int? id)
        {
            Session["Update"] = null;

            if (id != null)
            {
                if (id == 0)
                {
                    TempData["moduleid"] = null;
                    TempData["mydata"] = null;
                    ViewBag.Modules = GetModulesApiData();
                    ViewBag.Users = GetUsersApiData();
                    var s = new SelectList(GetRoleApiData(), "ROLE_ID", "ROLE_NAME");
                    ViewBag.Roles = s;
                }
                else
                {
                    TempData["mydata"] = GetUsersApiData((int)id);
                    TempData["moduleid"] = GetUsersApiData((int)id)[0].MODULE_INCKEY;
                    var s = new SelectList(GetRoleApiData(), "ROLE_ID", "ROLE_NAME");
                    ViewBag.Modules = GetModulesApiData();
                    ViewBag.Roles = s;
                }
            }
           

            return RedirectToAction("YetkiAyar");
        }
        public List<User> GetUsersApiData()
        {


            var apiUrl = "http://192.168.2.13:83/api/userwithroles/";

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

        public List<User> GetUsersApiData(int inc)
        {


            var apiUrl = "http://192.168.2.13:83/api/userwithroles/inc:" + inc;

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
        public List<Modules> GetModulesApiData()
        {


            var apiUrl = "http://192.168.2.13:83/api/modules";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Modules> jsonList = ser.Deserialize<List<Modules>>(json);

            //END

            return jsonList;
        }
        public List<Models.Roles> GetRoleApiData()
        {


            var apiUrl = "http://192.168.2.13:83/api/roles";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Models.Roles> jsonList = ser.Deserialize<List<Models.Roles>>(json);

            //END

            return jsonList;
        }
        public List<RolesAuth> GetRolesAuthData()
        {


            var apiUrl = "http://192.168.2.13:83/api/rolesauth";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<RolesAuth> jsonList = ser.Deserialize<List<RolesAuth>>(json);

            //END

            return jsonList;
        }
        public User GetUserByMail(string mail)
        {


            var apiUrl = "http://192.168.2.13:83/api/user/mail:"+mail;

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
        public async Task<ActionResult> YetkiAyar([Bind(Prefix = "Item1")] User user)
        {


            var apiUrl = "http://192.168.2.13:83/api/UserAuth/" + user.USER_ID + "/" + user.MODULE_INCKEY;


            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(user), Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);

            string apiResponse = await response.Content.ReadAsStringAsync();
            ViewBag.Users = GetUsersApiData();
            ViewBag.Roles = GetRoleApiData();

            Session["Update"] = null;

            return RedirectToAction("YetkiAyar");

        }
        [HttpPost]
        public async Task<ActionResult> RoleAyar([Bind(Prefix = "Item2")] RolesAuth role)
        {


            var apiUrl = "http://192.168.2.13:83/api/rolesauth/" + role.ROLE_ID + "/" + role.MODULE_INCKEY;


            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(role), Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);

            string apiResponse = await response.Content.ReadAsStringAsync();
            ViewBag.Users = GetUsersApiData();
            ViewBag.Roles = GetRoleApiData();
            ViewBag.RolesAuth = GetRolesAuthData();

            Session["Update"] = "updated";
            return RedirectToAction("YetkiAyar");

        }
        [HttpPost]
        public ActionResult SendMail([Bind(Prefix = "Item2")] Mail model)
        {
            GetKullaniciApiData();

            if (model.MailAdresi.Contains("0"))
            {
                if (model.Icerik == null || model.Icerik == "")
                {
                    for (int i = 0; i < GetKullaniciApiData().Count; i++)
                    {
                        if(GetKullaniciApiData()[i].USER_MAIL==null || GetKullaniciApiData()[i].USER_MAIL == " ")
                        {
                            
                        }
                        else
                        {


                            string subject = "";
                            if (model.Baslik != "NOVA | Kullanıcı Bilgileriniz")
                            {
                                subject = model.Baslik;

                            }
                            else
                            {
                                subject = "NOVA | Kullanıcı Bilgileriniz";
                            }
                            string body = "Merhaba Çalışma Arkadaşım,</br></br>Nova, Efece Galvaniz çalışanların çalışmalarını kolaylaştırmak üzere tasarlanmış mobil ve web uygulamalardır.</br></br>Mobil uygulama yüklemek için “İş ve Süreç Geliştirme” departmanı ile iletişime geçebilirsiniz.</br></br>Web uygulamaya;</br></br>Bilgisayar üzerinde → <strong><a href='http://nova.efece.com'>NOVA</a></strong> </br></br>Mobil için → <strong><a href='http://192.168.2.13'>NOVA</a></strong> </br></br>Linklerine tıklayarak ulaşabilirsiniz.</br></br>Efece Galvaniz çatısı altında yapacağın çalışmalarında sana kolaylık sağlayabilmem için uygulamaya giriş yapabileceğin kullanıcı adın ve şifren aşağıdaki gibidir;</br></br><strong>Kullanıcı Adı: " + GetUserByMail(model.MailAdresi[i]).USER_NAME + "</strong></br></br><strong>Şifre: " + GetUserByMail(model.MailAdresi[i]).USER_PASSWORD + "</strong></br></br>Kullanıcı bilgileriniz yapacağınız işlemlerde kayıt tutulmasında kullanılacağı için şifrenizi uygulamaya giriş yaptıktan sonra kişisel olmayan bir şifrenizle değiştirip <strong><u>kimseyle</u></strong> paylaşmamanızı öneririz.";


                            WebMail.SmtpServer = "192.168.2.13";
                            WebMail.Send(GetKullaniciApiData()[i].USER_MAIL, subject, body, "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);

                        }
                        
                        
                    }
                }
                else
                {

                    string subject = "";
                    if (model.Baslik != "NOVA | Kullanıcı Bilgileriniz")
                    {
                        subject = model.Baslik;

                    }
                    else
                    {
                        subject = "NOVA | Kullanıcı Bilgileriniz";
                    }
                    string body = model.Icerik.ToString();

                    WebMail.SmtpServer = "192.168.2.13";
                    WebMail.Send(model.SingleMailAdresi.ToString(), subject, body, "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);

                }
            }
            else
            {
                if (model.Icerik == null || model.Icerik == "")
                {
                    string subject = "";
                    for (int i = 0; i < model.MailAdresi.Count; i++)
                    {

                        WebMail.SmtpServer = "192.168.2.13";
                        if(model.Baslik!= "NOVA | Kullanıcı Bilgileriniz")
                        {
                           subject = model.Baslik;

                        }
                        else
                        {
                           subject = "NOVA | Kullanıcı Bilgileriniz";
                        }

                        string body = "Merhaba Çalışma Arkadaşım,</br></br>Nova, Efece Galvaniz çalışanların çalışmalarını kolaylaştırmak üzere tasarlanmış mobil ve web uygulamalardır.</br></br>Mobil uygulama yüklemek için “İş ve Süreç Geliştirme” departmanı ile iletişime geçebilirsiniz.</br></br>Web uygulamaya;</br></br>Bilgisayar üzerinde → <strong><a href='http://nova.efece.com'>NOVA</a></strong> </br></br>Mobil için → <strong><a href='http://192.168.2.13'>NOVA</a></strong> </br></br>Linklerine tıklayarak ulaşabilirsiniz.</br></br>Efece Galvaniz çatısı altında yapacağın çalışmalarında sana kolaylık sağlayabilmem için uygulamaya giriş yapabileceğin kullanıcı adın ve şifren aşağıdaki gibidir;</br></br><strong>Kullanıcı Adı: " + GetUserByMail(model.MailAdresi[i]).USER_NAME + "</strong></br></br><strong>Şifre: " + GetUserByMail(model.MailAdresi[i]).USER_PASSWORD + "</strong></br></br>Kullanıcı bilgileriniz yapacağınız işlemlerde kayıt tutulmasında kullanılacağı için şifrenizi uygulamaya giriş yaptıktan sonra kişisel olmayan bir şifrenizle değiştirip <strong><u>kimseyle</u></strong> paylaşmamanızı öneririz.";

                        WebMail.Send(model.MailAdresi[i].ToString(), subject, body, "sistem@efecegalvaniz.com", null, null,true, null, null, null, null, null, null);
                      
                    }
                }
                else
                {
                    for (int i = 0; i < model.MailAdresi.Count; i++)
                    {
                        string subject = "";
                        if (model.Baslik != "NOVA | Kullanıcı Bilgileriniz")
                        {
                            subject = model.Baslik;

                        }
                        else
                        {
                            subject = "NOVA | Kullanıcı Bilgileriniz";
                        }
                        string body = model.Icerik+ "Yeni yılınız kutlu olsun!</br>Efece'de \"Dijital Dönüşüm Süreci\" nin başladığı ve en önemli adımlarından biri olan Nova ile tanıştığımız 2022 yılına veda ederken;</br>Geliştirilen yeni modülleriyle beraber aktif olarak kullanıldığı son 6 ayında Nova Web ve Nova Android uygulamalarındaki kullanım istatistiklerini paylaşmak istedik.</br></br>En Fazla Oturum Sayısı:</br></br>1.\tmustafaduzdaslik</br>2.\tahmetkoparan</br>3.\tmuratruzgar</br>4.\tselineken</br>5.\tbugraguclu</br>6.\tfikretcetin</br>7.\tyusufakgul</br>8.\tmanar</br>9.\terkangundogan</br>10.\tyaseminkurutac</br>En Fazla Oturum Açılan Platform:</br>1.\tAndroid App       →  % 44</br>2.\tGoogle Chrome →  % 43</br>3.\tOpera                   →  %  7</br>4.\tSafari                     →  %  3</br>5.\tMicrosoft Edge  →  %  2</br>6.\tFirefox                  →  %  1</br></br>İlk 10'a giren kullanıcılara teşekkürlerimizi sunar, 2023 yılının Nova'nın daha da aktif kullanıldığı, sağlıklı, mutlu, huzurlu ve başarılı bir yıl olmasını dileriz.</br>";

                        WebMail.SmtpServer = "192.168.2.13";
                        WebMail.Send(model.MailAdresi[i].ToString(), subject, body, "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);
                    }
                   
                }
            }
           
            
            return RedirectToAction("KullaniciAyar");
        }
        [HttpPost]
        public async Task<ActionResult> KullaniciAyar([Bind(Prefix = "Item1")] User user)
        {
            string apiUrl;
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;
            string apiResponse;

            if (Request.Form["submitbutton1"] != null)
            {
                apiUrl = "http://192.168.2.13:83/api/user/" + user.USER_ID;



                request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(user), Encoding.UTF8, "application/json")
                };

                response = await httpClient.SendAsync(request);

                apiResponse = await response.Content.ReadAsStringAsync();
                ViewBag.Users = GetKullaniciApiData();
                ViewBag.Roles = GetRoleApiData();
            }
            else if (Request.Form["submitButton2"] != null)
            {
                apiUrl = "http://192.168.2.13:83/api/user/" + user.USER_ID;



                request = new HttpRequestMessage(HttpMethod.Delete, apiUrl)
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(user), Encoding.UTF8, "application/json")
                };

                response = await httpClient.SendAsync(request);

                apiResponse = await response.Content.ReadAsStringAsync();
                ViewBag.Users = GetKullaniciApiData();
                ViewBag.Roles = GetRoleApiData();
            }

           
            return RedirectToAction("KullaniciAyar");

        }
        [HttpPost]
        public async Task<ActionResult> DeleteUser([Bind(Prefix = "Item1")] User user)
        {


            var apiUrl = "http://192.168.2.13:83/api/user/" + user.USER_ID;


            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(user), Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);

            string apiResponse = await response.Content.ReadAsStringAsync();
            ViewBag.Users = GetKullaniciApiData();
            ViewBag.Roles = GetRoleApiData();


            return RedirectToAction("KullaniciAyar");

        }

        public ActionResult MailForm(string password)
        {


            
            Session["password"] =password;
            
            return RedirectToAction("KullaniciAyar");

        }

        public List<User> GetKullaniciApiData()
        {


            var apiUrl = "http://192.168.2.13:83/api/user";

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

        [HttpPost]
        public async Task<ActionResult> ModulEkle([Bind(Prefix = "Item1.modules")] Modules modul)
        {
            var apiUrl = "http://192.168.2.13:83/api/modules/" + modul.INCKEY;
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            if (Request.Form["update"] != null)
            {
                


                
                request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(modul), Encoding.UTF8, "application/json")
                };

                await httpClient.SendAsync(request);

                
                ViewBag.Users = GetUsersApiData();
                ViewBag.Roles = GetRoleApiData();

            }
            else if (Request.Form["sil"] != null)
            {
                request = new HttpRequestMessage(HttpMethod.Delete, apiUrl)
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(modul), Encoding.UTF8, "application/json")
                };

                await httpClient.SendAsync(request);


                ViewBag.Users = GetUsersApiData();
                ViewBag.Roles = GetRoleApiData();
            }
           


            return RedirectToAction("YetkiAyar");

        }
        [HttpPost]
        public async Task<ActionResult> AddModule([Bind(Prefix = "Item1.modules")] Modules modul)
        {


            var apiUrl = "http://192.168.2.13:83/api/modules/";


            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(modul), Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);

            string apiResponse = await response.Content.ReadAsStringAsync();
            ViewBag.Users = GetUsersApiData();
            ViewBag.Roles = GetRoleApiData();



            return RedirectToAction("YetkiAyar");

        }
        [HttpPost]
        public async Task<ActionResult> AddUser([Bind(Prefix = "Item1")] UserAdd user)
        {
            try
            {
                var apiUrl = "http://192.168.2.13:83/api/user";

                var httpClient = new HttpClient();
                var req = new JavaScriptSerializer().Serialize(user);
                var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = new StringContent(req, Encoding.UTF8, "application/json")
                };

                var response = await httpClient.SendAsync(request);

                string apiResponse = await response.Content.ReadAsStringAsync();
                ViewBag.Users = GetUsersApiData();

                Session["Durum"] = "Kullanıcı başarıyla eklendi!";
                
            }
            catch (Exception)
            {

                Session["Durum"] = "Kullanıcı eklenemedi!";
            }

            return RedirectToAction("KullaniciAyar");
        }

        public ActionResult UserDataFromView(User user)
        {

            string name = user.USER_NAME;
            return View();
        }
    }
}