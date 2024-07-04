
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

        private readonly IHubContext _hubContext;

        public ScoreBoardController()
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<CricketHub>();
        }

        public ActionResult UpdateBowlingScoreBoard()
        {
            return View();
        }

        public ActionResult ForChat()
        {
            return View();
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
            Dictionary<string, object> match = MatchValidationBLObject.GetMatchListByID(matchId);

            if (match == null)
            {
                return HttpNotFound("No matches scheduled for today.");
            }       MatchVO matchplayer = MatchValidationBLObject.GetMatchByID(matchId);

            ViewBag.Team1Players = MatchValidationBLObject.GetPlayersByTeamID(matchplayer.Team1);
            ViewBag.Team2Players = MatchValidationBLObject.GetPlayersByTeamID(matchplayer.Team2);

            ViewBag.PlayerListOrdered = MatchValidationBLObject.GetBattingOrderPlayers(matchId, matchplayer.Team1);
            ViewBag.BowlerOpposed = MatchValidationBLObject.GetPlayersByTeamIDAndPlayerType(matchplayer.Team2, 1);           //Role = Bowler (1)

            return View(match);
        }

        [HttpPost]
        public ActionResult RecordToss(int MatchID, int TossWonBy, string TossDecision)
        {
            try 
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

                return Json(new { success = true, message = "Toss Decision saved successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error saving Toss Decision: {ex.Message}" });
            }
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

    }
}

