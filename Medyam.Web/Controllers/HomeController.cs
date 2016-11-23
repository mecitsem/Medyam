using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Medyam.Core.Entities;
using Medyam.Services.Interfaces;
using Medyam.Web.Models;

namespace Medyam.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPhotoService _photoService;
        public HomeController(IPhotoService photoService)
        {
            _photoService = photoService;

        }

        public ActionResult Index()
        {
            IEnumerable<PhotoModel> photos = null;
            try
            {
                var data = _photoService.GetAll();

                photos = data.AsEnumerable().Where(d => !string.IsNullOrEmpty(d.Url)).Select(p => new PhotoModel()
                {
                    Url = p.Url,
                    Title = p.Title,
                    Id = p.ID
                }).ToList();
            }
            catch
            {
                // ignored
            }

            return View(photos);
        }



        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PhotoViewModel model, HttpPostedFileBase photo)
        {
            if (!ModelState.IsValid) return View(model);
            var key = Guid.NewGuid();
            var entity = new PhotoEntity(key.ToString(), "photo")
            {
                ID = key,
                Title = model.Title,
                Owner = "medyam"
            };

            _photoService.Create(entity, photo.InputStream);

            return RedirectToAction("Index");
        }
    }
}