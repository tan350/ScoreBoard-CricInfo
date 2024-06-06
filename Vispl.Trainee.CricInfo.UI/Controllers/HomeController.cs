﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vispl.Trainee.CricInfo.VO;
using Vispl.Trainee.CricInfo.BM;
using Syncfusion.EJ2.Grids;
using Vispl.Trainee.CricInfo.RES;
using Vispl.Trainee.CricInfo.BM.ITF;
using System.IO;
using System.Security.Cryptography;

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
            ValidationBLObject = new ValidationBL();
            ViewBag.NationalityList = ValidationBLObject.GetNationality();

            MatchValidationBLObject = new MatchValidationBL();

            List<TeamListVO> teamNames = MatchValidationBLObject.GetTeamNamesList();

            ViewBag.TeamNames = teamNames.Select(p => new { Id = p.TeamID, Value = p.TeamName }).ToList();


            return View(new PlayerVO { DateOfBirth =DateTime.Now, DebutDate = DateTime.Now});
        }


        [HttpPost]
        public ActionResult CreatePlayer(PlayerVO player,IEnumerable<HttpPostedFileBase> Image)
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
                                player.Picture = binaryReader.ReadBytes(file.ContentLength);
                            }
                        }
                    }

                    ValidationBLObject = new ValidationBL();
                    ValidationBLObject.Save(player);
                }

                return RedirectToAction("PlayerGrid", "Grid");
            }
            return View(player);
        }

        public ActionResult CreateTeam()
        {
            ValidationBLObject = new ValidationBL();

            List<PlayerListVO> playerNames = ValidationBLObject.GetPlayerNames();
            List<PlayerListVO> captainNames = ValidationBLObject.GetCaptainNames();

            ViewBag.PlayerNames = playerNames.Select(p => new { Id = p.PlayerId, Value = p.Name }).ToList();
            ViewBag.CaptainNames = captainNames.Select(c => new { Id = c.PlayerId, Value = c.Name }).ToList();

            return View(new TeamVO());
        }


        [HttpPost]
        public ActionResult CreateTeam(TeamVO team,IEnumerable<HttpPostedFileBase> Image)
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


        public ActionResult CreateMatch()
        {
            MatchValidationBLObject = new MatchValidationBL();
            ViewBag.TimeZoneList = MatchValidationBLObject.GetTimezones();

            List<TeamListVO> teamNames = MatchValidationBLObject.GetTeamNamesList();

            ViewBag.TeamNames = teamNames.Select(p => new { Id = p.TeamID, Value = p.TeamName }).ToList();

            return View(new MatchVO{MatchDateTimeZone = DateTime.Now});
        }

        [HttpPost]
        public ActionResult CreateMatch(MatchVO match)
        {
            if (ModelState.IsValid)
            {
                MatchValidationBLObject = new MatchValidationBL();
                MatchValidationBLObject.Save(match);
                return RedirectToAction("MatchGrid", "Grid");
            }
            return View(match);
        }

    }
}