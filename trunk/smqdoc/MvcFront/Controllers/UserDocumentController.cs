using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Helpers;
using MvcFront.Interfaces;
using MvcFront.Models;

namespace MvcFront.Controllers
{
    public class UserDocumentController : Controller
    {
        private readonly IDocumentRepository _documentRepository;
        public UserDocumentController(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        //
        // GET: /Document/

        public ActionResult Index()
        {
            var sessData = SessionHelper.GetUserSessionData(Session);
            return View(_documentRepository.GetAll().Where(x => x.Status != (int)DocumentStatus.Deleted && x.UserAccount_userid == sessData.UserId)
                .ToList().ConvertAll(DocumentListViewModel.DocumentToModelConverter).ToList());
        }

        //
        // GET: /Document/Details/5

        public ActionResult Details(long id)
        {
            return View(_documentRepository.GetDocumentById(id));
        }

        //
        // GET: /Document/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Document/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Document/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Document/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Document/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Document/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
