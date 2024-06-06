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
    public class MatchValidationBL : IMatchValidationBL
    {
        IMatchSqlDL MatchSqlDLObject;
        public List<MatchVO> ReadAllRecordsData()
        {
            MatchSqlDLObject = new MatchSqlDL();
            return MatchSqlDLObject.ReadAllRecords();
        }

        public void Save(MatchVO record)
        {
            MatchSqlDLObject = new MatchSqlDL();
            MatchSqlDLObject.AddRecord(record);
        }

        public string[] GetTimezones() 
        {
            MatchSqlDLObject = new MatchSqlDL();
            return MatchSqlDLObject.GetTimezoneList();
        }

        public List<TeamListVO> GetTeamNamesList()
        {
            MatchSqlDLObject = new MatchSqlDL();
            return MatchSqlDLObject.GetTeamList();
        }

        public void Dispose()
        {
            if (MatchSqlDLObject != null)
            {
                MatchSqlDLObject = null;
            }
        }
    }
}
