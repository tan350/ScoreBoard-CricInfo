using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
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
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    MatchVO record = new MatchVO
                    {
                        MatchID = Convert.ToInt32(reader["MatchID"]),
                        Team1 = Convert.ToInt32(reader["Team1"]),
                        Team2 = Convert.ToInt32(reader["Team2"]),
                        MatchFormat = reader["MatchFormat"].ToString(),
                        MatchDateTimeZone = ((DateTimeOffset)reader["MatchDateTimeZone"]).DateTime,
                        Venue = reader["Venue"].ToString()
                    };

                    records.Add(record);
                }
                reader.Close();
                ReleaseAndDispose(reader, command);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return records;
        }


        public void AddRecord(MatchVO record)
        {
            string queryString = @"INSERT INTO Matches (Team1, Team2, MatchFormat, MatchDateTimeZone, Venue)
                           VALUES (@Team1, @Team2, @MatchFormat, @MatchDateTimeZone, @Venue);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    command.Parameters.AddWithValue("@Team1", record.Team1);
                    command.Parameters.AddWithValue("@Team2", record.Team2);
                    command.Parameters.AddWithValue("@MatchFormat", record.MatchFormat);
                    DateTimeOffset matchDateTimeOffset;
                    if (record.MatchDateTimeZone == null || record.MatchDateTimeZone == DateTime.MinValue)
                    {
                        matchDateTimeOffset = DateTimeOffset.Now;
                    }
                    else
                    {
                        string offsetString = record.MatchOffset;
                        string[] offsetParts = offsetString.Split(':');

                        if (offsetParts.Length != 2)
                        {
                            throw new ArgumentException("Invalid offset format", nameof(record.MatchOffset));
                        }

                        int hours, minutes;

                        if (!int.TryParse(offsetParts[0], out hours) || !int.TryParse(offsetParts[1], out minutes))
                        {
                            throw new ArgumentException("Invalid offset format", nameof(record.MatchOffset));
                        }
                        TimeSpan offset = new TimeSpan(hours, minutes, 0);

                        matchDateTimeOffset = new DateTimeOffset(record.MatchDateTimeZone, offset);
                    }

                    command.Parameters.AddWithValue("@MatchDateTimeZone", matchDateTimeOffset);
                    command.Parameters.AddWithValue("@Venue", record.Venue);

                    connection.Open();
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

        public void AddToss(TossVO toss)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Toss (MatchID, TossWonBy, TossDecision) VALUES (@MatchID, @TossWonBy, @TossDecision)";

                SqlCommand command = new SqlCommand(query, connection);
                try
                { 
                    command.Parameters.AddWithValue("@MatchID", toss.MatchID);
                    command.Parameters.AddWithValue("@TossWonBy", toss.TossWonBy);
                    command.Parameters.AddWithValue("@TossDecision", toss.TossDecision);

                    connection.Open();
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


        public DataTable GetZones()
        {
            using(DataTable table = new DataTable())
            {
                string query = "Select TimeZone,Offset From TimeZones;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using(SqlDataAdapter adapter = new SqlDataAdapter(query,connection))
                    {
                        try
                        {
                            connection.Open();
                            adapter.Fill(table);
                            return table;
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                            adapter.Dispose();
                            table.Dispose();
                        }
                    }
                }
            }
        }

        public string[] GetTimezoneList()
        {
            List<string> timezones = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select TimeZone,Offset From TimeZones;";

                SqlCommand command = new SqlCommand(query, connection);
                try
                {
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
                finally
                {
                    command.Dispose();
                    connection.Close();
                    connection.Dispose();
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
                try
                {
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
                finally
                {
                    command.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
            return teamList;
        }

        public List<MatchVO> GetMatchesBetweenDates(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            List<MatchVO> records = new List<MatchVO>();

            string queryString = "SELECT * FROM Matches WHERE MatchDateTimeZone BETWEEN @fromDate AND @toDate;";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@fromDate", fromDate);
            command.Parameters.AddWithValue("@toDate", toDate);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    MatchVO record = new MatchVO
                    {
                        MatchID = Convert.ToInt32(reader["MatchID"]),
                        Team1 = Convert.ToInt32(reader["Team1"]),
                        Team2 = Convert.ToInt32(reader["Team2"]),
                        MatchFormat = reader["MatchFormat"].ToString(),
                        MatchDateTimeZone = ((DateTimeOffset)reader["MatchDateTimeZone"]).DateTime,
                        Venue = reader["Venue"].ToString()
                    };

                    records.Add(record);
                }
                reader.Close();
                ReleaseAndDispose(reader, command);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return records;
        }

        public List<MatchVO> GetTodayDateMatch(DateTime startDate, DateTime endDate)
        {
            List<MatchVO> records = new List<MatchVO>();

            string queryString = "SELECT * FROM Matches WHERE MatchDateTimeZone BETWEEN @startDate AND @endDate;";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@startDate", startDate);
            command.Parameters.AddWithValue("@endDate", endDate);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    MatchVO record = new MatchVO
                    {
                        MatchID = Convert.ToInt32(reader["MatchID"]),
                        Team1 = Convert.ToInt32(reader["Team1"]),
                        Team2 = Convert.ToInt32(reader["Team2"]),
                        MatchFormat = reader["MatchFormat"].ToString(),
                        MatchDateTimeZone = ((DateTimeOffset)reader["MatchDateTimeZone"]).DateTime,
                        Venue = reader["Venue"].ToString()
                    };
                    records.Add(record);
                }
                reader.Close();
                ReleaseAndDispose(reader, command);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return records;
        }

        public List<Dictionary<string, object>> GetMatchesByDateRange(DateTime startDate, DateTime endDate)
        {
            List<Dictionary<string, object>> matches = new List<Dictionary<string, object>>();

            string query = @"
SELECT 
    m.MatchID,
    m.Team1 AS Team1ID,
    m.Team2 AS Team2ID,
    m.MatchFormat,
    m.MatchDateTimeZone,
    m.Venue,
    t1.TeamName AS Team1Name,
    t1.TeamShortName AS Team1ShortName,
    t1.TeamIcon AS Team1Icon,
    t2.TeamName AS Team2Name,
    t2.TeamShortName AS Team2ShortName,
    t2.TeamIcon AS Team2Icon
FROM 
    Matches m
JOIN 
    Teams t1 ON m.Team1 = t1.TeamID
JOIN 
    Teams t2 ON m.Team2 = t2.TeamID
WHERE 
    m.MatchDateTimeZone BETWEEN @startDate AND @endDate";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@endDate", endDate);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var match = new Dictionary<string, object>
                            {
        { "MatchID", reader.GetInt32(reader.GetOrdinal("MatchID")) },
        { "MatchFormat", reader.GetString(reader.GetOrdinal("MatchFormat")) },
        { "MatchDateTimeZone", reader.GetDateTimeOffset(reader.GetOrdinal("MatchDateTimeZone")).DateTime },
        { "Venue", reader.GetString(reader.GetOrdinal("Venue")) },
        { "Team1ID", reader.GetInt32(reader.GetOrdinal("Team1ID")) },
        { "Team1Name", reader.GetString(reader.GetOrdinal("Team1Name")) },
        { "Team1ShortName", reader.GetString(reader.GetOrdinal("Team1ShortName")) },
        { "Team1Icon", reader["Team1Icon"] as byte[] },
        { "Team2ID", reader.GetInt32(reader.GetOrdinal("Team2ID")) },
        { "Team2Name", reader.GetString(reader.GetOrdinal("Team2Name")) },
        { "Team2ShortName", reader.GetString(reader.GetOrdinal("Team2ShortName")) },
        { "Team2Icon", reader["Team2Icon"] as byte[] }
    };

                        matches.Add(match);
                    }

                }
            }

            return matches;
        }





        public MatchVO GetMatchByID(int matchId) 
        {
            string query = @"SELECT * FROM Matches WHERE MatchID = @matchId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@matchId", matchId);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new MatchVO
                        {
                            MatchID = (int)reader["MatchID"],
                            MatchFormat = reader["MatchFormat"].ToString(),
                            Team1 = Convert.ToInt32(reader["Team1"]),
                            Team2 = Convert.ToInt32(reader["Team2"]),
                            Venue = reader["Venue"].ToString(),
                            MatchDateTimeZone = ((DateTimeOffset)reader["MatchDateTimeZone"]).DateTime
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public Dictionary<string, object> GetMatchListByID(int matchId)
        {
            Dictionary<string, object> match = null;

            string query = @"
        SELECT 
    m.MatchID,
    m.Team1 AS Team1ID,
    m.Team2 AS Team2ID,
    m.MatchFormat,
    m.MatchDateTimeZone,
    m.Venue,
    t1.TeamName AS Team1Name,
    t1.TeamShortName AS Team1ShortName,
    t1.TeamIcon AS Team1Icon,
    t2.TeamName AS Team2Name,
    t2.TeamShortName AS Team2ShortName,
    t2.TeamIcon AS Team2Icon
FROM 
    Matches m
JOIN 
    Teams t1 ON m.Team1 = t1.TeamID
JOIN 
    Teams t2 ON m.Team2 = t2.TeamID
        WHERE 
            m.MatchID = @matchId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@matchId", matchId);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        match = new Dictionary<string, object>
                {
                    { "MatchID", reader.GetInt32(reader.GetOrdinal("MatchID")) },
                    { "Team1ID", reader.GetInt32(reader.GetOrdinal("Team1ID")) },
                    { "Team2ID", reader.GetInt32(reader.GetOrdinal("Team2ID")) },
                    { "MatchFormat", reader.GetString(reader.GetOrdinal("MatchFormat")) },
                    { "MatchDateTimeZone", reader.GetDateTimeOffset(reader.GetOrdinal("MatchDateTimeZone")).DateTime },
                    { "Venue", reader.GetString(reader.GetOrdinal("Venue")) },
                    { "Team1Name", reader.GetString(reader.GetOrdinal("Team1Name")) },
                    { "Team1ShortName", reader.GetString(reader.GetOrdinal("Team1ShortName")) },
                    { "Team1Icon", reader["Team1Icon"] as byte[] },
                    { "Team2Name", reader.GetString(reader.GetOrdinal("Team2Name")) },
                    { "Team2ShortName", reader.GetString(reader.GetOrdinal("Team2ShortName")) },
                    { "Team2Icon", reader["Team2Icon"] as byte[] },
                };
                    }
                }
            }

            return match;
        }



        public List<Dictionary<string, object>> GetPlayersByTeamID(int teamID)
        {
            List<Dictionary<string, object>> players = new List<Dictionary<string, object>>();

            string query = @"SELECT p.PlayerID, p.Name, pd.PlayerType, pd.Role, p.Picture
                         FROM Players AS p
                         LEFT JOIN PlayerDetails AS pd ON p.PlayerID = pd.PlayerID
                         WHERE p.TeamId = @TeamID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TeamID", teamID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> player = new Dictionary<string, object>
                        {
                            { "PlayerID", reader.GetInt32(0) },
                            { "Name", reader.GetString(1) },
                            { "PlayerType", reader.IsDBNull(2) ? null : reader.GetString(2) },
                            { "Role", reader.IsDBNull(3) ? null : reader.GetString(3) },
                            { "Picture", reader["Picture"] as byte[] }
                        };

                            players.Add(player);
                        }
                    }
                }
            }

            return players;
        }

        public void SaveBattingOrder(List<int> team1PlayerIds, List<int> team2PlayerIds, int matchId, int team1Id, int team2Id)
        {
            ClearBattingEntries(matchId, team1Id);
            ClearBattingEntries(matchId, team2Id);

            // Save new batting entries based on the order
            SaveBattingEntries(team1PlayerIds, matchId, team1Id);
            SaveBattingEntries(team2PlayerIds, matchId, team2Id);
        }


        // Method to clear existing batting entries
        private void ClearBattingEntries(int matchId, int teamId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string clearQuery = "DELETE FROM Batting WHERE MatchId = @MatchId AND PlayerId IN (SELECT PlayerId FROM Players WHERE TeamId = @TeamId)";
                using (SqlCommand clearCmd = new SqlCommand(clearQuery, conn))
                {
                    clearCmd.Parameters.AddWithValue("@MatchId", matchId);
                    clearCmd.Parameters.AddWithValue("@TeamId", teamId);
                    clearCmd.ExecuteNonQuery();
                }
            }
        }

        // Method to save new batting entries
        private void SaveBattingEntries(List<int> playerIds, int matchId, int teamId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string insertQuery = @"INSERT INTO Batting (PlayerId, MatchId, Runs, Balls, Fours, Sixes, StrikeRate) VALUES (@PlayerId, @MatchId, 0, 0, 0, 0, 0.00)";
                foreach (int playerId in playerIds)
                {
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@PlayerId", playerId);
                        insertCmd.Parameters.AddWithValue("@MatchId", matchId);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }


        public List<Dictionary<string, object>> GetWicketTypes()
        {
            List<Dictionary<string, object>> wicketTypes = new List<Dictionary<string, object>>();

            string query = @"SELECT WicketTypeID, WicketTypeName FROM WicketType";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> wicketType = new Dictionary<string, object>
                            {
                                { "WicketTypeID", reader.GetInt32(0) },
                                { "WicketTypeName", reader.GetString(1) }
                            };

                            wicketTypes.Add(wicketType);
                        }
                    }
                }
            }

            return wicketTypes;
        }

        public List<Dictionary<string, object>> GetPlayersByTeamIDAndPlayerType(int teamID, int playerType)
        {
            List<Dictionary<string, object>> players = new List<Dictionary<string, object>>();

            string query = @"SELECT PlayerID, Name FROM Players WHERE TeamID = @TeamID AND PlayerType = @PlayerType";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TeamID", teamID);
                    command.Parameters.AddWithValue("@PlayerType", playerType);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> player = new Dictionary<string, object>
                            {
                                { "PlayerID", reader.GetInt32(0) },
                                { "Name", reader.GetString(1) }
                            };

                            players.Add(player);
                        }
                    }
                }
            }

            return players;
        }

        public List<PlayerListVO> GetBattingOrderPlayers(int matchId, int teamId)
        {
            List<PlayerListVO> players = new List<PlayerListVO>();

            string query = @"SELECT p.PlayerId, p.Name
            FROM Players p
            JOIN Batting b ON p.PlayerId = b.PlayerId
            WHERE b.MatchId = @MatchId AND p.TeamId = @TeamId
            ORDER BY b.BattingId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MatchId", matchId);
                command.Parameters.AddWithValue("@TeamId", teamId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    PlayerListVO player = new PlayerListVO();
                    player.PlayerId = Convert.ToInt32(reader["PlayerId"]);
                    player.Name = Convert.ToString(reader["Name"]);
                    players.Add(player);
                }

                reader.Close();
            }

            return players;
        }

        public void SaveWicketData(WicketVO wicket)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO fallOfWicket (matchId, batsmanId, bowlerId, fielderId, wicketTypeId) VALUES (@MatchId, @BatsmanId, @BowlerId, @FielderId, @WicketTypeId)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MatchId", wicket.MatchId);
                    command.Parameters.AddWithValue("@BatsmanId", wicket.BatsmanId);
                    command.Parameters.AddWithValue("@BowlerId", wicket.BowlerId);
                    command.Parameters.AddWithValue("@FielderId", (object)wicket.FielderId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@WicketTypeId", wicket.WicketTypeId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<WicketVO> GetWicketsByMatchId(int matchId)
        {
            List<WicketVO> wickets = new List<WicketVO>();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT matchId, batsmanId, bowlerId, fielderId, wicketTypeId FROM fallOfWicket WHERE matchId = @MatchId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MatchId", matchId);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            WicketVO wicket = new WicketVO
                            {
                                MatchId = reader.GetInt32(reader.GetOrdinal("matchId")),
                                BatsmanId = reader.GetInt32(reader.GetOrdinal("batsmanId")),
                                BowlerId = reader.GetInt32(reader.GetOrdinal("bowlerId")),
                                FielderId = reader.IsDBNull(reader.GetOrdinal("fielderId")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("fielderId")),
                                WicketTypeId = reader.GetInt32(reader.GetOrdinal("wicketTypeId"))
                            };

                            wickets.Add(wicket);
                        }
                    }
                }
            }

            return wickets;
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
