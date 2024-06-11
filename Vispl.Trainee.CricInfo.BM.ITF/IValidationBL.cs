using System.Collections.Generic;
using System.Data;
using Vispl.Trainee.CricInfo.VO;

namespace Vispl.Trainee.CricInfo.BM.ITF
{
    public interface IValidationBL
    {
        List<PlayerVO> ReadAllRecordsData();
        void Save(PlayerVO record);
        string[] GetNationality();
        List<PlayerListVO> GetPlayerNames();
        List<PlayerListVO> GetCaptainNames();
    }
}