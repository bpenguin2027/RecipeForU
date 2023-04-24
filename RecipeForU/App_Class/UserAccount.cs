using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RecipeForU.Models;

/// <summary>
/// 使用者資訊類別
/// </summary>
public static class UserAccount
{
    /// <summary>
    /// 登入使用者角色
    /// </summary>
    public static EnumList.LoginRole Role { get; set; } = EnumList.LoginRole.Guest;
    /// <summary>
    /// 登入使用者角色名稱
    /// </summary>
    public static string RoleName { get { return EnumList.GetRoleName(Role); } }
    /// <summary>
    /// 使用者帳號
    /// </summary>
    public static string UserNo { get; set; } = "";
    /// <summary>
    /// 使用者名稱
    /// </summary>
    public static string UserName { get; set; } = "";
    /// <summary>
    /// 使用者電子信箱
    /// </summary>
    public static string UserEmail { get; set; } = "";
    /// <summary>
    /// 使用者是否已登入
    /// </summary>
    public static bool IsLogin { get; set; } = false;

    public static void Login()
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var data = db.MEMBER.Where(m => m.email == UserEmail).FirstOrDefault();
            if (data == null)
                Logout();
            else
            {
                IsLogin = true;
                UserName = data.member_name;
                UserEmail = data.email;
                UserNo = data.member_id;
                Role = EnumList.GetRoleType(data.member_type);
            }
        }
    }

    public static void Logout()
    {
        IsLogin = false;
        Role = EnumList.LoginRole.Guest;
        UserNo = "";
        UserName = "";
        UserEmail = "";
        UserNo = "";
    }
}
