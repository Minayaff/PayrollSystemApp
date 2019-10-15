using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HrPayroll.DAL;
using HrPayroll.Models;
using Microsoft.AspNetCore.Authorization;
using HrPayroll.Utilities;

namespace HrPayroll.Controllers
{
    [Authorize(Roles = StaticData.PayrollSpecialistRole)]

    public class CampaignsController : Controller
    {
        private readonly AppDbContext _context;

        public CampaignsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Campaigns.Include(c => c.Branch);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campaign == null)
            {
                return NotFound();
            }

            return View(campaign);
        }

        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BranchId,Date,FromAmount,ToAmount,Bonus")] Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                _context.Add(campaign);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", campaign.BranchId);
            return View(campaign);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns.FindAsync(id);
            if (campaign == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", campaign.BranchId);
            return View(campaign);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BranchId,Date,FromAmount,ToAmount,Bonus")] Campaign campaign)
        {
            if (id != campaign.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(campaign);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampaignExists(campaign.Id))
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
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", campaign.BranchId);
            return View(campaign);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campaign == null)
            {
                return NotFound();
            }

            return View(campaign);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var campaign = await _context.Campaigns.FindAsync(id);
            _context.Campaigns.Remove(campaign);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampaignExists(int id)
        {
            return _context.Campaigns.Any(e => e.Id == id);
        }
    }
}
