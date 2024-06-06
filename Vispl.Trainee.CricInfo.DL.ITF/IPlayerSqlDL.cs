using System.Collections.Generic;
using System.Data;
using Vispl.Trainee.CricInfo.VO;

namespace Vispl.Trainee.CricInfo.DL.ITF
{
    public interface IPlayerSqlDL
    {
        void AddRecord(PlayerVO record);
        List<PlayerVO> ReadAllRecords();
        string[] GetNationalityList();
        List<PlayerListVO> GetPlayersNameList();
        List<PlayerListVO> GetCaptainNameList();
    }
}