using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HackathonManager.SrndResourcesManager
{
    public class SrndMentorCsvDownloader
    {
        private const string _url = @"https://clear.codeday.org/event/CBIo5VgIJQ24/registrations/downloadcsv?signature=33e3874e97f60e7d40a31ac68a01bd0379e0d08daecfbe8da0feb9351240c145";

        public string GetCsv()
        {
            //WebClient Client = new WebClient();
            //Client.DownloadFile(url, @"C:\MentorCsv.csv");

            //string contents;
            //using (var wc = new System.Net.WebClient())
            //    contents = wc.DownloadString(url);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();

            return results;
        }

        public string GetCsv(string url)
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
