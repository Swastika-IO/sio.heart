using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTS.Lib.Models;
using Microsoft.AspNetCore.Hosting;
using TTS.Lib.Repositories;
using TTS.Lib.ViewModels;
using System.Linq;

namespace TTS.Web.Controllers
{
    [Route("{culture}/Admin/Culture")]
    public class CultureController : BaseController
    {
        private readonly CultureRepository _repo;
        public CultureController(IHostingEnvironment env):base(env)
        {
            _repo = CultureRepository.GetInstance();
        }

        // GET: Culture
        [Route("Index")]
        public async Task<IActionResult> Index()
        {            
            return View(await _repo.GetModelListAsync());
        }

        // GET: Culture/Details/5
        [Route("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ttsCulture = await _repo.GetSingleModelAsync(m => m.Id == id);
            if (ttsCulture == null)
            {
                return NotFound();
            }

            return View(ttsCulture);
        }

        // GET: Culture/Create
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Culture/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Specificulture,Lcid,Alias,FullName,Description,Icon")] TtsCulture ttsCulture)
        {
            if (ModelState.IsValid)
            {               
                var result = await _repo.CreateModelAsync(ttsCulture);
                if (result.IsSucceed)
                {
                    listCultures.Add(result.Data);
                }
                return RedirectToAction("Index");
            }
            return View(ttsCulture);
        }

        // GET: Culture/Edit/5
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ttsCulture = await _repo.GetSingleModelAsync(m => m.Id == id);
            if (ttsCulture == null)
            {
                return NotFound();
            }
            return View(ttsCulture);
        }

        // POST: Culture/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CultureViewModel ttsCulture)
        {
            if (id != ttsCulture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var result = await ttsCulture.SaveModeAsync(); //_repo.EditModelAsync(ttsCulture.ParseModel());
                    if (result.IsSucceed)
                    {
                        var current = listCultures.FirstOrDefault(c => c.Id == ttsCulture.Id);
                        current = result.Data;
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repo.CheckIsExists(c => c.Specificulture == ttsCulture.Specificulture))
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
            return View(ttsCulture);
        }

        // GET: Culture/Delete/5
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ttsCulture = await _repo.GetSingleModelAsync(m => m.Id == id);
            if (ttsCulture == null)
            {
                return NotFound();
            }

            return View(ttsCulture);
        }

        // POST: Culture/Delete/5
        [Route("Delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.RemoveModelAsync(c => c.Id == id);
            return RedirectToAction("Index");
        }

    }
}
