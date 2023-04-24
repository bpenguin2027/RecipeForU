using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipeForU.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}