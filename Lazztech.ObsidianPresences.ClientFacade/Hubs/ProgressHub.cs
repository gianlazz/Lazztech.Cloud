using HackathonManager.Models;
using HackathonManager.PocoModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.ObsidianPresences.ClientFacade.Hubs
{
    public class ProgressHub : Hub
    {
        public void UpdateTeam(MentorRequest request, string message)
        {
            if (request.RequestAccepted)
            {
                Clients.Group(request.TeamName).RequestUpdate(request, "Mentor Accepted Request.");
            }
        }

        public Task<string> GetConnectionId()
        {
            return Task.Run(() =>
            {
                return Context.ConnectionId;
            });
        }

        public void AddConnectionToTeamGroup()
        {
            var teamCookieValue = Context.GetHttpContext().Request.Cookies["team"];

            if (!string.IsNullOrWhiteSpace(teamCookieValue))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, teamCookieValue);
            }

        }

        public void MessageTeam(Team team, string message)
        {
            Clients.Group(team.Name).message(message);
        }

        //https://docs.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/working-with-groups
        //public Task JoinRoom(string roomName)
        //{
        //    return Groups.Add(Context.ConnectionId, roomName);
        //}

        //public Task LeaveRoom(string roomName)
        //{
        //    return Groups.Remove(Context.ConnectionId, roomName);
        //}

        public static ConcurrentDictionary<string, Team> MyUsers = new ConcurrentDictionary<string, Team>();

        public override Task OnConnected()
        {
            string usersTeamCookie = null;
            if (Context.GetHttpContext().Request.Cookies.ContainsKey("team"))
                usersTeamCookie = Context.GetHttpContext().Request.Cookies["team"];
            if (usersTeamCookie != null)
            {
                MyUsers.TryAdd(Context.ConnectionId, new Team() { Name = usersTeamCookie });
                //string name = Context.User.Identity.Name;

                //Groups.Add(Context.ConnectionId, name);
                Groups.AddToGroupAsync(Context.ConnectionId, usersTeamCookie);

                if (usersTeamCookie != null)
                {
                    MyUsers.TryAdd(Context.ConnectionId, new Team() { Name = usersTeamCookie });
                    //string name = Context.User.Identity.Name;

                    //Groups.Add(Context.ConnectionId, name);

                    //Groups.Add(Context.ConnectionId, cookie.Value);
                    Groups.AddToGroupAsync(Context.ConnectionId, usersTeamCookie);
                }
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Team team;
            MyUsers.TryRemove(Context.ConnectionId, out team);
            return base.OnDisconnected(stopCalled);
        }

        //public override Task OnReconnected()
        //{
        //    Cookie cookie = Context.Request.Cookies["team"];
        //    if (cookie != null)
        //    {
        //        MyUsers.TryAdd(Context.ConnectionId, new Team() { Name = cookie.Value });
        //        //string name = Context.User.Identity.Name;

        //        //Groups.Add(Context.ConnectionId, name);
        //        Groups.Add(Context.ConnectionId, cookie.Name);

        //        if (cookie.Value != null)
        //        {
        //            MyUsers.TryAdd(Context.ConnectionId, new Team() { Name = cookie.Value });
        //            //string name = Context.User.Identity.Name;

        //            //Groups.Add(Context.ConnectionId, name);
        //            Groups.Add(Context.ConnectionId, cookie.Name);
        //        }
        //    }

        //    return base.OnReconnected();
        //}
    }
