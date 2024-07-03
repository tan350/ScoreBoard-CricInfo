using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.CricInfo.VO
{
    public class MatchVO
    {
        public int MatchID { get; set; }
        public int Team1 { get; set; }
        public int Team2 { get; set; }
        public string MatchFormat { get; set; }
        public DateTime MatchDateTimeZone { get; set; }
        public string MatchOffset { get; set; }
        public string Venue { get; set; }
    }
}
