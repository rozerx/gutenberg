using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GutenbergProjectVBS.Web.Models
{
    public class Book : MyEntityBase
    {
        private Observer _observer;

        public Book()
        {
            this.Authors = new HashSet<Author>();
            this.Categories = new HashSet<Category>();
            this.Languages = new HashSet<Language>();
            this.BookPages = new HashSet<BookPage>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }

        [Required]
        [DisplayName("E-book No")]
        public int EbookNo { get; set; }

        [Required]
        [StringLength(250)]
        public string BookTitle { get; set; }

        [StringLength(500)]
        public string BookContents { get; set; }

        [StringLength(250)]
        public string FileName { get; set; }

        [StringLength(250)]
        public string FileEpub { get; set; }

        [DisplayName("Copyright Status")]
        public bool CopyrightStatus { get; set; }

        public string MediaType { get; set; }

        public decimal? Price { get; set; }

        public int DownloadCount { get; set; }

        // Relations
        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Language> Languages { get; set; }
        public virtual ICollection<BookPage> BookPages { get; set; }

        public void Notify()
        {
            _observer.Update();
        }

        public void Attach(Observer observer)
        {
            _observer = observer;
        }
    }
}