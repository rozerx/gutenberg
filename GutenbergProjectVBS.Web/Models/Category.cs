using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GutenbergProjectVBS.Web.Models
{
    public class Category : MyEntityBase
    {
        public Category()
        {
            this.Books = new HashSet<Book>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(250)]
        public string CategoryTitle { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}