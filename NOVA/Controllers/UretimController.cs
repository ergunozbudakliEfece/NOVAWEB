using DocumentFormat.OpenXml.Office2010.Excel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using NetOpenX50;
using NOVA.Models;
using QRCoder;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static NOVA.Controllers.IsEmriController;
using ServiceStack.Text;
using System.Diagnostics;
using Microsoft.Win32;
using NOVA.Utils;
using System.Web.Security;
using System.Web.Helpers;

namespace NOVA.Controllers
{
    public class UretimController : Controller
    {
        Kernel kernel = new Kernel();
        IsEmri Isemri = default(IsEmri);
        IsEmri Isemri1 = default(IsEmri);
        Sirket sirket = default(Sirket);
        SerbestUSK uretim = default(SerbestUSK);
        SerbestUSK USK = default(SerbestUSK);
        NetRS netRS = default(NetRS);
        // GET: Uretim
        public async Task<ActionResult> Index()
        {
            int moduleId = 26;

            List<Modules> Modules = await AuthHelper.GetModules(moduleId);

            if (Modules[0].ACTIVE != "1")
            {
                return RedirectToAction("Maintenance", "Home");
            }

            User UserData = await RoleHelper.RoleControl(Request.Cookies["Id"].Value, moduleId);

            if (UserData.SELECT_AUTH != true)
            {
                Session["ModulYetkiMesajı"] = "Modüle yetkiniz bulunmamaktadır";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                bool Logged = await AuthHelper.LoginLog(Request.Cookies["Id"].Value, Request.Cookies["LogId"].Value, moduleId);

                if (!Logged)
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Login", "Login");
                }
            }

