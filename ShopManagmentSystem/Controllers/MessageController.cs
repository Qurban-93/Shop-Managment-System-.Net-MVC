﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Hubs;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;


namespace ShopManagmentSystem.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly AppDbContext _context;

        public MessageController(UserManager<AppUser> userManager, IHubContext<ChatHub> hubContext, AppDbContext context)
        {
            _userManager = userManager;
            _hubContext = hubContext;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            AppUser userMe = await _userManager.FindByNameAsync(User.Identity.Name);
            if (userMe == null) return BadRequest();
            
            MessageIndexVM indexVM = new()
            {
                Users = _userManager.Users.Where(u => u.UserName != User.Identity.Name).ToList(),
                NewMessages = _context.Messages.Where(m=>!m.IsRead && m.DestinationId == userMe.Id).ToList(),
              
            };


            return View(indexVM);
        }

        [HttpPost]
        public async Task<IActionResult> ChatHistory(string Id, int skip)
        {
            if (skip == 0) { skip = 10; }
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            AppUser? secondUSer = await _userManager.FindByIdAsync(Id);
            if (secondUSer == null || user == null) return BadRequest();
            if (Id == null || user == null) return NotFound();
            int allMessagesCount = _context.Messages.Where(m => (m.DestinationId == Id && m.SenderId == user.Id) ||
            (m.DestinationId == user.Id && m.SenderId == Id)).Count();
            if (allMessagesCount < skip) { skip = allMessagesCount; }
            List<Message> messages = _context.Messages.Where(m => (m.DestinationId == Id && m.SenderId == user.Id) ||
            (m.DestinationId == user.Id && m.SenderId == Id)).Skip(allMessagesCount - skip).ToList();
            List<int> ids = new();
            foreach (var message in messages)
            {
                if (message.IsRead == false)
                {
                    ids.Add(message.Id);
                    if (user.Id == message.DestinationId)
                    {
                        message.IsRead = true;
                        _context.SaveChanges();

                    }
                }
            }

            MessageVM messageVM = new()
            {
                Messages = messages,
                User = user,
                AllMessagesCount = allMessagesCount,
                UnreadIds = ids,
                LastSeen = secondUSer.LastSeen,
            };

            
            return PartialView("_ChatHistoryPartialView", messageVM);
        }
    }
}
