using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GutenbergProjectVBS.Web.Models
{
    public class BookPage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookPageId { get; set; }

        public int BookId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string HtmlContent { get; set; }

        // Relations
        public virtual Book Books { get; set; }
    }
}