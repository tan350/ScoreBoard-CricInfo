using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vispl.Trainee.CricInfo.VO;

namespace Vispl.Trainee.CricInfo.UI.SignalRHub
{
    public class CricketHub : Hub
    {
        public void SendBowlingUpdate(string bowlerName, int overs, int runs, int maidens, int wickets, double economy, string description)
        {
            Clients.All.updateBowling(bowlerName, overs, runs, maidens, wickets, economy, description);
        }

        public void SendWicketUpdate(WicketVO wicket)
        {
            Clients.All.updateWicket(wicket);
        }

        public void SendCommentary(string commentary)
        {
            Clients.All.updateCommentary(commentary);
        }
    }
}