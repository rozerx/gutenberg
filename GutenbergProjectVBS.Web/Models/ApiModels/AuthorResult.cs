using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GutenbergProjectVBS.Web.Models.ApiModels
{
    public class AuthorResult
    {
        public string Name { get; set; }
        public Nullable<int> Birth_year { get; set; }
        public Nullable<int> Death_year { get; set; }
    }
}