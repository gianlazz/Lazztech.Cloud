using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade
{
    public class StaticStrings
    {
        /// <summary>
        /// Used for the key name for cookies when identifing the device of the user
        /// for event requests.
        /// </summary>
        public static string eventUserIdCookieName => "uniqueUserId";
        public static string dataDir => @"/lazztech_data/";
    }
}
