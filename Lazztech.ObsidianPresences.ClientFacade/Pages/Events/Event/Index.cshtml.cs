﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackathonManager.DTO;
using HackathonManager.Models;
using HackathonManager.PocoModels;
using HackathonManager.Sms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Events.Event
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<Mentor> Mentors { get; set; }

        public IndexModel()
        {
            Mentors = new List<Mentor>();
        }

        public void OnGet()
        {
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
            //string id = (string)hubContext.Clients.All.GetConnectionId().Result;

            var Db = Startup.DbRepo;

            //var db = Context.GetMLabsMongoDbRepo();
            Mentors = Db.All<Mentor>().Where(x => x.IsPresent == true).ToList().OrderBy(e => e.IsAvailable ? 0 : 1).ToList();
            //model = Db.All<Mentor>().Where(x => x.IsPresent == true).ToList();

            //mentorViewModel.PresentMentors = repo.All<Mentor>().Where(x => x.IsPresent == true).ToList();
            //mentorViewModel.AvailableMentors = repo.All<Mentor>().Where(x => x.IsAvailable == true).ToList();
        }

        [HttpPost]
        public ActionResult TeamLogin(int teamPin)
        {
            var Db = Startup.DbRepo;

            var cookie = Request.Cookies["team"];

            //CHECK IF A TEAM BY THAT PIN NUMBER EXSISTS
            if (Db.Single<Team>(x => x.PinNumber == teamPin) != null)
            {
                Team team = Db.Single<Team>(x => x.PinNumber == teamPin);

                if (cookie == null)
                {
                    //var option = new CookieOptions();
                    //option.Expires = DateTime.UtcNow.AddDays(3);
                    //Response.Cookies["team"].Value = team.Name;
                    //Response.Cookies.Append("string", team.Name, option);
                    Response.Cookies.Append("team", team.Name, new CookieOptions() { Expires = DateTime.UtcNow.AddDays(3) });

                    //Response.Cookies["team"].Expires = DateTime.UtcNow.AddDays(3);
                }
                if (cookie != null && cookie.ToString() == "")
                {
                    //Response.Cookies["team"].Value = team.Name;
                    //Response.Cookies["team"].Expires = DateTime.UtcNow.AddDays(3);
                    Response.Cookies.Append("team", team.Name, new CookieOptions() { Expires = DateTime.UtcNow.AddDays(3) });
                }
                //if (cookie != null && cookie.Value == "")
                //{
                //    Response.Cookies["team"].Value = team.Name;
                //    Response.Cookies["team"].Expires = DateTime.UtcNow.AddDays(3);
                //}
            }

            return RedirectToAction("Index");
        }

        //public ActionResult LogOut()
        //{
        //    HttpCookie cookie = Request.Cookies["team"];
        //    if (cookie != null)
        //    {
        //        Response.Cookies["team"].Value = "";
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public ActionResult MentorRequest(int teamPin, Guid mentorGuidId)
        {
            var Db = Startup.DbRepo;
            var sms = Startup.SmsService;
            var request = new MentorRequest();
            Team team = null;
            Mentor mentor = null;
            try
            {
                team = Db.Single<Team>(x => x.PinNumber == teamPin);
                mentor = Db.Single<Mentor>(x => x.Id == mentorGuidId);
            }
            catch (Exception ex)
            {
                Db.Add<Log>(new Log() { Details = ex.ToString() });
            }

            request.Team = team;
            request.Mentor = mentor;
            var message = $"🔥 { mentor.FirstName}, team { team.Name}, located in { team.Location}, has requested your assistance.\n\n" +
    $"Reply with:\n" +
    $"Y to accept " +
    $"\nor\n " +
    $"N to reject the request";

            try
            {
                request.OutboundSms = sms.SendSms(mentor.PhoneNumber, message);
            }
            catch (Exception ex)
            {
                //IS THROWING EXCEPTION FOR NOT HAVING CORRECTLY SETUP NEWTONSOFT.JSON DEPENDENCY
                Db.Add<Log>(new Log() { Details = ex.ToString() });
            }

            try
            {
                SmsRoutingConductor.MentorRequests.Add(request);

                //THIS SHOULD BE HANDLED BY THE SMSROUTINGCONDUCTOR
                Db.Add<MentorRequest>(request);
            }
            catch (Exception ex)
            {
                Db.Add<Log>(new Log() { Details = ex.ToString() });
            }

            return RedirectToAction("Index");
        }
    }
}