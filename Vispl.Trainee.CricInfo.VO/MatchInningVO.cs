using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.CricInfo.VO
{
    public class MatchInningVO
    {
        public int InningID { get; set; }
        public int MatchID { get; set; }
        public int InningNumber { get; set; }
        public int BattingTeamID { get; set; }
        public int BowlingTeamID { get; set; }
        public int RunsScored { get; set; }
        public int WicketsLost { get; set; }
        public decimal OversBowled { get; set; }
    }
}
