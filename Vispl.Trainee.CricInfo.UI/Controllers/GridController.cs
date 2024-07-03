using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vispl.Trainee.CricInfo.BM;
using Vispl.Trainee.CricInfo.VO;
using Vispl.Trainee.CricInfo.BM.ITF;

namespace Vispl.Trainee.CricInfo.UI.Controllers
{
    public class GridController : Controller
    {
        IValidationBL ValidationBLObject;
        ITeamValidationBL TeamValidationBLObject;
        IMatchValidationBL MatchValidationBLObject;

        // GET: Grid
        public ActionResult PlayerGrid()
        {
            try
            {
                ValidationBLObject = new ValidationBL();
                /*var players = ValidationBLObject.ReadAllRecordsData();*/
                var players = ValidationBLObject.ReadAllRecordsDataTable();
                TeamValidationBLObject = new TeamValidationBL();
                ViewBag.TeamNames = TeamValidationBLObject.ReadAllRecordsData();

                return View(players);
            }
            finally
            {
                if (ValidationBLObject != null)
                {
                    ValidationBLObject = null;
                }
            }
        }

        public ActionResult TeamGrid()
        {
            try
            {
                ValidationBLObject = new ValidationBL();
                ViewBag.PlayerName = ValidationBLObject.GetPlayerNamesWithTeamID();

                TeamValidationBLObject = new TeamValidationBL();
                /*var teams = TeamValidationBLObject.ReadAllRecordsData();*/
                var teams = TeamValidationBLObject.ReadAllRecordsDataTable();

                return View(teams);
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
            }
        }

        public ActionResult MatchGrid()
        {
            try
            {
                MatchValidationBLObject = new MatchValidationBL();
                var matches = MatchValidationBLObject.ReadAllRecordsData();

                ViewBag.Offset = MatchValidationBLObject.GetTimezonesList();

                TeamValidationBLObject = new TeamValidationBL();
                ViewBag.TeamNames = TeamValidationBLObject.ReadAllRecordsData();
                return View(matches);
            }
            finally
            {
                if (MatchValidationBLObject != null)
                {
                    MatchValidationBLObject = null;
                }
                if (TeamValidationBLObject != null)
                {
                    TeamValidationBLObject = null;
                }
            }
        }

        public ActionResult MatchFilter()
        {
            try
            {
                MatchValidationBLObject = new MatchValidationBL();
                ViewBag.Offset = MatchValidationBLObject.GetTimezonesList();


                return View();
            }
            finally
            {
                if (MatchValidationBLObject != null)
                {
                    MatchValidationBLObject = null;
                }
            }
        }

        [HttpPost]
        public ActionResult MatchFilter(DateTime fromDate,string fromDateOffset, DateTime toDate, string toDateOffset)
        {
            try
            {
                MatchValidationBLObject = new MatchValidationBL();

                DateTimeOffset fromDateTimeZone = MatchValidationBLObject.ConvertToOffSet(fromDate,fromDateOffset);
                DateTimeOffset toDateTimeZone = MatchValidationBLObject.ConvertToOffSet(toDate, toDateOffset);

                List<MatchVO> matches = MatchValidationBLObject.GetFilteredMatches(fromDateTimeZone,toDateTimeZone);
                TeamValidationBLObject = new TeamValidationBL();
                ViewBag.TeamNames = TeamValidationBLObject.ReadAllRecordsData();

                return View("~/Views/Grid/MatchGrid.cshtml", matches);
            }
            finally
            {
                if (MatchValidationBLObject != null)
                {
                    MatchValidationBLObject = null;
                }
                if (TeamValidationBLObject != null)
                {
                    TeamValidationBLObject = null;
                }
            }
        }
    }
}