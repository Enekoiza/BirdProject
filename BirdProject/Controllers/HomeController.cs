using BirdProject.Model;
using BirdProject.Model.ViewModel;
using BirdProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BirdProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BirdProjectContext _db;

        public HomeController(ILogger<HomeController> logger, BirdProjectContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var objBird = _db.BirdBtos.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Index(IndexForm form1)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos");

                FileInfo fileInfo = new FileInfo(form1.birdPhoto.FileName);
                string fileName = "F222" + fileInfo.Extension;

                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    form1.birdPhoto.CopyTo(stream);
                }


                var newSpot = new SpotLog();
                newSpot.Longitude = form1.longitude;
                newSpot.Latitude = form1.latitude;
                newSpot.Date = form1.date;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}