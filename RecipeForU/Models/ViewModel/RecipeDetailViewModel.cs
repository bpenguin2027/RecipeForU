using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeForU.Models.ViewModel
{
    public class RecipeDetailViewModel
    {
        public RECIPE RECIPE { set; get; }
        public List<sRECIPE> sRECIPE { set; get; }
        public List<eRECIPE> eRECIPE { set; get; }
        public string Author { get; set; }
        public bool Favored { set; get; }
    }
}