using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotesApp.Data;
using NotesApp.Models;

namespace NotesApp.Controllers
{
    public class NoteItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NoteItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NoteItems
        public async Task<IActionResult> Index()
        {
              return _context.NoteItem != null ? 
                          View(await _context.NoteItem.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.NoteItem'  is null.");
        }

        // GET: NoteItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NoteItem == null)
            {
                return NotFound();
            }

            var noteItem = await _context.NoteItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (noteItem == null)
            {
                return NotFound();
            }

            //noteItem.Description = Markdown.ToHtml(noteItem.Description);
            return View(noteItem);
        }

        // GET: NoteItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NoteItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteItem noteItem)
        {
            if (ModelState.IsValid)
            {
                noteItem.CreatedAt = DateTime.Now;
                noteItem.UpdatedAt = DateTime.Now;
                _context.Add(noteItem);
                await _context.SaveChangesAsync();
                return Redirect("/");
            }
            return View(noteItem);
        }

        // GET: NoteItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NoteItem == null)
            {
                return NotFound();
            }

            var noteItem = await _context.NoteItem.FindAsync(id);
            if (noteItem == null)
            {
                return NotFound();
            }
            return View(noteItem);
        }

        // POST: NoteItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NoteItem noteItem)
        {
            if (id != noteItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(noteItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteItemExists(noteItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/");
            }
            return View(noteItem);
        }

        // GET: NoteItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NoteItem == null)
            {
                return NotFound();
            }

            var noteItem = await _context.NoteItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (noteItem == null)
            {
                return NotFound();
            }

            return View(noteItem);
        }

        // POST: NoteItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NoteItem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.NoteItem'  is null.");
            }
            var noteItem = await _context.NoteItem.FindAsync(id);
            if (noteItem != null)
            {
                _context.NoteItem.Remove(noteItem);
            }
            
            await _context.SaveChangesAsync();
            return Redirect("/");
        }

        private bool NoteItemExists(int id)
        {
          return (_context.NoteItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
