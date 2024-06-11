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
                        TeamMembers = row["TeamMembers"].ToString(),
                        Captain = row["Captain"].ToString(),
                        ViceCaptain = row["ViceCaptain"].ToString(),
                        WicketKeeper = row["WicketKeeper"].ToString(),
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


        public void AddRecord(TeamVO record)
        {
            string queryString1 = "SELECT MAX(TeamID) FROM Teams;";
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand(queryString1, connection))
                    {
                        var scalarResult = command.ExecuteScalar();
                        if (scalarResult != DBNull.Value && scalarResult != null)
                        {
                            result = Convert.ToInt32(scalarResult);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }


            string players = string.Join(",", record.TeamList);
            string query = $"SELECT Name FROM Players WHERE PlayerId IN ({players})";

            List<string> playerNames = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                playerNames.Add(reader.GetString(0));
                            }
                        }
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }

            string playersNameJoin = string.Join(",", playerNames);



            string queryString = @"INSERT INTO Teams (TeamName,TeamShortName, TeamMembers, Captain, ViceCaptain, WicketKeeper, TeamIcon)
                           VALUES (@TeamName,@TeamShortName, @TeamMembers, @Captain, @ViceCaptain, @WicketKeeper, @TeamIcon);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();

                    command.Parameters.AddWithValue("@TeamName", record.TeamName);
                    command.Parameters.AddWithValue("@TeamShortName", record.TeamShortName);
                    command.Parameters.AddWithValue("@TeamMembers", playersNameJoin);
                    command.Parameters.AddWithValue("@Captain", record.Captain);
                    command.Parameters.AddWithValue("@ViceCaptain", record.ViceCaptain);
                    command.Parameters.AddWithValue("@WicketKeeper", record.WicketKeeper);
                    if (record.TeamIcon != null && record.TeamIcon.Length > 1048576)
                    {
                        throw new ArgumentException("TeamIcon exceeds the maximum allowed size of 1 MB.");
                    }
                    command.Parameters.Add("@TeamIcon", SqlDbType.VarBinary, -1).Value = (object)record.TeamIcon ?? DBNull.Value;

                    command.ExecuteNonQuery();

                    foreach (var individualplayer in record.TeamList)
                    {
                        using (SqlCommand commandPlayer = new SqlCommand(@"UPDATE Players SET TeamId = @TeamId WHERE PlayerId = @PlayerId", connection))
                        {
                            commandPlayer.Parameters.AddWithValue("@TeamId", result + 1);
                            commandPlayer.Parameters.AddWithValue("@PlayerId", individualplayer);
                            commandPlayer.ExecuteNonQuery();
                        }
                    }
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
