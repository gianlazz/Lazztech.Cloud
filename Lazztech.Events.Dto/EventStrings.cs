using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Dto
{
    public class EventStrings
    {
        public static string OutBoundRequestSms(string firstName, string teamName, string teamLocation)
        {
            return $"🔥 { firstName}, team { teamName }, located in { teamLocation }, has requested your assistance.\n\n" +
                    $"Y: To accept \n" +
                    $"N: To reject the request";
        }
    }
}
