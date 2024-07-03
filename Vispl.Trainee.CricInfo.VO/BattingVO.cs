using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.CricInfo.VO
{
    public class BattingVO
    {
        public int BattingId { get; set; }
        public int PlayerId { get; set; }
        public int MatchId { get; set; }
        public int Runs { get; set; }
        public int Balls { get; set; }
        public int Fours { get; set; }
        public int Sixes { get; set; }
        public decimal StrikeRate { get; set; }
    }
    
}
