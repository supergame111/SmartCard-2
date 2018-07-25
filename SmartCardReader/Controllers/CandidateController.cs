using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SmartCard.Models;
using SmartCardReader.DAL;
using SmartCardReader.Models;

namespace SmartCardReader.Controllers
{
    public class CandidateController : Controller
    {
        private CandidateContext db = new CandidateContext();
        public static BeIDSmartCard sm;
        public CandidateController()
        {
            sm = new BeIDSmartCard();

        }
        // GET: CandidateGrids
        public ActionResult Index()
        {
            try
            {
                return View(db.Candidates.ToList());
            }
            catch (Exception e)
            {
                throw;
                //return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
            
        }
        
        // GET: CandidateGrids/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CandidateGrid candidateGrid = db.Candidates.Find(id);
            if (candidateGrid == null)
            {
                return HttpNotFound();
            }
            return View(candidateGrid);
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

                return View("Create",new CandidateGrid { Name = e.Message});
            }
        }

        // POST: CandidateGrids/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Description,Office,PrimaryPhone,Status,City,ActionCreatedOn")] CandidateGrid candidateGrid)
        {
            if (ModelState.IsValid)
            {
                db.Candidates.Add(candidateGrid);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(candidateGrid);
        }

        // GET: CandidateGrids/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CandidateGrid candidateGrid = db.Candidates.Find(id);
            if (candidateGrid == null)
            {
                return HttpNotFound();
            }
            return View(candidateGrid);
        }

        // POST: CandidateGrids/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Description,Office,PrimaryPhone,Status,City,ActionCreatedOn")] CandidateGrid candidateGrid)
        {
            if (ModelState.IsValid)
            {
                db.Entry(candidateGrid).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(candidateGrid);
        }

        // GET: CandidateGrids/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CandidateGrid candidateGrid = db.Candidates.Find(id);
            if (candidateGrid == null)
            {
                return HttpNotFound();
            }
            return View(candidateGrid);
        }

        // POST: CandidateGrids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CandidateGrid candidateGrid = db.Candidates.Find(id);
            db.Candidates.Remove(candidateGrid);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
