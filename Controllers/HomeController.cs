using AspNetCoreHero.ToastNotification.Abstractions;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Data;
using NotesApp.Models;
using System.Diagnostics;

namespace NotesApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext? Context { get; }
        private readonly UserManager<IdentityUser>? _userManager;
        private readonly INotyfService _notyf;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext? applicationDbContext, UserManager<IdentityUser>? userManager, INotyfService notyf)
        {
            Context = applicationDbContext;
            _logger = logger;
            _userManager = userManager;
            _notyf = notyf;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(User);
            List<NoteItem> NoteItems = Context.NoteItem.Where(NoteItem => NoteItem.UserId == userId).ToList();
            NoteItems = NoteItems.OrderByDescending(NoteItem => NoteItem.CreatedAt).ToList();
            foreach(var noteItem in NoteItems)
            {
                if (noteItem.Title.Length > 16)
                    noteItem.Title = noteItem.Title.Substring(0, 15);
                noteItem.Description = Markdown.ToPlainText(noteItem.Description);
                if (noteItem.Description.Length > 56)
                    noteItem.Description = noteItem.Description.Substring(0, 55);
            }
            return View(NoteItems);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Search(string query)
        {
            string userId = _userManager.GetUserId(User);
            List<NoteItem> NoteItems = Context.NoteItem.Where(NoteItem => NoteItem.UserId == userId).ToList();
            NoteItems = NoteItems.Where(NoteItem => NoteItem.Title.ToLower().Contains(query.ToLower()) || NoteItem.Description.ToLower().Contains(query.ToLower())).ToList();
            foreach (var noteItem in NoteItems)
            {
                if (noteItem.Title.Length > 16)
                    noteItem.Title = noteItem.Title.Substring(0, 15);
                noteItem.Description = Markdown.ToPlainText(noteItem.Description);
                if (noteItem.Description.Length > 56)
                    noteItem.Description = noteItem.Description.Substring(0, 55);
            }
            return View("Index", NoteItems);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}