using GutenbergProjectVBS.Web.Models;
using GutenbergProjectVBS.Web.Models.ApiModels;
using GutenbergProjectVBS.Web.Models.Context;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GutenbergProjectVBS.Web.Controllers
{
    public class HomeController : Controller
    {
        private VBSContext context = new VBSContext();
        public ActionResult Index()
        {
            List <Book> books = context.Books.AsQueryable().Where(x => x.DeletedAt == null).OrderByDescending(o => o.BookId).Take(10).ToList();
            return View(books);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string search)
        {
            SearchApi api = new SearchApi();
            List<SearchResult> books = api.searchData(search);
            ViewBag.searchData = search;
            return View(books);
        }
    }
}