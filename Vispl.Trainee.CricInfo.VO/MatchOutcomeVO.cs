using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.CricInfo.VO
{
    public class MatchOutcomeVO
    {
        public int ResultID { get; set; }
        public int MatchID { get; set; }
        public int WinningTeamID { get; set; }
        public int LosingTeamID { get; set; }
        public string ResultDescription { get; set; }
    }
}
