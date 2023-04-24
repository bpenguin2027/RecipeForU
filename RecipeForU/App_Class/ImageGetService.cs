using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// 讀取圖片
/// </summary>
public class ImageGetService
{
    /// <summary>
    /// 讀取食譜封面圖片
    /// </summary>
    /// <param name="recipe_id"></param>
    /// <returns></returns>
    public static string GetRecipeImage(string recipe_id)
    {
        string str_image = string.Format("~/Images/Uploads/Recipe/RecipeCover/{0}.jpg", recipe_id);
        if (File.Exists(HttpContext.Current.Server.MapPath(str_image)))
        {
            str_image = string.Format("~/Images/Uploads/Recipe/RecipeCover/{0}.jpg", recipe_id);
        }
        else
        {
            str_image = "~/Images/norecipecover.jpg";
        }

        return str_image;
    }

    /// <summary>
    /// 讀取步驟封面圖片
    /// </summary>
    /// <param name="step_cover"></param>
    /// <returns></returns>
    public static string GetStepImage(string step_cover)
    {
        string str_image = string.Format("~/Images/Uploads/Recipe/StepCover/{0}.jpg", step_cover);
        if (File.Exists(HttpContext.Current.Server.MapPath(str_image)))
            str_image = string.Format("~/Images/Uploads/Recipe/StepCover/{0}.jpg", step_cover);
        else
            str_image = "~/Images/norecipecover.jpg";
        return str_image;
    }
}
