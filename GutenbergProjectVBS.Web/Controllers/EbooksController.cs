using GutenbergProjectVBS.Web.Models;
using GutenbergProjectVBS.Web.Models.ApiModels;
using GutenbergProjectVBS.Web.Models.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Mvc;
using VersOne.Epub;

namespace GutenbergProjectVBS.Web.Controllers
{
    public class EbooksController : Controller
    {
        private VBSContext context = new VBSContext();

        // GET: Ebooks
        [HttpGet]
        public ActionResult View(int? id)
        {
            string path = string.Format("~/Files/{0}/epub_{0}.epub", 37295);
            EpubBook epubBook = EpubReader.ReadBook(Server.MapPath(path));

            Session.Add("RedirectToBack", HttpContext.Request.Url);

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Book book = context.Books.AsQueryable().Where(b => b.EbookNo == id).FirstOrDefault();
            // Book Not Found and Create Book
            if (book == null)
            {
                book = new Book();
                // Files
                string imageUrl = string.Format("http://www.gutenberg.org/files/{0}/{0}-h/images/cover.jpg", id);
                string epubUrl = string.Format("http://www.gutenberg.org/ebooks/{0}.epub.images", id);
                book.FileName = imageUrl;
                // Get Search Api Book
                SearchApi searchApi = new SearchApi();
                SearchResult searchResult = searchApi.bookDetail(Convert.ToInt32(id));

                // Book Save Thread
                Thread BookSaveThread = new Thread(new ThreadStart(delegate() {
                    BookSaveThreadMethod(searchResult);
                }));
                
                BookSaveThread.Start();

                // Book Class Synchronization
                book.EbookNo = searchResult.Id;
                book.BookTitle = searchResult.Title;
                book.Price = 0;
                book.MediaType = searchResult.Media_type;
                book.CopyrightStatus = searchResult.Copyright;
                // Set Author
                foreach (AuthorResult ar in searchResult.Authors)
                {
                    book.Authors.Add(new Author() { AuthorName = ar.Name, BirthYear = ar.Birth_year, DeathYear = ar.Death_year });
                }
                // Set Language
                foreach (var l in searchResult.Languages)
                {
                    book.Languages.Add(new Language() { LanguageTitle = GVBSHelpers.GVBSHelperClass.ConvertLanguage(l), LanguageCode = l });
                }
                // Set Category
                foreach (var s in searchResult.Subjects)
                {
                    book.Categories.Add(new Category() { CategoryTitle = s });
                }
            }

            return View(book);
        }

        /// <summary>
        /// Author Thread Method
        /// </summary>
        /// <param name="authorResults"></param>
        private void BookSaveThreadMethod(SearchResult book)
        {
            try
            {
                HashSet<Author> authors = new HashSet<Author>();
                HashSet<Category> categories = new HashSet<Category>();
                HashSet<Language> languages = new HashSet<Language>();
                // Download Files
                if (!Directory.Exists(Server.MapPath("~/Files/" + book.Id)))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Files/" + book.Id));
                }
                string imageUrl = string.Format("http://www.gutenberg.org/files/{0}/{0}-h/images/cover.jpg", book.Id);
                string epubUrl = string.Format("http://www.gutenberg.org/ebooks/{0}.epub.images", book.Id);
                string newImageUrl = null;
                string newEpubUrl = null;
                // Download Image
                try
                {
                    WebClient wcImage = new WebClient();
                    wcImage.DownloadFile(imageUrl, Server.MapPath("~/Files/" + book.Id + "/" + ("cover_" + book.Id + ".jpg")));
                    newImageUrl = "/Files/" + book.Id + "/" + ("cover_" + book.Id + ".jpg");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("############# book image error #############" + ex.Message);
                }
                // Download Epub
                try
                {
                    WebClient wcEpub = new WebClient();
                    wcEpub.DownloadFile(epubUrl, Server.MapPath("~/Files/" + book.Id + "/" + ("epub_" + book.Id + ".epub")));
                    newEpubUrl = "/Files/" + book.Id + "/" + ("epub_" + book.Id + ".epub");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("############# book epub error #############" + ex.Message);
                }
                // Book Save
                using (VBSContext db = new VBSContext())
                {
                    // Authors
                    foreach (AuthorResult ar in book.Authors)
                    {
                        Author author = db.Authors.AsQueryable().Where(a => a.AuthorName == ar.Name).FirstOrDefault();
                        if (author == null)
                        {
                            Author newAuthor = new Author()
                            {
                                AuthorName = ar.Name,
                                BirthYear = ar.Birth_year,
                                DeathYear = ar.Death_year,
                                CreatedAt = DateTime.Now
                            };
                            db.Authors.Add(newAuthor);
                            authors.Add(newAuthor);
                        }
                        else
                        {
                            authors.Add(author);
                        }
                    }
                    // Categories
                    foreach (var c in book.Subjects)
                    {
                        Category category = db.Categories.AsQueryable().Where(mc => mc.CategoryTitle == c).FirstOrDefault();
                        if (category == null)
                        {
                            Category newCategory = new Category()
                            {
                                CategoryTitle = c,
                                CreatedAt = DateTime.Now
                            };
                            db.Categories.Add(newCategory);
                            categories.Add(newCategory);
                        }
                        else
                        {
                            categories.Add(category);
                        }
                    }
                    // Languages
                    foreach (var lang in book.Languages)
                    {
                        Language language = db.Languages.AsQueryable().Where(ml => ml.LanguageCode == lang).FirstOrDefault();
                        if (language == null)
                        {
                            Language newLanguage = new Language()
                            {
                                LanguageTitle = GVBSHelpers.GVBSHelperClass.ConvertLanguage(lang),
                                LanguageCode = lang,
                                CreatedAt = DateTime.Now
                            };
                            db.Languages.Add(newLanguage);
                            languages.Add(newLanguage);
                        }
                        else
                        {
                            languages.Add(language);
                        }
                    }
                    // new book
                    Book newBook = new Book()
                    {
                        BookTitle = book.Title,
                        EbookNo = book.Id,
                        BookContents = "",
                        FileName = newImageUrl,
                        FileEpub = newEpubUrl,
                        MediaType = book.Media_type,
                        DownloadCount = book.Download_count,
                        Price = 0,
                        CopyrightStatus = book.Copyright,
                        Authors = authors,
                        Categories = categories,
                        Languages = languages,
                        CreatedAt = DateTime.Now
                    };
                    newBook.Attach(new EbookObserver(book.Id));
                    newBook.Notify();
                    db.Books.Add(newBook);
                    db.SaveChanges();
                }
            } catch(DbEntityValidationException ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("############# book error : {0} #############", ex.Message));
            } catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("############# book error : {0} #############", ex.Message));
            }
        }
    }
}