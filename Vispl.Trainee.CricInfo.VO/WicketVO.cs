using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.CricInfo.VO
{
    public class WicketVO
    {
        public int MatchId { get; set; }

        public int BatsmanId { get; set; }

        public int BowlerId { get; set; }

        public int? FielderId { get; set; }

        public int WicketTypeId { get; set; }

        public string DescriptionWicket { get; set; }
    }

}
