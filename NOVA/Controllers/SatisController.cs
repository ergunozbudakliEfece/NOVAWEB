
﻿using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
//using NetOpenX50;

﻿using Microsoft.AspNetCore.DataProtection.KeyManagement;


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
using NetOpenX50;
using System.Runtime.Serialization;
using System.Drawing.Printing;
using System.Reflection.Metadata;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ServiceStack.Text;
using Microsoft.Graph;
using DocumentFormat.OpenXml.Spreadsheet;
using Workbook = DocumentFormat.OpenXml.Spreadsheet.Workbook;
using DocumentFormat.OpenXml.Packaging;

namespace NOVA.Controllers
{
   
    public class SatisController : Controller
    {
        [HandleError]
        // GET: Satis
        public ActionResult Index()
        {
            var m=GetModules(2);
            if (m[0].ACTIVE != "1")
            {
                return RedirectToAction("Maintenance","Home");
            }

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
            var ziyaretkaydi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var musteriraporu = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var musteriraporuozel = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 31).USER_AUTH;
            var ziyaretplani = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var ozelyetki = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 32).USER_AUTH;
            var yonetimstok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 33).USER_AUTH;
            var fiyatyonetim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 34).USER_AUTH;
            var fiyatlistesi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 35).USER_AUTH;
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
        public ActionResult FiyatYonetimi()
        {
            if (Request.Cookies["Id"] == null)
            {
                FormsAuthentication.SignOut();
                TempData["LOG"] = "ok";
                return RedirectToAction("Login", "Login");
            }
            var yetki = GetYetki();
            var yetkiKontrol1 = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 34);
            if (yetkiKontrol1.SELECT_AUTH != true)
            {
                Session["ModulYetkiMesajı"] = "Modüle yetkiniz bulunmamaktadır";
                return RedirectToAction("Index", "Home");
            }
            
            
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
            var isemrikayit = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 27).USER_AUTH;
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

            return View();
        }
        public ActionResult FiyatListesi()
        {
            if (Request.Cookies["Id"] == null)
            {
                FormsAuthentication.SignOut();
                TempData["LOG"] = "ok";
                return RedirectToAction("Login", "Login");
            }
            var yetki = GetYetki();
            var yetkiKontrol1 = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 35);
            if (yetkiKontrol1.SELECT_AUTH != true)
            {
                Session["ModulYetkiMesajı"] = "Modüle yetkiniz bulunmamaktadır";
                return RedirectToAction("Index", "Home");
            }


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
            var isemrikayit = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 27).USER_AUTH;
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

            return View();
        }
        public ActionResult ZiyaretNotlarim()
        {
            return View();
        }
        public ActionResult ZiyaretPlanlari()
        {
            var yetki = GetYetki();
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

            if (Request.Cookies["Id"] == null)
            {
                FormsAuthentication.SignOut();
                TempData["LOG"] = "ok";
                return RedirectToAction("Login", "Login");
            }
            var yetki = GetYetki();
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30);

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

            var ziyaretkaydi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var musteriraporu = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var musteriraporuozel = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 31).USER_AUTH;
            var ziyaretplani = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var fiyatyonetim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 34).USER_AUTH;
            if (fiyatyonetim != true)
            {
                ViewBag.FiyatYonetim = "none";
            }
            else
            {
                ViewBag.FiyatYonetim = "unset";
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
            ViewBag.Id = Request.Cookies["Id"].Value.ToString();

            ViewBag.Name = Request.Cookies["UserName"].Value.ToString();
            ViewBag.OzelYetki = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 31).SELECT_AUTH;


            return View();
        }
        public ActionResult MusteriRaporu()
        {
            if (Request.Cookies["Id"] == null)
            {
                FormsAuthentication.SignOut();
                TempData["LOG"] = "ok";
                return RedirectToAction("Login", "Login");
            }
            var yetki = GetYetki();
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30);
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
            var ziyaretkaydi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var musteriraporu = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var musteriraporuozel = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 31).USER_AUTH;
            var ziyaretplani = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
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
            ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            ViewBag.Name = Request.Cookies["UserName"].Value.ToString();


            return View();
        }
        public ActionResult MusteriRaporuOzel()
        {
            if (Request.Cookies["Id"] == null)
            {
                FormsAuthentication.SignOut();
                TempData["LOG"] = "ok";
                return RedirectToAction("Login", "Login");
            }
            var yetki = GetYetki();
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 31);
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
            var ziyaretkaydi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var musteriraporu = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var musteriraporuozel = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 31).USER_AUTH;
            if (musteriraporuozel != true)
            {
                ViewBag.DisplayMusteriOzel = "none";
            }
            else
            {
                ViewBag.DisplayMusteriOzel = "unset";
            }
            if (musteriraporu != true)
            {
                ViewBag.DisplayMusteriRaporu = "none";
            }
            else
            {
                ViewBag.DisplayMusteriRaporu = "unset";
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
            ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            ViewBag.Name = Request.Cookies["UserName"].Value.ToString();


            return View();
        }
        public JsonResult GetMusteriUrun()
        {
            string URI = "http://192.168.2.13:83/api/login/login";
            string myParameters = "Email=ergunozbudakli@efecegalvaniz.com&Password=begum142088";
            string HtmlResult = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                HtmlResult = wc.UploadString(URI, myParameters);

            }

            var apiUrl = "http://192.168.2.13:83/api/musteri/urun";
            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers.Add("Authorization", "Bearer " + HtmlResult);
            string json = client.DownloadString(url);
            //END

            //JSON Parse START
           

            //END

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRandevu()
        {
            string URI = "http://192.168.2.13:83/api/login/login";
            string myParameters = "Email=ergunozbudakli@efecegalvaniz.com&Password=begum142088";
            string HtmlResult = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                HtmlResult = wc.UploadString(URI, myParameters);

            }

            var apiUrl = "http://192.168.2.13:83/api/musteri/randevu";
            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers.Add("Authorization", "Bearer " + HtmlResult);
            string json = client.DownloadString(url);
            

            //END

            return Json(json, JsonRequestBehavior.AllowGet);
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

            return jsonList.Where(x=>x.INCKEY==id).ToList();
        }
        public JsonResult GetUser()
        {
           

            var apiUrl = "http://192.168.2.13:83/api/user";
            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string json = client.DownloadString(url);


            //END

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> RandevuEkleGuncelle(MusteriRandevu m)
        {

            MusteriRandevu musteri = new MusteriRandevu();
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;
            string apiUrl = "http://192.168.2.13:83/api/musteri/randevu";

            if (m.INCKEY == 0)
            {
                musteri.MUSTERI_ID = m.MUSTERI_ID;
                musteri.KAYIT_YAPAN_KULLANICI_ID = Request.Cookies["Id"].Value.ToInt();
                musteri.RANDEVU_NOTU = m.RANDEVU_NOTU;
                musteri.PLANLANAN_TARIH = DateTime.Parse(m.PLANLANAN_TARIH.ToString()).ToLocalTime();
                request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(musteri), Encoding.UTF8, "application/json")
                };
            }
            else
            {
                musteri.INCKEY = m.INCKEY;
                musteri.MUSTERI_ID = m.MUSTERI_ID;
                musteri.DEGISIKLIK_YAPAN_KULLANICI_ID = Request.Cookies["Id"].Value.ToInt();
                if (m.RANDEVU_NOTU != null)
                {
                    musteri.RANDEVU_NOTU = m.RANDEVU_NOTU;
                }
                else
                {
                    
                    musteri.RANDEVU_NOTU = "";
                }
                if (m.GERCEKLESEN_TARIH != null)
                {if(m.GERCEKLESEN_TARIH== DateTime.Parse("1970-12-12"))
                    {
                        musteri.GERCEKLESEN_TARIH = null;
                    }
                    else
                    {
                        musteri.GERCEKLESEN_TARIH = DateTime.Parse(m.GERCEKLESEN_TARIH.ToString()).ToLocalTime();
                    }
                    
                }
               
                musteri.PLANLANAN_TARIH = DateTime.Parse(m.PLANLANAN_TARIH.ToString()).ToLocalTime();
                
                request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(musteri), Encoding.UTF8, "application/json")
                };
            }
         
            response = await httpClient.SendAsync(request);
            

            return Json(request, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> PostData(Musteri m)
        {
            Musteri musteri = new Musteri();
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;
            var decities = JsonConvert.DeserializeObject<List<DistrictsModel>>((string)GetDistricts().Data);
            
            if (m.MUSTERI_MAHALLE != null)
            {
                var demah = JsonConvert.DeserializeObject<List<MahalleModel>>((string)GetMahalleler(m.MUSTERI_ILCE).Data);
                musteri.MUSTERI_MAHALLE = demah.Where(x => x.MAHID == m.MUSTERI_MAHALLE.ToInt()).ToList()[0].MAHALLEADI;
            }
           


            string apiUrl = "http://192.168.2.13:83/api/musteri/";

            musteri.MUSTERI_ADI = m.MUSTERI_ADI;
            musteri.MUSTERI_IL = m.MUSTERI_IL;
            musteri.MUSTERI_ILCE = decities.Where(x => x.ILCEID == m.MUSTERI_ILCE.ToInt()).ToList()[0].ILCEADI;
           
            musteri.MUSTERI_ADRES = m.MUSTERI_ADRES;
            musteri.FIRMA_YETKILISI = m.FIRMA_YETKILISI;

            musteri.MUSTERI_TEL1 = m.MUSTERI_TEL1;
            musteri.MUSTERI_TEL2 = m.MUSTERI_TEL2;
            musteri.MUSTERI_MAIL = m.MUSTERI_MAIL;
            musteri.MUSTERI_SEKTOR = m.MUSTERI_SEKTOR;

            musteri.MUSTERI_SEKTOR_DIGER = m.MUSTERI_SEKTOR_DIGER;

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

        public async Task<ActionResult> MusteriGuncelle(Musteri m)
        {
            Musteri musteri = new Musteri();
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;
            var decities = JsonConvert.DeserializeObject<List<DistrictsModel>>((string)GetDistricts().Data);

            var demah = JsonConvert.DeserializeObject<List<MahalleModel>>((string)GetMahalleler(m.MUSTERI_ILCE).Data);
            string apiUrl = "http://192.168.2.13:83/api/musteri";


            musteri.MUSTERI_IL = m.MUSTERI_IL;
            musteri.MUSTERI_ILCE = decities.Where(x => x.ILCEID == m.MUSTERI_ILCE.ToInt()).ToList()[0].ILCEADI;
            musteri.MUSTERI_MAHALLE = demah.Where(x => x.MAHID == m.MUSTERI_MAHALLE.ToInt()).ToList()[0].MAHALLEADI;
            musteri.MUSTERI_ADRES = m.MUSTERI_ADRES;
            musteri.FIRMA_YETKILISI = m.FIRMA_YETKILISI;
            musteri.MUSTERI_TEL1 = m.MUSTERI_TEL1;
            musteri.MUSTERI_TEL2 = m.MUSTERI_TEL2;
            musteri.MUSTERI_ID = m.MUSTERI_ID;
            musteri.MUSTERI_MAIL = m.MUSTERI_MAIL;
            musteri.MUSTERI_SEKTOR = m.MUSTERI_SEKTOR;
            musteri.MUSTERI_SEKTOR_DIGER = m.MUSTERI_SEKTOR_DIGER;
            musteri.MUSTERI_NITELIK = m.MUSTERI_NITELIK;
            musteri.MUSTERI_NITELIK_DIGER = m.MUSTERI_NITELIK_DIGER;
            musteri.MUSTERI_NOTU = m.MUSTERI_NOTU;
            musteri.PLASIYER = Request.Cookies["UserName"].Value; 
            musteri.DUZELTME_YAPAN_KULLANICI = Request.Cookies["UserName"].Value;

            request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(musteri), Encoding.UTF8, "application/json")
            };

            response = await httpClient.SendAsync(request);


            return Json(request, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> UrunGonder(MusteriUrunModel m)
        {
            MusteriUrunModel musteri = new MusteriUrunModel();
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;



            string apiUrl = "http://192.168.2.13:83/api/musteri/urun";
            musteri.URUN_ID = m.URUN_ID;
            musteri.URUN_GRUBU = m.URUN_GRUBU;
            musteri.OLCU_BIRIMI = m.OLCU_BIRIMI;
            musteri.MUSTERI_ID = m.MUSTERI_ID;
            musteri.YILLIK_KULLANIM = m.YILLIK_KULLANIM;
            musteri.KAYIT_YAPAN_KULLANICI= Request.Cookies["UserName"].Value;
            try
            {
                request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(musteri), Encoding.UTF8, "application/json")
                };

                response = await httpClient.SendAsync(request);
            }
            catch (Exception)
            {

                return null;
            }
            

            return Json(request, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> UrunGuncelle(MusteriUrunModel m)
        {
            MusteriUrunModel musteri = new MusteriUrunModel();
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;



            string apiUrl = "http://192.168.2.13:83/api/musteri/guncelle";
            musteri.URUN_ID = m.URUN_ID;
            musteri.URUN_GRUBU = m.URUN_GRUBU;
            musteri.OLCU_BIRIMI = m.OLCU_BIRIMI;
            musteri.MUSTERI_ID = m.MUSTERI_ID;
            musteri.YILLIK_KULLANIM = m.YILLIK_KULLANIM;
            musteri.DEGISIKLIK_YAPAN_KULLANICI = Request.Cookies["UserName"].Value;

            request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(musteri), Encoding.UTF8, "application/json")
            };

            response = await httpClient.SendAsync(request);

            ViewBag.Urunler = GetMusteriUrun();
           
            return Json(request, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> RandevuGerceklesen(int id)
        {
            MusteriUrunModel musteri = new MusteriUrunModel();
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;



            string apiUrl = "http://192.168.2.13:83/api/musteri/randevu/"+id;



            request = new HttpRequestMessage(HttpMethod.Put, apiUrl);

            response = await httpClient.SendAsync(request);

            return Json(request, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> UrunSil(MusteriUrunModel m)
        {
            MusteriUrunModel musteri = new MusteriUrunModel();
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;



            string apiUrl = "http://192.168.2.13:83/api/musteri/sil/"+m.MUSTERI_ID+ "/"+ m.URUN_GRUBU;

           

            request = new HttpRequestMessage(HttpMethod.Delete, apiUrl);

            response = await httpClient.SendAsync(request);
            return Json(request, JsonRequestBehavior.AllowGet); 
        }
        public async Task<JsonResult> RandevuSil(MusteriRandevu m)
        {
            MusteriUrunModel musteri = new MusteriUrunModel();
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;



            string apiUrl = "http://192.168.2.13:83/api/musteri/randevu";



            request = new HttpRequestMessage(HttpMethod.Delete, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(m), Encoding.UTF8, "application/json")
            };

            response = await httpClient.SendAsync(request);
            return Json(request, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> MusteriSil(int id)
        {
            MusteriUrunModel musteri = new MusteriUrunModel();
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;



            string apiUrl = "http://192.168.2.13:83/api/musteri/sil/"+id;



            request = new HttpRequestMessage(HttpMethod.Delete, apiUrl);

            response = await httpClient.SendAsync(request);
            return Json(request, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> Sirala(List<FiyatSiraModel> m)
        {
            
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;
            
            string apiUrl = "http://192.168.2.13:83/api/fiyat";

           

            request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(m), Encoding.UTF8, "application/json")
            };

            response = await httpClient.SendAsync(request);


            return null;
        }
        public ActionResult DetayliSiparis()
        {
            if (Request.Cookies["Id"] == null)
            {
                FormsAuthentication.SignOut();
                TempData["LOG"] = "ok";
                return RedirectToAction("Login", "Login");
            }
            var m = GetModules(16);
            if (m[0].ACTIVE != "1")
            {
                return RedirectToAction("Maintenance", "Home");
            }
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
            var ziyaretkaydi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var yonetimstok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 33).USER_AUTH;
            var fiyatyonetim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 34).USER_AUTH;
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
            ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            
            



            return View();
        }
        public ActionResult HammaddeListesi()
        {
            if (Request.Cookies["Id"] == null)
            {
                FormsAuthentication.SignOut();
                TempData["LOG"] = "ok";
                return RedirectToAction("Login", "Login");
            }
            var m = GetModules(28);
            if (m[0].ACTIVE != "1")
            {
                return RedirectToAction("Maintenance", "Home");
            }
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
            var ziyaretkaydi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var yonetimstok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 33).USER_AUTH;
            var fiyatyonetim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 34).USER_AUTH;
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
            ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            FileInfo fi = new FileInfo("\\\\192.168.2.3\\ortak\\EFECE_LISTELER\\NOVA\\NOVA_EFECE_HAMMADDE_LISTESI.xlsx");
            var created = fi.CreationTime;
            var lastmodified = fi.LastWriteTime;
            ViewBag.LastModified = lastmodified.ToString();
            return View();
        }
        public ActionResult FiyatsizHammaddeListesi()
        {
            if (Request.Cookies["Id"] == null)
            {
                FormsAuthentication.SignOut();
                TempData["LOG"] = "ok";
                return RedirectToAction("Login", "Login");
            }
            var m = GetModules(29);
            if (m[0].ACTIVE != "1")
            {
                return RedirectToAction("Maintenance", "Home");
            }
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
            var fiyatyonetim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 34).USER_AUTH;
            if (fiyatyonetim != true)
            {
                ViewBag.FiyatYonetim = "none";
            }
            else
            {
                ViewBag.FiyatYonetim = "unset";
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
            var ustkalem = GetSiparisUstKAlem();


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
                                    x = x.Concat(ustkalem.Where(t => t.TESLIM_YERI == item && t.CARI_DOVIZ_TIPI == item2 && t.PLASIYER_ADI == item3&&t.URETILECEKMI=="E")).ToList();
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

                                x = x.Concat(ustkalem.Where(t => t.TESLIM_YERI == item && t.CARI_DOVIZ_TIPI == item2 && t.URETILECEKMI == "E")).ToList();
                                z.Add(item);
                                m.Add(item2);


                            }
                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in teslimArray)
                        {


                            x = x.Concat(ustkalem.Where(t => t.TESLIM_YERI == item && t.URETILECEKMI == "E")).ToList();
                            z.Add(item);



                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI == null)
                    {



                        x = ustkalem.Where(t=>t.URETILECEKMI == "E").ToList();




                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in cariArray)
                        {


                            x = x.Concat(ustkalem.Where(t => t.CARI_DOVIZ_TIPI == item && t.URETILECEKMI == "E")).ToList();

                            m.Add(item);

                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {


                            x = x.Concat(ustkalem.Where(t => t.PLASIYER_ADI == item && t.URETILECEKMI == "E")).ToList();
                            y.Add(item);


                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                x = x.Concat(ustkalem.Where(t => t.PLASIYER_ADI == item && t.CARI_DOVIZ_TIPI == item2 && t.URETILECEKMI == "E")).ToList();
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
                                x = x.Concat(ustkalem.Where(t => t.TESLIM_YERI == item && t.PLASIYER_ADI == item2 && t.URETILECEKMI == "E")).ToList();
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
                                    x = x.Concat(ustkalem.Where(t => t.TESLIM_YERI == item && t.CARI_DOVIZ_TIPI == item2 && t.PLASIYER_ADI == item3)).ToList();
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

                                x = x.Concat(ustkalem.Where(t => t.TESLIM_YERI == item && t.CARI_DOVIZ_TIPI == item2)).ToList();
                                z.Add(item);
                                m.Add(item2);


                            }
                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in teslimArray)
                        {


                            x = x.Concat(ustkalem.Where(t => t.TESLIM_YERI == item)).ToList();
                            z.Add(item);



                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI == null)
                    {



                        x = ustkalem.ToList();




                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in cariArray)
                        {


                            x = x.Concat(ustkalem.Where(t => t.CARI_DOVIZ_TIPI == item)).ToList();

                            m.Add(item);

                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {


                            x = x.Concat(ustkalem.Where(t => t.PLASIYER_ADI == item)).ToList();
                            y.Add(item);


                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                x = x.Concat(ustkalem.Where(t => t.PLASIYER_ADI == item && t.CARI_DOVIZ_TIPI == item2)).ToList();
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
                                x = x.Concat(ustkalem.Where(t => t.TESLIM_YERI == item && t.PLASIYER_ADI == item2)).ToList();
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
                Session["Filter"] = x.Where(t=>t.TARIH>=filter.BASLANGIC_TARIH).ToList(); 
                list= x.Where(t => t.TARIH >= filter.BASLANGIC_TARIH).ToList();
            }
            else if(filter.BASLANGIC_TARIH == null && filter.BITIS_TARIH != null)
            {
                Session["Filter"] = x.Where(t => t.TARIH <= filter.BITIS_TARIH).ToList(); 
                list = x.Where(t => t.TARIH <= filter.BITIS_TARIH).ToList();
            }
            else if(filter.BASLANGIC_TARIH != null && filter.BITIS_TARIH != null)
            {
                Session["Filter"] = x.Where(t => t.TARIH <= filter.BITIS_TARIH && t.TARIH>=filter.BASLANGIC_TARIH).ToList(); 
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
        public ActionResult Test()
        {
            return View();
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

        public JsonResult GetCities()
        {
            


            var apiUrl = "http://192.168.2.13:83/api/location/cities";
            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            

            return Json(json, JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult GetMahalleler(string e)
        {


            var apiUrl = "http://192.168.2.13:83/api/location/mahalle/"+e;
            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string json = client.DownloadString(url);
           
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetActiveUsers()

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

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMusteriler()

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

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDistricts()
        {
            


            var apiUrl = "http://192.168.2.13:83/api/location/districts";
            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            

            //END

            return Json(json, JsonRequestBehavior.AllowGet);

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
        public List<FiyatModel> GetFiyatlar()
        {


            var apiUrl = "http://192.168.2.13:83/api/fiyat/exec/000000000000001";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;
            List<FiyatModel> jsonList = ser.Deserialize<List<FiyatModel>>(json);

            //END

            return jsonList;
        }
        public class FiyatModel
        {
           public string FK_ACIKLAMA { get; set; }
            public string FIYATDOVIZTIPI { get; set; }
            public string FIYAT2 { get; set; }
            public string FIYAT3 { get; set; }
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
                System.IO.MemoryStream str = null;
                if (stoklar.Count > 600)
                {
                    str = DataToExcel(ToDataTable(stoklar.Take(600).ToList()));
                }
                else
                {
                    str = DataToExcel(ToDataTable(stoklar));
                }

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
        public ActionResult MailFiyatlist()
        {

            if (Request.Cookies["Mail"] != null)
            {
                string subject = "NOVA | Güncel Fiyat Listesi";

                string body = "";
                var fiyatlar = GetFiyatlar();
                System.IO.MemoryStream str = null;
                System.IO.MemoryStream str1 = null;
                if (fiyatlar.Count > 600)
                {
                   
                    str1 = exportpdf(ToDataTable(fiyatlar.Take(600).ToList()));
                }
                else
                {
                   
                    str1 = exportpdf(ToDataTable(fiyatlar));
                }

                //Attachment at = new Attachment(str, "FiyatListesi_" + DateTime.Now.Date.ToShortDateString() + ".xls");
                Attachment at1 = new Attachment(str1, "FiyatListesi_" +DateTime.Now.Date.ToShortDateString()+".pdf");
                MailMessage mail = new MailMessage();
                //mail.To.Add(Request.Cookies["Mail"].Value);
                mail.To.Add("ergunozbudakli@gmail.com");
                mail.From = new MailAddress("sistem@efecegalvaniz.com");
                mail.Body = body;
                mail.Subject = subject;

                mail.Attachments.Add(at1);
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




            return RedirectToAction("FiyatYonetimi");

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
                dgGrid.RenderControl(hw);
                Response.Buffer = true;
                Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.ContentEncoding = System.Text.Encoding.Default;

            }
            System.IO.MemoryStream s = new MemoryStream();
            System.Text.Encoding Enc = System.Text.Encoding.Default;
           
            byte[] mBArray = Enc.GetBytes(tw.ToString());
            s = new MemoryStream(mBArray, true);
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
        private MemoryStream exportpdf(DataTable dtEmployee)
        {

            // creating document object  
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            iTextSharp.text.Rectangle rec = new iTextSharp.text.Rectangle(PageSize.A4);
            rec.BackgroundColor = new BaseColor(System.Drawing.Color.Olive);
            iTextSharp.text.Document doc = new iTextSharp.text.Document(rec);
            doc.SetPageSize(iTextSharp.text.PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();

            //Creating paragraph for header  
            BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fntHead = new iTextSharp.text.Font(bfntHead, 16, 1, iTextSharp.text.BaseColor.BLUE);
            Paragraph prgHeading = new Paragraph();
            prgHeading.Alignment = Element.ALIGN_LEFT;
            prgHeading.Add(new Chunk("Fiyat Listesi "+DateTime.Now.ToLocalTime(), fntHead));
            doc.Add(prgHeading);

            //Adding paragraph for report generated by  
            Paragraph prgGeneratedBY = new Paragraph();
            BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fntAuthor = new iTextSharp.text.Font(btnAuthor, 8, 2, iTextSharp.text.BaseColor.BLUE);
            prgGeneratedBY.Alignment = Element.ALIGN_RIGHT;
            //prgGeneratedBY.Add(new Chunk("Report Generated by : ASPArticles", fntAuthor));  
            //prgGeneratedBY.Add(new Chunk("\nGenerated Date : " + DateTime.Now.ToShortDateString(), fntAuthor));  
            doc.Add(prgGeneratedBY);

            //Adding a line  
            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, iTextSharp.text.BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            doc.Add(p);

            //Adding line break  
            doc.Add(new Chunk("\n", fntHead));

            //Adding  PdfPTable  
            PdfPTable table = new PdfPTable(dtEmployee.Columns.Count);

            for (int i = 0; i < dtEmployee.Columns.Count; i++)
            {
                string cellText = Server.HtmlDecode(dtEmployee.Columns[i].ColumnName);
                PdfPCell cell = new PdfPCell();
                cell.Phrase = new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"))));
                cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#C8C8C8"));
                //cell.Phrase = new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 10, 1, new BaseColor(grdStudent.HeaderStyle.ForeColor)));  
                //cell.BackgroundColor = new BaseColor(grdStudent.HeaderStyle.BackColor);  
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.PaddingBottom = 5;
                table.AddCell(cell);
            }

            //writing table Data  
            for (int i = 0; i < dtEmployee.Rows.Count; i++)
            {
                for (int j = 0; j < dtEmployee.Columns.Count; j++)
                {
                    table.AddCell(dtEmployee.Rows[i][j].ToString());
                }
            }

            doc.Add(table);
            doc.Close();

            byte[] result = ms.ToArray();
            System.IO.MemoryStream s = new MemoryStream();
            s = new MemoryStream(result, false);
            return s;

        }
       
    }

    public class ActiveUsers
    {
        public string USER_NAME;
    }

    public class DistrictsModel
    {

        public int ILCEID { get; set; }
        public string ILCEADI { get; set; }
        public string SEHIRADI { get; set; }

    }

    public class CityModel
    {
        public string CITYNAME { get; set; }
        public int ID { get; set; }
    }

    public class MahalleModel
    {
        public string MAHALLEADI { get; set; }
        public int MAHID { get; set; }
    }
    public class FiyatSiraModel
    {
        public int SIRA_NO { set; get; }
        public string FIYATKODU { set; get; }
    }
}