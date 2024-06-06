using System;
using System.IO;
using Newtonsoft.Json;

namespace Vispl.Trainee.CricInfo.DL
{
    public class ConnectionStringManager
    {
        public string DataSource { get; set; }
        public string Database { get; set; }
        public string IntegratedSecurity { get; set; }
        public string TrustServerCertificate { get; set; }

        public static string GetConnectionString()
        {
            string jsonText = File.ReadAllText("C:\\Users\\hp7\\source\\repos\\ScoreBoard\\Vispl.Trainee.CricInfo.DL\\ConnectionString.json");

            ConnectionStringManager connectionStringManager = JsonConvert.DeserializeObject<ConnectionStringManager>(jsonText);

            string connectionString = $"Data Source={connectionStringManager.DataSource}; " +
                                        $"Database={connectionStringManager.Database}; " +
                                        $"Integrated Security={connectionStringManager.IntegratedSecurity}; " +
                                        $"TrustServerCertificate={connectionStringManager.TrustServerCertificate};";

            Console.WriteLine(connectionString);
            return connectionString;
        }
    }
}
