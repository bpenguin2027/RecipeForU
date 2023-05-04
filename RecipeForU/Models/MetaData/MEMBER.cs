using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipeForU.Models
{
    [MetadataType(typeof(MEMBERMetaData))]
    public partial class MEMBER
    {
        private class MEMBERMetaData
        {
            [Key]
            public int rowid { get; set; }

            [Display(Name = "會員身分")]
            public string member_type { get; set; }

            [Display(Name = "會員ID")] 
            public string member_id { get; set; }

            [Display(Name = "Email")]
            [Required(ErrorMessage = "請填寫有效Email")]
            [EmailAddress(ErrorMessage = "請填寫有效Email")]
            public string email { get; set; }

            [Display(Name = "密碼")]
            [Required(ErrorMessage = "請填寫密碼")]
            [StringLength(50, MinimumLength = 8, ErrorMessage = "密碼長度至少為8個字元")]
            public string password { get; set; }

            [Display(Name = "姓名")]
            [Required(ErrorMessage = "請填寫姓名")]
            public string member_name { get; set; }

            [Display(Name = "電話")]
            [Required(ErrorMessage = "請填寫10碼行動電話")]
            [RegularExpression(@"(\d){4}-(\d){3}-(\d){3}", ErrorMessage = "請填寫10碼行動電話")]
            public string phone { get; set; }

            [Display(Name = "性別")]
            [Required(ErrorMessage = "請填寫對應性別之數字")]
            [EnumDataType(typeof(EnumList.UserGender), ErrorMessage = "請填寫對應性別之數字")]
            public string gender { get; set; }

            [Display(Name = "出生日期")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public System.DateTime birthday { get; set; }

            [Display(Name = "職業")]
            public string occupation { get; set; }

            [Display(Name = "驗證")]
            public bool is_valid { get; set; }

            [Display(Name = "備註")]
            public string aka { get; set; }
        }
    }
}