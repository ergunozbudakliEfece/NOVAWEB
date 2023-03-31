using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NOVA.Controllers
{
    public class ScannerController : Controller
    {
        [AllowAnonymous]
        // GET: Scanner
        public ActionResult Scanner()
        {
            return View();
        }
    }
}