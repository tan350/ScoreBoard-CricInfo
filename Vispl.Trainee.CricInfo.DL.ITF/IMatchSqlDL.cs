using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Vispl.Trainee.CricInfo.VO;

namespace Vispl.Trainee.CricInfo.DL.ITF
{
    public interface IMatchSqlDL
    {
        void AddRecord(MatchVO record);
        void AddToss(TossVO toss);
        List<MatchVO> ReadAllRecords();
        string[] GetTimezoneList();
        List<TeamListVO> GetTeamList();
        DataTable GetZones();
        List<MatchVO> GetMatchesBetweenDates(DateTimeOffset fromDate, DateTimeOffset toDate);
        List<MatchVO> GetTodayDateMatch(DateTime startDate, DateTime endDate);
        List<Dictionary<string, object>> GetMatchesByDateRange(DateTime startDate, DateTime endDate);
        MatchVO GetMatchByID(int matchId);
        Dictionary<string, object> GetMatchListByID(int matchId);
        List<Dictionary<string, object>> GetPlayersByTeamID(int teamID);
        List<PlayerListVO> GetBattingOrderPlayers(int matchId, int teamId);
        List<Dictionary<string, object>> GetWicketTypes();
        List<Dictionary<string, object>> GetPlayersByTeamIDAndPlayerType(int teamID, int playerType);
        void SaveWicketData(WicketVO wicket);
        List<WicketVO> GetWicketsByMatchId(int matchId);
        void SaveBattingOrder(List<int> team1PlayerIds, List<int> team2PlayerIds, int matchId, int team1Id, int team2Id);
        bool UpdateBattingStatistics(int playerOnStrikeId, int runs, int balls, int fours, int sixes, out string errorMessage);
        List<BattingVO> GetAllBatting();
        void SaveMatchInning(MatchInningVO model);
        void UpdateFallOfWicket(WicketVO model);
        void UpdateMatchInning(MatchInningVO model);
        void UpdateBall(BallVO model);
        void UpdateBowling(BowlingVO model);
    }
}
