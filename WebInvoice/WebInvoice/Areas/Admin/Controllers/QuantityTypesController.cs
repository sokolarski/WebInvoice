using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebInvoice.Data;
using WebInvoice.Data.CompanyData.Models;

namespace WebInvoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuantityTypesController : Controller
    {
        private readonly CompanyDbContext _context;

        public QuantityTypesController(CompanyDbContext context)
        {
            _context = context;
        }

        // GET: Admin/QuantityTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.QuantityTypes.ToListAsync());
        }

        // GET: Admin/QuantityTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quantityType = await _context.QuantityTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quantityType == null)
            {
                return NotFound();
            }

            return View(quantityType);
        }

        // GET: Admin/QuantityTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/QuantityTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,Description,IsActive,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] QuantityType quantityType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quantityType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quantityType);
        }

        // GET: Admin/QuantityTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quantityType = await _context.QuantityTypes.FindAsync(id);
            if (quantityType == null)
            {
                return NotFound();
            }
            return View(quantityType);
        }

        // POST: Admin/QuantityTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Type,Description,IsActive,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] QuantityType quantityType)
        {
            if (id != quantityType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quantityType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuantityTypeExists(quantityType.Id))
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
            return View(quantityType);
        }

        // GET: Admin/QuantityTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quantityType = await _context.QuantityTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quantityType == null)
            {
                return NotFound();
            }

            return View(quantityType);
        }

        // POST: Admin/QuantityTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quantityType = await _context.QuantityTypes.FindAsync(id);
            _context.QuantityTypes.Remove(quantityType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuantityTypeExists(int id)
        {
            return _context.QuantityTypes.Any(e => e.Id == id);
        }
    }
}
