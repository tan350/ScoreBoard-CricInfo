
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vispl.Trainee.CricInfo.BM;
using Vispl.Trainee.CricInfo.UI.SignalRHub;
using Vispl.Trainee.CricInfo.VO;
using Vispl.Trainee.CricInfo.BM.ITF;
using System.Text.RegularExpressions;

namespace SatyaMVCsignalR.Controllers
{
    public class ScoreBoardController : Controller
    {
        IMatchValidationBL MatchValidationBLObject;
        public ActionResult UpdateBowlingScoreBoard()
        {
            return View();
        }

        public ActionResult ForChat()
        {
            return View();
        }

        private readonly IHubContext _hubContext;

        public ScoreBoardController()
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<CricketHub>();
        }

        public ActionResult BallingAndFallOfWicket()
        {
            MatchValidationBLObject = new MatchValidationBL();
            ViewBag.WicketType = MatchValidationBLObject.GetWicketTypes();
            ViewBag.Batsman = MatchValidationBLObject.GetPlayersByTeamIDAndPlayerType(1, 2);
            ViewBag.Bowler = MatchValidationBLObject.GetPlayersByTeamIDAndPlayerType(3, 1);
            ViewBag.Fielder = MatchValidationBLObject.GetPlayersByTeamID(3);
            return View();
        }

        public ActionResult LiveBallingAndFallOfWicket()
        {
            MatchValidationBLObject = new MatchValidationBL();
            List<WicketVO> wicket = new List<WicketVO>();
            int matchId = 1;
            wicket = MatchValidationBLObject.GetWicketsByMatchId(1);
            return View(wicket);
        }

        [HttpPost]
        public ActionResult SendBowlingUpdate(string bowlerName, int overs, int runs, int maidens, int wickets, double economy, string description)
        {
            string message = "BowlingUpdate uploaded successfully!";
            _hubContext.Clients.All.updateBowling(bowlerName, overs, runs, maidens, wickets, economy, description);
            return View("~/Views/ScoreBoard/UpdateConfirmation.cshtml", (object)message);
        }

        /*[HttpPost]
        public ActionResult SendWicketUpdate(string batsmanName, string bowlerNameWicket, string descriptionWicket)
        {
            string message = "WicketUpdate uploaded successfully!";
            _hubContext.Clients.All.updateWicket(batsmanName, bowlerNameWicket, descriptionWicket);
            return View("~/Views/ScoreBoard/UpdateConfirmation.cshtml", (object)message);
        }*/

        [HttpPost]
        public ActionResult SendWicketUpdate(WicketVO wicket)
        {
            string message = "WicketUpdate uploaded successfully!";
            _hubContext.Clients.All.updateWicket(wicket);

            if (ModelState.IsValid)
            {
                MatchValidationBLObject = new MatchValidationBL();
                MatchValidationBLObject.SaveWicketData(wicket);

                var cricketHub = GlobalHost.ConnectionManager.GetHubContext<CricketHub>();
                cricketHub.Clients.All.updateWicket(wicket);

                return View("~/Views/ScoreBoard/UpdateConfirmation.cshtml", (object)message);
            }
            MatchValidationBLObject = new MatchValidationBL();
            ViewBag.WicketType = MatchValidationBLObject.GetWicketTypes();
            ViewBag.Batsman = MatchValidationBLObject.GetPlayersByTeamIDAndPlayerType(1, 2);
            ViewBag.Bowler = MatchValidationBLObject.GetPlayersByTeamIDAndPlayerType(3, 1);
            ViewBag.Fielder = MatchValidationBLObject.GetPlayersByTeamID(3);
            return View("~/Views/ScoreBoard/BallingAndFallOfWicket.cshtml");

        }


