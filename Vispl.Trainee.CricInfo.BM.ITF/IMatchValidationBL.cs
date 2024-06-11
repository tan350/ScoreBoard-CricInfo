using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.CricInfo.VO;

namespace Vispl.Trainee.CricInfo.BM.ITF
{
    public interface IMatchValidationBL
    {
        List<MatchVO> ReadAllRecordsData();
        void Save(MatchVO record);
        string[] GetTimezones();
        List<TeamListVO> GetTeamNamesList();
        List<object> GetTimezonesList();
    }
}
