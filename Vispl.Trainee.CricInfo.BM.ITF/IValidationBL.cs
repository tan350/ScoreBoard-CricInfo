using System.Collections.Generic;
using System.Data;
using Vispl.Trainee.CricInfo.VO;

namespace Vispl.Trainee.CricInfo.BM.ITF
{
    public interface IValidationBL
    {
        List<PlayerVO> ReadAllRecordsData();
        DataTable ReadAllRecordsDataTable();
        void Save(PlayerVO record);
        List<Dictionary<string, object>> GetNationalityWithID();
        string[] GetNationality();
        List<PlayerListVO> GetPlayerNames();
        List<PlayerListVO> GetCaptainNames();
        List<PlayerListVO> GetPlayerNamesWithTeamID();
        List<PlayerListVO> GetRoleNameWithRoleID();
    }
}