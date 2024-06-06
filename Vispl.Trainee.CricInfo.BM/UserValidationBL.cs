using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.CricInfo.DL;
using Vispl.Trainee.CricInfo.VO;
using Vispl.Trainee.CricInfo.DL.ITF;

namespace Vispl.Trainee.CricInfo.BM
{
    public class UserValidationBL
    {
        private readonly IUserInfoDL readUser;
        public UserValidationBL()
        {
            readUser = new UserInfoSqlDL();
        }

        private bool SetUserRole(UserVO user)
        {
            readUser.setvalue(user);

            if (user.ErrorMessage.Length > 0)
            {
                return false;
            }
            return true;
        }

        public bool isUserPresent(UserVO user)
        {
            return SetUserRole(user);
        }

    }
}
