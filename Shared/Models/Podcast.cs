using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Podcasts
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AudioUrl { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
