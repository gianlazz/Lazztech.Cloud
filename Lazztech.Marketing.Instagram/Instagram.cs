using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lazztech.Marketing.Instagram
{
    public class Instagram
    {
        private readonly UserSessionData _userSession;
        private readonly IInstaApi _instaApi;

        public Instagram(string userName, string password)
        {
            _userSession = new UserSessionData
            {
                UserName = userName,
                Password = password
            };

            _instaApi = InstaApiBuilder.CreateBuilder()
                // required
                .SetUser(_userSession)
                // optional
                .UseLogger(new DebugLogger(LogLevel.Exceptions))
                // optional
                //.UseHttpClient(new HttpClient())
                // optional
                //.UseHttpClientHandler(httpHandlerWithSomeProxy)
                // optional
                //.SetRequestDelay(new SomeRequestDelay())
                // optional
                //.SetApiVersion(SomeApiVersion)
                .Build();
            InitiateSession().RunSynchronously();
        }

        private async Task InitiateSession()
        {
            const string stateFile = "state.bin";
            try
            {
                // load session file if exists
                if (File.Exists(stateFile))
                {
                    Console.WriteLine("Loading state from file");
                    using (var fs = File.OpenRead(stateFile))
                    {
                        _instaApi.LoadStateDataFromStream(fs);
                        // in .net core or uwp apps don't use LoadStateDataFromStream
                        // use this one:
                        // _instaApi.LoadStateDataFromString(new StreamReader(fs).ReadToEnd());
                        // you should pass json string as parameter to this function.
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (!_instaApi.IsUserAuthenticated)
            {
                // login
                Console.WriteLine($"Logging in as {_userSession.UserName}");
                var logInResult = await _instaApi.LoginAsync();
                if (!logInResult.Succeeded)
                {
                    Console.WriteLine($"Unable to login: {logInResult.Info.Message}");
                    return;
                }
            }
            // save session in file
            var state = _instaApi.GetStateDataAsStream();
            // in .net core or uwp apps don't use GetStateDataAsStream.
            // use this one:
            // var state = _instaApi.GetStateDataAsString();
            // this returns you session as json string.
            using (var fileStream = File.Create(stateFile))
            {
                state.Seek(0, SeekOrigin.Begin);
                state.CopyTo(fileStream);
            }
        }
    }
}
