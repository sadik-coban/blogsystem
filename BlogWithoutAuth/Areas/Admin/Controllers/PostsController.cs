using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogWithoutAuth.DataAccess;
using BlogWithoutAuth.Models;
using System.Diagnostics;

namespace BlogWithoutAuth.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly AppDbContext _context;

        public PostsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Posts
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Posts.Include(p => p.Author).Include(p => p.Category);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/Posts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Admin/Posts/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["Tags"] = new SelectList(_context.Tags,"Id", "Name");
            ViewData["TagList"] = _context.Tags.ToList();
            return View();
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AuthorId,CategoryId,TagString,Title,Content")] Post post)
        {
            post.Id = Guid.NewGuid();
            //Guid mainGuid = Guid.Parse("18930D8B-88AA-4FCE-02CB-08DBCB2F2C93");
            //Guid mainGuid2 = Guid.Parse("FF7DA7E2-DCCE-48E8-B2A2-FC8421CBBB41");
            //Debug.WriteLine(post.TagString.Length);
            List<Tag> tagsToDb = new List<Tag>();
            foreach (var tagId in post.TagString)
            {
                tagsToDb.Add(_context.Tags.Find(Guid.Parse(tagId)));
            }
            post.Tags = tagsToDb;
            
            _context.Add(post);
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Posts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", post.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
            ViewData["TagList"] = _context.Tags.ToList();
            return View(post);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,AuthorId,CategoryId,TagString,Title,Content")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }
            try
            {
                List<Tag> tagsToDb = new List<Tag>();
                foreach (var tagId in post.TagString)
                {
                    Debug.WriteLine("I am working");
                    tagsToDb.Add(_context.Tags.Find(Guid.Parse(tagId)));
                }
                _context.Database.ExecuteSql($"DELETE FROM dbo.PostTag WHERE PostsId = {id}");
                post.Tags = tagsToDb;
                //_context.Attach(post);
                _context.Update(post);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Admin/Posts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Admin/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'AppDbContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(Guid id)
        {
            return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
