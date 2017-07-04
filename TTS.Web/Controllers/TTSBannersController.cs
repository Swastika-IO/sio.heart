using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TTS.Lib.Models;
using TTS.Lib.Repositories;
using Microsoft.AspNetCore.Routing;
using TTS.Lib.ViewModels.Admin;
using System.Drawing;
using Swastika.Common;
using Microsoft.AspNetCore.Hosting;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Net.Http.Headers;
using TTS.Lib.ViewModels;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TTS.Web.Controllers
{
    [Route("{culture}/Admin/Banners")]
    public class TTSBannersController : BaseController
    {

        private readonly BannerRepository _repo;

        public TTSBannersController(IHostingEnvironment env) : base(env)
        {
            _repo = BannerRepository.GetInstance();

        }

        [Route("")]
        [Route("Index")]
        // GET: TTSBanners
        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetModelListByAsync(b => b.Specificulture == _lang));
        }


        // GET: TTSBanners/Details/5
        [Route("Details/{id}")]
        public async Task<IActionResult> Details(string id = null)
        {

            if (id == null)
            {
                return NotFound();
            }

            var tTSBanner = await _repo.GetSingleModelAsync(m => m.Id == id);
            if (tTSBanner == null)
            {
                return NotFound();
            }

            return View(tTSBanner);
        }


        // GET: TTSBanners/Create
        [Route("Create")]
        public IActionResult Create()
        {
            return View(new BannerViewModel(_lang)
            {
                Id = Guid.NewGuid().ToString()
            });
        }

        // POST: TTSBanners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BannerViewModel tTSBanner)
        {
            if (ModelState.IsValid)
            {

                var files = HttpContext.Request.Form.Files;

                string fileName = await UploadFileAsync(files.FirstOrDefault(), "Banners");
                if (!string.IsNullOrEmpty(fileName))
                {
                    tTSBanner.Image = fileName;
                    tTSBanner.CreatedDate = DateTime.UtcNow;
                    var result = await tTSBanner.SaveModelAsync();
                    if (result.IsSucceed)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        throw new Exception(result.Ex.StackTrace);
                    }
                }
                else
                {
                    return View(tTSBanner);
                }
            }
            else
            {
                return View(tTSBanner);
            }


        }

        // GET: TTSBanners/Edit/5
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(string id = null)
        {
            if (id == null)
            {
                return NotFound();
            }
            GetLanguage();
            var tTSBanner = await _repo.GetSingleModelAsync(m => m.Id == id && m.Specificulture == _lang);
            if (tTSBanner == null)
            {
                return NotFound();
            }
            return View(tTSBanner);
        }

        // POST: TTSBanners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, BannerViewModel tTSBanner)
        {
            if (id != tTSBanner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;

                    string fileName = await UploadFileAsync(files.FirstOrDefault(), "Banners");
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        tTSBanner.Image = fileName;
                    }

                    var result = await tTSBanner.SaveModelAsync();
                    if (result.IsSucceed)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Ex.Message);
                        return View(tTSBanner);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repo.CheckIsExists(b => b.Id == tTSBanner.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View(tTSBanner);
        }

        // GET: TTSBanners/Delete/5
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(string id = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tTSBanner = await _repo.GetSingleModelAsync(m => m.Id == id);
            if (tTSBanner == null)
            {
                return NotFound();
            }

            return View(tTSBanner);
        }

        // POST: TTSBanners/Delete/5
        [Route("Delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _repo.RemoveModelAsync(m => m.Id == id);
            return RedirectToAction("Index");
        }

    }
}
