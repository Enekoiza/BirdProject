using System;
using System.Collections.Generic;

namespace BirdProject.Model
{
    public partial class BirdBto
    {
        public BirdBto()
        {
            SpotLogs = new HashSet<SpotLog>();
        }

        public string MetalRing { get; set; } = null!;
        public string Sex { get; set; } = null!;
        public string Specie { get; set; } = null!;
        public string? ColourRingCode { get; set; }
        public string? ColourRingColour { get; set; }
        public string? ColourRingPosition { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string? GridRef { get; set; }

        public virtual ICollection<SpotLog> SpotLogs { get; set; }
    }
}
