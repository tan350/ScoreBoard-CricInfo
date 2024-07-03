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
            try
            {
                PlayerSqlDLObject = new PlayerSqlDL();
                return PlayerSqlDLObject.ReadAllRecords();
            }
            finally
            {
                PlayerSqlDLObject = null;
            }
        }

        public DataTable ReadAllRecordsDataTable()
        {
            try
            {
                PlayerSqlDLObject = new PlayerSqlDL();
                return PlayerSqlDLObject.ReadAllRecordsDataTable();
            }
            finally
            {
                PlayerSqlDLObject = null;
            }
        }

        public void Save(PlayerVO record)
        {
            try
            {
                PlayerSqlDLObject = new PlayerSqlDL();
                PlayerSqlDLObject.AddRecord(record);
            }
            finally
            {
                PlayerSqlDLObject = null;
            }
        }

        public List<Dictionary<string, object>> GetNationalityWithID()
        {
            try
            {
                PlayerSqlDLObject = new PlayerSqlDL();
                return PlayerSqlDLObject.GetNationalityWithID();
            }
            finally
            {
                PlayerSqlDLObject = null;
            }
        }

        public string[] GetNationality()
        {
            try
            {
                PlayerSqlDLObject = new PlayerSqlDL();
                return PlayerSqlDLObject.GetNationalityList();
            }
            finally
            {
                PlayerSqlDLObject = null;
            }
        }

        public List<PlayerListVO> GetPlayerNames()
        {
            try
            {
                PlayerSqlDLObject = new PlayerSqlDL();
                return PlayerSqlDLObject.GetPlayersNameList();
            }
            finally
            {
                PlayerSqlDLObject = null;
            }
        }

        public List<PlayerListVO> GetPlayerNamesWithTeamID()
        {
            try
            {
                PlayerSqlDLObject = new PlayerSqlDL();
                return PlayerSqlDLObject.GetPlayersNameWithTeamID();
            }
            finally
            {
                PlayerSqlDLObject = null;
            }
        }

        public List<PlayerListVO> GetCaptainNames()
        {
            try
            {
                PlayerSqlDLObject = new PlayerSqlDL();
                return PlayerSqlDLObject.GetCaptainNameList();
            }
            finally
            {
                PlayerSqlDLObject = null;
            }
        }

        public List<PlayerListVO> GetRoleNameWithRoleID()
        {
            try
            {
                PlayerSqlDLObject = new PlayerSqlDL();
                return PlayerSqlDLObject.GetRoleNameWithRoleID();
            }
            finally
            {
                PlayerSqlDLObject = null;
            }
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
