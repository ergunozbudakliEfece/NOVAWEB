using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetOpenX50;
using NOVA.Models;
using System.Net;
using System.Web.Script.Serialization;
using SqlApi.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web.Security;
using System.Web.Helpers;


namespace NOVA.Controllers
{

    public class IsEmriController : Controller
    {


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
                    catch (Exception ex) {
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
            ViewBag.Makine = GetMak();
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
            ViewBag.Makine = GetMak();
            ViewBag.Id = Request.Cookies["Id"].Value;
            ViewBag.UretimTakip = Get();
            ViewBag.SIRANO = GetMax();
            ViewBag.Stok_Adlari = GetStokAdlari();
            ViewBag.Cariler = GetCariler();

            return View();
        }



        [HttpPost]
        public ActionResult Post(List<IsEmriModel> isemri)
        {
            
            



                var stokadlari = GetStokAdlari();

              try
              {
                var isemridis = isemri.GroupBy(x => x.GIRDI2).Select(y => y.First()).ToList();
                Kernel kernel = new Kernel();

                  Sirket sirket = default(Sirket);

                  sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                               "TEST2022",
                                               "TEMELSET",
                                               "",
                                               "erguno",
                                               "begum142088", 0);

                  for (int i = 0; i < isemridis.Count(); i++)
                  {

                      if (isemridis[i].REF_ISEMRINO != null)
                      {
                          var stokkodu = stokadlari.Where(x => x.STOK_ADI == isemridis[i].REF_STOKOLCUSU);
                       
                          IsEmri Isemri = default(IsEmri);
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
                          Isemri.SeriNo = isemridis[i].GIRDI2;
                          Isemri.SeriNo2 = isemridis[i].GENISLIK;
                          Isemri.TeslimTarihi = Convert.ToDateTime("2022-12-31");
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

                          netRS.Ac("UPDATE TBLISEMRIREC SET SERINO='" + isemridis[i].GIRDI1 + "',DEPO_KODU='45',MIKTAR="+ m2 + ",MIKTARSABITLE='E' WHERE ISEMRINO='" + isemridis[i].REF_ISEMRINO + "'");
                      }
                      
                        

                      var stokkodu1 = stokadlari.Where(x => x.STOK_ADI == isemridis[i].STOKADI);
                      IsEmri Isemri1 = default(IsEmri);
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
                      Isemri1.SeriNo = isemridis[i].GIRDI2;
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
                      Isemri1.TeslimTarihi = Convert.ToDateTime("2022-12-31");
                      Isemri1.kayitYeni();
                      NetRS netRS2 = kernel.yeniNetRS(sirket);

                      netRS2.Ac("UPDATE TBLISEMRIREC SET SERINO='" + isemridis[i].GIRDI1 + "',MIKTAR=" + isemridis[i].HAMSARF + ",MIKTARSABITLE='E', DEPO_KODU='45' WHERE ISEMRINO='" + isemridis[i].ISEMRINO + "'");

                  }
              }
              catch (Exception ex)
              {

                  TempData["Hata"] ="HATA";
                  return View("Index");
              }
             

                 



            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Mail(List<MailModel> mail)
        {
            var m = mail.GroupBy(x => new { x.MUSTERI,x.STOKOLCULERI,x.KALINLIK, x.KALITE, x.KAPLAMA}).Select(grp=>new {MUSTERI=grp.First().MUSTERI,STOKOLCULERI=grp.First().STOKOLCULERI,KALINLIK=grp.First().KALINLIK,KALITE=grp.First().KALITE,KAPLAMA=grp.First().KAPLAMA,ADET=grp.Sum(s=>int.Parse(s.ADET)),AGIRLIK= grp.Sum(s => int.Parse(s.AGIRLIK))}).ToList();
            string subject = "";
            List<string> makineler =new List<string>();
            List<string> makinelerref = new List<string>();
            for (int i= 0; i < mail.Count; i++)
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
            for(int i = 0; i < subject.Split('-').Length ; i++)
            {
                if (subject.Split('-')[i] != "")
                {
                    upt = upt + subject.Split('-')[i]+"-";
                }
                
            }
            

            var subject2=upt.Substring(0, upt.Length - 1)+" İŞ EMRİ - "+Request.Cookies["UserName"].Value;
            string body = "<tr style='outline: thin solid;margin-bottom:15px'><th style='margin-right:10px'>MUSTERI</th><th style='margin-right:10px'>STOKOLCULERI</th><th style='margin-right:10px'>KALINLIK</th><th style='margin-right:10px'>KALITE</th><th style='margin-right:10px'>KAPLAMA</th><th style='margin-right:10px'>ADET</th><th style='margin-right:10px'>AĞIRLIK</th></tr>";
            for (int i = 0; i < m.Count; i++)
            {
                body = body + "<tr style='outline: thin solid;margin-bottom:10px'>" + "<td style='border-collapse: collapse;margin-right:10px' >" + m[i].MUSTERI +"</td>" + "<td style='border-collapse: collapse;margin-right:10px'' >" + m[i].STOKOLCULERI + "</td>" + "<td style='border-collapse: collapse;text-align: center;margin-right:10px''>" + m[i].KALINLIK  +"</td>"+ "<td style='border-collapse: collapse;text-align: center;margin-right:10px''>" + m[i].KALITE + "</td>"+ "<td style='border-collapse: collapse' style='text-align: center;margin-right:10px'' >" + m[i].KAPLAMA + "</td>"+ "<td style='border-collapse: collapse;text-align: center;margin-right:10px''>" + String.Format("{0:n0}", m[i].ADET) + "</td>"+ "<td style='border-collapse: collapse;text-align: center;margin-right:10px'' >" + String.Format("{0:n0}", m[i].AGIRLIK) + "</td>" + "</tr>";
            }
           


            WebMail.SmtpServer = "192.168.2.13";
            WebMail.Send(Request.Cookies["Mail"].Value, subject2, "<p>NOVA üzerinde <strong>"+Request.Cookies["UserName"].Value+"</strong> kullanıcısı tarafından oluşturulan iş emirleri aşağıdaki gibidir:</p> </br>"+"<table style='border: 1px solid black;border-collapse: collapse'>" + body +"</table>", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);
           /* WebMail.Send("ugurkonakci@efecegalvaniz.com", subject2, "<p>NOVA üzerinde <strong>" + Request.Cookies["UserName"].Value + "</strong> kullanıcısı tarafından oluşturulan iş emirleri aşağıdaki gibidir:</p> </br>" + "<table style='border: 1px solid black;border-collapse: collapse'>" + body + "</table>", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);*/
            WebMail.Send("ergunozbudakli@efecegalvaniz.com", subject2+TempData["Hata"], "<p>NOVA üzerinde <strong>" + Request.Cookies["UserName"].Value + "</strong> kullanıcısı tarafından oluşturulan iş emirleri aşağıdaki gibidir:</p> </br>" + "<table style='border: 1px solid black;border-collapse: collapse'>" + body + "</table>", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);
            return RedirectToAction("Index");
        }
        public class MailModel{
           
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
      
