using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.CricInfo.VO
{
    public class TossVO
    {
        public int TossID { get; set; }
        public int MatchID { get; set; }
        public int TossWonBy { get; set; }          // TeamID of the team that won the toss
        public string TossDecision { get; set; }    // 'batting' or 'bowling'

        public MatchVO Match { get; set; }
        public TeamVO Team { get; set; }
    }
}
