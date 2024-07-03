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

        public List<TeamVO> ReadAllRecords()
        {
            DataTable dataTable = new DataTable();
            string queryString = "SELECT * FROM Teams;";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(queryString, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            var records = new List<TeamVO>();
            try
            {
                connection.Open();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    var team = new TeamVO
                    {
                        TeamID = Convert.ToInt32(row["TeamID"]),
                        TeamName = row["TeamName"].ToString(),
                        TeamShortName = row["TeamShortName"].ToString(),
                        /*TeamMembers = row["TeamMembers"].ToString(),
                        Captain = row["Captain"].ToString(),
                        ViceCaptain = row["ViceCaptain"].ToString(),
                        WicketKeeper = row["WicketKeeper"].ToString(),*/
                        TeamIcon = row["TeamIcon"] as byte[]
                    };
                    records.Add(team);
                }
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                adapter.Dispose();
                ReleaseAndDispose(adapter, dataTable, command);
            }

            return  records;
        }

        public DataTable ReadAllRecordsDataTable()
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT * FROM TeamDetails;";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            try
            {
                connection.Open();
                adapter.Fill(dataTable);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                ReleaseAndDispose(adapter, dataTable, command);
            }
            return dataTable;
        }

        public void AddRecord(TeamVO record)
        {
            int maxTeamId = GetMaxTeamId();
            int newTeamId = maxTeamId + 1;

            InsertTeamRecord(record);

            UpdatePlayerTeamIds(record.TeamList, newTeamId);

            UpdatePlayerRoles(record.Captain, record.ViceCaptain, record.WicketKeeper);
        }

        private int GetMaxTeamId()
        {
            string queryString = "SELECT MAX(TeamID) FROM Teams;";
            int maxTeamId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        maxTeamId = Convert.ToInt32(result);
                    }
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return maxTeamId;
        }

        private void InsertTeamRecord(TeamVO record)
        {
            string queryString = @"INSERT INTO Teams (TeamName, TeamShortName, TeamIcon)
                           VALUES (@TeamName, @TeamShortName, @TeamIcon);";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@TeamName", record.TeamName);
                    command.Parameters.AddWithValue("@TeamShortName", record.TeamShortName);
                    if (record.TeamIcon != null && record.TeamIcon.Length > 1048576)
                    {
                        throw new ArgumentException("TeamIcon exceeds the maximum allowed size of 1 MB.");
                    }
                    command.Parameters.Add("@TeamIcon", SqlDbType.VarBinary, -1).Value = (object)record.TeamIcon ?? DBNull.Value;
                    command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        private void UpdatePlayerTeamIds(List<int> teamList, int teamId)
        {
            string queryString = @"UPDATE Players SET TeamId = @TeamId WHERE PlayerId = @PlayerId";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    foreach (var playerId in teamList)
                    {
                        using (SqlCommand command = new SqlCommand(queryString, connection))
                        {
                            command.Parameters.AddWithValue("@TeamId", teamId);
                            command.Parameters.AddWithValue("@PlayerId", playerId);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        private void UpdatePlayerRoles(int captain, int viceCaptain, int wicketKeeper)
        {
            string queryString = @"UPDATE Players
                           SET Role = CASE
                               WHEN PlayerID = @Captain THEN 1
                               WHEN PlayerID = @ViceCaptain THEN 2
                               WHEN PlayerID = @WicketKeeper THEN 3
                           END
                           WHERE PlayerID IN (@Captain, @ViceCaptain, @WicketKeeper)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Captain", captain);
                    command.Parameters.AddWithValue("@ViceCaptain", viceCaptain);
                    command.Parameters.AddWithValue("@WicketKeeper", wicketKeeper);
                    command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    connection.Dispose();
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
