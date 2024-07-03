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
        void SaveToss(TossVO toss);
        string[] GetTimezones();
        List<TeamListVO> GetTeamNamesList();
        List<object> GetTimezonesList();
        DateTimeOffset ConvertToOffSet(DateTime MatchDateTimeZone, string MatchOffset);
        List<MatchVO> GetFilteredMatches(DateTimeOffset fromDate, DateTimeOffset toDate);
        List<MatchVO> GetTodayMatch(DateTime startDate, DateTime endDate);
        List<Dictionary<string, object>> GetMatchesByDateRange(DateTime startDate, DateTime endDate);
        MatchVO GetMatchByID(int matchId);
        Dictionary<string, object> GetMatchListByID(int matchId);
        List<Dictionary<string, object>> GetPlayersByTeamID(int teamID);
        List<Dictionary<string, object>> GetWicketTypes();
        List<Dictionary<string, object>> GetPlayersByTeamIDAndPlayerType(int teamID, int playerType);
        void SaveWicketData(WicketVO wicket);
        List<WicketVO> GetWicketsByMatchId(int matchId);
        void SaveBattingOrder(List<int> team1PlayerIds, List<int> team2PlayerIds, int matchId, int team1Id, int team2Id);
    }
}
