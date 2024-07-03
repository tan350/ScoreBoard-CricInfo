using System.Collections.Generic;
using System.Data;
using Vispl.Trainee.CricInfo.VO;

namespace Vispl.Trainee.CricInfo.DL.ITF
{
    public interface IPlayerSqlDL
    {
        void AddRecord(PlayerVO record);
        List<PlayerVO> ReadAllRecords();
        DataTable ReadAllRecordsDataTable();
        List<Dictionary<string, object>> GetNationalityWithID();
        string[] GetNationalityList();
        List<PlayerListVO> GetPlayersNameList();
        List<PlayerListVO> GetCaptainNameList();
        List<PlayerListVO> GetPlayersNameWithTeamID();
        List<PlayerListVO> GetRoleNameWithRoleID();
    }
}