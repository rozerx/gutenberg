using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GutenbergProjectVBS.Web.Models
{
    public class Author : MyEntityBase
    {
        public Author()
        {
            this.Books = new HashSet<Book>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId { get; set; }

        [Required]
        [StringLength(150)]
        public string AuthorName { get; set; }

        public Nullable<int> BirthYear { get; set; }
        public Nullable<int> DeathYear { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}