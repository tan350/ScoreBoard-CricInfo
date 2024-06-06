using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.CricInfo.VO;

namespace Vispl.Trainee.CricInfo.DL.ITF
{
    public interface ITeamSqlDL
    {
        void AddRecord(TeamVO record);
        List<TeamVO> ReadAllRecords();
    }
}
