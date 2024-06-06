using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.CricInfo.VO;

namespace Vispl.Trainee.CricInfo.DL.ITF
{
    public interface IMatchSqlDL
    {
        void AddRecord(MatchVO record);
        List<MatchVO> ReadAllRecords();
        string[] GetTimezoneList();
        List<TeamListVO> GetTeamList();
    }
}
