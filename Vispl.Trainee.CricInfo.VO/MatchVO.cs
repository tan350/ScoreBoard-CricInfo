using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.CricInfo.VO
{
    public class MatchVO
    {
        public int MatchID { get; set; }
        public string Team1 { get; set; }
        public string Team2 { get; set; }
        public string MatchFormat { get; set; }
        public DateTime MatchDateTimeZone { get; set; }
        public string Venue { get; set; }
    }
}
