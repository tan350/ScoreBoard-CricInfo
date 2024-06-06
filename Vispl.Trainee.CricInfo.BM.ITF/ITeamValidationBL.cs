using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.CricInfo.VO;

namespace Vispl.Trainee.CricInfo.BM.ITF
{
    public interface ITeamValidationBL
    {
        List<TeamVO> ReadAllRecordsData();
        void Save(TeamVO record);
        void Dispose();
    }
}
