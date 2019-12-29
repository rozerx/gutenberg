using GutenbergProjectVBS.Web.Filters;
using GutenbergProjectVBS.Web.Models;
using GutenbergProjectVBS.Web.Models.Context;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using GutenbergProjectVBS.Web.Models.ViewModels;
using System;

namespace GutenbergProjectVBS.Web.Controllers
{
    [AuthFilterUserNull]
    public class MyLibraryController : Controller
    {
        // GET: MyLibrary
        public ActionResult Index()
        {
            LibraryViewModel myLib = new LibraryViewModel();

            using (VBSContext db = new VBSContext())
            {
                myLib.MySelectedBooks = db.Libraries
                            .Include(s => s.Book)
                            .Include(a => a.Book.Authors)
                            .Include(c => c.Book.Categories)
                            .Include(l => l.Book.Languages)
                            .AsQueryable()
                            .Where(x => x.UserId == CurrentSession.User.Id)
                            .Where(d => d.DeletedAt.Equals(null))
                            .ToList();

                myLib.MyDeletedSelectedBooks = db.Libraries
                            .Include(s => s.Book)
                            .Include(a => a.Book.Authors)
                            .Include(c => c.Book.Categories)
                            .Include(l => l.Book.Languages)
                            .AsQueryable()
                            .Where(x => x.UserId == CurrentSession.User.Id)
                            .Where(d => !d.DeletedAt.Equals(null))
                            .ToList();
            }
            return View(myLib);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int BookId, int EbookNo)
        {
            var userId = CurrentSession.User.Id;

            using (VBSContext db = new VBSContext())
            {
                Book getBook = db.Books.AsQueryable().Where(b => b.EbookNo == EbookNo).FirstOrDefault();
                var bookControl = db.Libraries.AsQueryable().Where(x => x.BookId == BookId && x.UserId == userId).FirstOrDefault();

                if(bookControl == null)
                {
                    Library newLibrary = new Library() {
                        BookId = BookId,
                        UserId = userId,
                        PageNumber = 0,
                        CreatedAt = System.DateTime.Now,
                        Book = getBook
                    };

                    List<Author> getAuthors = getBook.Authors.ToList();
                    List<Category> getCategories = getBook.Categories.ToList();
                    List<Language> getLanguages = getBook.Languages.ToList();

                    newLibrary.Book.Authors = getAuthors;
                    newLibrary.Book.Categories = getCategories;
                    newLibrary.Book.Languages = getLanguages;
                    db.Libraries.Add(newLibrary);
                    db.SaveChanges();
                    return RedirectToAction("Index", "MyLibrary");
                } else
                {
                    TempData.Add("EbookExistInLibrary", "This book has already exist in your library.");
                    return RedirectToAction("View", "Ebooks", new { id = EbookNo });
                }
            }
        }

        public ActionResult ReadBook(int? id)
        {
            ReadBookViewModel readBookViewModel = new ReadBookViewModel();
            using (VBSContext db = new VBSContext())
            {
                readBookViewModel.LibraryDetail = db.Libraries
                            .Include(s => s.Book)
                            .Include(a => a.Book.Authors)
                            .Include(c => c.Book.Categories)
                            .Include(l => l.Book.Languages)
                            .AsQueryable()
                            .FirstOrDefault(x => x.LibraryId == id);
            }
            return View(readBookViewModel);
        }

        public ActionResult SoftDelete(int? id)
        {
            var LibraryId = id;

            using (VBSContext db = new VBSContext())
            {
                try
                {
                    Library library = db.Libraries.AsQueryable().FirstOrDefault(x => x.LibraryId == id);
                    if (library != null)
                    {
                        db.Configuration.AutoDetectChangesEnabled = true;
                        db.ChangeTracker.Entries();
                        library.DeletedAt = System.DateTime.Now;
                        db.SaveChanges();
                        TempData.Add("BookSoftDeleteSuccess", "Book was deleted successfully.");
                    }
                } catch(Exception ex)
                {
                    TempData.Add("BookSoftDeleteError", "Book was not deleted successfully.");
                }
            }

            return RedirectToAction("Index", "MyLibrary");
        }

        public ActionResult Recovery(int? id)
        {
            var LibraryId = id;

            using (VBSContext db = new VBSContext())
            {
                try
                {
                    Library library = db.Libraries.AsQueryable().FirstOrDefault(x => x.LibraryId == id);
                    if (library != null)
                    {
                        db.Configuration.AutoDetectChangesEnabled = true;
                        db.ChangeTracker.Entries();
                        library.PageNumber = 0;
                        library.DeletedAt = null;
                        db.SaveChanges();
                    }
                    TempData.Add("BookRecoverySuccess", "Book was recovery successfully.");
                }
                catch (Exception ex)
                {
                    TempData.Add("BookRecoveryError", "Book was not recovery successfully.");
                }
            }

            return RedirectToAction("Index", "MyLibrary");
        }

        public ActionResult Delete(int? id)
        {
            var LibraryId = id;

            using (VBSContext db = new VBSContext())
            {
                try
                {
                    Library library = db.Libraries.AsQueryable().FirstOrDefault(x => x.LibraryId == id);
                    db.Libraries.Remove(library);
                    db.SaveChanges();
                    TempData.Add("BookDeleteSuccess", "Book was deleted successfully.");
                }
                catch (Exception ex)
                {
                    TempData.Add("BookDeleteError", "Book was not deleted successfully.");
                }
            }

            return RedirectToAction("Index", "MyLibrary");
        }
    }
}