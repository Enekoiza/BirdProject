using System;
using System.Collections.Generic;

namespace BirdProject.Model
{
    public partial class Person
    {
        public Person()
        {
            SpotLogs = new HashSet<SpotLog>();
        }

        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;

        public virtual ICollection<SpotLog> SpotLogs { get; set; }
    }
}
