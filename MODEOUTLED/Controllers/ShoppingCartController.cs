using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
namespace MODEOUTLED.Controllers
{
    public class ShoppingCartController : Controller
    {
        tuan_nanyspa_vn_dbEntities db = new tuan_nanyspa_vn_dbEntities();

        public ActionResult Cart()
        {
            return View();
        }

    }
}
