using BirdProject.Model;
using BirdProject.Model.ViewModel;
using BirdProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            var err = TempData["WrongRingCodeErrorMessage"] as String;


            if(err == "Error message")
            {
                ModelState.AddModelError("CustomError", "Sorry, it looks like the submission did not go through.");
            }

            return View();
        }


        [HttpPost]
        public ActionResult<string> birdData([FromHeader] string data)
        {

            var VM = new birdDataSolutionVM();

            JObject json = JObject.Parse(data);

            var userRing = json["ringCode"].ToString();

            bool check = _db.BirdBtos.Any(e=>e.ColourRingCode.Equals(userRing));

            if (check == false)
            {
                TempData["WrongRingCodeErrorMessage"] = "Error message";



                return RedirectToAction("ShowBirdLocations");
            }

            List<birdRecordVM> birdRecords = new List<birdRecordVM>();

            var metalRingHolder = _db.BirdBtos.Where(a => a.ColourRingCode.Equals(userRing)).ToList();

            var holder = _db.SpotLogs.Where(a => a.MetalRing == metalRingHolder[0].MetalRing);

            var holder1 = _db.BirdBtos.Where(a => a.MetalRing == metalRingHolder[0].MetalRing).ToList();


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

            var birdLogFirstCapture = new birdRecordVM
            {
                longitude = holder1[0].Longitude,
                latitude = holder1[0].Latitude,
                gridRef = holder1[0].GridRef,
                date = holder1[0].Date
            };

            birdRecords.Add(birdLogFirstCapture);

            VM.birdData = birdRecords;

            VM.metalRingID = userRing;


            string jsonResponse = JsonConvert.SerializeObject(VM);
            return jsonResponse;

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
                if (!_db.BirdBtos.Any(e => e.ColourRingCode.Equals(form1.colourRingCode)))
                {
                    ModelState.AddModelError("NoData", "Sorry, we are not currently holding data about that bird.");
                    return View();
                }


                if (form1.birdPhoto != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos");



                    string specie = form1.specie;
                    string ringCode = form1.colourRingCode;

                    FileInfo fileInfo = new FileInfo(form1.birdPhoto.FileName);

                    string fileName = form1.colourRingCode + fileInfo.Extension;

                    string fileNameWithPath = Path.Combine(path, fileName);

                    int fileCounter = 0;

                    while (System.IO.File.Exists(fileNameWithPath))
                    {
                        fileName = form1.colourRingCode + "-" + fileCounter.ToString() + fileInfo.Extension;

                        fileNameWithPath = Path.Combine(path, fileName);

                        fileCounter++;
                    }

                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        form1.birdPhoto.CopyTo(stream);
                    }
                }


                var holder = _db.BirdBtos.Where(a => a.ColourRingCode == form1.colourRingCode).ToList();

                var metalRingCode = holder[0].MetalRing;

                var newSpot = new SpotLog {
                    Latitude = form1.latitude,
                    Longitude = form1.longitude,
                    Date = form1.date,
                    GridRef = null,
                    Email = null,
                    MetalRing = metalRingCode

                };

                

                _db.SpotLogs.Add(newSpot);
                _db.SaveChanges();

                TempData["metalRing"] = form1.colourRingCode;

                return RedirectToAction("returnFullData");
            }

            return View();
        }

        public IActionResult returnFullData()
        {

            var metalRingCode = TempData["metalRing"] as string;

            var dataAfterRequest = new dataAfterRequestVM { metalRing = metalRingCode };

            return View(dataAfterRequest);
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

