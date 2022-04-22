using NOVA.Models;
using SqlApi.Models;
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

namespace NOVA.Controllers
{
    [_SessionController]
    public class YonetimPaneliController : Controller
    {
        [HandleError]
        public ActionResult YetkiAyar()
        {
            if (TempData["name"] == null || TempData["id"] == null)
            {
                if (Request.Cookies["Id"].Value.ToString() == null || Request.Cookies["Id"].Value.Equals("10003") || Request.Cookies["Id"].Value.Equals("10005"))
                {
                    ViewBag.Id = "2";
                    ViewBag.Name = Request.Cookies["Name"].Value.ToString();
                }
                else
                {
                    ViewBag.Name = Request.Cookies["Name"].Value.ToString();
                    ViewBag.Id = Request.Cookies["Id"].Value.ToString();
                }
            }
            else
            {
                HttpCookie cookiei;
                HttpCookie cookien = new HttpCookie("Name", TempData["name"].ToString());
                if (TempData["id"].ToString() == "10003" || TempData["id"].ToString() == "10005")
                {
                    cookiei = new HttpCookie("Id", "2");
                }
                else
                {
                    cookiei = new HttpCookie("Id", TempData["id"].ToString());
                }

                Response.Cookies.Add(cookien);
                Response.Cookies.Add(cookiei);
                ViewBag.Name = Request.Cookies["Name"].Value.ToString();
                ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            }

            ViewBag.Users = TempData["mydata"];
            ViewBag.Module_id = TempData["moduleid"];
            if (ViewBag.Module_id == null)
            {
                ViewBag.Module_id = 0;

            }
            if (ViewBag.Users == null)
            {

                ViewBag.Users = GetUsersApiData();
            }
            var s = new SelectList(GetRoleApiData(), "ROLE_ID", "ROLE_NAME");
            ViewBag.Modules = GetModulesApiData();
            ViewBag.Roles = s;
            ViewBag.Roles1=GetRoleApiData();

            return View("Yetki");
        }
        public ActionResult KullaniciAyar()
        {
            if (TempData["name"] == null || TempData["id"] == null)
            {
                if (Request.Cookies["Id"].Value.ToString() == null || Request.Cookies["Id"].Value.Equals("10003") || Request.Cookies["Id"].Value.Equals("10005"))
                {
                    ViewBag.Id = "2";
                    ViewBag.Name = Request.Cookies["Name"].Value.ToString();
                }
                else
                {
                    ViewBag.Name = Request.Cookies["Name"].Value.ToString();
                    ViewBag.Id = Request.Cookies["Id"].Value.ToString();
                }
            }
            else
            {
                HttpCookie cookiei;
                HttpCookie cookien = new HttpCookie("Name", TempData["name"].ToString());
                if (TempData["id"].ToString() == "10003" || TempData["id"].ToString() == "10005")
                {
                    cookiei = new HttpCookie("Id", "2");
                }
                else
                {
                    cookiei = new HttpCookie("Id", TempData["id"].ToString());
                }

                Response.Cookies.Add(cookien);
                Response.Cookies.Add(cookiei);
                ViewBag.Name = Request.Cookies["Name"].Value.ToString();
                ViewBag.Id = Request.Cookies["Id"].Value.ToString();
            }

            ViewBag.Users = GetKullaniciApiData();
            
            
            var s = new SelectList(GetRoleApiData().Select(u => new { Fullname = u.ROLE_ID+"-"+u.ROLE_NAME, USER_ROLE = u.ROLE_ID,SELECTED=u.ROLE_ID}), "USER_ROLE", "Fullname");
            ViewBag.Roles = s;
            ViewBag.Roles1=GetRoleApiData();
            return View("Kullanici");
        }
        public ActionResult Test()
        {
            ViewBag.Modules = GetModulesApiData();
            ViewBag.Users = GetUsersApiData();
            var s = new SelectList(GetRoleApiData(), "ROLE_ID", "ROLE_NAME");
            ViewBag.Roles = s;
            return View("Yedek");
        }
        public ActionResult Yetki(int id)
        {
            TempData["mydata"] = GetUsersApiData(id);
            TempData["moduleid"] = GetUsersApiData(id)[0].MODULE_INCKEY;
            var s = new SelectList(GetRoleApiData(), "ROLE_ID", "ROLE_NAME");
            ViewBag.Modules = GetModulesApiData();
            ViewBag.Roles = s;

            return RedirectToAction("YetkiAyar");
        }
        public List<User> GetUsersApiData()
        {


            var apiUrl = "http://192.168.2.209:83/api/userwithroles/";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<User> jsonList = ser.Deserialize<List<User>>(json);

            //END

            return jsonList;
        }

        public List<User> GetUsersApiData(int inc)
        {


            var apiUrl = "http://192.168.2.209:83/api/userwithroles/inc:" + inc;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<User> jsonList = ser.Deserialize<List<User>>(json);

            //END

            return jsonList;
        }
        public List<Modules> GetModulesApiData()
        {


            var apiUrl = "http://192.168.2.209:83/api/modules";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Modules> jsonList = ser.Deserialize<List<Modules>>(json);

            //END

            return jsonList;
        }
        public List<Roles> GetRoleApiData()
        {


            var apiUrl = "http://192.168.2.209:83/api/roles";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Roles> jsonList = ser.Deserialize<List<Roles>>(json);

            //END

            return jsonList;
        }
        [HttpPost]
        public async Task<ActionResult> YetkiAyar(User user)
        {


            var apiUrl = "http://192.168.2.209:83/api/userwithroles/" + user.USER_ID + "/" + user.MODULE_INCKEY;


            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(user), Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);

            string apiResponse = await response.Content.ReadAsStringAsync();
            ViewBag.Users = GetUsersApiData();
            ViewBag.Roles = GetRoleApiData();



            return RedirectToAction("YetkiAyar");

        }
        [HttpPost]
        public async Task<ActionResult> KullaniciAyar(User user)
        {


            var apiUrl = "http://192.168.2.209:83/api/user/" + user.USER_ID ;


            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(user), Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);

            string apiResponse = await response.Content.ReadAsStringAsync();
            ViewBag.Users = GetKullaniciApiData();
            ViewBag.Roles = GetRoleApiData();



            return RedirectToAction("KullaniciAyar");

        }
        
        public List<User> GetKullaniciApiData()
        {


            var apiUrl = "http://192.168.2.209:83/api/user";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<User> jsonList = ser.Deserialize<List<User>>(json);

            //END

            return jsonList;
        }

        [HttpPost]
        public async Task<ActionResult> ModulEkle(Modules modul)
        {


            var apiUrl = "http://192.168.2.209:83/api/modules";


            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(modul), Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);

            string apiResponse = await response.Content.ReadAsStringAsync();
            ViewBag.Users = GetUsersApiData();
            ViewBag.Roles = GetRoleApiData();



            return RedirectToAction("YetkiAyar");

        }
        [HttpPost]
        public async Task<ActionResult> AddUser(UserAdd user)
        {

            var apiUrl = "http://192.168.2.209:83/api/user";

            var httpClient = new HttpClient();
            var req = new JavaScriptSerializer().Serialize(user);
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
            {
                Content = new StringContent(req, Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);

            string apiResponse = await response.Content.ReadAsStringAsync();
            ViewBag.Users = GetUsersApiData();


            return RedirectToAction("KullaniciAyar");

        }

        public ActionResult UserDataFromView(User user)
        {

            string name = user.USER_NAME;
            return View();
        }
    }
}