using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessHard.Models
{
    class ServiceForGrid
    {
        public string Title { get; set; } = null!;
        public int Count { get; set; }
        public int ServiceId { get; set; }
        public int ClientId { get; internal set; }
    }
}
