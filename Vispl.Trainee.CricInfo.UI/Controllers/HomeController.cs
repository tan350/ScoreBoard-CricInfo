using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vispl.Trainee.CricInfo.VO;
using Vispl.Trainee.CricInfo.BM;
using Vispl.Trainee.CricInfo.RES;
using Vispl.Trainee.CricInfo.BM.ITF;
using System.IO;

namespace Vispl.Trainee.CricInfo.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        IValidationBL ValidationBLObject;
        ITeamValidationBL TeamValidationBLObject;
        IMatchValidationBL MatchValidationBLObject;
        public ActionResult CreatePlayer()
        {
            try
            {
                ValidationBLObject = new ValidationBL();
                ViewBag.NationalityList = ValidationBLObject.GetNationalityWithID();

                TeamValidationBLObject = new TeamValidationBL();
                ViewBag.TeamNames = TeamValidationBLObject.ReadAllRecordsData();


                return View(new PlayerVO { DateOfBirth = DateTime.Now, DebutDate = DateTime.Now });
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


        [HttpPost]
        public ActionResult CreatePlayer(PlayerVO player,IEnumerable<HttpPostedFileBase> Image/*, string PlayerType*/)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    /*string playersSelected = Request.Form["PlayerType"];
                    player.PlayerType = playersSelected;*/
                    if (Image != null)
                    {
                        foreach (var file in Image)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                using (var binaryReader = new BinaryReader(file.InputStream))
                                {
                                    player.Picture = binaryReader.ReadBytes(file.ContentLength);
                                }
                            }
                        }
                        /*player.PlayerType = GetPlayerType(PlayerType);*/

                        ValidationBLObject = new ValidationBL();
                        ValidationBLObject.Save(player);
                    }

                    return RedirectToAction("PlayerGrid", "Grid");
                }
                else
                {
                    // Log ModelState errors
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    System.Diagnostics.Debug.WriteLine(string.Join(", ", errors));
                }
                return View(player);
            }
            finally
            {
                if (ValidationBLObject != null)
                {
                    ValidationBLObject = null;
                }
            }

        }

        public int GetPlayerType(string Player) 
        {
            switch (Player) 
            {
                case "Bowler":
                    return 1;
                case "Batsman":
                    return 2;
                case "AllRounder":
                    return 3;
                default:
                    return 2;
            }
        }

        public ActionResult CreateTeam()
        {
            try
            {
                ValidationBLObject = new ValidationBL();

                ViewBag.PlayerNames = ValidationBLObject.GetPlayerNames();
                ViewBag.CaptainNames = ValidationBLObject.GetCaptainNames();

                return View(new TeamVO());
            }
            finally
            {
                if (ValidationBLObject != null)
                {
                    ValidationBLObject = null;
                }
            }
        }


        [HttpPost]
        public ActionResult CreateTeam(TeamVO team,IEnumerable<HttpPostedFileBase> Image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Image != null)
                    {
                        foreach (var file in Image)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                using (var binaryReader = new BinaryReader(file.InputStream))
                                {
                                    team.TeamIcon = binaryReader.ReadBytes(file.ContentLength);
                                }
                            }
                        }

                        TeamValidationBLObject = new TeamValidationBL();
                        TeamValidationBLObject.Save(team);
                        return RedirectToAction("TeamGrid", "Grid");
                    }
                }
                return View(team);
            }
            finally
            {
                if (TeamValidationBLObject != null)
                {
                    TeamValidationBLObject = null;
                }
            }
        }


        public ActionResult CreateMatch()
        {
            try
            {
                MatchValidationBLObject = new MatchValidationBL();
                ViewBag.Offset = MatchValidationBLObject.GetTimezonesList();

                List<TeamListVO> teamNames = MatchValidationBLObject.GetTeamNamesList();

                ViewBag.TeamNames = teamNames.Select(p => new { Id = p.TeamID, Value = p.TeamName }).ToList();

                return View(new MatchVO { MatchDateTimeZone = DateTime.Now });
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
        public ActionResult CreateMatch(MatchVO match)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MatchValidationBLObject = new MatchValidationBL();
                    MatchValidationBLObject.Save(match);
                    return RedirectToAction("MatchGrid", "Grid");
                }
                return View(match);
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