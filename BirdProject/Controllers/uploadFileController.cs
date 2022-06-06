using BirdProject.Model.ViewModel;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;

namespace BirdProject.Controllers
{
    public class uploadFileController : Controller
    {
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
                                while (!sr.EndOfStream)
                                {
                                    string[] rows = sr.ReadLine().Split(',');
                                    

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
