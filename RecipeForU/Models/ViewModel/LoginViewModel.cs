using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipeForU.Models.ViewModel
{
    public class LoginViewModel
    {
        [Display(Name = "會員信箱")]
        //[RegularExpression(@"([A-Za-z0-9.]{6,30})(@.)([A-Za-z0-9.]+)$", ErrorMessage = "請填寫有效會員信箱")]
        [Required(ErrorMessage = "請填寫有效會員信箱")]
        [EmailAddress(ErrorMessage = "請填寫有效會員信箱")]
        public string email { get; set; }
        [Display(Name = "會員密碼")]
        [Required(ErrorMessage = "請填寫有效會員密碼")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}