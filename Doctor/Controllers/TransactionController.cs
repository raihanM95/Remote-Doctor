using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doctor.Controllers
{
    public class TransactionController : Controller
    {
        // GET: Transaction
        public ActionResult Index()
        {
            return this.PartialView();
        }

        //public ActionResult Transaction()
        //{
        //    return RedirectToAction("Index", "Transaction");
        //}
    }
}