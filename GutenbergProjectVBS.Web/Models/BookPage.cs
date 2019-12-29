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
        public BookPage()
        {
            this.Books = new HashSet<Book>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookPageId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        // Relations
        public virtual ICollection<Book> Books { get; set; }
    }
}