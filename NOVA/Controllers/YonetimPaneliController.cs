
//using NetOpenX50;
using NOVA.Models;
using ServiceStack;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using static NOVA.Controllers.LoginController;
using static ServiceStack.LicenseUtils;
using System.Security.Cryptography;
using System.IO;

namespace NOVA.Controllers
{
    
    public class YonetimPaneliController : Controller
    {
        
        [HandleError]
        public ActionResult YetkiAyar()
        {
           

           
            var yetki= GetYetki(Request.Cookies["Id"].Value.ToInt());
            var yetkiKontrol = yetki.FirstOrDefault(x=> x.MODULE_INCKEY==11);
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
            var yetkiKontrolyonetim = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 9);
            var kullaniciayar = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 10);
            var kullaniciyetki = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 11);
            var istatistik = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 12);
            var yetkiKontrolSatis = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 1);
            var yetkiKontrolStok = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 2);
            var yetkiKontrolUretim = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 3);
            var yetkiKontrolSatinAlma = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 4);
            var yetkiKontrolFinans = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 5);
            var yetkiKontrolMuhasebe = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 6);
            var yetkiKontrolYonetim = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 8);
            var yetkiKontrolSevkiyat = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 17);
            var yetkiKontrolDetayliSip = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 16);
            var yetkiKontrolSiparisRaporu = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 18);
            var yetkidetaylisatinalma = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 19);
            var yetkisaticisiparisi = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 20);
            var yetkifiyatlistok = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 18);
            var yetkifiyatsizstok = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 29);
            var ziyaretkaydi = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 30);
            var fiyatlistesi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 35).USER_AUTH;
            var kuryetki = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 36).USER_AUTH;
            var uygulamaistatistik = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 37).USER_AUTH;
            if (uygulamaistatistik != true)
            {
                ViewBag.Display2 = "none";
            }
            else
            {
                ViewBag.Display2 = "unset";
            }
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
            var ik = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 7);
            if (ik.USER_AUTH != true)
            {
                ViewBag.Display4 = "none";
            }
            else
            {
                ViewBag.Display4 = "unset";
            }
            var ik1 = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 13);
            if (ik1.USER_AUTH != true)
            {
                ViewBag.Display5 = "none";
            }
            else
            {
                ViewBag.Display5 = "unset";
            }
            var ik2 = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 14);
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
        public List<User> GetYetki(int id)
        {
            var apiUrl = "http://192.168.2.13:83/api/userwithroles/"+id;

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

       
        public ActionResult KullaniciAyar()
        {
            var m = GetModules(10);
            if (m[0].ACTIVE != "1")
            {
                return RedirectToAction("Maintenance", "Home");
            }
            if (Session["Durum"] != null)
            {
                ViewBag.Durum = Session["Durum"];
            }
            else
            {
                ViewBag.Durum = null;
            }
            if (Request.Cookies["Id"] == null)
            {
                FormsAuthentication.SignOut();
                TempData["LOG"] = "ok";
                return RedirectToAction("Login", "Login");
            }
            var yetki = GetYetki(Request.Cookies["Id"].Value.ToInt());
       
            var yetkiKontrol = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 10); 
            if (yetkiKontrol.SELECT_AUTH != true)
            {
                Session["ModulYetkiMesajı"] = "Modüle yetkiniz bulunmamaktadır";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //Kullanıcının en son logid si bulunur
                    string json1 = null;
                    LoginModel createdlog = null;
                    var apiUrl1 = "http://192.168.2.13:83/api/UserLogin/" + Request.Cookies["Id"].Value.ToInt();
                    Uri url1 = new Uri(apiUrl1);
                    WebClient client1 = new WebClient();
                    client1.Encoding = System.Text.Encoding.UTF8;

                    json1 = client1.DownloadString(url1);
                    JavaScriptSerializer ser1 = new JavaScriptSerializer();
                    createdlog = ser1.Deserialize<LoginModel>(json1);





                    //Kullanıcının en son logid si bulunur

                    string json2 = null;
                    List<ExecModel> createdlog1 = null;
                    var apiUrl2 = "http://192.168.2.13:83/api/UserLogin/exec/" + Request.Cookies["LogId"].Value;
                    Uri url2 = new Uri(apiUrl2);
                    WebClient client2 = new WebClient();
                    client2.Encoding = System.Text.Encoding.UTF8;

                    json2 = client2.DownloadString(url2);
                    JavaScriptSerializer ser2 = new JavaScriptSerializer();
                    createdlog1 = ser2.Deserialize<List<ExecModel>>(json2);

                    if (createdlog1[0].SITUATION != false)
                    {
                        LoginModel login = new LoginModel();
                        login.LOG_ID = createdlog.LOG_ID;
                        login.LAST_ACTIVITY = 10;
                        var apiUrlnew = "http://192.168.2.13:83/api/UserLogin";

                        var httpClientnew = new System.Net.Http.HttpClient();
                        var requestnew = new HttpRequestMessage(HttpMethod.Put, apiUrlnew)
                        {
                            Content = new StringContent(new JavaScriptSerializer().Serialize(login), Encoding.UTF8, "application/json")
                        };

                        var responsenew = httpClientnew.SendAsync(requestnew);
                    }
                    else
                    {
                        FormsAuthentication.SignOut();
                        return RedirectToAction("Login", "Login");
                    }


                
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



          


            ViewBag.RoleName = Request.Cookies["RoleName"].Value.ToString();
            ViewBag.Name = Session["Name"];
            var yetkiKontrolyonetim = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 9);
            var kullaniciayar = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 10);
            var kullaniciyetki = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 11);
            var istatistik = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 12);
            var yetkiKontrolSatis = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 1);
            var yetkiKontrolStok = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 2);
            var yetkiKontrolUretim = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 3);
            var yetkiKontrolSatinAlma = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 4);
            var yetkiKontrolFinans = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 5);
            var yetkiKontrolMuhasebe = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 6);
            var yetkiKontrolYonetim = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 8);
            var yetkiKontrolSevkiyat = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 17);
            var yetkiKontrolDetayliSip = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 16);
            var yetkiKontrolSiparisRaporu = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 18);
            var yetkidetaylisatinalma = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 19);
            var yetkisaticisiparisi = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 20);
            var yetkifiyatlistok = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 18);
            var yetkifiyatsizstok = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 29);
            var ziyaretkaydi = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 30);
            var fiyatyonetim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 34).USER_AUTH;
            var fiyatlistesi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 35).USER_AUTH;
            var kuryetki = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 36).USER_AUTH;
            var uygulamaistatistik = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 37).USER_AUTH;
            if (uygulamaistatistik != true)
            {
                ViewBag.Istatistik = "none";
            }
            else
            {
                ViewBag.Istatistik = "unset";
            }
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
            var ik = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 7);
            if (ik.USER_AUTH != true)
            {
                ViewBag.Display4 = "none";
            }
            else
            {
                ViewBag.Display4 = "unset";
            }
            var ik1 = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 13);
            if (ik1.USER_AUTH != true)
            {
                ViewBag.Display5 = "none";
            }
            else
            {
                ViewBag.Display5 = "unset";
            }
            var ik2 = yetki.FirstOrDefault(x => x.MODULE_INCKEY == 14);
            if (ik2.USER_AUTH != true)
            {
                ViewBag.Display6 = "none";
            }
            else
            {
                ViewBag.Display6 = "unset";
            }
            ViewBag.Id = Request.Cookies["Id"].Value.ToString();

            ViewBag.Users = GetUsersApiData();
            var s = new SelectList(GetRoleApiData(), "ROLE_ID", "ROLE_NAME");
            ViewBag.Roles = s;

            return View("Kullanici");
        }
        public ActionResult UygulamaIstatistik()
        {
            var m = GetModules(37);
            if (m[0].ACTIVE != "1")
            {
                return RedirectToAction("Maintenance", "Home");
            }

            if (Request.Cookies["Id"] == null)
            {
                FormsAuthentication.SignOut();
                TempData["LOG"] = "ok";
                return RedirectToAction("Login", "Login");
            }


            ViewBag.Page = 1;

            var yetki = GetYetki();
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 37);
            if (yetkiKontrol.SELECT_AUTH != true)
            {
                Session["ModulYetkiMesajı"] = "Modüle yetkiniz bulunmamaktadır";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //Kullanıcının en son logid si bulunur
                string json1 = null;
                LoginModel createdlog = null;
                var apiUrl1 = "http://192.168.2.13:83/api/UserLogin/" + Request.Cookies["Id"].Value.ToInt();
                Uri url1 = new Uri(apiUrl1);
                WebClient client1 = new WebClient();
                client1.Encoding = System.Text.Encoding.UTF8;

                json1 = client1.DownloadString(url1);
                JavaScriptSerializer ser1 = new JavaScriptSerializer();
                createdlog = ser1.Deserialize<LoginModel>(json1);
                //Kullanıcının en son logid si bulunur
                string json2 = null;
                List<ExecModel> createdlog1 = null;
                var apiUrl2 = "http://192.168.2.13:83/api/UserLogin/exec/" + Request.Cookies["LogId"].Value;
                Uri url2 = new Uri(apiUrl2);
                WebClient client2 = new WebClient();
                client2.Encoding = System.Text.Encoding.UTF8;

                json2 = client2.DownloadString(url2);
                JavaScriptSerializer ser2 = new JavaScriptSerializer();
                createdlog1 = ser2.Deserialize<List<ExecModel>>(json2);

                if (createdlog1[0].SITUATION != false)
                {
                    LoginModel login = new LoginModel();
                    login.LOG_ID = createdlog.LOG_ID;
                    login.LAST_ACTIVITY = 37;
                    var apiUrlnew = "http://192.168.2.13:83/api/UserLogin";

                    var httpClientnew = new System.Net.Http.HttpClient();
                    var requestnew = new HttpRequestMessage(HttpMethod.Put, apiUrlnew)
                    {
                        Content = new StringContent(new JavaScriptSerializer().Serialize(login), Encoding.UTF8, "application/json")
                    };

                    var responsenew = httpClientnew.SendAsync(requestnew);
                }
                else
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Login", "Login");
                }



            }

            if (yetkiKontrol.UPDATE_AUTH == true)
            {
                ViewBag.Update = "Yetkili";
            }

            var yetkiKontrolSatis = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 1).USER_AUTH;
            var yetkiKontrolStok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 2).USER_AUTH;
            var yetkiKontrolUretim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 3).USER_AUTH;
            var yetkiKontrolSatinAlma = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 4).USER_AUTH;
            var yetkiKontrolFinans = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 5).USER_AUTH;
            var yetkiKontrolMuhasebe = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 6).USER_AUTH;
            var yetkiKontrol1 = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 9).USER_AUTH;
            var kullaniciayar = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 10).USER_AUTH;
            var kullaniciyetki = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 11).USER_AUTH;
            var istatistik = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 12).USER_AUTH;
            var yetkiKontrolYonetim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 8).USER_AUTH;
            var yetkiKontrolSube = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 15).USER_AUTH;
            var yetkiKontrolSevkiyat = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 17).USER_AUTH;
            var yetkiKontrolDetayliSip = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 16).USER_AUTH;
            var yetkiKontrolSiparisRaporu = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 18).USER_AUTH;
            var yetkidetaylisatinalma = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 19).USER_AUTH;
            var yetkisaticisiparisi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 20).USER_AUTH;
            var yetkifiyatlistok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 28).USER_AUTH;
            var yetkifiyatsizstok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 29).USER_AUTH;
            var ziyaretkaydi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var musteriraporu = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var musteriraporuozel = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 31).USER_AUTH;
            var ziyaretplani = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var ozelyetki = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 32).USER_AUTH;
            var yonetimstok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 33).USER_AUTH;
            var fiyatyonetim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 34).USER_AUTH;
            var fiyatlistesi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 35).USER_AUTH;
            var kuryetki = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 36).USER_AUTH;
            var uygulamaistatistik = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 37).USER_AUTH;
            if (uygulamaistatistik != true)
            {
                ViewBag.Istatistik = "none";
            }
            else
            {
                ViewBag.Istatistik = "unset";
            }
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
            ViewBag.OzelYetki = ozelyetki;
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
            if ((musteriraporu == true && musteriraporuozel == false) || (Request.Cookies["Id"].Value == "10001" || Request.Cookies["Id"].Value == "10002"))
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
            if (yetkiKontrol1 != true)
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




            return View();
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
        public List<Modules> GetModules(int id)
        {


            var apiUrl = "http://192.168.2.13:83/api/modules";
            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string json = client.DownloadString(url);
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Modules> jsonList = ser.Deserialize<List<Modules>>(json);

            //END

            return jsonList.Where(x => x.INCKEY == id).ToList();
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
        public int GetLink()
        {


            var apiUrl = "http://192.168.2.13:83/api/user/link";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<LinkM> jsonList = ser.Deserialize<List<LinkM>>(json);

            //END

            return jsonList[jsonList.Count-1].ID;
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
        public async Task<ActionResult> SendMail([Bind(Prefix = "Item2")] Mail model)
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
                            string u = "http://localhost:44332/sifre/index?l=U2FsdGVkX1+2HP5R1j2vhaIUaH77YI3wYDqv3RSHko4=&us=U2FsdGVkX1+ro1yUzE2P+eB5lCVq54zigdaUmTCdunI=";
                            string body = "Merhaba Çalışma Arkadaşım,</br></br>Nova, Efece Galvaniz çalışanların çalışmalarını kolaylaştırmak üzere tasarlanmış mobil ve web uygulamalardır.</br></br>Mobil uygulama yüklemek için “İş ve Süreç Geliştirme” departmanı ile iletişime geçebilirsiniz.</br></br>Web uygulamaya;</br></br>Bilgisayar üzerinde → <strong><a href='http://nova.efece.com'>NOVA</a></strong> </br></br>Mobil için → <strong><a href='http://192.168.2.13'>NOVA</a></strong> </br></br>Linklerine tıklayarak ulaşabilirsiniz.</br></br>Efece Galvaniz çatısı altında yapacağın çalışmalarında sana kolaylık sağlayabilmem için uygulamaya giriş yapabileceğin kullanıcı adın ve şifren aşağıdaki gibidir;</br></br><strong>Kullanıcı Adı: " + GetUserByMail(model.MailAdresi[i]).USER_NAME + "</strong></br></br><strong>Şifre: <a href='"+u+"'> Şifreyi değiştirmek için tıklayın.</a></strong></br></br>Kullanıcı bilgileriniz yapacağınız işlemlerde kayıt tutulmasında kullanılacağı için şifrenizi uygulamaya giriş yaptıktan sonra kişisel olmayan bir şifrenizle değiştirip <strong><u>kimseyle</u></strong> paylaşmamanızı öneririz.";


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
                        var apiUrl = "http://192.168.2.13:83/api/user/link";

                        var httpClient = new HttpClient();
                        var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                        var response = await httpClient.SendAsync(request);

                        
                        string u = "http://nova.efece.com/sifre/index?l="+ Encrypt(GetLink().ToString()) + "&us="+ Encrypt(GetUserByMail(model.MailAdresi[i]).USER_NAME);
                        string body = "Merhaba Çalışma Arkadaşım,</br></br>Nova, Efece Galvaniz çalışanların çalışmalarını kolaylaştırmak üzere tasarlanmış mobil ve web uygulamalardır.</br></br>Mobil uygulama yüklemek için “İş ve Süreç Geliştirme” departmanı ile iletişime geçebilirsiniz.</br></br>Web uygulamaya;</br></br>Bilgisayar üzerinde → <strong><a href='http://nova.efece.com'>NOVA</a></strong> </br></br>Mobil için → <strong><a href='http://192.168.2.13'>NOVA</a></strong> </br></br>Linklerine tıklayarak ulaşabilirsiniz.</br></br>Efece Galvaniz çatısı altında yapacağın çalışmalarında sana kolaylık sağlayabilmem için uygulamaya giriş yapabileceğin kullanıcı adın ve şifren aşağıdaki gibidir;</br></br><strong>Kullanıcı Adı: " + GetUserByMail(model.MailAdresi[i]).USER_NAME + "</strong></br></br><strong>Şifre: <a href='" + u + "'>Şifreyi değiştirmek için tıklayın.</a></strong></br></br>Kullanıcı bilgileriniz yapacağınız işlemlerde kayıt tutulmasında kullanılacağı için şifrenizi uygulamaya giriş yaptıktan sonra kişisel olmayan bir şifrenizle değiştirip <strong><u>kimseyle</u></strong> paylaşmamanızı öneririz.";

                        WebMail.Send(model.MailAdresi[i].ToString(), subject, body, "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);
                        //WebMail.Send("ergunozbudakli@efecegalvaniz.com,ergunozbudakli@gmail.com,ugurkonakci@gmail.com,ugurkonakci@efecegalvaniz.com", subject, body, "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);

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
                        string body = model.Icerik;

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

        public async Task<ActionResult> KullaniciUpdate(UserModel user)
        {
            string apiUrl;
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;


            apiUrl = "http://192.168.2.13:83/api/user/update/" + user.USER_NAME;
            user.USER_ID = "0";


            request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(user), Encoding.UTF8, "application/json")
            };

            response = await httpClient.SendAsync(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> YetkiUpdate(YetkiModel user)
        {
            string apiUrl;
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;


            apiUrl = "http://192.168.2.13:83/api/user/auth/"+user.USER_ID+"/"+user.MODULE_INCKEY;


            request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(user), Encoding.UTF8, "application/json")
            };

            response = await httpClient.SendAsync(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> RoleUpdate(RoleModel role)
        {
            string apiUrl;
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;


            apiUrl = "http://192.168.2.13:83/api/rolesauth/" + role.ROLE_ID + "/" + role.MODULE_INCKEY;


            request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(role), Encoding.UTF8, "application/json")
            };

            response = await httpClient.SendAsync(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> ModuleActiveUpdate(ModuleActive mod)
        {
            string apiUrl;
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;
            if (mod.ACTIVE == "False")
            {
                mod.ACTIVE = "0";
            }
            else
            {
                mod.ACTIVE = "1";
            }

            apiUrl = "http://192.168.2.13:83/api/modules/active";


            request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(mod), Encoding.UTF8, "application/json")
            };

            response = await httpClient.SendAsync(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public class ModuleActive
        {
            public int INCKEY { set; get; }
            public string ACTIVE { set; get; }
        }
        public class UserModel
        {
            public string USER_ID { get; set; }


            public string USER_NAME { get; set; }

            public string USER_PASSWORD { get; set; }

            public string USER_FIRSTNAME { get; set; }


            public string USER_LASTNAME { get; set; }


            public string USER_ROLE { get; set; }
            public bool ACTIVE { get; set; }
            public string USER_MAIL { get; set; }
        }
        public class YetkiModel
        {
            public string USER_ID { get; set; }


            public string MODULE_INCKEY { get; set; }

            public bool SELECT_AUTH { get; set; }

            public bool USER_AUTH { get; set; }


            public bool UPDATE_AUTH { get; set; }


            public bool INSERT_AUTH { get; set; }
            public bool DELETE_AUTH { get; set; }
        }
        public class RoleModel
        {
            public string ROLE_ID { get; set; }


            public string MODULE_INCKEY { get; set; }

            public bool SELECT_AUTH { get; set; }

            public bool USER_AUTH { get; set; }


            public bool UPDATE_AUTH { get; set; }


            public bool INSERT_AUTH { get; set; }
            public bool DELETE_AUTH { get; set; }
        }
        public ActionResult MailForm(string password)
        {


            
            Session["password"] =password;
            
            return RedirectToAction("KullaniciAyar");

        }

        public List<User> GetKullaniciApiData()
        {


            string URI = "http://192.168.2.13:83/api/login/login";
            string myParameters = "Email=ergunozbudakli@efecegalvaniz.com&Password=begum142088";
            string HtmlResult = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                HtmlResult = wc.UploadString(URI, myParameters);

            }

            var apiUrl = "http://192.168.2.13:83/api/user";
            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers.Add("Authorization", "Bearer " + HtmlResult);
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
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "abc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "abc123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}