using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdProject.Model;
using BirdProject.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BirdProject.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net;
using Geocoding.Google;
using Geocoding;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BirdProject.Controllers
{
    public class resultsController : Controller
    {
        private readonly ILogger<resultsController> _logger;
        private readonly BirdProjectContext _db;

        public resultsController(ILogger<resultsController> logger, BirdProjectContext db)
        {
            _logger = logger;
            _db = db;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


        
        public IActionResult generateSearchResult(string test)
        {

            

            TempData["birds"] = test;

            return RedirectToAction("exposeResults");
        }

        public async Task<IActionResult> exposeResults()
        {

            static async Task<string> getPlaceAsync(double latitude, double longitude)
            {
                using var client = new HttpClient();

                string urlLatitude = latitude.ToString();
                string urlLongitude = longitude.ToString();

                string baseURL = "https://maps.googleapis.com/maps/api/geocode/json?latlng=";

                string concatenatedURL = baseURL + latitude + "," + longitude + "&key=";

                var result = await client.GetAsync(concatenatedURL);

                var contents = await result.Content.ReadAsStringAsync();

                JObject json = JObject.Parse(contents);

                var fullCompoundCode = json["plus_code"]["compound_code"].ToString();

                var placeName = Regex.Match(fullCompoundCode, @"\s(.*)").Groups[1].Value;


                return placeName;
            }


            string convertedBirdDataString = (string)TempData["birds"];

            if (convertedBirdDataString == null) return RedirectToAction("ShowBirdLocations", "Home");

            var birdRecords = JsonConvert.DeserializeObject<birdDataSolutionVM>(convertedBirdDataString);

            var firstCaptureData = _db.BirdBtos.Where(a => a.MetalRing == "FB11111").ToList();

            List<string> allPlaceNames = new List<string>();

            foreach (var item in birdRecords.birdData)
            {
                var f = await getPlaceAsync((double)item.latitude, (double)item.longitude);


                allPlaceNames.Add(f.ToString());
            }


            fullBirdData fullData = new fullBirdData
            {
                firstCapture = new firstCaptureData()
                {
                    ColourRingCode = firstCaptureData[0].ColourRingCode,
                    ColourRingPosition = firstCaptureData[0].ColourRingPosition,
                    Sex = firstCaptureData[0].Sex,
                    Specie = firstCaptureData[0].Specie,
                    ColourRingColour = firstCaptureData[0].ColourRingColour
                },
                restCaptures = birdRecords,
                cityNames = allPlaceNames
            };






            return View(fullData);
        }



        
    }
}

