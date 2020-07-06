using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Models;
using Microsoft.AspNetCore.Mvc;

namespace GraniteHouse.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SpecialTagsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.SpecialTags.ToList());
        }

        //Get Create for Special Tags
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SpecialTags specialTags)
        {
            if(ModelState.IsValid)
            {
                _db.Add(specialTags);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialTags);
        }

        //Get Edit for Special Tags
        public async Task<IActionResult> Edit(int? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            var specialTags = await _db.SpecialTags.FindAsync(Id);
            if(specialTags == null)
            {
                return NotFound();
            }
            return View(specialTags);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, SpecialTags specialTags)
        {
            if(Id != specialTags.Id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                _db.Update(specialTags);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialTags);
        }

        //Get Details for Special Tags
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var specialTags = await _db.SpecialTags.FindAsync(Id);
            if (specialTags == null)
            {
                return NotFound();
            }
            return View(specialTags);
        }

        //Get Delete Method
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var specialTags = await _db.SpecialTags.FindAsync(Id);
            if (specialTags == null)
            {
                return NotFound();
            }
            return View(specialTags);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var specialTags = await _db.SpecialTags.FindAsync(Id);
            _db.Remove(specialTags);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}