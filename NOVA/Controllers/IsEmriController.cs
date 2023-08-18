﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
//using NetOpenX50;
using NOVA.Models;
using System.Net;
using System.Web.Script.Serialization;
using SqlApi.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web.Security;
using System.Web.Helpers;
using NetOpenX50;
using static NOVA.Controllers.SatisController;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Ajax.Utilities;
using System.Net.Sockets;
using static NOVA.Controllers.IsEmriController;
using System.Runtime.ConstrainedExecution;
using ServiceStack;
using DocumentFormat.OpenXml.Wordprocessing;

namespace NOVA.Controllers
{

    public class IsEmriController : Controller
    {
        Kernel kernel = new Kernel();
        IsEmri Isemri = default(IsEmri);
        IsEmri Isemri1 = default(IsEmri);
        IsEmri isemri = default(IsEmri);
        Sirket sirket = default(Sirket);
        SerbestUSK uretim = default(SerbestUSK);
        public ActionResult DrawImages()
        {
            Image playbutton;
            try
            {
                playbutton = Image.FromFile("C:\\Users\\surec.gelistirme2\\Desktop\\111.png");
            }
            catch (Exception ex)
            {
                return null;
            }

            Image frame;
            try
            {
                frame = Image.FromFile("C:\\Users\\surec.gelistirme2\\Desktop\\222.png");
            }
            catch (Exception ex)
            {
                return null;
            }

            using (frame)
            {
                using (var bitmap = new Bitmap(600, 1200))
                {
                    using (var canvas = Graphics.FromImage(bitmap))
                    {
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        canvas.DrawImage(playbutton,
                                         (bitmap.Width / 2) - (playbutton.Width / 2),
                                         (bitmap.Height / 2) - (playbutton.Height / 2));
                        canvas.DrawImage(frame,
                                        (bitmap.Width / 2) - (playbutton.Width / 2),
                                         575, 100, 50);
                        canvas.Save();
                    }
                    try
                    {
                        bitmap.Save("C:\\Users\\surec.gelistirme2\\Desktop\\hello.jpeg",
                                    System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }
            return null;
        }
        // GET: NetOpenx
        public ActionResult Index()
        {
            if (TempData["Hata"] != null)
            {
                ViewBag.Hata = TempData["Hata"];
            }

            if (Request.Cookies["Id"] == null)
            {
                FormsAuthentication.SignOut();
                TempData["LOG"] = "ok";
                return RedirectToAction("Login", "Login");
            }
            //DrawImages();
            ViewBag.Makine = GetMak(1);
            if (Request.Cookies["Id"] != null)
            {
                ViewBag.Id = Request.Cookies["Id"].Value;
            }

            ViewBag.UretimTakip = Get();
            ViewBag.SIRANO = GetMax();
            ViewBag.Stok_Adlari = GetStokAdlari();
            ViewBag.Cariler = GetCariler();
            ViewBag.Sip = GetSip();
            ViewBag.Seriler = GetSeri();

            return View();
        }

        public ActionResult Test()
        {
            if (TempData["Hata"] != null)
            {
                ViewBag.Hata = TempData["Hata"];
            }

            if (Request.Cookies["Id"] == null)
            {
                FormsAuthentication.SignOut();
                TempData["LOG"] = "ok";
                return RedirectToAction("Login", "Login");
            }
            //DrawImages();
            ViewBag.Makine = GetMak(2);
            if (Request.Cookies["Id"] != null)
            {
                ViewBag.Id = Request.Cookies["Id"].Value;
            }

            ViewBag.UretimTakip = Get();
            ViewBag.SIRANO = GetMax();
            ViewBag.Stok_Adlari = GetStokAdlari();
            ViewBag.Cariler = GetCariler();
            ViewBag.Sip = GetSip();
            ViewBag.Seriler = GetSeri();

            return View();
        }
        public class IsEmriMod
        {
            public string ISEMRINO { get; set; }
            public string SERI_NO { get; set; }
        }
        
        public Microsoft.AspNetCore.Mvc.StatusCodeResult UretimSonuKaydı(string hatkodu, string stokkodu, string genislik, string mik1, string mik2)
        {
            var uretimTipi = UretimTipi(hatkodu)[0].URETIM_TIPI;

            try
            {
                
                
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                             "TEST2022",
                                             "TEMELSET",
                                             "",
                                             "nova",
                                             "Efc@+180", 0);


                JavaScriptSerializer ser = new JavaScriptSerializer();
                if (genislik == "")
                {
                    genislik = "0";
                }
                var apiUrl2 = "http://192.168.2.13:83/api/ie/USK/" + hatkodu + "/" + stokkodu + "/0/" + mik1 + "/" + mik2 + "/" + genislik;

                //Connect API
                Uri url2 = new Uri(apiUrl2);
                WebClient client = new WebClient();
                client.Encoding = System.Text.Encoding.UTF8;

                var json2 = client.DownloadString(url2);
                List<USKModel> jsonList = ser.Deserialize<List<USKModel>>(json2);

                try
                {

                    var ilkseri = "";
                    var karsi = "";
                    for (var i = 0; i < jsonList.Count; i++)
                    {
                        NetRS netRS = kernel.yeniNetRS(sirket);
                        netRS.Ac("SELECT * FROM TBLISEMRIREC WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");

                        var miktarsabitle = netRS.FieldByName("MIKTARSABITLE").AsString;
                        netRS.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                        var eskimiktar = netRS.FieldByName("MIKTAR").AsFloat;
                        if (jsonList[i].KULL_MIKTAR != eskimiktar)
                        {
                            if (miktarsabitle == "E")
                            {
                                netRS.Ac("UPDATE TBLISEMRI SET MIKTAR=" + jsonList[i].KULL_MIKTAR + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                            }

                        }


                        uretim = kernel.yeniSerbestUSK(sirket);
                        uretim.IsEmrindenGetir(jsonList[i].ISEMRINO);
                        uretim.UretSon_FisNo = uretim.SonFisNumarasi("N");

                        uretim.UretSon_Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                        uretim.BelgeTipi = TBelgeTipi.btIsEmri;
                        uretim.Proje_Kodu = "1";
                        uretim.UretSon_Miktar = jsonList[i].KULL_MIKTAR;
                        if (miktarsabitle == "E")
                        {
                            uretim.F_Yedek1 = 1;
                        }
                        else
                        {
                            uretim.F_Yedek1 = jsonList[i].MIKTAR2;
                        }

                        uretim.UretSon_Depo = 45;
                        uretim.I_Yedek1 = 45;
                        uretim.I_Yedek2 = 0;
                        uretim.OTO_YMAM_GIRDI_CIKTI = true;
                        uretim.OTO_YMAM_STOK_KULLAN = false;


                        uretim.BAKIYE_DEPO = 0;
                        netRS.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                        var seri = netRS.FieldByName("SERINO").AsString;
                        if (seri == null)
                        {
                            uretim.OTOSERIURET();
                            uretim.SeriEkle(uretim.SeriOku(0).Seri1, "", "", "", jsonList[i].KULL_MIKTAR, jsonList[i].MIKTAR2);
                        }

                        NetRS netRS1 = kernel.yeniNetRS(sirket);
                        uretim.FisUret();
                        uretim.Kaydet();
                        //if (miktarsabitle == "E")
                        //{
                        //    netRS.Ac("UPDATE TBLISEMRI SET MIKTAR=" + eskimiktar + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                        //}

                        //netRS1.Ac("UPDATE TBLSERITRA SET MIKTAR2=" + jsonList[i].MIKTAR2 + " WHERE  GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                        netRS1.Ac("SELECT * FROM TBLSERITRA WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + jsonList[0].ISEMRINO + "'");

                        if (i == 0)
                        {
                            karsi = netRS1.FieldByName("SERI_NO").AsString;
                        }

                        netRS1.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                        //if (eskimiktar!= jsonList[i].KULL_MIKTAR)
                        //{
                        //    netRS1.Ac("UPDATE TBLISEMRIREC SET MIKTAR=" + (eskimiktar - jsonList[i].KULL_MIKTAR) + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                        //}
                        if (miktarsabitle != "E")
                        {
                            netRS1.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                        }
                        else
                        {
                            
                            netRS1.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='"+ jsonList[i].ISEMRINO + "'");
                            var referans = netRS1.FieldByName("REFISEMRINO").AsString;
                            netRS1.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + referans + "'");
                            var eski = netRS1.FieldByName("MIKTAR").AsFloat;
                            var oran =  jsonList[i].KULL_MIKTAR/ eski;
                            var miktar2 = netRS1.FieldByName("ACIKLAMA").AsString;
                            var yeni = miktar2.ToDouble() * oran;
                            
                            netRS1.Ac("UPDATE TBLISEMRI SET MIKTAR='" + jsonList[i].KULL_MIKTAR + "',ACIKLAMA='"+ Math.Round(yeni) + "' WHERE ISEMRINO='" + referans + "'");

                        }


                    }

                    //Stok hareketleri gerçeklestiriliyor
                    //uretim.kayitFisNoIleUretimSonu(uretim.UretSon_FisNo, TUretSonDepo.usdAktif,false,false);
                }
                catch (Exception e)
                {
                    var exp = e.Message;
                    System.Diagnostics.Debug.Write(exp);
                    return new Microsoft.AspNetCore.Mvc.StatusCodeResult(404);
                }
                finally
                {
                    Marshal.ReleaseComObject(uretim);
                    Marshal.ReleaseComObject(sirket);
                    kernel.FreeNetsisLibrary();
                    Marshal.ReleaseComObject(kernel);
                }
            }
            catch (Exception e)
            {
                var exp = e.Message;
                Console.WriteLine(e.Message);
                throw;
            }
            
           
            
            
                return new Microsoft.AspNetCore.Mvc.StatusCodeResult(200);
            }
        public Microsoft.AspNetCore.Mvc.StatusCodeResult TrpzUretim(string stokkodu,string ISEMRINO,string SERI_NO, string KULL_MIKTAR, string mik2)
        {
            if (KULL_MIKTAR != "0")
            {
                try
                {

                    
                    sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                                 "TEST2022",
                                                 "TEMELSET",
                                                 "",
                                                 "nova",
                                                 "Efc@+180", 0);


                    uretim = kernel.yeniSerbestUSK(sirket);
                    uretim.IsEmrindenGetir(ISEMRINO);
                    uretim.UretSon_FisNo = uretim.SonFisNumarasi("N");

                    uretim.UretSon_Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                    uretim.BelgeTipi = TBelgeTipi.btIsEmri;
                    uretim.Proje_Kodu = "1";
                    uretim.UretSon_Miktar = KULL_MIKTAR.ToDouble();

                    uretim.F_Yedek1 = mik2.ToDouble();


                    uretim.UretSon_Depo = 45;
                    uretim.I_Yedek1 = 45;
                    uretim.I_Yedek2 = 0;
                    uretim.OTO_YMAM_GIRDI_CIKTI = true;
                    uretim.OTO_YMAM_STOK_KULLAN = false;


                    uretim.BAKIYE_DEPO = 0;
                    uretim.OTOSERIURET();
                    uretim.SeriEkle(uretim.SeriOku(0).Seri1, "", "", "", KULL_MIKTAR.ToDouble(), mik2.ToDouble());
                    NetRS netRS = kernel.yeniNetRS(sirket);
                    netRS.Ac("UPDATE TBLISEMRIREC SET SERINO='" + SERI_NO + "' WHERE ISEMRINO='" + ISEMRINO + "'");


                    uretim.FisUret();
                    uretim.Kaydet();
                    netRS.Ac("UPDATE TBLISEMRIREC SET SERINO=NULL WHERE ISEMRINO='" + ISEMRINO + "'");
                    
                    netRS.Ac("SELECT * FROM TBLSERITRA WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
                    var karsi = netRS.FieldByName("SERI_NO").AsString;
                    

                    netRS.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
                    //if (eskimiktar!= jsonList[i].KULL_MIKTAR)
                    //{
                    //    netRS1.Ac("UPDATE TBLISEMRIREC SET MIKTAR=" + (eskimiktar - jsonList[i].KULL_MIKTAR) + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                    //}
                    
                   
                    netRS.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" +ISEMRINO + "'");
                    

                }
                catch (Exception exp)
                {
                    var message = exp.Message;
                    return new Microsoft.AspNetCore.Mvc.StatusCodeResult(404);
                }
            }
            else
            {
                try
                {
                    Kernel kernel = new Kernel();
                    Sirket sirket = default(Sirket);
                    sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                                 "TEST2022",
                                                 "TEMELSET",
                                                 "",
                                                 "nova",
                                                 "Efc@+180", 0);
                    NetRS netRS = kernel.yeniNetRS(sirket);
                    netRS.Ac("UPDATE TBLISEMRI SET KAPALI='E' WHERE ISEMRINO='" + ISEMRINO + "'");
                    netRS.Ac("SELECT TOP(1) * FROM TBLSERITRA WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "' ORDER BY BELGENO DESC");
                    var karsi = netRS.FieldByName("SERI_NO").AsString;
                    netRS.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "' WHERE SIPNO='" + ISEMRINO + "'");
                    netRS.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
                }
                catch (Exception)
                {

                    return new Microsoft.AspNetCore.Mvc.StatusCodeResult(404);
                }
                
            }
          
            return new Microsoft.AspNetCore.Mvc.StatusCodeResult(200);
        }
        public Microsoft.AspNetCore.Mvc.StatusCodeResult UretimSonuKaydi2()
        {
           
            
            string serino = null;
            var apiUrl = "http://192.168.2.13:83/api/ie/USK/DL01/150.200.0000.0000";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<USKModel> jsonList = ser.Deserialize<List<USKModel>>(json);
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                                 "TEST2022",
                                                 "TEMELSET",
                                                 "",
                                                 "nova",
                                                 "Efc@+180", 0);

               
                for(var i = 0; i < jsonList.Count; i++)
                {
                    NetRS netRS = kernel.yeniNetRS(sirket);
                    netRS.Ac("SELECT * FROM TBLISEMRIREC WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                    var eskimiktar = netRS.FieldByName("MIKTAR").AsFloat;
                    netRS.Ac("UPDATE TBLISEMRIREC SET MIKTAR=" + jsonList[i].KULL_MIKTAR + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                    uretim = kernel.yeniSerbestUSK(sirket);
                    uretim.IsEmrindenGetir(jsonList[i].ISEMRINO);
                    uretim.UretSon_FisNo = uretim.SonFisNumarasi("N");
                    
                    uretim.UretSon_Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                    uretim.BelgeTipi = TBelgeTipi.btIsEmri;
                    uretim.Proje_Kodu = "1";
                    uretim.UretSon_Miktar = jsonList[i].KULL_MIKTAR;
                    uretim.UretSon_Depo = 45;
                    uretim.I_Yedek1 = 45;
                    uretim.OTO_YMAM_GIRDI_CIKTI = true;
                    uretim.OTO_YMAM_STOK_KULLAN = false;
                    uretim.F_Yedek1 = 0;
                    uretim.BAKIYE_DEPO = 0;
                    if (serino == null)
                    {
                        uretim.OTOSERIURET();
                        uretim.SeriEkle(uretim.SeriOku(0).Seri1, "", "", "", jsonList[i].KULL_MIKTAR, 0);
                    }
                    else
                    {
                        uretim.SeriEkle(jsonList[i].SERI_NO, "", "", "", jsonList[i].KULL_MIKTAR, 0);
                    }
                    NetRS netRS1 = kernel.yeniNetRS(sirket);
                    uretim.FisUret();
                    uretim.Kaydet();
                    netRS1.Ac("UPDATE TBLISEMRIREC SET MIKTAR=" + eskimiktar + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                }
                
                //Stok hareketleri gerçeklestiriliyor
                //uretim.kayitFisNoIleUretimSonu(uretim.UretSon_FisNo, TUretSonDepo.usdAktif,false,false);
            }
            catch (Exception e)
            {
                var exp = e.Message;
                Console.WriteLine(e.Message);
            }
            finally
            {
                Marshal.ReleaseComObject(uretim);
                Marshal.ReleaseComObject(sirket);
                kernel.FreeNetsisLibrary();
                Marshal.ReleaseComObject(kernel);
            }
            return new Microsoft.AspNetCore.Mvc.StatusCodeResult(200);
        }
        public List<HatModel> UretimTipi(string hatKodu)
        {
            
            var apiUrl2 = "http://192.168.2.13:83/api/ie/hatprm/"+hatKodu;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            //Connect API
            Uri url2 = new Uri(apiUrl2);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            var json2 = client.DownloadString(url2);
            List<HatModel> jsonList = ser.Deserialize<List<HatModel>>(json2);

            return jsonList;
        }
        public List<TRPZModel> TRPZUret(string stokkodu,int miktar)
        {

            var apiUrl2 = "http://192.168.2.13:83/api/ie/TRPZ/"+stokkodu+"/"+miktar;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            //Connect API
            Uri url2 = new Uri(apiUrl2);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            var json2 = client.DownloadString(url2);
            List<TRPZModel> jsonList = ser.Deserialize<List<TRPZModel>>(json2);

            return jsonList;
        }
        [HttpPost]
        public ActionResult PostTRPZ(List<IsEmriModel2> isemri)
        {
            List<SeriModel> createdlog1 = null;
            var apiUrl2 = "http://192.168.2.13:83/api/seri/1";
            Uri url2 = new Uri(apiUrl2);
            WebClient client2 = new WebClient();
            client2.Encoding = System.Text.Encoding.UTF8;

            var json2 = client2.DownloadString(url2);
            JavaScriptSerializer ser2 = new JavaScriptSerializer();
            createdlog1 = ser2.Deserialize<List<SeriModel>>(json2);

            try
            {
                var stokadlari = GetStokAdlari();
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                             "TEST2022",
                                             "TEMELSET",
                                             "",
                                             "nova",
                                             "Efc@+180", 0);

                for (int i = 0; i < isemri.Count(); i++)
                {

                    if (isemri[i].REF_ISEMRINO != "-")
                    {
                        var stokkodu = stokadlari.Where(x => x.STOK_ADI == isemri[i].REF_STOKADI);


                        Isemri = kernel.yeniIsEmri(sirket);
                        Isemri.IsEmriNo = isemri[i].REF_ISEMRINO;
                        Isemri.StokKodu = stokkodu.First().STOK_KODU;
                        Isemri.Kapali = false;
                        Isemri.ReceteSaklansin = true;
                        Isemri.ProjeKodu = "1";
                        Isemri.Oncelik = 0;
                        Isemri.Aciklama = isemri[i].REF_ADET;
                        Isemri.DepoKodu = 45;
                        Isemri.CikisDepoKodu = 45;
                        Isemri.SeriNo = null;
                        Isemri.Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                        Isemri.TeslimTarihi = Convert.ToDateTime("2023-12-31");

                        double m2 = 0;
                        if (isemri[i].AGIRLIK.Contains('.'))
                        {
                            m2 = Double.Parse(isemri[i].AGIRLIK.Split('.')[0] + isemri[i].AGIRLIK.Split('.')[1]);
                        }
                        else
                        {
                            m2 = Double.Parse(isemri[i].AGIRLIK);
                        }
                        Isemri.Miktar = m2;
                        Isemri.kayitYeni();
                        NetRS netRS = kernel.yeniNetRS(sirket);
                        if (isemri[i].GİRDİ2 != "-")
                        {
                            netRS.Ac("UPDATE TBLISEMRIREC SET SERINO='" + isemri[i].GİRDİ2 + "',DEPO_KODU='45',MIKTAR=1,MIKTARSABITLE='H' WHERE ISEMRINO='" + isemri[i].REF_ISEMRINO + "'");
                        }


                    }



                    var stokkodu1 = stokadlari.Where(x => x.STOK_ADI == isemri[i].STOKADI);

                    Isemri1 = kernel.yeniIsEmri(sirket);
                    Isemri1.IsEmriNo = isemri[i].ISEMRINO;
                    Isemri1.StokKodu = stokkodu1.First().STOK_KODU;
                    Isemri1.Aciklama = isemri[i].ADET;
                    Isemri1.Kapali = false;
                    Isemri1.ReceteSaklansin = true;
                    Isemri1.ProjeKodu = "1";
                    Isemri1.Oncelik = 0;
                    if (isemri[i].REF_ISEMRINO != "-")
                    {
                        Isemri1.RefIsEmriNo = isemri[i].REF_ISEMRINO;
                    }
                    Isemri1.DepoKodu = 45;
                    Isemri1.CikisDepoKodu = 45;
                    if (isemri[i].GİRDİ2 != "-")
                    {
                        Isemri1.SeriNo = isemri[i].GİRDİ2;
                    }
                    double mik = 0;
                    if (isemri[i].AGIRLIK.Contains('.'))
                    {
                        mik = Double.Parse(isemri[i].AGIRLIK.Split('.')[0] + isemri[i].AGIRLIK.Split('.')[1]);
                    }
                    else
                    {
                        mik = Double.Parse(isemri[i].AGIRLIK);
                    }
                    Isemri1.Miktar = mik;
                    Isemri1.TeslimTarihi = Convert.ToDateTime("2023-12-31");
                    Isemri1.Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                    Isemri1.kayitYeni();
                    NetRS netRS2 = kernel.yeniNetRS(sirket);
                    netRS2.Ac("UPDATE TBLISEMRIREC SET MIKTAR=1,MIKTARSABITLE='E', DEPO_KODU='45' WHERE ISEMRINO='" + isemri[i].ISEMRINO + "'");

                }
            }
            catch (Exception ex)
            {

                TempData["Hata"] = "HATA";
                var msg = ex.Message;
                return View("Index");
            }






            return RedirectToAction("Index");
        }
        public class IsEmriModel2
        {

            public string GİRDİ2 { get; set; }
            public string ISEMRINO { get; set; }
            public string STOKADI { get; set; }
            public string AGIRLIK { get; set; }
            public string ADET { get; set; }
            public string REF_ADET { get; set; }
            public string REF_ISEMRINO { get; set; }
            public string REF_STOKADI { get; set; }


        }
        [HttpPost]
        public ActionResult Post(List<IsEmriModel> isemri)
        {

            var isemridis = isemri.GroupBy(x => x.GIRDI2).Select(y => y.First()).ToList();
            List<SeriModel> createdlog1 = null;
            var apiUrl2 = "http://192.168.2.13:83/api/seri/1";
            Uri url2 = new Uri(apiUrl2);
            WebClient client2 = new WebClient();
            client2.Encoding = System.Text.Encoding.UTF8;

            var json2 = client2.DownloadString(url2);
            JavaScriptSerializer ser2 = new JavaScriptSerializer();
            createdlog1 = ser2.Deserialize<List<SeriModel>>(json2);

            try
            {
                var stokadlari = GetStokAdlari();
                Kernel kernel = new Kernel();

                Sirket sirket = default(Sirket);
                IsEmri Isemri = default(IsEmri);
                IsEmri Isemri1 = default(IsEmri);
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                             "TEST2022",
                                             "TEMELSET",
                                             "",
                                             "nova",
                                             "Efc@+180", 0);

                for (int i = 0; i < isemridis.Count(); i++)
                {

                    if (isemridis[i].REF_ISEMRINO != null)
                    {
                        var stokkodu = stokadlari.Where(x => x.STOK_ADI == isemridis[i].REF_STOKOLCUSU);
                        
                        
                        Isemri = kernel.yeniIsEmri(sirket);
                        Isemri.IsEmriNo = isemridis[i].REF_ISEMRINO;
                        Isemri.StokKodu = stokkodu.First().STOK_KODU;
                        Isemri.Kapali = false;
                        Isemri.ReceteSaklansin = true;
                        Isemri.ProjeKodu = "1";
                        Isemri.Oncelik = 0;
                        Isemri.Aciklama = isemridis[i].REF_ADET;
                        Isemri.DepoKodu = 45;
                        Isemri.CikisDepoKodu = 45;
                        Isemri.SeriNo = null;
                        Isemri.SeriNo2 = isemridis[i].GENISLIK;
                        Isemri.Tarih= Convert.ToDateTime(DateTime.Now.Year+"-"+ DateTime.Now.Month+"-"+ DateTime.Now.Day);
                        Isemri.TeslimTarihi = Convert.ToDateTime("2023-12-31");
                        Isemri.SipKont = isemridis[i].SIPKONT;
                        Isemri.SiparisNo = isemridis[i].SIPARISNO;
                        
                        double m2 = 0;
                        if (isemridis[i].AGIRLIK.Contains('.'))
                        {
                            m2 = Double.Parse(isemridis[i].AGIRLIK.Split('.')[0] + isemridis[i].AGIRLIK.Split('.')[1]);
                        }
                        else
                        {
                            m2 = Double.Parse(isemridis[i].AGIRLIK);
                        }
                        Isemri.Miktar = m2;
                        Isemri.kayitYeni();
                        NetRS netRS = kernel.yeniNetRS(sirket);
                        netRS.Ac("UPDATE TBLISEMRIREC SET SERINO='" + isemridis[i].GIRDI2 + "',DEPO_KODU='45',MIKTAR=1,MIKTARSABITLE='H' WHERE ISEMRINO='" + isemridis[i].REF_ISEMRINO + "'");
                        
                    }



                    var stokkodu1 = stokadlari.Where(x => x.STOK_ADI == isemridis[i].STOKADI);
                    
                    Isemri1 = kernel.yeniIsEmri(sirket);
                    Isemri1.IsEmriNo = isemridis[i].ISEMRINO;
                    Isemri1.StokKodu = stokkodu1.First().STOK_KODU;
                    Isemri1.Aciklama = isemridis[i].ADET;
                    Isemri1.Kapali = false;
                    Isemri1.ReceteSaklansin = true;
                    Isemri1.ProjeKodu = "1";
                    Isemri1.Oncelik = 0;
                    Isemri1.RefIsEmriNo = isemridis[i].REF_ISEMRINO;
                    Isemri1.DepoKodu = 45;
                    Isemri1.CikisDepoKodu = 45;
                    Isemri1.SeriNo = isemri[i].GIRDI2;
                    Isemri1.SeriNo2 = isemridis[i].GENISLIK;
                    double mik = 0;
                    if (isemridis[i].AGIRLIK.Contains('.'))
                    {
                        mik = Double.Parse(isemridis[i].AGIRLIK.Split('.')[0] + isemridis[i].AGIRLIK.Split('.')[1]);
                    }
                    else
                    {
                        mik = Double.Parse(isemridis[i].AGIRLIK);
                    }
                    Isemri1.Miktar = mik;
                    Isemri1.TeslimTarihi = Convert.ToDateTime("2023-12-31");
                    Isemri1.Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                    Isemri1.kayitYeni();
                    NetRS netRS2 = kernel.yeniNetRS(sirket);

                    netRS2.Ac("UPDATE TBLISEMRIREC SET SERINO='" + isemridis[i].GIRDI1 + "',MIKTAR=" + isemridis[i].HAMSARF + ",MIKTARSABITLE='E', DEPO_KODU='45' WHERE ISEMRINO='" + isemridis[i].ISEMRINO + "'");

                }
            }
            catch (Exception ex)
            {

                TempData["Hata"] = "HATA";
                return View("Index");
            }






            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Post1()
        {

            try
            {
                
                Kernel kernel = new Kernel();

                Sirket sirket = default(Sirket);
                SerbestUSK USK = default(SerbestUSK);
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                             "TEST2022",
                                             "TEMELSET",
                                             "",
                                             "nova",
                                             "Efc@+180", 0);
                USK = kernel.yeniSerbestUSK(sirket);
                USK.IsEmrindenGetir("000000000000001");
                USK.OTOSERIURET();
                USK.SeriEkle(USK.SeriOku(0).Seri1, "", "", "", 100, 1);
                USK.FisUret();
                USK.Kaydet();

            }
            catch (Exception ex)
            {

                TempData["Hata"] = "HATA";
                return View("Index");
            }






            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Mail(List<MailModel> mail)
        {
            var m = mail.GroupBy(x => new { x.MUSTERI, x.STOKOLCULERI, x.KALINLIK, x.KALITE, x.KAPLAMA }).Select(grp => new { MUSTERI = grp.First().MUSTERI, STOKOLCULERI = grp.First().STOKOLCULERI, KALINLIK = grp.First().KALINLIK, KALITE = grp.First().KALITE, KAPLAMA = grp.First().KAPLAMA, ADET = grp.Sum(s => int.Parse(s.ADET)), AGIRLIK = grp.Sum(s => int.Parse(s.AGIRLIK)) }).ToList();
            string subject = "";
            List<string> makineler = new List<string>();
            List<string> makinelerref = new List<string>();
            for (int i = 0; i < mail.Count; i++)
            {
                if (!makineler.Contains(mail[i].MAKINE))
                {
                    if (mail[i].MAKINE != "")
                    {
                        makineler.Add(mail[i].MAKINE);
                    }

                }
                if (!makinelerref.Contains(mail[i].MAKINEREF))
                {
                    if (mail[i].MAKINEREF != "")
                    {
                        makinelerref.Add(mail[i].MAKINEREF);
                    }

                }
            }
            for (int i = 0; i < makineler.Count; i++)
            {
                subject = subject + makineler[i] + "-";
            }
            for (int i = 0; i < makinelerref.Count; i++)
            {

                subject = subject + makinelerref[i] + "-";


            }
            var upt = "";
            for (int i = 0; i < subject.Split('-').Length; i++)
            {
                if (subject.Split('-')[i] != "")
                {
                    upt = upt + subject.Split('-')[i] + "-";
                }

            }


            var subject2 = upt.Substring(0, upt.Length - 1) + " İŞ EMRİ - " + Request.Cookies["UserName"].Value;
            string body = "<tr style='outline: thin solid;margin-bottom:15px'><th style='margin-right:10px'>MUSTERI</th><th style='margin-right:10px'>STOKOLCULERI</th><th style='margin-right:10px'>KALINLIK</th><th style='margin-right:10px'>KALITE</th><th style='margin-right:10px'>KAPLAMA</th><th style='margin-right:10px'>ADET</th><th style='margin-right:10px'>AĞIRLIK</th></tr>";
            for (int i = 0; i < m.Count; i++)
            {
                body = body + "<tr style='outline: thin solid;margin-bottom:10px'>" + "<td style='border-collapse: collapse;margin-right:10px' >" + m[i].MUSTERI + "</td>" + "<td style='border-collapse: collapse;margin-right:10px'' >" + m[i].STOKOLCULERI + "</td>" + "<td style='border-collapse: collapse;text-align: center;margin-right:10px''>" + m[i].KALINLIK + "</td>" + "<td style='border-collapse: collapse;text-align: center;margin-right:10px''>" + m[i].KALITE + "</td>" + "<td style='border-collapse: collapse' style='text-align: center;margin-right:10px'' >" + m[i].KAPLAMA + "</td>" + "<td style='border-collapse: collapse;text-align: center;margin-right:10px''>" + String.Format("{0:n0}", m[i].ADET) + "</td>" + "<td style='border-collapse: collapse;text-align: center;margin-right:10px'' >" + String.Format("{0:n0}", m[i].AGIRLIK) + "</td>" + "</tr>";
            }



            WebMail.SmtpServer = "192.168.2.13";
            WebMail.Send(Request.Cookies["Mail"].Value, subject2, "<p>NOVA üzerinde <strong>" + Request.Cookies["UserName"].Value + "</strong> kullanıcısı tarafından oluşturulan iş emirleri aşağıdaki gibidir:</p> </br>" + "<table style='border: 1px solid black;border-collapse: collapse'>" + body + "</table>", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);
            /* WebMail.Send("ugurkonakci@efecegalvaniz.com", subject2, "<p>NOVA üzerinde <strong>" + Request.Cookies["UserName"].Value + "</strong> kullanıcısı tarafından oluşturulan iş emirleri aşağıdaki gibidir:</p> </br>" + "<table style='border: 1px solid black;border-collapse: collapse'>" + body + "</table>", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);*/
            WebMail.Send("ergunozbudakli@efecegalvaniz.com", subject2 + TempData["Hata"], "<p>NOVA üzerinde <strong>" + Request.Cookies["UserName"].Value + "</strong> kullanıcısı tarafından oluşturulan iş emirleri aşağıdaki gibidir:</p> </br>" + "<table style='border: 1px solid black;border-collapse: collapse'>" + body + "</table>", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);
            return RedirectToAction("Index");
        }
        public class HatModel
        {
            public string HAT_KODU { get; set; }
            public int URETIM_TIPI { get; set; }
            public int ISEMRI_TIPI { get; set; }

        }
        public class TRPZModel
        {
            public string ISEMRINO { get; set; }
            public string SERI_NO { get; set; }
            public decimal KULL_MIKTAR { get; set; }

        }
        public class MailModel
        {

            public string MAKINE { get; set; }
            public string MAKINEREF { get; set; }
            public string MUSTERI { get; set; }
            public string STOKOLCULERI { get; set; }
            public string KALINLIK { get; set; }
            public string KAPLAMA { get; set; }
            public string KALITE { get; set; }
            public string ADET { get; set; }
            public string AGIRLIK { get; set; }
        }
        public List<MakKoduModel> GetMak(int id)
        {
            var apiUrl = "http://192.168.2.13:83/api/makine/uretim/" + id;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<MakKoduModel> jsonList = ser.Deserialize<List<MakKoduModel>>(json);

            //END

            return jsonList;
        }
        public List<StokKartlari> GetStokAdlari()
        {
            var apiUrl = "http://192.168.2.13:83/api/stokkartlari";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<StokKartlari> jsonList = ser.Deserialize<List<StokKartlari>>(json);

            //END

            return jsonList;
        }
        public List<MSIPAcik> GetCariler()
        {
            var apiUrl = "http://192.168.2.13:83/api/MSIP";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<MSIPAcik> jsonList = ser.Deserialize<List<MSIPAcik>>(json);

            //END

            return jsonList;
        }
        public List<UretimTakip> Get()
        {
            var apiUrl = "http://192.168.2.13:83/api/uretimtakip/";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<UretimTakip> jsonList = ser.Deserialize<List<UretimTakip>>(json);

            //END

            return jsonList;
        }
        public List<SiparisModel> GetSip()
        {
            var apiUrl = "http://192.168.2.13:83/api/detaylisip/acik";

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
        public List<Cikti_SERI_MODEL> GetSeri()
        {
            var apiUrl = "http://192.168.2.13:83/api/msip/seri";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;
            List<Cikti_SERI_MODEL> jsonList = ser.Deserialize<List<Cikti_SERI_MODEL>>(json);

            //END

            return jsonList;
        }
        public List<IESIRANO> GetMax()
        {
            var apiUrl = "http://192.168.2.13:83/api/isemri/max";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<IESIRANO> jsonList = ser.Deserialize<List<IESIRANO>>(json);

            //END

            return jsonList;
        }

        //        public void IsEmriSend(List<IsEmriModel1> IsEmriModel)
        //        {
        //            Kernel kernel = new Kernel();
        //            Sirket sirket = default(Sirket);
        //            sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
        //                                          "TEST2022",
        //                                          "TEMELSET",
        //                                          "",
        //                                          "erguno",
        //                                          "begum142088", 0);


        //            IsEmri Isemri = default(IsEmri);
        //            Isemri = kernel.yeniIsEmri(sirket);
        //            Isemri.kayitOku(TOkumaTipi.otSon);
        //            Cari cari = default(Cari);
        //            cari = kernel.yeniCari(sirket);

        //            IsEmri Isemri = default(IsEmri);
        //            Isemri = kernel.yeniIsEmri(sirket);
        //            Isemri.kayitOku(TOkumaTipi.otSon);
        //            Cari cari= default(Cari);
        //            cari = kernel.yeniCari(sirket);

        //            CariRiskBilgi risk=default(CariRiskBilgi);
        //            cari.kayitOku(TOkumaTipi.otAc, "");
        //            {
        //                cari.kayitOku(TOkumaTipi.otIlk);
        //                double v=risk.Irsaliye;
        //            };
        //>>>>>>> 583f979c8b4f162108744191180e5cc5f2f95024

        //            //CariRiskBilgi risk=default(CariRiskBilgi);
        //            //cari.kayitOku(TOkumaTipi.otAc, "");
        //            //{
        //            //    cari.kayitOku(TOkumaTipi.otIlk);
        //            //    double v=risk.Irsaliye;
        //            //};

        //            //Isemri.IsEmriNo = IsEmriModel[0].ISEMRINO;
        //            ////Isemri.Tarih = Convert.ToDateTime(isemri.TARIH);
        //            //Isemri.ReceteSaklansin = true;
        //            //Isemri.RezervasyonDurumu = TRezervasyonDurumu.rdYeni;

        //            //Isemri.StokKodu = IsEmriModel[0].STOK_KODU;
        //            //Isemri.Kapali = false;
        //            //Isemri.Miktar = IsEmriModel[0].MIKTAR;
        //            //Isemri.Aciklama = IsEmriModel[0].ADET;
        //            //Isemri.SeriNo2 = IsEmriModel[0].GENISLIK;
        //            //Isemri.DepoKodu =0;
        //            //Isemri.CikisDepoKodu = IsEmriModel[0].DEPOKODU;
        //            //Isemri.ProjeKodu = "1";

        //            //Isemri.kayitYeni();

        //            //var e = IsEmriModel;

        //        }
        public class IsEmriModel1
        {
            public string ISEMRINO { get; set; }
            public string STOK_KODU { get; set; }
            public int MIKTAR { get; set; }
            public string GENISLIK { get; set; }
            public string ADET { get; set; }
            public int DEPOKODU { get; set; }

        }
        public class IsEmriModel
        {
            public string SIPARISNO { get; set; }
            public string GIRDI1 { get; set; }
            public string GIRDI2 { get; set; }
            public string ISEMRINO { get; set; }
            public string STOKADI { get; set; }
            public string KALINLIK { get; set; }

            public string KALITE { get; set; }
            public string KAPLAMA { get; set; }
            public string GENISLIK { get; set; }
            public string ADET { get; set; }

            public string REF_ISEMRINO { get; set; }
            public string REF_STOKOLCUSU { get; set; }
            public string REF_ADET { get; set; }
            public string AGIRLIK { get; set; }
            public Int16 SIPKONT { get; set; }
            public string HAMSARF { get; set; }
            public string FIRE { get; set; }


        }
        public class Thing
        {
            public int Id { get; set; }
            public string Color { get; set; }
        }
        public class SeriModel
        {
            public string BASLANGICKARAKTER { get; set; }
            public int BASLANGICNUMARASI { get; set; }
            public int SONNUMARA { get; set; }
            public int UZUNLUK { get; set; }
        }
        public class USKModel
        {
            
            public string SERI_NO { get; set; }
            public string ISEMRINO { get; set; }
            public double KULL_MIKTAR { get; set; }
            public double MIKTAR2 { get; set; }
        }
    }
}