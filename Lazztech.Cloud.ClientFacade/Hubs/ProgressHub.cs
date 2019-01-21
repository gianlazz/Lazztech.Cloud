using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Hubs
{
    public class ProgressHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            string uniqueUsersCookie = null;
            if (Context.GetHttpContext().Request.Cookies.ContainsKey(StaticStrings.eventUserIdCookieName))
                uniqueUsersCookie = Context.GetHttpContext().Request.Cookies[StaticStrings.eventUserIdCookieName];
            if (uniqueUsersCookie != null)
                Groups.AddToGroupAsync(Context.ConnectionId, uniqueUsersCookie);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public void UpdateTeamOfMentorRequest(string UniqueUserId, bool accepted, string message = null)
        {
            var percentage = (20 * 100) / 100;
            Clients.Group(UniqueUserId).SendAsync("requestUpdate", message, percentage);
        }

        public void UpdateTeam(MentorRequest request, string message)
        {
            if (request.RequestAccepted)
            {
                Clients.Group(request.UniqueRequesteeId).SendAsync("RequestUpdate", request, "Mentor Accepted Request.");
            }
        }

        public Task<string> GetConnectionId()
        {
            return Task.Run(() =>
            {
                return Context.ConnectionId;
            });
        }

        public void MessageTeam(Team team, string message)
        {
            Clients.Group(team.Name).SendAsync("message", message);
        }
    }
}