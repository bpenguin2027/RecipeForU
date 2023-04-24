using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipeForU.Models
{
    [MetadataType(typeof(eRECIPEMetaData))]
    public partial class eRECIPE
    {
        public string ActionNo { get; set; }
        private class eRECIPEMetaData
        {
            [Key]
            public int rowid { get; set; }
            [Display(Name = "食譜代號")]
            public string recipe_id { get; set; }
            [Display(Name = "食材名稱")]
            [Required(ErrorMessage = "食材名稱不可空白!!")]
            public string element_id { get; set; }
            [Display(Name = "食材用量")]
            [Required(ErrorMessage = "食材用量不可空白!!")]
            public string qty { get; set; }
        }
    }
}