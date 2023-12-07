using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json;
using NOVA.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static ServiceStack.Diagnostics.Events;

namespace NOVA.Utils
{
    public static class RoleHelper
    {
        public static async Task CheckRoles(Controller Controller)
        {
            using (System.Net.Http.HttpClient Client = new System.Net.Http.HttpClient())
            {
                using (HttpResponseMessage response = await Client.GetAsync($"http://192.168.2.13:83/api/user/auth/{Controller.Request.Cookies["Id"].Value.ToInt()}"))
                {
                    var byteArray = await response.Content.ReadAsByteArrayAsync();
                    var result = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

                    List<User> yetki = new JavaScriptSerializer().Deserialize<List<User>>(result);

                    #region Check Role
                    bool? yetkiKontrolSatis = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 1).USER_AUTH;
                    bool? yetkiKontrolStok = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 2).USER_AUTH;
                    bool? yetkiKontrolUretim = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 3).USER_AUTH;
                    bool? yetkiKontrolSatinAlma = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 4).USER_AUTH;
                    bool? yetkiKontrolFinans = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 5).USER_AUTH;
                    bool? yetkiKontrolMuhasebe = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 6).USER_AUTH;
                    bool? yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 9).USER_AUTH;
                    bool? kullaniciayar = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 10).USER_AUTH;
                    bool? kullaniciyetki = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 11).USER_AUTH;
                    bool? istatistik = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 12).USER_AUTH;
                    bool? yetkiKontrolYonetim = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 8).USER_AUTH;
                    bool? yetkiKontrolSube = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 15).USER_AUTH;
                    bool? yetkiKontrolSevkiyat = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 17).USER_AUTH;
                    bool? yetkiKontrolDetayliSip = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 16).USER_AUTH;
                    bool? yetkiKontrolSiparisRaporu = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 18).USER_AUTH;
                    bool? yetkiKontrolIstatistik = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 8).USER_AUTH;
                    bool? yetkidetaylisatinalma = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 19).USER_AUTH;
                    bool? yetkisaticisiparisi = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 20).USER_AUTH;
                    bool? isemrikayit = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 27).USER_AUTH;
                    bool? yetkianlikuretim = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 26).USER_AUTH;
                    bool? yetkifiyatlistok = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 28).USER_AUTH;
                    bool? yetkifiyatsizstok = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 29).USER_AUTH;
                    bool? ziyaretkaydi = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
                    bool? musteriraporu = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
                    bool? musteriraporuozel = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 31).USER_AUTH;
                    bool? ziyaretplani = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
                    bool? yonetimstok = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 33).USER_AUTH;
                    bool? fiyatyonetim = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 34).USER_AUTH;
                    bool? fiyatlistesi = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 35).USER_AUTH;
                    bool? kuryetki = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 36).USER_AUTH;
                    bool? uygulamaistatistik = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 37).USER_AUTH;
                    bool? puantaj = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 38).USER_AUTH;
                    bool? teklif = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 39).USER_AUTH;
                    bool? tekliflerim = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 40).USER_AUTH;
                    bool? ik = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 7).USER_AUTH;
                    bool? ik1 = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 13).USER_AUTH;
                    bool? ik2 = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 14).USER_AUTH;
                    bool? yetkiMalKabulForm = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 41).USER_AUTH;
                    bool? yetkiSevkMalKabul = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 42).USER_AUTH;
                    bool? depoTransfer = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 44).USER_AUTH;
                    bool? mes = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 45).USER_AUTH;
                    bool? hurdaveikincikalite = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 23).USER_AUTH;
                    #endregion

                    #region SetViewBag
                    Controller.ViewBag.DisplayTekliflerim = tekliflerim is true ? "block" : "none";
                    Controller.ViewBag.DisplayTeklif = teklif is true ? "block" : "none";
                    Controller.ViewBag.Puantaj = puantaj is true ? "block" : "none";
                    Controller.ViewBag.Istatistik = uygulamaistatistik is true ? "block" : "none";
                    Controller.ViewBag.DisplayKur = kuryetki is true ? "block" : "none";
                    Controller.ViewBag.DisplayFiyatListesi = fiyatlistesi is true ? "block" : "none";
                    Controller.ViewBag.FiyatYonetim = fiyatyonetim is true ? "block" : "none";
                    Controller.ViewBag.Stok = yonetimstok is true ? "block" : "none";
                    Controller.ViewBag.DisplayZiyaretPlani = ziyaretplani is true ? "block" : "none";
                    Controller.ViewBag.DisplayMusteriOzel = musteriraporuozel is true ? "block" : "none";
                    Controller.ViewBag.DisplayMusteriRaporu = ((musteriraporu == true && musteriraporuozel == false) || (Controller.Request.Cookies["Id"].Value == "10001" || Controller.Request.Cookies["Id"].Value == "10002")) ? "block" : "none";
                    Controller.ViewBag.DisplayZiyaretKaydi = ziyaretkaydi is true ? "block" : "none";
                    Controller.ViewBag.DisplayFiyatsizStok = yetkifiyatsizstok is true ? "block" : "none";
                    Controller.ViewBag.DisplayFiyatliStok = yetkifiyatlistok is true ? "block" : "none";
                    Controller.ViewBag.DisplayAnlikUretim = yetkianlikuretim is true ? "block" : "none";
                    Controller.ViewBag.DisplayIsEmriKayit = isemrikayit is true ? "block" : "none";
                    Controller.ViewBag.DisplaySaticiSiparisRaporu = yetkisaticisiparisi is true ? "block" : "none";
                    Controller.ViewBag.DisplaySatinAlmaRaporu = yetkidetaylisatinalma is true ? "block" : "none";
                    Controller.ViewBag.DisplayIstatistik = yetkiKontrolIstatistik is true ? "block" : "none";
                    Controller.ViewBag.DisplaySiparisRaporu = yetkiKontrolSiparisRaporu is true ? "block" : "none";
                    Controller.ViewBag.DisplaySevkiyat = yetkiKontrolSevkiyat is true ? "block" : "none";
                    Controller.ViewBag.DisplaySiparis = yetkiKontrolDetayliSip is true ? "block" : "none";
                    Controller.ViewBag.Sube = yetkiKontrolSube is true ? "block" : "none";
                    Controller.ViewBag.DisplayYonetim = yetkiKontrolYonetim is true ? "block" : "none";
                    Controller.ViewBag.DisplayUretim = yetkiKontrolUretim is true ? "block" : "none";
                    Controller.ViewBag.DisplaySatinAlma = yetkiKontrolSatinAlma is true ? "block" : "none";
                    Controller.ViewBag.DisplayFinans = yetkiKontrolFinans is true ? "block" : "none";
                    Controller.ViewBag.DisplayMuhasebe = yetkiKontrolMuhasebe is true ? "block" : "none";
                    Controller.ViewBag.DisplayStok = yetkiKontrolStok is true ? "block" : "none";
                    Controller.ViewBag.DisplaySatis = yetkiKontrolSatis is true ? "block" : "none";
                    Controller.ViewBag.Display = yetkiKontrol is true ? "block" : "none";
                    Controller.ViewBag.Display1 = kullaniciayar is true ? "block" : "none";
                    Controller.ViewBag.Display2 = kullaniciyetki is true ? "block" : "none";
                    Controller.ViewBag.Display3 = istatistik is true ? "block" : "none";
                    Controller.ViewBag.Display4 = ik is true ? "block" : "none";
                    Controller.ViewBag.Display5 = ik1 is true ? "block" : "none";
                    Controller.ViewBag.Display6 = ik2 is true ? "block" : "none";
                    Controller.ViewBag.DisplayMalKabulForm = yetkiMalKabulForm is true ? "block" : "none";
                    Controller.ViewBag.DisplaySevkMalKabul = yetkiSevkMalKabul is true ? "block" : "none";
                    Controller.ViewBag.DisplayDepoTransfer = depoTransfer is true ? "block" : "none";
                    Controller.ViewBag.DisplayMES = mes is true ? "block" : "none";
                    Controller.ViewBag.DisplayHurdaVeIkinciKalite = hurdaveikincikalite is true ? "block" : "none";
                    #endregion
                }
            }
        }

        public static async Task<User> RoleControl(string UserId, int ModuleId)
        {
            using (System.Net.Http.HttpClient Client = new System.Net.Http.HttpClient())
            {
                using (HttpResponseMessage response = await Client.GetAsync($"http://192.168.2.13:83/api/user/auth/{UserId.ToInt()}"))
                {
                    var byteArray = await response.Content.ReadAsByteArrayAsync();
                    var result = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

                    List<User> Users = new JavaScriptSerializer().Deserialize<List<User>>(result);

                    return Users.FirstOrDefault(t => t.USER_ID == UserId && t.MODULE_INCKEY == ModuleId);
                }
            }
        }
    }
}