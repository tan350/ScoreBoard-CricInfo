using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
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

        public void SaveToss(TossVO toss)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                MatchSqlDLObject.AddToss(toss);
            }
            finally
            {
                MatchSqlDLObject = null;
            }
        }


        public List<MatchVO> GetFilteredMatches(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                return MatchSqlDLObject.GetMatchesBetweenDates(fromDate, toDate);
            }
            finally
            {
                MatchSqlDLObject = null;
            }
        }

        public List<MatchVO> GetTodayMatch(DateTime startDate, DateTime endDate)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                return MatchSqlDLObject.GetTodayDateMatch(startDate, endDate);
            }
            finally
            {
                MatchSqlDLObject = null;
            }
        }

        public List<Dictionary<string, object>> GetMatchesByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                return MatchSqlDLObject.GetMatchesByDateRange(startDate, endDate);
            }
            finally
            {
                MatchSqlDLObject = null;
            }
        }

        public List<Dictionary<string, object>> GetPlayersByTeamID(int teamID)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                return MatchSqlDLObject.GetPlayersByTeamID(teamID);
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

        public DateTimeOffset ConvertToOffSet(DateTime MatchDateTimeZone, string MatchOffset) 
        {
            string offsetString = MatchOffset;
            string[] offsetParts = offsetString.Split(':');

            if (offsetParts.Length != 2)
            {
                throw new ArgumentException("Invalid offset format", nameof(MatchOffset));
            }

            int hours, minutes;

            if (!int.TryParse(offsetParts[0], out hours) || !int.TryParse(offsetParts[1], out minutes))
            {
                throw new ArgumentException("Invalid offset format", nameof(MatchOffset));
            }
            TimeSpan offset = new TimeSpan(hours, minutes, 0);

            DateTimeOffset matchDateTimeOffset = new DateTimeOffset(MatchDateTimeZone, offset);

            return matchDateTimeOffset;
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

        public MatchVO GetMatchByID(int matchId)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                return MatchSqlDLObject.GetMatchByID(matchId);
            }
            finally
            {
                MatchSqlDLObject = null;
            }
        }


        public Dictionary<string, object> GetMatchListByID(int matchId)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                return MatchSqlDLObject.GetMatchListByID(matchId);
            }
            finally
            {
                MatchSqlDLObject = null;
            }
        }


        public List<Dictionary<string, object>> GetWicketTypes()
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                return MatchSqlDLObject.GetWicketTypes();
            }
            finally
            {
                MatchSqlDLObject = null;
            }
        }

        public List<Dictionary<string, object>> GetPlayersByTeamIDAndPlayerType(int teamID, int playerType)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                return MatchSqlDLObject.GetPlayersByTeamIDAndPlayerType(teamID, playerType);
            }
            finally
            {
                MatchSqlDLObject = null;
            }
        }

        public List<PlayerListVO> GetBattingOrderPlayers(int matchId, int teamId)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                return MatchSqlDLObject.GetBattingOrderPlayers(matchId, teamId);
            }
            finally
            {
                MatchSqlDLObject = null;
            }
        }

        public void SaveWicketData(WicketVO wicket)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                MatchSqlDLObject.SaveWicketData(wicket);
            }
            finally
            {
                MatchSqlDLObject = null;
            }
        }

        public List<WicketVO> GetWicketsByMatchId(int matchId)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                return MatchSqlDLObject.GetWicketsByMatchId(matchId);
            }
            finally
            {
                MatchSqlDLObject = null;
            }
        }

        public void SaveBattingOrder(List<int> team1PlayerIds, List<int> team2PlayerIds, int matchId, int team1Id, int team2Id)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                MatchSqlDLObject.SaveBattingOrder(team1PlayerIds, team2PlayerIds, matchId, team1Id, team2Id);
            }
            finally
            {
                MatchSqlDLObject = null;
            }
        }

        public bool UpdateBattingStatistics(int playerOnStrikeId, int runs, int balls, int fours, int sixes, out string errorMessage)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                return MatchSqlDLObject.UpdateBattingStatistics(playerOnStrikeId, runs, balls, fours, sixes, out errorMessage);
            }
            finally
            {
                    MatchSqlDLObject = null;
            }
            
        }

        public void SaveMatchInning(MatchInningVO model)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                MatchSqlDLObject.SaveMatchInning(model);
            }
            finally
            {
                MatchSqlDLObject = null;
            }

        }

        public List<BattingVO> GetAllBatting()
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                return MatchSqlDLObject.GetAllBatting();
            }
            finally
            {
                MatchSqlDLObject = null;
            }

        }

        public void UpdateFallOfWicket(WicketVO model)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                MatchSqlDLObject.UpdateFallOfWicket(model);
            }
            finally
            {
                MatchSqlDLObject = null;
            }

        }

        public void UpdateMatchInning(MatchInningVO model)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                MatchSqlDLObject.UpdateMatchInning(model);
            }
            finally
            {
                MatchSqlDLObject = null;
            }

        }

        public void UpdateBall(BallVO model)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                MatchSqlDLObject.UpdateBall(model);
            }
            finally
            {
                MatchSqlDLObject = null;
            }

        }

        public void UpdateBowling(BowlingVO model)
        {
            try
            {
                MatchSqlDLObject = new MatchSqlDL();
                MatchSqlDLObject.UpdateBowling(model);
            }
            finally
            {
                MatchSqlDLObject = null;
            }

        }
    }
}
