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
                        PlayerTypeFielder = Convert.ToBoolean(row["PlayerTypeFielder"]),
                        PlayerTypeBowler = Convert.ToBoolean(row["PlayerTypeBowler"]),
                        PlayerTypeBatsman = Convert.ToBoolean(row["PlayerTypeBatsman"]),
                        PlayerTypeAllRounder = Convert.ToBoolean(row["PlayerTypeAllRounder"]),
                        IsCaptain = Convert.ToBoolean(row["IsCaptain"]),
                        Nationality = row["Nationality"].ToString(),
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


        public void AddRecord(PlayerVO record)
        {
            string queryString = @"INSERT INTO Players (JerseyNo, Name, DateOfBirth, Age, BirthPlace, PlayerTypeFielder, PlayerTypeBowler, PlayerTypeBatsman, PlayerTypeAllRounder, IsCaptain, Nationality, TeamId, MatchesPlayed, RunsScored, WicketsTaken, BattingAverage, BowlingAverage, Centuries, HalfCenturies, DebutDate, ICCRanking, Picture)
                           VALUES (@JerseyNo, @Name, @DateOfBirth, @Age, @BirthPlace, @PlayerTypeFielder, @PlayerTypeBowler, @PlayerTypeBatsman, @PlayerTypeAllRounder, @IsCaptain, @Nationality, @TeamId, @MatchesPlayed, @RunsScored, @WicketsTaken, @BattingAverage, @BowlingAverage, @Centuries, @HalfCenturies, @DebutDate, @ICCRanking, @Picture);";

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
                    command.Parameters.AddWithValue("@PlayerTypeFielder", record.PlayerTypeFielder);
                    command.Parameters.AddWithValue("@PlayerTypeBowler", record.PlayerTypeBowler);
                    command.Parameters.AddWithValue("@PlayerTypeBatsman", record.PlayerTypeBatsman);
                    command.Parameters.AddWithValue("@PlayerTypeAllRounder", record.PlayerTypeAllRounder);
                    command.Parameters.AddWithValue("@IsCaptain", record.IsCaptain);
                    command.Parameters.AddWithValue("@Nationality", record.Nationality);
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
                    command.Parameters.AddWithValue("@Picture", record.Picture ?? (object)DBNull.Value);

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
                string query = "Select PlayerId, Name From Players;";

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
                string query = "Select PlayerId, Name From Players where IsCaptain = 1;";

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
