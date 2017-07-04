using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TTS.Lib.Models;

namespace TTS.Web.Controllers
{
    [Route("{culture}/Admin/Menu")]
    public class TtsMenuController : Controller
    {
        private readonly ttsContext _context;

        public TtsMenuController(ttsContext context)
        {
            _context = context;    
        }

        // GET: TtsMenu
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var ttsContext = _context.TtsMenu.Include(t => t.SpecificultureNavigation);
            return View(await ttsContext.ToListAsync());
        }

        // GET: TtsMenu/Details/5
        [Route("Deatils/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ttsMenu = await _context.TtsMenu
                .Include(t => t.SpecificultureNavigation)
                .SingleOrDefaultAsync(m => m.MenuId == id);
            if (ttsMenu == null)
            {
                return NotFound();
            }

            return View(ttsMenu);
        }

        // GET: TtsMenu/Create
        [Route("Create")]
        public IActionResult Create()
        {
            ViewData["Specificulture"] = new SelectList(_context.TtsCulture, "Specificulture", "Specificulture");
            return View();
        }

        // POST: TtsMenu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuId,Specificulture,Icon,Title,Description,Views,Position,IsDeleted,Sename,Seotitle,Seodescription,Seokeywords,CreatedDate,CreatedBy,Level,ParentMenuId")] TtsMenu ttsMenu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ttsMenu);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["Specificulture"] = new SelectList(_context.TtsCulture, "Specificulture", "Specificulture", ttsMenu.Specificulture);
            return View(ttsMenu);
        }

        // GET: TtsMenu/Edit/5
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ttsMenu = await _context.TtsMenu.SingleOrDefaultAsync(m => m.MenuId == id);
            if (ttsMenu == null)
            {
                return NotFound();
            }
            ViewData["Specificulture"] = new SelectList(_context.TtsCulture, "Specificulture", "Specificulture", ttsMenu.Specificulture);
            return View(ttsMenu);
        }

        // POST: TtsMenu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MenuId,Specificulture,Icon,Title,Description,Views,Position,IsDeleted,Sename,Seotitle,Seodescription,Seokeywords,CreatedDate,CreatedBy,Level,ParentMenuId")] TtsMenu ttsMenu)
        {
            if (id != ttsMenu.MenuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ttsMenu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TtsMenuExists(ttsMenu.MenuId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["Specificulture"] = new SelectList(_context.TtsCulture, "Specificulture", "Specificulture", ttsMenu.Specificulture);
            return View(ttsMenu);
        }

        // GET: TtsMenu/Delete/5
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ttsMenu = await _context.TtsMenu
                .Include(t => t.SpecificultureNavigation)
                .SingleOrDefaultAsync(m => m.MenuId == id);
            if (ttsMenu == null)
            {
                return NotFound();
            }

            return View(ttsMenu);
        }

        // POST: TtsMenu/Delete/5
        [Route("Delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ttsMenu = await _context.TtsMenu.SingleOrDefaultAsync(m => m.MenuId == id);
            _context.TtsMenu.Remove(ttsMenu);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TtsMenuExists(int id)
        {
            return _context.TtsMenu.Any(e => e.MenuId == id);
        }
    }
}
