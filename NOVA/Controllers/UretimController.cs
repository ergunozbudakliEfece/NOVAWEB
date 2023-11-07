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
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml;
using System.Globalization;
using Newtonsoft.Json;
using System.Runtime.ConstrainedExecution;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;

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
        Fatura fatura = default(Fatura);
        FatUst fatUst = default(FatUst);
        FatKalem fatKalem = default(FatKalem);
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
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);

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
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);
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
        public Microsoft.AspNetCore.Mvc.StatusCodeResult IsEmriniAc(string isemri)
        {
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                            "TEST2022",
                                            "TEMELSET",
                                            "",
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);
                NetRS netRS = kernel.yeniNetRS(sirket);
                netRS.Ac("UPDATE TBLISEMRI SET KAPALI='H' WHERE ISEMRINO='" + isemri + "'");
                
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
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);
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
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);
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
        public string UretimSonuKaydi(string hatkodu, string stokkodu, string genislik, string mik1, string mik2, bool kontrol, bool etiket, bool onizleme)
        {
            var uretimTipi = UretimTipi(hatkodu)[0].URETIM_TIPI;
            var stokadi = GetStokAdlari().FirstOrDefault(x => x.STOK_KODU == stokkodu);
            double f = 0;
          
            var Etiket = "";
            try
            {


                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                            "TEST2022",
                                            "TEMELSET",
                                            "",
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);

                List<string> SeriNoListesi = new List<string>();
                

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
                        var recmik = netRS.FieldByName("MIKTAR").AsFloat;
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
                        var ACIK1 ="";
                        var ACIK2 = "";
                        var SERI_NO_3 = "";
                        var SERI_NO_4 = "";
                        if (seri == null)
                        {
                            uretim.OTOSERIURET();
                            uretim.SeriEkle(uretim.SeriOku(0).Seri1, "", "", "", jsonList[i].KULL_MIKTAR, jsonList[i].MIKTAR2);
                            seri = uretim.SeriOku(0).Seri1;
                        }
                        uretim.FisUret();
                        uretim.Kaydet();
                        NetRS netRS1 = kernel.yeniNetRS(sirket);
                        f = recmik - jsonList[i].KULL_MIKTAR;
                        if (f != 0 && hatkodu == "DL01")
                        {
                            
                            fatura = kernel.yeniFatura(sirket, TFaturaTip.ftAmbarG);

                            fatUst = fatura.Ust();
                            fatUst.FATIRS_NO = fatura.YeniNumara("A");
                            fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                            fatUst.CikisYeri = TCikisYeri.cySerbest;
                            fatUst.CariKod = "12035200100406";
                            fatUst.Tarih = DateTime.Now;
                            fatUst.ENTEGRE_TRH = DateTime.Now;
                            fatUst.FiiliTarih = DateTime.Now;
                            fatUst.Proje_Kodu = "1";
                            fatUst.Aciklama = jsonList[i].ISEMRINO.Substring(0, 4);
                            fatKalem = fatura.kalemYeni("HURDA");
                            fatKalem.DEPO_KODU = 60;
                            fatKalem.Olcubr = 1;
                            fatKalem.ProjeKodu= "1";
                            fatKalem.D_YEDEK10 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")); 
                            fatKalem.STra_GCMIK = f;
                            fatKalem.STra_NF = 0;
                            fatKalem.STra_BF = 0;
                            fatKalem.STra_ACIK = jsonList[i].ISEMRINO;

                            fatura.kayitYeni();
                            Marshal.ReleaseComObject(fatura);
                            Marshal.ReleaseComObject(fatUst);
                            Marshal.ReleaseComObject(fatKalem);
                        }
                        //if (miktarsabitle == "E")
                        //{
                        //    netRS.Ac("UPDATE TBLISEMRI SET MIKTAR=" + eskimiktar + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                        //}

                        //netRS1.Ac("UPDATE TBLSERITRA SET MIKTAR2=" + jsonList[i].MIKTAR2 + " WHERE  GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                        netRS1.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                        if (seri == null)
                        {
                            seri = netRS1.FieldByName("SERI_NO").AsString;
                        }
                       
                        if (i == 0)
                        {
                            karsi = netRS1.FieldByName("SERI_NO").AsString;
                        }
                        netRS1.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='C' AND SIPNO='" + jsonList[i].ISEMRINO + "'");

                        var seri_no = netRS1.FieldByName("SERI_NO").AsString;
                        netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE SERI_NO='" + seri_no + "' AND GCKOD='G'");
                        ACIK1 = netRS.FieldByName("ACIK1").AsString;
                        ACIK2 = netRS.FieldByName("ACIK2").AsString;
                        SERI_NO_3 = netRS.FieldByName("SERI_NO_3").AsString;
                        SERI_NO_4 = netRS.FieldByName("SERI_NO_4").AsString;

                        //netRS1.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                        //if (eskimiktar!= jsonList[i].KULL_MIKTAR)
                        //{
                        //    netRS1.Ac("UPDATE TBLISEMRIREC SET MIKTAR=" + (eskimiktar - jsonList[i].KULL_MIKTAR) + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                        //}
                        if (miktarsabitle != "E")
                        {
                            netRS1.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            netRS1.Ac("UPDATE TBLSERITRA SET ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "' AND GCKOD='G'");
                            if (hatkodu != "BK01")
                            {
                                netRS1.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                                netRS1.Ac("UPDATE TBLSERITRA SET ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            }
                        }
                        else
                        {
                            netRS1.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            var karsi1 = netRS1.FieldByName("SERI_NO").AsString;
                            netRS1.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                            var referans = netRS1.FieldByName("REFISEMRINO").AsString;
                            netRS1.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + referans + "'");
                            var eski = netRS1.FieldByName("MIKTAR").AsFloat;
                            var oran = jsonList[i].KULL_MIKTAR / eski;
                            var miktar2 = netRS1.FieldByName("ACIKLAMA").AsString;
                            var yeni = miktar2.ToDouble() * oran;
                            netRS1.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi1 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            netRS1.Ac("UPDATE TBLSERITRA SET MIKTAR2='1' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "' AND GCKOD='C'");
                            netRS1.Ac("UPDATE TBLSERITRA SET ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "' AND GCKOD='G'");
                            netRS1.Ac("UPDATE TBLISEMRI SET MIKTAR='" + jsonList[i].KULL_MIKTAR + "',ACIKLAMA='" + Math.Round(yeni) + "' WHERE ISEMRINO='" + referans + "'");
                            if (kontrol == true)
                            {
                                netRS1.Ac("UPDATE TBLISEMRI SET KAPALI='E' WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                            }
                        }

                        SeriNoListesi.Add(seri);
                    }

                    //Stok hareketleri gerçeklestiriliyor
                    //uretim.kayitFisNoIleUretimSonu(uretim.UretSon_FisNo, TUretSonDepo.usdAktif,false,false);
                }
                catch (Exception e)
                {
                    var exp = e.Message;
                    System.Diagnostics.Debug.Write(exp);
                    return $"Hata: {exp}";
                }
                

                if (etiket)
                {
                    List<BarkodModel> Etiketler = new List<BarkodModel>();
                    WebClient Client = new WebClient() { Encoding= Encoding.UTF8 };

                    foreach (var item in SeriNoListesi)
                    {
                        string Response = Client.DownloadString(new Uri("http://192.168.2.13:83/api/seri/kontrol/" + item));
                        List<BarkodModel> Result = ser.Deserialize<List<BarkodModel>>(Response);

                        if(Result.Count > 0) 
                        {
                            Etiketler.Add(Result[0]);
                        }
                    }

                    Etiket = UretimKaydiSonuBarkodCiktisi(Etiketler, onizleme);
                }
            }
            catch (Exception e)
            {
                var exp = e.Message;
                Console.WriteLine(e.Message);
                //return $"Hata: {exp}";
            }
            finally
            {
                sirket.LogOff();
                Marshal.ReleaseComObject(uretim);
                Marshal.ReleaseComObject(sirket);
                kernel.FreeNetsisLibrary();
                Marshal.ReleaseComObject(kernel);
            }

            return Etiket;
        }
        public string Hurda(string hatkodu, string mik1, string stokkodu)
        {
            Kernel kernel1 = new Kernel();
            NetRS netRS1 = default(NetRS);
            Sirket sirket1 = kernel1.yeniSirket(TVTTipi.vtMSSQL,
                                           "TEST2022",
                                           "TEMELSET",
                                           "",
                                           Request.Cookies["UserName"].Value,
                                           LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);

            try
            {
                var stoklar = GetStokAdlari();
                var stokadi = stoklar.Find(x => x.STOK_KODU == stokkodu).STOK_ADI;
                netRS1 = kernel1.yeniNetRS(sirket1);
                var a=netRS1.Ac("SELECT MAX(FISNO) AS FISNO FROM TEST2022.dbo.TBLSTHAR WHERE FISNO LIKE 'Z%'");
                
                var f = netRS1.FieldByName("FISNO").AsString;
               
                var fisno = (f.Substring(1, f.Count() - 1).ToInt() + 1).ToString().PadLeft(14, '0');
               
                StokHareket stok = kernel.yeniStokHareket(sirket1);
                stok.Stok_Kodu = stokkodu;
                stok.Sthar_Aciklama = "HURDA";
                stok.Fisno = "Z" + fisno;
                stok.Sthar_Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                stok.Proje_Kodu = "1";
                stok.Sthar_Gckod = "C";
                stok.Sthar_Bf = 0;
                stok.Sthar_Nf = 0;
                stok.Sthar_Gcmik = mik1.ToDouble();
                stok.Sthar_Gcmik2 = 0;
                stok.DEPO_KODU = 45;
                stok.Sthar_Htur = "A";
                stok.kayitYeni();
                StokHareket stok2 = kernel.yeniStokHareket(sirket1);
                stok2.Stok_Kodu = "HURDA";
                stok2.Sthar_Aciklama = stokadi;
                stok2.Fisno = "Z" + fisno;
                stok2.Sthar_Bf = 0;
                stok2.Sthar_Nf = 0;
                stok2.Sthar_Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                stok2.Proje_Kodu = "1";
                stok2.Sthar_Gckod = "G";
                stok2.Sthar_Gcmik = stok.Sthar_Gcmik;
                stok2.Sthar_Gcmik2 = 0;
                stok2.DEPO_KODU = 45;
                stok2.Sthar_Htur = "A";
                stok2.kayitYeni();
            }
            catch (Exception e)
            {

                return e.Message;
            }
            finally
            {
                sirket1.LogOff();
            }
           
            return "BAŞARILI";
        }
        public class HurdaModel
        {
            public string INCKEY { get; set; }
            public string STOK_KODU { get; set; }
            public string SERI_NO { get; set; }
            public string HAT_KODU { get; set; }
            public string MIKTAR { get; set; }
            public string MIKTAR2 { get; set; }
        }
        public string IkinciKalite(string hatkodu, string stokkodu, string genislik, string mik1, string mik2,string stokadi,string tip)
        {
            var uretimTipi = UretimTipi(hatkodu)[0].URETIM_TIPI;
            var Etiket = "";
            try
            {

                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                            "TEST2022",
                                            "TEMELSET",
                                            "",
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);

                List<string> SeriNoListesi = new List<string>();


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
                        var ACIK1 = "";
                        var ACIK2 = "";
                        var SERI_NO_3 = "";
                        var SERI_NO_4 = "";
                        if (seri == null)
                        {
                            uretim.OTOSERIURET();
                            uretim.SeriEkle(uretim.SeriOku(0).Seri1, "", "", "", jsonList[i].KULL_MIKTAR, jsonList[i].MIKTAR2);
                            seri = uretim.SeriOku(0).Seri1;
                        }

                        NetRS netRS1 = kernel.yeniNetRS(sirket);
                        uretim.FisUret();
                        uretim.Kaydet();

                        //if (miktarsabitle == "E")
                        //{
                        //    netRS.Ac("UPDATE TBLISEMRI SET MIKTAR=" + eskimiktar + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                        //}

                        //netRS1.Ac("UPDATE TBLSERITRA SET MIKTAR2=" + jsonList[i].MIKTAR2 + " WHERE  GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                        netRS1.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                        if (seri == null)
                        {
                            seri = netRS1.FieldByName("SERI_NO").AsString;
                        }

                        if (i == 0)
                        {
                            karsi = netRS1.FieldByName("SERI_NO").AsString;
                        }
                        netRS1.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='C' AND SIPNO='" + jsonList[i].ISEMRINO + "'");

                        var seri_no = netRS1.FieldByName("SERI_NO").AsString;
                        netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE SERI_NO='" + seri_no + "' AND GCKOD='G'");
                        ACIK1 = netRS.FieldByName("ACIK1").AsString;
                        ACIK2 = netRS.FieldByName("ACIK2").AsString;
                        SERI_NO_3 = netRS.FieldByName("SERI_NO_3").AsString;
                        SERI_NO_4 = netRS.FieldByName("SERI_NO_4").AsString;

                        //netRS1.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                        //if (eskimiktar!= jsonList[i].KULL_MIKTAR)
                        //{
                        //    netRS1.Ac("UPDATE TBLISEMRIREC SET MIKTAR=" + (eskimiktar - jsonList[i].KULL_MIKTAR) + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                        //}
                        if (miktarsabitle != "E")
                        {
                            netRS1.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            netRS1.Ac("UPDATE TBLSERITRA SET ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "' AND GCKOD='G'");
                            if (hatkodu != "BK01")
                            {
                                netRS1.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                                netRS1.Ac("UPDATE TBLSERITRA SET ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            }
                        }
                        else
                        {
                            netRS1.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            var karsi1 = netRS1.FieldByName("SERI_NO").AsString;
                            netRS1.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                            var referans = netRS1.FieldByName("REFISEMRINO").AsString;
                            netRS1.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + referans + "'");
                            var eski = netRS1.FieldByName("MIKTAR").AsFloat;
                            var oran = jsonList[i].KULL_MIKTAR / eski;
                            var miktar2 = netRS1.FieldByName("ACIKLAMA").AsString;
                            var yeni = miktar2.ToDouble() * oran;
                            netRS1.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi1 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            netRS1.Ac("UPDATE TBLSERITRA SET MIKTAR2='1' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "' AND GCKOD='C'");
                            netRS1.Ac("UPDATE TBLSERITRA SET ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "' AND GCKOD='G'");
                            netRS1.Ac("UPDATE TBLISEMRI SET MIKTAR='" + jsonList[i].KULL_MIKTAR + "',ACIKLAMA='" + Math.Round(yeni) + "' WHERE ISEMRINO='" + referans + "'");
                            
                        }
                        netRS.Ac("SELECT MAX(FISNO) AS FISNO FROM TEST2022..TBLSTHAR");
                        var fisno= (netRS.FieldByName("FISNO").AsString.Substring(1, netRS.FieldByName("FISNO").AsString.Count()-1).ToInt()+1).ToString().PadLeft(14,'0');
                        var fisno2 = (netRS.FieldByName("FISNO").AsString.Substring(1, netRS.FieldByName("FISNO").AsString.Count()-1).ToInt() + 2).ToString().PadLeft(14, '0');
                        netRS.Ac("SELECT * FROM TEST2022..TBLSTHAR WHERE FISNO='"+ uretim.UretSon_FisNo + "'");
                        var fiyat = netRS.FieldByName("STHAR_BF").AsString;
                        SeriNoListesi.Add(seri);
                        StokHareket stok = kernel.yeniStokHareket(sirket);
                        stok.Stok_Kodu = uretim.UretSon_Mamul;
                        stok.Sthar_Aciklama = stokadi + "-" +uretim.UretSon_FisNo + "-" + jsonList[i].ISEMRINO.Substring(0, 4);
                        stok.Fisno ="Z"+fisno;
                        stok.Sthar_Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                        stok.Proje_Kodu = "1";
                        stok.Sthar_Gckod = "C";
                        stok.Sthar_Bf = fiyat.ToDouble();
                        stok.Sthar_Nf = fiyat.ToDouble();
                        stok.Sthar_Gcmik = jsonList[i].KULL_MIKTAR;
                        stok.Sthar_Gcmik2= jsonList[i].MIKTAR2;
                        stok.DEPO_KODU = 45;
                        stok.Sthar_Htur = "A";
                        stok.kayitYeni();
                        StokHareket stok2 = kernel.yeniStokHareket(sirket);
                        stok2.Stok_Kodu = tip=="ikinci"?"IKINCIKALITE":"HURDA";
                        stok2.Sthar_Aciklama = stokadi+"-" + uretim.UretSon_FisNo + "-" + jsonList[i].ISEMRINO.Substring(0, 4);
                        stok2.Fisno = "Z" + fisno2;
                        stok2.Sthar_Bf = fiyat.ToDouble();
                        stok2.Sthar_Nf = fiyat.ToDouble();
                        stok2.Sthar_Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                        stok2.Proje_Kodu = "1";
                        stok2.Sthar_Gckod = "G";
                        stok2.Sthar_Gcmik = stok.Sthar_Gcmik;
                        stok2.Sthar_Gcmik2 = jsonList[i].MIKTAR2;
                        stok2.DEPO_KODU = 45;
                        stok2.Sthar_Htur = "A";
                        stok2.kayitYeni();


                    }

                    //Stok hareketleri gerçeklestiriliyor
                    //uretim.kayitFisNoIleUretimSonu(uretim.UretSon_FisNo, TUretSonDepo.usdAktif,false,false);
                }
                catch (Exception e)
                {
                    var exp = e.Message;
                    System.Diagnostics.Debug.Write(exp);
                    //return $"Hata: {exp}";
                }

            }
            catch (Exception e)
            {
                var exp = e.Message;
                Console.WriteLine(e.Message);
                //return $"Hata: {exp}";
            }
            finally
            {
                sirket.LogOff();
                Marshal.ReleaseComObject(uretim);
                Marshal.ReleaseComObject(sirket);
                kernel.FreeNetsisLibrary();
                Marshal.ReleaseComObject(kernel);
            }

            return "BAŞARILI";
        }
        public string TrpzUretim(string stokkodu, string ISEMRINO, string SERI_NO, string KULL_MIKTAR, string mik2,bool kontrol,string hurdamik, string ikincimik1, string ikincimik2,int dongu,bool sonMu)
        {
            var seri = "";
            if (KULL_MIKTAR != "0")
            {
                try
                {


                    sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                            "TEST2022",
                                            "TEMELSET",
                                            "",
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);





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

                    seri = netRS.FieldByName("SERINO").AsString;
                    var Stokkodu = netRS.FieldByName("STOK_KODU").AsString;

                    NetRS SeriNoTakipRS = kernel.yeniNetRS(sirket);

                    SeriNoTakipRS.Ac($"SELECT * FROM TBLSTSABIT WHERE STOK_KODU='{Stokkodu}'");

                    var SeriNoTakip = SeriNoTakipRS.FieldByName("GIRIS_SERI").AsString;

                    if (SeriNoTakip == "E" && dongu == 0)
                    {
                        if (seri == null )
                        { 
                            uretim.OTOSERIURET();
                            uretim.SeriEkle(uretim.SeriOku(0).Seri1, "", "", "", KULL_MIKTAR.ToDouble(), mik2.ToDouble());
                        }
                        else
                        {
                            uretim.SeriEkle(seri, "", "", "", KULL_MIKTAR.ToDouble(), mik2.ToDouble());
                        }
                    }
                    else
                    {
                        netRS.Ac("SELECT TOP(1) * FROM TBLSERITRA WHERE GCKOD='G' AND STOK_KODU='" + Stokkodu + "' ORDER BY BELGENO DESC");
                        var s = netRS.FieldByName("SERI_NO").AsString;
                        uretim.SeriEkle(s, "", "", "", KULL_MIKTAR.ToDouble(), mik2.ToDouble());
                    }

                    
                    if (SERI_NO != null && ISEMRINO.Substring(0, 2) != "MH")
                    {
                        netRS.Ac("UPDATE TBLISEMRIREC SET SERINO='" + SERI_NO + "' WHERE ISEMRINO='" + ISEMRINO + "'");
                    }
                    if (ISEMRINO == stokkodu + "23000001001" || ISEMRINO == stokkodu+"23000001002")
                    {
                        netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE GCKOD='G' AND SERI_NO='" + SERI_NO + "'");
                        var seritraStok= netRS.FieldByName("STOK_KODU").AsString;
                        netRS.Ac("UPDATE TBLISEMRIREC SET HAM_KODU='"+seritraStok+"' WHERE ISEMRINO='" + ISEMRINO + "'");
                        netRS.Ac("UPDATE TBLISEMRI SET KAPALI='H' WHERE ISEMRINO='" + ISEMRINO + "'");
                    }
                    
                    uretim.FisUret();
                    uretim.Kaydet();
                    if(ISEMRINO== stokkodu + "23000001001" || ISEMRINO == stokkodu + "23000001002")
                    {
                        netRS.Ac("UPDATE TBLISEMRIREC SET HAM_KODU='HURDA',SERINO=NULL WHERE ISEMRINO='" + ISEMRINO + "'");
                        netRS.Ac("UPDATE TBLISEMRI SET KAPALI='E' WHERE ISEMRINO='" + ISEMRINO + "'");
                    }
                   

                    netRS.Ac("SELECT * FROM TBLSERITRA WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
                    var karsi = netRS.FieldByName("SERI_NO").AsString;

                    netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "' AND BELGENO<>'" + uretim.UretSon_FisNo + "' ORDER BY BELGENO DESC");
                    var mikold = netRS.FieldByName("MIKTAR").AsFloat;
                    netRS.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + ISEMRINO + "'");

                    //if (eskimiktar!= jsonList[i].KULL_MIKTAR)
                    //{
                    //    netRS1.Ac("UPDATE TBLISEMRIREC SET MIKTAR=" + (eskimiktar - jsonList[i].KULL_MIKTAR) + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                    //}
                    netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE SERI_NO='" + SERI_NO + "' AND GCKOD='G'");
                    var ACIK1 = netRS.FieldByName("ACIK1").AsString;
                    var ACIK2 = netRS.FieldByName("ACIK2").AsString;
                    var SERI_NO_3 = netRS.FieldByName("SERI_NO_3").AsString;
                    var SERI_NO_4 = netRS.FieldByName("SERI_NO_4").AsString;
                    netRS.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "',ACIK1='"+ACIK1+"',ACIK2='"+ACIK2+ "',SERI_NO_3='"+ SERI_NO_3 + "',SERI_NO_4='"+ SERI_NO_4 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + ISEMRINO + "'");

                    netRS.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + ISEMRINO + "'");
                    var referans = netRS.FieldByName("REFISEMRINO").AsString;

                    //netRS.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + referans + "'");

                    //var eski = netRS.FieldByName("MIKTAR").AsFloat;
                    //var oran = KULL_MIKTAR.ToDouble() / eski;
                    //var miktar2 = netRS.FieldByName("ACIKLAMA").AsString;
                    //var yeni = miktar2.ToDouble() * oran;

                    netRS.Ac("UPDATE TBLISEMRI SET MIKTAR='" + (mikold != 0 ? mikold + KULL_MIKTAR.ToDouble() : KULL_MIKTAR.ToDouble()) + "' WHERE ISEMRINO='" + referans + "'");



                    if (ISEMRINO.Substring(0, 2) == "MH" || ISEMRINO.Substring(0, 2) == "BK")
                    {
                        Isemri = kernel.yeniIsEmri(sirket);
                        Isemri.kayitOku(TOkumaTipi.otAc, "ISEMRINO = \'" + ISEMRINO + "\'");
                        Isemri.ReceteSaklansin = false;
                        Isemri.Kapali = true;
                        Isemri.kayitDuzelt();
                    }
                    netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
                    seri = netRS.FieldByName("SERI_NO").AsString;

                    if (sonMu == true)
                    {
                        if (kontrol)
                        {
                            Isemri = kernel.yeniIsEmri(sirket);
                            Isemri.kayitOku(TOkumaTipi.otAc, "ISEMRINO = \'" + ISEMRINO + "\'");
                            Isemri.ReceteSaklansin = false;
                            Isemri.Kapali = true;
                            Isemri.kayitDuzelt();
                        }
                        netRS.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + ISEMRINO + "'");

                        seri = netRS.FieldByName("SERINO").AsString;
                        var Stokkoduu = netRS.FieldByName("STOK_KODU").AsString;
                        netRS.Ac("SELECT TOP(1) * FROM TBLSERITRA WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "' ORDER BY BELGENO DESC");
                        var karsii = netRS.FieldByName("SERI_NO").AsString;

                        Fatura fatura = default(Fatura);
                        FatUst fatUst = default(FatUst);
                        FatKalem fatKalem = default(FatKalem);
                        if (hurdamik != null && hurdamik != "0")
                        {
                            fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);

                            fatUst = fatura.Ust();
                            fatUst.FATIRS_NO = fatura.YeniNumara("E");
                            fatUst.CariKod = "12035200100406";
                            fatUst.TIPI = TFaturaTipi.ft_Bos;
                            fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                            fatUst.Tarih = DateTime.Now;
                            fatUst.FiiliTarih = DateTime.Now;
                            fatUst.PLA_KODU = "45";
                            fatUst.Proje_Kodu = "1";
                            fatUst.KDV_DAHILMI = true;
                            fatUst.Aciklama = ISEMRINO;
                            fatKalem = fatura.kalemYeni(Stokkoduu);

                            ///Giriş Depo Kodu
                            fatKalem.Gir_Depo_Kodu = 60;
                            fatKalem.DEPO_KODU = 45;
                            fatKalem.STra_GCMIK = hurdamik.ToDouble();
                            fatKalem.STra_BF = 0;
                            fatKalem.STra_ACIK = ISEMRINO.Substring(0, 4);
                            fatKalem.Olcubr = 1;
                            fatKalem.ProjeKodu = "1";
                            fatKalem.D_YEDEK10 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

                            fatKalem.SeriEkle(karsii, "", "", "", hurdamik.ToDouble(), 0);
                            fatura.kayitYeni();
                        }
                        if (ikincimik1 != null && ikincimik1 != "0")
                        {
                            fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);

                            fatUst = fatura.Ust();
                            fatUst.FATIRS_NO = fatura.YeniNumara("E");
                            fatUst.CariKod = "12035200100406";
                            fatUst.TIPI = TFaturaTipi.ft_Bos;
                            fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                            fatUst.Tarih = DateTime.Now;
                            fatUst.FiiliTarih = DateTime.Now;
                            fatUst.PLA_KODU = "45";
                            fatUst.Proje_Kodu = "1";
                            fatUst.Aciklama = ISEMRINO.Substring(0, 4);
                            fatUst.KDV_DAHILMI = true;
                            fatKalem = fatura.kalemYeni(Stokkoduu);

                            ///Giriş Depo Kodu
                            fatKalem.Gir_Depo_Kodu = 55;
                            fatKalem.DEPO_KODU = 45;
                            fatKalem.STra_GCMIK = ikincimik1.ToDouble();
                            fatKalem.STra_GCMIK2 = ikincimik2.ToDouble();
                            fatKalem.STra_ACIK = ISEMRINO.Substring(0, 4);
                            fatKalem.STra_BF = 0;
                            fatKalem.Olcubr = 1;
                            fatKalem.ProjeKodu = "1";
                            fatKalem.D_YEDEK10 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                            fatKalem.SeriEkle(karsii, "", "", "", ikincimik1.ToDouble(), ikincimik2.ToDouble());
                            fatura.kayitYeni();
                        }


                        netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE SERI_NO='" + SERI_NO + "' AND GCKOD='G'");
                        var ACIKK1 = netRS.FieldByName("ACIK1").AsString;
                        var ACIKK2 = netRS.FieldByName("ACIK2").AsString;
                        var SERI_NOO_3 = netRS.FieldByName("SERI_NO_3").AsString;
                        var SERI_NOO_4 = netRS.FieldByName("SERI_NO_4").AsString;
                        netRS.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsii + "',ACIK1='" + ACIKK1 + "',ACIK2='" + ACIKK2 + "',SERI_NO_3='" + SERI_NOO_3 + "',SERI_NO_4='" + SERI_NOO_4 + "' WHERE SIPNO='" + ISEMRINO + "'");
                        netRS.Ac("UPDATE TBLSERITRA SET ACIK1='" + ACIKK1 + "',ACIK2='" + ACIKK2 + "',SERI_NO_3='" + SERI_NOO_3 + "',SERI_NO_4='" + SERI_NOO_4 + "' WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
                        return karsii;
                    }


                }
                catch (Exception exp)
                {
                    sirket.LogOff();
                    var message = exp.Message;
                    System.Diagnostics.Debug.Write(exp);
                    WebMail.SmtpServer = "192.168.2.13";
                    WebMail.Send("ergunozbudakli@efecegalvaniz.com,ugurkonakci@efecegalvaniz.com,dincersipka@efecegalvaniz.com", "Dertler Derya Olmuş", "<p><b>"+ISEMRINO + "</b> sıkıntılı ayağınızı denk alın!</p><p>Sıkıntı: "+message+"</p>", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);
                    
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
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);

                    netRS = kernel.yeniNetRS(sirket);
                    if (kontrol)
                    {
                        Isemri = kernel.yeniIsEmri(sirket);
                        Isemri.kayitOku(TOkumaTipi.otAc, "ISEMRINO = \'" + ISEMRINO + "\'");
                        Isemri.ReceteSaklansin = false;
                        Isemri.Kapali = true;
                        Isemri.kayitDuzelt();
                    }
                    netRS.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + ISEMRINO + "'");

                    seri = netRS.FieldByName("SERINO").AsString;
                    var Stokkodu = netRS.FieldByName("STOK_KODU").AsString;
                    netRS.Ac("SELECT TOP(1) * FROM TBLSERITRA WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "' ORDER BY BELGENO DESC");
                    var karsi = netRS.FieldByName("SERI_NO").AsString;

                    Fatura fatura = default(Fatura);
                    FatUst fatUst = default(FatUst);
                    FatKalem fatKalem = default(FatKalem);
                    if (hurdamik != null&&hurdamik != "0")
                    {
                        fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);

                        fatUst = fatura.Ust();
                        fatUst.FATIRS_NO = fatura.YeniNumara("E");
                        fatUst.TIPI = TFaturaTipi.ft_Bos;
                        fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                        fatUst.Tarih = DateTime.Now;
                        fatUst.FiiliTarih = DateTime.Now;
                        fatUst.PLA_KODU = "45";
                        fatUst.Proje_Kodu = "1";
                        fatUst.KDV_DAHILMI = true;
                        fatUst.Aciklama = ISEMRINO;
                        fatKalem = fatura.kalemYeni(Stokkodu);

                        ///Giriş Depo Kodu
                        fatKalem.Gir_Depo_Kodu = 60;
                        fatKalem.DEPO_KODU = 45;
                        fatKalem.STra_GCMIK = hurdamik.ToDouble();
                        fatKalem.STra_BF = 0;
                        fatKalem.STra_ACIK = ISEMRINO.Substring(0, 4);
                        fatKalem.Olcubr = 1;
                        fatKalem.ProjeKodu = "1";
                        fatKalem.D_YEDEK10 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

                        fatKalem.SeriEkle(karsi, "", "", "", hurdamik.ToDouble(), 0);
                        fatura.kayitYeni();
                    }
                    if (ikincimik1 != null&&ikincimik1 != "0")
                    {
                        fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);

                        fatUst = fatura.Ust();
                        fatUst.FATIRS_NO = fatura.YeniNumara("E");
                        fatUst.TIPI = TFaturaTipi.ft_Bos;
                        fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                        fatUst.Tarih = DateTime.Now;
                        fatUst.FiiliTarih = DateTime.Now;
                        fatUst.PLA_KODU = "45";
                        fatUst.Proje_Kodu = "1";
                        fatUst.Aciklama = ISEMRINO.Substring(0, 4);
                        fatUst.KDV_DAHILMI = true;
                        fatKalem = fatura.kalemYeni(Stokkodu);

                        ///Giriş Depo Kodu
                        fatKalem.Gir_Depo_Kodu = 55;
                        fatKalem.DEPO_KODU = 45;
                        fatKalem.STra_GCMIK = ikincimik1.ToDouble();
                        fatKalem.STra_GCMIK2 = ikincimik2.ToDouble();
                        fatKalem.STra_ACIK = ISEMRINO.Substring(0, 4);
                        fatKalem.STra_BF = 0;
                        fatKalem.Olcubr = 1;
                        fatKalem.ProjeKodu = "1";
                        fatKalem.D_YEDEK10 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                        fatKalem.SeriEkle(karsi, "", "", "", ikincimik1.ToDouble(), ikincimik2.ToDouble());
                        fatura.kayitYeni();
                    }


                    netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE SERI_NO='" + SERI_NO + "' AND GCKOD='G'");
                    var ACIK1 = netRS.FieldByName("ACIK1").AsString;
                    var ACIK2 = netRS.FieldByName("ACIK2").AsString;
                    var SERI_NO_3 = netRS.FieldByName("SERI_NO_3").AsString;
                    var SERI_NO_4 = netRS.FieldByName("SERI_NO_4").AsString;
                    netRS.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "',ACIK1='"+ACIK1+"',ACIK2='"+ACIK2+ "',SERI_NO_3='"+ SERI_NO_3 + "',SERI_NO_4='"+ SERI_NO_4 + "' WHERE SIPNO='" + ISEMRINO + "'");
                    netRS.Ac("UPDATE TBLSERITRA SET ACIK1='"+ACIK1+"',ACIK2='"+ACIK2+ "',SERI_NO_3='"+ SERI_NO_3 + "',SERI_NO_4='"+ SERI_NO_4 + "' WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
                    seri = karsi;
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

            return seri;
        }
        public string BKUretim(string stokkodu, string ISEMRINO, string SERI_NO, string KULL_MIKTAR, string mik2, bool kontrol, string hurdamik, string ikincimik1, string ikincimik2, int dongu)
        {
            var seri = "";
            if (KULL_MIKTAR != "0")
            {
                try
                {


                    sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                            "TEST2022",
                                            "TEMELSET",
                                            "",
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);





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

                    seri = netRS.FieldByName("SERINO").AsString;
                    var Stokkodu = netRS.FieldByName("STOK_KODU").AsString;

                    NetRS SeriNoTakipRS = kernel.yeniNetRS(sirket);

                    SeriNoTakipRS.Ac($"SELECT * FROM TBLSTSABIT WHERE STOK_KODU='{Stokkodu}'");

                    var SeriNoTakip = SeriNoTakipRS.FieldByName("GIRIS_SERI").AsString;

                    if (SeriNoTakip == "E" && dongu == 0)
                    {
                        if (seri == null)
                        {
                            uretim.OTOSERIURET();
                            uretim.SeriEkle(uretim.SeriOku(0).Seri1, "", "", "", KULL_MIKTAR.ToDouble(), mik2.ToDouble());
                        }
                        else
                        {
                            uretim.SeriEkle(seri, "", "", "", KULL_MIKTAR.ToDouble(), mik2.ToDouble());
                        }
                    }
                    else
                    {
                        netRS.Ac("SELECT TOP(1) * FROM TBLSERITRA WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "' ORDER BY BELGENO DESC");
                        var s = netRS.FieldByName("SERI_NO").AsString;
                        uretim.SeriEkle(s, "", "", "", KULL_MIKTAR.ToDouble(), mik2.ToDouble());
                    }


                    if (SERI_NO != null && ISEMRINO.Substring(0, 2) != "MH")
                    {
                        netRS.Ac("UPDATE TBLISEMRIREC SET SERINO='" + SERI_NO + "' WHERE ISEMRINO='" + ISEMRINO + "'");
                    }
                    if (ISEMRINO == stokkodu + "23000001001" || ISEMRINO == stokkodu + "23000001002")
                    {
                        netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE GCKOD='G' AND SERI_NO='" + SERI_NO + "'");
                        var seritraStok = netRS.FieldByName("STOK_KODU").AsString;
                        netRS.Ac("UPDATE TBLISEMRIREC SET HAM_KODU='" + seritraStok + "' WHERE ISEMRINO='" + ISEMRINO + "'");
                        netRS.Ac("UPDATE TBLISEMRI SET KAPALI='H' WHERE ISEMRINO='" + ISEMRINO + "'");
                    }

                    uretim.FisUret();
                    uretim.Kaydet();
                    if (ISEMRINO == stokkodu + "23000001001" || ISEMRINO == stokkodu + "23000001002")
                    {
                        netRS.Ac("UPDATE TBLISEMRIREC SET HAM_KODU='HURDA',SERINO=NULL WHERE ISEMRINO='" + ISEMRINO + "'");
                        netRS.Ac("UPDATE TBLISEMRI SET KAPALI='E' WHERE ISEMRINO='" + ISEMRINO + "'");
                    }


                    netRS.Ac("SELECT * FROM TBLSERITRA WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
                    var karsi = netRS.FieldByName("SERI_NO").AsString;

                    netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "' AND BELGENO<>'" + uretim.UretSon_FisNo + "' ORDER BY BELGENO DESC");
                    var mikold = netRS.FieldByName("MIKTAR").AsFloat;
                    netRS.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + ISEMRINO + "'");

                    //if (eskimiktar!= jsonList[i].KULL_MIKTAR)
                    //{
                    //    netRS1.Ac("UPDATE TBLISEMRIREC SET MIKTAR=" + (eskimiktar - jsonList[i].KULL_MIKTAR) + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                    //}
                    netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE SERI_NO='" + SERI_NO + "' AND GCKOD='G'");
                    var ACIK1 = netRS.FieldByName("ACIK1").AsString;
                    var ACIK2 = netRS.FieldByName("ACIK2").AsString;
                    var SERI_NO_3 = netRS.FieldByName("SERI_NO_3").AsString;
                    var SERI_NO_4 = netRS.FieldByName("SERI_NO_4").AsString;
                    netRS.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "',ACIK1='" + ACIK1 + "',ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + ISEMRINO + "'");

                    netRS.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + ISEMRINO + "'");
                    var referans = netRS.FieldByName("REFISEMRINO").AsString;

                    //netRS.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + referans + "'");

                    //var eski = netRS.FieldByName("MIKTAR").AsFloat;
                    //var oran = KULL_MIKTAR.ToDouble() / eski;
                    //var miktar2 = netRS.FieldByName("ACIKLAMA").AsString;
                    //var yeni = miktar2.ToDouble() * oran;

                    netRS.Ac("UPDATE TBLISEMRI SET MIKTAR='" + (mikold != 0 ? mikold + KULL_MIKTAR.ToDouble() : KULL_MIKTAR.ToDouble()) + "' WHERE ISEMRINO='" + referans + "'");



                    if (ISEMRINO.Substring(0, 2) == "MH" || ISEMRINO.Substring(0, 2) == "BK")
                    {
                        Isemri = kernel.yeniIsEmri(sirket);
                        Isemri.kayitOku(TOkumaTipi.otAc, "ISEMRINO = \'" + ISEMRINO + "\'");
                        Isemri.ReceteSaklansin = false;
                        Isemri.Kapali = true;
                        Isemri.kayitDuzelt();
                    }
                    netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
                    seri = netRS.FieldByName("SERI_NO").AsString;
                   
                    netRS.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + ISEMRINO + "'");

                    seri = netRS.FieldByName("SERINO").AsString;
                   

                    Fatura fatura = default(Fatura);
                    FatUst fatUst = default(FatUst);
                    FatKalem fatKalem = default(FatKalem);
                    if (hurdamik != null && hurdamik != "0")
                    {
                        fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);

                        fatUst = fatura.Ust();
                        fatUst.FATIRS_NO = fatura.YeniNumara("E");
                        fatUst.TIPI = TFaturaTipi.ft_Bos;
                        fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                        fatUst.Tarih = DateTime.Now;
                        fatUst.FiiliTarih = DateTime.Now;
                        fatUst.PLA_KODU = "45";
                        fatUst.Proje_Kodu = "1";
                        fatUst.KDV_DAHILMI = true;
                        fatUst.Aciklama = ISEMRINO;
                        fatKalem = fatura.kalemYeni(Stokkodu);

                        ///Giriş Depo Kodu
                        fatKalem.Gir_Depo_Kodu = 60;
                        fatKalem.DEPO_KODU = 45;
                        fatKalem.STra_GCMIK = hurdamik.ToDouble();
                        fatKalem.STra_BF = 0;
                        fatKalem.STra_ACIK = ISEMRINO.Substring(0, 4);
                        fatKalem.Olcubr = 1;
                        fatKalem.ProjeKodu = "1";
                        fatKalem.D_YEDEK10 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

                        fatKalem.SeriEkle(karsi, "", "", "", hurdamik.ToDouble(), 0);
                        fatura.kayitYeni();
                    }
                    if (ikincimik1 != null && ikincimik1 != "0")
                    {
                        fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);

                        fatUst = fatura.Ust();
                        fatUst.FATIRS_NO = fatura.YeniNumara("E");
                        fatUst.TIPI = TFaturaTipi.ft_Bos;
                        fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                        fatUst.Tarih = DateTime.Now;
                        fatUst.FiiliTarih = DateTime.Now;
                        fatUst.PLA_KODU = "45";
                        fatUst.Proje_Kodu = "1";
                        fatUst.Aciklama = ISEMRINO;
                        fatUst.KDV_DAHILMI = true;
                        fatKalem = fatura.kalemYeni(Stokkodu);

                        ///Giriş Depo Kodu
                        fatKalem.Gir_Depo_Kodu = 55;
                        fatKalem.DEPO_KODU = 45;
                        fatKalem.STra_GCMIK = ikincimik1.ToDouble();
                        fatKalem.STra_GCMIK2 = ikincimik2.ToDouble();
                        fatKalem.STra_ACIK = ISEMRINO.Substring(0, 4);
                        fatKalem.STra_BF = 0;
                        fatKalem.Olcubr = 1;
                        fatKalem.ProjeKodu = "1";
                        fatKalem.D_YEDEK10 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

                        fatKalem.SeriEkle(karsi, "", "", "", ikincimik1.ToDouble(), ikincimik2.ToDouble());
                        fatura.kayitYeni();
                    }


                   



                }
                catch (Exception exp)
                {
                    sirket.LogOff();
                    var message = exp.Message;
                    System.Diagnostics.Debug.Write(exp);
                    WebMail.SmtpServer = "192.168.2.13";
                    WebMail.Send("ergunozbudakli@efecegalvaniz.com,ugurkonakci@efecegalvaniz.com,dincersipka@efecegalvaniz.com", "Dertler Derya Olmuş", "<p><b>" + ISEMRINO + "</b> sıkıntılı ayağınızı denk alın!</p><p>Sıkıntı: " + message + "</p>", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);

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
           

            return seri;
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
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);

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
        public string UretimKaydiSonuBarkodCiktisi(List<BarkodModel> Data, bool ETIKET_ONIZLEME)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A6, 10f, 10f, 10f, 10f);

            string imagePath = System.IO.Path.Combine(Server.MapPath("~\\DesignOutput\\Sevkiyat\\Content"), "SevkiyatDesign.png");
            string pdfPath = System.IO.Path.Combine(Server.MapPath("~\\DesignOutput\\Uretim\\UretimSonuKaydi"), $"{DateTime.UtcNow.ToUnixTime()}.pdf");

            FileStream Memory = new FileStream(pdfPath, FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, Memory);

            document.Open();

            for (int i = 0; i < Data.Count; i++)
            {
                document.NewPage();
                iTextSharp.text.Image BackgroundImage = iTextSharp.text.Image.GetInstance(imagePath);
                BackgroundImage.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
                BackgroundImage.Alignment = iTextSharp.text.Image.UNDERLYING;
                BackgroundImage.SetAbsolutePosition(0, 0);
                document.Add(BackgroundImage);

                PdfContentByte cb = writer.DirectContent;

                iTextSharp.text.Font fontNormal = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 11, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font fontBoldHeader = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 13, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font fontBoldContent = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 11, iTextSharp.text.Font.BOLD);

                ColumnText Header = new ColumnText(cb);
                Header.SetSimpleColumn(45, 125, 270, 335);
                Header.AddElement(new iTextSharp.text.Paragraph(Data[i].SERI_NO) { Alignment = Element.ALIGN_CENTER, Font = fontBoldHeader });
                Header.AddElement(new iTextSharp.text.Paragraph(Data[i].STOK_ADI) { Alignment = Element.ALIGN_CENTER, Font = fontBoldHeader, SpacingBefore = 10f, MultipliedLeading = 1f });
                Header.Go();

                ColumnText Content = new ColumnText(cb) { Alignment = Element.ALIGN_CENTER };
                Content.SetSimpleColumn(35, 70, 280, 270);

                iTextSharp.text.Paragraph GrupIsim = new iTextSharp.text.Paragraph("GRUP İSİM   ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                GrupIsim.Add(new Chunk($": {BosDegerKontrolu(Data[i].GRUP_ISIM)}", fontNormal));
                Content.AddElement(GrupIsim);

                iTextSharp.text.Paragraph Boy = new iTextSharp.text.Paragraph("BOY         ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Boy.Add(new Chunk($": {DegerFormat((int)Data[i].BOY, "BOY")}", fontNormal));
                Content.AddElement(Boy);

                iTextSharp.text.Paragraph Miktar1 = new iTextSharp.text.Paragraph("MİKTAR      ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Miktar1.Add(new Chunk($": {MiktarFormat(Data[i].MIKTAR1, Data[i].OLCU_BR1)}", fontNormal));
                Content.AddElement(Miktar1);

                iTextSharp.text.Paragraph Miktar2 = new iTextSharp.text.Paragraph("MİKTAR 2    ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Miktar2.Add(new Chunk($": {MiktarFormat(Data[i].MIKTAR2, Data[i].OLCU_BR2)}", fontNormal));
                Content.AddElement(Miktar2);

                iTextSharp.text.Paragraph Kalinlik = new iTextSharp.text.Paragraph("KALINLIK    ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Kalinlik.Add(new Chunk($": {BosDegerKontrolu(Data[i].KALINLIK)}", fontNormal));
                Content.AddElement(Kalinlik);

                iTextSharp.text.Paragraph Genislik = new iTextSharp.text.Paragraph("GENİŞLİK    ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Genislik.Add(new Chunk($": {DegerFormat(Data[i].GENISLIK,"GENISLIK")}", fontNormal));
                Content.AddElement(Genislik);

                iTextSharp.text.Paragraph Metraj = new iTextSharp.text.Paragraph("METRAJ      ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Metraj.Add(new Chunk($": {MetrajFormat(Data[i].METRAJ, "M")}", fontNormal));
                Content.AddElement(Metraj);

                iTextSharp.text.Paragraph Tarih = new iTextSharp.text.Paragraph("TARİH/SAAT  ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Tarih.Add(new Chunk($": {TarihFormat(Data[i].KAYITTARIHI)}", fontNormal));
                Content.AddElement(Tarih);

                iTextSharp.text.Paragraph MakineOperator = new iTextSharp.text.Paragraph("MAKİNE/OPR. ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                MakineOperator.Add(new Chunk($": {BosDegerKontrolu(Data[i].MAK_KODU)}/{BosDegerKontrolu(Data[i].KAYITYAPANKUL)}", fontNormal));
                Content.AddElement(MakineOperator);

                iTextSharp.text.Paragraph Musteri = new iTextSharp.text.Paragraph("MÜŞTERİ     ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Musteri.Add(new Chunk($": {BosDegerKontrolu(Data[i].SIPARIS_CARI)}", fontNormal));
                Content.AddElement(Musteri);
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

            if (!ETIKET_ONIZLEME)
            {
                try
                {

                    PrintHelper.Print(pdfPath, "Olivetti d-COPIA 4023MF MUHASEBE");
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

            return ToBase64String(pdfPath);
        }
        private string DegerFormat(double Deger,string Tip)
        {
            if (Deger == 0)
            {

                return "-";
            }
            else
            {
                if (Tip == "BOY")
                    return Deger.ToString() + " MM";
                else
                    return Deger.ToString();
            }
            
        }

        [HttpPost]
        public string TrpzUretimKaydiSonuBarkodCiktisi(string BARKOD_NO, bool ETIKET_ONIZLEME)
        {

            try
            {
                WebClient Client = new WebClient() { Encoding = Encoding.UTF8 };

                string Response = Client.DownloadString(new Uri("http://192.168.2.13:83/api/seri/kontrol/" + BARKOD_NO));
                List<BarkodModel> Result = new JavaScriptSerializer().Deserialize<List<BarkodModel>>(Response);

                if (Result.Count < 1)
                    return "";

                BarkodModel Model = Result[0];

                System.Diagnostics.Debug.WriteLine("METRAJ: " + Model.METRAJ);
                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A6, 10f, 10f, 10f, 10f);

                string imagePath = System.IO.Path.Combine(Server.MapPath("~\\DesignOutput\\Sevkiyat\\Content"), "SevkiyatDesign.png");
                string pdfPath = System.IO.Path.Combine(Server.MapPath("~\\DesignOutput\\Uretim\\UretimSonuKaydi"), $"{DateTime.UtcNow.ToUnixTime()}.pdf");

                FileStream Memory = new FileStream(pdfPath, FileMode.Create);
                PdfWriter writer = PdfWriter.GetInstance(document, Memory);

                document.Open();

                iTextSharp.text.Image BackgroundImage = iTextSharp.text.Image.GetInstance(imagePath);
                BackgroundImage.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
                BackgroundImage.Alignment = iTextSharp.text.Image.UNDERLYING;
                BackgroundImage.SetAbsolutePosition(0, 0);
                document.Add(BackgroundImage);

                PdfContentByte cb = writer.DirectContent;

                iTextSharp.text.Font fontNormal = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 11, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font fontBoldHeader = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 13, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font fontBoldContent = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 11, iTextSharp.text.Font.BOLD);

                ColumnText Header = new ColumnText(cb);
                Header.SetSimpleColumn(45, 125, 270, 335);
                Header.AddElement(new iTextSharp.text.Paragraph(Model.SERI_NO) { Alignment = Element.ALIGN_CENTER, Font = fontBoldHeader });
                Header.AddElement(new iTextSharp.text.Paragraph(Model.STOK_ADI) { Alignment = Element.ALIGN_CENTER, Font = fontBoldHeader, SpacingBefore = 10f, MultipliedLeading = 1f });
                Header.Go();

                ColumnText Content = new ColumnText(cb) { Alignment = Element.ALIGN_CENTER };
                Content.SetSimpleColumn(35, 70, 280, 270);

                iTextSharp.text.Paragraph GrupIsim = new iTextSharp.text.Paragraph("GRUP İSİM   ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                GrupIsim.Add(new Chunk($": {BosDegerKontrolu(Model.GRUP_ISIM)}", fontNormal));
                Content.AddElement(GrupIsim);

                iTextSharp.text.Paragraph Boy = new iTextSharp.text.Paragraph("BOY         ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Boy.Add(new Chunk($": {DegerFormat(((int)Model.BOY),"BOY")}", fontNormal));
                Content.AddElement(Boy);

                iTextSharp.text.Paragraph Miktar1 = new iTextSharp.text.Paragraph("MİKTAR      ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Miktar1.Add(new Chunk($": {MiktarFormat(Model.MIKTAR1, Model.OLCU_BR1)}", fontNormal));
                Content.AddElement(Miktar1);

                iTextSharp.text.Paragraph Miktar2 = new iTextSharp.text.Paragraph("MİKTAR 2    ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Miktar2.Add(new Chunk($": {MiktarFormat(Model.MIKTAR2, Model.OLCU_BR2)}", fontNormal));
                Content.AddElement(Miktar2);

                iTextSharp.text.Paragraph Kalinlik = new iTextSharp.text.Paragraph("KALINLIK    ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Kalinlik.Add(new Chunk($": {BosDegerKontrolu(Model.KALINLIK)}", fontNormal));
                Content.AddElement(Kalinlik);

                iTextSharp.text.Paragraph Genislik = new iTextSharp.text.Paragraph("GENİŞLİK    ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Genislik.Add(new Chunk($": {DegerFormat(Model.GENISLIK,"GENISLIK")}", fontNormal));
                Content.AddElement(Genislik);

                iTextSharp.text.Paragraph Metraj = new iTextSharp.text.Paragraph("METRAJ      ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Metraj.Add(new Chunk($": {MetrajFormat(Model.METRAJ, "M")}", fontNormal));
                Content.AddElement(Metraj);

                iTextSharp.text.Paragraph Tarih = new iTextSharp.text.Paragraph("TARİH/SAAT  ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Tarih.Add(new Chunk($": {TarihFormat(Model.KAYITTARIHI)}", fontNormal));
                Content.AddElement(Tarih);

                iTextSharp.text.Paragraph MakineOperator = new iTextSharp.text.Paragraph("MAKİNE/OPR. ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                MakineOperator.Add(new Chunk($": {BosDegerKontrolu(Model.MAK_KODU)}/{BosDegerKontrolu(Model.KAYITYAPANKUL)}", fontNormal));
                Content.AddElement(MakineOperator);

                iTextSharp.text.Paragraph Musteri = new iTextSharp.text.Paragraph("MÜŞTERİ     ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Musteri.Add(new Chunk($": {BosDegerKontrolu(Model.SIPARIS_CARI)}", fontNormal));
                Content.AddElement(Musteri);
                Content.Go();

                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(Model.SERI_NO, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                System.Drawing.Image qrCodeImage = qrCode.GetGraphic(45, System.Drawing.Color.Black, System.Drawing.Color.Transparent, true);

                iTextSharp.text.Image QR = iTextSharp.text.Image.GetInstance(ImageToByteArray(qrCodeImage));
                QR.ScaleToFit(75, 75);
                QR.Alignment = iTextSharp.text.Image.UNDERLYING;
                QR.SetAbsolutePosition(195, 12);
                document.Add(QR);

                document.Close();
                Memory.Close();

                //Eğer önizleme kapalıysa direkt yazdır.
                if (!ETIKET_ONIZLEME)
                {
                    PrintHelper.Print(pdfPath, "Olivetti d-COPIA 4023MF MUHASEBE");
                }

                return ToBase64String(pdfPath);
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        public class BarkodModel
        {
            public string SERI_NO { get; set; }
            public string STOK_ADI { get; set; }
            public double MIKTAR1 { get; set; }
            public string OLCU_BR1 { get; set; }
            public double MIKTAR2 { get; set; }
            public string OLCU_BR2 { get; set; }
            public string STOK_KODU { get; set; }
            public double GENISLIK { get; set; }
            public string GRUP_ISIM { get; set; }
            public double BOY { get; set; }
            public string KAYITYAPANKUL { get; set; }
            public string KAYITTARIHI { get; set; }
            public string SIPARIS_CARI { get; set; }
            public string MAK_KODU { get; set; }
            public string KALINLIK { get; set; }
            public double METRAJ { get; set; }

            public bool ETIKET_ONIZLEME { get; set; }
        }
        public string ToBase64String(string fileName)
        {
            using (FileStream reader = new FileStream(fileName, FileMode.Open))
            {
                byte[] buffer = new byte[reader.Length];
                reader.Read(buffer, 0, (int)reader.Length);
                return Convert.ToBase64String(buffer);
            }
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
        private string BosDegerKontrolu(double Deger)
        {
            return Deger == 0|| Deger == 0.0 ? "-" : Deger.ToString();
        }
        private string TarihFormat(string Tarih)
        {
            if(string.IsNullOrEmpty(Tarih))
            {
                return "-";
            }

            return DateTime.Parse(Tarih).ToString("dd/MM/yyyy HH:mm:ff");
        }
        private string MiktarFormat(double Miktar, string OlcuBirimi) 
        {
            if(Miktar == 0)
            {
                return "-";
            }

            return $"{Miktar.ToString("c", new CultureInfo("tr-TR"))} {OlcuBirimi}";
        }
        private string MetrajFormat(double Metraj, string OlcuBirimi)
        {
            if(Metraj == 0)
            {
                return "-";
            }

            return $"{Metraj.ToString("c", new CultureInfo("tr-TR"))} {OlcuBirimi}";
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

        #region MES

        public async Task<ActionResult> MES()
        {
            int moduleId = 45;

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

            return View();
        }

        #endregion
    }
}