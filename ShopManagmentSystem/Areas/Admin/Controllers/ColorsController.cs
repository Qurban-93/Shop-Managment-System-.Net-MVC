using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class ColorsController : Controller
    {
        private readonly AppDbContext _context;

        public ColorsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() { return View(await _context.Colors.ToListAsync()); }
        public IActionResult Create() { return View(); }      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColorVM color)
        {
            if (!ModelState.IsValid) return View();
            if (await _context.Colors.AnyAsync(c => c.ColorName.Trim().ToLower() == color.ColorName.Trim().ToLower()))
            {
                ModelState.AddModelError("ColorName", "Bu adla color movcuddur !");
                return View();

            }
            Color newColor = new()
            {
                ColorName= color.ColorName,
                CreateDate = DateTime.Now,
            };


            _context.Colors.Add(newColor);
            await _context.SaveChangesAsync();
            TempData["Create"] = true;
            return RedirectToAction(nameof(Index));
        } 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
         
            Color? color = await _context.Colors.FindAsync(id);
            if (color == null) return NotFound();
            ColorVM existcolor = new()
            {
                ColorName= color.ColorName,
            };
            
            return View(existcolor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,ColorVM color)
        {
            if (id == null || id == 0) return NotFound();
            Color? existcolor = await _context.Colors.FindAsync(id);
            if (existcolor == null) return NotFound();
            if (!ModelState.IsValid) return View();
            if (await _context.Colors.AnyAsync(c => c.ColorName.Trim().ToLower() == color.ColorName.Trim().ToLower() && c.Id != id))
            {
                ModelState.AddModelError("ColorName", "Bu adla color movcuddur !");
                return View();
            }
            existcolor.UpdateDate = DateTime.Now;
            existcolor.ColorName= color.ColorName;

            await _context.SaveChangesAsync();
            TempData["Edit"] = true;

            return RedirectToAction("Index");
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
             Color? color = await _context.Colors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (color == null) return NotFound();
            if (color.Products.Count > 0) return BadRequest("Color Have Products");
            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();

            return Ok(color);
        }
      
    }
}
