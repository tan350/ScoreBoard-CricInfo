using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vispl.Trainee.CricInfo.VO;
using Vispl.Trainee.CricInfo.DL.ITF;
using static Vispl.Trainee.CricInfo.RES.PlayerRES;
using System.Data;
using System.Windows.Input;

namespace Vispl.Trainee.CricInfo.DL
{
    public class PlayerSqlDL :IPlayerSqlDL
    {
        private readonly string connectionString = ConnectionStringManager.GetConnectionString();

        public List<PlayerVO> ReadAllRecords()
        {
            DataTable dataTable = new DataTable();
            string queryString = "SELECT * FROM Players;";

            SqlConnection connection = new SqlConnection(connectionString); 
            SqlCommand command = new SqlCommand(queryString, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            var records = new List<PlayerVO>();
            try
            {
                connection.Open();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    var player = new PlayerVO
                    {
                        Picture = row["Picture"] as byte[],
                        JerseyNo = Convert.ToInt32(row["JerseyNo"]),
                        Name = row["Name"].ToString(),
                        DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                        Age = Convert.ToInt32(row["Age"]),
                        BirthPlace = row["BirthPlace"].ToString(),
                        PlayerType = row["PlayerType"] != DBNull.Value ? (int?)Convert.ToInt32(row["PlayerType"]) : null,
                        Role = row["Role"] != DBNull.Value ? (int?)Convert.ToInt32(row["Role"]) : null,
                        Nationality = row["Nationality"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["Nationality"]),
                        Team = row["TeamId"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["TeamId"]),
                        MatchesPlayed = Convert.ToInt32(row["MatchesPlayed"]),
                        RunsScored = Convert.ToInt32(row["RunsScored"]),
                        WicketsTaken = Convert.ToInt32(row["WicketsTaken"]),
                        BattingAverage = Convert.ToDouble(row["BattingAverage"]),
                        BowlingAverage = Convert.ToDouble(row["BowlingAverage"]),
                        Centuries = Convert.ToInt32(row["Centuries"]),
                        HalfCenturies = Convert.ToInt32(row["HalfCenturies"]),
                        DebutDate = Convert.ToDateTime(row["DebutDate"]),
                        ICCRanking = Convert.ToInt32(row["ICCRanking"])
                    };
                    records.Add(player);
                }
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                ReleaseAndDispose(adapter, dataTable, command);
            }
           
            return records;
        }

        public DataTable ReadAllRecordsDataTable()
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT * FROM PlayerDetails";
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


