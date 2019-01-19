using Lazztech.Events.Domain;
using Lazztech.Events.Dto;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Lazztech.Cloud.ClientFacade.Controllers
{
    public class HomeController : Controller
    {
        private IMentorRequestConductor _conductor;

        public HomeController(IMentorRequestConductor conductor)
        {
            _conductor = conductor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TeamLogin(int teamPin)
        {
            var Db = Startup.DbRepo;

            var cookie = Request.Cookies[StaticStrings.eventUserIdCookieName];

            //CHECK IF A TEAM BY THAT PIN NUMBER EXSISTS
            if (Db.Single<Team>(x => x.PinNumber == teamPin) != null)
            {
                Team team = Db.Single<Team>(x => x.PinNumber == teamPin);

                if (cookie == null)
                {
                    //var option = new CookieOptions();
                    //option.Expires = DateTime.UtcNow.AddDays(3);
                    //Response.Cookies[StaticStrings.eventUserIdCookieName].Value = team.Name;
                    //Response.Cookies.Append("string", team.Name, option);
                    Response.Cookies.Append(StaticStrings.eventUserIdCookieName, team.Name, new CookieOptions() { Expires = DateTime.UtcNow.AddDays(3) });

                    //Response.Cookies[StaticStrings.eventUserIdCookieName].Expires = DateTime.UtcNow.AddDays(3);
                }
                if (cookie != null && cookie.ToString() == "")
                {
                    //Response.Cookies[StaticStrings.eventUserIdCookieName].Value = team.Name;
                    //Response.Cookies[StaticStrings.eventUserIdCookieName].Expires = DateTime.UtcNow.AddDays(3);
                    Response.Cookies.Append(StaticStrings.eventUserIdCookieName, team.Name, new CookieOptions() { Expires = DateTime.UtcNow.AddDays(3) });
                }
                //if (cookie != null && cookie.Value == "")
                //{
                //    Response.Cookies[StaticStrings.eventUserIdCookieName].Value = team.Name;
                //    Response.Cookies[StaticStrings.eventUserIdCookieName].Expires = DateTime.UtcNow.AddDays(3);
                //}
            }
            //return Redirect("");
            return RedirectToPage("/Events/Event/Index");
        }

        //public ActionResult LogOut()
        //{
        //    HttpCookie cookie = Request.Cookies[StaticStrings.eventUserIdCookieName];
        //    if (cookie != null)
        //    {
        //        Response.Cookies[StaticStrings.eventUserIdCookieName].Value = "";
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public ActionResult MentorRequest(string teamName, string teamLocation, Guid mentorGuidId)
        {
            var Db = Startup.DbRepo;
            var sms = Startup.SmsService;
            bool succeded = false;

            var mentor = Db.Single<Mentor>(x => x.Id == mentorGuidId);
            if (mentor != null)
            {
                try
                {
                    var message = EventStrings.OutBoundRequestSms(mentor.FirstName, teamName, teamLocation);
                    var request = new MentorRequest()
                    {
                        UniqueRequesteeId = teamName,
                        Mentor = mentor,
                        OutboundSms = sms.SendSms(mentor.PhoneNumber, message)
                    };
                    succeded = _conductor.TryAddRequest(request);
                    if (succeded)
                        Db.Add<MentorRequest>(request);
                }
                catch (Exception ex)
                {
                    Db.Add<Log>(new Log() { Details = ex.ToString() });
                }
            }

            if (succeded)
                return RedirectToPage("/Events/Event/Index", new { message = "Your request has been sent! Please wait for your reply." });
            else
                return RedirectToPage("/Events/Event/Index", new { alert = "The mentor you selected is currently helping someone else! Please select another." });
        }
    }
}