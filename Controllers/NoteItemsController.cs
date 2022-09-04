using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser>? _userManager;
        private readonly INotyfService _notyf;
        public NoteItemsController(ApplicationDbContext context, UserManager<IdentityUser>? userManager, INotyfService notyf)
        {
            _context = context;
            _userManager = userManager;
            _notyf = notyf;
        }

        // GET: NoteItems
        [Authorize]
        public async Task<IActionResult> Index()
        {
              return _context.NoteItem != null ? 
                          View(await _context.NoteItem.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.NoteItem'  is null.");
        }

        // GET: NoteItems/Details/5
        [Authorize]
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
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: NoteItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(NoteItem noteItem, IFormFile Image)
        {
            /*if (ModelState.IsValid)
            {*/
                noteItem.UserId = _userManager.GetUserId(User);
                noteItem.CreatedAt = DateTime.Now;
                noteItem.UpdatedAt = DateTime.Now;
                if (Image != null)
                {
                    string uploadPath = Path.Combine(".\\wwwroot", "Images");
                    string fileName = Path.GetRandomFileName();
                    string filePath = Path.Combine(uploadPath, Image.FileName);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        Image.CopyTo(fileStream);
                    }
                    FileInfo fileInfo = new FileInfo(filePath);
                    var fileExtension = fileInfo.Extension;
                    fileName = fileName.Substring(0, fileName.IndexOf("."));
                    fileName = fileName + fileExtension;
                    if (fileInfo.Exists)
                    {
                        fileInfo.MoveTo(Path.Combine(uploadPath, fileName));
                    }
                    noteItem.Image = fileName;
                }
                _context.Add(noteItem);
                await _context.SaveChangesAsync();
                _notyf.Success("Note added successfully.");
                return Redirect("/");
            /*}
            return View(noteItem);*/
        }

        // GET: NoteItems/Edit/5
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Edit(NoteItem item, IFormFile Image)
        {
            /*if (id != noteItem.Id)
            {
                return NotFound();
            }*/

            NoteItem noteItem = _context.NoteItem.Find(item.Id);

            /*if (ModelState.IsValid)
            {*/
                try
                {
                    noteItem.Title = item.Title;
                    noteItem.Description = item.Description;
                    noteItem.UpdatedAt = DateTime.Now;
                    if(Image != null)
                    {
                        string uploadPath = Path.Combine(".\\wwwroot", "Images");
                        string fileName = Path.GetRandomFileName();
                        string filePath = Path.Combine(uploadPath, Image.FileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            Image.CopyTo(fileStream);
                        }
                        FileInfo fileInfo = new FileInfo(filePath);
                        var fileExtension = fileInfo.Extension;
                        fileName = fileName.Substring(0, fileName.IndexOf("."));
                        fileName = fileName + fileExtension;
                        if (fileInfo.Exists)
                        {
                            fileInfo.MoveTo(Path.Combine(uploadPath, fileName));
                        }
                        if (noteItem.Image != null)
                        {
                            string deletePath = Path.Combine(".\\wwwroot", "Images");
                            string fileDeletePath = Path.Combine(deletePath, noteItem.Image);
                            FileInfo deleteFile = new FileInfo(fileDeletePath);
                            if (deleteFile.Exists)
                            {
                                deleteFile.Delete();
                            }
                        }
                        noteItem.Image = fileName;
                    }
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
                _notyf.Success("Note edited successfully.");
                return Redirect("/");
            /*}
            return View(noteItem);*/
        }

        // GET: NoteItems/Delete/5
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NoteItem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.NoteItem'  is null.");
            }
            var noteItem = await _context.NoteItem.FindAsync(id);
            if (noteItem != null)
            {
                if (noteItem.Image != null)
                {
                    string deletePath = Path.Combine(".\\wwwroot", "Images");
                    string fileDeletePath = Path.Combine(deletePath, noteItem.Image);
                    FileInfo deleteFile = new FileInfo(fileDeletePath);
                    if (deleteFile.Exists)
                    {
                        deleteFile.Delete();
                    }
                }
                _context.NoteItem.Remove(noteItem);
            }
            
            await _context.SaveChangesAsync();
            _notyf.Success("Note deleted successfully.");
            return Redirect("/");
        }

        private bool NoteItemExists(int id)
        {
          return (_context.NoteItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
