﻿using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcFront.Helpers
{
    public static class HtmlExtentions
    {
        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper,string linkText,string actionName,string controllerName)
        {
            string currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            string currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            if (controllerName == currentController && currentAction == actionName)
            {
                return htmlHelper.ActionLink(
                    linkText,
                    actionName,
                    controllerName,
                    null,
                    new
                    {
                        @class = "current"
                    });
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName);
        }
    }
}