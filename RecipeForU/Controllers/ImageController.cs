using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipeForU.Controllers
{
    public class ImageController : Controller
    {
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult UploadImage()
        {
            return View();
        }

        [LoginAuthorize(RoleList = "Admin,User")]
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            ImageUpdateService.UserUploadImage(file);
            //if (!string.IsNullOrEmpty(ImageUpdateService.ReturnAreaName))
            //{
            //if (!string.IsNullOrEmpty(ImageUpdateService.ReturnParmName))
            //{
            //    return RedirectToAction(ImageUpdateService.ReturnActionName, ImageUpdateService.ReturnControllerName, new { area = ImageUpdateService.ReturnActionName, id = ImageUpdateService.ReturnParmValue });
            //}
            //else
            //{
            //    return RedirectToAction(ImageUpdateService.ReturnActionName, ImageUpdateService.ReturnControllerName, new { area = ImageUpdateService.ReturnActionName });
            //}
            //}
            //轉址位置
            if (!string.IsNullOrEmpty(ImageUpdateService.ReturnParmName))
            {
                return RedirectToAction(ImageUpdateService.ReturnActionName, ImageUpdateService.ReturnControllerName, new { id = ImageUpdateService.ReturnParmValue });
            }
            else
            {
                return RedirectToAction(ImageUpdateService.ReturnActionName, ImageUpdateService.ReturnControllerName);
            }
        }
    }
}