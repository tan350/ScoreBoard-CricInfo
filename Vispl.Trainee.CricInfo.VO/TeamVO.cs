using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.CricInfo.VO
{
    public class TeamVO
    {
        public int TeamID { get; set; }

        [Required(ErrorMessage = "Team name is required.")]
        public string TeamName { get; set; }

        [Required(ErrorMessage = "Team short name is required.")]
        public string TeamShortName { get; set; }

        [Required(ErrorMessage = "Team members are required.")]
        public List<int> TeamList { get; set; }

        [Required(ErrorMessage = "Captain is required.")]
        public int Captain { get; set; }
        public int ViceCaptain { get; set; }

        [Required(ErrorMessage = "Wicket-keeper is required.")]
        public int WicketKeeper { get; set; }

        public byte[] TeamIcon { get; set; }
    }

    public class TeamListVO
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
    }
}
