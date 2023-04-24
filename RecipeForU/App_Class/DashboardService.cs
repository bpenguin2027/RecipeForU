using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using RecipeForU.Models;

/// <summary>
/// 主控台類別
/// </summary>
public class DashboardService
{
    /// <summary>
    /// 食譜流量排行
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static RECIPE ShowPopular(int index)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var datas = db.RECIPE.OrderByDescending(m => m.view_times).ToList();
            return datas[index];
        }
    }

    /// <summary>
    /// 計算食譜總數
    /// </summary>
    /// <returns></returns>
    public static int CountRecipe()
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            return db.RECIPE.Count();
        }
    }

    /// <summary>
    /// 計算會員總數
    /// </summary>
    /// <returns></returns>
    public static int CountMember()
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            return db.MEMBER.Count();
        }
    }

    /// <summary>
    /// 計算瀏覽次數
    /// </summary>
    /// <returns></returns>
    public static int CountViewTimes()
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            return db.RECIPE.Sum(m => m.view_times);
        }
    }
}
