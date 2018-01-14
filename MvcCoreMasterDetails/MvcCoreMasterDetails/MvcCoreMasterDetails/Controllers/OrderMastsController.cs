using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCoreMasterDetails.Data;
using MvcCoreMasterDetails.Models.OrderMgmt.Models;

namespace MvcCoreMasterDetails.Controllers
{
    public class OrderMastsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderMastsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrderMasts
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderMast.ToListAsync());
        }

        // GET: OrderMasts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderMast = await _context.OrderMast
                .SingleOrDefaultAsync(m => m.Id == id);
            if (orderMast == null)
            {
                return NotFound();
            }

            return View(orderMast);
        }

        // GET: OrderMasts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderMasts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerName,OrderDate")] OrderMast orderMast)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderMast);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderMast);
        }

        // GET: OrderMasts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderMast = await _context.OrderMast.SingleOrDefaultAsync(m => m.Id == id);
            if (orderMast == null)
            {
                return NotFound();
            }
            return View(orderMast);
        }

        // POST: OrderMasts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerName,OrderDate")] OrderMast orderMast)
        {
            if (id != orderMast.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderMast);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderMastExists(orderMast.Id))
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
            return View(orderMast);
        }

        // GET: OrderMasts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderMast = await _context.OrderMast
                .SingleOrDefaultAsync(m => m.Id == id);
            if (orderMast == null)
            {
                return NotFound();
            }

            return View(orderMast);
        }

        // POST: OrderMasts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderMast = await _context.OrderMast.SingleOrDefaultAsync(m => m.Id == id);
            _context.OrderMast.Remove(orderMast);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderMastExists(int id)
        {
            return _context.OrderMast.Any(e => e.Id == id);
        }
    }
}
