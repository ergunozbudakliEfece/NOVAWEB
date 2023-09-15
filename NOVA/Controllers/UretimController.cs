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
        // GET: Uretim
        public ActionResult Index()
        {
            ViewBag.Makine = GetMak();
            ViewBag.Girdi = GetGirdi();
            ViewBag.Cikti = GetCikti();
            ViewBag.Id = Request.Cookies["Id"].Value;

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
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 26);
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
            var yetkiAnlikUretim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 26).USER_AUTH;
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
            if (yetkiAnlikUretim != true)
            {
                ViewBag.DisplayAnlıkUretim = "none";
            }
            else
            {
                ViewBag.DisplayAnlıkUretim = "unset";
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
            if (TempData["Başarı"] != null)
            {
                ViewBag.Message = TempData["Başarı"];
            }
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
        public string UretimSonuKaydı(string hatkodu, string stokkodu, string genislik, string mik1, string mik2, bool kontrol, bool etiket)
        {
            var uretimTipi = UretimTipi(hatkodu)[0].URETIM_TIPI;
            string BarkodCikti = null;
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




                        //netRS1.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                        //if (eskimiktar!= jsonList[i].KULL_MIKTAR)
                        //{
                        //    netRS1.Ac("UPDATE TBLISEMRIREC SET MIKTAR=" + (eskimiktar - jsonList[i].KULL_MIKTAR) + " WHERE ISEMRINO='" + jsonList[i].ISEMRINO + "'");
                        //}
                        if (miktarsabitle != "E")
                        {
                            netRS1.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
                            netRS1.Ac("UPDATE TBLSERITRA SET SERI_NO='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND  GCKOD='G' AND SIPNO='" + jsonList[i].ISEMRINO + "'");
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
                }
                finally
                {
                    Marshal.ReleaseComObject(uretim);
                    Marshal.ReleaseComObject(sirket);
                    kernel.FreeNetsisLibrary();
                    Marshal.ReleaseComObject(kernel);
                }

                if (etiket)
                {
                    BarkodCikti = UretimKaydiSonuBarkodCiktisi(jsonList);
                }
            }
            catch (Exception e)
            {
                var exp = e.Message;
                Console.WriteLine(e.Message);
                throw;
            }


            return BarkodCikti;
        }

        #region BarkodPDF
        public string UretimKaydiSonuBarkodCiktisi(List<USKModel> Data)
        {
            string imagepath = Server.MapPath("~\\DesignOutput\\Sevkiyat\\Content");
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A6, 10f, 10f, 10f, 10f);

            MemoryStream Memory = new MemoryStream();
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

            return Convert.ToBase64String(Memory.ToArray());
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