using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipeForU.Models;
using PagedList;

namespace RecipeForU.Controllers
{
    public class RecipesController : Controller
    {
        /// <summary>
        /// 全站食譜列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var datas = db.RECIPE
                    .Where(m => m.recipe_id != null && m.recipe_id.Trim() != null)
                    .OrderByDescending(m => m.time)
                    .ToPagedList(page, pageSize);
                return View(datas);
            }
        }

        /// <summary>
        /// 新增食譜新版
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        [HttpGet]
        public ActionResult RecipeAdd(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                RecipeService.NewRecipeID();
                RecipeViewModel model = new RecipeViewModel()
                {
                    ActionNo = "Add",
                    RowID = "0",
                    StepNo = "01",
                    RecipeID = RecipeService.RecipeID,
                    RecipeName = "",
                    RecipeIntro = ""
                };

                RecipeService.Recipe = model;
                RecipeService.RecipeElement = new List<eRECIPE>();
                RecipeService.RecipeStep = new List<sRECIPE>();
                return View(model);
            }
            else
            {
                if (RecipeService.RecipeStep.Count > 0)
                {
                    RecipeService.RecipeStep = RecipeService.RecipeStep.OrderBy(m => m.step_id).ToList();
                }
                return View(RecipeService.Recipe);
            }
        }
        /// <summary>
        /// 新增食譜新版
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        [HttpPost]
        public ActionResult RecipeAdd(RecipeViewModel model)
        {
            RecipeService.Recipe = model;
            string str_action_no = "";
            if (model.ActionNo == "ElementAdd") str_action_no = model.ActionNo;
            if (model.ActionNo == "ElementEdit") str_action_no = model.ActionNo;
            if (model.ActionNo == "ElementDelete") str_action_no = model.ActionNo;
            if (model.ActionNo == "StepAdd") str_action_no = model.ActionNo;
            if (model.ActionNo == "StepEdit") str_action_no = model.ActionNo;
            if (model.ActionNo == "StepDelete") str_action_no = model.ActionNo;
            if (model.ActionNo == "SaveRecipe") str_action_no = model.ActionNo;
            if (model.ActionNo == "RecipeUpload") str_action_no = "UploadImage";
            if (model.ActionNo == "StepUpload") str_action_no = "UploadImage";

            if (!string.IsNullOrEmpty(str_action_no))
                return RedirectToAction(str_action_no);

            return RedirectToAction("RecipeAdd", new { id = RecipeService.RecipeID });
        }

        /// <summary>
        /// 新增食材新版
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        [HttpGet]
        public ActionResult ElementAdd()
        {
            int int_rowid = 1;
            if (RecipeService.RecipeElement.Count > 0)
            {
                var data = RecipeService.RecipeElement.OrderByDescending(m => m.rowid).First();
                int_rowid = data.rowid + 1;
            }

            eRECIPE model = new eRECIPE();
            model.rowid = int_rowid;
            model.recipe_id = RecipeService.RecipeID;
            model.element_id = "";
            model.qty = "";
            model.ActionNo = "";
            return View(model);
        }

        /// <summary>
        /// 新增食材新版
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        [HttpPost]
        public ActionResult ElementAdd(eRECIPE model)
        {
            if (!ModelState.IsValid) return View(model);
            RecipeService.RecipeElement.Add(model);

            if (model.ActionNo == "SaveContinue") return RedirectToAction("ElementAdd");
            return RedirectToAction("RecipeAdd", new { id = RecipeService.RecipeID });
        }

        /// <summary>
        /// 修改食材
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        [HttpGet]
        public ActionResult ElementEdit()
        {
            int int_rowid = int.Parse(RecipeService.Recipe.RowID);
            var model = RecipeService.RecipeElement.Where(m => m.rowid == int_rowid).FirstOrDefault();
            return View(model);
        }
        /// <summary>
        /// 修改食材
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        [HttpPost]
        public ActionResult ElementEdit(eRECIPE model)
        {
            if (!ModelState.IsValid) return View(model);
            var data = RecipeService.RecipeElement.Where(m => m.rowid == model.rowid).First();
            var index = RecipeService.RecipeElement.IndexOf(data);
            if (index >= 0) RecipeService.RecipeElement[index] = model;
            return RedirectToAction("RecipeAdd", new { id = RecipeService.RecipeID });
        }

        /// <summary>
        /// 刪除食材
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        [HttpGet]
        public ActionResult ElementDelete()
        {
            int int_rowid = int.Parse(RecipeService.Recipe.RowID);
            var model = RecipeService.RecipeElement.Where(m => m.rowid == int_rowid).FirstOrDefault();
            if (model != null) { RecipeService.RecipeElement.Remove(model); }
            return RedirectToAction("RecipeAdd", new { id = RecipeService.RecipeID });
        }

        /// <summary>
        /// 新增步驟
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        [HttpGet]
        public ActionResult StepAdd()
        {
            int step_id = 1;
            string str_step_id = step_id.ToString().PadLeft(2, '0');
            if (RecipeService.RecipeStep.Count > 0)
            {
                sRECIPE data = RecipeService.RecipeStep.OrderByDescending(m => m.rowid).FirstOrDefault();
                step_id = RecipeService.RecipeStep.Count + 1;
                str_step_id = step_id.ToString().PadLeft(2, '0');               
            }

            sRECIPE model = new sRECIPE();
            model.recipe_id = RecipeService.RecipeID;
            model.step_id = str_step_id;
            model.step_body = "";
            model.step_cover = RecipeService.RecipeID + "_" + str_step_id;
            return View(model);
        }
        /// <summary>
        /// 新增步驟
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        [HttpPost]
        public ActionResult StepAdd(sRECIPE model)
        {
            if (!ModelState.IsValid)
                return View(model);
            RecipeService.RecipeStep.Add(model);
            if (model.ActionNo == "SaveContinue")
                return RedirectToAction("StepAdd");
            return RedirectToAction("RecipeAdd", new { id = RecipeService.RecipeID });
        }

        /// <summary>
        /// 修改步驟
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        [HttpGet]
        public ActionResult StepEdit()
        {
            string recipe_id = RecipeService.Recipe.RecipeID;
            string step_id = RecipeService.Recipe.StepNo;
            sRECIPE model = RecipeService.RecipeStep.Where(m => m.recipe_id == recipe_id && m.step_id == step_id).FirstOrDefault();
            return View(model);
        }

        /// <summary>
        /// 修改步驟
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        [HttpPost]
        public ActionResult StepEdit(sRECIPE model)
        {
            string recipe_id = RecipeService.Recipe.RecipeID;
            string step_id = RecipeService.Recipe.StepNo;
            if (!ModelState.IsValid) return View(model);
            sRECIPE data = RecipeService.RecipeStep.Where(m => m.recipe_id == model.recipe_id && m.step_id == step_id).FirstOrDefault();
            int index = RecipeService.RecipeStep.IndexOf(data);
            if (index >= 0) RecipeService.RecipeStep[index] = model;
            return RedirectToAction("RecipeAdd", new { id = RecipeService.RecipeID });
        }

        /// <summary>
        /// 刪除步驟
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        [HttpGet]
        public ActionResult StepDelete()
        {
            string recipe_id = RecipeService.Recipe.RecipeID;
            string step_id = RecipeService.Recipe.StepNo;
            sRECIPE model = RecipeService.RecipeStep.Where(m => m.recipe_id == recipe_id && m.step_id == step_id).FirstOrDefault();
            if (model != null)
            {
                RecipeService.RecipeStep.Remove(model);
                if (RecipeService.RecipeStep.Count > 0)
                {
                    for (int i = 0; i < RecipeService.RecipeStep.Count; i++)
                    {
                        RecipeService.RecipeStep[i].step_id = (i + 1).ToString().PadLeft(2, '0');
                    }
                }
            }
            return RedirectToAction("RecipeAdd", new { id = RecipeService.RecipeID });
        }

        /// <summary>
        /// 儲存食譜
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult SaveRecipe()
        {
            RecipeService.AddRecipeData();
            return RedirectToAction("MyList", "Recipe");
        }

        /// <summary>
        /// 圖片上傳進入點
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize(RoleList = "Admin,User")]
        [HttpGet]
        public ActionResult UploadImage()
        {
            if (RecipeService.Recipe.ActionNo == "RecipeUpload")
            {
                ImageUpdateService.ImageTitle = string.Format("食譜 {0}：上傳圖片", RecipeService.Recipe.RecipeName);
                ImageUpdateService.ImageName = RecipeService.Recipe.RecipeID;
            }
            if (RecipeService.Recipe.ActionNo == "StepUpload")
            {
                var data = RecipeService.GetRecipeStep(RecipeService.Recipe.RecipeID);
                ImageUpdateService.ImageTitle = string.Format("食譜 {0} - {1}：上傳圖片", RecipeService.Recipe.RecipeName, data.step_id);
                ImageUpdateService.ImageName = data.step_cover;
            }
            ImageUpdateService.ImageFolder = "~/Images/Uploads";
            ImageUpdateService.ImageSubFolder = "Temp";
            ImageUpdateService.ImageExtention = "jpg";
            ImageUpdateService.UploadImageMode = true;
            ImageUpdateService.ReturnAction("Recipes", "RecipeAdd", "id", "image");
            return RedirectToAction("UploadImage", "Image");
        }
    }
}