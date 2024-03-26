using NOVA.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static NOVA.Controllers.LoginController;
using System.Web.Security;
using System.Text;
using System.Threading.Tasks;
using NOVA.Utils;

namespace NOVA.Controllers
{
    public class YonetimController : Controller
    {
        public async Task<ActionResult> Index()
        {
            int moduleId = 8;

            List<Modules> Modules = await AuthHelper.GetModules(moduleId);

            if (Modules[0].ACTIVE != "1")
            {
                return RedirectToAction("Maintenance", "Home");
            }
            if (Request.Cookies["Id"] == null)
            {
                return RedirectToAction("Index", "Home");
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
                    string Action = this.ControllerContext.RouteData.Values["action"].ToString();
                    string Controller = this.ControllerContext.RouteData.Values["controller"].ToString();

                    FormsAuthentication.SignOut();

                    return RedirectToAction($"Login?ReturnUrl=%2f{Controller}%2f{Action}", "Login");
                }
            }

            await RoleHelper.CheckRoles(this);

            return View();
        }

        public async Task<ActionResult> Stok()
        {
            int moduleId = 33;

            List<Modules> Modules = await AuthHelper.GetModules(moduleId);

            if (Modules[0].ACTIVE != "1")
            {
                return RedirectToAction("Maintenance", "Home");
            }
            if (Request.Cookies["Id"] == null)
            {
                return RedirectToAction("Index", "Home");
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
                    string Action = this.ControllerContext.RouteData.Values["action"].ToString();
                    string Controller = this.ControllerContext.RouteData.Values["controller"].ToString();

                    FormsAuthentication.SignOut();

                    return RedirectToAction($"Login?ReturnUrl=%2f{Controller}%2f{Action}", "Login");
                }
            }

            await RoleHelper.CheckRoles(this);

            return View();
        }
    }
}