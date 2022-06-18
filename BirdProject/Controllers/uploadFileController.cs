using BirdProject.Model;
using BirdProject.Model.ViewModel;
using CsvHelper;
using Geocoding;
using Geocoding.Google;
using GoogleMaps.LocationServices;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BirdProject.Controllers
{
    public class uploadFileController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly BirdProjectContext _db;

        public uploadFileController(ILogger<HomeController> logger, BirdProjectContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(uploadBTOCSVFileVM BTOfile)
        {
            if (ModelState.IsValid)
            {
                if(BTOfile != null)
                {

                    //FileInfo object to hold the file
                    FileInfo fileInfo = new FileInfo(BTOfile.CSVFile.FileName);

                    string? gridRef = null, colourRingCode = null, colourRingPosition = null, colourRingColour = null;
                    double? latitude = null, longitude = null;
                    string location = "", cityName = "", metalRing = "", sex = "", specie = "";
                    List<double> coordinates;



                    IGeocoder geocoder = new GoogleGeocoder() { ApiKey = "AIzaSyC9CgPS4ItuYn1SjMRJ6um7P9qqmCnPyC4" };


                    static async Task<List<double>> getCoordinatesByPlaceAsync(string placeName, IGeocoder geocoder)
                    {
                        IEnumerable<Address> addresses = await geocoder.GeocodeAsync(placeName);
                        List<double> coordinates = new List<double>();

                        coordinates.Add(addresses.First().Coordinates.Latitude);
                        coordinates.Add(addresses.First().Coordinates.Longitude);

                        return coordinates;
                    }


                    if (fileInfo.Extension == ".csv")
                    {
                        using (var stream = BTOfile.CSVFile.OpenReadStream())
                        {
                            using (StreamReader sr = new StreamReader(stream))
                            {
                                string[] headers = sr.ReadLine().Split(',');

                                while (!sr.EndOfStream)
                                {
                                    string[] rows = sr.ReadLine().Split(',');





                                    location = "";
                                    cityName = "";
                                    latitude = null;
                                    longitude = null;
                                    gridRef = null;
                                    metalRing = rows[4];
                                    sex = rows[12];
                                    specie = rows[8];


                                    DateTime dt = DateTime.ParseExact(rows[16], "dd/MM/yyyy", CultureInfo.InvariantCulture);


                                    if (rows.Length == 110)
                                    {
                                        if (!Regex.Match(rows[20], @"^[A-Z]{2}\d+").Success)
                                        {
                                            location = rows[18] + rows[19];
                                            cityName = Regex.Match(location, @"\(([^)]*)\)").Groups[1].Value;
                                            coordinates = await getCoordinatesByPlaceAsync(cityName, geocoder);
                                            latitude = coordinates[0];
                                            longitude = coordinates[1];
                                            gridRef = null;
                                        }
                                        else
                                        {
                                            gridRef = rows[20];
                                            latitude = null;
                                            longitude = null;
                                        }

                                        bool exists = _db.BirdBtos.Any(e => e.MetalRing == rows[4]);

                                        if (!exists && rows[3] == "N")
                                        {

                                            if (rows[78].Length > 2)
                                            {
                                                colourRingCode = rows[78];
                                                colourRingPosition = "Left-below";
                                                colourRingColour = rows[78].Substring(0, 1);
                                            }
                                            else if (rows[79].Length > 2)
                                            {
                                                colourRingCode = rows[79];
                                                colourRingPosition = "Right-below";
                                                colourRingColour = rows[79].Substring(0, 1);
                                            }
                                            else if (rows[80].Length > 2)
                                            {
                                                colourRingCode = rows[80];
                                                colourRingPosition = "Left-above";
                                                colourRingColour = rows[80].Substring(0, 1);
                                            }
                                            else if (rows[81].Length > 2)
                                            {
                                                colourRingCode = rows[81];
                                                colourRingPosition = "Right-above";
                                                colourRingColour = rows[81].Substring(0, 1);
                                            }
                                            else
                                            {
                                                colourRingCode = null;
                                                colourRingPosition = null;
                                                colourRingColour = null;
                                            }

                                            switch (colourRingColour)
                                            {
                                                case "B":
                                                    colourRingColour = "Blue";
                                                    break;
                                                case "W":
                                                    colourRingColour = "White";
                                                    break;
                                                case "G":
                                                    colourRingColour = "Green";
                                                    break;
                                            }

                                            if(colourRingCode != null) colourRingCode = Regex.Match(colourRingCode, @"\(([^)]*)\)").Groups[1].Value;





                                            _db.BirdBtos.Add(new BirdBto
                                            {
                                                MetalRing = metalRing,
                                                Sex = sex,
                                                Specie = specie,
                                                GridRef = gridRef,
                                                Latitude = latitude,
                                                Longitude = longitude,
                                                ColourRingCode = colourRingCode,
                                                ColourRingPosition = colourRingPosition,
                                                ColourRingColour = colourRingColour,
                                                Date = dt

                                            });
                                        }
                                    }
                                    else if (rows.Length == 109)
                                    {
                                        gridRef = rows[19];

                                        bool exists = _db.BirdBtos.Any(e => e.MetalRing == rows[4]);

                                        if (!exists && rows[3] == "N")
                                        {

                                            if (rows[77].Length > 2)
                                            {
                                                colourRingCode = rows[77];
                                                colourRingPosition = "Left-below";
                                                colourRingColour = rows[77].Substring(0, 1);
                                            }
                                            else if (rows[78].Length > 2)
                                            {
                                                colourRingCode = rows[78];
                                                colourRingPosition = "Right-below";
                                                colourRingColour = rows[78].Substring(0, 1);
                                            }
                                            else if (rows[79].Length > 2)
                                            {
                                                colourRingCode = rows[79];
                                                colourRingPosition = "Left-above";
                                                colourRingColour = rows[79].Substring(0, 1);
                                            }
                                            else if (rows[80].Length > 2)
                                            {
                                                colourRingCode = rows[80];
                                                colourRingPosition = "Right-above";
                                                colourRingColour = rows[80].Substring(0, 1);
                                            }
                                            else
                                            {
                                                colourRingCode = null;
                                                colourRingPosition = null;
                                                colourRingColour = null;
                                            }

                                            switch (colourRingColour)
                                            {
                                                case "B":
                                                    colourRingColour = "Blue";
                                                    break;
                                                case "W":
                                                    colourRingColour = "White";
                                                    break;
                                                case "G":
                                                    colourRingColour = "Green";
                                                    break;

                                            }

                                            if (colourRingCode != null) colourRingCode = Regex.Match(colourRingCode, @"\(([^)]*)\)").Groups[1].Value;

                                            _db.BirdBtos.Add(new BirdBto
                                            {
                                                MetalRing = metalRing,
                                                Sex = sex,
                                                Specie = specie,
                                                GridRef = gridRef,
                                                Latitude = latitude,
                                                Longitude = longitude,
                                                ColourRingCode = colourRingCode,
                                                ColourRingPosition = colourRingPosition,
                                                ColourRingColour = colourRingColour,
                                                Date = dt

                                            });
                                        }
                                    }
                                }
                            }

                            _db.SaveChanges();




                            using (var stream1 = BTOfile.CSVFile.OpenReadStream())
                            {
                                using (StreamReader sr = new StreamReader(stream1))
                                {
                                    string[] headers = sr.ReadLine().Split(',');

                                    while (!sr.EndOfStream)
                                    {

                                        string[] rows = sr.ReadLine().Split(',');
                                        bool exists = _db.BirdBtos.Any(e => e.MetalRing == rows[4]);
                                        if (exists && rows[3] == "S")
                                        {
                                            DateTime dt = DateTime.ParseExact(rows[16], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            metalRing = rows[4];


                                            if (rows.Length == 110)
                                            {
                                                if (!Regex.Match(rows[20], @"^[A-Z]{2}\d+").Success)
                                                {
                                                    location = rows[18] + rows[19];
                                                    cityName = Regex.Match(location, @"\(([^)]*)\)").Groups[1].Value;
                                                    coordinates = await getCoordinatesByPlaceAsync(cityName, geocoder);
                                                    latitude = coordinates[0];
                                                    longitude = coordinates[1];
                                                    gridRef = null;


                                                    _db.SpotLogs.Add(new SpotLog
                                                    {
                                                        Date = dt,
                                                        Longitude = longitude,
                                                        Latitude = latitude,
                                                        GridRef = gridRef,
                                                        MetalRing = metalRing,
                                                        Email = null
                                                    });
                                                }
                                                else
                                                {
                                                    gridRef = rows[20];
                                                    latitude = null;
                                                    longitude = null;
                                                    _db.SpotLogs.Add(new SpotLog
                                                    {
                                                        Date = dt,
                                                        Longitude = longitude,
                                                        Latitude = latitude,
                                                        GridRef = gridRef,
                                                        MetalRing = metalRing,
                                                        Email = null
                                                    });

                                                }
                                            }
                                            else if (rows.Length == 109)
                                            {
                                                if (!Regex.Match(rows[19], @"^[A-Z]{2}\d+").Success)
                                                {
                                                    location = rows[18];

                                                    cityName = Regex.Match(location, @"\(([^)]*)\)").Groups[1].Value;
                                                    Console.WriteLine("The city name is: " + cityName);
                                                    coordinates = await getCoordinatesByPlaceAsync(cityName, geocoder);
                                                    latitude = coordinates[0];
                                                    longitude = coordinates[1];
                                                    gridRef = null;


                                                    _db.SpotLogs.Add(new SpotLog
                                                    {
                                                        Date = dt,
                                                        Longitude = longitude,
                                                        Latitude = latitude,
                                                        GridRef = gridRef,
                                                        MetalRing = metalRing,
                                                        Email = null
                                                    });
                                                }
                                                else
                                                {
                                                    gridRef = rows[19];
                                                    latitude = null;
                                                    longitude = null;
                                                    _db.SpotLogs.Add(new SpotLog
                                                    {
                                                        Date = dt,
                                                        Longitude = longitude,
                                                        Latitude = latitude,
                                                        GridRef = gridRef,
                                                        MetalRing = metalRing,
                                                        Email = null
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            _db.SaveChanges();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("IncorrectExtension", "Sorry, you entered a file with a wrong extension.");
                        ModelState.AddModelError("IncorrectExtension2", "The file extension must be .csv");

                        return View();
                    }

                }
                else
                {
                    ModelState.AddModelError("NullFile", "Sorry, it looks like the submission did not go through.");
                    ModelState.AddModelError("NullFile2", "Please, contact an administrator.");
                }
            }

            return View();
        }
    }
}
