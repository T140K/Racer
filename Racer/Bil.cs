using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Racer
{
    public class Car
    {
        public int? id { get; set; }

        public string? name { get; set; }

        public decimal speed { get; set; }

        public decimal distanceSoFar { get; set; }

        public decimal distanceLeft { get; set; }

        public decimal timeSoFar { get; set; }

        public int delay { get; set; }

        public decimal RemainingTime()
        {
            return distanceLeft / (speed / 3.6m);
        }
    }
}
