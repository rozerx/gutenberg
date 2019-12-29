using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GutenbergProjectVBS.Web.Models.ViewModels
{
    public class LibraryViewModel
    {
        public LibraryViewModel()
        {
            MySelectedBooks = new List<Library>();
            MyDeletedSelectedBooks = new List<Library>();
        }

        public ICollection<Library> MySelectedBooks { get; set; }
        public ICollection<Library> MyDeletedSelectedBooks { get; set; }
    }
}