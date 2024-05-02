using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Areas.Manage.ViewModels;
using Pustok.Models;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("manage")]
    public class BookController : Controller
    {
        private readonly PustokDbContext _context;

        public BookController(PustokDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page=1)
        {
            var query = _context.Books.Include(x => x.Author).Include(x => x.Genre).Include(x=>x.BookTags).Include(x => x.BookImages.Where(x => x.Status == true)).OrderByDescending(x => x.Id);
            
            return View(PaginatedList<Book>.Create(query,page,2));
        }
        public IActionResult Create()
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.BookTags.Include(x=>x.Tag).ToList();    

            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Authors = _context.Authors.ToList();
                ViewBag.Genres = _context.Genres.ToList();
                ViewBag.BookTags = _context.BookTags.ToList();
                ViewBag.Tags = _context.BookTags.Include(x => x.Tag).ToList();

                return View(book);
            }

            if (!_context.Authors.Any(x => x.Id == book.AuthorId))
                return RedirectToAction("notfound", "error");

            if (!_context.Genres.Any(x => x.Id == book.GenreId))
                return RedirectToAction("notfound", "error");

            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Book book = _context.Books.Find(id);

            if(book == null) return RedirectToAction("notfound", "error");


            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            Book? existBook = _context.Books.Find(book.Id);
            if (existBook == null) return RedirectToAction("notfound", "error");


            if (book.AuthorId!=existBook.AuthorId && !_context.Authors.Any(x => x.Id == book.AuthorId))
                return RedirectToAction("notfound", "error");

            if (book.GenreId != existBook.GenreId && !_context.Genres.Any(x => x.Id == book.GenreId))
                return RedirectToAction("notfound", "error");

            existBook.Name = book.Name;
            existBook.Desc = book.Desc;
            existBook.SalePrice = book.SalePrice;
            existBook.CostPrice = book.CostPrice;
            existBook.DiscountPercent = book.DiscountPercent;
            existBook.IsNew = book.IsNew;   
            existBook.IsFeatured = book.IsFeatured;
            existBook.StockStatus = book.StockStatus;

            existBook.ModifiedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
