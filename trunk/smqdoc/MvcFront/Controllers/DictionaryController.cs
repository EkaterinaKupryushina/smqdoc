using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcFront.Interfaces;

namespace MvcFront.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly IUserAccountRepository _userRepository;

        public DictionaryController(IUserAccountRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #region Пользователи

        /// <summary>
        ///   Загрузка списка пользователей по заданной маске.
        /// </summary>
        /// <param name="text"> Маска для поиска по названию. </param>
        [HttpPost]
        public ActionResult AjaxUserAccountList(string text)
        {
            var data = _userRepository.GetAll();
            if (!String.IsNullOrEmpty(text))
            {
                data = data.Where(p => p.Login != null && p.Login.ToLower().Contains(text.ToLower())).Take(20);
            }
            return new JsonResult { Data = new SelectList(data.ToList(), "Id", "Login") };
        }

        #endregion
    }
}
