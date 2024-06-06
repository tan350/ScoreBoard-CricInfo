using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Vispl.Trainee.CricInfo.RES.PlayerRES;
using Vispl.Trainee.CricInfo.DL.ITF;
using Vispl.Trainee.CricInfo.VO;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlTypes;

namespace Vispl.Trainee.CricInfo.DL
{
    public class TeamSqlDL : ITeamSqlDL
    {
        string connectionString = ConnectionStringManager.GetConnectionString();

        public DataTable ReadRecordsInDataTable()
        {
            DataTable dataTable = new DataTable();
            string queryString = "SELECT * FROM Teams;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);
                connection.Open();
                adapter.Fill(dataTable);
            }

            return dataTable;
        }

        private List<TeamVO> ConvertDataTableToTeamVOList(DataTable dataTable)
        {
            var TeamList = new List<TeamVO>();

            foreach (DataRow row in dataTable.Rows)
            {
                var team = new TeamVO
                {
                    TeamID = Convert.ToInt32(row["TeamID"]),
                    TeamName = row["TeamName"].ToString(),
                    TeamShortName = row["TeamShortName"].ToString(),
                    TeamMembers = row["TeamMembers"].ToString(),
                    Captain = row["Captain"].ToString(),
                    ViceCaptain = row["ViceCaptain"].ToString(),
                    WicketKeeper = row["WicketKeeper"].ToString(),
                    TeamIcon = row["TeamIcon"] as byte[]
                };
                TeamList.Add(team);
            }

            return TeamList;
        }

        public List<TeamVO> ReadAllRecords()
        {
            DataTable dataTable = new DataTable();
            string queryString = "SELECT * FROM Teams;";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(queryString, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            connection.Open();
            adapter.Fill(dataTable);

            var records = new List<TeamVO>();

            foreach (DataRow row in dataTable.Rows)
            {
                var team = new TeamVO
                {
                    TeamID = Convert.ToInt32(row["TeamID"]),
                    TeamName = row["TeamName"].ToString(),
                    TeamShortName = row["TeamShortName"].ToString(),
                    TeamMembers = row["TeamMembers"].ToString(),
                    Captain = row["Captain"].ToString(),
                    ViceCaptain = row["ViceCaptain"].ToString(),
                    WicketKeeper = row["WicketKeeper"].ToString(),
                    TeamIcon = row["TeamIcon"] as byte[]
                };
                records.Add(team);
            }
            ReleaseAndDispose(adapter, dataTable, command);

            return  records;
        }


        public void AddRecord(TeamVO record)
        {
            string players = string.Join(",", record.TeamList);
            string queryString1 = "SELECT MAX(TeamID) FROM Teams;";
            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using(var command = new SqlCommand(queryString1, connection))
                {
                     result = (int)command.ExecuteScalar();
                }
            }

            string queryString = @"INSERT INTO Teams (TeamName,TeamShortName, TeamMembers, Captain, ViceCaptain, WicketKeeper, TeamIcon)
                           VALUES (@TeamName,@TeamShortName, @TeamMembers, @Captain, @ViceCaptain, @WicketKeeper, @TeamIcon);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {

                    command.Parameters.AddWithValue("@TeamName", record.TeamName);
                    command.Parameters.AddWithValue("@TeamShortName", record.TeamShortName);
                    command.Parameters.AddWithValue("@TeamMembers", players);
                    command.Parameters.AddWithValue("@Captain", record.Captain);
                    command.Parameters.AddWithValue("@ViceCaptain", record.ViceCaptain);
                    command.Parameters.AddWithValue("@WicketKeeper", record.WicketKeeper);
                    if (record.TeamIcon != null && record.TeamIcon.Length > 1048576)
                    {
                        throw new ArgumentException("TeamIcon exceeds the maximum allowed size of 1 MB.");
                    }
                    command.Parameters.Add("@TeamIcon", SqlDbType.VarBinary, -1).Value = (object)record.TeamIcon ?? DBNull.Value;

                    command.ExecuteNonQuery();
                }

                foreach (var individualplayer in record.TeamList)
                {
                    using (SqlCommand commandPlayer = new SqlCommand(@"UPDATE Players SET TeamId = @TeamId WHERE PlayerId = @PlayerId", connection))
                    {
                        commandPlayer.Parameters.AddWithValue("@TeamId", result+1);
                        commandPlayer.Parameters.AddWithValue("@PlayerId", individualplayer);
                        commandPlayer.ExecuteNonQuery();
                    }
                }


            }
        }

        private void ReleaseAndDispose(SqlDataAdapter adapter, DataTable dataTable, SqlCommand command = null)
        {
            if (dataTable != null)
            {
                dataTable.Dispose();
                dataTable = null;
            }
            if (adapter != null)
            {
                adapter.Dispose();
                adapter = null;
            }
            if (command != null)
            {
                command.Dispose();
                command = null;
            }
        }

    }
}
