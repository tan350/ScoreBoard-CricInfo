using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.CricInfo.VO
{
    public class BowlingVO
    {
        public int BowlingID { get; set; }
        public int BowlerID { get; set; }
        public int TeamID { get; set; }
        public int MatchID { get; set; }
        public decimal TotalOver { get; set; }
        public int RunsScored { get; set; }
        public int Maiden { get; set; }
        public int? WicketID { get; set; }
        public decimal ECO { get; set; }
    }
}