        public void AddRecord(PlayerVO record)
        {
            string queryString = @"INSERT INTO Players (JerseyNo, Name, DateOfBirth, Age, BirthPlace, PlayerType, Role, Nationality, TeamId, MatchesPlayed, RunsScored, WicketsTaken, BattingAverage, BowlingAverage, Centuries, HalfCenturies, DebutDate, ICCRanking, Picture)
                           VALUES (@JerseyNo, @Name, @DateOfBirth, @Age, @BirthPlace, @PlayerType, @Role, @Nationality, @TeamId, @MatchesPlayed, @RunsScored, @WicketsTaken, @BattingAverage, @BowlingAverage, @Centuries, @HalfCenturies, @DebutDate, @ICCRanking, @Picture);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    command.Parameters.AddWithValue("@JerseyNo", record.JerseyNo);
                    command.Parameters.AddWithValue("@Name", record.Name);
                    if (record.DateOfBirth == null || record.DateOfBirth == DateTime.MinValue)
                    {
                        command.Parameters.AddWithValue("@DateOfBirth", new DateTime(1900, 1, 1));
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@DateOfBirth", record.DateOfBirth);
                    }
                    command.Parameters.AddWithValue("@Age", record.Age);
                    command.Parameters.AddWithValue("@BirthPlace", record.BirthPlace);
                    command.Parameters.AddWithValue("@PlayerType", record.PlayerType);
                    if (record.Role == null)
                    {
                        command.Parameters.AddWithValue("@Role", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Role", record.Role);
                    }
                    if (record.Nationality == null)
                    {
                        command.Parameters.AddWithValue("@Nationality", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Nationality", record.Nationality);
                    }
                    if (record.Team == null)
                    {
                        command.Parameters.AddWithValue("@TeamId", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@TeamId", record.Team);
                    }

                    command.Parameters.AddWithValue("@MatchesPlayed", record.MatchesPlayed);
                    command.Parameters.AddWithValue("@RunsScored", record.RunsScored);
                    command.Parameters.AddWithValue("@WicketsTaken", record.WicketsTaken);
                    command.Parameters.AddWithValue("@BattingAverage", record.BattingAverage);
                    command.Parameters.AddWithValue("@BowlingAverage", record.BowlingAverage);
                    command.Parameters.AddWithValue("@Centuries", record.Centuries);
                    command.Parameters.AddWithValue("@HalfCenturies", record.HalfCenturies);
                    if (record.DebutDate == null || record.DebutDate == DateTime.MinValue)
                    {
                        command.Parameters.AddWithValue("@DebutDate", new DateTime(1900, 1, 1));
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@DebutDate", record.DebutDate);
                    }
                    command.Parameters.AddWithValue("@ICCRanking", record.ICCRanking);
                    SqlParameter pictureParameter = new SqlParameter("@Picture", SqlDbType.VarBinary, -1);
                    pictureParameter.Value = record.Picture ?? (object)DBNull.Value;
                    command.Parameters.Add(pictureParameter);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }


            }
        }

        public List<Dictionary<string, object>> GetNationalityWithID()
        {
            List<Dictionary<string, object>> nationalityList = new List<Dictionary<string, object>>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT NationalityID, NationalityName FROM Nationality;";

                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var nationality = new Dictionary<string, object>
                    {
                        { "NationalityID", reader.GetInt32(0) },
                        { "NationalityName", reader.GetString(1) }
                    };
                            nationalityList.Add(nationality);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return nationalityList;
        }


        public string[] GetNationalityList()
        {
            List<string> nationalityList = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select NationalityName From Nationality;";

                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nationality = reader.GetString(0);
                            nationalityList.Add(nationality);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return nationalityList.ToArray();
        }


        public List<PlayerListVO> GetPlayersNameList()
        {
            List<PlayerListVO> playerList = new List<PlayerListVO>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select PlayerId, Name From Players where TeamId Is Null;";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlayerListVO player = new PlayerListVO
                            {
                                PlayerId = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            };
                            playerList.Add(player);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return playerList;
        }

        public List<PlayerListVO> GetCaptainNameList()
        {
            List<PlayerListVO> captainList = new List<PlayerListVO>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select PlayerId, Name From Players where IsCaptain = 'Yes' AND TeamId Is Null;";

                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlayerListVO captain = new PlayerListVO
                            {
                                PlayerId = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            };
                            captainList.Add(captain);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return captainList;
        }

        public List<PlayerListVO> GetPlayersNameWithTeamID()
        {
            List<PlayerListVO> playerList = new List<PlayerListVO>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select TeamID, Name From Players Where TeamID Is NOT NULL;";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlayerListVO player = new PlayerListVO
                            {
                                PlayerId = reader.GetInt32(0), //TeamID stored in PlayerID
                                Name = reader.GetString(1)
                            };
                            playerList.Add(player);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return playerList;
        }

        public List<PlayerListVO> GetRoleNameWithRoleID()
        {
            List<PlayerListVO> playerList = new List<PlayerListVO>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select * From Role;";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlayerListVO role = new PlayerListVO
                            {
                                PlayerId = reader.GetInt32(0), //RoleID stored in PlayerID
                                Name = reader.GetString(1)     //RoleName stored in Name
                            };
                            playerList.Add(role);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return playerList;
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
