using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vispl.Trainee.CricInfo.BM;
using Vispl.Trainee.CricInfo.BM.ITF;

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
            ValidationBLObject = new ValidationBL();
            TeamValidationBLObject = new TeamValidationBL();
            MatchValidationBLObject = new MatchValidationBL();

            ViewBag.playerCount = ValidationBLObject.ReadAllRecordsData().ToList().Count();
            ViewBag.teamCount = TeamValidationBLObject.ReadAllRecordsData().ToList().Count();
            ViewBag.matchesCount = MatchValidationBLObject.ReadAllRecordsData().ToList().Count();
            return View();
        }

        [Authorize]
        [Route("Dashboard/AdminDashboard")]
        public ActionResult AdminDashboard()
        {
            ValidationBLObject = new ValidationBL();
            TeamValidationBLObject = new TeamValidationBL();
            MatchValidationBLObject = new MatchValidationBL();

            ViewBag.playerCount = ValidationBLObject.ReadAllRecordsData().ToList().Count();
            ViewBag.teamCount = TeamValidationBLObject.ReadAllRecordsData().ToList().Count();
            ViewBag.matchesCount = MatchValidationBLObject.ReadAllRecordsData().ToList().Count();
            return View();

        }
    }
}