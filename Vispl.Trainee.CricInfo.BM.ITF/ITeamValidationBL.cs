using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.CricInfo.VO;

namespace Vispl.Trainee.CricInfo.BM.ITF
{
    public interface ITeamValidationBL
    {
        List<TeamVO> ReadAllRecordsData();
        DataTable ReadAllRecordsDataTable();
        void Save(TeamVO record);
        void Dispose();
    }
}
