using NOVA.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static NOVA.Controllers.LoginController;
using System.Web.Security;
using System.Text;

namespace NOVA.Controllers
{
    public class YonetimController : Controller
    {
        // GET: Yonetim
        public ActionResult Index()
        {
            ViewBag.RoleName = Request.Cookies["RoleName"].Value;
            ViewBag.Id = Request.Cookies["Id"].Value;
            var yetki = GetYetki();
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 8);
            if (yetkiKontrol.SELECT_AUTH != true)
            {
                Session["ModulYetkiMesajı"] = "Modüle yetkiniz bulunmamaktadır";

                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Request.Cookies["Id"].Value.ToInt() != 10050 && Request.Cookies["Id"].Value.ToInt() != 10000)
                { //Kullanıcının en son logid si bulunur
                    string json1 = null;
                    LoginModel createdlog = null;
                    var apiUrl1 = "http://192.168.2.13:83/api/UserLogin/" + Request.Cookies["Id"].Value.ToInt();
                    Uri url1 = new Uri(apiUrl1);
                    WebClient client1 = new WebClient();
                    client1.Encoding = System.Text.Encoding.UTF8;

                    json1 = client1.DownloadString(url1);
                    JavaScriptSerializer ser1 = new JavaScriptSerializer();
                    createdlog = ser1.Deserialize<LoginModel>(json1);


                    HttpCookie logid = new HttpCookie("LogId", createdlog.LOG_ID.ToString());
                    Response.Cookies.Add(logid);
                    //Kullanıcının en son logid si bulunur

                    string json2 = null;
                    List<ExecModel> createdlog1 = null;
                    var apiUrl2 = "http://192.168.2.13:83/api/UserLogin/exec/" + createdlog.LOG_ID;
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
                        login.LAST_ACTIVITY = 8;
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
            }
            if (yetkiKontrol.UPDATE_AUTH == true)
            {
                ViewBag.Yetki = "yetkili";
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
            var yetkiKontrolIstatistik = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 8).USER_AUTH;
            var yetkidetaylisatinalma = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 19).USER_AUTH;
            var yetkisaticisiparisi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 20).USER_AUTH;
            var yetkifiyatlistok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 28).USER_AUTH;
            var yetkifiyatsizstok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 29).USER_AUTH;
            var ziyaretkaydi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var fiyatlistesi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 35).USER_AUTH;
            if (fiyatlistesi != true)
            {
                ViewBag.DisplayFiyatListesi = "none";
            }
            else
            {
                ViewBag.DisplayFiyatListesi = "unset";
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
        public ActionResult Stok()
        {
            var yetki = GetYetki();
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 33);
            if (yetkiKontrol.SELECT_AUTH != true)
            {
                Session["ModulYetkiMesajı"] = "Modüle yetkiniz bulunmamaktadır";

                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Request.Cookies["Id"].Value.ToInt() != 10050 && Request.Cookies["Id"].Value.ToInt() != 10000)
                { //Kullanıcının en son logid si bulunur
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
                        login.LAST_ACTIVITY = 33;
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
            ViewBag.OzelYetki = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 31).UPDATE_AUTH;
            return View();
        }
    }
}