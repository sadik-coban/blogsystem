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
using BlogWithoutAuth.Models.ViewModels;

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
            var appDbContext = _context.Posts.Include(p => p.Author).Include(p => p.Category).Include(p=>p.Tags);
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
            ViewData["Tags"] = new SelectList(_context.Tags, "Id", "Name");
            return View();
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AuthorId,CategoryId,TagIds,Title,Content")] Post post)
        {
            post.Tags = await _context.Tags.Where(tagDB => post.TagIds.Any(tagSelected => tagSelected == tagDB.Id)).ToListAsync();
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

            //Post? post = await _context.Posts.Include(post => post.Tags).FirstOrDefaultAsync(post => post.Id == id);
            Post post = await _context.Posts.FindAsync(id);
            //Debug.WriteLine(post.Tags.Count);
            if (post == null)
            {
                return NotFound();
            }
            //Guid[] tagIds = await _context.Posts.Where(post => post.Id == id).SelectMany(post => post.PostTags.Select(postTag => postTag.TagsId)).ToArrayAsync();

            //post.Tags.Remove(post.Tags.Select(tag => tag).ToArray()[0]);
            _context.SaveChanges();
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", post.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
            ViewData["Tags"] = new MultiSelectList(_context.Tags, "Id", "Name", post.Tags.Select(tag=> tag.Id).ToArray());
            return View(post);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,AuthorId,CategoryId,TagIds,Title,Content")] Post post)
        {
            
            if (id != post.Id)
            {
                return NotFound();
            }
            try
            {

                Post? post_joined = await _context.Posts.Include(post => post.Tags).FirstOrDefaultAsync(post => post.Id == id);
                Debug.WriteLine(post_joined.Tags.Count);
                foreach(var tag in post_joined.Tags.ToList())
                {
                    post_joined.Tags.Remove(tag);
                }
                await _context.SaveChangesAsync();
                Debug.WriteLine("anger loads!");
                _context.Entry(post_joined).State = EntityState.Detached;
                //post.Tags.RemoveRange(0, post.Tags.Count);
                //await _context.Posts.Where(post => post.Id == id).SelectMany(post => post.PostTags).ExecuteDeleteAsync();
                //_context.Posts.SingleOrDefault(post => post.Id == id).Tags.AsQueryable()
                post.Tags = await _context.Tags.Where(tagDB => post.TagIds.Any(tagSelected => tagSelected == tagDB.Id)).ToListAsync();
                
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
