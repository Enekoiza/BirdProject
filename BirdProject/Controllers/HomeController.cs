using BirdProject.Model;
using BirdProject.Model.ViewModel;
using BirdProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        public IActionResult ShowBirdLocations()
        {

            return View();
        }


        [HttpPost]
        public async Task<string> birdData([FromHeader] string data)
        {
            var VM = new birdLocationsVM();
            List<birdRecordVM> birdRecords = new List<birdRecordVM>();


            var bird1 = new birdRecordVM { longitude = 25.2F, latitude = 25.2F, gridRef = null };
            var bird2 = new birdRecordVM { longitude = 40.2F, latitude = 25.2F, gridRef = null };
            var bird3 = new birdRecordVM { longitude = 23.4F, latitude = 25.2F, gridRef = null };
            var bird4 = new birdRecordVM { longitude = null, latitude = null, gridRef = "ST2264" };



            birdRecords.Add(bird1);
            birdRecords.Add(bird2);
            birdRecords.Add(bird3);
            birdRecords.Add(bird4);

            VM.birdRecords = birdRecords;




            string a = JsonConvert.SerializeObject(VM);
            return a;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IndexForm form1)
        {
            if (ModelState.IsValid)
            {
                if(form1.birdPhoto != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos");

                    string specie = form1.specie;
                    string ringCode = form1.colourRingCode;

                    FileInfo fileInfo = new FileInfo(form1.birdPhoto.FileName);
                    string fileName = "F22" + fileInfo.Extension;

                    string fileNameWithPath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        form1.birdPhoto.CopyTo(stream);
                    }
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