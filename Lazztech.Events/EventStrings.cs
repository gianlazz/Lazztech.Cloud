using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Domain
{
    public class EventStrings
    {
        public static string OutBoundRequestSms(string firstName, string teamName, string teamLocation)
        {
            return $"🔥 { firstName}, team { teamName }, located in { teamLocation }, has requested your assistance.\n\n" +
                    $"Y: To accept \n" +
                    $"N: To reject the request";
        }

        public static string MentorRegistrationResponse(string firstName)
        {
            return $"Welcome {firstName}, you will interact with the service through SMS. " + 
                "When you recieve a request you will be prompted with a message with details on how to respond." + Environment.NewLine + Environment.NewLine +
                "You may also enter the following at any time." + Environment.NewLine +
                "Guide: Reads out valid responses" + Environment.NewLine +
                "Busy: Sets you as unavailable" + Environment.NewLine +
                "Available: Sets you as available";
        }
    }
}
