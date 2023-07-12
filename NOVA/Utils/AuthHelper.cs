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

namespace NOVA.Utils
{
    public static class AuthHelper
    {
        public static bool LoginLog(string Id, string LogId, int LAST_ACTIVITY) 
        {
            #region Id
            string json1 = null;
            LoginModel createdlog = null;
            var apiUrl1 = "http://192.168.2.13:83/api/UserLogin/" + Id.ToInt();
            Uri url1 = new Uri(apiUrl1);
            WebClient client1 = new WebClient();
            client1.Encoding = System.Text.Encoding.UTF8;

            json1 = client1.DownloadString(url1);
            JavaScriptSerializer ser1 = new JavaScriptSerializer();
            createdlog = ser1.Deserialize<LoginModel>(json1);
            #endregion

            #region LogId
            string json2 = null;
            List<ExecModel> createdlog1 = null;
            var apiUrl2 = "http://192.168.2.13:83/api/UserLogin/exec/" + LogId;
            Uri url2 = new Uri(apiUrl2);
            WebClient client2 = new WebClient();
            client2.Encoding = System.Text.Encoding.UTF8;

            json2 = client2.DownloadString(url2);
            JavaScriptSerializer ser2 = new JavaScriptSerializer();
            createdlog1 = ser2.Deserialize<List<ExecModel>>(json2);
            #endregion

            if (createdlog1[0].SITUATION != false)
            {
                LoginModel login = new LoginModel();
                login.LOG_ID = createdlog.LOG_ID;
                login.LAST_ACTIVITY = LAST_ACTIVITY;
                var apiUrlnew = "http://192.168.2.13:83/api/UserLogin";

                var httpClientnew = new System.Net.Http.HttpClient();
                var requestnew = new HttpRequestMessage(HttpMethod.Put, apiUrlnew)
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(login), Encoding.UTF8, "application/json")
                };

                var responsenew = httpClientnew.SendAsync(requestnew);

                return true;
            }

            return false;
        }

        public static List<SignIn> GetSession(int id)
        {
            var apiUrl = $"http://192.168.2.13:83/api/log/max:{id}";

            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);

            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SignIn> jsonList = ser.Deserialize<List<SignIn>>(json);

            return jsonList;
        }
    }
}