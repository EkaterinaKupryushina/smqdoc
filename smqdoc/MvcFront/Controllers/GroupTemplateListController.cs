using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcFront.Interfaces;
using MvcFront.DB;
using MvcFront.Models;


namespace MvcFront.Controllers
{
    public class GroupTemplateListController : Controller
    {
        private readonly IGroupTemplateRepository _groupTemplateRepository;
        public GroupTemplateListController(IGroupTemplateRepository groupTemplateRepository)
        {
            _groupTemplateRepository = groupTemplateRepository;
        }

        // GET: /GroupTemplate/

        public ActionResult Index()
        {
            return View(_groupTemplateRepository.GetAllGroupTemplates().Where(x => x.Status != (int)GroupTemplateStatus.Deleted).ToList().ConvertAll(GroupTemplateListViewModel.GroupTemplateToModelConverter).ToList());
        }

    }
}
