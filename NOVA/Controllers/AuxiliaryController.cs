using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NOVA.Controllers
{
    public class AuxiliaryController : Controller
    {
        [HttpPost]
        public JsonResult IsLogOff()
        {
            // You may do stuff here
            return new JsonResult { Data = "success" };
        }
    }
}