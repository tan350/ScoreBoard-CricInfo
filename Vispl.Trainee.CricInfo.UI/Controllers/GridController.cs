using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vispl.Trainee.CricInfo.BM;
using Vispl.Trainee.CricInfo.VO;

namespace Vispl.Trainee.CricInfo.UI.Controllers
{
    public class GridController : Controller
    {
        ValidationBL ValidationBLObject;
        TeamValidationBL TeamValidationBLObject;
        MatchValidationBL MatchValidationBLObject;

        // GET: Grid
        public ActionResult PlayerGrid()
        {
            ValidationBLObject = new ValidationBL();
            var players = ValidationBLObject.ReadAllRecordsData();
            return View(players);
        }

        public ActionResult TeamGrid()
        {
            TeamValidationBLObject = new TeamValidationBL();
            var teams = TeamValidationBLObject.ReadAllRecordsData();
            return View(teams);
        }

        public ActionResult MatchGrid()
        {
            MatchValidationBLObject = new MatchValidationBL();
            var matches = MatchValidationBLObject.ReadAllRecordsData();
            return View(matches);
        }

        public ActionResult MatchFilter()
        {
            MatchValidationBLObject = new MatchValidationBL();
            ViewBag.TimeZoneList = MatchValidationBLObject.GetTimezones();

            return View();
        }

        [HttpPost]
        public ActionResult MatchFilter(DateTime fromDate, DateTime toDate)
        {
            MatchValidationBLObject = new MatchValidationBL();
            List<MatchVO> matches = MatchValidationBLObject.ReadAllRecordsData().Where(m => m.MatchDateTimeZone >= fromDate && m.MatchDateTimeZone <= toDate).ToList();

            return View("~/Views/Grid/MatchGrid.cshtml", matches);
        }
    }
}