using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vispl.Trainee.CricInfo.BM;
using Vispl.Trainee.CricInfo.BM.ITF;
using Vispl.Trainee.CricInfo.VO;

namespace Vispl.Trainee.CricInfo.UI.Controllers
{
    public class DashboardController : Controller
    {
        IValidationBL ValidationBLObject;
        ITeamValidationBL TeamValidationBLObject;
        IMatchValidationBL MatchValidationBLObject;

        // GET: Dashboard
        public ActionResult Dashboard()
        {
            try
            {
                ValidationBLObject = new ValidationBL();
                TeamValidationBLObject = new TeamValidationBL();
                MatchValidationBLObject = new MatchValidationBL();

                ViewBag.playerCount = ValidationBLObject.ReadAllRecordsData().ToList().Count();
                ViewBag.teamCount = TeamValidationBLObject.ReadAllRecordsData().ToList().Count();
                ViewBag.matchesCount = MatchValidationBLObject.ReadAllRecordsData().ToList().Count();
                return View();
            }
            finally
            {
                if (ValidationBLObject != null)
                {
                    ValidationBLObject = null;
                }
                if (TeamValidationBLObject != null)
                {
                    TeamValidationBLObject = null;
                }
                if (MatchValidationBLObject != null)
                {
                    MatchValidationBLObject = null;
                }
            }
        }

        [Authorize]
        [Route("Dashboard/AdminDashboard")]
        public ActionResult AdminDashboard()
        {
            try
            {
                ValidationBLObject = new ValidationBL();
                TeamValidationBLObject = new TeamValidationBL();
                MatchValidationBLObject = new MatchValidationBL();

                ViewBag.playerCount = ValidationBLObject.ReadAllRecordsData().ToList().Count();
                ViewBag.teamCount = TeamValidationBLObject.ReadAllRecordsData().ToList().Count();
                ViewBag.matchesCount = MatchValidationBLObject.ReadAllRecordsData().ToList().Count();

                DateTime now = DateTime.Now;
                DateTime startOfToday = now.Date;
                DateTime endOfToday = startOfToday.AddDays(1).AddSeconds(-1);
                /*List<MatchVO> TodayMatch = MatchValidationBLObject.GetTodayMatch(startOfToday, endOfToday);*/
                List<Dictionary<string, object>> TodayMatch = MatchValidationBLObject.GetMatchesByDateRange(startOfToday, endOfToday);

                return View(TodayMatch);
            }
            finally
            {
                if (ValidationBLObject != null)
                {
                    ValidationBLObject = null;
                }
                if (TeamValidationBLObject != null)
                {
                    TeamValidationBLObject = null;
                }
                if (MatchValidationBLObject != null)
                {
                    MatchValidationBLObject = null;
                }
            }

        }
    }
}