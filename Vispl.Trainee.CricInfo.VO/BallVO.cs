using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.CricInfo.VO
{
    public class BallVO
    {
        public int BallID { get; set; }
        public int InningID { get; set; }
        public int OverNumber { get; set; }
        public int BallNumber { get; set; }
        public int BowlerID { get; set; }
        public int BatsmanID { get; set; }
        public int RunsScored { get; set; }
        public int? WicketID { get; set; }
    }
}
