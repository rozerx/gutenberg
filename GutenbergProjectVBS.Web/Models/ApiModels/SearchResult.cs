using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GutenbergProjectVBS.Web.Models.ApiModels
{
    public class SearchResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<AuthorResult> Authors { get; set; }
        public List<string> Subjects { get; set; }
        public List<string> Bookshelves { get; set; }
        public List<string> Languages { get; set; }
        public bool Copyright { get; set; }
        public string Media_type { get; set; }
        public Dictionary<string, string> Formats { get; set; }
        public int Download_count { get; set; }
    }
}