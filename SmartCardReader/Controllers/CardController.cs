using SmartCard.Models;
using SmartCardReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartCardReader.Controllers
{
    public class CardController : Controller
    {
        public static SmartCardReaderInterop sm;
        public CardController()
        {
            sm = new SmartCardReaderInterop();

        }
        // GET: Card
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        public ActionResult ReadCard()
        {
            try
            {

                IdentityCard candidate;
                sm.PullData(out candidate);
                return View("Create", candidate);
            }
            catch (Exception e)
            {
                throw;
                //return View("Create", new IdentityCard { Firstnames = e.Message });
            }
        }
    }
}