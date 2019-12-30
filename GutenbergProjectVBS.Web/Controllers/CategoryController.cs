using GutenbergProjectVBS.Web.Models;
using GutenbergProjectVBS.Web.Models.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GutenbergProjectVBS.Web.Controllers
{
    public class CategoryController : Controller
    {
        private VBSContext context = new VBSContext();

        // GET: Category
        public ActionResult Index()
        {
            IQueryable<Category> categories = context.Categories.AsQueryable().Where(x => x.DeletedAt == null);

            return View(categories);
        }

        public ActionResult Detail(int? categotyID)
        {
            var books = context.Books
                .Where(b => b.Categories
                .Any(c => c.CategoryId == categotyID));

            return View(books);
        }
    }
}