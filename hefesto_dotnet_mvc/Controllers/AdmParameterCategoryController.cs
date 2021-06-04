using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hefesto.admin.Models;

namespace hefesto_dotnet_mvc.Controllers
{
    public class AdmParameterCategoryController : Controller
    {
        private readonly dbhefestoContext _context;

        public AdmParameterCategoryController(dbhefestoContext context)
        {
            _context = context;
        }

        // GET: AdmParameterCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.AdmParameterCategories.ToListAsync());
        }

        // GET: AdmParameterCategories/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admParameterCategory = await _context.AdmParameterCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admParameterCategory == null)
            {
                return NotFound();
            }

            return View(admParameterCategory);
        }

        // GET: AdmParameterCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdmParameterCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Order")] AdmParameterCategory admParameterCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admParameterCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admParameterCategory);
        }

        // GET: AdmParameterCategories/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admParameterCategory = await _context.AdmParameterCategories.FindAsync(id);
            if (admParameterCategory == null)
            {
                return NotFound();
            }
            return View(admParameterCategory);
        }

        // POST: AdmParameterCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Description,Order")] AdmParameterCategory admParameterCategory)
        {
            if (id != admParameterCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admParameterCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdmParameterCategoryExists(admParameterCategory.Id))
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
            return View(admParameterCategory);
        }

        // GET: AdmParameterCategories/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admParameterCategory = await _context.AdmParameterCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admParameterCategory == null)
            {
                return NotFound();
            }

            return View(admParameterCategory);
        }

        // POST: AdmParameterCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var admParameterCategory = await _context.AdmParameterCategories.FindAsync(id);
            _context.AdmParameterCategories.Remove(admParameterCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdmParameterCategoryExists(long id)
        {
            return _context.AdmParameterCategories.Any(e => e.Id == id);
        }
    }
}
