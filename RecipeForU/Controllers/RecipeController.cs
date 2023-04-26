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
    public class RecipeController : Controller
    {
        /// <summary>
        /// 我的食譜
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult MyList(int page = 1, int pageSize = 10)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var datas = db.RECIPE
                    .Where(m => m.recipe_author == UserAccount.UserNo)
                    .OrderByDescending(m => m.time)
                    .ToPagedList(page, pageSize);
                return View(datas);
            }
        }

        /// <summary>
        /// 食譜資料舊版
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Detail(int id = 0)
        {
            if (id == 0) return View(new RECIPE());
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.RECIPE.Where(m => m.rowid == id).FirstOrDefault();
                data.view_times += 1;
                db.SaveChanges();
                if (data != null) return View(data);
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// 食譜資料新版，如果將id修改為recipe_id，recipe_id和step_id格式不統一
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult RecipeDetail(string id)
        {
            RecipeDetailViewModel RecipeDatas = new RecipeDetailViewModel();
            RecipeDatas.RECIPE = RecipeDetailService.GetRecipeById(id);
            RecipeDatas.sRECIPE = RecipeDetailService.GetRecipeStepsById(id);
            RecipeDatas.eRECIPE = RecipeDetailService.GetRecipeElementsById(id);
            //RecipeDatas.Favored = RecipeDetailService.CheckFavored(UserAccount.UserNo, id);
            //RecipeDatas.Author = RecipeDetailService.GetRecipeAuthorById(id);
            RecipeDetailService.AddViewTimes(id);
            return View(RecipeDatas);
        }

        /// <summary>
        /// 修改食譜新版
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult EditRecipeDetail(string id)
        {
            RecipeDetailViewModel RecipeDatas = new RecipeDetailViewModel();
            RecipeDatas.RECIPE = RecipeDetailService.GetRecipeById(id);
            RecipeDatas.sRECIPE = RecipeDetailService.GetRecipeStepsById(id);
            RecipeDatas.eRECIPE = RecipeDetailService.GetRecipeElementsById(id);
            return View(RecipeDatas);
        }
        /// <summary>
        /// 修改食譜新版
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult EditRecipeDetail(string id, RecipeDetailViewModel RecipeDatas)
        {
            RecipeDetailService.EditRecipe(id, RecipeDatas.RECIPE);
            RecipeDetailService.EditRecipeElements(id, RecipeDatas.eRECIPE);
            RecipeDetailService.EditRecipeSteps(id, RecipeDatas.sRECIPE);
            return View("MyList", "Recipe");
        }

        /// <summary>
        /// 修改食譜舊版
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(string id)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.RECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
                if (data == null) return RedirectToAction("MyList", "Recipe");
                return View(data);
            }
        }
        /// <summary>
        /// 修改食譜舊版
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(RECIPE model)
        {
            if (!ModelState.IsValid) return View(model);
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.RECIPE.Where(m => m.recipe_id == model.recipe_id).FirstOrDefault();
                if (data == null) return View(model);
                data.recipe_author = model.recipe_author;
                data.recipe_cover = model.recipe_cover;
                data.recipe_id = model.recipe_id;
                data.recipe_intro = model.recipe_intro;
                data.recipe_name = model.recipe_name;
                data.rowid = model.rowid;
                data.time = model.time;
                data.view_times = model.view_times;
                db.SaveChanges();
                return RedirectToAction("MyList", "Recipe");
            }
        }

        /// <summary>
        /// 新增食譜舊版
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult AddRecipe()
        {
            RECIPE model = new RECIPE()
            {
                time = DateTime.Now
            };
            return View(model);
        }

        /// <summary>
        /// 新增食譜舊版
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult AddRecipe(FormCollection collection)
        {
            //接步驟資料
            var inputCount = 0;
            var inputValues = new List<string>();
            string inputname, inputintro;
            if (int.TryParse(collection["TextBoxCount"], out inputCount))
            {
                for (int i = 1; i <= inputCount; i++)
                {
                    if (!string.IsNullOrWhiteSpace(collection["textbox" + i]))
                    {
                        inputValues.Add(collection["textbox" + i]);
                    }
                }
            }
            TempData["InputResult"] = inputValues;

            //接食材資料
            var inputCountx = 0;
            var inputValuesx = new List<string>();
            var inputValuesxq = new List<string>();
            if (int.TryParse(collection["TextBoxCountx"], out inputCountx))
            {
                for (int i = 1; i <= inputCountx; i++)
                {
                    if (!string.IsNullOrWhiteSpace(collection["textboxx" + i]))
                    {
                        inputValuesx.Add(collection["textboxx" + i]);
                        inputValuesxq.Add(collection["textboxxq" + i]);
                    }
                }
            }
            TempData["InputResultx"] = inputValuesx;

            //接食譜資料
            inputname = collection["textbox_name"];
            inputintro = collection["textbox_intro"];
            TempData["InputResultname"] = inputname;
            TempData["InputResultintro"] = inputintro;

            //傳入資料庫
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                //食譜資料
                RECIPE data = new RECIPE();
                data.recipe_author = UserAccount.UserNo;
                data.recipe_cover = "0";
                data.recipe_id = "尚未編號";
                data.recipe_intro = TempData["InputResultintro"].ToString();
                data.recipe_name = TempData["InputResultname"].ToString();
                data.recommended = false;
                data.time = DateTime.Now;
                data.view_times = 0;
                db.RECIPE.Add(data);
                db.SaveChanges();

                //步驟資料
                foreach (var item in (List<string>)TempData["InputResult"])
                {
                    sRECIPE sdata = new sRECIPE();
                    sdata.recipe_id = "尚未編號";
                    sdata.step_body = item;
                    sdata.step_cover = "0";
                    sdata.step_id = "0";
                    db.sRECIPE.Add(sdata);
                }
                //食材資料
                int l = 0;
                foreach (var item in (List<string>)TempData["InputResultx"])
                {
                    eRECIPE edata = new eRECIPE();
                    edata.recipe_id = "尚未編號";
                    edata.element_id = item;
                    edata.qty = inputValuesxq[l];
                    db.eRECIPE.Add(edata);
                    l++;
                }
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }

        /// <summary>
        /// 刪除食譜
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult Delete(string id)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.RECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
                if (data != null)
                {
                    db.RECIPE.Remove(data);
                    db.SaveChanges();
                }
                var sdata = db.sRECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
                if (sdata != null)
                {
                    db.sRECIPE.Remove(sdata);
                    db.SaveChanges();
                }
                var edata = db.eRECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
                if (edata != null)
                {
                    db.eRECIPE.Remove(edata);
                    db.SaveChanges();
                }
                return RedirectToAction("MyList", "Recipe");
            }
        }

        /// <summary>
        /// 多張圖片上傳舊版
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FileUpload()
        {
            string paths = "";
            //多張圖片上傳
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                var fileName = Guid.NewGuid().ToString("N");
                var filePath = Server.MapPath("~/Image/Temp/");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                paths += "~/Image/Temp/" + fileName + ",";
                file.SaveAs(Path.Combine(filePath, fileName));
            }
            return Json(new { paths }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增Like
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult Put(string id)
        {
            bool flag = RecipeDetailService.CheckFavored(UserAccount.UserNo, id);
            if (flag == false)
            {
                RecipeDetailService.AddFavor(UserAccount.UserNo, id);
            }
            return View();
        }

        /// <summary>
        /// 刪除Like
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult Pop(string id)
        {
            bool Favored = RecipeDetailService.CheckFavored(UserAccount.UserNo, id);
            if (Favored == true)
            {
                RecipeDetailService.PopFavor(UserAccount.UserNo, id);
            }
            return View();
        }

        /// <summary>
        /// 新增推薦
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult SetRecommended(string id)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.RECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
                data.recommended = true;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Recipes");
        }
        /// <summary>
        /// 刪除推薦
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult CancelRecommended(string id)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.RECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
                data.recommended = false;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Recipes");
        }

        /// <summary>
        /// 我的食譜收藏
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult MyFavoredList(int page = 1, int pageSize = 10)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var ResultList = (from s in db.RECIPE
                                  join a in db.rMEMBER on s.recipe_id equals a.recipe_id
                                  where a.member_id == UserAccount.UserNo
                                  select s)
                                  .OrderBy(s => s.time)
                                  .ToPagedList(page, pageSize);
                return View(ResultList);
            }
        }
    }
}