            await RoleHelper.CheckRoles(this);
            ViewBag.Makine = GetMak();
            ViewBag.Girdi = GetGirdi();
            ViewBag.Cikti = GetCikti();
            ViewBag.Id = Request.Cookies["Id"].Value;

           
            return View();
        }
        public ActionResult IsEmri()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Post(List<IsEmriModel2> isemri)
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
        public Microsoft.AspNetCore.Mvc.StatusCodeResult IsEmirleriniKapat(List<IsEmriMod> isemrimod)
        {
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                                 "TEST2022",
                                                 "TEMELSET",
                                                 "",
                                                 "nova",
                                                 "Efc@+180", 0);
                NetRS netRS = kernel.yeniNetRS(sirket);
                for (var i = 0; i < isemrimod.Count; i++)
                {
                    netRS.Ac("UPDATE TBLISEMRI SET KAPALI='E' WHERE ISEMRINO='" + isemrimod[i].ISEMRINO + "'");
                }
            }
            catch (Exception)
            {

                return new Microsoft.AspNetCore.Mvc.StatusCodeResult(404);
            }


            return new Microsoft.AspNetCore.Mvc.StatusCodeResult(200);

        }
        public Microsoft.AspNetCore.Mvc.StatusCodeResult IsEmirleriniAc(List<IsEmriMod> isemrimod)
        {
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                                 "TEST2022",
                                                 "TEMELSET",
                                                 "",
                                                 "nova",
                                                 "Efc@+180", 0);
                NetRS netRS = kernel.yeniNetRS(sirket);
                for (var i = 0; i < isemrimod.Count; i++)
                {
                    netRS.Ac("UPDATE TBLISEMRI SET KAPALI='H' WHERE ISEMRINO='" + isemrimod[i].ISEMRINO + "'");
                }
            }
            catch (Exception)
            {

                return new Microsoft.AspNetCore.Mvc.StatusCodeResult(404);
            }


            return new Microsoft.AspNetCore.Mvc.StatusCodeResult(200);

        }
        public Microsoft.AspNetCore.Mvc.StatusCodeResult EksikGuncelle(string seri, int miktar)
        {
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                                 "TEST2022",
                                                 "TEMELSET",
                                                 "",
                                                 "nova",
                                                 "Efc@+180", 0);
                NetRS netRS = kernel.yeniNetRS(sirket);
                var list = GetSarfs(seri, miktar);
                for (var i = 0; i < list.Count; i++)
                {
                    netRS.Ac("UPDATE TBLISEMRIREC SET MIKTAR=" + list[i].SARF + " WHERE ISEMRINO='" + list[i].ISEMRINO + "'");
                }
            }
            catch (Exception)
            {

                return new Microsoft.AspNetCore.Mvc.StatusCodeResult(404);
            }


            return new Microsoft.AspNetCore.Mvc.StatusCodeResult(200);

        }
        public string UretimSonuKaydi(string hatkodu, string stokkodu, string genislik, string mik1, string mik2, bool kontrol, bool etiket)
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
                        netRS = kernel.yeniNetRS(sirket);
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




                        //netRS1.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                        //if (eskimiktar!= jsonList[i].KULL_MIKTAR)
                        //{
                        //    netRS1.Ac("UPDATE TBLISEMRIREC SET MIKTAR=" + (eskimiktar - jsonList[i].KULL_MIKTAR) + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                        //}
                        if (miktarsabitle != "E")
                        {
                            netRS1.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            if (hatkodu != "BK01")
                            {
                                netRS1.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            }
                        }
                        else
                        {
                            netRS1.Ac("SELECT * FROM TBLSERITRA WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            var karsi1 = netRS1.FieldByName("SERI_NO").AsString;
                            netRS1.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                            var referans = netRS1.FieldByName("REFISEMRINO").AsString;
                            netRS1.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + referans + "'");
                            var eski = netRS1.FieldByName("MIKTAR").AsFloat;
                            var oran = jsonList[i].KULL_MIKTAR / eski;
                            var miktar2 = netRS1.FieldByName("ACIKLAMA").AsString;
                            var yeni = miktar2.ToDouble() * oran;
                            netRS1.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi1 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "'");

                            netRS1.Ac("UPDATE TBLISEMRI SET MIKTAR='" + jsonList[i].KULL_MIKTAR + "',ACIKLAMA='" + Math.Round(yeni) + "' WHERE ISEMRINO='" + referans + "'");
                            if (kontrol == true)
                            {
                                netRS1.Ac("UPDATE TBLISEMRI SET KAPALI='E' WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                            }
                        }


                    }

                    //Stok hareketleri gerçeklestiriliyor
                    //uretim.kayitFisNoIleUretimSonu(uretim.UretSon_FisNo, TUretSonDepo.usdAktif,false,false);
                }
                catch (Exception e)
                {
                    var exp = e.Message;
                    System.Diagnostics.Debug.Write(exp);
                    return $"{exp}";
                }


                if (etiket)
                {
                    UretimKaydiSonuBarkodCiktisi(jsonList);
                }
            }
            catch (Exception e)
            {
                var exp = e.Message;
                Console.WriteLine(e.Message);
                return $"{exp}";
            }
            finally
            {
                Marshal.ReleaseComObject(uretim);
                Marshal.ReleaseComObject(sirket);
                kernel.FreeNetsisLibrary();
                Marshal.ReleaseComObject(kernel);
            }
            return $"Başarılı";
        }
        public string TrpzUretim(string stokkodu, string ISEMRINO, string SERI_NO, string KULL_MIKTAR, string mik2)
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
                   
                       
                   
                    

                    netRS = kernel.yeniNetRS(sirket);

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

                    netRS.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + ISEMRINO + "'");

                    var seri = netRS.FieldByName("SERINO").AsString;

                    if (seri == null)
                    {
                        uretim.OTOSERIURET();
                        uretim.SeriEkle(uretim.SeriOku(0).Seri1, "", "", "", KULL_MIKTAR.ToDouble(), mik2.ToDouble());
                    }
                    else
                    {
                        uretim.SeriEkle(seri, "", "", "", KULL_MIKTAR.ToDouble(), mik2.ToDouble());
                    }

                    if (SERI_NO != null && ISEMRINO.Substring(0, 2) != "MH")
                    {
                        netRS.Ac("UPDATE TBLISEMRIREC SET SERINO='" + SERI_NO + "' WHERE ISEMRINO='" + ISEMRINO + "'");
                    }


                    uretim.FisUret();
                    uretim.Kaydet();
                    //netRS.Ac("UPDATE TBLISEMRIREC SET SERINO=NULL WHERE ISEMRINO='" + ISEMRINO + "'");

                    netRS.Ac("SELECT * FROM TBLSERITRA WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
                    var karsi = netRS.FieldByName("SERI_NO").AsString;

                    netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "' AND BELGENO<>'"+ uretim.UretSon_FisNo + "' ORDER BY BELGENO DESC");
                    var mikold = netRS.FieldByName("MIKTAR").AsFloat;
                    netRS.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
                    
                    //if (eskimiktar!= jsonList[i].KULL_MIKTAR)
                    //{
                    //    netRS1.Ac("UPDATE TBLISEMRIREC SET MIKTAR=" + (eskimiktar - jsonList[i].KULL_MIKTAR) + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                    //}

                    netRS.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + ISEMRINO + "'");

                    netRS.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + ISEMRINO + "'");
                    var referans = netRS.FieldByName("REFISEMRINO").AsString;

                    //netRS.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + referans + "'");

                    //var eski = netRS.FieldByName("MIKTAR").AsFloat;
                    //var oran = KULL_MIKTAR.ToDouble() / eski;
                    //var miktar2 = netRS.FieldByName("ACIKLAMA").AsString;
                    //var yeni = miktar2.ToDouble() * oran;
                   
                    netRS.Ac("UPDATE TBLISEMRI SET MIKTAR='" + (mikold != 0 ? mikold + KULL_MIKTAR.ToDouble() : KULL_MIKTAR.ToDouble()) +"' WHERE ISEMRINO='" + referans + "'");
                    
                   

                    if (ISEMRINO.Substring(0, 2) == "MH" || ISEMRINO.Substring(0, 2) == "BK")
                    {
                        Isemri = kernel.yeniIsEmri(sirket);
                        Isemri.kayitOku(TOkumaTipi.otAc, "ISEMRINO = \'" + ISEMRINO + "\'");
                        Isemri.ReceteSaklansin = false;
                        Isemri.Kapali = true;
                        Isemri.kayitDuzelt();
                    }

                }
                catch (Exception exp)
                {
                    sirket.LogOff();
                    WebMail.SmtpServer = "192.168.2.13";
                    WebMail.Send("ergunozbudakli@efecegalvaniz.com,ugurkonakci@efecegalvaniz.com,dincersipka@efecegalvaniz.com", "Dertler Derya Olmuş", ISEMRINO+" sıkıntılı ayağınızı den alın!", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);
                    var message = exp.Message;
                    System.Diagnostics.Debug.Write(exp);
                    return $"Hata: {message}";
                }
                finally
                {
                    sirket.LogOff();
                    Marshal.ReleaseComObject(sirket);
                    kernel.FreeNetsisLibrary();
                    Marshal.ReleaseComObject(kernel);
                }
            }
            else
            {
                try
                {

                    
                        sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                                 "TEST2022",
                                                 "TEMELSET",
                                                 "",
                                                 "nova",
                                                 "Efc@+180", 0);
                   
                    netRS = kernel.yeniNetRS(sirket);
                    Isemri = kernel.yeniIsEmri(sirket);
                    Isemri.kayitOku(TOkumaTipi.otAc, "ISEMRINO = \'" + ISEMRINO + "\'");
                    Isemri.ReceteSaklansin = false;
                    Isemri.Kapali = true;
                    Isemri.kayitDuzelt();
                    netRS.Ac("SELECT TOP(1) * FROM TBLSERITRA WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "' ORDER BY BELGENO DESC");
                    var karsi = netRS.FieldByName("SERI_NO").AsString;
                    netRS.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "' WHERE SIPNO='" + ISEMRINO + "'");
                    netRS.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
                }
                catch (Exception e)
                {
                    sirket.LogOff();
                    return $"Hata: {e.Message}";
                }
                finally
                {
                    sirket.LogOff();
                    Marshal.ReleaseComObject(sirket);
                    kernel.FreeNetsisLibrary();
                    Marshal.ReleaseComObject(kernel);
                }

            }

            return $"Başarılı";
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
                    netRS2.Ac("UPDATE TBLISEMRIREC SET MIKTAR=1,MIKTARSABITLE='H', DEPO_KODU='45' WHERE ISEMRINO='" + isemri[i].ISEMRINO + "'");

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
        #region BarkodPDF
        public void UretimKaydiSonuBarkodCiktisi(List<USKModel> Data)
        {
            string imagepath = Server.MapPath("~\\DesignOutput\\Sevkiyat\\Content");
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A6, 10f, 10f, 10f, 10f);

            string pdfPath = Path.Combine(Server.MapPath("~\\DesignOutput\\Uretim\\UretimSonuKaydi"), $"{ DateTime.UtcNow.ToUnixTime()}.pdf");

            FileStream Memory = new FileStream(pdfPath, FileMode.Create);
            //MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, Memory);

            document.Open();

            for (int i = 0; i < Data.Count; i++)
            {
                document.NewPage();
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imagepath + "/SevkiyatDesign.png");
                png.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
                png.Alignment = iTextSharp.text.Image.UNDERLYING;
                png.SetAbsolutePosition(0, 0);
                document.Add(png);

                PdfContentByte cb = writer.DirectContent;

                iTextSharp.text.Font fontNormal = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 9, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font fontBoldHeader = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 10, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font fontBoldContent = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 9, iTextSharp.text.Font.BOLD);

                ColumnText Header = new ColumnText(cb);
                Header.SetSimpleColumn(45, 125, 270, 335);
                Header.AddElement(new iTextSharp.text.Paragraph(Data[i].SERI_NO) { Alignment = Element.ALIGN_CENTER, Font = fontBoldHeader });
                Header.Go();

                ColumnText Content = new ColumnText(cb) { Alignment = Element.ALIGN_CENTER };
                Content.SetSimpleColumn(35, 60, 280, 260);

                //iTextSharp.text.Paragraph Miktar1 = new iTextSharp.text.Paragraph("MİKTAR 1      ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                //Miktar1.Add(new Chunk($": {BosDegerKontrolu(Data[i].MIKTAR)} {BosDegerKontrolu(Data[i].OLCU_BR1)}", fontNormal));
                //Content.AddElement(Miktar1);


                //if (Data[i].OLCU_BR1 != Data[i].OLCU_BR2)
                //{
                //    iTextSharp.text.Paragraph Miktar2 = new iTextSharp.text.Paragraph("MİKTAR 2      ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                //    Miktar2.Add(new Chunk($": {BosDegerKontrolu(Data[i].MIKTAR2)} {BosDegerKontrolu(Data[i].OLCU_BR2)}", fontNormal));
                //    Content.AddElement(Miktar2);

                //    iTextSharp.text.Paragraph BirimMiktar = new iTextSharp.text.Paragraph("BİRİM MİKTAR  ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                //    BirimMiktar.Add(new Chunk($": {BosDegerKontrolu(Data[i].BIRIM_MIKTAR)} {BosDegerKontrolu(Data[i].OLCU_BR1)}/{BosDegerKontrolu(Data[i].OLCU_BR2)}", fontNormal));
                //    Content.AddElement(BirimMiktar);
                //}
                //else
                //{
                //    iTextSharp.text.Paragraph Miktar2 = new iTextSharp.text.Paragraph("MİKTAR 2      ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                //    Miktar2.Add(new Chunk($": -", fontNormal));
                //    Content.AddElement(Miktar2);

                //    iTextSharp.text.Paragraph BirimMiktar = new iTextSharp.text.Paragraph("BİRİM MİKTAR  ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                //    BirimMiktar.Add(new Chunk($": -", fontNormal));
                //    Content.AddElement(BirimMiktar);
                //}

                iTextSharp.text.Paragraph Kalinlik = new iTextSharp.text.Paragraph("KALINLIK      ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Kalinlik.Add(new Chunk($": {BosDegerKontrolu(Data[i].ISEMRINO)}", fontNormal));
                Content.AddElement(Kalinlik);

                iTextSharp.text.Paragraph Genislik = new iTextSharp.text.Paragraph("GENİŞLİK      ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Genislik.Add(new Chunk($": {BosDegerKontrolu(Data[i].KULL_MIKTAR.ToString())}", fontNormal));
                Content.AddElement(Genislik);

                iTextSharp.text.Paragraph Kalite = new iTextSharp.text.Paragraph("KALİTE        ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Kalite.Add(new Chunk($": {BosDegerKontrolu(Data[i].MIKTAR2.ToString())}", fontNormal));
                Content.AddElement(Kalite);
                Content.Go();

                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(Data[i].SERI_NO, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                System.Drawing.Image qrCodeImage = qrCode.GetGraphic(45, System.Drawing.Color.Black, System.Drawing.Color.Transparent, true);

                iTextSharp.text.Image QR = iTextSharp.text.Image.GetInstance(ImageToByteArray(qrCodeImage));
                QR.ScaleToFit(75, 75);
                QR.Alignment = iTextSharp.text.Image.UNDERLYING;
                QR.SetAbsolutePosition(195, 12);
                document.Add(QR);
            }

            document.Close();
            Memory.Close();

            //PrintHelper.Print(pdfPath, "Microsoft Print to PDF");
        }
        private byte[] ImageToByteArray(System.Drawing.Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
        private string BosDegerKontrolu(string Deger)
        {
            return string.IsNullOrEmpty(Deger) ? "-" : Deger;
        }
        #endregion
        public List<HatModel> UretimTipi(string hatKodu)
        {

            var apiUrl2 = "http://192.168.2.13:83/api/ie/hatprm/" + hatKodu;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            //Connect API
            Uri url2 = new Uri(apiUrl2);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            var json2 = client.DownloadString(url2);
            List<HatModel> jsonList = ser.Deserialize<List<HatModel>>(json2);

            return jsonList;
        }
        public List<MakKoduModel> GetMak()
        {
            var apiUrl = "http://192.168.2.13:83/api/makine";

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
        public List<uretimGirdiModel> GetGirdi()
        {
            var apiUrl = "http://192.168.2.13:83/api/uretimbekleyen/vw";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<uretimGirdiModel> jsonList = ser.Deserialize<List<uretimGirdiModel>>(json);

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
            List<User> jsonList = ser.Deserialize<List<User>>(json);
            return jsonList;
        }
        public List<SarfModel> GetSarfs(string seri, int miktar)
        {
            var apiUrl = "http://192.168.2.13:83/api/ie/SARF/" + seri + "/" + miktar;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SarfModel> jsonList = ser.Deserialize<List<SarfModel>>(json);

            //END

            return jsonList;
        }

        public List<UretimBekleyenCiktiModel> GetCikti()
        {


            var apiUrl = "http://192.168.2.13:83/api/uretbeklcikti/vw";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<UretimBekleyenCiktiModel> jsonList = ser.Deserialize<List<UretimBekleyenCiktiModel>>(json);

            //END

            return jsonList;
        }
        [HttpPost]
        public async Task<ActionResult> UretGirPost([Bind(Prefix = "Item1")] UretGir user)
        {
            string x = "";

            string apiUrl;
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;
            string apiResponse;


            apiUrl = "http://192.168.2.13:83/api/uretimgirdi/" + user.INCKEY;



            request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(user), Encoding.UTF8, "application/json")
            };

            response = await httpClient.SendAsync(request);

            apiResponse = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                x = "Başarılı";
            }
            else
            {
                x = "Başarısız";
            }

            ViewBag.Makine = GetMak();
            ViewBag.Girdi = GetGirdi();
            ViewBag.Cikti = GetCikti();
            ViewBag.Id = Request.Cookies["Id"].Value;
            ViewBag.Message = x;
            TempData["Başarı"] = x;

            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<ActionResult> UretCiktiPost([Bind(Prefix = "Item2")] UretCikti user)
        {
            string x = "";

            string apiUrl;
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;
            string apiResponse;


            apiUrl = "http://192.168.2.13:83/api/uretimcikti/" + user.INCKEY;



            request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(user), Encoding.UTF8, "application/json")
            };

            response = await httpClient.SendAsync(request);

            apiResponse = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                x = "Başarılı";
            }
            else
            {
                x = "Başarısız";
            }




            ViewBag.Makine = GetMak();
            ViewBag.Girdi = GetGirdi();
            ViewBag.Cikti = GetCikti();
            ViewBag.Id = Request.Cookies["Id"].Value;
            ViewBag.Message = x;
            TempData["Başarı"] = x;
            return RedirectToAction("Index");

        }
        public List<Personel> GetPersonelById(int id)
        {
            var apiUrl = "http://192.168.2.13:83/api/personel/id:" + id;

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
        public class SarfModel
        {
            public string ISEMRINO { get; set; }
            public string SARF { get; set; }
            public string HES_SARF { get; set; }
        }

    }
}