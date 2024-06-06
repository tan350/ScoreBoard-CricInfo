using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Vispl.Trainee.CricInfo.VO;
using Vispl.Trainee.CricInfo.DL;
using Vispl.Trainee.CricInfo.DL.ITF;
using Vispl.Trainee.CricInfo.BM.ITF;

namespace Vispl.Trainee.CricInfo.BM
{
    public class ValidationBL : IValidationBL
    {
        IPlayerSqlDL PlayerSqlDLObject;

        public List<PlayerVO> ReadAllRecordsData()
        {
            PlayerSqlDLObject = new PlayerSqlDL();
            return PlayerSqlDLObject.ReadAllRecords();
        }

        public void Save(PlayerVO record)
        {
            PlayerSqlDLObject = new PlayerSqlDL();
            PlayerSqlDLObject.AddRecord(record);
        }

        public string[] GetNationality()
        {
            PlayerSqlDLObject = new PlayerSqlDL();
            return PlayerSqlDLObject.GetNationalityList();
        }

        public List<PlayerListVO> GetPlayerNames()
        {
            PlayerSqlDLObject = new PlayerSqlDL();
            return PlayerSqlDLObject.GetPlayersNameList();
        }
        public List<PlayerListVO> GetCaptainNames()
        {
            PlayerSqlDLObject = new PlayerSqlDL();
            return PlayerSqlDLObject.GetCaptainNameList();
        }

        
        public void Dispose()
        {
            if (PlayerSqlDLObject != null)
            {
                PlayerSqlDLObject = null;
            }
        }

    }
}
