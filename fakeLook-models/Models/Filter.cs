using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fakeLook_models.Models
{
    public class Filter
    {
        public ICollection<string> Publishers { get; set; }
        public DateTime? startingDate { get; set; } = null;
        public DateTime? endingDate { get; set; } = null;
        public ICollection<string> tags { get; set; }
        public ICollection<string> taggedUsers { get; set; }
    }
}
