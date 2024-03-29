﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "MainAdmin")]
    public class BranchController : Controller
    {
        private readonly AppDbContext _context;

        public BranchController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Branches.Where(b=>b.Id != 5 && !b.IsDeleted).
                ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(BranchVM branchVM)
        {
            if(!ModelState.IsValid) return View(branchVM);
            if(await _context.Branches.AnyAsync(b=>b.Name.ToLower().Trim()==branchVM.Name.ToLower().Trim() && !b.IsDeleted)) 
            {
                ModelState.AddModelError("Name", "Bu adla Branch movcuddur !");
                return View(branchVM);
            }
            Branch branch = new() { Name = branchVM.Name };
            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();
            TempData["Success"] = true;
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null || id == 0) return NotFound();
            Branch? branch = await _context.Branches.FirstOrDefaultAsync(b=>b.Id == id && !b.IsDeleted);
            if(branch == null) return NotFound();
            BranchVM branchVM = new() { Name = branch.Name };
            return View(branchVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id, BranchVM branchVM)
        {
            if (!ModelState.IsValid || id == null || id == 0) return NotFound();
            Branch? branch = await _context.Branches.FirstOrDefaultAsync(a=>a.Id == id && !a.IsDeleted);
            if(branch == null) return NotFound();
            if(await _context.Branches.AnyAsync(b=>b.Name.Trim().ToLower() == branch.Name.Trim().ToLower() && b.Id != id && !b.IsDeleted))
            {
                ModelState.AddModelError("Name", "Bu adla Branch movcuddur !");
                return View(branchVM);
            }
            branch.Name = branchVM.Name;
            TempData["Edit"] = true;
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null || id == 0)  return NotFound(); 
            Branch? existBranc = await _context.Branches.FirstOrDefaultAsync(b=>b.Id == id);
            if(existBranc == null) return NotFound();
            existBranc.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Ok(existBranc.Name);
        }
    }
}
