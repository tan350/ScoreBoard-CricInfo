"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// Import necessary typings if using SignalR and jQuery in TypeScript
var $ = require("jquery");
require("signalr");
// Ensure typings for SignalR are properly installed and referenced in your project
$(function () {
    // Declare a variable for cricketHub
    var cricketHub = $.connection.cricketHub;
    // Define the client-side method 'updateWicket' to handle incoming wicket updates
    cricketHub.client.updateWicket = function (wicket) {
        // Handle updating the bowling table for team1
        var rowBowl = "<tr>\n            <td>".concat(wicket.BowlerId, "</td>\n            <td>").concat(wicket.FielderId, "</td>\n            <td>").concat(wicket.DescriptionWicket, "</td>\n        </tr>");
        $('#team1-bowling').append(rowBowl);
        // Handle updating the wickets table for team2
        var row = "<tr>\n            <td>".concat(wicket.MatchId, "</td>\n            <td>").concat(wicket.BatsmanId, "</td>\n            <td>").concat(wicket.WicketTypeId, "</td>\n        </tr>");
        $('#team2-wickets').append(row);
    };
    // Start the SignalR connection
    $.connection.hub.start().done(function () {
        console.log('SignalR connected');
    });
});
