
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

            List<BattingVO> battingInning1 = MatchValidationBLObject.GetAllBatting();
            ViewBag.BattingInning1 = battingInning1;

            List<WicketVO> wicket = MatchValidationBLObject.GetWicketsByMatchId(matchId);
            ViewBag.FallOfWicket = wicket;


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

                // Determine the batting and bowling teams based on the toss decision
                int battingTeamID = TossWonBy;
                int bowlingTeamID = (battingTeamID == match.Team1) ? match.Team2 : match.Team1;

                if (TossDecision.Equals("Bowling", StringComparison.OrdinalIgnoreCase))
                {
                    // Swap if the toss winner chose to bowl
                    int temp = battingTeamID;
                    battingTeamID = bowlingTeamID;
                    bowlingTeamID = temp;
                }

                // Create the initial inning record
                MatchInningVO initialInning = new MatchInningVO
                {
                    MatchID = MatchID,
                    InningNumber = 1,
                    BattingTeamID = battingTeamID,
                    BowlingTeamID = bowlingTeamID,
                    RunsScored = 0,
                    WicketsLost = 0,
                    OversBowled = 0.0M // Initial overs bowled is 0.0
                };

                // Save the inning record
                MatchValidationBLObject.SaveMatchInning(initialInning);

                // Retrieve players for each team
                ViewBag.Team1Players = MatchValidationBLObject.GetPlayersByTeamID(match.Team1);
                ViewBag.Team2Players = MatchValidationBLObject.GetPlayersByTeamID(match.Team2);

                return Json(new { success = true, message = "Toss Decision and Inning record saved successfully!" });
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

                return Json(new { success = true, message = "Players batting order saved successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error saving Batting Order: {ex.Message}" });
            }
        }

        [HttpPost]
        public ActionResult UpdateBattingStats(int playerOnStrikeId, int playerOffStrikeId, int bowlerId, int runs, int balls, int fours, int sixes)
        {
            string errorMessage;
            MatchValidationBLObject = new MatchValidationBL();
            bool isSuccess = MatchValidationBLObject.UpdateBattingStatistics(playerOnStrikeId, runs, balls, fours, sixes, out errorMessage);

            if (isSuccess)
            {
                return Json(new { success = true, message = "Batting stats saved successfully!" });
            }
            else
            {
                return Json(new { success = false, error = errorMessage });
            }
        }

        [HttpPost]
        public ActionResult UpdateFallOfWicket(int matchId, int batsmanId, int bowlerId, int wicketTypeId, int? fielderId, int inningNumber, int runsScored, int wicketsLost, decimal oversBowled) 
        {
            try
            {
                // Create a new entry in the fallOfWicket table
                WicketVO newWicket = new WicketVO
                {
                    MatchId = matchId,
                    BatsmanId = batsmanId,
                    BowlerId = bowlerId,
                    WicketTypeId = wicketTypeId,
                    FielderId = fielderId // Modify this as per your actual requirement
                };

                MatchInningVO inningWicket = new MatchInningVO
                {
                    MatchID = matchId,
                    InningNumber = inningNumber,
                    RunsScored = runsScored,
                    WicketsLost = wicketsLost,
                    OversBowled = oversBowled // Initial overs bowled is 0.0
                };

                MatchValidationBLObject = new MatchValidationBL();
                MatchValidationBLObject.UpdateFallOfWicket(newWicket);
                MatchValidationBLObject.UpdateMatchInning(inningWicket);

                // Return success message or necessary data
                return Json(new { success = true, message = "Fall of wicket and Inning updated successfully." });
            }
            catch (Exception ex)
            {
                // Handle exception
                return Json(new { success = false, message = "Error updating fall of wicket and Inning: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult UpdateBall(int inningId, int overNumber, int ballNumber, int bowlerId, int batsmanId, int runsScored, int wicketId)
        {
            try
            {
                // Logic to update the Ball table
                BallVO newBall = new BallVO
                {
                    InningID = inningId,
                    OverNumber = overNumber,
                    BallNumber = ballNumber,
                    BowlerID = bowlerId,
                    BatsmanID = batsmanId,
                    RunsScored = runsScored,
                    WicketID = wicketId
                };

                MatchValidationBLObject = new MatchValidationBL();
                MatchValidationBLObject.UpdateBall(newBall);

                // Return success message or necessary data
                return Json(new { success = true, message = "Ball details updated successfully." });
            }
            catch (Exception ex)
            {
                // Handle exception
                return Json(new { success = false, message = "Error updating Ball: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult UpdateBowlingStats(int bowlerId, int teamId, int matchId, decimal totalOver, int runsScored, int maiden, int wicketId, decimal eco)
        {
            try
            {
                // Logic to update the Bowling table
                BowlingVO newBowling = new BowlingVO
                {
                    BowlerID = bowlerId,
                    TeamID = teamId,
                    MatchID = matchId,
                    TotalOver = totalOver,
                    RunsScored = runsScored,
                    Maiden = maiden,
                    WicketID = wicketId,
                    ECO = eco
                };

                MatchValidationBLObject = new MatchValidationBL();
                MatchValidationBLObject.UpdateBowling(newBowling);

                // Return success message or necessary data
                return Json(new { success = true, message = "Bowling stats updated successfully." });
            }
            catch (Exception ex)
            {
                // Handle exception
                return Json(new { success = false, message = "Error updating Bowling stats: " + ex.Message });
            }
        }



    }
}

