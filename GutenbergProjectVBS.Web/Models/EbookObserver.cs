using GutenbergProjectVBS.Web.Models.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using VersOne.Epub;

namespace GutenbergProjectVBS.Web.Models
{
    public class EbookObserver : Observer
    {
        private int EbookNo;
        public EbookObserver(int EbookNo)
        {
            this.EbookNo = EbookNo;
        }

        public override void Update()
        {
            Thread.Sleep(3000);
            System.Diagnostics.Debug.WriteLine(string.Format("kitap ebook ekleme başlıyor.... ID : {0}", this.EbookNo));
            try
            {
                string path = string.Format("~/Files/{0}/epub_{0}.epub", this.EbookNo);
                using (var fs = new FileStream(HostingEnvironment.MapPath(path), FileMode.Open))
                {
                    EpubBook epubBook = EpubReader.ReadBook(fs);
                    foreach (EpubTextContentFile textContentFile in epubBook.ReadingOrder)
                    {
                        // HTML of current text content file
                        string htmlContent = textContentFile.Content;
                        HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                        htmlDocument.LoadHtml(htmlContent);
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(htmlDocument.DocumentNode.InnerText);

                        // Insert to db
                        using (VBSContext db = new VBSContext())
                        {
                            Book getBook = db.Books.AsQueryable().Where(x => x.EbookNo == EbookNo).FirstOrDefault();
                            BookPage bp = new BookPage();
                            bp.Books.Add(getBook);
                            bp.Description = stringBuilder.ToString();
                            db.BookPages.Add(bp);
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);
            }
        }
    }
}