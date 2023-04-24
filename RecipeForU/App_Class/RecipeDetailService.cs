using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using RecipeForU.Models;

/// <summary>
/// 抓取食譜明細、修改食譜
/// </summary>
public class RecipeDetailService
{
    /// <summary>
    /// 透過id對應食譜
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static RECIPE GetRecipeById(string id)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var data = db.RECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
            return data;
        }
    }

    /// <summary>
    /// 透過id對應食譜步驟
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static List<sRECIPE> GetRecipeStepsById(string id)
    {
        List<sRECIPE> steps = new List<sRECIPE>();
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var data = db.sRECIPE.Where(m => m.recipe_id == id).OrderBy(m => m.step_id);
            foreach (var item in data)
            {
                steps.Add(item);
            }
            return steps;
        }
    }

    /// <summary>
    /// 透過id對應食材
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static List<eRECIPE> GetRecipeElementsById(string id)
    {
        List<eRECIPE> elements = new List<eRECIPE>();
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var data = db.eRECIPE.Where(m => m.recipe_id == id);
            foreach (var item in data)
            {
                elements.Add(item);
            }
            return elements;
        }
    }

    /// <summary>
    /// 透過id對應作者
    /// </summary>
    /// <param name="recipe_id"></param>
    /// <returns></returns>
    public static string GetRecipeAuthorById(string id)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            string member_id = db.RECIPE.Where(m => m.recipe_id == id).Select(m => m.recipe_author).FirstOrDefault();
            string member_name = db.MEMBER.Where(m => m.member_id == member_id).Select(m => m.member_name).FirstOrDefault();
            return member_name;
        }
    }

    /// <summary>
    /// 推薦食譜排序
    /// </summary>
    /// <param name="place"></param>
    /// <returns></returns>
    public static RECIPE ShowRecomended(int place)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var datas = db.RECIPE.Where(m => m.recommended == true).OrderByDescending(m => m.view_times).ToList();
            var oneatatime = datas[place];
            return oneatatime;
        }
    }

    /// <summary>
    /// 熱門食譜排序
    /// </summary>
    /// <param name="place"></param>
    /// <returns></returns>
    public static RECIPE ShowPopular(int place)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var datas = db.RECIPE.OrderByDescending(m => m.view_times).ToList();
            var oneatatime = datas[place];
            return oneatatime;
        }
    }

    /// <summary>
    /// 檢查是否收藏此食譜
    /// </summary>
    /// <param name="member_id"></param>
    /// <param name="recipe_id"></param>
    /// <returns></returns>
    public static bool CheckFavored(string member_id, string recipe_id)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            bool favored = true;
            if (db.rMEMBER.Where(m => m.recipe_id == recipe_id && m.member_id == member_id).FirstOrDefault() == null)
            {
                favored = false;
            }

            return favored;
        }
    }

    public static void AddFavor(string member_id, string recipe_id)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            rMEMBER data = new rMEMBER();
            data.recipe_id = recipe_id;
            data.member_id = member_id;
            db.rMEMBER.Add(data);
            db.SaveChanges();
        }
    }
    public static void PopFavor(string member_id, string recipe_id)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            if (db.rMEMBER.Where(m => m.recipe_id == recipe_id && m.member_id == member_id) != null)
            {
                var data = db.rMEMBER.Where(m => m.recipe_id == recipe_id && m.member_id == member_id).FirstOrDefault();
                db.rMEMBER.Remove(data);
                db.SaveChanges();
            }
        }
    }

    public static int CountFavored(string id)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var data = db.rMEMBER.Where(m => m.recipe_id == id).Count();
            return data;
        }
    }

    /// <summary>
    /// 增加瀏覽次數
    /// </summary>
    /// <param name="recipe_id"></param>
    public static void AddViewTimes(string id)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var data = db.RECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
            data.view_times += 1;
            db.SaveChanges();
        }
    }

    /// <summary>
    /// 修改食譜資料
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newdata"></param>
    public static void EditRecipe(string id, RECIPE newData)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var data = db.RECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
            data.recipe_name = newData.recipe_name;
            data.recipe_intro = newData.recipe_intro;
            db.SaveChanges();
        }
    }

    /// <summary>
    /// 修改食材資料
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newdata"></param>
    public static void EditRecipeElements(string id, List<eRECIPE> newDatas)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var data = db.eRECIPE.Where(m => m.recipe_id == id);

            foreach (var item in data)
            {
                item.element_id = "To be deleted";
            }

            foreach (var item in newDatas)
            {
                eRECIPE newData = new eRECIPE();
                newData.element_id = item.element_id;
                newData.qty = item.qty;
                newData.recipe_id = id;
                db.eRECIPE.Add(newData);
            }
            db.SaveChanges();
        }
    }

    /// <summary>
    /// 修改步驟資料
    /// </summary>
    /// <param name="prevdata"></param>
    /// <param name="newdata"></param>
    public static void EditRecipeSteps(string id, List<sRECIPE> newdata)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var data = db.sRECIPE.Where(m => m.recipe_id == id);
            foreach (var item in data)
            {
                item.step_id = "To be deleted";
            }
            int i = 1;
            foreach (var item in newdata)
            {
                sRECIPE newData = new sRECIPE();
                newData.step_id = i.ToString().PadLeft(2, '0');
                newData.step_body = item.step_body;
                newData.recipe_id = id;
                db.sRECIPE.Add(newData);
                i++;
            }
            db.SaveChanges();
        }
    }
}


