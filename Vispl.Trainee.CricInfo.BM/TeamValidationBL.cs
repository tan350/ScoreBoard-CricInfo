using System;
using System.Collections.Generic;
using System.Data;
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
            try
            {
                TeamSqlDLObject = new TeamSqlDL();
                return TeamSqlDLObject.ReadAllRecords();
            }
            finally
            {
                TeamSqlDLObject = null;
            }
        }

        public DataTable ReadAllRecordsDataTable()
        {
            try
            {
                TeamSqlDLObject = new TeamSqlDL();
                return TeamSqlDLObject.ReadAllRecordsDataTable();
            }
            finally
            {
                TeamSqlDLObject = null;
            }
        }
        public void Save(TeamVO record)
        {
            try
            {
                TeamSqlDLObject = new TeamSqlDL();
                TeamSqlDLObject.AddRecord(record);
            }
            finally
            {
                TeamSqlDLObject = null;
            }
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
