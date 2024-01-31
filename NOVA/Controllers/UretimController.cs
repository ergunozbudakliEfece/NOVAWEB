using DocumentFormat.OpenXml.Spreadsheet;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NetOpenX50;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NOVA.Models;
using NOVA.Utils;
using QRCoder;
using ServiceStack;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Windows.Media.Animation;
using static NOVA.Controllers.IsEmriController;

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
            if (Request.Cookies["Id"] == null)
            {
                return RedirectToAction("Index", "Home");
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
        public async Task<ActionResult> UretimPlanlama()
        {
            int moduleId = 47;

            List<Modules> Modules = await AuthHelper.GetModules(moduleId);

            if (Modules[0].ACTIVE != "1")
            {
                return RedirectToAction("Maintenance", "Home");
            }
            if (Request.Cookies["Id"] == null)
            {
                return RedirectToAction("Index", "Home");
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
            ViewBag.Id = Request.Cookies["Id"].Value;

            return View();
        }

        public async Task<ActionResult> UretimKaynakPlanlama()
        {
            int moduleId = 48;

            List<Modules> Modules = await AuthHelper.GetModules(moduleId);

            if (Modules[0].ACTIVE != "1")
            {
                return RedirectToAction("Maintenance", "Home");
            }
            if (Request.Cookies["Id"] == null)
            {
                return RedirectToAction("Index", "Home");
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
            ViewBag.Id = Request.Cookies["Id"].Value;

            return View();
        }
        public async Task<ActionResult> AmbarGiris()
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
                                            "EFECE2023",
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
        public Microsoft.AspNetCore.Mvc.StatusCodeResult IsEmirleriniKapat(List<string> isemrimod)
        {
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                            "EFECE2023",
                                            "TEMELSET",
                                            "",
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);
                NetRS netRS = kernel.yeniNetRS(sirket);
                for (var i = 0; i < isemrimod.Count; i++)
                {
                    netRS.Ac("UPDATE TBLISEMRI SET KAPALI='E' WHERE ISEMRINO='" + isemrimod[i] + "'");
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
                                            "EFECE2023",
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
                                            "EFECE2023",
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
                                            "EFECE2023",
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
        public string UretimSonuKaydi(string hatkodu, string stokkodu, string genislik, string mik1, string mik2, bool? kontrol, bool etiket, bool onizleme, string YAZICI)
        {
            var uretimTipi = UretimTipi(hatkodu)[0].URETIM_TIPI;
            var stokadi = GetStokAdlari().FirstOrDefault(x => x.STOK_KODU == stokkodu);
            double f = 0;

            List<string> SeriNoListesi = new List<string>();
            var BelgeNolar = "";
            try
            {


               


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
                    
                    if (json2 != null)
                    {
                        
                        sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                           "EFECE2023",
                                           "TEMELSET",
                                           "",
                                           Request.Cookies["UserName"].Value,
                                           LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);
                        var karsi = "";
                        for (var i = 0; i < jsonList.Count; i++)
                        {
                        
                            netRS = kernel.yeniNetRS(sirket);
                            netRS.Ac("SELECT * FROM TBLISEMRIREC WITH(NOLOCK) WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                            var recmik = netRS.FieldByName("MIKTAR").AsFloat;
                            var miktarsabitle = netRS.FieldByName("MIKTARSABITLE").AsString;
                            netRS.Ac("SELECT * FROM TBLISEMRI WITH(NOLOCK) WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
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
                            uretim.UretSon_FisNo = jsonList[i].FIS_NO;

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
                                if (hatkodu == "DL01")
                                {
                                    uretim.F_Yedek1 = 1;
                                }
                                else
                                {
                                    uretim.F_Yedek1 = jsonList[i].MIKTAR2;
                                }
                            }

                            uretim.UretSon_Depo = 45;
                            uretim.I_Yedek1 = 45;
                            uretim.I_Yedek2 = 0;
                            uretim.OTO_YMAM_GIRDI_CIKTI = true;
                            uretim.OTO_YMAM_STOK_KULLAN = false;


                            uretim.BAKIYE_DEPO = 0;
                            netRS.Ac("SELECT * FROM TBLISEMRI WITH(NOLOCK) WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                            var seri = netRS.FieldByName("SERINO").AsString;
                            var ACIK1 = "";
                            var ACIK2 = "";
                            var SERI_NO_3 = "";
                            var SERI_NO_4 = "";
                            var fisno = jsonList[i].FIS_NO;
                            BelgeNolar += " " + fisno;
                            if (seri == null || seri == "0")
                            {
                                uretim.OTOSERIURET();
                                if (hatkodu == "DL01")
                                {
                                    
                                    uretim.SeriEkle(uretim.SeriOku(0).Seri1, "", "", "", jsonList[i].KULL_MIKTAR, 1);
                                }
                                else
                                {

                                    uretim.SeriEkle(uretim.SeriOku(0).Seri1, "", "", "", jsonList[i].KULL_MIKTAR, jsonList[i].MIKTAR2);
                                }
                                seri = uretim.SeriOku(0).Seri1;
                            }
                            

                            //WebClient client2 = new WebClient();
                            //client2.Encoding = System.Text.Encoding.UTF8;

                            //var json3 = client2.DownloadString(new Uri($"http://192.168.2.13:83/api/uretim/kontrol/{fisno}"));

                            //var Response = JsonConvert.DeserializeObject<List<FisKontrol>>(json3);

                            //bool FIS_NO_VAR = Response?[0].KONTROL == "1";

                            //while (FIS_NO_VAR)
                            //{
                            //    fisno = uretim.SonFisNumarasi("N");
                            //    uretim.UretSon_FisNo=fisno;

                            //    json3 = client2.DownloadString(new Uri($"http://192.168.2.13:83/api/uretim/kontrol/{fisno}"));

                            //    Response = JsonConvert.DeserializeObject<List<FisKontrol>>(json3);
                            //    FIS_NO_VAR = Response?[0].KONTROL == "1";
                            //}

                            
                            uretim.FisUret();
                            uretim.Kaydet();
                                
                            
                          
                            NetRS netRS1 = kernel.yeniNetRS(sirket);
                            f = recmik - jsonList[i].KULL_MIKTAR;
                            if (miktarsabitle != "H")
                            {
                                if (f > 0 && hatkodu == "DL01")
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
                                    fatUst.Aciklama = fisno;
                                    fatKalem = fatura.kalemYeni("HURDA");
                                    fatKalem.DEPO_KODU = 60;
                                    fatKalem.Olcubr = 1;
                                    fatKalem.ProjeKodu = "1";
                                    fatKalem.D_YEDEK10 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                                    fatKalem.STra_GCMIK = f;
                                    fatKalem.STra_NF = 0;
                                    fatKalem.STra_BF = 0;
                                    fatKalem.STra_ACIK = fisno;


                                    fatura.kayitYeni();
                                    Marshal.ReleaseComObject(fatura);
                                    Marshal.ReleaseComObject(fatUst);
                                    Marshal.ReleaseComObject(fatKalem);
                                }
                            }

                            //if (miktarsabitle == "E")
                            //{
                            //    netRS.Ac("UPDATE TBLISEMRI SET MIKTAR=" + eskimiktar + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                            //}

                            //netRS1.Ac("UPDATE TBLSERITRA SET MIKTAR2=" + jsonList[i].MIKTAR2 + " WHERE  GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            netRS1.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            if (seri == null)
                            {
                                seri = netRS1.FieldByName("SERI_NO").AsString;
                            }

                            if (i == 0)
                            {
                                karsi = netRS1.FieldByName("SERI_NO").AsString;
                            }
                            netRS1.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='C' AND SIPNO='" + jsonList[i].ISEMRINO + "'");

                            var seri_no = netRS1.FieldByName("SERI_NO").AsString;
                            netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE SERI_NO='" + seri_no + "' AND GCKOD='G'");
                            ACIK1 = netRS.FieldByName("ACIK1").AsString;
                            ACIK2 = netRS.FieldByName("ACIK2").AsString;
                            var ACIK3 = netRS.FieldByName("ACIK3").AsString;
                            var ACIKLAMA_4 = netRS.FieldByName("ACIKLAMA_4").AsString;
                            var ACIKLAMA_5 = netRS.FieldByName("ACIKLAMA_5").AsString;
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
                                netRS1.Ac("UPDATE TBLSERITRA SET ACIK1 = '" + ACIK1 + "',ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "',ACIK3='" + ACIK3 + "',ACIKLAMA_4='" + ACIKLAMA_4 + "',ACIKLAMA_5='" + ACIKLAMA_5 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "' AND GCKOD='C' ");
                                netRS1.Ac("UPDATE TBLSERITRA SET ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "',MIKTAR2='1' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "' AND GCKOD='G' ");

                                if (hatkodu != "BK01" && hatkodu != "DL01")
                                {
                                    netRS1.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                                    netRS1.Ac("UPDATE TBLSERITRA SET ACIK1 = '" + ACIK1 + "',ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "'  AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                                }
                                if (kontrol == true)
                                {
                                    netRS1.Ac("UPDATE TBLISEMRI SET KAPALI='E' WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                                }
                                if (etiket)
                                {
                                    netRS1.Ac("SELECT * FROM TBLSERITRA WITH(NOLOCK) WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G'");
                                    var serino = netRS1.FieldByName("SERI_NO").AsString;
                                    if (serino != null)
                                    {
                                        if (!SeriNoListesi.Contains(serino))
                                        {
                                            SeriNoListesi.Add(serino);

                                        }
                                    }
                                    else
                                    {

                                        SeriNoListesi.Add(karsi);
                                    }
                                }

                            }
                            else
                            {
                                netRS1.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                                var karsi1 = netRS1.FieldByName("SERI_NO").AsString;
                                netRS1.Ac("SELECT * FROM TBLISEMRI WITH(NOLOCK) WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                                var referans = netRS1.FieldByName("REFISEMRINO").AsString;
                                netRS1.Ac("SELECT * FROM TBLISEMRI WITH(NOLOCK) WHERE ISEMRINO='" + referans + "'");
                                var eski = netRS1.FieldByName("MIKTAR").AsFloat;
                                var oran = jsonList[i].KULL_MIKTAR / eski;
                                var miktar2 = netRS1.FieldByName("ACIKLAMA").AsString;
                                var yeni = miktar2.ToDouble() * oran;
                                netRS1.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi1 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                                netRS1.Ac("UPDATE TBLSERITRA SET MIKTAR2='1' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "' AND GCKOD='C'");
                                netRS1.Ac("UPDATE TBLSERITRA SET ACIK1 = '" + ACIK1 + "',ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "',ACIK3='" + ACIK3 + "',ACIKLAMA_4='" + ACIKLAMA_4 + "',ACIKLAMA_5='" + ACIKLAMA_5 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "' AND GCKOD='C' ");
                                netRS1.Ac("UPDATE TBLSERITRA SET ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "',MIKTAR2='1' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "' AND GCKOD='G' ");
                                if (referans != null && referans != "")
                                {
                                    netRS1.Ac("UPDATE TBLISEMRI SET MIKTAR='" + jsonList[i].KULL_MIKTAR + "',ACIKLAMA='" + Math.Round(yeni) + "' WHERE ISEMRINO='" + referans + "'");
                                }
                                if (kontrol == true)
                                {
                                    netRS1.Ac("UPDATE TBLISEMRI SET KAPALI='E' WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                                }
                                if (etiket)
                                {
                                    netRS1.Ac("SELECT * FROM TBLSERITRA WITH(NOLOCK) WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G'");
                                    var serino = netRS1.FieldByName("SERI_NO").AsString;
                                    if (serino != null)
                                    {
                                        if (!SeriNoListesi.Contains(serino))
                                        {
                                            SeriNoListesi.Add(serino);

                                        }
                                    }
                                    else
                                    {
                                        SeriNoListesi.Add(karsi1);
                                    }
                                }

                            }


                        }
                        var isemirleri = "";
                        foreach (var item in jsonList)
                        {
                            isemirleri += " " + item.ISEMRINO;
                        }
                        var mailList = "";
                        foreach (var itemlist in jsonList)
                        {
                            mailList += JsonConvert.SerializeObject(itemlist) + "</br>";
                        }
                        WebMail.SmtpServer = "192.168.2.13";
                        WebMail.Send("ergunozbudakli@efecegalvaniz.com,ugurkonakci@efecegalvaniz.com,dincersipka@efecegalvaniz.com", "İş Emirleri", "<p><b>" + isemirleri + "</b></p></br><p>Fiş bilgileri:</p><p>" + BelgeNolar + "</p>"+"</br>"+ mailList, "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);

                    }


                    //Stok hareketleri gerçeklestiriliyor
                    //uretim.kayitFisNoIleUretimSonu(uretim.UretSon_FisNo, TUretSonDepo.usdAktif,false,false);


                    return JsonConvert.SerializeObject(SeriNoListesi);
                }
                catch (Exception e)
                {
                    var exp = e.Message;
                    var message = exp;
                    System.Diagnostics.Debug.Write(exp);
                    WebMail.SmtpServer = "192.168.2.13";
                    var isemirleri = "";
                    foreach(var item in jsonList)
                    {
                        isemirleri += " "+item.ISEMRINO;
                    }
                    WebMail.Send("ergunozbudakli@efecegalvaniz.com,ugurkonakci@efecegalvaniz.com,dincersipka@efecegalvaniz.com", "Üretim Sonu Kaydı Hata", "<p><b>" + isemirleri + "</b> üzerinde bir hata oluştu!</p><p>Hata: " + message + "</p><p>Fiş bilgileri:</p><p>" + BelgeNolar + "</p>", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);
                    return $"Hata: {exp}";
                }
            }
            catch (Exception e)
            {
                var exp = e.Message;
                Console.WriteLine(e.Message);
                return $"Hata: {exp}";
            }
            finally
            {
                sirket.LogOff();
                kernel.FreeNetsisLibrary();
            }
        }

        public string Hurda(string hatkodu, string mik1, string stokkodu)
        {
            Kernel kernel1 = new Kernel();
            NetRS netRS1 = default(NetRS);
            Sirket sirket1 = kernel1.yeniSirket(TVTTipi.vtMSSQL,
                                           "EFECE2023",
                                           "TEMELSET",
                                           "",
                                           Request.Cookies["UserName"].Value,
                                           LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);

            try
            {
                var stoklar = GetStokAdlari();
                var stokadi = stoklar.Find(x => x.STOK_KODU == stokkodu).STOK_ADI;
                netRS1 = kernel1.yeniNetRS(sirket1);
                var a = netRS1.Ac("SELECT MAX(FISNO) AS FISNO FROM TEST2022.dbo.TBLSTHAR WHERE FISNO LIKE 'Z%'");

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
        public string AmbarGirCik(string seri, string miktar, string stokkodu, string YAZICI)
        {
            var yeniseri = seri.Substring(0, 3) + ((seri[3].ToString().ToInt()) + 1).ToString() + seri.Substring(4);
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                            "EFECE2023",
                                            "TEMELSET",
                                            "",
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);

                fatura = kernel.yeniFatura(sirket, TFaturaTip.ftAmbarC);
                fatUst = fatura.Ust();
                fatUst.FATIRS_NO = fatura.YeniNumara("A");
                fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                fatUst.CikisYeri = TCikisYeri.cySerbest;
                fatUst.CariKod = "12035200100406";
                fatUst.Tarih = DateTime.Now;
                fatUst.ENTEGRE_TRH = DateTime.Now;
                fatUst.FiiliTarih = DateTime.Now;
                fatUst.Proje_Kodu = "1";
                fatUst.KOD1 = "N";
                fatUst.Aciklama = yeniseri;
                fatKalem = fatura.kalemYeni(stokkodu);
                fatKalem.DEPO_KODU = 45;
                fatKalem.Olcubr = 1;
                fatKalem.ProjeKodu = "1";
                fatKalem.SeriEkle(seri, "", "", "", miktar.ToDouble(), 1);
                fatKalem.D_YEDEK10 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                fatKalem.STra_GCMIK = miktar.ToDouble();
                fatKalem.STra_GCMIK2 = 1;
                fatKalem.STra_NF = 0;
                fatKalem.STra_BF = 0;
                fatKalem.STra_ACIK = yeniseri;


                fatura.kayitYeni();

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
                fatUst.KOD1 = "N";
                fatUst.Aciklama = seri;
                fatKalem = fatura.kalemYeni(stokkodu);
                fatKalem.DEPO_KODU = 45;
                fatKalem.Olcubr = 1;
                fatKalem.ProjeKodu = "1";
                fatKalem.SeriEkle(yeniseri, "", "", "", miktar.ToDouble(), 1);
                fatKalem.D_YEDEK10 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                fatKalem.STra_GCMIK = miktar.ToDouble();
                fatKalem.STra_GCMIK2 = 1;
                fatKalem.STra_NF = 0;
                fatKalem.STra_BF = 0;
                fatKalem.STra_ACIK = seri;


                fatura.kayitYeni();
                netRS = kernel.yeniNetRS(sirket);
                netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE SERI_NO='" + seri + "' AND GCKOD='G'");
                var ACIK1 = netRS.FieldByName("ACIK1").AsString;
                var ACIK2 = netRS.FieldByName("ACIK2").AsString;
                var SERI_NO_3 = netRS.FieldByName("SERI_NO_3").AsString;
                var SERI_NO_4 = netRS.FieldByName("SERI_NO_4").AsString;
                netRS.Ac("UPDATE TBLSERITRA SET ACIK1 = '" + ACIK1 + "',ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "' WHERE SERI_NO='" + yeniseri + "' AND GCKOD='G'");
            }
            catch (Exception ex)
            {

                return "Hata " + ex.Message;
            }
            List<string> seriList = new List<string>();
            seriList.Add(yeniseri);
            return UretilmisEtiketleriYazdir(seriList, "Sevkiyat", true, YAZICI);
        }
        public string AmbarGir(List<AmbarModel> datalist)
        {
            List<string> stoklist = new List<string>();
            List<AmbarModel> stokgrup = new List<AmbarModel>();
            for (int i = 0; i < datalist.Count; i++)
            {
                if (!stoklist.Contains(datalist[i].STOK_KODU))
                {
                    stoklist.Add(datalist[i].STOK_KODU);
                }
            }
            for (int i = 0; i < stoklist.Count; i++)
            {
                var nd = datalist.Where(x => x.STOK_KODU == stoklist[i]).ToList();
                double sum = 0;
                AmbarModel nerAmbar = new AmbarModel();
                for (int a = 0; a < nd.Count; a++)
                {
                    sum += nd[a].MIKTAR;
                }
                nerAmbar.STOK_KODU = stoklist[i];
                nerAmbar.MIKTAR = sum;
                stokgrup.Add(nerAmbar);
            }

            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                            "EFECE2023",
                                            "TEMELSET",
                                            "",
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);
                var belgeNo = "";

                foreach (var item in stokgrup)
                {

                    fatura = kernel.yeniFatura(sirket, TFaturaTip.ftAmbarC);
                    fatUst = fatura.Ust();
                    belgeNo = fatura.YeniNumara("A");
                    fatUst.FATIRS_NO = belgeNo;
                    fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                    fatUst.CikisYeri = TCikisYeri.cySerbest;
                    fatUst.CariKod = "12035200100406";
                    fatUst.Tarih = DateTime.Now;
                    fatUst.ENTEGRE_TRH = DateTime.Now;
                    fatUst.FiiliTarih = DateTime.Now;
                    fatUst.Proje_Kodu = "1";
                    fatUst.KOD1 = "N";
                    var d = datalist.Where(x => x.STOK_KODU == item.STOK_KODU).ToList();
                    fatUst.Aciklama = d[0].HAT_KODU;

                    fatKalem = fatura.kalemYeni(item.STOK_KODU);
                    fatKalem.DEPO_KODU = d[0].TIP.ToInt();
                    fatKalem.Olcubr = 1;
                    fatKalem.ProjeKodu = "1";
                    for (var i = 0; i < d.Count; i++)
                    {
                        fatKalem.SeriEkle(d[i].SERI_NO, "", "", "", d[i].MIKTAR, 1);
                    }
                    fatKalem.D_YEDEK10 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                    var mik = stokgrup.FirstOrDefault(x => x.STOK_KODU == item.STOK_KODU).MIKTAR;
                    fatKalem.STra_GCMIK = mik;
                    fatKalem.STra_GCMIK2 = 1;
                    fatKalem.STra_NF = 0;
                    fatKalem.STra_BF = 0;
                    fatKalem.STra_ACIK = d[0].HAT_KODU;

                    fatura.kayitYeni();

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
                    fatUst.KOD1 = "N";
                    fatUst.Aciklama = belgeNo;

                    fatKalem = fatura.kalemYeni(d[0].TIP == "60" ? "HURDA" : "IKINCIKALITE");
                    fatKalem.DEPO_KODU = 45;
                    fatKalem.Olcubr = 1;
                    fatKalem.ProjeKodu = "1";
                    fatKalem.D_YEDEK10 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                    fatKalem.STra_GCMIK = mik;
                    fatKalem.STra_GCMIK2 = 1;
                    fatKalem.STra_NF = 0;
                    fatKalem.STra_BF = 0;
                    fatKalem.STra_ACIK = belgeNo;
                    fatura.kayitYeni();
                }

            }
            catch (Exception ex)
            {

                return "Hata " + ex.Message;
            }

            return "Başarılı";
        }

        [HttpPost]
        public string AmbarGiris(string a)
        {
            var data = JsonConvert.DeserializeObject<List<AmbarGirModel>>(a);
            List<string> StokKodlari=new List<string>();
            for(int i=0;i< data.Count;i++)
            {
                if (!StokKodlari.Contains(data[i].STOK_KODU))
                    StokKodlari.Add(data[i].STOK_KODU);
            }
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                                           "EFECE2023",
                                                           "TEMELSET",
                                                           "",
                                                           Request.Cookies["UserName"].Value,
                                                           LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);
                for(int i=0;i<StokKodlari.Count;i++)
                {
                    fatura = kernel.yeniFatura(sirket, TFaturaTip.ftAmbarG);
                    fatUst = fatura.Ust();
                    fatUst.FATIRS_NO = fatura.YeniNumara("A");
                    fatUst.AMBHARTUR = TAmbarHarTur.htDevir;
                    fatUst.CikisYeri = TCikisYeri.cySerbest;
                    fatUst.CariKod = "12035200100406";
                    fatUst.Tarih = DateTime.Now;
                    fatUst.ENTEGRE_TRH = DateTime.Now;
                    fatUst.FiiliTarih = DateTime.Now;
                    fatUst.Proje_Kodu = "1";
                    fatUst.Aciklama = "FABRİKA STOK DÜZELTME";
                    fatUst.DovBazTarihi = DateTime.Now;
                    var altkalemler = data.Where(x => x.STOK_KODU == StokKodlari[i]).ToList();
                    for(int j=0;j<altkalemler.Count;j++)
                    {
                        fatKalem = fatura.kalemYeni(StokKodlari[i]);
                        fatKalem.DEPO_KODU = 45;
                        fatKalem.Olcubr = 1;
                        fatKalem.ProjeKodu = "1";
                        fatKalem.D_YEDEK10 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                        fatKalem.STra_GCMIK = altkalemler[j].MIKTAR;
                        fatKalem.STra_GCMIK2 = altkalemler[j].MIKTAR2;
                        fatKalem.STra_DOVTIP = 1;
                        fatKalem.DOVTIP = 1;
                        fatKalem.STra_DOVFIAT = altkalemler[j].FIYAT;
                        fatKalem.STra_ACIK = StokKodlari[i];
                        fatKalem.SeriEkle(altkalemler[j].SERI_NO, altkalemler[j].ACIK1, altkalemler[j].ACIK2, altkalemler[j].ACIKLAMA_4, altkalemler[j].MIKTAR, altkalemler[j].MIKTAR2, altkalemler[j].SERI_NO_3, altkalemler[j].SERI_NO_4, altkalemler[j].ACIK3, altkalemler[j].ACIKLAMA_5);
                    }
                    fatura.kayitYeni();
                }
                
                
                
            }
            catch (Exception e)
            {

                return "Hata: " + e.Message;
            }
           
            return "Başarılı";
        }
        public class AmbarModel
        {
            public string STOK_KODU { set; get; }
            public string SERI_NO { set; get; }
            public double MIKTAR { set; get; }
            public string TIP { get; set; }
            public string HAT_KODU { get; set; }
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
        public string IkinciKalite(string hatkodu, string stokkodu, string genislik, string mik1, string mik2, string stokadi, string tip)
        {
            var uretimTipi = UretimTipi(hatkodu)[0].URETIM_TIPI;
            var Etiket = "";
            try
            {

                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                            "EFECE2023",
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
                        netRS.Ac("SELECT * FROM TBLISEMRIREC WITH(NOLOCK) WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");

                        var miktarsabitle = netRS.FieldByName("MIKTARSABITLE").AsString;
                        netRS.Ac("SELECT * FROM TBLISEMRI WITH(NOLOCK)  WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
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
                        netRS.Ac("SELECT * FROM TBLISEMRI WITH(NOLOCK) WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
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
                        netRS1.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                        if (seri == null)
                        {
                            seri = netRS1.FieldByName("SERI_NO").AsString;
                        }

                        if (i == 0)
                        {
                            karsi = netRS1.FieldByName("SERI_NO").AsString;
                        }
                        netRS1.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='C' AND SIPNO='" + jsonList[i].ISEMRINO + "'");

                        var seri_no = netRS1.FieldByName("SERI_NO").AsString;
                        netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE SERI_NO='" + seri_no + "' AND GCKOD='G'");
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
                            netRS1.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            var karsi1 = netRS1.FieldByName("SERI_NO").AsString;
                            netRS1.Ac("SELECT * FROM TBLISEMRI WITH(NOLOCK) WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                            var referans = netRS1.FieldByName("REFISEMRINO").AsString;
                            netRS1.Ac("SELECT * FROM TBLISEMRI WITH(NOLOCK) WHERE ISEMRINO='" + referans + "'");
                            var eski = netRS1.FieldByName("MIKTAR").AsFloat;
                            var oran = jsonList[i].KULL_MIKTAR / eski;
                            var miktar2 = netRS1.FieldByName("ACIKLAMA").AsString;
                            var yeni = miktar2.ToDouble() * oran;
                            netRS1.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi1 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            netRS1.Ac("UPDATE TBLSERITRA SET MIKTAR2='1' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "' AND GCKOD='C'");
                            netRS1.Ac("UPDATE TBLSERITRA SET ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "' AND GCKOD='G'");
                            netRS1.Ac("UPDATE TBLISEMRI SET MIKTAR='" + jsonList[i].KULL_MIKTAR + "',ACIKLAMA='" + Math.Round(yeni) + "' WHERE ISEMRINO='" + referans + "'");

                        }
                        netRS.Ac("SELECT MAX(FISNO) AS FISNO FROM TEST2022..TBLSTHAR WITH(NOLOCK)");
                        var fisno = (netRS.FieldByName("FISNO").AsString.Substring(1, netRS.FieldByName("FISNO").AsString.Count() - 1).ToInt() + 1).ToString().PadLeft(14, '0');
                        var fisno2 = (netRS.FieldByName("FISNO").AsString.Substring(1, netRS.FieldByName("FISNO").AsString.Count() - 1).ToInt() + 2).ToString().PadLeft(14, '0');
                        netRS.Ac("SELECT * FROM TEST2022..TBLSTHAR WITH(NOLOCK) WHERE FISNO='" + uretim.UretSon_FisNo + "'");
                        var fiyat = netRS.FieldByName("STHAR_BF").AsString;
                        SeriNoListesi.Add(seri);
                        StokHareket stok = kernel.yeniStokHareket(sirket);
                        stok.Stok_Kodu = uretim.UretSon_Mamul;
                        stok.Sthar_Aciklama = stokadi + "-" + uretim.UretSon_FisNo + "-" + jsonList[i].ISEMRINO.Substring(0, 4);
                        stok.Fisno = "Z" + fisno;
                        stok.Sthar_Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                        stok.Proje_Kodu = "1";
                        stok.Sthar_Gckod = "C";
                        stok.Sthar_Bf = fiyat.ToDouble();
                        stok.Sthar_Nf = fiyat.ToDouble();
                        stok.Sthar_Gcmik = jsonList[i].KULL_MIKTAR;
                        stok.Sthar_Gcmik2 = jsonList[i].MIKTAR2;
                        stok.DEPO_KODU = 45;
                        stok.Sthar_Htur = "A";
                        stok.kayitYeni();
                        StokHareket stok2 = kernel.yeniStokHareket(sirket);
                        stok2.Stok_Kodu = tip == "ikinci" ? "IKINCIKALITE" : "HURDA";
                        stok2.Sthar_Aciklama = stokadi + "-" + uretim.UretSon_FisNo + "-" + jsonList[i].ISEMRINO.Substring(0, 4);
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

        public class FisKontrol
        {
            public string KONTROL { get; set; }
        }

        public string TrpzUretim(string stokkodu, string ISEMRINO, string SERI_NO, string KULL_MIKTAR, string mik2, bool kontrol, string hurdamik, string ikincimik1, string ikincimik2, int dongu, bool sonMu)
        {
            var seri = "";

            string FisHata = "";
            if (ISEMRINO.Substring(0, 4) == "PB01" || ISEMRINO.Substring(0, 4) == "PB02" || ISEMRINO.Substring(0, 4) == "RF03" || ISEMRINO.Substring(0, 4) == "RF01" || ISEMRINO.Substring(0, 4) == "TP01")
            {
                var fisler = "";
                var sum = 0;
                var fisno = "";
                var  apiUrl2 = "";
                try
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    var topmik = 0;
                    if(ISEMRINO.Substring(0, 4) == "TP01")
                    {
                        topmik = Convert.ToInt32(KULL_MIKTAR) + Convert.ToInt32(ikincimik1) + Convert.ToInt32(hurdamik);
                    }
                    else
                    {
                        topmik = Convert.ToInt32(KULL_MIKTAR) + Convert.ToInt32(ikincimik1);
                    }
                    var topmik2 = Convert.ToInt32(mik2) + Convert.ToInt32(ikincimik2);
                    if(ISEMRINO.Substring(0, 4) == "TP01")
                    {
                        apiUrl2 = "http://192.168.2.13:83/api/ie/USK/" + ISEMRINO.Substring(0, 4) + "/" + stokkodu + "/"+ISEMRINO+"/" + topmik + "/" + topmik2 + "/" + 0;
                    }
                    else
                    {
                        apiUrl2 = "http://192.168.2.13:83/api/ie/USK/" + ISEMRINO.Substring(0, 4) + "/" + stokkodu + "/0/" + topmik + "/" + topmik2 + "/" + 0;
                    }
                    

                    //Connect API
                    Uri url2 = new Uri(apiUrl2);
                    WebClient client = new WebClient();
                    client.Encoding = System.Text.Encoding.UTF8;
                    var json2 = client.DownloadString(url2);
                    if(json2 != null)
                    {
                        sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                            "EFECE2023",
                                            "TEMELSET",
                                            "",
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);
                        List<USKModel> jsonList = ser.Deserialize<List<USKModel>>(json2);

                        for (var i = 0; i < jsonList.Count; i++)
                        {
                            double miktar2 = 0;
                            double miktar1 = 0;
                            if (i != jsonList.Count - 1)
                            {
                                var m2 = Math.Floor(jsonList[i].MIKTAR2);
                                miktar2 = m2;
                                sum += Convert.ToInt32(m2);
                                miktar1 = jsonList[i].KULL_MIKTAR;
                            }
                            else
                            {
                                if (sum != Convert.ToInt32(topmik2))
                                {
                                    var m2 = Math.Floor(jsonList[i].MIKTAR2);
                                    sum += Convert.ToInt32(m2);
                                    var f = sum - Convert.ToInt32(topmik2);
                                    if (sum != 0)
                                    {
                                        miktar2 = m2 - f;
                                        miktar1 = jsonList[i].KULL_MIKTAR;
                                    }
                                    else
                                    {

                                        miktar2 = m2 + Convert.ToInt32(ikincimik2);
                                        miktar1 = jsonList[i].KULL_MIKTAR;
                                    }
                                }
                            }
                        

                            netRS = kernel.yeniNetRS(sirket);
                            uretim = kernel.yeniSerbestUSK(sirket);
                            uretim.IsEmrindenGetir(jsonList[i].ISEMRINO);
                            uretim.UretSon_FisNo = jsonList[i].FIS_NO;

                            uretim.UretSon_Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                            uretim.BelgeTipi = TBelgeTipi.btIsEmri;
                            uretim.Proje_Kodu = "1";




                            uretim.UretSon_Depo = 45;
                            uretim.I_Yedek1 = 45;
                            uretim.I_Yedek2 = 0;
                            uretim.OTO_YMAM_GIRDI_CIKTI = true;
                            uretim.OTO_YMAM_STOK_KULLAN = false;

                            uretim.BAKIYE_DEPO = 0;
                            uretim.F_Yedek1 = miktar2;
                            uretim.UretSon_Miktar = miktar1;
                            netRS.Ac("SELECT * FROM TBLISEMRI WITH(NOLOCK)  WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");

                            seri = netRS.FieldByName("SERINO").AsString;
                            var Stokkodu = netRS.FieldByName("STOK_KODU").AsString;

                            NetRS SeriNoTakipRS = kernel.yeniNetRS(sirket);

                            SeriNoTakipRS.Ac($"SELECT * FROM TBLSTSABIT  WITH(NOLOCK) WHERE STOK_KODU='{jsonList[i].ISEMRINO}'");

                            var SeriNoTakip = SeriNoTakipRS.FieldByName("GIRIS_SERI").AsString;

                            if (SeriNoTakip == "E")
                            {

                                uretim.SeriEkle(jsonList[i].SERI_NO, "", "", "", uretim.UretSon_Miktar, uretim.F_Yedek1);

                            }
                            else
                            {
                                if (seri == null)
                                {

                                    uretim.OTOSERIURET();
                                    uretim.SeriEkle(uretim.SeriOku(0).Seri1, "", "", "", uretim.UretSon_Miktar, uretim.F_Yedek1);
                                }
                                else
                                {
                                    uretim.SeriEkle(seri, "", "", "", uretim.UretSon_Miktar, uretim.F_Yedek1);
                                }
                            }



                            netRS.Ac("UPDATE TBLISEMRIREC SET SERINO='" + jsonList[i].SERI_NO + "' WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");

                            fisno = uretim.UretSon_FisNo;

                          
                                uretim.FisUret();
                                uretim.Kaydet();
                            
                            if (i != jsonList.Count - 1)
                            {

                                fisler = fisler + "'" + fisno + "'" + ",";
                            }
                            else
                            {

                                fisler = fisler + "'" + fisno + "'";
                            }

                            netRS.Ac("SELECT * FROM TBLSERITRA WITH(NOLOCK) WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            var karsi = netRS.FieldByName("SERI_NO").AsString;

                            netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "' AND BELGENO<>'" + fisno + "' ORDER BY SIRA_NO DESC");
                            var mikold = netRS.FieldByName("MIKTAR").AsFloat;
                            netRS.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + fisno + "' AND  GCKOD='G'");

                            //if (eskimiktar!= jsonList[i].KULL_MIKTAR)
                            //{
                            //    netRS1.Ac("UPDATE TBLISEMRIREC SET MIKTAR=" + (eskimiktar - jsonList[i].KULL_MIKTAR) + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                            //}
                            netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE SERI_NO='" + jsonList[i].SERI_NO + "' AND GCKOD='G'");
                            var ACIK1 = netRS.FieldByName("ACIK1").AsString;
                            var ACIK2 = netRS.FieldByName("ACIK2").AsString;
                            var SERI_NO_3 = netRS.FieldByName("SERI_NO_3").AsString;
                            var SERI_NO_4 = netRS.FieldByName("SERI_NO_4").AsString;
                            netRS.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "',ACIK1='" + ACIK1 + "',ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "' WHERE BELGENO='" + fisno + "' AND SIPNO='" + jsonList[i].ISEMRINO + "'");

                            netRS.Ac("SELECT * FROM TBLISEMRI WITH(NOLOCK) WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                            var referans = netRS.FieldByName("REFISEMRINO").AsString;


                            //netRS.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + referans + "'");

                            //var eski = netRS.FieldByName("MIKTAR").AsFloat;
                            //var oran = KULL_MIKTAR.ToDouble() / eski;
                            //var miktar2 = netRS.FieldByName("ACIKLAMA").AsString;
                            //var yeni = miktar2.ToDouble() * oran;
                            //var m1 = "0";
                            //if (hurdamik != "0")
                            //{
                            //    m1 = (jsonList[i].KULL_MIKTAR - hurdamik.ToDouble()).ToString();
                            //}
                            //if (ikincimik1 != "0")
                            //{
                            //    m1 = (jsonList[i].KULL_MIKTAR - ikincimik1.ToDouble()).ToString();
                            //}
                            netRS.Ac("UPDATE TBLISEMRI SET MIKTAR='" + (mikold != 0 ? mikold + jsonList[i].KULL_MIKTAR : jsonList[i].KULL_MIKTAR) + "' WHERE ISEMRINO='" + referans + "'");



                            netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            seri = netRS.FieldByName("SERI_NO").AsString;
                            if (i == jsonList.Count - 1)
                            {
                                if (kontrol)
                                {
                                    Isemri = kernel.yeniIsEmri(sirket);
                                    Isemri.kayitOku(TOkumaTipi.otAc, "ISEMRINO = \'" + jsonList[i].ISEMRINO + "\'");
                                    Isemri.ReceteSaklansin = false;
                                    Isemri.Kapali = true;
                                    Isemri.kayitDuzelt();
                                }
                                netRS.Ac("SELECT * FROM TBLISEMRI WITH(NOLOCK) WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");

                                seri = netRS.FieldByName("SERINO").AsString;
                                var Stokkoduu = netRS.FieldByName("STOK_KODU").AsString;
                                netRS.Ac("SELECT TOP(1) * FROM TBLSERITRA WITH(NOLOCK) WHERE GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "' ORDER BY SIRA_NO DESC");
                                var karsii = netRS.FieldByName("SERI_NO").AsString;

                                Fatura fatura = default(Fatura);
                                FatUst fatUst = default(FatUst);
                                FatKalem fatKalem = default(FatKalem);
                                netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE SERI_NO='" + jsonList[i].SERI_NO + "' AND GCKOD='G'");
                                var ACIKK1 = netRS.FieldByName("ACIK1").AsString;
                                var ACIKK2 = netRS.FieldByName("ACIK2").AsString;
                                var SERI_NOO_3 = netRS.FieldByName("SERI_NO_3").AsString;
                                var SERI_NOO_4 = netRS.FieldByName("SERI_NO_4").AsString;
                                if (hurdamik != null && hurdamik != "0")
                                {
                                    fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);

                                    fatUst = fatura.Ust();
                                    fatUst.FATIRS_NO = fatura.YeniNumara("D");
                                    fatUst.CariKod = "12035200100406";
                                    fatUst.TIPI = TFaturaTipi.ft_Bos;
                                    fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                                    fatUst.Tarih = DateTime.Now;
                                    fatUst.FiiliTarih = DateTime.Now;
                                    fatUst.PLA_KODU = "45";
                                    fatUst.Proje_Kodu = "1";
                                    fatUst.KDV_DAHILMI = true;
                                    fatUst.Aciklama = fisno;
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

                                    fatKalem.SeriEkle(karsii, ACIKK1, ACIKK2, "", hurdamik.ToDouble(), 0, SERI_NOO_3, SERI_NOO_4);
                                    fatura.kayitYeni();
                                }
                                if (ikincimik1 != null && ikincimik1 != "0")
                                {
                                    fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);

                                    fatUst = fatura.Ust();
                                    fatUst.FATIRS_NO = fatura.YeniNumara("D");
                                    fatUst.CariKod = "12035200100406";
                                    fatUst.TIPI = TFaturaTipi.ft_Bos;
                                    fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                                    fatUst.Tarih = DateTime.Now;
                                    fatUst.FiiliTarih = DateTime.Now;
                                    fatUst.PLA_KODU = "45";
                                    fatUst.Proje_Kodu = "1";
                                    fatUst.Aciklama = fisno;
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
                                    fatKalem.SeriEkle(karsii, ACIKK1, ACIKK2, "", ikincimik1.ToDouble(), ikincimik2.ToDouble(), SERI_NOO_3, SERI_NOO_4);
                                    fatura.kayitYeni();
                                }

                                System.Diagnostics.Debug.WriteLine(fisler);
                                netRS.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO IN (" + fisler + ") AND GCKOD='G'");
                                netRS.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "' WHERE BELGENO IN (" + fisler + ")");

                                var mailList = "";
                                foreach(var itemlist in jsonList)
                                {
                                    mailList += JsonConvert.SerializeObject(itemlist) + "</br>";
                                }
                                WebMail.SmtpServer = "192.168.2.13";
                                WebMail.Send("ergunozbudakli@efecegalvaniz.com,ugurkonakci@efecegalvaniz.com,dincersipka@efecegalvaniz.com", "Fiş Bilgi", "<p><b>" + jsonList[i].ISEMRINO + "</b> Fiş bilgileri:</p><p>" + fisler + "</p>" + "</br>" + mailList, "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);

                                //netRS.Ac("UPDATE TBLSERITRA SET ACIK1='" + ACIKK1 + "',ACIK2='" + ACIKK2 + "',SERI_NO_3='" + SERI_NOO_3 + "',SERI_NO_4='" + SERI_NOO_4 + "' WHERE BELGENO='" + fisno + "' AND SIPNO='" + ISEMRINO + "'");

                                return karsii;
                            }


                        }
                    }
                   
                }
                catch (Exception exp)
                {
                    sirket.LogOff();
                    var message = exp.Message;
                    System.Diagnostics.Debug.Write(exp);
                    WebMail.SmtpServer = "192.168.2.13";
                    WebMail.Send("ergunozbudakli@efecegalvaniz.com,ugurkonakci@efecegalvaniz.com,dincersipka@efecegalvaniz.com", "Üretim Sonu Kaydı Hata", "<p><b>" + ISEMRINO + "</b> üzerinde bir hata oluştu!</p><p>Hata: " + message + "</p><p>Fiş bilgileri:</p><p>" + fisler + "</p>", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);
                   
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
                if (KULL_MIKTAR != "0")
                {
                    try
                    {


                        sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                                "EFECE2023",
                                                "TEMELSET",
                                                "",
                                                Request.Cookies["UserName"].Value,
                                                LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);




                    repeat:
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

                        netRS.Ac("SELECT * FROM TBLISEMRI WITH(NOLOCK)  WHERE ISEMRINO='" + ISEMRINO + "'");

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

                            //netRS.Ac("SELECT TOP(1) * FROM TBLSERITRA WHERE GCKOD='G' AND STOK_KODU='" + Stokkodu + "' ORDER BY BELGENO DESC");
                            //var s = netRS.FieldByName("SERI_NO").AsString;
                            //uretim.SeriEkle(s, "", "", "", KULL_MIKTAR.ToDouble(), mik2.ToDouble());
                            uretim.OTOSERIURET();
                            uretim.SeriEkle(uretim.SeriOku(0).Seri1, "", "", "", KULL_MIKTAR.ToDouble(), mik2.ToDouble());
                        }


                        if (SERI_NO != null && ISEMRINO.Substring(0, 2) != "MH")
                        {
                            netRS.Ac("UPDATE TBLISEMRIREC SET SERINO='" + SERI_NO + "' WHERE ISEMRINO='" + ISEMRINO + "'");
                        }
                        if (ISEMRINO == stokkodu + "23000001001" || ISEMRINO == stokkodu + "23000001002")
                        {
                            netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE GCKOD='G' AND SERI_NO='" + SERI_NO + "'");
                            var seritraStok = netRS.FieldByName("STOK_KODU").AsString;
                            netRS.Ac("UPDATE TBLISEMRIREC SET HAM_KODU='" + seritraStok + "' WHERE ISEMRINO='" + ISEMRINO + "'");
                            netRS.Ac("UPDATE TBLISEMRI SET KAPALI='H' WHERE ISEMRINO='" + ISEMRINO + "'");
                        }
                        var fisno = uretim.UretSon_FisNo;

                        //WebClient client2 = new WebClient();
                        //client2.Encoding = System.Text.Encoding.UTF8;

                        //var json2 = client2.DownloadString(new Uri($"http://192.168.2.13:83/api/uretim/kontrol/{fisno}"));

                        //var Response = JsonConvert.DeserializeObject<List<FisKontrol>>(json2);

                        //bool FIS_NO_VAR = Response?[0].KONTROL == "1";

                        //while (FIS_NO_VAR)
                        //{

                        //    fisno = uretim.SonFisNumarasi("N");
                        //    uretim.UretSon_FisNo = fisno;
                        //    json2 = client2.DownloadString(new Uri($"http://192.168.2.13:83/api/uretim/kontrol/{fisno}"));

                        //    Response = JsonConvert.DeserializeObject<List<FisKontrol>>(json2);
                        //    FIS_NO_VAR = Response?[0].KONTROL == "1";
                        //}
                        try
                        {
                            uretim.FisUret();
                            uretim.Kaydet();
                            if (TempData["FIS_NO"] != null)
                            {
                                TempData["FIS_NO"] = TempData["FIS_NO"].ToString() + "," + "'" + uretim.UretSon_FisNo + "'";
                            }
                            else
                            {
                                TempData["FIS_NO"] = "'" + uretim.UretSon_FisNo + "'";
                            }

                            TempData.Keep("FIS_NO");
                        }
                        catch (Exception exp)
                        {
                            if (exp.Message == "Fiş numarası sistemde kayıtlı. " + fisno)
                            {
                                goto repeat;
                            }
                        }




                        FisHata = (string)TempData["FIS_NO"];
                        if (ISEMRINO == stokkodu + "23000001001" || ISEMRINO == stokkodu + "23000001002")
                        {
                            netRS.Ac("UPDATE TBLISEMRIREC SET HAM_KODU='HURDA',SERINO=NULL WHERE ISEMRINO='" + ISEMRINO + "'");
                            netRS.Ac("UPDATE TBLISEMRI SET KAPALI='E' WHERE ISEMRINO='" + ISEMRINO + "'");
                        }


                        netRS.Ac("SELECT * FROM TBLSERITRA WITH(NOLOCK) WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
                        var karsi = netRS.FieldByName("SERI_NO").AsString;

                        netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "' AND BELGENO<>'" + fisno + "' ORDER BY SIRA_NO DESC");
                        var mikold = netRS.FieldByName("MIKTAR").AsFloat;
                        netRS.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + fisno + "' AND  GCKOD='G'");

                        //if (eskimiktar!= jsonList[i].KULL_MIKTAR)
                        //{
                        //    netRS1.Ac("UPDATE TBLISEMRIREC SET MIKTAR=" + (eskimiktar - jsonList[i].KULL_MIKTAR) + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                        //}
                        netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WHERE SERI_NO='" + SERI_NO + "' AND GCKOD='G'");
                        var ACIK1 = netRS.FieldByName("ACIK1").AsString;
                        var ACIK2 = netRS.FieldByName("ACIK2").AsString;
                        var SERI_NO_3 = netRS.FieldByName("SERI_NO_3").AsString;
                        var SERI_NO_4 = netRS.FieldByName("SERI_NO_4").AsString;
                        netRS.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "',ACIK1='" + ACIK1 + "',ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "' WHERE BELGENO='" + fisno + "' AND SIPNO='" + ISEMRINO + "'");

                        netRS.Ac("SELECT * FROM TBLISEMRI WITH(NOLOCK) WHERE ISEMRINO='" + ISEMRINO + "'");
                        var referans = netRS.FieldByName("REFISEMRINO").AsString;

                        //netRS.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + referans + "'");

                        //var eski = netRS.FieldByName("MIKTAR").AsFloat;
                        //var oran = KULL_MIKTAR.ToDouble() / eski;
                        //var miktar2 = netRS.FieldByName("ACIKLAMA").AsString;
                        //var yeni = miktar2.ToDouble() * oran;
                        if (hurdamik != "0")
                        {
                            KULL_MIKTAR = (KULL_MIKTAR.ToDouble() - hurdamik.ToDouble()).ToString();
                        }
                        if (ikincimik1 != "0")
                        {
                            KULL_MIKTAR = (KULL_MIKTAR.ToDouble() - ikincimik1.ToDouble()).ToString();
                        }
                        netRS.Ac("UPDATE TBLISEMRI SET MIKTAR='" + (mikold != 0 ? mikold + KULL_MIKTAR.ToDouble() : KULL_MIKTAR.ToDouble()) + "' WHERE ISEMRINO='" + referans + "'");



                        if (ISEMRINO.Substring(0, 2) == "MH" || ISEMRINO.Substring(0, 2) == "BK")
                        {
                            Isemri = kernel.yeniIsEmri(sirket);
                            Isemri.kayitOku(TOkumaTipi.otAc, "ISEMRINO = \'" + ISEMRINO + "\'");
                            Isemri.ReceteSaklansin = false;
                            Isemri.Kapali = true;
                            Isemri.kayitDuzelt();
                        }
                        netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
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
                            netRS.Ac("SELECT * FROM TBLISEMRI WITH(NOLOCK) WHERE ISEMRINO='" + ISEMRINO + "'");

                            seri = netRS.FieldByName("SERINO").AsString;
                            var Stokkoduu = netRS.FieldByName("STOK_KODU").AsString;
                            netRS.Ac("SELECT TOP(1) * FROM TBLSERITRA WITH(NOLOCK) WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "' ORDER BY SIRA_NO DESC");
                            var karsii = netRS.FieldByName("SERI_NO").AsString;

                            Fatura fatura = default(Fatura);
                            FatUst fatUst = default(FatUst);
                            FatKalem fatKalem = default(FatKalem);
                            netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE SERI_NO='" + SERI_NO + "' AND GCKOD='G'");
                            var ACIKK1 = netRS.FieldByName("ACIK1").AsString;
                            var ACIKK2 = netRS.FieldByName("ACIK2").AsString;
                            var SERI_NOO_3 = netRS.FieldByName("SERI_NO_3").AsString;
                            var SERI_NOO_4 = netRS.FieldByName("SERI_NO_4").AsString;
                            if (hurdamik != null && hurdamik != "0")
                            {
                                fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);

                                fatUst = fatura.Ust();
                                fatUst.FATIRS_NO = fatura.YeniNumara("D");
                                fatUst.CariKod = "12035200100406";
                                fatUst.TIPI = TFaturaTipi.ft_Bos;
                                fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                                fatUst.Tarih = DateTime.Now;
                                fatUst.FiiliTarih = DateTime.Now;
                                fatUst.PLA_KODU = "45";
                                fatUst.Proje_Kodu = "1";
                                fatUst.KDV_DAHILMI = true;
                                fatUst.Aciklama = fisno;
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

                                fatKalem.SeriEkle(karsii, ACIKK1, ACIKK2, "", hurdamik.ToDouble(), 0, SERI_NOO_3, SERI_NOO_4);
                                fatura.kayitYeni();
                            }
                            if (ikincimik1 != null && ikincimik1 != "0")
                            {
                                fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);

                                fatUst = fatura.Ust();
                                fatUst.FATIRS_NO = fatura.YeniNumara("D");
                                fatUst.CariKod = "12035200100406";
                                fatUst.TIPI = TFaturaTipi.ft_Bos;
                                fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                                fatUst.Tarih = DateTime.Now;
                                fatUst.FiiliTarih = DateTime.Now;
                                fatUst.PLA_KODU = "45";
                                fatUst.Proje_Kodu = "1";
                                fatUst.Aciklama = fisno;
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
                                fatKalem.SeriEkle(karsii, ACIKK1, ACIKK2, "", ikincimik1.ToDouble(), ikincimik2.ToDouble(), SERI_NOO_3, SERI_NOO_4);
                                fatura.kayitYeni();
                            }
                            var fisler = TempData["FIS_NO"];
                            System.Diagnostics.Debug.WriteLine(fisler);
                            netRS.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO IN (" + fisler + ") AND GCKOD='G'");
                            netRS.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "' WHERE BELGENO IN (" + fisler + ")");
                            TempData.Remove("FIS_NO");

                            WebMail.SmtpServer = "192.168.2.13";
                            WebMail.Send("ergunozbudakli@efecegalvaniz.com,ugurkonakci@efecegalvaniz.com,dincersipka@efecegalvaniz.com", "Fiş Bilgi", "<p><b>" + ISEMRINO + "</b> Fiş bilgileri:</p><p>" + (string)fisler + "</p>", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);

                            //netRS.Ac("UPDATE TBLSERITRA SET ACIK1='" + ACIKK1 + "',ACIK2='" + ACIKK2 + "',SERI_NO_3='" + SERI_NOO_3 + "',SERI_NO_4='" + SERI_NOO_4 + "' WHERE BELGENO='" + fisno + "' AND SIPNO='" + ISEMRINO + "'");

                            return karsii;
                        }


                    }
                    catch (Exception exp)
                    {
                        sirket.LogOff();
                        var message = exp.Message;
                        System.Diagnostics.Debug.Write(exp);
                        WebMail.SmtpServer = "192.168.2.13";
                        WebMail.Send("ergunozbudakli@efecegalvaniz.com,ugurkonakci@efecegalvaniz.com,dincersipka@efecegalvaniz.com", "Üretim Sonu Kaydı Hata", "<p><b>" + ISEMRINO + "</b> üzerinde bir hata oluştu!</p><p>Hata: " + message + "</p><p>Fiş bilgileri:</p><p>" + (string)FisHata + "</p>", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);
                        TempData.Remove("FIS_NO");

                        TempData.Remove("FIS_NO");
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
                                                "EFECE2023",
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
                        netRS.Ac("SELECT TOP(1) * FROM TBLSERITRA WITH(NOLOCK) WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "' ORDER BY SIRA_NO DESC");
                        var karsi = netRS.FieldByName("SERI_NO").AsString;
                        netRS.Ac("SELECT * FROM TBLSTOKURS WITH(NOLOCK) WHERE URETSON_SIPNO='" + ISEMRINO + "'");
                        var fisno = netRS.FieldByName("URETSON_FISNO").AsString;
                        Fatura fatura = default(Fatura);
                        FatUst fatUst = default(FatUst);
                        FatKalem fatKalem = default(FatKalem);
                        if (hurdamik != null && hurdamik != "0")
                        {
                            fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);

                            fatUst = fatura.Ust();
                            fatUst.FATIRS_NO = fatura.YeniNumara("D");
                            fatUst.TIPI = TFaturaTipi.ft_Bos;
                            fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                            fatUst.Tarih = DateTime.Now;
                            fatUst.FiiliTarih = DateTime.Now;
                            fatUst.PLA_KODU = "45";
                            fatUst.Proje_Kodu = "1";
                            fatUst.KDV_DAHILMI = true;
                            fatUst.Aciklama = fisno;
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
                            fatUst.FATIRS_NO = fatura.YeniNumara("D");
                            fatUst.TIPI = TFaturaTipi.ft_Bos;
                            fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                            fatUst.Tarih = DateTime.Now;
                            fatUst.FiiliTarih = DateTime.Now;
                            fatUst.PLA_KODU = "45";
                            fatUst.Proje_Kodu = "1";
                            fatUst.Aciklama = fisno;
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


                        netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE SERI_NO='" + SERI_NO + "' AND GCKOD='G'");
                        var ACIK1 = netRS.FieldByName("ACIK1").AsString;
                        var ACIK2 = netRS.FieldByName("ACIK2").AsString;
                        var SERI_NO_3 = netRS.FieldByName("SERI_NO_3").AsString;
                        var SERI_NO_4 = netRS.FieldByName("SERI_NO_4").AsString;
                        netRS.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "',ACIK1='" + ACIK1 + "',ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "' WHERE SIPNO='" + ISEMRINO + "'");
                        netRS.Ac("UPDATE TBLSERITRA SET ACIK1='" + ACIK1 + "',ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "' WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
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
                                            "EFECE2023",
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
                        netRS.Ac("SELECT TOP(1) * FROM TBLSERITRA WITH(NOLOCK) WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "' ORDER BY BELGENO DESC");
                        var s = netRS.FieldByName("SERI_NO").AsString;
                        uretim.SeriEkle(s, "", "", "", KULL_MIKTAR.ToDouble(), mik2.ToDouble());
                    }


                    if (SERI_NO != null && ISEMRINO.Substring(0, 2) != "MH")
                    {
                        netRS.Ac("UPDATE TBLISEMRIREC SET SERINO='" + SERI_NO + "' WHERE ISEMRINO='" + ISEMRINO + "'");
                    }
                    if (ISEMRINO == stokkodu + "23000001001" || ISEMRINO == stokkodu + "23000001002")
                    {
                        netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE GCKOD='G' AND SERI_NO='" + SERI_NO + "'");
                        var seritraStok = netRS.FieldByName("STOK_KODU").AsString;
                        netRS.Ac("UPDATE TBLISEMRIREC SET HAM_KODU='" + seritraStok + "' WHERE ISEMRINO='" + ISEMRINO + "'");
                        netRS.Ac("UPDATE TBLISEMRI SET KAPALI='H' WHERE ISEMRINO='" + ISEMRINO + "'");
                    }
                    var fisno = uretim.UretSon_FisNo;
                    //netRS.Ac("SELECT * FROM TBLSTOKURS WHERE URETSON_FISNO='" + fisno + "'");
                    //if (netRS.EOF)
                    //{
                    //    uretim.UretSon_FisNo = uretim.SonFisNumarasi("N");
                    //    fisno = uretim.UretSon_FisNo;
                    //}
                    uretim.FisUret();
                    uretim.Kaydet();
                    if (ISEMRINO == stokkodu + "23000001001" || ISEMRINO == stokkodu + "23000001002")
                    {
                        netRS.Ac("UPDATE TBLISEMRIREC SET HAM_KODU='HURDA',SERINO=NULL WHERE ISEMRINO='" + ISEMRINO + "'");
                        netRS.Ac("UPDATE TBLISEMRI SET KAPALI='E' WHERE ISEMRINO='" + ISEMRINO + "'");
                    }


                    netRS.Ac("SELECT * FROM TBLSERITRA WITH(NOLOCK) WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
                    var karsi = netRS.FieldByName("SERI_NO").AsString;

                    netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE GCKOD='G' AND SIPNO='" + ISEMRINO + "' AND BELGENO<>'" + uretim.UretSon_FisNo + "' ORDER BY BELGENO DESC");
                    var mikold = netRS.FieldByName("MIKTAR").AsFloat;
                    netRS.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + ISEMRINO + "'");

                    //if (eskimiktar!= jsonList[i].KULL_MIKTAR)
                    //{
                    //    netRS1.Ac("UPDATE TBLISEMRIREC SET MIKTAR=" + (eskimiktar - jsonList[i].KULL_MIKTAR) + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                    //}
                    netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE SERI_NO='" + SERI_NO + "' AND GCKOD='G'");
                    var ACIK1 = netRS.FieldByName("ACIK1").AsString;
                    var ACIK2 = netRS.FieldByName("ACIK2").AsString;
                    var SERI_NO_3 = netRS.FieldByName("SERI_NO_3").AsString;
                    var SERI_NO_4 = netRS.FieldByName("SERI_NO_4").AsString;
                    netRS.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "',ACIK1='" + ACIK1 + "',ACIK2='" + ACIK2 + "',SERI_NO_3='" + SERI_NO_3 + "',SERI_NO_4='" + SERI_NO_4 + "',MIKTAR2="+ mik2.ToDouble() + " WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + ISEMRINO + "'");

                    netRS.Ac("SELECT * FROM TBLISEMRI WITH(NOLOCK) WHERE ISEMRINO='" + ISEMRINO + "'");
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
                    netRS.Ac("SELECT TOP(1)* FROM TBLSERITRA WITH(NOLOCK) WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND GCKOD='G' AND SIPNO='" + ISEMRINO + "'");
                    seri = netRS.FieldByName("SERI_NO").AsString;

                    netRS.Ac("SELECT * FROM TBLISEMRI WITH(NOLOCK) WHERE ISEMRINO='" + ISEMRINO + "'");

                    seri = netRS.FieldByName("SERINO").AsString;
                    seri = karsi;
                    fisno = uretim.UretSon_FisNo;
                    Fatura fatura = default(Fatura);
                    FatUst fatUst = default(FatUst);
                    FatKalem fatKalem = default(FatKalem);
                    if (hurdamik != null && hurdamik != "0")
                    {
                        fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);

                        fatUst = fatura.Ust();
                        fatUst.FATIRS_NO = fatura.YeniNumara("D");
                        fatUst.TIPI = TFaturaTipi.ft_Bos;
                        fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                        fatUst.Tarih = DateTime.Now;
                        fatUst.FiiliTarih = DateTime.Now;
                        fatUst.PLA_KODU = "45";
                        fatUst.Proje_Kodu = "1";
                        fatUst.KDV_DAHILMI = true;
                        fatUst.Aciklama = fisno;
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
                        fatUst.FATIRS_NO = fatura.YeniNumara("D");
                        fatUst.TIPI = TFaturaTipi.ft_Bos;
                        fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                        fatUst.Tarih = DateTime.Now;
                        fatUst.FiiliTarih = DateTime.Now;
                        fatUst.PLA_KODU = "45";
                        fatUst.Proje_Kodu = "1";
                        fatUst.Aciklama = fisno;
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



                    WebMail.SmtpServer = "192.168.2.13";
                    WebMail.Send("ergunozbudakli@efecegalvaniz.com,ugurkonakci@efecegalvaniz.com,dincersipka@efecegalvaniz.com", "Fiş Bilgi", "<p><b>" + ISEMRINO + "</b> Fiş bilgileri:</p><p>" + uretim.UretSon_FisNo + "</p>" , "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);



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
        public string DepoTransfer(string seri, string stokkodu, string miktar, string depo)
        {

            try
            {
                Fatura fatura = default(Fatura);
                FatUst fatUst = default(FatUst);
                FatKalem fatKalem = default(FatKalem);
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                           "EFECE2023",
                                           "TEMELSET",
                                           "",
                                           Request.Cookies["UserName"].Value,
                                           LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);
                fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);

                fatUst = fatura.Ust();
                fatUst.FATIRS_NO = fatura.YeniNumara("D");
                fatUst.CariKod = "12035200100406";
                fatUst.TIPI = TFaturaTipi.ft_Bos;
                fatUst.AMBHARTUR = TAmbarHarTur.htUretim;
                fatUst.Tarih = DateTime.Now;
                fatUst.FiiliTarih = DateTime.Now;
                fatUst.PLA_KODU = "45";
                fatUst.Proje_Kodu = "1";
                fatUst.KDV_DAHILMI = true;
                fatUst.Aciklama = "SERI DUZELTME";
                fatKalem = fatura.kalemYeni(stokkodu);

                ///Giriş Depo Kodu
                fatKalem.Gir_Depo_Kodu = depo.ToInt();
                fatKalem.DEPO_KODU = 45;
                fatKalem.STra_GCMIK = miktar.ToDouble();
                fatKalem.STra_BF = 0;
                fatKalem.STra_ACIK = "SERI DUZELTME";
                fatKalem.Olcubr = 1;
                fatKalem.ProjeKodu = "1";
                fatKalem.D_YEDEK10 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

                fatKalem.SeriEkle(seri, "", "", "", miktar.ToDouble(), 0);
                fatura.kayitYeni();
            }
            catch (Exception ex)
            {
                return "Hata: " + ex.Message;
            }
            finally
            {
                sirket.LogOff();
                Marshal.ReleaseComObject(sirket);
                kernel.FreeNetsisLibrary();
                Marshal.ReleaseComObject(kernel);
            }
            return "Başarılı";
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
                                            "EFECE2023",
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

        #region UretilmisBarkodlar

        [HttpPost]
        public string UretilmisEtiketleriYazdir(List<string> BarkodListesi, string EtiketDizayn, bool DirektYazdir, string Yazici, bool Onizle = false)
        {
            try
            {
                if (!(DirektYazdir || Onizle))
                    return "";

                List<Etiket> EtiketBilgileri = UretilmisEtiketBilgileri(BarkodListesi);

                Dictionary<string, string> Etiket;

                switch (EtiketDizayn)
                {
                    case "Uretim":
                        Etiket = UretimEtiket(EtiketBilgileri);
                        break;
                    case "Sevkiyat":
                        Etiket = SevkiyatEtiket(EtiketBilgileri);
                        break;
                    case "DL01Uretim":
                        Etiket = DL01UretimEtiket(EtiketBilgileri);
                        break;
                    default:
                        return "HATA: Yanlış etiketi türü.";
                }

                if (Etiket != null)
                {
                    if (Onizle)
                    {
                        return System.IO.Path.GetFileName(Etiket["Path"]);
                    }
                    else if (DirektYazdir)
                    {
                        try
                        {
                            using (var pdocument = PdfiumViewer.PdfDocument.Load(Etiket["Path"]))
                            {
                                using (var printDocument = pdocument.CreatePrintDocument())
                                {
                                    printDocument.PrinterSettings.PrintFileName = "Report_9ae93aa7-4359-444e-a033-eb5bf17f5ce6.pdf";
                                    printDocument.PrinterSettings.PrinterName = Yazici;
                                    printDocument.DocumentName = "file.pdf";
                                    printDocument.PrinterSettings.PrintFileName = "file.pdf";
                                    printDocument.PrintController = new StandardPrintController();
                                    printDocument.Print();
                                }
                            }

                            var httpClient1 = new HttpClient();

                            foreach (string Barkod in BarkodListesi) 
                            {
                                var apiUrl1 = $"http://192.168.2.13:83/api/uretim/Yazdirilanlar/{Barkod}";

                                var request1 = new HttpRequestMessage(HttpMethod.Get, apiUrl1) { };

                                httpClient1.SendAsync(request1);
                            }
                            
                            return "Etiket başarıyla yazdırıldı.";
                        }
                        catch (Exception ex)
                        {
                            return $"HATA: Etiket yazdırma işleminde bir hata oluştu. (Detay: {ex.Message})";
                        }
                    }
                    else
                    {
                        return "Etiket başarıyla oluşturuldu.";
                    }
                }
                else
                {
                    return "HATA: Etiket verilerinde bir sorun oluştu.";
                }
            }
            catch (Exception ex)
            {
                return $"HATA: Etiket oluşturulurken bir hata oluştu. (Detay: {ex.Message})";
            }
        }

        public List<Etiket> UretilmisEtiketBilgileri(List<string> BarkodListesi)
        {
            try
            {
                List<Etiket> Etiketler = new List<Etiket>();
                WebClient Client = new WebClient() { Encoding = Encoding.UTF8 };

                foreach (var item in BarkodListesi)
                {
                    string Response = Client.DownloadString(new Uri("http://192.168.2.13:83/api/seri/kontrol/" + item));
                    List<Etiket> Result = JsonConvert.DeserializeObject<List<Etiket>>(Response, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                    if (Result.Count > 0)
                    {
                        Etiketler.Add(Result[0]);
                    }
                }

                return Etiketler;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Dictionary<string, string> UretimEtiket(List<Etiket> EtiketBilgileri)
        {
            if (EtiketBilgileri.Count <= 0)
                return null;

            iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(246.61417323f, 462.04724409f), 0, 0, 0, 0);

            string imagePath = System.IO.Path.Combine(Server.MapPath("~\\Outputs\\Sources"), "EFECE_ETIKET.png");
            string pdfPath = System.IO.Path.Combine(Server.MapPath("~\\Outputs\\Labels\\Production"), $"{DateTime.UtcNow.ToUnixTime()}.pdf");

            FileStream Memory = new FileStream(pdfPath, FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, Memory);

            document.Open();

            PdfContentByte cb = writer.DirectContent;

            iTextSharp.text.Font fontNormal = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 11, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fontBoldHeader = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 13, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fontBoldContent = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 11, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fontBoldTarget = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 16, iTextSharp.text.Font.BOLD);



            for (int i = 0; i < EtiketBilgileri.Count; i++)
            {
                document.NewPage();

                if(EtiketBilgileri[i].MAK_KODU == "DL01")
                {
                    fontBoldHeader = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 13, iTextSharp.text.Font.BOLD);
                    fontBoldContent = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 13, iTextSharp.text.Font.BOLD);
                    fontBoldTarget = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 16, iTextSharp.text.Font.BOLD);

                    iTextSharp.text.Image BackgroundImage = iTextSharp.text.Image.GetInstance(imagePath);
                    BackgroundImage.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
                    BackgroundImage.Alignment = iTextSharp.text.Image.UNDERLYING;
                    BackgroundImage.SetAbsolutePosition(0, 0);
                    document.Add(BackgroundImage);
                }

                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = 225;
                table.SetWidths(new float[] { 4.25f, 0.5f, 5.25f });

                // Header row.
                table.AddCell(GetHeaderCell(EtiketBilgileri[i].SERI_NO, fontBoldHeader, 3, 1));
                table.AddCell(GetHeaderCell(EtiketBilgileri[i].STOK_ADI, fontBoldHeader, 3, 2));

                PdfPCell cellBlankRow = new PdfPCell(new Phrase(" "));
                cellBlankRow.Border = 0;
                cellBlankRow.Colspan = 3;
                cellBlankRow.HorizontalAlignment = 1;
                table.AddCell(cellBlankRow);

                // Inner middle row.
                table.AddCell(GetContentCell("MİKTAR", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{MiktarFormat(EtiketBilgileri[i].MIKTAR1, EtiketBilgileri[i].OLCU_BR1)}", fontBoldContent));


                if ((EtiketBilgileri[i].OLCU_BR1 != EtiketBilgileri[i].OLCU_BR2) && EtiketBilgileri[i].MIKTAR2 != 0)
                {
                    table.AddCell(GetContentCell("MİKTAR 2", fontBoldContent));
                    table.AddCell(GetContentCell(":", fontBoldContent));
                    table.AddCell(GetContentCell($"{MiktarFormat(EtiketBilgileri[i].MIKTAR2, EtiketBilgileri[i].OLCU_BR2)}", fontBoldContent));
                }
                else
                {
                    table.AddCell(GetContentCell("MİKTAR 2", fontBoldContent));
                    table.AddCell(GetContentCell(":", fontBoldContent));
                    table.AddCell(GetContentCell($"-", fontBoldContent));
                }


                table.AddCell(GetContentCell("KALINLIK", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{(EtiketBilgileri[i].MAK_KODU == "DL01" ? BosDegerKontrolu(EtiketBilgileri[i].KALINLIK) : "-")}", fontBoldContent));

                table.AddCell(GetContentCell("GENİŞLİK", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{(EtiketBilgileri[i].MAK_KODU == "DL01" || EtiketBilgileri[i].MAK_KODU == "BK01" ? DegerFormat(EtiketBilgileri[i].GENISLIK, "GENISLIK") : "-")}", fontBoldContent));

                table.AddCell(GetContentCell("BOY", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{DegerFormat((int)EtiketBilgileri[i].BOY, "BOY")}", fontBoldContent));

                table.AddCell(GetContentCell("METRAJ", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{(EtiketBilgileri[i].MAK_KODU == "DL01" ? MetrajFormat(EtiketBilgileri[i].METRAJ, "M") : "-")}", fontBoldContent));

                table.AddCell(GetContentCell("TARİH/SAAT", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{TarihFormat(EtiketBilgileri[i].KAYITTARIHI)}", fontBoldContent));

                table.AddCell(GetContentCell("MAKİNE/OPR.", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{BosDegerKontrolu(EtiketBilgileri[i].MAK_KODU)}/{BosDegerKontrolu(EtiketBilgileri[i].KAYITYAPANKUL)}", fontBoldContent));

                table.AddCell(GetContentCell("MÜŞTERİ", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{BosDegerKontrolu(EtiketBilgileri[i].SIPARIS_CARI)}", fontBoldContent));

                table.WriteSelectedRows(0, -1, 15, 390, cb);

                PdfPTable table2 = new PdfPTable(3);
                table2.TotalWidth = 150;
                table2.SetWidths(new float[] { 3f, 0.5f, 6.5f });

                if (EtiketBilgileri[i].REF_HAT_KODU != null)
                {
                    table2.AddCell(GetContentCell("HEDEF", fontBoldContent));
                    table2.AddCell(GetHeaderCellLeft(":", fontBoldContent));
                    table2.AddCell(GetContentCell($"{BosDegerKontrolu(EtiketBilgileri[i].REF_HAT_KODU)}", fontBoldContent));
                    table2.AddCell(GetHeaderCellLeft($"{BosDegerKontrolu(EtiketBilgileri[i].REF_STOK_ADI).Substring(0, BosDegerKontrolu(EtiketBilgileri[i].REF_STOK_ADI).Length >= 75 ? 75 : BosDegerKontrolu(EtiketBilgileri[i].REF_STOK_ADI).Length)}", fontBoldTarget, colSpan: 3));
                }

                table2.WriteSelectedRows(0, -1, 15, 145, cb);

                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(EtiketBilgileri[i].SERI_NO, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                System.Drawing.Image qrCodeImage = qrCode.GetGraphic(45, System.Drawing.Color.Black, System.Drawing.Color.Transparent, true);

                iTextSharp.text.Image QR = iTextSharp.text.Image.GetInstance(ImageToByteArray(qrCodeImage));
                QR.ScaleToFit(85, 85);
                QR.Alignment = iTextSharp.text.Image.UNDERLYING;
                QR.SetAbsolutePosition(155, 70);
                document.Add(QR);
            }

            document.Close();
            Memory.Close();

            return new Dictionary<string, string>()
            {
                {"Path", pdfPath },
                {"Base64", ToBase64String(pdfPath) }
            };
        }

        public Dictionary<string, string> SevkiyatEtiket(List<Etiket> EtiketBilgileri)
        {
            if (EtiketBilgileri.Count <= 0)
                return null;

            iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(246.61417323f, 462.04724409f), 10f, 10f, 10f, 10f);

            string imagePath = System.IO.Path.Combine(Server.MapPath("~\\Outputs\\Sources"), "EFECE_ETIKET.png");
            string pdfPath = System.IO.Path.Combine(Server.MapPath("~\\Outputs\\Labels\\Shipment"), $"{DateTime.UtcNow.ToUnixTime()}.pdf");

            FileStream Memory = new FileStream(pdfPath, FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, Memory);

            document.Open();

            PdfContentByte cb = writer.DirectContent;

            iTextSharp.text.Font fontNormal = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 11, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fontBoldHeader = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 13, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fontBoldContent = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 11, iTextSharp.text.Font.BOLD);

            for (int i = 0; i < EtiketBilgileri.Count; i++)
            {
                document.NewPage();

                iTextSharp.text.Image BackgroundImage = iTextSharp.text.Image.GetInstance(imagePath);
                BackgroundImage.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
                BackgroundImage.Alignment = iTextSharp.text.Image.UNDERLYING;
                BackgroundImage.SetAbsolutePosition(0, 0);
                document.Add(BackgroundImage);

                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = 210; 
                table.SetWidths(new float[] { 4f, 0.5f, 5.5f });

                // Header row.
                table.AddCell(GetHeaderCell(EtiketBilgileri[i].SERI_NO, fontBoldHeader, 3, 1));
                table.AddCell(GetHeaderCell(EtiketBilgileri[i].STOK_ADI, fontBoldHeader, 3, 2));

                PdfPCell cellBlankRow = new PdfPCell(new Phrase(" "));
                cellBlankRow.Border = 0;
                cellBlankRow.Colspan = 3;
                cellBlankRow.HorizontalAlignment = 1;
                table.AddCell(cellBlankRow);

                // Inner middle row.
                table.AddCell(GetContentCell("GRUP İSİM", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{BosDegerKontrolu(EtiketBilgileri[i].GRUP_ISIM)}", fontBoldContent));

                table.AddCell(GetContentCell("MİKTAR 1", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{BosDegerKontrolu(EtiketBilgileri[i].MIKTAR1)} {BosDegerKontrolu(EtiketBilgileri[i].OLCU_BR1)}", fontBoldContent));

                if (EtiketBilgileri[i].OLCU_BR1 != EtiketBilgileri[i].OLCU_BR2)
                {
                    table.AddCell(GetContentCell("MİKTAR 2", fontBoldContent));
                    table.AddCell(GetContentCell(":", fontBoldContent));
                    table.AddCell(GetContentCell($"{BosDegerKontrolu(EtiketBilgileri[i].MIKTAR2)} {BosDegerKontrolu(EtiketBilgileri[i].OLCU_BR2)}", fontBoldContent));

                    table.AddCell(GetContentCell("BİRİM MİKTAR", fontBoldContent));
                    table.AddCell(GetContentCell(":", fontBoldContent));
                    table.AddCell(GetContentCell($"{BosDegerKontrolu(EtiketBilgileri[i].MIKTAR1/ EtiketBilgileri[i].MIKTAR2)} {BosDegerKontrolu(EtiketBilgileri[i].OLCU_BR1)}/{BosDegerKontrolu(EtiketBilgileri[i].OLCU_BR2)}", fontBoldContent));
                }
                else
                {
                    table.AddCell(GetContentCell("MİKTAR 2", fontBoldContent));
                    table.AddCell(GetContentCell(":", fontBoldContent));
                    table.AddCell(GetContentCell($"-", fontBoldContent));

                    table.AddCell(GetContentCell("BİRİM MİKTAR", fontBoldContent));
                    table.AddCell(GetContentCell(":", fontBoldContent));
                    table.AddCell(GetContentCell($"-", fontBoldContent));
                }

                table.AddCell(GetContentCell("KALINLIK", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{BosDegerKontrolu(EtiketBilgileri[i].KALINLIK)}", fontBoldContent));

                table.AddCell(GetContentCell("GENİŞLİK", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{BosDegerKontrolu(EtiketBilgileri[i].GENISLIK)}", fontBoldContent));

                table.AddCell(GetContentCell("KALİTE", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{BosDegerKontrolu(EtiketBilgileri[i].KALITE)}", fontBoldContent));

                table.AddCell(GetContentCell("KAPLAMA", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{BosDegerKontrolu(EtiketBilgileri[i].KAPLAMA)}", fontBoldContent));

                table.AddCell(GetContentCell("TARİH", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{TarihFormat(EtiketBilgileri[i].KAYITTARIHI)}", fontBoldContent));

                table.AddCell(GetContentCell("MENŞEİ/SN", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{BosDegerKontrolu(EtiketBilgileri[i].MENSEI?.Split(' ')[0])}/{BosDegerKontrolu(EtiketBilgileri[i].FIRMA_SERI_NO)}", fontBoldContent));


                table.AddCell(GetContentCell("ESKİ BOBİN NO", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{BosDegerKontrolu(EtiketBilgileri[i].ACIKLAMA_5)}", fontBoldContent));

                table.WriteSelectedRows(0, -1, 20, 390, cb);

                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(EtiketBilgileri[i].SERI_NO, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                System.Drawing.Image qrCodeImage = qrCode.GetGraphic(45, System.Drawing.Color.Black, System.Drawing.Color.Transparent, true);

                iTextSharp.text.Image QR = iTextSharp.text.Image.GetInstance(ImageToByteArray(qrCodeImage));
                QR.ScaleToFit(85, 85);
                QR.Alignment = iTextSharp.text.Image.UNDERLYING;
                QR.SetAbsolutePosition(155, 60);
                document.Add(QR);
            }

            document.Close();
            Memory.Close();

            return new Dictionary<string, string>()
            {
                {"Path", pdfPath },
                {"Base64", ToBase64String(pdfPath) }
            };
        }

        public Dictionary<string, string> DL01UretimEtiket(List<Etiket> EtiketBilgileri)
        {
            if (EtiketBilgileri.Count <= 0)
                return null;

            iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(246.61417323f, 462.04724409f), 0, 0, 0, 0);

            string imagePath = System.IO.Path.Combine(Server.MapPath("~\\Outputs\\Sources"), "EFECE_ETIKET.png");
            string pdfPath = System.IO.Path.Combine(Server.MapPath("~\\Outputs\\Labels\\Production"), $"{DateTime.UtcNow.ToUnixTime()}.pdf");

            FileStream Memory = new FileStream(pdfPath, FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, Memory);

            document.Open();

            PdfContentByte cb = writer.DirectContent;

            iTextSharp.text.Font fontNormal = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 13, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fontBoldHeader = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 13, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fontBoldContent = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 13, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fontBoldTarget = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 16, iTextSharp.text.Font.BOLD);

            for (int i = 0; i < EtiketBilgileri.Count; i++)
            {
                document.NewPage();

                if (EtiketBilgileri[i].MAK_KODU == "DL01")
                {
                    iTextSharp.text.Image BackgroundImage = iTextSharp.text.Image.GetInstance(imagePath);
                    BackgroundImage.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
                    BackgroundImage.Alignment = iTextSharp.text.Image.UNDERLYING;
                    BackgroundImage.SetAbsolutePosition(0, 0);
                    document.Add(BackgroundImage);
                }

                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = 225;
                table.SetWidths(new float[] { 4.25f, 0.5f, 5.25f });

                // Header row.
                table.AddCell(GetHeaderCell(EtiketBilgileri[i].SERI_NO, fontBoldHeader, 3, 1));
                table.AddCell(GetHeaderCell(EtiketBilgileri[i].STOK_ADI, fontBoldHeader, 3, 2));

                PdfPCell cellBlankRow = new PdfPCell(new Phrase(" "));
                cellBlankRow.Border = 0;
                cellBlankRow.Colspan = 3;
                cellBlankRow.HorizontalAlignment = 1;
                table.AddCell(cellBlankRow);

                // Inner middle row.
                table.AddCell(GetContentCell("MİKTAR", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{MiktarFormat(EtiketBilgileri[i].MIKTAR1, EtiketBilgileri[i].OLCU_BR1)}", fontBoldContent));


                if ((EtiketBilgileri[i].OLCU_BR1 != EtiketBilgileri[i].OLCU_BR2) && EtiketBilgileri[i].MIKTAR2 != 0)
                {
                    table.AddCell(GetContentCell("MİKTAR 2", fontBoldContent));
                    table.AddCell(GetContentCell(":", fontBoldContent));
                    table.AddCell(GetContentCell($"{MiktarFormat(EtiketBilgileri[i].MIKTAR2, EtiketBilgileri[i].OLCU_BR2)}", fontBoldContent));
                }
                else
                {
                    table.AddCell(GetContentCell("MİKTAR 2", fontBoldContent));
                    table.AddCell(GetContentCell(":", fontBoldContent));
                    table.AddCell(GetContentCell($"-", fontBoldContent));
                }


                table.AddCell(GetContentCell("KALINLIK", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{(EtiketBilgileri[i].MAK_KODU == "DL01" ? BosDegerKontrolu(EtiketBilgileri[i].KALINLIK) : "-")}", fontBoldContent));

                table.AddCell(GetContentCell("GENİŞLİK", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{(EtiketBilgileri[i].MAK_KODU == "DL01" || EtiketBilgileri[i].MAK_KODU == "BK01" ? DegerFormat(EtiketBilgileri[i].GENISLIK, "GENISLIK") : "-")}", fontBoldContent));

                table.AddCell(GetContentCell("BOY", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{DegerFormat((int)EtiketBilgileri[i].BOY, "BOY")}", fontBoldContent));

                table.AddCell(GetContentCell("METRAJ", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{(EtiketBilgileri[i].MAK_KODU == "DL01" ? MetrajFormat(EtiketBilgileri[i].METRAJ, "M") : "-")}", fontBoldContent));

                table.AddCell(GetContentCell("TARİH/SAAT", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{TarihFormat(EtiketBilgileri[i].KAYITTARIHI)}", fontBoldContent));

                table.AddCell(GetContentCell("MAKİNE/OPR.", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{BosDegerKontrolu(EtiketBilgileri[i].MAK_KODU)}/{BosDegerKontrolu(EtiketBilgileri[i].KAYITYAPANKUL)}", fontBoldContent));

                table.AddCell(GetContentCell("MÜŞTERİ", fontBoldContent));
                table.AddCell(GetContentCell(":", fontBoldContent));
                table.AddCell(GetContentCell($"{BosDegerKontrolu(EtiketBilgileri[i].SIPARIS_CARI)}", fontBoldContent));

                table.WriteSelectedRows(0, -1, 15, 390, cb);

                PdfPTable table2 = new PdfPTable(3);
                table2.TotalWidth = 150;
                table2.SetWidths(new float[] { 3f, 0.5f, 6.5f });

                if (EtiketBilgileri[i].REF_HAT_KODU != null)
                {
                    table2.AddCell(GetContentCell("HEDEF", fontBoldContent));
                    table2.AddCell(GetHeaderCellLeft(":", fontBoldContent));
                    table2.AddCell(GetContentCell($"{BosDegerKontrolu(EtiketBilgileri[i].REF_HAT_KODU)}", fontBoldContent));
                    table2.AddCell(GetHeaderCellLeft($"{BosDegerKontrolu(EtiketBilgileri[i].REF_STOK_ADI).Substring(0, BosDegerKontrolu(EtiketBilgileri[i].REF_STOK_ADI).Length >= 75 ? 75 : BosDegerKontrolu(EtiketBilgileri[i].REF_STOK_ADI).Length)}", fontBoldTarget, colSpan: 3));
                }

                table2.WriteSelectedRows(0, -1, 15, 145, cb);

                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(EtiketBilgileri[i].SERI_NO, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                System.Drawing.Image qrCodeImage = qrCode.GetGraphic(45, System.Drawing.Color.Black, System.Drawing.Color.Transparent, true);

                iTextSharp.text.Image QR = iTextSharp.text.Image.GetInstance(ImageToByteArray(qrCodeImage));
                QR.ScaleToFit(85, 85);
                QR.Alignment = iTextSharp.text.Image.UNDERLYING;
                QR.SetAbsolutePosition(155, 70);
                document.Add(QR);
            }

            document.Close();
            Memory.Close();

            return new Dictionary<string, string>()
            {
                {"Path", pdfPath },
                {"Base64", ToBase64String(pdfPath) }
            };
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

        private string DegerFormat(double? Deger, string Tip)
        {
            if (Deger == 0 || Deger == null)
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

        private string BosDegerKontrolu(string Deger)
        {
            return (string.IsNullOrEmpty(Deger) || string.IsNullOrWhiteSpace(Deger)) ? "-" : Deger;
        }

        private string BosDegerKontrolu(double? Deger)
        {
            return (Deger == 0 || Deger == 0.0 || Deger == null) ? "-" : Deger.ToString();
        }

        private string TarihFormat(DateTime? Tarih)
        {
            return Tarih is null ? "-" : Tarih?.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private string MiktarFormat(double? Miktar, string OlcuBirimi)
        {
            if (Miktar == 0 || Miktar == null)
            {
                return "-";
            }

            return $"{Miktar?.ToString("c", new CultureInfo("tr-TR"))} {OlcuBirimi}";
        }
       
        private string MetrajFormat(double? Metraj, string OlcuBirimi)
        {
            if (Metraj == 0 || Metraj == null)
            {
                return "-";
            }

            return $"{Metraj?.ToString("c", new CultureInfo("tr-TR"))} {OlcuBirimi}";
        }

        private PdfPCell GetHeaderCell(string text, iTextSharp.text.Font font, int colSpan = 1, int rowSpan = 1)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.Border = 0;
            cell.HorizontalAlignment = 1;
            cell.Colspan = colSpan;
            cell.Rowspan = rowSpan;

            return cell;
        }

        private PdfPCell GetHeaderCellLeft(string text, iTextSharp.text.Font font, int colSpan = 1, int rowSpan = 1)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.Border = 0;
            cell.HorizontalAlignment = 0;
            cell.Colspan = colSpan;
            cell.Rowspan = rowSpan;

            return cell;
        }

        private PdfPCell GetContentCell(string text, iTextSharp.text.Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = 0;
            cell.Border = 0;

            return cell;
        }
        #endregion

        #region Hurda & 2. Kalite Transfer

        public async Task<ActionResult> HurdaVeIkinciKaliteTransfer()
        {
            int moduleId = 23;

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

            ViewBag.Id = Request.Cookies["Id"].Value;


            return View();
        }

        #endregion
    }
}