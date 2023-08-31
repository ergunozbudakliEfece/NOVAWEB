using NOVA.Models;
using static NOVA.Controllers.LoginController;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Web.Script.Serialization;
using System;
using System.Web.Mvc;
using ServiceStack;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Office2010.Excel;
using Azure;
using System.Web.UI.WebControls;
using System.Net.Http.Headers;
using System.Net.Mime;
using static NOVA.Controllers.HomeController;
using ServiceStack.Web;
using static ServiceStack.Diagnostics.Events;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.WebUtilities;
using Azure.Core;

namespace NOVA.Utils
{
    public static class AuthHelper
    {
        public static async Task<bool> LoginLog(string Id, string LogId, int LAST_ACTIVITY)
        {
            LoginModel LoginModel;
            List<ExecModel> ExecModel;

            using (System.Net.Http.HttpClient Client = new System.Net.Http.HttpClient())
            {
                string LoginResponse = await Client.GetStringAsync($"http://192.168.2.13:83/api/UserLogin/{Id.ToInt()}");
                LoginModel = new JavaScriptSerializer().Deserialize<LoginModel>(LoginResponse);

                string ExecResponse = await Client.GetStringAsync($"http://192.168.2.13:83/api/UserLogin/exec/{LogId}");
                ExecModel = new JavaScriptSerializer().Deserialize<List<ExecModel>>(ExecResponse);

                if (ExecModel[0].SITUATION != false)
                {
                    LoginModel Login = new LoginModel();
                    Login.LOG_ID = LoginModel.LOG_ID;
                    Login.LAST_ACTIVITY = LAST_ACTIVITY;

                    await Client.PutAsync("http://192.168.2.13:83/api/UserLogin", new StringContent(new JavaScriptSerializer().Serialize(Login), Encoding.UTF8, "application/json"));

                    return true;
                }

                return false;
            }

            //#region Id
            //string json1 = null;
            //LoginModel createdlog = null;
            //var apiUrl1 = "http://192.168.2.13:83/api/UserLogin/" + Id.ToInt();
            //Uri url1 = new Uri(apiUrl1);
            //WebClient client1 = new WebClient();
            //client1.Encoding = System.Text.Encoding.UTF8;

            //json1 = client1.DownloadString(url1);
            //JavaScriptSerializer ser1 = new JavaScriptSerializer();
            //createdlog = ser1.Deserialize<LoginModel>(json1);
            //#endregion

            //#region LogId
            //string json2 = null;
            //List<ExecModel> createdlog1 = null;
            //var apiUrl2 = "http://192.168.2.13:83/api/UserLogin/exec/" + LogId;
            //Uri url2 = new Uri(apiUrl2);
            //WebClient client2 = new WebClient();
            //client2.Encoding = System.Text.Encoding.UTF8;

            //json2 = client2.DownloadString(url2);
            //JavaScriptSerializer ser2 = new JavaScriptSerializer();
            //createdlog1 = ser2.Deserialize<List<ExecModel>>(json2);
            //#endregion

            //if (createdlog1[0].SITUATION != false)
            //{
            //    LoginModel login = new LoginModel();
            //    login.LOG_ID = createdlog.LOG_ID;
            //    login.LAST_ACTIVITY = LAST_ACTIVITY;
            //    var apiUrlnew = "http://192.168.2.13:83/api/UserLogin";

            //    var httpClientnew = new System.Net.Http.HttpClient();
            //    var requestnew = new HttpRequestMessage(HttpMethod.Put, apiUrlnew)
            //    {
            //        Content = new StringContent(new JavaScriptSerializer().Serialize(login), Encoding.UTF8, "application/json")
            //    };

            //    var responsenew = httpClientnew.SendAsync(requestnew);

            //    return true;
            //}

            //return false;
        }

        public static async Task<List<Modules>> GetModules(int id)
        {
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync("http://192.168.2.13:83/api/modules"))
                {
                    var byteArray = await response.Content.ReadAsByteArrayAsync();
                    var result = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

                    List<Modules> Modules = new JavaScriptSerializer().Deserialize<List<Modules>>(result);

                    return Modules.Where(x => x.INCKEY == id).ToList();
                }
            }

            //var apiUrl = "http://192.168.2.13:83/api/modules";
            ////Connect API
            //Uri url = new Uri(apiUrl);
            //WebClient client = new WebClient();
            //client.Encoding = System.Text.Encoding.UTF8;
            //string json = client.DownloadString(url);
            //JavaScriptSerializer ser = new JavaScriptSerializer();
            //List<Modules> jsonList = ser.Deserialize<List<Modules>>(json);
            ////END
            //return jsonList.Where(x => x.INCKEY == id).ToList();
        }

        public static async Task<string> GetToken(string Route, string Email, string Password)
        {
            string URI = $"http://192.168.2.13:83/api/login/{Route}";
            string myParameters = $"Email={Email}&Password={Password}";
            using (System.Net.Http.HttpClient Client = new System.Net.Http.HttpClient())
            {
                HttpResponseMessage Response = await Client.PostAsync(URI, new StringContent(myParameters, Encoding.UTF8, "application/x-www-form-urlencoded"));

                return await Response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> GetUrlWithToken(string Url, string Token)
        {
            Uri url = new Uri(Url);
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.Headers.Add("Authorization", "Bearer " + Token);
            string json = client.DownloadString(url);

            return json;
        }

        public static async Task<string> PostWithToken(string Url, string Token, SupernovaRequestModel Model)
        {
            using (System.Net.Http.HttpClient Client = new System.Net.Http.HttpClient())
            {
                Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
                var Response = await Client.PostAsJsonAsync(Url, Model);
                var StringResult = await Response.Content.ReadAsStringAsync();

                return StringResult;
            }
        }

    }
}