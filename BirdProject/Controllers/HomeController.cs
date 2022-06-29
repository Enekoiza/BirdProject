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
        public string birdData([FromHeader] string data)
        {
            //var VM = new birdLocationsVM();

            var VM = new birdDataSolutionVM();

            List<birdRecordVM> birdRecords = new List<birdRecordVM>();

            var holder = _db.SpotLogs.Where(a => a.MetalRing == "FB11111");

            foreach (var item in holder)
            {

                var birdLog = new birdRecordVM
                {
                    longitude = item.Longitude,
                    latitude = item.Latitude,
                    gridRef = item.GridRef,
                    date = item.Date

                };

                birdRecords.Add(birdLog);
            }


            VM.birdData = birdRecords;

            VM.metalRingID = "FB11111";


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