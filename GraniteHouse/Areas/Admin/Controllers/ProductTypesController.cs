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
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {

            return View(_db.ProductTypes.ToList());
        }

        //Get create Action Method
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if(ModelState.IsValid)
            {
                _db.Add(productTypes);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(productTypes);
        }

        //Get edit   Action Method
        public async Task<IActionResult> Edit(int? Id)
        {
            if(Id==null)
            {
                return NotFound();
            }
            var productType = await _db.ProductTypes.FindAsync(Id);
            if(productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, ProductTypes productTypes)
        {
            if (Id != productTypes.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Update(productTypes);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(productTypes);
        }

        //Get details   Action Method
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var productType = await _db.ProductTypes.FindAsync(Id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        //Get delete Method
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var productType = await _db.ProductTypes.FindAsync(Id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        //Added ActionName to keep names on get and post equals
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var productTypes = await _db.ProductTypes.FindAsync(Id);
            _db.ProductTypes.Remove(productTypes);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}