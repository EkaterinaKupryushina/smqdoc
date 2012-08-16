using System.Web;
using System.Web.Mvc;
using MvcFront.Enums;
using MvcFront.Helpers;

namespace MvcFront.Infrastructure.Security
{
    public class GroupUserAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var sessData = SessionHelper.GetUserSessionData(httpContext.Session);
            return sessData != null && sessData.UserType == UserProfileTypes.Groupuser;
        }
    }
}