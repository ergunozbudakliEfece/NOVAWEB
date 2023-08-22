using NetOpenX50;
using NOVA.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            var yetki = GetYetki();
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