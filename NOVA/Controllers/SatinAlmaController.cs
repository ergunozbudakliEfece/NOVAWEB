﻿using NOVA.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace NOVA.Controllers
{
    public class SatinAlmaController : Controller
    {
        // GET: SatinAlma
        public ActionResult SatinAlmaRaporu()
        {
            if (Request.Cookies["Id"] == null)
            {
                FormsAuthentication.SignOut();
                TempData["LOG"] = "ok";
                return RedirectToAction("Login", "Login");
            }
            if (Session["Filter"] != null || Session["Filter1"] != null || Session["AltKalem"]!=null)
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
                ViewBag.Toplam1 = Session["Toplam1"];

                ViewBag.ToplamTryFilter = Session["ToplamTryFilter2"];
                ViewBag.ToplamEur = Session["ToplamEur2"];
                ViewBag.ToplamUsd = Session["ToplamUsd2"];
                ViewBag.ToplamGbp = Session["ToplamGbp2"];
                ViewBag.UstKalem = Session["Filter2"];
                ViewBag.FilterPlasiyer = Session["FilterPlasiyer"];
                ViewBag.FilterTeslim = Session["FilterTeslim"];
                ViewBag.FilterDoviz = Session["FilterDoviz"];
                ViewBag.FilterUretilecekmi = Session["FilterUretilecekmi"];
                ViewBag.ToplamTryFilter1 = Session["ToplamTryFilter1"];
                ViewBag.ToplamEur1 = Session["ToplamEur1"];
                ViewBag.ToplamUsd1 = Session["ToplamUsd1"];
                ViewBag.ToplamGbp1 = Session["ToplamGbp1"];
                if ((string)Session["AltKalem"] == "altkalem")
                {
                    ViewBag.Yuzde = Session["Yuzde"];
                    ViewBag.ToplamMiktar1 = Session["ToplamMiktar1"];
                    ViewBag.ToplamMiktar2 = Session["ToplamMiktar2"];
                    ViewBag.AltKalem = GetSiparisApiData();
                    ViewBag.AltKalem1= Session["Filter2"];
                    Session["AltKalem"] = "test";
                    return View();

                }
                else if ((string)Session["AltKalem"] == "test")
                {
                    Session["AltKalem"] = null;
                }
                ViewBag.AltKalem = GetSiparisApiData();
                ViewBag.AltKalem1 = ViewBag.AltKalem;


                return View();
            }
            else
            {
                ViewBag.UstKalem = GetSiparisUstKAlem();
                ViewBag.AltKalem = GetSiparisApiData();
                ViewBag.AltKalem1 = ViewBag.AltKalem;
            }
            List<USTKALEMMODEL> list = GetSiparisUstKAlem();
            List<SiparisModel> list1 = GetSiparisApiData();
            decimal? toplamtry = 0;
            decimal? toplamtryfilter = 0;
            decimal? toplameur = 0;
            decimal? toplamusd = 0;
            decimal? toplamgbp = 0;
            decimal? toplamtry1 = 0;
            decimal? toplamtryfilter1 = 0;
            decimal? toplameur1 = 0;
            decimal? toplamusd1 = 0;
            decimal? toplamgbp1 = 0;
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

            }
            foreach (var item in list1)
            {
                toplamtry1 = toplamtry1 + item.NET_TUTAR_KALEM;
                
                    if (item.DOV_TIP == "USD")
                    {
                        toplamusd1 = toplamusd1 + item.DOV_TUTAR;
                    }
                    if (item.DOV_TIP == "EURO")
                    {
                        toplameur1 = toplameur1 + item.DOV_TUTAR;
                    }
                    if (item.DOV_TIP == "GBP")
                    {
                        toplamgbp1 = toplamgbp1 + item.DOV_TUTAR;
                    }
                if (item.DOV_TIP == null)
                {
                    toplamtryfilter1 = toplamtryfilter1 + item.NET_TUTAR_KALEM;
                }
                
               
                    
                

            }
            ViewBag.Toplam = toplamtry;

            ViewBag.ToplamTryFilter = toplamtryfilter;
            ViewBag.ToplamEur = toplameur;
            ViewBag.ToplamUsd = toplamusd;
            ViewBag.ToplamGbp = toplamgbp;
            ViewBag.Toplam1 = toplamtry1;

            ViewBag.ToplamTryFilter1 = toplamtryfilter1;
            ViewBag.ToplamEur1 = toplameur1;
            ViewBag.ToplamUsd1 = toplamusd1;
            ViewBag.ToplamGbp1 = toplamgbp1;
            Session["Toplam1"] = toplamtry1;
            Session["ToplamTryFilter1"] = toplamtryfilter1;
            Session["ToplamEur1"] = toplameur1;
            Session["ToplamUsd1"] = toplamusd1;
            Session["ToplamGbp1"] = toplamgbp1;
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
            ViewBag.RoleName = Request.Cookies["RoleName"].Value;
            ViewBag.Id = Request.Cookies["Id"].Value;
            var yetki = GetYetki();
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 19);
            if (yetkiKontrol.SELECT_AUTH != true)
            {
                Session["ModulYetkiMesajı"] = "Modüle yetkiniz bulunmamaktadır";

                return RedirectToAction("Index", "Home");
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
        public ActionResult Test()
        {
           
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
        public ActionResult Filter(FilterModel filter)
        {
            Session["Toplam"] = null;
            Session["ToplamTryFilter"] = null;
            Session["ToplamEur"] = null;
            Session["ToplamUsd"] = null;
            Session["ToplamGbp"] = null;
            List<USTKALEMMODEL> x = new List<USTKALEMMODEL>();
            List<string> y = new List<string>();
            List<string> z = new List<string>();
            List<string> m = new List<string>();
            bool u = false;
            string teslim = "";
            string cari = "";
            string plasiyer = "";
            if (filter.TESLIM_YERI != null)
                teslim = filter.TESLIM_YERI[0];
            if (filter.CARI_DOVIZ_TIPI != null)
                cari = filter.CARI_DOVIZ_TIPI[0];
            if (filter.PLASIYER_ADI != null)
                plasiyer = filter.PLASIYER_ADI[0];
            var teslimArray = teslim.Split(',');
            var cariArray = cari.Split(',');
            var plasiyerArray = plasiyer.Split(',');



            if (filter != null)
            {
                if (filter.URETILECEKMI)
                {
                    u = true;
                    if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in teslimArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                foreach (var item3 in plasiyerArray)
                                {
                                    x = x.Concat(GetSiparisUstKAlem().Where(t => t.CARI_DOVIZ_TIPI == item2 && t.CARI_ISIM == item3 && t.TESLIM_YERI == "EFECE 4")).ToList();
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

                                x = x.Concat(GetSiparisUstKAlem().Where(t => t.CARI_DOVIZ_TIPI == item2 && t.TESLIM_YERI == "EFECE 4")).ToList();
                                z.Add(item);
                                m.Add(item2);


                            }
                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in teslimArray)
                        {


                            x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == "EFECE 4")).ToList();
                            z.Add(item);



                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI == null)
                    {



                        x = GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == "EFECE 4").ToList();




                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in cariArray)
                        {


                            x = x.Concat(GetSiparisUstKAlem().Where(t => t.CARI_DOVIZ_TIPI == item && t.TESLIM_YERI == "EFECE 4")).ToList();

                            m.Add(item);

                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {


                            x = x.Concat(GetSiparisUstKAlem().Where(t => t.CARI_ISIM == item && t.TESLIM_YERI == "EFECE 4")).ToList();
                            y.Add(item);


                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                x = x.Concat(GetSiparisUstKAlem().Where(t => t.CARI_ISIM == item && t.CARI_DOVIZ_TIPI == item2 && t.TESLIM_YERI == "EFECE 4")).ToList();
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
                                x = x.Concat(GetSiparisUstKAlem().Where(t => t.CARI_ISIM == item2 && t.TESLIM_YERI == "EFECE 4")).ToList();
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
                                    x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == item && t.CARI_DOVIZ_TIPI == item2 && t.CARI_ISIM == item3)).ToList();
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


                            x = x.Concat(GetSiparisUstKAlem().Where(t => t.CARI_ISIM == item)).ToList();
                            y.Add(item);


                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                x = x.Concat(GetSiparisUstKAlem().Where(t => t.CARI_ISIM == item && t.CARI_DOVIZ_TIPI == item2)).ToList();
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
                                x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == item && t.CARI_ISIM == item2)).ToList();
                                y.Add(item2);
                                z.Add(item);
                            }





                        }
                    }
                }


            }
            ViewBag.AltKalem = GetSiparisApiData();
            var list = x;
            var list1= GetSiparisApiData();
            if (filter.BASLANGIC_TARIH != null && filter.BITIS_TARIH == null)
            {
                Session["Filter"] = x.Where(t => t.TARIH >= filter.BASLANGIC_TARIH);
                list = x.Where(t => t.TARIH >= filter.BASLANGIC_TARIH).ToList();
            }
            else if (filter.BASLANGIC_TARIH == null && filter.BITIS_TARIH != null)
            {
                Session["Filter"] = x.Where(t => t.TARIH <= filter.BITIS_TARIH);
                list = x.Where(t => t.TARIH <= filter.BITIS_TARIH).ToList();
            }
            else if (filter.BASLANGIC_TARIH != null && filter.BITIS_TARIH != null)
            {
                Session["Filter"] = x.Where(t => t.TARIH <= filter.BITIS_TARIH && t.TARIH >= filter.BASLANGIC_TARIH);
                list = x.Where(t => t.TARIH <= filter.BITIS_TARIH && t.TARIH >= filter.BASLANGIC_TARIH).ToList();
            }
            else
            {
                Session["Filter"] = x;
            }

            if (filter.SSIP_CHECK == "E")
            {
                Session["Filter"] = ((List<USTKALEMMODEL>)Session["Filter"]).Where(t => t.SSIP_CHECK == "E");
                list = list.Where(t => t.SSIP_CHECK == "E").ToList();
            }

            decimal? toplamtry = 0;
            decimal? toplamtryfilter = 0;
            decimal? toplameur = 0;
            decimal? toplamusd = 0;
            decimal? toplamgbp = 0;
            decimal? toplamtry1 = 0;
            decimal? toplamtryfilter1 = 0;
            decimal? toplameur1 = 0;
            decimal? toplamusd1 = 0;
            decimal? toplamgbp1 = 0;
            
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

            }
            foreach (var item in list1)
            {
                
                toplamtry1 = toplamtry1 + item.NET_TUTAR_KALEM;
                if (item.DOV_TIP != null)
                {
                    if (item.DOV_TIP == "USD")
                    {
                        toplamusd1 = toplamusd1 + item.DOV_GENELTOPLAM;
                    }
                    if (item.DOV_TIP == "EUR" || item.DOV_TIP == "EURO")
                    {
                        toplameur1 = toplameur1 + item.DOV_GENELTOPLAM;
                    }
                    if (item.DOV_TIP == "GBP")
                    {
                        toplamgbp1 = toplamgbp1 + item.DOV_GENELTOPLAM;
                    }

                }
                else
                {
                    toplamtryfilter1 = toplamtryfilter1 + item.NET_TUTAR_KALEM;
                }
                
            }
            Session["Toplam"] = toplamtry;
            Session["ToplamTryFilter"] = toplamtryfilter;
            Session["ToplamEur"] = toplameur;
            Session["ToplamUsd"] = toplamusd;
            Session["ToplamGbp"] = toplamgbp;
            Session["Toplam1"] = toplamtry1;
            Session["ToplamTryFilter1"] = toplamtryfilter1;
            Session["ToplamEur1"] = toplameur1;
            Session["ToplamUsd1"] = toplamusd1;
            Session["ToplamGbp1"] = toplamgbp1;
            ViewBag.FilterPlasiyer = y;
            ViewBag.FilterTeslim = z;
            ViewBag.FilterDoviz = m;
            ViewBag.FilterUretilecekmi = u;
            return null;
        }
        public ActionResult FilterAltKalem(FilterModel filter)
        {
            Session["AltKalem"] = "altkalem";
            Session["Toplam1"] = null;
            Session["ToplamTryFilter1"] = null;
            Session["ToplamEur1"] = null;
            Session["ToplamUsd1"] = null;
            Session["ToplamGbp1"] = null;
            List<SiparisModel> x = new List<SiparisModel>();
            List<string> y = new List<string>();
            List<string> z = new List<string>();
            List<string> m = new List<string>();
            bool u = false;
            string teslim = "";
            string cari = "";
            string plasiyer = "";
            if (filter.TESLIM_YERI != null)
                teslim = filter.TESLIM_YERI[0];
            if (filter.CARI_DOVIZ_TIPI != null)
                cari = filter.CARI_DOVIZ_TIPI[0];
            if (filter.STOK_ADI != null)
                plasiyer = filter.STOK_ADI[0];
            var teslimArray = teslim.Split(',');
            var plasiyerArray = plasiyer.Split('*');
            var cariArray = cari.Split(',');
            
            //string[] plasiyerArray = { plasiyer };



            if (filter != null)
            {
                if (filter.URETILECEKMI)
                {
                    u = true;
                    if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI != null && filter.STOK_ADI != null)
                    {
                        foreach (var item in teslimArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                foreach (var item3 in plasiyerArray)
                                {
                                    if (item3[item3.Length - 1] == ',')
                                    {
                                        x = x.Concat(GetSiparisApiData().Where(t => t.CARI_ISIM == item2 && t.STOK_ADI == item3.Substring(0, item3.Length - 1) && t.TESLIM_YERI == "EFECE 4")).ToList();
                                    }
                                    else
                                    {
                                        x = x.Concat(GetSiparisApiData().Where(t => t.CARI_ISIM == item2 && t.STOK_ADI == item3.Substring(0, item3.Length) && t.TESLIM_YERI == "EFECE 4")).ToList();
                                    }
                                   
                                    z.Add(item);
                                    m.Add(item2);
                                    y.Add(item3);
                                }
                            }
                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI != null && filter.STOK_ADI == null)
                    {
                        foreach (var item in teslimArray)
                        {
                            foreach (var item2 in cariArray)
                            {

                                x = x.Concat(GetSiparisApiData().Where(t => t.CARI_ISIM == item2 && t.TESLIM_YERI == "EFECE 4")).ToList();
                                z.Add(item);
                                m.Add(item2);


                            }
                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.STOK_ADI == null)
                    {
                        foreach (var item in teslimArray)
                        {


                            x = x.Concat(GetSiparisApiData().Where(t => t.TESLIM_YERI == "EFECE 4")).ToList();
                            z.Add(item);



                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.STOK_ADI == null)
                    {



                        x = GetSiparisApiData().Where(t => t.TESLIM_YERI == "EFECE 4").ToList();




                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.STOK_ADI == null)
                    {
                        foreach (var item in cariArray)
                        {


                            x = x.Concat(GetSiparisApiData().Where(t => t.CARI_ISIM == item && t.TESLIM_YERI == "EFECE 4")).ToList();

                            m.Add(item);

                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.STOK_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {
                            if (item.Length != 0)
                            {
                                if (item[item.Length-1] == ',')
                                {
                                    x = x.Concat(GetSiparisApiData().Where(t => t.STOK_ADI == item.Substring(0, item.Length - 1) && t.TESLIM_YERI == "EFECE 4")).ToList();
                                }
                                else
                                {
                                    x = x.Concat(GetSiparisApiData().Where(t => t.STOK_ADI == item.Substring(0, item.Length) && t.TESLIM_YERI == "EFECE 4")).ToList();
                                }
                                
                                y.Add(item);
                            }

                            


                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.STOK_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                if (item2.Length != 0)
                                {
                                    if (item2[item2.Length-1] == ',')
                                    {
                                        x = x.Concat(GetSiparisApiData().Where(t => t.STOK_ADI == item.Substring(0, item.Length - 1) && t.CARI_ISIM == item2 && t.TESLIM_YERI == "EFECE 4")).ToList();
                                    }
                                    else
                                    {
                                        x = x.Concat(GetSiparisApiData().Where(t => t.STOK_ADI == item.Substring(0, item.Length) && t.CARI_ISIM == item2 && t.TESLIM_YERI == "EFECE 4")).ToList();
                                    }
                                    m.Add(item2);
                                    y.Add(item);
                                }
                               
                            }





                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.STOK_ADI != null)
                    {
                        foreach (var item in teslimArray)
                        {
                            foreach (var item2 in plasiyerArray)
                            {
                                if (item2.Length != 0)
                                {
                                    if (item2[item2.Length-1] == ',')
                                    {
                                        x = x.Concat(GetSiparisApiData().Where(t => t.STOK_ADI == item2.Substring(0, item2.Length - 1) && t.TESLIM_YERI == "EFECE 4")).ToList();
                                    }
                                    else
                                    {
                                        x = x.Concat(GetSiparisApiData().Where(t => t.STOK_ADI == item2.Substring(0, item2.Length) && t.TESLIM_YERI == "EFECE 4")).ToList();
                                    }
                                    
                                    y.Add(item2);
                                    z.Add(item);
                                }
                              
                            }





                        }
                    }
                }
                else
                {
                    if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI != null && filter.STOK_ADI != null)
                    {
                        foreach (var item in teslimArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                foreach (var item3 in plasiyerArray)
                                {
                                    if (item3.Length != 0)
                                    {
                                        if (item3[item3.Length-1] == ',')
                                        {
                                            x = x.Concat(GetSiparisApiData().Where(t => t.TESLIM_YERI == item && t.CARI_ISIM == item2 && t.STOK_ADI == item3.Substring(0, item3.Length - 1))).ToList();
                                        }
                                        else
                                        {
                                            x = x.Concat(GetSiparisApiData().Where(t => t.TESLIM_YERI == item && t.CARI_ISIM == item2 && t.STOK_ADI == item3.Substring(0, item3.Length))).ToList();
                                        }
                                       
                                        z.Add(item);
                                        m.Add(item2);
                                        y.Add(item3);
                                    }
                                   
                                }
                            }
                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI != null && filter.STOK_ADI == null)
                    {
                        foreach (var item in teslimArray)
                        {
                            foreach (var item2 in cariArray)
                            {

                                x = x.Concat(GetSiparisApiData().Where(t => t.TESLIM_YERI == item && t.CARI_ISIM == item2)).ToList();
                                z.Add(item);
                                m.Add(item2);


                            }
                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.STOK_ADI == null)
                    {
                        foreach (var item in teslimArray)
                        {


                            x = x.Concat(GetSiparisApiData().Where(t => t.TESLIM_YERI == item)).ToList();
                            z.Add(item);



                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.STOK_ADI == null)
                    {



                        x = GetSiparisApiData().ToList();




                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.STOK_ADI == null)
                    {
                        foreach (var item in cariArray)
                        {


                            x = x.Concat(GetSiparisApiData().Where(t => t.CARI_ISIM == item)).ToList();

                            m.Add(item);

                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.STOK_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {

                            if (item.Length != 0)
                            {
                                if (item[item.Length-1] == ',')
                                {
                                    x = x.Concat(GetSiparisApiData().Where(t => t.STOK_ADI == item.Substring(0, item.Length - 1))).ToList();
                                }
                                else
                                {
                                    x = x.Concat(GetSiparisApiData().Where(t => t.STOK_ADI == item.Substring(0, item.Length))).ToList();
                                }
                                
                                y.Add(item);
                            }
                            


                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.STOK_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                if (item2.Length != 0)
                                {
                                    if (item2[item2.Length-1] == ',')
                                    {
                                        x = x.Concat(GetSiparisApiData().Where(t => t.STOK_ADI == item.Substring(0, item.Length - 1) && t.CARI_ISIM == item2)).ToList();
                                    }
                                    else
                                    {
                                        x = x.Concat(GetSiparisApiData().Where(t => t.STOK_ADI == item.Substring(0, item.Length) && t.CARI_ISIM == item2)).ToList();
                                    }
                                    
                                    m.Add(item2);
                                    y.Add(item);

                                }
                               
                            }





                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.STOK_ADI != null)
                    {
                        foreach (var item in teslimArray)
                        {
                            foreach (var item2 in plasiyerArray)
                            {if(item2.Length != 0)
                                {
                                    if (item2[item2.Length-1] == ',')
                                    {
                                        x = x.Concat(GetSiparisApiData().Where(t => t.TESLIM_YERI == item && t.STOK_ADI == item2.Substring(0, item2.Length - 1))).ToList();
                                    }
                                    else
                                    {
                                        x = x.Concat(GetSiparisApiData().Where(t => t.TESLIM_YERI == item && t.STOK_ADI == item2.Substring(0, item2.Length))).ToList();
                                    }
                                   
                                    y.Add(item2);
                                    z.Add(item);
                                }
                                
                            }





                        }
                    }
                }


            }
            var list = x;
            if (filter.BASLANGIC_TARIH != null && filter.BITIS_TARIH == null)
            {
                Session["Filter"] = x.Where(t => t.TARIH >= filter.BASLANGIC_TARIH);
                list = x.Where(t => t.TARIH >= filter.BASLANGIC_TARIH).ToList();
            }
            else if (filter.BASLANGIC_TARIH == null && filter.BITIS_TARIH != null)
            {
                Session["Filter"] = x.Where(t => t.TARIH <= filter.BITIS_TARIH);
                list = x.Where(t => t.TARIH <= filter.BITIS_TARIH).ToList();
            }
            else if (filter.BASLANGIC_TARIH != null && filter.BITIS_TARIH != null)
            {
                Session["Filter"] = x.Where(t => t.TARIH <= filter.BITIS_TARIH && t.TARIH >= filter.BASLANGIC_TARIH);
                list = x.Where(t => t.TARIH <= filter.BITIS_TARIH && t.TARIH >= filter.BASLANGIC_TARIH).ToList();
            }
            else
            {
                Session["Filter"] = x;
            }

            

            decimal? toplamtry = 0;
            decimal? toplamtryfilter = 0;
            decimal? toplameur = 0;
            decimal? toplamusd = 0;
            decimal? toplamgbp = 0;
            decimal? toplammiktar1 = 0;
            decimal? toplammiktar2 = 0;
            decimal? yuzde = 0;
            int i = 0;
            foreach (var item in list)
            {
                yuzde = yuzde + item.TESLIMAT_ORANI;
                toplamtry = toplamtry + item.NET_TUTAR_KALEM;
                toplammiktar1 = toplammiktar1 + item.MIKTAR;
                toplammiktar2 = toplammiktar2 + item.MIKTAR2;
                if (item.DOV_TIP == "USD")
                    {
                        toplamusd = toplamusd + item.DOV_TUTAR;
                    }
                    if (item.DOV_TIP == "EURO")
                    {
                        toplameur = toplameur + item.DOV_TUTAR;
                    }
                    if (item.DOV_TIP == "GBP")
                    {
                        toplamgbp = toplamgbp + item.DOV_TUTAR;
                    }
                    if (item.DOV_TIP == null)
                    {
                        toplamtryfilter = toplamtryfilter + item.NET_TUTAR_KALEM;
                    }

                i++;
            }
            Session["Toplam1"] = toplamtry;
            Session["ToplamTryFilter1"] = toplamtryfilter;
            Session["ToplamEur1"] = toplameur;
            Session["ToplamUsd1"] = toplamusd;
            Session["ToplamGbp1"] = toplamgbp;
            Session["ToplamMiktar1"] = toplammiktar1;
            Session["ToplamMiktar2"] = toplammiktar2;
            if (yuzde != 0)
            {
                Session["Yuzde"] = yuzde / i;
            }
            else { Session["Yuzde"] = yuzde; }
            ViewBag.FilterPlasiyer = y;
            ViewBag.FilterTeslim = z;
            ViewBag.FilterDoviz = m;
            ViewBag.FilterUretilecekmi = u;
            return null;
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
       
        public List<SiparisModel> GetSiparisApiData()
        {


            var apiUrl = "http://192.168.2.13:83/api/detayliSSIP";

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
        public List<SSIPExcel> GetSiparisApiDataExcel()
        {


            var apiUrl = "http://192.168.2.13:83/api/detayliSSIP";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;
            List<SSIPExcel> jsonList = ser.Deserialize<List<SSIPExcel>>(json);

            //END

            return jsonList;
        }
        public List<USTKALEMMODEL> GetSiparisUstKAlem()
        {


            var apiUrl = "http://192.168.2.13:83/api/detaysizSSIP";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;
            List<USTKALEMMODEL> jsonList = ser.Deserialize<List<USTKALEMMODEL>>(json);

            //END

            return jsonList;
        }
        public ActionResult MailDetayliSip()
        {

            if (Request.Cookies["Mail"] != null)
            {

                Session["Info"] = "Başarılı";


                string subject = "NOVA | Detaylı Satın Alma Bilgisi";

                string body = "";
                var stoklar = GetSiparisApiDataExcel();
                System.IO.MemoryStream str = DataToExcel(ToDataTable(stoklar));
                Attachment at = new Attachment(str, "DetayliSatinAlmaBilgisi_" + DateTime.Now + ".xls");
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




            return RedirectToAction("SatinAlmaRaporu");

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
}