using Net.Sf.Pkcs11;
using SmartCard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartCardReader.Controllers
{
    public class SmartCardController : ApiController
    {
        public static BeIDSmartCard sm;
        public SmartCardController()
        {
            sm = new BeIDSmartCard();
        }

        [HttpGet]
        public IHttpActionResult Status(int id)
        {
            try
            {
                bool status = sm.IsSlotActive(id);
                return Json(new { Id = id, Active = status });
            }
            catch (IndexOutOfRangeException e)
            {
                return Json(new { Id = id, Active = false, Error = "Slot ID not assigned" });
            }

        }

        [HttpGet]
        public IHttpActionResult Status()
        {
            try
            {
                bool status = sm.IsSlotActive(0);
                return Json(new { Id = 0, Active = status });

            }
            catch (IndexOutOfRangeException e)
            {
                return Json(new { Id = 0, Active = false,Error = "Slot ID not assigned" });
            }
           
        }

        [HttpGet]
        [ActionName("Slot")]
        public IHttpActionResult MainSlot()
        {
            return Slot(0);
        }

        [HttpGet]
        public IHttpActionResult Slot(int id)
        {
            try
            {
                if (sm.IsSlotActive(id))
                {
                    return Json(sm.GetSlotInfo(id));
                }
                else
                {

                    return Json(new { Error = "Geen kaart gededecteerd" });
                }
            }
            catch (IndexOutOfRangeException e)
            {
                return Json(new { Id = id, Message = $"No info available for slot {id}" });
            }

        }

        [HttpGet]
        [ActionName("Slots")]
        public IHttpActionResult GetSlots()
        {
            try
            {
                //Slot[] slots = sm.Slots(); //Generates error
                //return Json(new { Slots = slots});
                return Content(HttpStatusCode.Accepted,"Not implemented");
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message });
            }
        }
    }
}
