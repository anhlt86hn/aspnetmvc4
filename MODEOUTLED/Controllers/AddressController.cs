using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;

namespace MODEOUTLED.Controllers.Address
{
    public class AddressController : Controller
    {
        //
        // GET: /Members/
        tuan_nanyspa_vn_dbEntities db = new tuan_nanyspa_vn_dbEntities();

        public ActionResult Index()
        {
            
            return View();
        }

        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }

        

    }
}