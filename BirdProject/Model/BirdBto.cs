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
        public string? Sex { get; set; }
        public string? Specie { get; set; }
        public string? MetalPos { get; set; }
        public string ColourPos { get; set; } = null!;
        public string ColourRing { get; set; } = null!;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public virtual ICollection<SpotLog> SpotLogs { get; set; }
    }
}
