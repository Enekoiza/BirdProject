using System;
using System.Collections.Generic;

namespace BirdProject.Model
{
    public partial class SpotLog
    {
        public int SpotId { get; set; }
        public DateTime? Date { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string? MetalRing { get; set; }
        public string? Email { get; set; }
        public string? GridRef { get; set; }

        public virtual Person? EmailNavigation { get; set; }
        public virtual BirdBto? MetalRingNavigation { get; set; }
    }
}
