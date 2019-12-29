using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GutenbergProjectVBS.Web.Models
{
    public class Language : MyEntityBase
    {
        public Language()
        {
            this.Books = new HashSet<Book>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LanguageId { get; set; }

        [Required]
        [StringLength(250)]
        public string LanguageTitle { get; set; }

        [Required]
        [StringLength(4)]
        public string LanguageCode { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}