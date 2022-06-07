using BirdProject.Model;
using BirdProject.Model.ViewModel;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;

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
                                    if (!exists)
                                    {
                                        _db.BirdBtos.Add(new BirdBto
                                        {

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