        public ActionResult Toss(int matchId)
        {
            MatchValidationBLObject = new MatchValidationBL();
            /*var match = MatchValidationBLObject.GetMatchByID(matchId);*/
            Dictionary<string, object> match = MatchValidationBLObject.GetMatchListByID(matchId);
            if (match == null)
            {
                return HttpNotFound("No matches scheduled for today.");
            }

            /* MatchVO toss = new MatchVO
             {
                 MatchID = matchId,
                 Team1 = match.Team1,
                 Team2 = match.Team2,
                 Venue = match.Venue,
                 MatchDateTimeZone = match.MatchDateTimeZone,
                 MatchFormat = match.MatchFormat,
             };
 */
            MatchVO matchplayer = MatchValidationBLObject.GetMatchByID(matchId);

            ViewBag.Team1Players = MatchValidationBLObject.GetPlayersByTeamID(matchplayer.Team1);
            ViewBag.Team2Players = MatchValidationBLObject.GetPlayersByTeamID(matchplayer.Team2);
            return View(match);
        }

        [HttpPost]
        public ActionResult RecordToss(int MatchID, int TossWonBy, string TossDecision)
        {
            var toss = new TossVO
            {
                MatchID = MatchID,
                TossWonBy = TossWonBy,
                TossDecision = TossDecision
            };
            MatchValidationBLObject = new MatchValidationBL();
            MatchValidationBLObject.SaveToss(toss);

            MatchVO match = MatchValidationBLObject.GetMatchByID(MatchID);

            ViewBag.Team1Players = MatchValidationBLObject.GetPlayersByTeamID(match.Team1);
            ViewBag.Team2Players = MatchValidationBLObject.GetPlayersByTeamID(match.Team2);

            return RedirectToAction("MatchRunAndBalls", new { matchId = MatchID });
        }

        [HttpPost]
        public ActionResult SaveBattingOrder(List<int> team1PlayerIds, List<int> team2PlayerIds, int matchId, int team1Id, int team2Id)
        {
            try
            {
                MatchValidationBLObject = new MatchValidationBL();
                MatchValidationBLObject.SaveBattingOrder(team1PlayerIds, team2PlayerIds, matchId, team1Id, team2Id);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public ActionResult MatchRunAndBalls(int matchId)
        {
            MatchValidationBLObject = new MatchValidationBL();
            var match = MatchValidationBLObject.GetMatchByID(matchId);

            if (match == null)
            {
                return HttpNotFound("No scheduled match for Today.");
            }
            /*
                        var players = _playerService.GetPlayersForTeams(match.Team1Name, match.Team2Name);

                        var matchDetailsViewModel = new MatchDetailsViewModel
                        {
                            MatchId = matchId,
                            Team1Name = match.Team1Name,
                            Team2Name = match.Team2Name,
                            Players = players
                        };

                        return View(matchDetailsViewModel);*/
            return View();
        }

    }
}



/*using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Vispl.Trainee.CricInfo.UI.SignalRHub;

namespace Vispl.Trainee.CricInfo.UI.Controllers
{
    public class ScoreBoardController : Controller
    {
        // GET: ScoreBoard
        public ActionResult BattingBowlingScoreBoard()
        {
            return View();
        }

        // GET: UpdateBowlingScoreBoard
        public ActionResult UpdateBowlingScoreBoard()
        {
            return View();
        }

        // POST: UpdateBowlingScoreBoard
        [HttpPost]
        public async Task<ActionResult> UpdateBowlingScoreBoard(string BowlerName, int Overs, int Runs, int Maidens, int Wickets, double Economy, string Batsman, string Over)
        {
            var scoreData = new
            {
                team1 = new
                {
                    bowling = new List<object>
                    {
                        new { name = BowlerName, overs = Overs, runs = Runs, maidens = Maidens, wickets = Wickets, economy = Economy }
                    }
                },
                team2 = new
                {
                    wickets = new List<object>
                    {
                        new { batsman = Batsman, over = Over }
                    }
                }
            };

            var context = GlobalHost.ConnectionManager.GetHubContext<ScoreHub>();
            await context.Clients.All.ReceiveScoreUpdate(Newtonsoft.Json.JsonConvert.SerializeObject(scoreData));
            return RedirectToAction("BattingBowlingScoreBoard");
        }
    }
}*/

