﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApp.Data;
using NotesApp.Models;
using PagedList;
using System.Diagnostics;

namespace NotesApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext? Context { get; }
        private readonly INotyfService _notyf;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext? applicationDbContext, INotyfService notyf)
        {
            Context = applicationDbContext;
            _logger = logger;
            _notyf = notyf;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index(int? page)
        {
            string userId = Convert.ToString(Context.NoteUser.Where(NoteUser => NoteUser.UserName == User.Identity.Name).First().Id);
            List<NoteItem> NoteItems = Context.NoteItem.Where(NoteItem => Convert.ToString(NoteItem.UserId) == userId).ToList();
            NoteItems = NoteItems.OrderByDescending(NoteItem => NoteItem.CreatedAt).ToList();
            foreach(var noteItem in NoteItems)
            {
                if (noteItem.Title.Length > 16)
                    noteItem.Title = noteItem.Title.Substring(0, 15);
                noteItem.Description = Markdown.ToPlainText(noteItem.Description);
                if (noteItem.Description.Length > 56)
                    noteItem.Description = noteItem.Description.Substring(0, 55);
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(NoteItems.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Search(string query)
        {
            string userId = Convert.ToString(Context.NoteUser.Where(NoteUser => NoteUser.UserName == User.Identity.Name).First().Id);
            List<NoteItem> NoteItems = Context.NoteItem.Where(NoteItem => Convert.ToString(NoteItem.UserId) == userId).ToList();
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