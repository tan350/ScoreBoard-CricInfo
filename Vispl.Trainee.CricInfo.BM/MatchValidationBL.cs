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
    public class MatchValidationBL : IMatchValidationBL
    {
        IMatchSqlDL MatchSqlDLObject;
        public List<MatchVO> ReadAllRecordsData()
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                return MatchSqlDLObject.ReadAllRecords();
            }
            finally
            {
                MatchSqlDLObject = null;
            }
        }

        public void Save(MatchVO record)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                MatchSqlDLObject.AddRecord(record);
            }
            finally
            {
                MatchSqlDLObject = null;
            }
        }

        public string[] GetTimezones() 
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                return MatchSqlDLObject.GetTimezoneList();
            }
            finally
            {
                MatchSqlDLObject = null;
            }
        }

        public List<object> GetTimezonesList()
        {
            MatchSqlDLObject = new MatchSqlDL();

            using (DataTable table = MatchSqlDLObject.GetZones())
            {
                try
                {
                    List<object> details = new List<object>();
                    string name;
                    string id;
                    foreach (DataRow row in table.Rows)
                    {
                        name = row["TimeZone"].ToString();
                        id = row["Offset"].ToString();

                        details.Add(new { Text = name + " " + id, value = id });
                    }
                    return details;
                }
                finally
                {
                    MatchSqlDLObject = null;
                }
            }

        }

        public List<TeamListVO> GetTeamNamesList()
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                return MatchSqlDLObject.GetTeamList();
            }
            finally
            {
                MatchSqlDLObject = null;
            }
        }
    }
}
