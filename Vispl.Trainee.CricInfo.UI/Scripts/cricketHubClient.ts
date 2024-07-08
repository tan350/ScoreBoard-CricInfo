/*// Import necessary typings if using SignalR and jQuery in TypeScript
import * as $ from 'jquery';
import 'signalr';

// Ensure typings for SignalR are properly installed and referenced in your project

$(function () {
    // Declare a variable for cricketHub
    let cricketHub: any = ($.connection as any).cricketHub;

    // Define the client-side method 'updateWicket' to handle incoming wicket updates
    cricketHub.client.updateWicket = function (wicket: any) {
        // Handle updating the bowling table for team1
        let rowBowl: string = `<tr>
            <td>${wicket.BowlerId}</td>
            <td>${wicket.FielderId}</td>
            <td>${wicket.DescriptionWicket}</td>
        </tr>`;
        $('#team1-bowling').append(rowBowl);

        // Handle updating the wickets table for team2
        let row: string = `<tr>
            <td>${wicket.MatchId}</td>
            <td>${wicket.BatsmanId}</td>
            <td>${wicket.WicketTypeId}</td>
        </tr>`;
        $('#team2-wickets').append(row);
    };

    // Start the SignalR connection
    ($.connection as any).hub.start().done(function () {
        console.log('SignalR connected');
    });
});
*/