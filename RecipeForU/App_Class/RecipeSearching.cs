using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using RecipeForU.Models;

/// <summary>
/// 食譜查詢功能
/// </summary>
public class RecipeSearching
{
    /// <summary>
    /// 以食譜名稱查詢
    /// </summary>
    /// <param name="keyword"></param>
    /// <returns></returns>
    public static List<RECIPE> SearchByRecipe(string keyword)
    {
        string[] keywordList = keyword.Split(' ');
        List<RECIPE> AllResultList = new List<RECIPE>();

        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            foreach (string keywords in keywordList)
            {
                List<RECIPE> ResultList = db.RECIPE
                    .Where(m => m.recipe_name.Contains(keywords))
                    .ToList();

                AllResultList = AllResultList.Concat(ResultList).Distinct().ToList();
            }
        }

        return AllResultList;
    }

    /// <summary>
    /// 以食譜名稱查詢我的食譜
    /// </summary>
    /// <param name="keyword"></param>
    /// <returns></returns>
    public static List<RECIPE> SearchByMine(string keyword)
    {
        string[] keywordList = keyword.Split(' ');
        List<RECIPE> AllResultList = new List<RECIPE>();

        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            foreach (string keywords in keywordList)
            {
                List<RECIPE> ResultList = db.RECIPE
                    .Where(m => m.recipe_name.Contains(keywords) && m.recipe_author == UserAccount.UserNo)
                    .ToList();

                AllResultList = AllResultList.Concat(ResultList).Distinct().ToList();
            }
        }

        return AllResultList;
    }

    /// <summary>
    /// 以食材名稱查詢
    /// </summary>
    /// <param name="keyword"></param>
    /// <returns></returns>
    public static List<RECIPE> SearchByElement(string keyword)
    {
        string[] keywordList = keyword.Split(' ');
        List<RECIPE> AllResultList = new List<RECIPE>();

        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            foreach (string keywords in keywordList)
            {
                var ResultList = (from r in db.RECIPE
                                  join e in db.eRECIPE
                                  on r.recipe_id equals e.recipe_id
                                  where e.element_id == keywords
                                  select r)
                                  .ToList();

                AllResultList = AllResultList.Concat(ResultList).Distinct().ToList();
            }
        }

        return AllResultList;
    }

    /// <summary>
    /// 以作者名稱查詢
    /// </summary>
    /// <param name="keyword"></param>
    /// <returns></returns>
    public static List<RECIPE> SearchByAuthor(string keyword)
    {
        string[] keywordList = keyword.Split(' ');
        List<RECIPE> AllResultList = new List<RECIPE>();

        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            foreach (string keywords in keywordList)
            {
                var ResultList = (from r in db.RECIPE
                                  join m in db.MEMBER
                                  on r.recipe_author equals m.member_id
                                  where m.member_id == keywords
                                  select r)
                                  .ToList();

                AllResultList = AllResultList.Concat(ResultList).Distinct().ToList();
            }
        }

        return AllResultList;
    }
}
