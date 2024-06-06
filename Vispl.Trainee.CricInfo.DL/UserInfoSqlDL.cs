using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.CricInfo.DL.ITF;
using Vispl.Trainee.CricInfo.VO;

namespace Vispl.Trainee.CricInfo.DL
{
    public class UserInfoSqlDL : IUserInfoDL
    {
        string connectionString = ConnectionStringManager.GetConnectionString();

        public void setvalue(UserVO user)
        {
            string queryString = "SELECT COUNT(*) FROM Users WHERE UserName = @UserName AND UserPassword = @UserPassword AND UserRole = @UserRole";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@UserPassword", user.Password);
                command.Parameters.AddWithValue("@UserRole", user.UserRole);

                connection.Open();

                int count = (int)command.ExecuteScalar();

                if (count == 0)
                {
                    user.ErrorMessage = "Credential not valid";
                }
            }
        }

    }
}
