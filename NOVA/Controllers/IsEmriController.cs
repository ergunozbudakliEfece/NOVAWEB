using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
using NOVA.Utils;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;
using QRCoder;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using ServiceStack.Text;
using System.Net.Http;
using Newtonsoft.Json;
using NOVA.Wrappers.Abstract;
using System.Windows.Interop;

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
        SerbestUSK USK = default(SerbestUSK);
        public ActionResult DrawImages()
        {
            System.Drawing.Image playbutton;
            try
            {
                playbutton = System.Drawing.Image.FromFile("C:\\Users\\surec.gelistirme2\\Desktop\\111.png");
            }
            catch (Exception ex)
            {
                return null;
            }

            System.Drawing.Image frame;
            try
            {
                frame = System.Drawing.Image.FromFile("C:\\Users\\surec.gelistirme2\\Desktop\\222.png");
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
        public ActionResult Is()
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


            ViewBag.SIRANO = GetMax();
            ViewBag.Stok_Adlari = GetStokAdlari();
            ViewBag.Cariler = GetCariler();
            ViewBag.Sip = GetSip();
            ViewBag.Seriler = GetSeri();

            return View();
        }
        public async Task<ActionResult> IsEmri()
        {
            int moduleId = 27;

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
            //DrawImages();
            ViewBag.Makine = GetMak(1);
            if (Request.Cookies["Id"] != null)
            {
                ViewBag.Id = Request.Cookies["Id"].Value;
            }

            ViewBag.Stok_Adlari = GetStokAdlari();
            ViewBag.SIRANO = GetMax();
            ViewBag.Sip = GetSip();
            ViewBag.Cariler = GetCariler();
            ViewBag.Seriler = GetSeri();

            return View();
        }

        //27
        public async Task<ActionResult> Index()
        {
            int moduleId = 27;

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

        public string UretimSonuKaydı(string hatkodu, string stokkodu, string genislik, string mik1, string mik2, bool kontrol, bool etiket)
        {
            var uretimTipi = UretimTipi(hatkodu)[0].URETIM_TIPI;
            
            string BarkodCikti = null;
            try
            {


                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                            "EFECE2023",
                                            "TEMELSET",
                                            "",
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);


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
                

                if (etiket)
                {
                    List<BarkodModel> Etiketler = new List<BarkodModel>();

                    WebClient Client = new WebClient() { Encoding = Encoding.UTF8 };

                    foreach (var item in jsonList)
                    {
                        string Response = Client.DownloadString(new Uri("http://192.168.2.13:83/api/seri/kontrol/" + item.SERI_NO));
                        List<BarkodModel> Result = ser.Deserialize<List<BarkodModel>>(Response);

                        Etiketler.Add(Result[0]);
                    }

                    UretimKaydiSonuBarkodCiktisi(Etiketler);
                }
            }
            catch (Exception e)
            {
                var exp = e.Message;
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                sirket.LogOff();
                Marshal.ReleaseComObject(uretim);
                Marshal.ReleaseComObject(sirket);
                kernel.FreeNetsisLibrary();
                Marshal.ReleaseComObject(kernel);
            }


            return BarkodCikti;
        }

        #region BarkodPDF
        public void UretimKaydiSonuBarkodCiktisi(List<BarkodModel> Data)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A6, 10f, 10f, 10f, 10f);

            string imagePath = Path.Combine(Server.MapPath("~\\DesignOutput\\Sevkiyat\\Content"), "SevkiyatDesign.png");
            string pdfPath = Path.Combine(Server.MapPath("~\\DesignOutput\\Uretim\\UretimSonuKaydi"), $"{DateTime.UtcNow.ToUnixTime()}.pdf");

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

                iTextSharp.text.Font fontNormal = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 9, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font fontBoldHeader = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 10, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font fontBoldContent = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 9, iTextSharp.text.Font.BOLD);

                ColumnText Header = new ColumnText(cb);
                Header.SetSimpleColumn(45, 125, 270, 335);
                Header.AddElement(new iTextSharp.text.Paragraph(Data[i].SERI_NO) { Alignment = Element.ALIGN_CENTER, Font = fontBoldHeader });
                Header.Go();

                ColumnText Content = new ColumnText(cb) { Alignment = Element.ALIGN_CENTER };
                Content.SetSimpleColumn(35, 60, 280, 260);

                iTextSharp.text.Paragraph Miktar1 = new iTextSharp.text.Paragraph("MİKTAR        ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Miktar1.Add(new Chunk($": {BosDegerKontrolu(Data[i].MIKTAR1.ToString())}", fontNormal));
                Content.AddElement(Miktar1);

                iTextSharp.text.Paragraph Miktar2 = new iTextSharp.text.Paragraph("MİKTAR 2      ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Miktar2.Add(new Chunk($": {BosDegerKontrolu(Data[i].MIKTAR2.ToString())}", fontNormal));
                Content.AddElement(Miktar2);

                iTextSharp.text.Paragraph Musteri = new iTextSharp.text.Paragraph("MÜŞTERİ       ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Musteri.Add(new Chunk($": {BosDegerKontrolu(Data[i].SIPARIS_CARI)}", fontNormal));
                Content.AddElement(Musteri);

                iTextSharp.text.Paragraph MakineOperator = new iTextSharp.text.Paragraph("MAKİNE/OPR.   ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                MakineOperator.Add(new Chunk($": {Data[i].MAK_KODU}/{BosDegerKontrolu(Data[i].KAYITYAPANKUL)}", fontNormal));
                Content.AddElement(MakineOperator);

                iTextSharp.text.Paragraph Boy = new iTextSharp.text.Paragraph("BOY           ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Boy.Add(new Chunk($": {BosDegerKontrolu(Data[i].BOY.ToString())}", fontNormal));
                Content.AddElement(Boy);

                iTextSharp.text.Paragraph Tarih = new iTextSharp.text.Paragraph("TARİH         ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Tarih.Add(new Chunk($": {BosDegerKontrolu(Data[i].KAYITTARIHI)}", fontNormal));
                Content.AddElement(Tarih);
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
        [HttpPost]
        public void TrpzUretimKaydiSonuBarkodCiktisi(BarkodModel Model)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A6, 10f, 10f, 10f, 10f);

            string imagePath = Path.Combine(Server.MapPath("~\\DesignOutput\\Sevkiyat\\Content"), "SevkiyatDesign.png");
            string pdfPath = Path.Combine(Server.MapPath("~\\DesignOutput\\Uretim\\UretimSonuKaydi"), $"{DateTime.UtcNow.ToUnixTime()}.pdf");

            FileStream Memory = new FileStream(pdfPath, FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, Memory);

            document.Open();

            iTextSharp.text.Image BackgroundImage = iTextSharp.text.Image.GetInstance(imagePath);
            BackgroundImage.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
            BackgroundImage.Alignment = iTextSharp.text.Image.UNDERLYING;
            BackgroundImage.SetAbsolutePosition(0, 0);
            document.Add(BackgroundImage);

            PdfContentByte cb = writer.DirectContent;

            iTextSharp.text.Font fontNormal = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 9, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font fontBoldHeader = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 10, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fontBoldContent = FontFactory.GetFont(BaseFont.COURIER, "CP1254", 9, iTextSharp.text.Font.BOLD);

            ColumnText Header = new ColumnText(cb);
            Header.SetSimpleColumn(45, 125, 270, 335);
            Header.AddElement(new iTextSharp.text.Paragraph(Model.SERI_NO) { Alignment = Element.ALIGN_CENTER, Font = fontBoldHeader });
            Header.Go();

            ColumnText Content = new ColumnText(cb) { Alignment = Element.ALIGN_CENTER };
            Content.SetSimpleColumn(35, 60, 280, 260);

            iTextSharp.text.Paragraph Miktar1 = new iTextSharp.text.Paragraph("MİKTAR        ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
            Miktar1.Add(new Chunk($": {BosDegerKontrolu(Model.MIKTAR1.ToString())}", fontNormal));
            Content.AddElement(Miktar1);

            iTextSharp.text.Paragraph Miktar2 = new iTextSharp.text.Paragraph("MİKTAR 2      ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
            Miktar2.Add(new Chunk($": {BosDegerKontrolu(Model.MIKTAR2.ToString())}", fontNormal));
            Content.AddElement(Miktar2);

            iTextSharp.text.Paragraph Musteri = new iTextSharp.text.Paragraph("MÜŞTERİ       ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
            Musteri.Add(new Chunk($": {BosDegerKontrolu(Model.SIPARIS_CARI)}", fontNormal));
            Content.AddElement(Musteri);

            iTextSharp.text.Paragraph MakineOperator = new iTextSharp.text.Paragraph("MAKİNE/OPR.   ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
            MakineOperator.Add(new Chunk($": {Model.MAK_KODU}/{BosDegerKontrolu(Model.KAYITYAPANKUL)}", fontNormal));
            Content.AddElement(MakineOperator);

            iTextSharp.text.Paragraph Boy = new iTextSharp.text.Paragraph("BOY           ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
            Boy.Add(new Chunk($": {BosDegerKontrolu(Model.BOY.ToString())}", fontNormal));
            Content.AddElement(Boy);

            iTextSharp.text.Paragraph Tarih = new iTextSharp.text.Paragraph("TARİH         ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
            Tarih.Add(new Chunk($": {BosDegerKontrolu(Model.KAYITTARIHI)}", fontNormal));
            Content.AddElement(Tarih);
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

        public Microsoft.AspNetCore.Mvc.StatusCodeResult TrpzUretim(string stokkodu, string ISEMRINO, string SERI_NO, string KULL_MIKTAR, string mik2)
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
                    NetRS netRS = kernel.yeniNetRS(sirket);
                    netRS.Ac("SELECT * FROM TBLISEMRI WHERE ISEMRINO='" + ISEMRINO + "'");
                    var seri = netRS.FieldByName("SERINO").AsString;
                    if (seri == null)
                    {
                        uretim.OTOSERIURET();
                        uretim.SeriEkle(uretim.SeriOku(0).Seri1, "", "", "", KULL_MIKTAR.ToDouble(), mik2.ToDouble());
                    }
                    
                    netRS.Calistir("UPDATE TBLISEMRIREC SET SERINO='" + SERI_NO + "' WHERE ISEMRINO='" + ISEMRINO + "'");


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


                    netRS.Ac("UPDATE TBLSERITRA SET KARSISERI='" + karsi + "' WHERE BELGENO='" + uretim.UretSon_FisNo + "' AND SIPNO='" + ISEMRINO + "'");


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
                                            "EFECE2023",
                                            "TEMELSET",
                                            "",
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);
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
                                            "EFECE2023",
                                            "TEMELSET",
                                            "",
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);


                for (var i = 0; i < jsonList.Count; i++)
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
        public List<TRPZModel> TRPZUret(string stokkodu, int miktar)
        {

            var apiUrl2 = "http://192.168.2.13:83/api/ie/TRPZ/" + stokkodu + "/" + miktar;
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
        public JsonResult PostTRPZ(List<IsEmriModel2> isemri)
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
                        var stokkodu = stokadlari.Where(x => x.STOK_ADI == isemri[i].REF_STOKOLCUSU);


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
                        Isemri.SipKont = isemri[i].SIPKONT;
                        Isemri.SiparisNo = isemri[i].SIPARISNO;
                        Isemri.Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                        Isemri.TeslimTarihi = Convert.ToDateTime(DateTime.Now.Year + "-12-31");

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
                        
                        if (isemri[i].GIRDI2 != "-")
                        {
                            netRS.Ac("UPDATE TBLISEMRIREC SET SERINO='" + isemri[i].GIRDI2 + "',DEPO_KODU='45',MIKTAR=1,MIKTARSABITLE='H' WHERE ISEMRINO='" + isemri[i].REF_ISEMRINO + "'");
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
                    Isemri1.SipKont = isemri[i].SIPKONT;
                    Isemri1.SiparisNo = isemri[i].SIPARISNO;
                    Isemri1.Oncelik = 0;
                    if (isemri[i].REF_ISEMRINO != "-")
                    {
                        Isemri1.RefIsEmriNo = isemri[i].REF_ISEMRINO;
                    }
                    Isemri1.DepoKodu = 45;
                    Isemri1.CikisDepoKodu = 45;
                    if (isemri[i].GIRDI2 != "-")
                    {
                        Isemri1.SeriNo = isemri[i].GIRDI2;
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
                    
                    Isemri1.TeslimTarihi = Convert.ToDateTime(DateTime.Now.Year+"-12-31");
                    Isemri1.Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                    Isemri1.kayitYeni();
                    NetRS netRS2 = kernel.yeniNetRS(sirket);
                    if (isemri[i].GIRDI1 != "-" && isemri[i].GIRDI1!="" && isemri[i].GIRDI1!=null)
                    {
                        netRS2.Ac("UPDATE TBLISEMRIREC SET SERINO='" + isemri[i].GIRDI1 + "',DEPO_KODU='45' WHERE ISEMRINO='" + isemri[i].ISEMRINO + "'");
                    }
                    else
                    {
                        netRS2.Ac("UPDATE TBLISEMRIREC SET DEPO_KODU='45' WHERE ISEMRINO='" + isemri[i].ISEMRINO + "'");
                    }
                   

                }
            }
            catch (Exception ex)
            {

                TempData["Hata"] = "HATA";
                var msg = ex.Message;
                return Json(new Wrappers.Concrete.ErrorResponse(msg), JsonRequestBehavior.AllowGet);
            }
            finally
            {
                sirket.LogOff();
            }

            return Json(new Wrappers.Concrete.SuccessResponse<string>("Başarılı!"), JsonRequestBehavior.AllowGet);
        }
        public class IsEmriModel2
        {
            public string GIRDI1 { get; set; }
            public string GIRDI2 { get; set; }
            public string ISEMRINO { get; set; }
            public string STOKADI { get; set; }
            public string AGIRLIK { get; set; }
            public string ADET { get; set; }
            public string REF_ADET { get; set; }
            public string REF_ISEMRINO { get; set; }
            public string REF_STOKOLCUSU { get; set; }
            public string SIPARIS_NO { get; set; }
            public int SIPKONT { get; set; }
            public string MUSTERI { get; set; }
            public string SIPARISNO { get; set; }

        }
        [HttpPost]
        public Wrappers.Abstract.IResponse Post(List<IsEmriModel> isemri)
        {

            var isemridis = isemri.GroupBy(x => x.ISEMRINO).Select(x => x.First()).ToList();
            List<SeriModel> createdlog1 = null;
            var apiUrl2 = "http://192.168.2.13:83/api/seri/1";
            Uri url2 = new Uri(apiUrl2);
            WebClient client2 = new WebClient();
            client2.Encoding = System.Text.Encoding.UTF8;
            var cariler = GetCariler();
            var json2 = client2.DownloadString(url2);
            JavaScriptSerializer ser2 = new JavaScriptSerializer();
            createdlog1 = ser2.Deserialize<List<SeriModel>>(json2);
            var sipkont = 1;
            try
            {
                var stokadlari = GetStokAdlari();
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                            "EFECE2023",
                                            "TEMELSET",
                                            "",
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);

                for (int i = 0; i < isemridis.Count(); i++)
                {

                    if (isemridis[i].REF_ISEMRINO != null && isemridis[i].REF_ISEMRINO != "-")
                    {
                        var stokkodu = stokadlari.Where(x => x.STOK_ADI == isemridis[i].REF_STOKOLCUSU);
                        if (isemridis[i].SIPARISNO != null && isemridis[i].SIPARISNO != "")
                        {
                            sipkont = cariler.FirstOrDefault(x => x.STOK_KODU == stokkodu.First().STOK_KODU && x.FISNO == isemridis[i].SIPARISNO).SIPKONT;
                        }



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
                        Isemri.SeriNo2 = isemridis[i].GENISLIK.ReplaceAll(".", ",");
                        Isemri.Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                        Isemri.TeslimTarihi = Convert.ToDateTime(DateTime.Now.Year + "-12-31");
                        Isemri.SipKont = Isemri.SipKont = (sipkont == 1 ? isemridis[i].SIPKONT : sipkont);
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

                        netRS.Ac("UPDATE TBLISEMRIREC SET SERINO='" + isemridis[i].GIRDI2 + "',DEPO_KODU='45',MIKTAR='1',MIKTARSABITLE='H' WHERE ISEMRINO='" + isemridis[i].REF_ISEMRINO + "'");



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
                    if (isemridis[i].REF_ISEMRINO != "-")
                    {

                        Isemri1.RefIsEmriNo = isemridis[i].REF_ISEMRINO;
                    }
                    Isemri1.DepoKodu = 45;
                    Isemri1.CikisDepoKodu = 45;
                    if (isemridis[i].GIRDI2 != "-")
                    {
                        Isemri1.SeriNo = isemridis[i].GIRDI2.ReplaceAll(".", ",");
                    }
                    else
                    {
                        Isemri1.SeriNo = "0";
                    }

                    if (isemridis[i].ISEMRINO.Substring(0, 2) == "DL")
                    {
                        Isemri1.SeriNo2 = isemridis[i].GENISLIK.ReplaceAll(".", ",");
                    }
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
                    Isemri1.TeslimTarihi = Convert.ToDateTime(DateTime.Now.Year + "-12-31");
                    Isemri1.Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                    Isemri1.kayitYeni();
                    NetRS netRS2 = kernel.yeniNetRS(sirket);


                    if (isemridis[i].MIKTAR_SABITLE == "E")
                    {
                        netRS2.Ac("UPDATE TBLISEMRIREC SET SERINO='" + isemridis[i].GIRDI1 + "',MIKTAR='" + isemridis[i].HAMSARF + "',MIKTARSABITLE='E', DEPO_KODU='45' WHERE ISEMRINO='" + isemridis[i].ISEMRINO + "'");

                    }
                    else
                    {
                        netRS2.Ac("UPDATE TBLISEMRIREC SET SERINO='" + isemridis[i].GIRDI1 + "', DEPO_KODU='45',MIKTAR=1,MIKTARSABITLE='H' WHERE ISEMRINO='" + isemridis[i].ISEMRINO + "'");
                    }


                }
            }
            catch (Exception ex)
            {

                TempData["Hata"] = ex.Message;
                return new Wrappers.Concrete.ErrorResponse(ex.Message);

            }
            finally
            {
                sirket.LogOff();
            }





            return new Wrappers.Concrete.SuccessResponse<string>("Başarılı!");
        }
        [HttpPost]
        public IResponse PostBKPBRF(List<IsEmriModel> isemri, bool TamamiKullanilsin)
        {

            var isemridis = isemri.GroupBy(x => x.ISEMRINO).Select(x => x.First()).ToList();
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

                for (int i = 0; i < isemridis.Count(); i++)
                {
                    var stokkodu1 = stokadlari.Where(x => x.STOK_ADI == isemridis[i].STOKADI);

                    Isemri1 = kernel.yeniIsEmri(sirket);
                    Isemri1.IsEmriNo = isemridis[i].ISEMRINO;
                    Isemri1.StokKodu = stokkodu1.First().STOK_KODU;
                    Isemri1.Aciklama = isemridis[i].ADET2;
                    Isemri1.Kapali = false;
                    Isemri1.ReceteSaklansin = true;
                    Isemri1.ProjeKodu = "1";
                    Isemri1.Oncelik = 0;
                    Isemri1.DepoKodu = 45;
                    Isemri1.CikisDepoKodu = 45;
                    if (isemridis[i].GENISLIK != null)
                    {
                        Isemri1.SeriNo2 = isemridis[i].GENISLIK.ReplaceAll(".", ",");
                    }
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
                    Isemri1.TeslimTarihi = Convert.ToDateTime(DateTime.Now.Year + "-12-31");
                    Isemri1.Tarih = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
                    Isemri1.kayitYeni();
                    NetRS netRS2 = kernel.yeniNetRS(sirket);
                    if (isemridis[i].SIPARISNO != null)
                    {
                        netRS2.Ac("UPDATE TBLISEMRIEK SET KT_SIPNO='" + isemridis[i].SIPARISNO + "' WHERE ISEMRINO='" + isemridis[i].ISEMRINO + "'");
                    }
                    if (TamamiKullanilsin == true && isemridis[i].ISEMRINO.Substring(0, 2) != "RF" && isemridis[i].ISEMRINO.Substring(0, 2) != "PB")
                    {
                        
                        netRS2.Ac("UPDATE TBLISEMRIREC SET SERINO='" + isemridis[i].GIRDI1 + "',DEPO_KODU='45',MIKTAR=" + mik +",MIKTARSABITLE='E' WHERE ISEMRINO='" + isemridis[i].ISEMRINO + "'");

                    }
                    else
                    {
                        netRS2.Ac("UPDATE TBLISEMRIREC SET SERINO='" + isemridis[i].GIRDI1 + "', DEPO_KODU='45',MIKTAR=1,MIKTARSABITLE='H' WHERE ISEMRINO='" + isemridis[i].ISEMRINO + "'");
                    }
                }
            }
            catch (Exception ex)
            {

                TempData["Hata"] = ex.Message;
                return new Wrappers.Concrete.ErrorResponse(ex.Message);
            }
            finally
            {
                sirket.LogOff();
            }



            return new Wrappers.Concrete.SuccessResponse<string>("Başarılı!");
        }

        [HttpPost]
        public ActionResult Post1()
        {

            try
            {


                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                            "EFECE2023",
                                            "TEMELSET",
                                            "",
                                            Request.Cookies["UserName"].Value,
                                            LoginController.Decrypt(Request.Cookies["UserPassword"].Value), 0);
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
        public ActionResult Mail(List<MailModel2> mail)
        {

            string subject = "";
            List<string> makineler = new List<string>();
            List<string> makinelerref = new List<string>();




            var subject2 = mail[0].MAKINE + " İŞ EMRİ - " + Request.Cookies["UserName"].Value;
            string body = "<tr style='outline: thin solid;margin-bottom:15px'><th style='margin-right:10px'>STOK ADI</th><th style='margin-right:10px'>MAKİNE</th><th style='margin-right:10px'>TOPLAM</th></tr>";
            for (int i = 0; i < mail.Count; i++)
            {
                body = body + "<tr style='outline: thin solid;margin-bottom:10px'>" + "<td style='border-collapse: collapse;margin-right:10px' >" + mail[i].STOK_ADI + "</td>" + "<td style='border-collapse: collapse;margin-right:10px'' >" + mail[i].MAKINE + "</td>" + "<td style='border-collapse: collapse;text-align: center;margin-right:10px''>" + mail[i].TOPLAM + "</td></tr>";
            }



            WebMail.SmtpServer = "192.168.2.13";
            WebMail.Send(Request.Cookies["Mail"].Value, subject2, "<p>NOVA üzerinde <strong>" + Request.Cookies["UserName"].Value + "</strong> kullanıcısı tarafından oluşturulan iş emirleri aşağıdaki gibidir:</p> </br>" + "<table style='border: 1px solid black;border-collapse: collapse'>" + body + "</table>", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);

            return RedirectToAction("Index");
        }
        //public ActionResult Mail(List<MailModel> mail,string hamkod)
        //{

        //    string subject = "";
        //    List<string> makineler = new List<string>();
        //    List<string> makinelerref = new List<string>();




        //    var subject2 = hamkod + " İŞ EMRİ - " + Request.Cookies["UserName"].Value;
        //    string body = "<tr style='outline: thin solid;margin-bottom:15px'><th style='margin-right:10px'>MUSTERI</th><th style='margin-right:10px'>STOKOLCULERI</th><th style='margin-right:10px'>KALINLIK</th><th style='margin-right:10px'>KALITE</th><th style='margin-right:10px'>KAPLAMA</th><th style='margin-right:10px'>ADET</th><th style='margin-right:10px'>AĞIRLIK</th></tr>";
        //    for (int i = 0; i < mail.Count; i++)
        //    {
        //        body = body + "<tr style='outline: thin solid;margin-bottom:10px'>" + "<td style='border-collapse: collapse;margin-right:10px' >" + mail[i].MUSTERI + "</td>" + "<td style='border-collapse: collapse;margin-right:10px'' >" + mail[i].STOKADI + "</td>" + "<td style='border-collapse: collapse;text-align: center;margin-right:10px''>" + mail[i].KALINLIK + "</td>" + "<td style='border-collapse: collapse;text-align: center;margin-right:10px''>" + mail[i].KALITE + "</td>" + "<td style='border-collapse: collapse' style='text-align: center;margin-right:10px'' >" + mail[i].KAPLAMA + "</td>" + "<td style='border-collapse: collapse;text-align: center;margin-right:10px''>" + String.Format("{0:n0}", mail[i].ADET) + "</td>" + "<td style='border-collapse: collapse;text-align: center;margin-right:10px'' >" + String.Format("{0:n0}", mail[i].AGIRLIK) + "</td>" + "</tr>";
        //    }



        //    WebMail.SmtpServer = "192.168.2.13";
        //    WebMail.Send(Request.Cookies["Mail"].Value, subject2, "<p>NOVA üzerinde <strong>" + Request.Cookies["UserName"].Value + "</strong> kullanıcısı tarafından oluşturulan iş emirleri aşağıdaki gibidir:</p> </br>" + "<table style='border: 1px solid black;border-collapse: collapse'>" + body + "</table>", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);
        //    /* WebMail.Send("ugurkonakci@efecegalvaniz.com", subject2, "<p>NOVA üzerinde <strong>" + Request.Cookies["UserName"].Value + "</strong> kullanıcısı tarafından oluşturulan iş emirleri aşağıdaki gibidir:</p> </br>" + "<table style='border: 1px solid black;border-collapse: collapse'>" + body + "</table>", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);*/
        //    WebMail.Send("ergunozbudakli@efecegalvaniz.com", subject2 + TempData["Hata"], "<p>NOVA üzerinde <strong>" + Request.Cookies["UserName"].Value + "</strong> kullanıcısı tarafından oluşturulan iş emirleri aşağıdaki gibidir:</p> </br>" + "<table style='border: 1px solid black;border-collapse: collapse'>" + body + "</table>", "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);
        //    return RedirectToAction("Index");
        //}
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

            public string ISEMRINO { get; set; }
            public string REF_ISEMRINO { get; set; }
            public string MUSTERI { get; set; }
            public string STOKADI { get; set; }
            public string KALINLIK { get; set; }
            public string KAPLAMA { get; set; }
            public string KALITE { get; set; }
            public string ADET { get; set; }
            public string AGIRLIK { get; set; }
        }
        public class MailModel2
        {

            public string STOK_ADI { get; set; }
            public string MAKINE { get; set; }
            public double TOPLAM { get; set; }

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
            var apiUrl = "http://192.168.2.13:83/api/MSIP/TEST";

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
        //                                          "EFECE2023",
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
        public class BarkodModel
        {
            public string SERI_NO { get; set; }
            public double MIKTAR1 { get; set; }
            public double MIKTAR2 { get; set; }
            public string STOK_KODU { get; set; }
            public string GRUP_ISIM { get; set; }
            public double BOY { get; set; }
            public string KAYITYAPANKUL { get; set; }
            public string KAYITTARIHI { get; set; }
            public string SIPARIS_CARI { get; set; }
            public string MAK_KODU { get; set; }
        }
        public class IsEmriModel    
        {
            public string MIKTAR_SABITLE { get;set; }
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
            public string ADET2 { get; set; }
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
            public string FIS_NO { get; set; }
            public string SERI_NO { get; set; }
            public string ISEMRINO { get; set; }
            public double KULL_MIKTAR { get; set; }
            public double MIKTAR2 { get; set; }
            public double FIRE { get; set; }
        }
    }
}