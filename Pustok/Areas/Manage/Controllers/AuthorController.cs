using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Areas.Manage.ViewModels;
using Pustok.Models;

namespace Pustok.Areas.Manage.Controllers
{
    
        [Area("manage")]
        public class AuthorController : Controller
        {
            private readonly PustokDbContext _context;

            public AuthorController(PustokDbContext context)
            {
                _context = context;
            }
            public IActionResult Index(int page = 1)
            {
                var query = _context.Authors;
                return View(PaginatedList<Author>.Create(query, page, 2));
            }

            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public IActionResult Create(Author author)
            {
                if (!ModelState.IsValid)
                {
                    return View(author);
                }

                if (_context.Authors.Any(x => x.Fullname == author.Fullname))
                {
                    ModelState.AddModelError("Name", "Genre already exists!");
                    return View(author);
                }

                _context.Authors.Add(author);
                _context.SaveChanges();

                return RedirectToAction("index");
            }

            public IActionResult Edit(int id)
            {
                Author author = _context.Authors.Find(id);

                if (author == null) return RedirectToAction("Error", "NotFound");

                return View(author);
            }

            [HttpPost]
            public IActionResult Edit(Author author)
            {
                if (!ModelState.IsValid)
                {
                    return View(author);
                }

                Author existAuthor = _context.Authors.Find(author.Id);

                if (existAuthor == null) return RedirectToAction("Error", "NotFound");

                if (_context.Authors.Any(x => x.Fullname == author.Fullname))
                {
                    ModelState.AddModelError("Name", "Genre already exists!");
                    return View(author);
                }

                existAuthor.Fullname = author.Fullname;
                _context.SaveChanges();

                return RedirectToAction("index");
            }
        }
    }



