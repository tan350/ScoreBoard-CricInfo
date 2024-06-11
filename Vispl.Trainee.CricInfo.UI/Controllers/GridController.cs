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
            try
            {
                ValidationBLObject = new ValidationBL();
                var players = ValidationBLObject.ReadAllRecordsData();
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
                TeamValidationBLObject = new TeamValidationBL();
                var teams = TeamValidationBLObject.ReadAllRecordsData();
                return View(teams);
            }
            finally
            {
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
                return View(matches);
            }
            finally
            {
                if (MatchValidationBLObject != null)
                {
                    MatchValidationBLObject = null;
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
        public ActionResult MatchFilter(DateTime fromDate, DateTime toDate)
        {
            try
            {
                MatchValidationBLObject = new MatchValidationBL();
                List<MatchVO> matches = MatchValidationBLObject.ReadAllRecordsData().Where(m => m.MatchDateTimeZone >= fromDate && m.MatchDateTimeZone <= toDate).ToList();

                return View("~/Views/Grid/MatchGrid.cshtml", matches);
            }
            finally
            {
                if (MatchValidationBLObject != null)
                {
                    MatchValidationBLObject = null;
                }
            }
        }
    }
}