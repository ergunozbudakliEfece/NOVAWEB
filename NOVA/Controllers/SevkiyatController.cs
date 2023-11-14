using iTextSharp.text;
using iTextSharp.text.pdf;
using NOVA.Models;
using NOVA.Utils;
using QRCoder;
using ServiceStack;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace NOVA.Controllers
{
    public class SevkiyatController : Controller
    {
        #region Müsteri Siparis Raporu

        public async Task<ActionResult> SiparisRaporu()
        {
            int moduleId = 18;

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

            if (UserData.UPDATE_AUTH == true)
            {
                ViewBag.Update = "Yetkili";
            }

            await RoleHelper.CheckRoles(this);

            ViewBag.Id= Request.Cookies["Id"].Value.ToInt();

            return View();
        }

        #endregion

        #region Satici Siparis Raporu
        public async Task<ActionResult> SaticiSiparisRaporu()
        {
            int moduleId = 20;

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

        #region Depolar Arası Transfer
        public async Task<ActionResult> DepoTransfer()
        {
            int moduleId = 44;

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

        #region Sevk Ve Kabul İşlemleri

        public async Task<ActionResult> SevkMalKabul()
        {
            int moduleId = 42;

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

        #region BarkodPDF
        [HttpPost]
        public string SevkiyatBarkodCiktisi(SevkiyatBarkodFormModel Model, bool Yazdir, string Yazici)
        {
            Document document = new Document(PageSize.A6, 10f, 10f, 10f, 10f);

            string imagePath = System.IO.Path.Combine(Server.MapPath("~\\DesignOutput\\Sevkiyat\\Content"), "SevkiyatDesign.png");
            string pdfPath = System.IO.Path.Combine(Server.MapPath("~\\DesignOutput\\Sevkiyat\\Etiketler"), $"{DateTime.UtcNow.ToUnixTime()}.pdf");

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
            Header.AddElement(new Paragraph(Model.STOK_KODU) { Alignment = Element.ALIGN_CENTER, Font = fontBoldHeader });
            Header.AddElement(new Paragraph(Model.STOK_ADI) { Alignment = Element.ALIGN_CENTER, Font = fontBoldHeader, SpacingBefore = 10f, MultipliedLeading = 1f });
            Header.AddElement(new Paragraph(Model.BARKOD_NO) { Alignment = Element.ALIGN_CENTER, Font = fontBoldHeader });
            Header.Go();

            ColumnText Content = new ColumnText(cb) { Alignment = Element.ALIGN_CENTER };
            Content.SetSimpleColumn(35, 60, 280, 260);

            Paragraph Miktar1 = new Paragraph("MİKTAR 1      ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
            Miktar1.Add(new Chunk($": {BosDegerKontrolu(Model.MIKTAR)} {BosDegerKontrolu(Model.OLCU_BR1)}", fontNormal));
            Content.AddElement(Miktar1);


            if (Model.OLCU_BR1 != Model.OLCU_BR2)
            {
                Paragraph Miktar2 = new Paragraph("MİKTAR 2      ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Miktar2.Add(new Chunk($": {BosDegerKontrolu(Model.MIKTAR2)} {BosDegerKontrolu(Model.OLCU_BR2)}", fontNormal));
                Content.AddElement(Miktar2);

                Paragraph BirimMiktar = new Paragraph("BİRİM MİKTAR  ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                BirimMiktar.Add(new Chunk($": {BosDegerKontrolu(Model.BIRIM_MIKTAR)} {BosDegerKontrolu(Model.OLCU_BR1)}/{BosDegerKontrolu(Model.OLCU_BR2)}", fontNormal));
                Content.AddElement(BirimMiktar);
            }
            else
            {
                Paragraph Miktar2 = new Paragraph("MİKTAR 2      ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                Miktar2.Add(new Chunk($": -", fontNormal));
                Content.AddElement(Miktar2);

                Paragraph BirimMiktar = new Paragraph("BİRİM MİKTAR  ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
                BirimMiktar.Add(new Chunk($": -", fontNormal));
                Content.AddElement(BirimMiktar);
            }

            Paragraph Kalinlik = new Paragraph("KALINLIK      ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
            Kalinlik.Add(new Chunk($": {BosDegerKontrolu(Model.KALINLIK)}", fontNormal));
            Content.AddElement(Kalinlik);

            Paragraph Genislik = new Paragraph("GENİŞLİK      ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
            Genislik.Add(new Chunk($": {BosDegerKontrolu(Model.GENISLIK)}", fontNormal));
            Content.AddElement(Genislik);

            Paragraph Kalite = new Paragraph("KALİTE        ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
            Kalite.Add(new Chunk($": {BosDegerKontrolu(Model.KALITE)}", fontNormal));
            Content.AddElement(Kalite);

            Paragraph Kaplama = new Paragraph("KAPLAMA       ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
            Kaplama.Add(new Chunk($": {BosDegerKontrolu(Model.KAPLAMA)}", fontNormal));
            Content.AddElement(Kaplama);

            Paragraph Tarih = new Paragraph("TARİH         ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
            Tarih.Add(new Chunk($": {BosDegerKontrolu(Model.TARIH)}", fontNormal));
            Content.AddElement(Tarih);

            Paragraph Mensei = new Paragraph("MENŞEİ        ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
            Mensei.Add(new Chunk($": {BosDegerKontrolu(Model.MENSEI)}", fontNormal));
            Mensei.SpacingBefore = 5f;
            Mensei.MultipliedLeading = 1f;
            Content.AddElement(Mensei);

            Paragraph FirmaSeriNo = new Paragraph("FİRMA SERİ NO ", fontBoldContent) { Alignment = Element.ALIGN_LEFT };
            FirmaSeriNo.Add(new Chunk($": {BosDegerKontrolu(Model.FIRMA_SERI_NO)}", fontNormal));
            Content.AddElement(FirmaSeriNo);
            Content.Go();

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(Model.BARKOD_NO, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            System.Drawing.Image qrCodeImage = qrCode.GetGraphic(45, Color.Black, Color.Transparent, true);

            iTextSharp.text.Image QR = iTextSharp.text.Image.GetInstance(ImageToByteArray(qrCodeImage));
            QR.ScaleToFit(75, 75);
            QR.Alignment = iTextSharp.text.Image.UNDERLYING;
            QR.SetAbsolutePosition(195, 12);
            document.Add(QR);

            document.Close();
            Memory.Close();

            if (Yazdir)
            {
                using (var pdocument = PdfiumViewer.PdfDocument.Load(pdfPath))
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
            }

            return ToBase64String(pdfPath);
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
        public string ToBase64String(string fileName)
        {
            using (FileStream reader = new FileStream(fileName, FileMode.Open))
            {
                byte[] buffer = new byte[reader.Length];
                reader.Read(buffer, 0, (int)reader.Length);
                return Convert.ToBase64String(buffer);
            }
        }
        #endregion

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

            return jsonList.Where(x => x.INCKEY == id).ToList();
        }
        public List<SignIn> GetSession(int id)
        {
            var apiUrl = "http://192.168.2.13:83/api/log/max:" + id;

            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);

            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SignIn> jsonList = ser.Deserialize<List<SignIn>>(json);

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
        public JsonResult GetSip()
        {


            var apiUrl = "http://192.168.2.13:83/api/detaylisip";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
         

            return Json(json, JsonRequestBehavior.AllowGet);
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
            ser.MaxJsonLength = int.MaxValue;
            List<USTKALEMMODEL> jsonList = ser.Deserialize<List<USTKALEMMODEL>>(json);

            //END

            return jsonList.OrderBy(t=>t.CARI_ISIM).ToList();
        }
        public ActionResult Filter(FilterModel filter)
        {

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
                                    x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == item && t.CARI_DOVIZ_TIPI == item2 && t.PLASIYER_ADI == item3 && t.URETILECEKMI == "E")).ToList();
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

                                x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == item && t.CARI_DOVIZ_TIPI == item2 && t.URETILECEKMI == "E")).ToList();
                                z.Add(item);
                                m.Add(item2);


                            }
                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in teslimArray)
                        {


                            x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == item && t.URETILECEKMI == "E")).ToList();
                            z.Add(item);



                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI == null)
                    {



                        x = GetSiparisUstKAlem().Where(t => t.URETILECEKMI == "E").ToList();




                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in cariArray)
                        {


                            x = x.Concat(GetSiparisUstKAlem().Where(t => t.CARI_DOVIZ_TIPI == item && t.URETILECEKMI == "E")).ToList();

                            m.Add(item);

                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {


                            x = x.Concat(GetSiparisUstKAlem().Where(t => t.PLASIYER_ADI == item && t.URETILECEKMI == "E")).ToList();
                            y.Add(item);


                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                x = x.Concat(GetSiparisUstKAlem().Where(t => t.PLASIYER_ADI == item && t.CARI_DOVIZ_TIPI == item2 && t.URETILECEKMI == "E")).ToList();
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
                                x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == item && t.PLASIYER_ADI == item2 && t.URETILECEKMI == "E")).ToList();
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
                                    x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == item && t.CARI_DOVIZ_TIPI == item2 && t.PLASIYER_ADI == item3)).ToList();
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


                            x = x.Concat(GetSiparisUstKAlem().Where(t => t.PLASIYER_ADI == item)).ToList();
                            y.Add(item);


                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                x = x.Concat(GetSiparisUstKAlem().Where(t => t.PLASIYER_ADI == item && t.CARI_DOVIZ_TIPI == item2)).ToList();
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
                                x = x.Concat(GetSiparisUstKAlem().Where(t => t.TESLIM_YERI == item && t.PLASIYER_ADI == item2)).ToList();
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
                Session["Filter"] = x.Where(t => t.TARIH > filter.BASLANGIC_TARIH);
                list = x.Where(t => t.TARIH > filter.BASLANGIC_TARIH).ToList();
            }
            else if (filter.BASLANGIC_TARIH == null && filter.BITIS_TARIH != null)
            {
                Session["Filter"] = x.Where(t => t.TARIH < filter.BITIS_TARIH);
                list = x.Where(t => t.TARIH < filter.BITIS_TARIH).ToList();
            }
            else if (filter.BASLANGIC_TARIH != null && filter.BITIS_TARIH != null)
            {
                Session["Filter"] = x.Where(t => t.TARIH < filter.BITIS_TARIH && t.TARIH > filter.BASLANGIC_TARIH);
                list = x.Where(t => t.TARIH < filter.BITIS_TARIH && t.TARIH > filter.BASLANGIC_TARIH).ToList();
            }
            else
            {
                Session["Filter"] = x;
            }
            var ses = ((IEnumerable<USTKALEMMODEL>)Session["Filter"]).ToList() as List<USTKALEMMODEL>;
            Session["Filter"] = ses.OrderBy(t=>t.CARI_ISIM).ToList();
            ViewBag.FilterPlasiyer = y;
            ViewBag.FilterTeslim = z;
            ViewBag.FilterDoviz = m;
            ViewBag.FilterUretilecekmi = u;
            return null;
        }
        public ActionResult FilterSatici(FilterModel filter)
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
                                    x = x.Concat(GetSiparisUstKAlemSSIP().Where(t => t.CARI_DOVIZ_TIPI == item2 && t.PLASIYER_ADI == item3 && t.TESLIM_YERI == "EFECE 4")).ToList();
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

                                x = x.Concat(GetSiparisUstKAlemSSIP().Where(t => t.CARI_DOVIZ_TIPI == item2 && t.TESLIM_YERI == "EFECE 4")).ToList();
                                z.Add(item);
                                m.Add(item2);


                            }
                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in teslimArray)
                        {


                            x = x.Concat(GetSiparisUstKAlemSSIP().Where(t => t.TESLIM_YERI == "EFECE 4")).ToList();
                            z.Add(item);



                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI == null)
                    {



                        x = GetSiparisUstKAlemSSIP().Where(t => t.TESLIM_YERI == "EFECE 4").ToList();




                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in cariArray)
                        {


                            x = x.Concat(GetSiparisUstKAlemSSIP().Where(t => t.CARI_DOVIZ_TIPI == item && t.TESLIM_YERI == "EFECE 4")).ToList();

                            m.Add(item);

                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {


                            x = x.Concat(GetSiparisUstKAlemSSIP().Where(t => t.PLASIYER_ADI == item && t.TESLIM_YERI == "EFECE 4")).ToList();
                            y.Add(item);


                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                x = x.Concat(GetSiparisUstKAlemSSIP().Where(t => t.PLASIYER_ADI == item && t.CARI_DOVIZ_TIPI == item2 && t.TESLIM_YERI == "EFECE 4")).ToList();
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
                                x = x.Concat(GetSiparisUstKAlemSSIP().Where(t => t.PLASIYER_ADI == item2 && t.TESLIM_YERI == "EFECE 4")).ToList();
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
                                    x = x.Concat(GetSiparisUstKAlemSSIP().Where(t => t.TESLIM_YERI == item && t.CARI_DOVIZ_TIPI == item2 && t.PLASIYER_ADI == item3)).ToList();
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

                                x = x.Concat(GetSiparisUstKAlemSSIP().Where(t => t.TESLIM_YERI == item && t.CARI_DOVIZ_TIPI == item2)).ToList();
                                z.Add(item);
                                m.Add(item2);


                            }
                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in teslimArray)
                        {


                            x = x.Concat(GetSiparisUstKAlemSSIP().Where(t => t.TESLIM_YERI == item)).ToList();
                            z.Add(item);



                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI == null)
                    {



                        x = GetSiparisUstKAlemSSIP().ToList();




                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI == null)
                    {
                        foreach (var item in cariArray)
                        {


                            x = x.Concat(GetSiparisUstKAlemSSIP().Where(t => t.CARI_DOVIZ_TIPI == item)).ToList();

                            m.Add(item);

                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {


                            x = x.Concat(GetSiparisUstKAlemSSIP().Where(t => t.PLASIYER_ADI == item)).ToList();
                            y.Add(item);


                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.PLASIYER_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                x = x.Concat(GetSiparisUstKAlemSSIP().Where(t => t.PLASIYER_ADI == item && t.CARI_DOVIZ_TIPI == item2)).ToList();
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
                                x = x.Concat(GetSiparisUstKAlemSSIP().Where(t => t.TESLIM_YERI == item && t.PLASIYER_ADI == item2)).ToList();
                                y.Add(item2);
                                z.Add(item);
                            }





                        }
                    }
                }


            }
            ViewBag.AltKalem = GetSiparisApiDataSSIP();
            var list = x;
            var list1 = GetSiparisApiDataSSIP();
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
            var ses = Session["Filter"] as List<USTKALEMMODEL>;
            Session["Filter"] = ses.OrderBy(t => t.CARI_ISIM);
            ViewBag.FilterPlasiyer = y;
            ViewBag.FilterTeslim = z;
            ViewBag.FilterDoviz = m;
            ViewBag.FilterUretilecekmi = u;
            return null;
        }
        public ActionResult FilterAltKalemSatici(FilterModel filter)
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
            var cariArray = cari.Split(',');
            string[] plasiyerArray = { plasiyer };



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
                                    x = x.Concat(GetSiparisApiDataSSIP().Where(t => t.CARI_DOVIZ_TIPI == item2 && t.STOK_ADI == item3 && t.TESLIM_YERI == "EFECE 4")).ToList();
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

                                x = x.Concat(GetSiparisApiDataSSIP().Where(t => t.CARI_DOVIZ_TIPI == item2 && t.TESLIM_YERI == "EFECE 4")).ToList();
                                z.Add(item);
                                m.Add(item2);


                            }
                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.STOK_ADI == null)
                    {
                        foreach (var item in teslimArray)
                        {


                            x = x.Concat(GetSiparisApiDataSSIP().Where(t => t.TESLIM_YERI == "EFECE 4")).ToList();
                            z.Add(item);



                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.STOK_ADI == null)
                    {



                        x = GetSiparisApiDataSSIP().Where(t => t.TESLIM_YERI == "EFECE 4").ToList();




                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.STOK_ADI == null)
                    {
                        foreach (var item in cariArray)
                        {


                            x = x.Concat(GetSiparisApiDataSSIP().Where(t => t.CARI_DOVIZ_TIPI == item && t.TESLIM_YERI == "EFECE 4")).ToList();

                            m.Add(item);

                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.STOK_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {


                            x = x.Concat(GetSiparisApiDataSSIP().Where(t => t.STOK_ADI == item && t.TESLIM_YERI == "EFECE 4")).ToList();
                            y.Add(item);


                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.STOK_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                x = x.Concat(GetSiparisApiDataSSIP().Where(t => t.STOK_ADI == item && t.CARI_DOVIZ_TIPI == item2 && t.TESLIM_YERI == "EFECE 4")).ToList();
                                m.Add(item2);
                                y.Add(item);
                            }





                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.STOK_ADI != null)
                    {
                        foreach (var item in teslimArray)
                        {
                            foreach (var item2 in plasiyerArray)
                            {
                                x = x.Concat(GetSiparisApiDataSSIP().Where(t => t.STOK_ADI == item2 && t.TESLIM_YERI == "EFECE 4")).ToList();
                                y.Add(item2);
                                z.Add(item);
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
                                    x = x.Concat(GetSiparisApiDataSSIP().Where(t => t.TESLIM_YERI == item && t.CARI_DOVIZ_TIPI == item2 && t.STOK_ADI == item3)).ToList();
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

                                x = x.Concat(GetSiparisApiDataSSIP().Where(t => t.TESLIM_YERI == item && t.CARI_DOVIZ_TIPI == item2)).ToList();
                                z.Add(item);
                                m.Add(item2);


                            }
                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.STOK_ADI == null)
                    {
                        foreach (var item in teslimArray)
                        {


                            x = x.Concat(GetSiparisApiDataSSIP().Where(t => t.TESLIM_YERI == item)).ToList();
                            z.Add(item);



                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.STOK_ADI == null)
                    {



                        x = GetSiparisApiDataSSIP().ToList();




                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.STOK_ADI == null)
                    {
                        foreach (var item in cariArray)
                        {


                            x = x.Concat(GetSiparisApiDataSSIP().Where(t => t.CARI_DOVIZ_TIPI == item)).ToList();

                            m.Add(item);

                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI == null && filter.STOK_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {


                            x = x.Concat(GetSiparisApiDataSSIP().Where(t => t.STOK_ADI == item)).ToList();
                            y.Add(item);


                        }
                    }
                    else if (filter.TESLIM_YERI == null && filter.CARI_DOVIZ_TIPI != null && filter.STOK_ADI != null)
                    {
                        foreach (var item in plasiyerArray)
                        {
                            foreach (var item2 in cariArray)
                            {
                                x = x.Concat(GetSiparisApiDataSSIP().Where(t => t.STOK_ADI == item && t.CARI_DOVIZ_TIPI == item2)).ToList();
                                m.Add(item2);
                                y.Add(item);
                            }





                        }
                    }
                    else if (filter.TESLIM_YERI != null && filter.CARI_DOVIZ_TIPI == null && filter.STOK_ADI != null)
                    {
                        foreach (var item in teslimArray)
                        {
                            foreach (var item2 in plasiyerArray)
                            {
                                x = x.Concat(GetSiparisApiDataSSIP().Where(t => t.TESLIM_YERI == item && t.STOK_ADI == item2)).ToList();
                                y.Add(item2);
                                z.Add(item);
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

            Session["Yuzde"] = yuzde / i;
            ViewBag.FilterPlasiyer = y;
            ViewBag.FilterTeslim = z;
            ViewBag.FilterDoviz = m;
            ViewBag.FilterUretilecekmi = u;
            return null;
        }

        public List<SiparisModel> GetSiparisApiDataSSIP()
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

            return jsonList.OrderBy(t=>t.CARI_ISIM).ToList();
        }
        public List<USTKALEMMODEL> GetSiparisUstKAlemSSIP()
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
        public ActionResult MailMusteriSip()
        {

            if (Request.Cookies["Mail"] != null)
            {

                Session["Info"] = "Başarılı";


                string subject = "NOVA | Detaylı Müşteri Siparişi Bilgisi";

                string body = "";
                var stoklar = GetSiparisApiDataSSIP();
                System.IO.MemoryStream str = DataToExcel(ToDataTable(stoklar));
                Attachment at = new Attachment(str, "DetayliMusteriSiparisi_" + DateTime.Now + ".xls");
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




            return RedirectToAction("SiparisRaporu");

        }
        public ActionResult MailSaticiSip()
        {

            if (Request.Cookies["Mail"] != null)
            {

                Session["Info"] = "Başarılı";


                string subject = "NOVA | Detaylı Satıcı Siparişi Bilgisi";

                string body = "";
                var stoklar = GetSiparisApiDataSSIP();
                System.IO.MemoryStream str = DataToExcel(ToDataTable(stoklar));
                Attachment at = new Attachment(str, "DetayliSaticiSiparisi_" + DateTime.Now + ".xls");
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




            return RedirectToAction("SaticiSiparisRaporu");

        }
        public MemoryStream DataToExcel(DataTable dt)
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