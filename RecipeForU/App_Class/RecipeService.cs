using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using RecipeForU.Models;

/// <summary>
/// 上傳食譜
/// </summary>
public static class RecipeService
{
    /// <summary>
    /// 食譜ID
    /// </summary>
    public static string RecipeID { get; set; } = NewRecipeID();

    /// <summary>
    /// 新增食譜ID
    /// </summary>
    /// <returns></returns>
    public static string NewRecipeID()
    {
        string str_now = DateTime.Now.ToString("yyyyMMddHHmmss");
        string str_guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
        RecipeID = string.Format("{0}_{1}", str_now, str_guid);
        return RecipeID;
    }

    /// <summary>
    /// 食譜封面圖片連結
    /// </summary>
    /// <param name="imageName"></param>
    /// <returns></returns>
    public static string RecipeImageUrl(string imageName)
    {
        return string.Format("~/Images/Uploads/Recipe/RecipeCover/{0}.jpg", imageName);
    }

    /// <summary>
    /// 食譜步驟圖片連結
    /// </summary>
    /// <param name="imageName"></param>
    /// <returns></returns>
    public static string StepImageUrl(string imageName)
    {
        return string.Format("~/Images/Uploads/Recipe/StepCover/{0}.jpg", imageName);
    }

    /// <summary>
    /// 暫存連結
    /// </summary>
    /// <param name="imageName"></param>
    /// <returns></returns>
    public static string TempImageUrl(string imageName)
    {
        return string.Format("~/Images/Uploads/Temp/{0}.jpg", imageName);
    }

    /// <summary>
    /// 利用Session儲存食譜資料
    /// </summary>
    public static RecipeViewModel Recipe
    {
        get
        {
            if (HttpContext.Current.Session["Recipe"] == null)
                return new RecipeViewModel();
            return (RecipeViewModel)HttpContext.Current.Session["Recipe"];
        }
        set { HttpContext.Current.Session["Recipe"] = value; }
    }

    /// <summary>
    /// 利用Session儲存食材資料
    /// </summary>
    public static List<eRECIPE> RecipeElement
    {
        get
        {
            if (HttpContext.Current.Session["RecipeElement"] == null)
                return new List<eRECIPE>();
            return (List<eRECIPE>)HttpContext.Current.Session["RecipeElement"];
        }
        set { HttpContext.Current.Session["RecipeElement"] = value; }
    }

    /// <summary>
    /// 利用Session儲存步驟資料
    /// </summary>
    public static List<sRECIPE> RecipeStep
    {
        get
        {
            if (HttpContext.Current.Session["RecipeStep"] == null) return new List<sRECIPE>();
            return (List<sRECIPE>)HttpContext.Current.Session["RecipeStep"];
        }
        set { HttpContext.Current.Session["RecipeStep"] = value; }
    }

    /// <summary>
    /// 讀取食譜步驟
    /// </summary>
    /// <param name="rowID"></param>
    /// <returns></returns>
    public static sRECIPE GetRecipeStep(string RowID)
    {
        return RecipeStep.Where(m => m.recipe_id == RowID).FirstOrDefault();
    }

    /// <summary>
    /// 圖片由暫存區正式移至儲存位置
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="fileName"></param>
    private static void MoveFileFromTemp(string typeName, string fileName)
    {
        string str_file = TempImageUrl(fileName);
        string str_file_name = HttpContext.Current.Server.MapPath(str_file);
        string str_new = "";
        string str_new_name = "";
        if (File.Exists(str_file_name))
        {
            if (typeName == "Recipe") str_new = RecipeImageUrl(fileName);
            if (typeName == "Step") str_new = StepImageUrl(fileName);
            if (!string.IsNullOrEmpty(str_new))
            {
                str_new_name = HttpContext.Current.Server.MapPath(str_new);
                if (File.Exists(str_new_name)) File.Delete(str_new_name);
                File.Copy(str_file_name, str_new_name);
                File.Delete(str_file_name);
            }
        }
    }

    /// <summary>
    /// 儲存食譜
    /// </summary>
    public static void AddRecipeData()
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            RECIPE newRecipe = new RECIPE();
            newRecipe.recipe_id = RecipeID;
            newRecipe.recipe_name = Recipe.RecipeName;
            newRecipe.recipe_author = UserAccount.UserNo;
            newRecipe.time = DateTime.Now;
            newRecipe.view_times = 0;
            newRecipe.recipe_intro = Recipe.RecipeIntro;
            newRecipe.recipe_cover = RecipeID;
            
            db.RECIPE.Add(newRecipe);
            db.SaveChanges();
            MoveFileFromTemp("Recipe", RecipeID);

            if (RecipeElement.Count() > 0)
            {
                foreach (var item in RecipeElement)
                {
                    db.eRECIPE.Add(item);
                }
                db.SaveChanges();
            }
            if (RecipeStep.Count() > 0)
            {
                foreach (var item in RecipeStep)
                {
                    db.sRECIPE.Add(item);
                    MoveFileFromTemp("Step", item.step_cover);
                }
                db.SaveChanges();
            }
        }
    }
}
