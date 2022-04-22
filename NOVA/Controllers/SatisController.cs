using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NOVA.Controllers
{
    [_SessionController]
    public class SatisController : Controller
    {
        [HandleError]
        // GET: Satis
        public ActionResult Index()
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

            return View();
        }
        public ActionResult Maintenance()
        {
            return View("maintenance");
        }
    }
}