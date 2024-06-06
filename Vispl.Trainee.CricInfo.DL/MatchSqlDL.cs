using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.CricInfo.DL.ITF;
using Vispl.Trainee.CricInfo.VO;

namespace Vispl.Trainee.CricInfo.DL
{
    public class MatchSqlDL : IMatchSqlDL
    {
        string connectionString = ConnectionStringManager.GetConnectionString();

        public List<MatchVO> ReadAllRecords()
        {
            List<MatchVO> records = new List<MatchVO>();

            string queryString = "SELECT * FROM Matches;";

            SqlConnection connection = new SqlConnection(connectionString);


            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                MatchVO record = new MatchVO
                {
                    MatchID = Convert.ToInt32(reader["MatchID"]),
                    Team1 = reader["Team1"].ToString(),
                    Team2 = reader["Team2"].ToString(),
                    MatchFormat = reader["MatchFormat"].ToString(),
                    MatchDateTimeZone = Convert.ToDateTime(reader["MatchDateTimeZone"]),
                    Venue = reader["Venue"].ToString()
                };

                records.Add(record);
            }
            reader.Close();
            ReleaseAndDispose(reader, command);

            return records;
        }


        public void AddRecord(MatchVO record)
        {
            string queryString = @"INSERT INTO Matches (Team1, Team2, MatchFormat, MatchDateTimeZone, Venue)
                           VALUES (@Team1, @Team2, @MatchFormat, @MatchDateTimeZone, @Venue);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                command.Parameters.AddWithValue("@Team1", record.Team1);
                command.Parameters.AddWithValue("@Team2", record.Team2);
                command.Parameters.AddWithValue("@MatchFormat", record.MatchFormat);
                if (record.MatchDateTimeZone == null || record.MatchDateTimeZone == DateTime.MinValue)
                {
                    command.Parameters.AddWithValue("@MatchDateTimeZone", DateTime.Now);
                }
                else
                {
                    command.Parameters.AddWithValue("@MatchDateTimeZone", record.MatchDateTimeZone);
                }

                command.Parameters.AddWithValue("@Venue", record.Venue);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public string[] GetTimezoneList()
        {
            List<string> timezones = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select TimeZone From TimeZones;";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string timezone = reader.GetString(0);
                        timezones.Add(timezone);
                    }
                }
            }
            return timezones.ToArray();
        }

        public List<TeamListVO> GetTeamList()
        {
            List<TeamListVO> teamList = new List<TeamListVO>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select TeamID, TeamName From Teams;";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TeamListVO team = new TeamListVO
                        {
                            TeamID = reader.GetInt32(0),
                            TeamName = reader.GetString(1)
                        };
                        teamList.Add(team);
                    }
                }
            }
            return teamList;
        }

        private void ReleaseAndDispose(SqlDataReader reader,SqlCommand command = null)
        {
            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }
            if (command != null)
            {
                command.Dispose();
                command = null;
            }
        }

    }
}
