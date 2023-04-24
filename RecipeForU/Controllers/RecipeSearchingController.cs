using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RecipeForU.Models;
using RecipeForU.Models.ViewModel;
using PagedList;

namespace RecipeForU.Controllers
{
    public class RecipeSearchingController : Controller
    {
        /// <summary>
        /// 食譜查詢
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult RecipeSearchListForAdmin(string keyword = "", int page = 1, int pageSize = 10)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var ResultList = db.RECIPE
                    .Where(m =>
                    m.recipe_name.Contains(keyword) ||
                    m.recipe_id.Contains(keyword) ||
                    m.recipe_intro.Contains(keyword) ||
                    m.recipe_author.Contains(keyword))
                    .OrderByDescending(m => m.time)
                    .ToPagedList(page, pageSize);
                return View(ResultList);
            }
        }

        /// <summary>
        /// 後台食譜查詢
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult SearchDashboardRecipe(FormCollection collection)
        {
            string keyword = collection[collection.GetKey(0)];
            
            return RedirectToAction("SearchDashboardResult", "RecipeSearching", new { keyword = keyword });
        }

        /// <summary>
        /// 後台食譜查詢結果
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult SearchDashboardResult(string keyword = "", int page = 1, int pageSize = 10)
        {
            if (UserAccount.Role == EnumList.LoginRole.Admin)
            {
                return View(RecipeSearching.SearchByRecipe(keyword).ToPagedList(page, pageSize));
            }
            else
            {
                return View(RecipeSearching.SearchByMine(keyword).ToPagedList(page, pageSize));
            }
        }

        /// <summary>
        /// 首頁食譜查詢
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public ActionResult SearchRecipe(FormCollection collection)
        {
            string keyword = collection[collection.GetKey(0)];
            string category = collection.GetKey(0);

            return RedirectToAction("SearchResult", "RecipeSearching", new { keyword = keyword, category = category });
        }

        /// <summary>
        /// 首頁食譜查詢結果
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public ActionResult SearchResult(string keyword = "", string category = "", int page = 1, int pageSize = 10)
        {
            if (category == "keyRecipe")
            {
                return View(RecipeSearching.SearchByRecipe(keyword).ToPagedList(page, pageSize));
            }
            else
            {
                return View(RecipeSearching.SearchByElement(keyword).ToPagedList(page, pageSize));
            }
        }
    }
}
