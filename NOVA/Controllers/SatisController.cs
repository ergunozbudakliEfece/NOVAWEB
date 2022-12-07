using Microsoft.AspNetCore.DataProtection.KeyManagement;
using NetOpenX50;
using NOVA.Models;
using ServiceStack;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NOVA.Controllers
{
   
    public class SatisController : Controller
    {
        [HandleError]
        // GET: Satis
        public ActionResult Index()
        {
            if (Request.Cookies["Id"] == null)
            {
                FormsAuthentication.SignOut();
                TempData["LOG"] = "ok";
                return RedirectToAction("Login", "Login");
            }
            if (GetSession(Request.Cookies["Id"].Value.ToInt())[0].LOG_DATETIME != null)
            {
                var xm = DateTime.Parse(GetSession(Request.Cookies["Id"].Value.ToInt())[0].LOG_DATETIME);
                var xn = DateTime.Parse(Request.Cookies["SignIn"].Value).AddHours(-3);
                if ((xm != xn) && GetSession(Request.Cookies["Id"].Value.ToInt())[0].ACTIVITY_TYPE == "login")
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Login", "Login");
                }
            }
            ViewBag.Page = 1;
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

            // Stop Caching in Firefox
            Response.Cache.SetNoStore();
            var yetki = GetYetki();
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 2);
            if (yetkiKontrol.SELECT_AUTH != true)
            {
                Session["ModulYetkiMesajı"] = "Modüle yetkiniz bulunmamaktadır";
                return RedirectToAction("Index", "Home");
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
        public ActionResult ZiyaretNotlarim()
        {
            return View();
        }
        public ActionResult Test()
        {
            return View();
        }
       public class StokListeLocal
        {
            public string STOK_KODU { get; set; }
            public string STOK_ADI { get; set; }
            public string GRUP_KODU { get; set; }
            public string GRUP_ISIM { get; set; }
            public string OLCU_BIRIMI { get; set; }
            public string OLCU_BIRIMI2 { get; set; }
            public decimal BAKIYE { get; set; }
            public decimal AD_BAKIYE { get; set; }
            public decimal BEKLEYEN_SIPARIS { get; set; }
            public decimal BEK_SIP_ADET { get; set; }
            public decimal SATISA_HAZIR { get; set; }
            public decimal SAT_HZR_ADET { get; set; }
            public decimal BEKL_IE_MIKTAR { get; set; }
            public decimal BEKL_IE_AD { get; set; }
            public decimal FAB_STOK_MIK { get; set; }
            public decimal FAB_STOK_AD { get; set; }
            public decimal SATISFIYAT1 { get; set; }
            public decimal SATISFIYAT2 { get; set; }
            public decimal SATISFIYAT3 { get; set; }
            public decimal SATISFIYAT4 { get; set; }
            public decimal ALISFIYAT1 { get; set; }
            public decimal ALISFIYAT2 { get; set; }
            public decimal ALISFIYAT3 { get; set; }
            public decimal ALISFIYAT4 { get; set; }
          
            
            
          
            
            
        }
     
        public ActionResult SetViewBag(List<StokListeLocal> data)
        {
            ViewBag.Stoks1 = data;
            return View("Index");
        }
        public ActionResult ZiyaretKaydiListesi()
        {
            var yetki = GetYetki();
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 28);
            if (yetkiKontrol.SELECT_AUTH != true)
            {
                Session["ModulYetkiMesajı"] = "Modüle yetkiniz bulunmamaktadır";
                return RedirectToAction("Index", "Home");
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
            ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            ViewBag.Cities = GetCities();
            ViewBag.Districts = GetDistricts();
            ViewBag.Actives = GetActiveUsers();
            ViewBag.Musteriler = GetMusteriler().Where(x => x.PLASIYER == Request.Cookies["UserName"].Value);
            return View();
        }
       
        public async Task<ActionResult> PostData(Musteri m)
        {
            Musteri musteri = new Musteri();
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;
           

            
            string apiUrl = "http://192.168.2.13:83/api/musteri/";

            musteri.MUSTERI_ADI = m.MUSTERI_ADI;
            musteri.MUSTERI_IL = GetCities().Where(x => x.ID == m.MUSTERI_IL.ToInt()).ToList()[0].CITYNAME;
            musteri.MUSTERI_ILCE = m.MUSTERI_ILCE;
            musteri.MUSTERI_TEL1 = m.MUSTERI_TEL1;
            musteri.MUSTERI_TEL2 = m.MUSTERI_TEL2;
            musteri.MUSTERI_MAIL = m.MUSTERI_MAIL;
            musteri.MUSTERI_SEKTOR = m.MUSTERI_SEKTOR;
            musteri.MUSTERI_NITELIK = m.MUSTERI_NITELIK;
            musteri.MUSTERI_NOTU = m.MUSTERI_NOTU;
            musteri.PLASIYER = m.PLASIYER;
            musteri.KAYIT_TARIHI = DateTime.Now;
            musteri.KAYIT_YAPAN_KULLANICI = Request.Cookies["UserName"].Value;
            
            request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(musteri), Encoding.UTF8, "application/json")
            };

            response = await httpClient.SendAsync(request);


            return null;
        }
        public ActionResult DetayliSiparis()
        {
            
            if (Session["Filter"] != null || Session["Filter1"]!=null)
            {
                if (Session["Filter1"] == null)
                {
                    Session["Filter1"] = "filter1";
                   
                        Session["Filter2"] = Session["Filter"];
                    
                    
                    Session["ToplamUsd2"] = Session["ToplamUsd"];
                    Session["ToplamEur2"] = Session["ToplamEur"];
                    Session["ToplamGbp2"] = Session["ToplamGbp"];
                    Session["ToplamTryFilter2"] = Session["ToplamTryFilter"];
                }
                else
                {
                    Session["Filter1"] = null;
                }
                ViewBag.Toplam = Session["Toplam"];

                ViewBag.ToplamTryFilter = Session["ToplamTryFilter2"];
                ViewBag.ToplamEur=Session["ToplamEur2"];
                ViewBag.ToplamUsd= Session["ToplamUsd2"]; 
                ViewBag.ToplamGbp = Session["ToplamGbp2"];
                ViewBag.UstKalem = Session["Filter2"];
                ViewBag.FilterPlasiyer = Session["FilterPlasiyer"];
                ViewBag.FilterTeslim = Session["FilterTeslim"];
                ViewBag.FilterDoviz = Session["FilterDoviz"];
                ViewBag.FilterUretilecekmi = Session["FilterUretilecekmi"];
                ViewBag.AltKalem = GetSiparisApiData();
                
                
                return View();
            }
            else
            {
                ViewBag.UstKalem = GetSiparisUstKAlem();
                ViewBag.AltKalem = GetSiparisApiData();
            }
            List<USTKALEMMODEL> list = GetSiparisUstKAlem();

            decimal? toplamtry = 0;
            decimal? toplamtryfilter = 0;
            decimal? toplameur = 0;
            decimal? toplamusd = 0;
            decimal? toplamgbp = 0;
            foreach (var item in list)
            {
                toplamtry = toplamtry + item.GENELTOPLAM;
                
                    if (item.DOV_TIP == "USD")
                    {
                        toplamusd = toplamusd + item.DOV_GENELTOPLAM;
                    }
                    if (item.DOV_TIP == "EUR")
                    {
                        toplameur = toplameur + item.DOV_GENELTOPLAM;
                    }
                    if (item.DOV_TIP == "GBP")
                    {
                        toplamgbp = toplamgbp + item.DOV_GENELTOPLAM;
                    }
                    if (item.DOV_TIP == "TRY")
                    {
                        toplamtryfilter = toplamtryfilter + item.GENELTOPLAM;
                    }
                
               

            }
            ViewBag.Toplam = toplamtry;

            ViewBag.ToplamTryFilter = toplamtryfilter;
            ViewBag.ToplamEur = toplameur;
            ViewBag.ToplamUsd = toplamusd;
            ViewBag.ToplamGbp = toplamgbp;
            Session["Toplam"] = toplamtry;
            Session["ToplamTryFilter"] = toplamtryfilter;
            Session["ToplamEur"] = toplameur;
            Session["ToplamUsd"] = toplamusd;
            Session["ToplamGbp"] = toplamgbp;


            ViewBag.Page = 4;
            if (GetSession(Request.Cookies["Id"].Value.ToInt())[0].LOG_DATETIME != null)
            {
                var xm = DateTime.Parse(GetSession(Request.Cookies["Id"].Value.ToInt())[0].LOG_DATETIME);
                var xn = DateTime.Parse(Request.Cookies["SignIn"].Value).AddHours(-3);
                if ((xm != xn) && GetSession(Request.Cookies["Id"].Value.ToInt())[0].ACTIVITY_TYPE == "login")
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Login", "Login");
                }
            }
          
            var yetki = GetYetki();
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 16);
            if (yetkiKontrol.SELECT_AUTH != true)
            {
                Session["ModulYetkiMesajı"] = "Modüle yetkiniz bulunmamaktadır";
                return RedirectToAction("Index", "Home");
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
            ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            
            



            return View();
        }
        public ActionResult HammaddeListesi()
        {
            var yetki = GetYetki();
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 28);
            if (yetkiKontrol.SELECT_AUTH != true)
            {
                Session["ModulYetkiMesajı"] = "Modüle yetkiniz bulunmamaktadır";
                return RedirectToAction("Index", "Home");
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
            ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            FileInfo fi = new FileInfo("\\\\192.168.2.3\\ortak\\EFECE_LISTELER\\NOVA\\NOVA_EFECE_HAMMADDE_LISTESI.xlsx");
            var created = fi.CreationTime;
            var lastmodified = fi.LastWriteTime;
            ViewBag.LastModified = lastmodified.ToString();
            return View();
        }
        public ActionResult FiyatsizHammaddeListesi()
        {
            var yetki = GetYetki();
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 29);
            if (yetkiKontrol.SELECT_AUTH != true)
            {
                Session["ModulYetkiMesajı"] = "Modüle yetkiniz bulunmamaktadır";
                return RedirectToAction("Index", "Home");
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
            ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            FileInfo fi = new FileInfo("\\\\192.168.2.3\\ortak\\EFECE_LISTELER\\NOVA\\NOVA_EFECE_HAMMADDE_LISTESI.xlsx");
            var created = fi.CreationTime;
            var lastmodified = fi.LastWriteTime;
            ViewBag.LastModified = lastmodified.ToString();
            return View();
        }
        public ActionResult Filter(FilterModel filter)
        {
            Session["Toplam"] = null;
            Session["ToplamTryFilter"] = null;
            Session["ToplamEur"] = null;
            Session["ToplamUsd"] = null;
            Session["ToplamGbp"] = null;
            List<USTKALEMMODEL> x=new List<USTKALEMMODEL>();
            List<string> y = new List<string>();
            List<string> z = new List<string>();
            List<string> m = new List<string>();
            bool u = false;
            string teslim = "";
            string cari = "";
            string plasiyer = "";
            if (filter.TESLIM_YERI!=null)
            teslim = filter.TESLIM_YERI[0];
            if (filter.CARI_DOVIZ_TIPI != null)
                cari = filter.CARI_DOVIZ_TIPI[0];
            if (filter.PLASIYER_ADI != null)
                plasiyer = filter.PLASIYER_ADI[0];
            var teslimArray=teslim.Split(',');
            var cariArray = cari.Split(',');
            var plasiyerArray = plasiyer.Split(',');



            if (filter != null)
            {
                if (filter.URETILECEKMI)
                {   u= true;
                    if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in teslimArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                foreach (var item3 in plasiyerArray)
                                {
                                    x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == item && t.CARI_DOVIZ_TIPI == item2 && t.PLASIYER_ADI == item3&&t.URETILECEKMI=="E")).ToList();
                                    z.Add(item);
                                    m.Add(item2);
                                    y.Add(item3);
                                }
                            }
                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in teslimArray)
                        {
                            foreach (var item2 in cariArray)
                            {

                                x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == item && t.CARI_DOVIZ_TIPI == item2 && t.URETILECEKMI == "E")).ToList();
                                z.Add(item);
                                m.Add(item2);


                            }
                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in teslimArray)
                        {


                            x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == item && t.URETILECEKMI == "E")).ToList();
                            z.Add(item);



                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI == null)
                    {



                        x = GetSiparisUstKAlem().Where(t=>t.URETILECEKMI == "E").ToList();




                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in cariArray)
                        {


                            x = x.Concat(GetSiparisUstKAlem().Where(t => t.CARI_DOVIZ_TIPI == item && t.URETILECEKMI == "E")).ToList();

                            m.Add(item);

                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {


                            x = x.Concat(GetSiparisUstKAlem().Where(t => t.PLASIYER_ADI == item && t.URETILECEKMI == "E")).ToList();
                            y.Add(item);


                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                x = x.Concat(GetSiparisUstKAlem().Where(t => t.PLASIYER_ADI == item && t.CARI_DOVIZ_TIPI == item2 && t.URETILECEKMI == "E")).ToList();
                                m.Add(item2);
                                y.Add(item);
                            }





                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in teslimArray)
                        {
                            foreach (var item2 in plasiyerArray)
                            {
                                x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == item && t.PLASIYER_ADI == item2 && t.URETILECEKMI == "E")).ToList();
                                y.Add(item2);
                                z.Add(item);
                            }





                        }
                    }
                }
                else
                {
                    if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in teslimArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                foreach (var item3 in plasiyerArray)
                                {
                                    x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == item && t.CARI_DOVIZ_TIPI == item2 && t.PLASIYER_ADI == item3)).ToList();
                                    z.Add(item);
                                    m.Add(item2);
                                    y.Add(item3);
                                }
                            }
                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in teslimArray)
                        {
                            foreach (var item2 in cariArray)
                            {

                                x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == item && t.CARI_DOVIZ_TIPI == item2)).ToList();
                                z.Add(item);
                                m.Add(item2);


                            }
                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in teslimArray)
                        {


                            x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == item)).ToList();
                            z.Add(item);



                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI == null)
                    {



                        x = GetSiparisUstKAlem().ToList();




                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in cariArray)
                        {


                            x = x.Concat(GetSiparisUstKAlem().Where(t => t.CARI_DOVIZ_TIPI == item)).ToList();

                            m.Add(item);

                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {


                            x = x.Concat(GetSiparisUstKAlem().Where(t => t.PLASIYER_ADI == item)).ToList();
                            y.Add(item);


                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                x = x.Concat(GetSiparisUstKAlem().Where(t => t.PLASIYER_ADI == item && t.CARI_DOVIZ_TIPI == item2)).ToList();
                                m.Add(item2);
                                y.Add(item);
                            }





                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in teslimArray)
                        {
                            foreach (var item2 in plasiyerArray)
                            {
                                x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == item && t.PLASIYER_ADI == item2)).ToList();
                                y.Add(item2);
                                z.Add(item);
                            }





                        }
                    }
                }
               

            }
            ViewBag.AltKalem = GetSiparisApiData();
            var list = x;
            if (filter.BASLANGIC_TARIH != null && filter.BITIS_TARIH == null)
            {
                Session["Filter"] = x.Where(t=>t.TARIH>=filter.BASLANGIC_TARIH);
                list= x.Where(t => t.TARIH >= filter.BASLANGIC_TARIH).ToList();
            }
            else if(filter.BASLANGIC_TARIH == null && filter.BITIS_TARIH != null)
            {
                Session["Filter"] = x.Where(t => t.TARIH <= filter.BITIS_TARIH);
                list = x.Where(t => t.TARIH <= filter.BITIS_TARIH).ToList();
            }
            else if(filter.BASLANGIC_TARIH != null && filter.BITIS_TARIH != null)
            {
                Session["Filter"] = x.Where(t => t.TARIH <= filter.BITIS_TARIH && t.TARIH>=filter.BASLANGIC_TARIH);
                list = x.Where(t => t.TARIH <= filter.BITIS_TARIH && t.TARIH >= filter.BASLANGIC_TARIH).ToList();
            }
            else
            {
                Session["Filter"] = x;
            }

            if (filter.SSIP_CHECK=="E")
            {
                Session["Filter"] = ((List<USTKALEMMODEL>)Session["Filter"]).Where(t=>t.SSIP_CHECK=="E");
                list =list.Where(t=>t.SSIP_CHECK=="E").ToList();
            }

            decimal? toplamtry = 0;
            decimal? toplamtryfilter = 0;
            decimal? toplameur = 0;
            decimal? toplamusd = 0;
            decimal? toplamgbp = 0;
            foreach (var item in list)
            {
                toplamtry = toplamtry + item.GENELTOPLAM;
                if (item.DOV_TIP != null)
                {
                    if (item.DOV_TIP == "USD")
                    {
                        toplamusd = toplamusd + item.DOV_GENELTOPLAM;
                    }
                    if (item.DOV_TIP == "EUR")
                    {
                        toplameur=toplameur+ item.DOV_GENELTOPLAM;
                    }
                    if (item.DOV_TIP == "GBP")
                    {
                        toplamgbp = toplamgbp + item.DOV_GENELTOPLAM;
                    }
                    if(item.DOV_TIP == "TRY")
                    {
                        toplamtryfilter=toplamtryfilter+ item.GENELTOPLAM;
                    }
                }
                
            }
            Session["Toplam"] = toplamtry;
            Session["ToplamTryFilter"] = toplamtryfilter;
            Session["ToplamEur"] = toplameur;
            Session["ToplamUsd"] = toplamusd;
            Session["ToplamGbp"] = toplamgbp;
            ViewBag.FilterPlasiyer =y;
            ViewBag.FilterTeslim = z;
            ViewBag.FilterDoviz = m;
            ViewBag.FilterUretilecekmi = u;
            return null;
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
        public List<User> GetYetki(int id,int inckey)
        {
            var apiUrl = "http://192.168.2.13:83/api/userwithroles/"+id+"/"+inckey;

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

      
        
        public ActionResult Search(string name)
        {   
            return RedirectToAction("Index");
        }
        public List<StokListe> GetStoksApiData()
        {


            var apiUrl = "http://192.168.2.13:83/api/stokliste/0005";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<StokListe> jsonList = ser.Deserialize<List<StokListe>>(json);

            //END

            return jsonList;
        }
        public List<CityModel> GetCities()
        {
            string URI = "http://192.168.2.13:83/api/login/login";
            string myParameters = "Email=ergunozbudakli@efecegalvaniz.com&Password=begum142088";
            string HtmlResult = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                HtmlResult = wc.UploadString(URI, myParameters);
               
            }

            var apiUrl = "http://192.168.2.13:83/api/location/cities";
            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers.Add("Authorization", "Bearer "+HtmlResult);
            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;
            List<CityModel> jsonList = ser.Deserialize<List<CityModel>>(json);

            //END

            return jsonList;
        }
        public List<ActiveUsers> GetActiveUsers()
        {
            string URI = "http://192.168.2.13:83/api/login/login";
            string myParameters = "Email=ergunozbudakli@efecegalvaniz.com&Password=begum142088";
            string HtmlResult = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                HtmlResult = wc.UploadString(URI, myParameters);

            }

            var apiUrl = "http://192.168.2.13:83/api/user/active";
            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers.Add("Authorization", "Bearer " + HtmlResult);
            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;
            List<ActiveUsers> jsonList = ser.Deserialize<List<ActiveUsers>>(json);

            //END

            return jsonList;
        }
        public List<Musteri> GetMusteriler()
        {
            string URI = "http://192.168.2.13:83/api/login/login";
            string myParameters = "Email=ergunozbudakli@efecegalvaniz.com&Password=begum142088";
            string HtmlResult = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                HtmlResult = wc.UploadString(URI, myParameters);

            }

            var apiUrl = "http://192.168.2.13:83/api/musteri";
            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers.Add("Authorization", "Bearer " + HtmlResult);
            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;
            List<Musteri> jsonList = ser.Deserialize<List<Musteri>>(json);

            //END

            return jsonList;
        }
        public List<DistrictsModel> GetDistricts()
        {
            string URI = "http://192.168.2.13:83/api/login/login";
            string myParameters = "Email=ergunozbudakli@efecegalvaniz.com&Password=begum142088";
            string HtmlResult = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                HtmlResult = wc.UploadString(URI, myParameters);

            }

            var apiUrl = "http://192.168.2.13:83/api/location/districts";
            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers.Add("Authorization", "Bearer " + HtmlResult);
            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;
            List<DistrictsModel> jsonList = ser.Deserialize<List<DistrictsModel>>(json);

            //END

            return jsonList;
        }
        public List<SiparisModel> GetSiparisApiData()
        {


            var apiUrl = "http://192.168.2.13:83/api/detaylisip";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;
            List<SiparisModel> jsonList = ser.Deserialize<List<SiparisModel>>(json);

            //END

            return jsonList;
        }
        public List<SiparisModelExcel> GetSiparisApiDataExcel()
        {


            var apiUrl = "http://192.168.2.13:83/api/detaylisip";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;
            List<SiparisModelExcel> jsonList = ser.Deserialize<List<SiparisModelExcel>>(json);

            //END

            return jsonList;
        }
        public List<USTKALEMMODEL> GetSiparisUstKAlem()
        {


            var apiUrl = "http://192.168.2.13:83/api/detaysizsip";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<USTKALEMMODEL> jsonList = ser.Deserialize<List<USTKALEMMODEL>>(json);
            
            //END

            return jsonList;
        }
        public List<StokListe> GetStoksApiDataByGroup(string s)
        {
            
            if (s == null)
            {
                Session["GetStoks"] = "0";
            }
            else
            {
                Session["GetStoks"] = s;
            }

            var apiUrl = "http://192.168.2.13:83/api/stokliste/" + s;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;
            List<StokListe> jsonList = ser.Deserialize<List<StokListe>>(json);

            //END

            return jsonList;
        }
        public ActionResult GetStoksApiExcelDetails(string grupkodu)
        {

           

            var apiUrl = "http://192.168.2.13:83/api/stokliste/"+grupkodu;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<StokListeExcel> jsonList = ser.Deserialize<List<StokListeExcel>>(json);
            if (Request.Cookies["Mail"] != null)
            {

                Session["Info"] = "Başarılı";


                string subject = "NOVA | Stok Bilgisi";

                string body = "";
                var stoklar = jsonList;
                System.IO.MemoryStream str = DataToExcel(ToDataTable(stoklar));
                Attachment at = new Attachment(str, "StokBilgisi_" + DateTime.Now + ".xls");
                //WebMail.SmtpServer = "192.168.2.13";
                //WebMail.Send(Request.Cookies["Mail"].Value, subject, body, "sistem@efecegalvaniz.com", null,null, true, null, null, null, null, "High", null);
                MailMessage mail = new MailMessage();
                mail.To.Add(Request.Cookies["Mail"].Value);
                mail.From = new MailAddress("sistem@efecegalvaniz.com");
                mail.Body = body;
                mail.Subject = subject;

                mail.Attachments.Add(at);

                mail.IsBodyHtml = true;

                SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = "192.168.2.13";
                smtp.UseDefaultCredentials = true;
                smtp.Send(mail);

            }
            else
            {
                Session["AlertLogin"] = "Bu mail adresi sistemimizde kayıtlı değil!";
            }




            return RedirectToAction("Index");
           
        }
        public List<StokListeExcel> GetStoksApiExcel()
        {



            var apiUrl = "http://192.168.2.13:83/api/stokliste/";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<StokListeExcel> jsonList = ser.Deserialize<List<StokListeExcel>>(json);

            //END

            return jsonList;
        }
        /*public ActionResult GetStoks(string id)
        {
           Session["Group"] = GetStoksApiDataByGroup(id);
            
           

            return RedirectToAction("Index");
        }*/
        public ActionResult GetStoks(string id)
        {
            
            Session["Group"] = GetStoksApiDataByGroup(id);



            return null;
        }
        
        public ActionResult FavAdd(decimal id)
        {
            _ = AddFav(id);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> AddFav(decimal stokKodu)
        {   Fav fav=new Fav();
            fav.USER_ID= Request.Cookies["Id"].Value.ToInt();
            fav.FAVORITE_STOKS = stokKodu.ToString();

            var apiUrl = "http://192.168.2.13:83/api/fav";

            var httpClient = new HttpClient();
            var req = new JavaScriptSerializer().Serialize(fav);
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
            {
                Content = new StringContent(req, Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);

            string apiResponse = await response.Content.ReadAsStringAsync();


            return null;

        }
        public ActionResult Maintenance()
        {
            return View("maintenance");
        }
       
       
        public ActionResult MailControl()
        {

            if (Request.Cookies["Mail"] != null)
            {
                
                Session["Info"] = "Başarılı";


                string subject = "NOVA | Stok Bilgisi";

                string body = "";
                var stoklar = GetStoksApiExcel();
                System.IO.MemoryStream str = DataToExcel(ToDataTable(stoklar));
                Attachment at = new Attachment(str, "StokBilgisi_"+DateTime.Now+".xls");
                //WebMail.SmtpServer = "192.168.2.13";
                //WebMail.Send(Request.Cookies["Mail"].Value, subject, body, "sistem@efecegalvaniz.com", null,null, true, null, null, null, null, "High", null);
                MailMessage mail = new MailMessage();
                mail.To.Add(Request.Cookies["Mail"].Value);
                mail.From = new MailAddress("sistem@efecegalvaniz.com");
                mail.Body = body;
                mail.Subject = subject;
                
                mail.Attachments.Add(at);

                mail.IsBodyHtml = true;

                SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = "192.168.2.13";
                smtp.UseDefaultCredentials = true;
                smtp.Send(mail);

            }
            else
            {
                Session["AlertLogin"] = "Bu mail adresi sistemimizde kayıtlı değil!";
            }



            
            return RedirectToAction("Index");

        }
        public ActionResult MailDetayliSip()
        {

            if (Request.Cookies["Mail"] != null)
            {

                Session["Info"] = "Başarılı";


                string subject = "NOVA | Detaylı Sipariş Bilgisi";

                string body = "";
                var stoklar = GetSiparisApiDataExcel();
                System.IO.MemoryStream str = DataToExcel(ToDataTable(stoklar));
                Attachment at = new Attachment(str, "DetayliSiparisBilgisi_" + DateTime.Now + ".xls");
                //WebMail.SmtpServer = "192.168.2.13";
                //WebMail.Send(Request.Cookies["Mail"].Value, subject, body, "sistem@efecegalvaniz.com", null,null, true, null, null, null, null, "High", null);
                MailMessage mail = new MailMessage();
                mail.To.Add(Request.Cookies["Mail"].Value);
                mail.From = new MailAddress("sistem@efecegalvaniz.com");
                mail.Body = body;
                mail.Subject = subject;

                mail.Attachments.Add(at);

                mail.IsBodyHtml = true;

                SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = "192.168.2.13";
                smtp.UseDefaultCredentials = true;
                smtp.Send(mail);

            }
            else
            {
                Session["AlertLogin"] = "Bu mail adresi sistemimizde kayıtlı değil!";
            }




            return RedirectToAction("DetayliSiparis");

        }
        public System.IO.MemoryStream DataToExcel(DataTable dt)
        {
            //StreamWriter sw = new StreamWriter();
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            if (dt.Rows.Count > 0)
            {

                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();
                dgGrid.HeaderStyle.Font.Bold = true;
                dgGrid.HeaderStyle.ToCsvField();
                
                 //Get the HTML for the control.
                 dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.
                //Response.ContentType = application/vnd.ms-excel;
                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.ContentEncoding = System.Text.Encoding.Default;

            }
            System.IO.MemoryStream s = new MemoryStream();
            System.Text.Encoding Enc = System.Text.Encoding.Default;
            byte[] mBArray = Enc.GetBytes(tw.ToString());
            s = new MemoryStream(mBArray, false);

            return s;
        }
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }

    public class ActiveUsers
    {
        public string USER_NAME;
    }

    public class DistrictsModel
    {
        public int SEHIRID { get; set; }
        public string ILCENAME { get; set; }
    }

    public class CityModel
    {
        public string CITYNAME { get; set; }
        public int ID { get; set; }
    }
}