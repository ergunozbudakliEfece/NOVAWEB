using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Kiota.Abstractions;
using NOVA.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using static NOVA.Controllers.LoginController;

namespace NOVA.Controllers
{
  
    public class IKController : Controller
    {
        // GET: IK
        public ActionResult PersonelBilgi()
        {
            var m = GetModules(13);
            if (m[0].ACTIVE != "1")
            {
                return RedirectToAction("Maintenance", "Home");
            }

            var x = GetPersonelById(Request.Cookies["Id"].Value.ToInt());
            if (x.Count != 0)
            {
                ViewBag.PersonelBilgi = x[0];
            }
            
            ViewBag.Page = 1;
            ViewBag.Name = Session["Name"];
            ViewBag.RoleName = Request.Cookies["RoleName"].Value.ToString();
            ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            var yetki = GetYetki(Request.Cookies["Id"].Value.ToInt());
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 13);
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
                        login.LAST_ACTIVITY = 13;
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
            var yonetimstok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 33).USER_AUTH;
            var fiyatyonetim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 34).USER_AUTH;
            var fiyatlistesi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 35).USER_AUTH;
            var kuryetki = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 36).USER_AUTH;
            var uygulamaistatistik = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 37).USER_AUTH;
            var puantaj = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 38).USER_AUTH;
            var teklif = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 39).USER_AUTH;
            if (teklif != true)
            {
                ViewBag.DisplayTeklif = "none";
            }
            else
            {
                ViewBag.DisplayTeklif = "unset";
            }
            if (puantaj != true)
            {
                ViewBag.Puantaj = "none";
            }
            else
            {
                ViewBag.Puantaj = "unset";
            }
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
        public List<User> GetYetki(int id)
        {
            var apiUrl = "http://192.168.2.13:83/api/user/auth/" + id;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;
            List<User> jsonList = ser.Deserialize<List<User>>(json);
            return jsonList;
        }
        public ActionResult PersonelBilgiYonetim()
        {
            var m = GetModules(14);
            if (m[0].ACTIVE != "1")
            {
                return RedirectToAction("Maintenance", "Home");
            }
            if (Session["Select"] != null)
            {
                ViewBag.Select = Session["Select"];
                Session["Select"] = null;
            }
            else
            {
                ViewBag.Select = null;
            }
            if (Session["Ekle"] != null)
            {
                ViewBag.Ekle = "ekle";
            }
            else
            {
                ViewBag.Ekle = null;
            }
            ViewBag.Page = 2;
            ViewBag.Name = Session["Name"];
            ViewBag.RoleName = Request.Cookies["RoleName"].Value.ToString();
            ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            var yetki = GetYetki(Request.Cookies["Id"].Value.ToInt());
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 14);
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
                        login.LAST_ACTIVITY = 14;
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
            var uygulamaistatistik = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 37).USER_AUTH;
            var puantaj = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 38).USER_AUTH;
            var teklif = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 39).USER_AUTH;
            if (teklif != true)
            {
                ViewBag.DisplayTeklif = "none";
            }
            else
            {
                ViewBag.DisplayTeklif = "unset";
            }
            if (puantaj != true)
            {
                ViewBag.Puantaj = "none";
            }
            else
            {
                ViewBag.Puantaj = "unset";
            }
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
            ViewBag.Personels = GetPersonels().OrderBy(t=>t.ISIM);
            if (Session["Personel"] != null)
            {
                ViewBag.Personel = Session["Personel"];
            }
            return View();
        }
        public ActionResult PersonelSubeYonetim()
        {
            var m = GetModules(15);
            if (m[0].ACTIVE != "1")
            {
                return RedirectToAction("Maintenance", "Home");
            }

            var sube = GetPersonelById(Request.Cookies["Id"].Value.ToInt())[0].SUBE;
            if (Session["Select"] != null)
            {
                ViewBag.Select = Session["Select"];
                Session["Select"] = null;
            }
            else
            {
                ViewBag.Select = null;
            }
            if (Session["Ekle"] != null)
            {
                ViewBag.Ekle = "ekle";
            }
            else
            {
                ViewBag.Ekle = null;
            }
            ViewBag.Page = 3;
            ViewBag.Name = Session["Name"];
            ViewBag.RoleName = Request.Cookies["RoleName"].Value.ToString();
            ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            var yetki = GetYetki(Request.Cookies["Id"].Value.ToInt());
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 15);
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
                        login.LAST_ACTIVITY = 15;
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
            var uygulamaistatistik = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 37).USER_AUTH;
            var puantaj = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 38).USER_AUTH;
            var teklif = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 39).USER_AUTH;
            if (teklif != true)
            {
                ViewBag.DisplayTeklif = "none";
            }
            else
            {
                ViewBag.DisplayTeklif = "unset";
            }
            if (puantaj != true)
            {
                ViewBag.Puantaj = "none";
            }
            else
            {
                ViewBag.Puantaj = "unset";
            }
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
            ViewBag.Personels = GetPersonelsBySube(sube);
            if (Session["Personel"] != null)
            {
                ViewBag.Personel = Session["Personel"];
            }
            return View();
        }
        public ActionResult PersonelPuantaj()
        {
            var yetki = GetYetki(Request.Cookies["Id"].Value.ToInt());
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 38);
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
                    login.LAST_ACTIVITY = 38;
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
            var uygulamaistatistik = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 37).USER_AUTH;
            var puantaj = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 38).USER_AUTH;
            var teklif = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 39).USER_AUTH;
            if (teklif != true)
            {
                ViewBag.DisplayTeklif = "none";
            }
            else
            {
                ViewBag.DisplayTeklif = "unset";
            }
            if (puantaj != true)
            {
                ViewBag.Puantaj = "none";
            }
            else
            {
                ViewBag.Puantaj = "unset";
            }
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
        [HttpPost]
        public async Task<ActionResult> Personel(Personel personel)
        {
            try
            {   
                if (personel.USER_ID == 0 && Session["Ekle"]==null)
                {
                    personel.USER_ID = Request.Cookies["Id"].Value.ToInt();
                }else if(personel.USER_ID == 0 && Session["Ekle"] != null)
                {
                    personel.USER_ID = Request.Cookies["Id"].Value.ToInt();
                    personel.KAYIT_TARIH = DateTime.Now.AddHours(3);
                    var apiUrl1 = "http://192.168.2.13:83/api/personel/";


                    var httpClient1 = new HttpClient();
                    var request1 = new HttpRequestMessage(HttpMethod.Post, apiUrl1)
                    {
                        Content = new StringContent(new JavaScriptSerializer().Serialize(personel), Encoding.UTF8, "application/json")
                    };

                    var response1 = await httpClient1.SendAsync(request1);

                    string apiResponse1 = await response1.Content.ReadAsStringAsync();
                    Session["Alert"] = "Personel başarılı bir şekilde eklenmiştir.";
                    return RedirectToAction("PersonelBilgi");
                }
               
                    personel.KAYIT_TARIH = DateTime.Now.AddHours(3);
                    var apiUrl = "http://192.168.2.13:83/api/personel/" + personel.USER_ID;


                    var httpClient = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
                    {
                        Content = new StringContent(new JavaScriptSerializer().Serialize(personel), Encoding.UTF8, "application/json")
                    };

                    var response = await httpClient.SendAsync(request);

                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Session["Alert"] = "Personel başarılı bir şekilde güncellenmiştir.";
                
               
            }
            catch (Exception)
            {

                Session["Alert"] = "Personel kaydı başarısız!";
            }
            
            return RedirectToAction("PersonelBilgi");
        }
        [HttpPost]
        public async Task<ActionResult> PersonelYonetim(Personel personel)
        {
            try
            {
                if (personel.USER_ID == 0 && Session["Ekle"] == null)
                {
                    personel.USER_ID = Request.Cookies["Id"].Value.ToInt();
                }
                else if (personel.USER_ID == 0 && Session["Ekle"] != null)
                {
                    personel.USER_ID = Request.Cookies["Id"].Value.ToInt();
                    personel.KAYIT_TARIH = DateTime.Now.AddHours(3);
                    var apiUrl1 = "http://192.168.2.13:83/api/personel/";


                    var httpClient1 = new HttpClient();
                    var request1 = new HttpRequestMessage(HttpMethod.Post, apiUrl1)
                    {
                        Content = new StringContent(new JavaScriptSerializer().Serialize(personel), Encoding.UTF8, "application/json")
                    };

                    var response1 = await httpClient1.SendAsync(request1);

                    string apiResponse1 = await response1.Content.ReadAsStringAsync();
                    Session["Alert"] = "Personel başarılı bir şekilde eklenmiştir.";
                    return RedirectToAction("PersonelBilgi");
                }

                personel.KAYIT_TARIH = DateTime.Now.AddHours(3);
                var apiUrl = "http://192.168.2.13:83/api/personel/" + personel.USER_ID;


                var httpClient = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(personel), Encoding.UTF8, "application/json")
                };

                var response = await httpClient.SendAsync(request);

                string apiResponse = await response.Content.ReadAsStringAsync();
                Session["Alert"] = "Personel başarılı bir şekilde güncellenmiştir.";


            }
            catch (Exception)
            {

                Session["Alert"] = "Personel kaydı başarısız!";
            }
            Session["Select"] = personel.USER_ID;
            Session["Personel"] = GetPersonelById(personel.USER_ID)[0];
            return RedirectToAction("PersonelBilgiYonetim");
        }
        [HttpPost]
        public async Task<ActionResult> PersonelSubeYonetim(Personel personel)
        {
            try
            {
                if (personel.USER_ID == 0 && Session["Ekle"] == null)
                {
                    personel.USER_ID = Request.Cookies["Id"].Value.ToInt();
                }
                else if (personel.USER_ID == 0 && Session["Ekle"] != null)
                {
                    personel.USER_ID = Request.Cookies["Id"].Value.ToInt();
                    personel.KAYIT_TARIH = DateTime.Now.AddHours(3);
                    var apiUrl1 = "http://192.168.2.13:83/api/personel/";


                    var httpClient1 = new HttpClient();
                    var request1 = new HttpRequestMessage(HttpMethod.Post, apiUrl1)
                    {
                        Content = new StringContent(new JavaScriptSerializer().Serialize(personel), Encoding.UTF8, "application/json")
                    };

                    var response1 = await httpClient1.SendAsync(request1);

                    string apiResponse1 = await response1.Content.ReadAsStringAsync();
                    Session["Alert"] = "Personel başarılı bir şekilde eklenmiştir.";
                    return RedirectToAction("PersonelBilgi");
                }

                personel.KAYIT_TARIH = DateTime.Now.AddHours(3);
                var apiUrl = "http://192.168.2.13:83/api/personel/" + personel.USER_ID;


                var httpClient = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(personel), Encoding.UTF8, "application/json")
                };

                var response = await httpClient.SendAsync(request);

                string apiResponse = await response.Content.ReadAsStringAsync();
                Session["Alert"] = "Personel başarılı bir şekilde güncellenmiştir.";


            }
            catch (Exception)
            {

                Session["Alert"] = "Personel kaydı başarısız!";
            }
            Session["Select"] = personel.USER_ID;
            Session["Personel"] = GetPersonelById(personel.USER_ID)[0];
            return RedirectToAction("PersonelSubeYonetim");
        }
        [HttpPost]
        public async Task<ActionResult> PostData(PersonalOffdateModel personel)
        {
           


                var apiUrl1 = "http://192.168.2.13:83/api/attendance";


                var httpClient1 = new HttpClient();
                var request1 = new HttpRequestMessage(HttpMethod.Post, apiUrl1)
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(personel), Encoding.UTF8, "application/json")
                };

                var response1 = await httpClient1.SendAsync(request1);

                string apiResponse1 = await response1.Content.ReadAsStringAsync();
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> PostDataMesai(PersonalMesModel personel)
        {
            if (personel.START_INCKEY!=0)
            {
                PersonalMesaiModel personel1 = new PersonalMesaiModel();
                personel1.INCKEY = personel.START_INCKEY;
                personel1.DATE = personel.START_DATE;
                personel1.USER_ID = personel.USER_ID;
                personel1.DEV_ID = 10003;

                var apiUrl1 = "http://192.168.2.13:83/api/attendance/mesai";


                var httpClient1 = new HttpClient();
                var request1 = new HttpRequestMessage(HttpMethod.Put, apiUrl1)
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(personel1), Encoding.UTF8, "application/json")
                };

                var response1 = await httpClient1.SendAsync(request1);

            }
            else
            {
                if (personel.START_DATE.ToString().Split(' ')[1] != "00:00:00")
                {
                    var tarih = personel.START_DATE.ToString().Split('.')[2].Substring(0, 4) + "-" + personel.START_DATE.ToString().Split('.')[1] + "-" + personel.START_DATE.ToString().Split('.')[0] + " " + personel.START_DATE.ToString().Split(' ')[1];
                    var apiUrl1 = "http://192.168.2.13:83/api/attendance/manual/" + personel.USER_ID + "/" + tarih + "/S";


                    var httpClient1 = new HttpClient();
                    var request1 = new HttpRequestMessage(HttpMethod.Get, apiUrl1);
                    var response1 = await httpClient1.SendAsync(request1); 
                }
            }
            if (personel.END_INCKEY != 0)
            {
                PersonalMesaiModel personel2 = new PersonalMesaiModel();
                personel2.INCKEY = personel.END_INCKEY;
                personel2.DATE = personel.END_DATE;
                personel2.USER_ID = personel.USER_ID;
                personel2.DEV_ID = 10003;

                var apiUrl2 = "http://192.168.2.13:83/api/attendance/mesai";


                var httpClient2 = new HttpClient();
                var request2 = new HttpRequestMessage(HttpMethod.Put, apiUrl2)
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(personel2), Encoding.UTF8, "application/json")
                };

                var response2 = await httpClient2.SendAsync(request2);
                string apiResponse2 = await response2.Content.ReadAsStringAsync();
            }
            else
            {   if(personel.END_DATE.ToString().Split(' ')[1] != "00:00:00")
                {
                    var tarih = personel.END_DATE.ToString().Split('.')[2].Substring(0, 4) + "-" + personel.END_DATE.ToString().Split('.')[1] + "-" + personel.END_DATE.ToString().Split('.')[0] + " " + personel.END_DATE.ToString().Split(' ')[1];
                    var apiUrl1 = "http://192.168.2.13:83/api/attendance/manual/" + personel.USER_ID + "/" + tarih + "/E"+"/"+ personel.START_INCKEY;
                    var httpClient1 = new HttpClient();
                    var request1 = new HttpRequestMessage(HttpMethod.Get, apiUrl1);
                    var response1 = await httpClient1.SendAsync(request1);
                }
                
            }


            return Json(false, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> PutData(PersonalOffdateModel personel)
        {



            var apiUrl1 = "http://192.168.2.13:83/api/attendance";


            var httpClient1 = new HttpClient();
            if (personel.START_DATE != null)
            {
                personel.START_DATE = personel.START_DATE.Value.AddHours(3);
            }
            if (personel.END_DATE != null)
            {
                personel.END_DATE = personel.END_DATE.Value.AddHours(3);
            }
            
            var request1 = new HttpRequestMessage(HttpMethod.Put, apiUrl1)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(personel), Encoding.UTF8, "application/json")
            };

            var response1 = await httpClient1.SendAsync(request1);

            string apiResponse1 = await response1.Content.ReadAsStringAsync();
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public class PersonalMesModel
        {
            public int? START_INCKEY { get; set; }
            public int? END_INCKEY { get; set; }
            public int USER_ID { get; set; }
            public string DATE { get; set; }
            public DateTime? START_DATE { get; set; }
            public DateTime? END_DATE { get; set; }
            public int OFFDAY_CODE { get; set; }
            public int CONFIRM_CODE { get; set; }

        }
        public class PersonalMesaiModel
        {
            [Key]
            public int? INCKEY { get; set; }
            public int USER_ID { get; set; }
            public DateTime? DATE { get; set; }
            public string STARTEND { get; set; }
            public int? DEV_ID { get; set; }
            public int? START_INCKEY { get; set; }
        }
        public class PersonalOffdateModel
        {
            public int? INCKEY { get; set; }
            public int USER_ID { get; set; }
            public string DATE { get; set; }
            public DateTime? START_DATE { get; set; }
            public DateTime? END_DATE { get; set; }
            public int OFFDAY_CODE { get; set; }
            public int CONFIRM_CODE { get; set; }

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
            
            

            return RedirectToAction("PersonelBilgi");
        }
        [HttpPost]
        public ActionResult UploadFileYonetim(HttpPostedFileBase file, int id)
        {
            if (id != 0)
            {
                if (file.ContentLength > 0)
                {
                    try
                    {
                        var filename = id + ".png";
                        var path = Path.Combine(Server.MapPath("/assets/img/avatars/"), filename);
                        file.SaveAs(path);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            else
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
            }

            return RedirectToAction("PersonelBilgiYonetim");
        }
        [HttpPost]
        public ActionResult UploadFileSubeYonetim(HttpPostedFileBase file, int id)
        {
            if (id != 0)
            {
                if (file.ContentLength > 0)
                {
                    try
                    {
                        var filename = id + ".png";
                        var path = Path.Combine(Server.MapPath("/assets/img/avatars/"), filename);
                        file.SaveAs(path);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            else
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
            }

            return RedirectToAction("PersonelSubeYonetim");
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
        
        public List<Personel> GetPersonels()
        {


            var apiUrl = "http://192.168.2.13:83/api/personel";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Personel> jsonList = ser.Deserialize<List<Personel>>(json);

            //END

            return jsonList;
        }
        public List<Personel> GetPersonelsBySube(string sube)
        {


            var apiUrl = "http://192.168.2.13:83/api/personel/sube:"+sube;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Personel> jsonList = ser.Deserialize<List<Personel>>(json);

            //END

            return jsonList;
        }

        public ActionResult Get(int id)
        {
            if (id == 0)
            {
                
                return RedirectToAction("PersonelBilgiYonetim");
            }
            Session["Personel"] = GetPersonelById(id)[0];
            Session["Select"] = id;
            
            return RedirectToAction("PersonelBilgiYonetim");
        }
        public ActionResult GetSube(int id)
        {
            if (id == 0)
            {

                return RedirectToAction("PersonelSubeYonetim");
            }
            Session["Personel"] = GetPersonelById(id)[0];
            Session["Select"] = id;

            return RedirectToAction("PersonelSubeYonetim");
        }

        public List<Personel> GetPersonelById(int id)
        {
            var apiUrl = "http://192.168.2.13:83/api/personel/id:"+id;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string json = client.DownloadString(url);
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Personel> jsonList = ser.Deserialize<List<Personel>>(json);

            //END

            return jsonList;

        }  
    }
}