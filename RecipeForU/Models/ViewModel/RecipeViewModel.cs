using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RecipeForU.Models;

public class RecipeViewModel
{
    [Display(Name = "動作名稱")]
    public string ActionNo { get; set; }
    [Display(Name = "明細選取用RowID")]
    public string RowID { get; set; }
    [Display(Name = "明細選取的步驟編號")]
    public string StepNo { get; set; }
    [Display(Name = "食譜ID")]
    public string RecipeID { get; set; }
    [Display(Name = "食譜名稱")]
    public string RecipeName { get; set; }
    [Display(Name = "食譜介紹")]
    public string RecipeIntro { get; set; }
    public eRECIPE RecipeElement { get; set; }
    public sRECIPE RecipeStep { get; set; }
}
