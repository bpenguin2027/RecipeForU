using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipeForU.Models
{
    [MetadataType(typeof(RECIPEMetaData))]
    public partial class RECIPE
    {
        private class RECIPEMetaData
        {
            public int rowid { get; set; }
            [Display(Name = "食譜ID")]
            public string recipe_id { get; set; }
            [Display(Name = "食譜名稱")]
            public string recipe_name { get; set; }
            [Display(Name = "食譜作者")]
            public string recipe_author { get; set; }
            [Display(Name = "建立日期")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public System.DateTime time { get; set; }
            [Display(Name = "觀看次數")]
            public int view_times { get; set; }
            [Display(Name = "食譜簡介")]
            public string recipe_intro { get; set; }
            [Display(Name = "封面圖片")]
            public string recipe_cover { get; set; }
            [Display(Name = "推薦食譜")]
            public bool recommended { get; set; }
        }
    }
}