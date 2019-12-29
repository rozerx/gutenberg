using GutenbergProjectVBS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GutenbergProjectVBS.Web.Filters
{
    public class AuthFilterUserNull : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if(CurrentSession.User == null)
            {
                filterContext.Controller.TempData.Add("UserLoginFail", "Please, login or create account.");
                filterContext.Result = new RedirectResult("/Account/Login");
            }
        }

    }
}