        public void IsEmriSend(List<IsEmriModel1> IsEmriModel)
        {
            Kernel kernel = new Kernel();
            Sirket sirket = default(Sirket);
            sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                          "TEST2022",
                                          "TEMELSET",
                                          "",
                                          "erguno",
                                          "begum142088", 0);
            
            IsEmri Isemri = default(IsEmri);
            Isemri = kernel.yeniIsEmri(sirket);
            Isemri.kayitOku(TOkumaTipi.otSon);
            
            Isemri.IsEmriNo = IsEmriModel[0].ISEMRINO;
            //Isemri.Tarih = Convert.ToDateTime(isemri.TARIH);
            Isemri.ReceteSaklansin = true;
            Isemri.RezervasyonDurumu = TRezervasyonDurumu.rdYeni;
            
            Isemri.StokKodu = IsEmriModel[0].STOK_KODU;
            Isemri.Kapali = false;
            Isemri.Miktar = IsEmriModel[0].MIKTAR;
            Isemri.Aciklama = IsEmriModel[0].ADET;
            Isemri.SeriNo2 = IsEmriModel[0].GENISLIK;
            Isemri.DepoKodu =0;
            Isemri.CikisDepoKodu = IsEmriModel[0].DEPOKODU;
            Isemri.ProjeKodu = "1";
            
            Isemri.kayitYeni();

            var e = IsEmriModel;
          
        }
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
    }
}