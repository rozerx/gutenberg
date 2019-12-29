using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GutenbergProjectVBS.Web.Models
{
    public class Library : MyEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibraryId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int PageNumber { get; set; }
        public int OrderBook { get; set; }
        public virtual Book Book { get; set; }
    }
}