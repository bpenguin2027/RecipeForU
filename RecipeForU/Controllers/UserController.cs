using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipeForU.Models;
using RecipeForU.Models.ViewModel;
using PagedList;

namespace RecipeForU.Controllers
{
    public class UserController : Controller
    {
        /// <summary>
        /// 會員列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult List(int page = 1, int pageSize = 10)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var datas = db.MEMBER
                    .Where(m => m.member_type == "U")
                    .OrderBy(m => m.rowid)
                    .ToPagedList(page, pageSize);
                return View(datas);
            }
        }

        /// <summary>
        /// 會員登入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            UserAccount.Logout();
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }
        /// <summary>
        /// 會員登入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            using (Cryptographys cryp = new Cryptographys())
            {
                string str_password = cryp.SHA256Encode(model.password);
                using (RecipeForUEntities db = new RecipeForUEntities())
                {
                    var data = db.MEMBER
                        .Where(m => m.email == model.email)
                        .Where(m => m.password == str_password)
                        .FirstOrDefault();

                    if (data == null)
                    {
                        ModelState.AddModelError("email", "會員信箱或密碼輸入錯誤");
                        return View(model);
                    }

                    if (data.is_valid == false)
                    {
                        ModelState.AddModelError("email", "此信箱未完成驗證");
                        return View(model);
                    }

                    UserAccount.UserEmail = model.email;
                    UserAccount.Login();

                    if (UserAccount.Role == EnumList.LoginRole.Admin)
                    {
                        return RedirectToAction("Dashboard", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
        }

        /// <summary>
        /// 管理者新增會員
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Create()
        {
            MEMBER model = new MEMBER()
            {
                birthday = DateTime.Today,
                is_valid = false
            };
            return View(model);
        }
        /// <summary>
        /// 管理者新增會員
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Create(MEMBER model)
        {

            if (!ModelState.IsValid) return View(model);
            using (Cryptographys cryp = new Cryptographys())
            {
                using (RecipeForUEntities db = new RecipeForUEntities())
                {
                    MEMBER data = new MEMBER();
                    data.member_id = "";
                    data.member_name = model.member_name;
                    data.member_type = "U";
                    data.password = cryp.SHA256Encode(model.password);
                    data.birthday = model.birthday;
                    data.email = model.email;
                    data.phone = model.phone;
                    data.gender = model.gender;
                    data.occupation = model.occupation;
                    data.is_valid = model.is_valid;
                    data.aka = model.aka;
                    db.MEMBER.Add(data);
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
            }
        }

        /// <summary>
        /// 管理者修改會員資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(int id)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.MEMBER.Where(m => m.rowid == id).FirstOrDefault();
                if (data == null) return RedirectToAction("List");
                return View(data);
            }
        }
        /// <summary>
        /// 管理者修改會員資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(MEMBER model)
        {
            if (!ModelState.IsValid) return View(model);
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.MEMBER.Where(m => m.rowid == model.rowid).FirstOrDefault();
                if (data == null) return View(model);
                data.member_name = model.member_name;
                data.birthday = model.birthday;
                data.email = model.email;
                data.phone = model.phone;
                data.gender = model.gender;
                data.occupation = model.occupation;
                data.aka = model.aka;
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }

        /// <summary>
        /// 管理者刪除會員
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Delete(int id)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.MEMBER.Where(m => m.rowid == id).FirstOrDefault();
                if (data != null)
                {
                    db.MEMBER.Remove(data);
                    db.SaveChanges();
                }
                return RedirectToAction("List", "User");
            }
        }

        /// <summary>
        /// 會員密碼變更
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult ResetPassword()
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            return View(model);
        }
        /// <summary>
        /// 會員密碼變更
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            using (Cryptographys cryp = new Cryptographys())
            {
                string str_password = cryp.SHA256Encode(model.NewPassword);
                using (RecipeForUEntities db = new RecipeForUEntities())
                {
                    var data = db.MEMBER.Where(m => m.email == UserAccount.UserEmail).FirstOrDefault();
                    if (data != null)
                    {
                        data.password = str_password;
                        db.SaveChanges();
                    }
                    TempData["MessageText"] = "密碼已更新，下次登入請使用新的密碼";
                    return RedirectToAction("MessageText");
                }
            }
        }

        /// <summary>
        /// 會員資料修改
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditMyInfo()
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.MEMBER.Where(m => m.email == UserAccount.UserEmail).FirstOrDefault();
                if (data == null) return RedirectToAction("Index");
                return View(data);
            }
        }
        /// <summary>
        /// 會員資料修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditMyInfo(MEMBER model)
        {
            if (!ModelState.IsValid) return View(model);
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.MEMBER.Where(m => m.email == UserAccount.UserEmail).FirstOrDefault();
                if (data == null) return View(model);
                data.member_name = model.member_name;
                data.birthday = model.birthday;
                data.email = model.email;
                data.phone = model.phone;
                data.gender = model.gender;
                data.occupation = model.occupation;
                data.aka = model.aka;

                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            string str_key = Guid.NewGuid()
                .ToString()
                .Replace("-", "")
                .ToUpper()
                .Substring(0, 20);
            MEMBER model = new MEMBER()
            {
                member_type = "U",
                birthday = DateTime.Today,
                is_valid = false,
                aka = str_key
            };
            return View(model);
        }
        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(MEMBER model)
        {
            if (!ModelState.IsValid) return View(model);
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.MEMBER.Where(m => m.email == model.email).FirstOrDefault();
                if (data != null)
                {
                    ModelState.AddModelError("email", "電子信箱重覆註冊！");
                    return View(model);
                }

                using (Cryptographys cryp = new Cryptographys())
                {
                    MEMBER data_member = new MEMBER();
                    string str_password = cryp.SHA256Encode(model.password);

                    data_member.member_id = "";
                    data_member.member_name = model.member_name;
                    data_member.member_type = "U";
                    data_member.password = cryp.SHA256Encode(model.password);
                    data_member.birthday = model.birthday;
                    data_member.email = model.email;
                    data_member.phone = model.phone;
                    data_member.gender = model.gender;
                    data_member.occupation = model.occupation;
                    data_member.is_valid = model.is_valid;
                    data_member.aka = model.aka;

                    db.MEMBER.Add(data_member);
                    db.SaveChanges();

                    //寄出註冊驗證信件
                    using (AppMail mail = new AppMail())
                    {
                        mail.UserRegister(model.email);
                    }
                    TempData["MessageText"] = "驗證信已寄出，請前往信箱確認。";
                    return RedirectToAction("MessageText");
                }
            }
        }

        /// <summary>
        /// 會員註冊電子信箱驗證
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Verify(string guid)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                bool bln_valid = false;
                var data = db.MEMBER.Where(m => m.aka == guid).FirstOrDefault();
                if (data == null)
                {
                    TempData["MessageText"] = "驗證碼找不到";
                    return RedirectToAction("MessageText");
                }
                bln_valid = data.is_valid;
                if (bln_valid)
                {
                    TempData["MessageText"] = "此電子信箱已驗證過，不可重覆驗證";
                    return RedirectToAction("MessageText");
                }
                data.is_valid = true;
                data.aka = "";
                db.SaveChanges();
                TempData["MessageText"] = "電子信箱已驗證成功";
                return RedirectToAction("MessageText");
            }
        }

        /// <summary>
        /// 跨頁面傳送訊息
        /// </summary>
        /// <returns></returns>
        public ActionResult MessageText()
        {
            ViewBag.MessageText = TempData["MessageText"].ToString();
            return View();
        }
    }
}