using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcFront.Interfaces;

namespace MvcFront.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserAccountRepository _userRepository;

        public HomeController(IUserAccountRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
