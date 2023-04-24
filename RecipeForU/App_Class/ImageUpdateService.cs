using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// 圖片上傳
/// </summary>
public static class ImageUpdateService
{
    /// <summary>
    /// 圖片分類標題
    /// </summary>
    public static string ImageTitle { get; set; } = "圖片上傳";
    /// <summary>
    /// 圖片資料夾路徑
    /// </summary>
    public static string ImageFolder { get; set; } = "~/Images/Uploads";
    /// <summary>
    /// 圖片資料夾子路徑
    /// </summary>
    public static string ImageSubFolder { get; set; } = "Temp";
    /// <summary>
    /// 圖片檔名
    /// </summary>
    public static string ImageName { get; set; } = "001";
    /// <summary>
    /// 圖片副檔名
    public static string ImageExtention { get; set; } = "jpg";
    //public static string ReturnAreaName { get; set; } = "";
    public static string ReturnControllerName { get; set; } = "Home";
    public static string ReturnActionName { get; set; } = "Index";
    public static string ReturnParmName { get; set; } = "";
    public static string ReturnParmValue { get; set; } = "";
    /// <summary>
    /// 圖片資料夾複合路徑
    /// </summary>
    public static string ImageFolderName { get { return string.Format("{0}/{1}", ImageFolder, ImageSubFolder); } }
    /// <summary>
    /// 圖片複合檔名
    /// </summary>
    public static string ImageFileName { get { return string.Format("{0}.{1}", ImageName, ImageExtention); } }
    /// <summary>
    /// 圖片完整路徑
    /// </summary>
    public static string ImageUrl
    {
        get
        {
            return string.Format("~/{0}/{1}/{2}.{3}?t={4}", ImageFolder, ImageSubFolder, ImageName, ImageExtention, DateTime.Now.ToString("yyyyMMddHHmmss"));
        }
    }
    /// <summary>
    /// 圖片預覽路徑
    /// </summary>
    public static string RecipeImageUrl { get { return string.Format("{0}/{1}/{2}.jpg?t={3}", ImageFolder, ImageSubFolder, RecipeService.RecipeID, DateTime.Now.ToString("yyyyMMddHHmmss")); } }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="imageName"></param>
    /// <returns></returns>
    public static string GetImageUrl(int imageName)
    {
        return string.Format("~/{0}/{1}/{2}.{3}?t={4}", ImageFolder, ImageSubFolder, imageName, ImageExtention, DateTime.Now.ToString("yyyyMMddHHmmss"));
    }
    /// <summary>
    /// 
    /// </summary>
    public static void CheckImageFolder()
    {
        if (!Directory.Exists(HttpContext.Current.Server.MapPath(ImageFolderName)))
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(ImageFolderName));
    }
    /// <summary>
    /// 圖片上傳狀態
    /// </summary>
    public static bool UploadImageMode { get; set; } = false;
    /// <summary>
    /// 圖片上傳後轉址位置
    /// </summary>
    /// <param name="areaName"></param>
    /// <param name="controllerName"></param>
    /// <param name="actionName"></param>
    /// <param name="parmName"></param>
    /// <param name="parmValue"></param>
    public static void ReturnAction(string controllerName, string actionName, string parmName, string parmValue)
    {
        //ReturnAreaName = areaName;
        ReturnControllerName = controllerName;
        ReturnActionName = actionName;
        ReturnParmName = parmName;
        ReturnParmValue = parmValue;
    }
    /// <summary>
    /// 上傳圖片
    /// </summary>
    /// <param name="file"></param>
    public static void UserUploadImage(HttpPostedFileBase file)
    {
        if (file != null)
        {
            if (file.ContentLength > 0)
            {
                var path = Path.Combine(HttpContext.Current.Server.MapPath(ImageFolderName), ImageFileName);
                if (File.Exists(path)) File.Delete(path);
                file.SaveAs(path);
            }
        }
        UploadImageMode = false;
    }
}
