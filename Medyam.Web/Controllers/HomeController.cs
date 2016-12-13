using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Medyam.Core.Entities;
using Medyam.Services.Interfaces;
using Medyam.Web.Models;
using Newtonsoft.Json;

namespace Medyam.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IPhotoService _photoService;
        public HomeController(IPhotoService photoService)
        {
            _photoService = photoService;

        }

        public async Task<ActionResult> Index()
        {
            IEnumerable<PhotoModel> photos = null;
            try
            {
                var data = await _photoService.GetAllAsync();

                photos = data.AsEnumerable().Where(d => !string.IsNullOrEmpty(d.Url)).Select(p => new PhotoModel()
                {
                    Url = p.Url,
                    Title = p.Title,
                    Id = p.ID,
                    Tags = p.Tags

                }).ToList();


                var tagCloud = new List<string>();
                foreach (var p in photos)
                {
                    if (string.IsNullOrEmpty(p.Tags)) continue;

                    foreach (var tag in p.Tags.Split(','))
                    {
                        if (!tagCloud.Contains(tag))
                            tagCloud.Add(tag);
                    }
                }

                ViewBag.TagCloudData = JsonConvert.SerializeObject(tagCloud);
            }
            catch
            {
                // ignored
            }


            return View(photos);
        }

        [HttpGet]
        public async Task<JsonResult> GetTags()
        {
            var tagCloud = new List<string>();
            try
            {
                IEnumerable<PhotoModel> photos = null;
                var data = await _photoService.GetAllAsync();

                photos = data.AsEnumerable().Where(d => !string.IsNullOrEmpty(d.Url)).Select(p => new PhotoModel()
                {
                    Url = p.Url,
                    Title = p.Title,
                    Id = p.ID,
                    Tags = p.Tags

                }).ToList();
              
                foreach (var p in photos)
                {
                    if (string.IsNullOrEmpty(p.Tags)) continue;

                    foreach (var tag in p.Tags.Split(','))
                    {
                        if (!tagCloud.Contains(tag))
                            tagCloud.Add(tag);
                    }
                }


            }
            catch
            {
                // ignored
            }

            return Json(tagCloud, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PhotoViewModel model, HttpPostedFileBase photo)
        {
            if (!ModelState.IsValid) return View(model);
            var key = Guid.NewGuid();
            var entity = new PhotoEntity(key.ToString(), "photo")
            {
                ID = key,
                Title = model.Title,
                Owner = "medyam"
            };

            await _photoService.CreateAsync(entity, photo.InputStream);

            return RedirectToAction("Index");
        }
    }
}