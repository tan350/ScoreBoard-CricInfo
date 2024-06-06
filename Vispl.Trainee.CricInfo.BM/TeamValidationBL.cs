using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.CricInfo.BM.ITF;
using Vispl.Trainee.CricInfo.DL;
using Vispl.Trainee.CricInfo.DL.ITF;
using Vispl.Trainee.CricInfo.VO;

namespace Vispl.Trainee.CricInfo.BM
{
    public class TeamValidationBL : ITeamValidationBL
    {
        ITeamSqlDL TeamSqlDLObject;
        public List<TeamVO> ReadAllRecordsData()
        {
            TeamSqlDLObject = new TeamSqlDL();
            return TeamSqlDLObject.ReadAllRecords();
        }

        public void Save(TeamVO record)
        {
            TeamSqlDLObject = new TeamSqlDL();
            TeamSqlDLObject.AddRecord(record);
        }

        public void Dispose()
        {
            if (TeamSqlDLObject != null)
            {
                TeamSqlDLObject = null;
            }
        }
    }
}
