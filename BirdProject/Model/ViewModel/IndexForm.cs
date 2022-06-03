using System.Web;

namespace BirdProject.Model.ViewModel
{
    public class IndexForm
    {
        public string colourRingCode { get; set; }

        public string ringColour { get; set; }

        public DateTime date { get; set; }

        public string status { get; set; }

        public string specie { get; set; }

        public string? comments { get; set; }

        public float latitude { get; set; }

        public float longitude { get; set; }

        public IFormFile? birdPhoto { get; set; }
    }
}
