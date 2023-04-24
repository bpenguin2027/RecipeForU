using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipeForU.Models
{
    public class ResetPasswordViewModel
    {
        [Display(Name = "新的密碼")]
        [Required(ErrorMessage = "新的密碼不可空白")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "密碼長度至少為8個字元")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = "確認密碼不可空白")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "密碼長度至少為8個字元")]
        [Compare("NewPassword", ErrorMessage = "新密碼與確認密碼不相同")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}