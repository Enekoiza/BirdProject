using BirdProject.Model;
using BirdProject.Model.ViewModel;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index(uploadBTOCSVFileVM BTOfile)
        {
            if (ModelState.IsValid)
            {
                if(BTOfile != null)
                {
                    FileInfo fileInfo = new FileInfo(BTOfile.CSVFile.FileName);

                    if(fileInfo.Extension == ".csv")
                    {
                        using (var stream = BTOfile.CSVFile.OpenReadStream())
                        {
                            using (StreamReader sr = new StreamReader(stream))
                            {
                                string[] headers = sr.ReadLine().Split(',');

                                while (!sr.EndOfStream)
                                {
                                    string[] rows = sr.ReadLine().Split(',');
                                    bool exists = _db.BirdBtos.Any(e => e.MetalRing == rows[4]);
                                    if (!exists && rows[3] == "N")
                                    {
                                        string ringCode, ringPos;
                                        if (rows[77].Length > 2) { ringCode = rows[77]; ringPos = "Left-below";  }
                                        else if (rows[78].Length > 2) { ringCode = rows[78]; ringPos = "Right-below"; }
                                        else if (rows[79].Length > 2) { ringCode = rows[79]; ringPos = "Left-above"; }
                                        else if (rows[80].Length > 2) { ringCode = rows[80]; ringPos = "Right-above"; }
                                        else { ringCode = "noRing"; ringPos = "noRing"; }

                                        string result = Regex.Match(ringCode, @"\(([^)]*)\)").Groups[1].Value;

                                        _db.BirdBtos.Add(new BirdBto
                                        {
                                            MetalRing = rows[4],
                                            Sex = rows[12],
                                            Specie = rows[8],
                                            GridRef = rows[19],
                                            Latitude = null,
                                            Longitude = null,
                                            ColourRingCode = ringCode,
                                            ColourRingPosition = ringPos,

                                        });
                                    }
                                }
                                
                            }
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
