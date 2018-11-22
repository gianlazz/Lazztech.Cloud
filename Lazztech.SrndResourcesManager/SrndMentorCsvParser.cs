using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HackathonManager.DTO;

namespace HackathonManager
{
    public class SrndMentorCsvParser
    {
        /* [0] = event
        * [1] = type
        * [2] = lastname
        * [3] = firstname
        * [4] = email
        * [5] = age
        * [6] = promocode
        * [7] = paid
        * [8] = parentname
        * [9] = parentemail
        * [10] = parentphone
        * [11] = parentphonealt
        * [12] = checkedin
        * [13] = created
        */

        public List<Mentor> Parse(string csv)
        {
            var mentors = new List<Mentor>();
            var mentorCsvLines = new List<string>();

            using (var reader = new StringReader(csv))
            {
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    //Skip the first header line
                    if (line.Contains("event"))
                        continue;
                    mentorCsvLines.Add(line);
                }
            }

            foreach (var line in mentorCsvLines)
            {
                var lineValues = line.Split(',').ToArray();
                var mentor = new Mentor()
                {
                    GuidId = Guid.NewGuid(),
                    FirstName = lineValues[3],
                    LastName = lineValues[2],
                    Age = (lineValues[5] != string.Empty) ? int.Parse(lineValues[5]) : 0,
                    Email = lineValues[4],
                    Event = lineValues[0],
                    MentorType = lineValues[1]
                };
                mentors.Add(mentor);
            }

            return mentors;
        }

        public List<Mentor> ParseFromSrndOnline()
        {
            return Parse(GetCsv(@"https://clear.codeday.org/event/CBIo5VgIJQ24/registrations/downloadcsv?signature=33e3874e97f60e7d40a31ac68a01bd0379e0d08daecfbe8da0feb9351240c145"));
        }

        private string GetCsv(string url)
        {
            //WebClient Client = new WebClient();
            //Client.DownloadFile(url, @"C:\MentorCsv.csv");

            //string contents;
            //using (var wc = new System.Net.WebClient())
            //    contents = wc.DownloadString(url);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();

            return results;
        }
    }
}
