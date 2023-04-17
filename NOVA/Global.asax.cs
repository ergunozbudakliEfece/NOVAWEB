

using ServiceStack;
using SqlApi.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace NOVA
{
    public class MvcApplication : System.Web.HttpApplication
    {
        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;

        }
     

        protected void Session_Start(object sender, EventArgs e)
        {
            
            //when session in start application variable is increased by 1  
            Application.Lock();
            
            Application.UnLock();
        }
        protected void Session_Abandon(object sender, EventArgs e)
        {
            Log log = new Log();
            log.USER_NAME = Request.Cookies["UserName"].Value;
            log.USER_ID = Request.Cookies["Id"].Value.ToInt();
            log.ACTIVITY_START = Request.Cookies["Login"].Value;
            log.ACTIVITY_END = DateTime.Now.ToString();
            log.MODULE_ID = 0;
            log.PROGRAM_ID = 0;
            log.ACTIVITY_TYPE = "Log in";
            var apiUrl = "http://192.168.2.13:83/api/userlog/" + Request.Cookies["Login"].Value;

        }


        protected void Page_Unload()
        {
            
        }



    }

}
