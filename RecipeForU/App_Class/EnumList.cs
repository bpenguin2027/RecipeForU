using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 枚舉類型
/// </summary>
public class EnumList
{
    /// <summary>
    /// 登入角色
    /// </summary>
    public enum LoginRole
    {
        /// <summary>
        /// 訪客
        /// </summary>
        [Description("訪客")]
        Guest = 0,
        /// <summary>
        /// 使用者
        /// </summary>
        [Description("使用者")]
        User = 1,
        /// <summary>
        /// 管理者
        /// </summary>
        [Description("管理者")]
        Admin = 2
    }
    /// <summary>
    /// 取得登入角色名稱
    /// </summary>
    /// <param name="enumType">登入角色代號</param>
    /// <returns></returns>
    public static string GetRoleName(string roleNo)
    {
        string str_name = "訪客";
        if (roleNo == "A") str_name = "管理者";
        if (roleNo == "U") str_name = "使用者";
        return str_name;
    }
    /// <summary>
    /// 取得登入角色類型名稱
    /// </summary>
    /// <param name="enumType">角色類型</param>
    /// <returns></returns>
    public static string GetRoleName(LoginRole enumType)
    {
        return Enum.GetName(typeof(LoginRole), enumType);
    }
    /// <summary>
    /// 取得登入角色類型
    /// </summary>
    /// <param name="roleNo">登入角色代號</param>
    /// <returns></returns>
    public static LoginRole GetRoleType(string roleNo)
    {
        LoginRole role = LoginRole.Guest;
        if (roleNo == "A") role = EnumList.LoginRole.Admin;
        if (roleNo == "U") role = EnumList.LoginRole.User;
        return role;
    }

    /// <summary>
    /// 性別
    /// </summary>
    public enum UserGender
    {
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        Male = 1,
        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        Female = 2,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 3
    }
    /// <summary>
    /// 取得性別類型名稱
    /// </summary>
    /// <param name="enumType">性別類型</param>
    /// <returns></returns>
    public static string GetGenderName(UserGender enumType)
    {
        return Enum.GetName(typeof(UserGender), enumType);
    }
}
