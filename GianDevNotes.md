## Docker notes
docker pull gianlazzarini/lazztech_containerizedface_recognition
docker ps

docker run -it -v "Full path to images directory without quotes" --entrypoint /bin/bash "Image id without quotes"

docker run -it -v /Users/gianlazzarini/Desktop/face_recognition:/face/ --entrypoint /bin/bash ba34ace8a4cc

docker system prune -a

Docker links:
https://stackoverflow.com/questions/39988844/docker-compose-up-vs-docker-compose-up-build-vs-docker-compose-build-no-cach
https://www.digitalocean.com/community/tutorials/how-to-remove-docker-images-containers-and-volumes


## july, 27, 2018
- [x] Finished getting docker-compose.yml configured to bind mount volumes to services by absolute path.

Why doesn't docker-compose up work? It always says that it can't find the docker-compose.dcsproj. Running the docker-compose.dcsproj from vs4mac doesn't seem to have this issue if I run the command then run it in vs4mac... Weird.

Having issues with the working directory for the Process object running the correct path for the face_recognition arguments.

Run this line while connected to the vpn to move over all of the images from the rpi to my dev machine:

`rsync -a --progress --remove-source-files pi@pi1:/var/lib/motion Desktop/face_recognition/`

Also I may want to make `/var/lib/motion` a bind mount path volume in the container?

## july, 28, 2018
Using details like the image capture date from the metadata to populate the Snapshot object's properties would likely be a
good idea. Here's some links I found on getting these values.
https://medium.com/@dannyc/get-image-file-metadata-in-c-using-net-88603e6da63f
https://docs.microsoft.com/en-us/dotnet/framework/winforms/advanced/how-to-read-image-metadata
https://stackoverflow.com/questions/2280948/reading-data-metadata-from-jpeg-xmp-or-exif-in-c-sharp
https://www.dreamincode.net/forums/topic/231165-how-to-read-image-metadata-in-c%23/

Write if else logic in FacialRecognitionManager.CheckAllAssetsValid();

## july, 30, 2018
Succumbing to the complexity of FacialRecognitionManager.cs as it develops and turning to PDD(pain driven developmetn). I'm working to transistion to using vscode as much as possible as my primary development enviroment.
```
mkdir [Project layer name for dir]
cd [new project folder]
dotnet new classlib -f netcoreapp2.1
cmd + shift + p => nuget add:
NUnit
NUnit3TestAdapter
Microsoft.NET.Test.Sdk
cd ..
dotnet sln add [new project folder/newproject.csproj]
```

## july, 31, 2018
Project still builds though it's throwing warnings. Setting up CI could be a good idea though I want to keep the repo private. Maybe a heroku jenkins instance? Or azure jenkins? I do have credits. It's probably too soon to worry about that though. However since the project is dockerized it could be more simple when the time comes.

Setup interface for FacialRecognitionManager and a unit test against the resulting Snapshot collection.

Adding reference to domain to the testing layer requires a kind of long chaned command:

`dotnet add Lazztech.ObsidianPresenses.Vision.Microservice.Tests/Lazztech.ObsidianPresenses.Vision.Microservice.Tests.csproj reference Lazztech.ObsidianPresenses.Vision.Microservice.Domain/Lazztech.ObsidianPresenses.Vision.Microservice.Domain.csproj`

`dotnet add [Project to be added] reference [project to get reference to other project]`

References are all setup and I've gotten together a very simple unit test though I've got issues with the Nunit3TestAdapter.

Oh this is slightly more complicated. I need to make sure I'm testing against how this class processes the lines output from the cmd line tools not that those perform. Otherwise I'd have to run the docker-compose project to get the domain tested and that would make it more of an integration test instead of a unit test.

- [x] Setup test data for realistic output of cmdline process stdout that I'm parsing into a desired valid collection of Snapshots.

- [ ] Break out responsabilities for the process execution so that it's an interface that returns the List<string> stdout from the respective processes that I'm testing against for the facial recognitiona and facial coordinates.

- [ ] resolve "Some projects have trouble loading. Please review the output for more details" eventually.

- [x] Created mock for IFacialIdentityHandler using the static stdout test data string

Now I need to setup dependency injection into the FacialRecognitionManager in the ctor so that I can inject the mock or real interface implementation for the handling of the process.

Leaving the project in a non-building state to be resolved in the future.

## august, 1, 2018
- [x] dotnet build Lazztech.ObsidianPresences.sln fails: There is no argument given that corresponds to the required formal parameter 'facialIdentityHandler'
Solution builds again: `dotnet build Lazztech.ObsidianPresences.sln`
Maybe it would be easier to just use xunit instead? I'm having some issues debugging and running the NUnit tests fixture.
Yeah it looks like I'm going to have a smoother development experience if I transistion to using xunit instead of NUnit with with dotnet core.
https://xunit.github.io/docs/why-did-we-build-xunit-1.0.html
```
dotnet sln remove Lazztech.ObsidianPresenses.Vision.Microservice.Tests/Lazztech.ObsidianPresenses.Vision.Microservice.Tests.csproj
rm -r Lazztech.ObsidianPresenses.Vision.Microservice.Tests/
mkdir Lazztech.ObsidianPresenses.Vision.Microservice.Tests/
cd Lazztech.ObsidianPresenses.Vision.Microservice.Tests
dotnet new xunit
cd ..
dotnet sln add Lazztech.ObsidianPresenses.Vision.Microservice.Tests/Lazztech.ObsidianPresenses.Vision.Microservice.Tests.csproj
dotnet add Lazztech.ObsidianPresenses.Vision.Microservice.Tests/Lazztech.ObsidianPresenses.Vision.Microservice.Tests.csproj reference Lazztech.ObsidianPresenses.Vision.Microservice.Domain/Lazztech.ObsidianPresenses.Vision.Microservice.Domain.csproj
```
I then clicked restore on the vscode prompt.

All switched to xUnit now however the test failes because "Access to the path '/face/known/' is denied." so I'll need to make sure that all is handled by the interface implementation that I'm mocking not the FacialRecognitionManager.cs that depends on them and handles their stdout.

Switching to xunit seems to have resolved the "Some projects have trouble loading. Please review the output for more details"
- [x] resolve "Some projects have trouble loading. Please review the output for more details" eventually.
vscode test explorer issue:
MSBUILD : error MSB1011: Specify which project or solution file to use because this folder contains more than one project or solution file.

I think I need to add configrations for the other project layers to the vscode tasks.json and launch.json to get the test explorer working.
https://code.visualstudio.com/docs/editor/tasks
https://code.visualstudio.com/docs/editor/debugging
https://stackoverflow.com/questions/41483477/what-is-the-difference-between-launch-json-and-task-json-in-visual-studio-code
Okay the .NET TEST EXPLORERER: issue "Please open or set the test project..." could actually be a bug.
https://github.com/formulahendry/vscode-dotnet-test-explorer/issues/39

https://github.com/formulahendry/vscode-dotnet-test-explorer
Trying adding `"dotnet-test-explorer.testProjectPath": "Lazztech.ObsidianPresenses.Vision.Microservice.Tests/Lazztech.ObsidianPresenses.Vision.Microservice.Tests.csproj",` to the workspace settings.

## august 2, 2018
FacialRecognitionManager ctor was interacting with the explicit paths causinging and issue with testability. After removing those lines in the ctor the xUnit test case passes.
Also I wonder if there may be an issue with the xUnit tests not showing up in the explorer from missing the test adapter? Does that come packaged in the dotnet new xunit proj?
I'm also unable to get the vs4mac debugger working on the unit tests... It just executes them and returns 0 however never hits any of the breakpoints.
I need to fix the issue with the unit test debugging not hitting their breakpoints however I do think that the issue is in .CheckAllValidAssets() where it's consulting with the directories for the file count. That needs to be moved to the interfaces as well.
Do I need an interface for .CheckAllValidAssets() to mock it or can I move it into an existing other mockabible responsability? 
I suppose this method really is doing more than one responsability it'self too. I should probably break those out. It's adding all relivant directorys to the collections then it's also new'ing up respective snapshot objects foreach of those.
I've moved the first responsability to a method called .CollectAllImageDirs(); and the second to a method called .InstantiateSnapshotsFromDirs();

Oh, well the xUnit debugging actually works alright in vscode as is so I'll continue with that. I looks like if I comment out the issues related to directories then it still throws an exception due to not having valid Snapshots constructed. This issue is in HandleIdentities. Oh gotcha it's because the InstantiateSnapshotsFromDirs needs to probably happen in the interfaces as well... It's looking for valid Snapshots that should already exist at this point however don't.

I need a file directory finding interface for mocking.
God-damn, making this testable seems like it's quadrupaling the amount of code...

I think I need to put everything that uses the System.IO namespace in an interface that would be mocked since all of that will likely not matche the runtime enviroment in the docker container.

The next remaining testability issue is the InstantiateSnapshotsFromDirs() method setting the Snapshot.DateTimeWhenCaptured = File.GetCreationTime(imageDir) which will need to be mocked as it's trying to reference the externality through System.IO which will most likely only exist during run time in the docker container.

## August Friday 3, 2018
Maybe I should just make an IFileServices that can be mocked...
Got the test explorer working today with xUnit project via workspace settings.

I'm giving IFileServices the responsabilities that IImageDirectoriesFinder had and removing IImageDirectoriesFinder.

What is this:

Failed to start debugger: "System.InvalidOperationException: Debug session already started.\n at 

`cmd + shift + p` then reloading the window seems to have fixed this.

Should I mock the snapshot.ImageDir setting? Also now I'm getting exceptions with the Snapshot.Status code. Things are definitly moving along now.

Actually yeah I'm pretty close as I seem to have mocked all System.IO code with IFileServices now. Also if I comment out HandleIdentities() the basic non-null return value test passes.

Does the code actually run like I expect it in the docker container thought? Also the thought of integration tests has crossed my mind again..

Yes it works in the docker container! Now I have to get HandleIdentities() to work with the tests also I need to extract FaceDetection to it's own interface for the coordinates process. Then I can mock it and start testing the logic for instantiating the coordinates correctly from the process stdout.

#### Achieved unit testablility! ~9:00PM on 108th commit id #81379b50
- [x] Passed first unit test against FacialRecognitionManager with it now fully testable

I would also like to look into logging my hours some how in a serializable way in the repo.

## August Saturday, 4 2018
This tool looks perfect for tracking my time programming. This will go nicely with these markdown files and the .json based kanban board. Hopefully it's json based so I can keep it in the repo.
https://github.com/wakatime/vscode-wakatime
https://wakatime.com/vs-code-time-tracking
Hmm it looks great but it looks like it's not really git backed and has a monthly membership for all of the features... I want something .json based and gitbacked.

More git based time trackers:
https://github.com/git-time-metric/gtm
https://git-time-metric.github.io
https://github.com/timeglass/glass
http://gitstats.sourceforge.net/examples/git/activity.html
https://github.com/kimmobrunfeldt/git-hours

https://docs.gitlab.com/ee/workflow/time_tracking.html
https://kris.cool/2015/09/time-tracking-with-git

Also I could use some kind of json based pomodoro tool for time tracking... May end up going that route.

It could also be nice to visualize statistics from the git activity? And also I should look into the .net test explorer code coverage output and see if that's viewable too?

https://stackoverflow.com/questions/1542213/how-to-find-the-number-of-cpu-cores-via-net-c

https://stackoverflow.com/questions/565075/how-to-set-the-value-of-a-read-only-property-with-generic-getters-and-setters

Added new tests now I'm looking seriously into how to set the coordinates of the face bounding boxes. I wonder if I should also add the image dimensions to the snapshot property since the bounding box coordinates use "pixel coordinates"

#### How do I debug long chained linq expressions?
https://stackoverflow.com/questions/118341/how-to-debug-a-linq-statement
https://www.linqpad.net scott hanselman even advocates for this one apperantly
https://stackoverflow.com/questions/952796/debugging-linq-queries
```
.Select(z =>
{return z;}
)
```
https://blogs.msdn.microsoft.com/ericwhite/2008/11/06/debugging-linq-queries/
https://www.red-gate.com/simple-talk/dotnet/net-development/linq-debugging-visualization/
https://www.red-gate.com/simple-talk/dotnet/net-framework/linq-secrets-revealed-chaining-and-debugging/
Also apperantly vs2017 can evaluate lamdas? I wonder about vscode?
https://stackoverflow.com/questions/18620819/view-result-of-linq-query-in-watch-debugger

https://stackoverflow.com/questions/11440911/how-to-debug-linq-result

Also I was curious and looked up this because I was unsure if it was correct to call it a linq statment or expression. It seems like
it's an expression because it has a return value where as a statement just does an action.
https://stackoverflow.com/questions/19132/expression-versus-statement

Also heres information comparing the two different linq syntax styles of using the querying keywords or using the extension methods like I have been.
https://stackoverflow.com/questions/796246/what-is-the-difference-between-linq-query-expressions-and-extension-methods

Oh and I've been thinking a lot about eventually putting together an Application Facade for all of the microservices as a graphql api instead of just a REST api. I want to eventually look into using linq against a graphql web api as I think I heard something about that in a podcast.

Based on the python code example for face_recognition bounding box image drawing it looks like the first two numbers are the x,y for the (left, top) then the (right, bottom). I should probably name the property accordingly.
https://github.com/ageitgey/face_recognition/blob/master/examples/identify_and_draw_boxes_on_faces.py

Look up structs vs primitives.

It looks like I'm having an issue with capturing the new line \r from the stdout for the person.Name properties:
```p.Name == "Gian Lazzarini\r"```
vs 
```p.Name == "Gian Lazzarini")```
https://stackoverflow.com/questions/873043/removing-carriage-return-and-new-line-from-the-end-of-a-string-in-c-sharp

Also I wonder if this return carriage only is happening because of how I pasted in the stdout lines instead of just having it line by line? I suppose I should probably debug it during actual containerized execution to check...

I wish vscode would change to the light theme when it's bright out or even adjust between dark and light depending on the sunlight. That way I could keep maximum viewability without having to change anything manually.

* I have to run .ToList() on a .Where() linq query before .Any() will work.

## Sunday, August 5, 2018
Finished fixing the name return carriage issue however I'm still not certain it's actually a reall issue and not a test data mock problem.

After getting the FaceBoundingBox's set for the person in the relivant snapshot I should really start testing cases where there's multiple people. A lot of my code has expressions like : `Snap.Persons.First().FaceBoundingBox = bb;`

Using TDD to solve that will be helpful.

https://stackoverflow.com/questions/30878856/is-there-a-shortcut-to-hide-the-side-bar-in-visual-studio-code

I wish I could edit this .md in the preview, rendered view like in Forestry.io with my blog.

vscode shortcut to move through tabs:
`cmd + option + Right or Left`
or
`ctrl + [number of tab like 1, 2, or 3 etc.]`

Wtf I passed the test then ran it again and now it's saying it fails though the output looks passing...
```
[xUnit.net 00:00:00.6506820]       Expected: FaceBoundingBox { LeftTopCoordinate = PixelCoordinateVertex { x = 54, y = 181 }, RightBottomCoordinate = PixelCoordinateVertex { x = 158, y = 77 } }
[xUnit.net 00:00:00.6507420]       Actual:   FaceBoundingBox { LeftTopCoordinate = PixelCoordinateVertex { x = 54, y = 181 }, RightBottomCoordinate = PixelCoordinateVertex { x = 158, y = 77 } }
```

Looks like I may have a fundimental miss-understanding about oop instance equality:
https://grantwinney.com/how-to-compare-two-objects-testing-for-equality-in-c/

Yeah so it looks like it's checking to see if it's the exact same instance which it isn't however the values are the same for the two different objects. That's what I want to make assertions for. I wonder if xUnit has an assertion for that? I could hash both and compare that?
Hmm hashing them and comparing that fails too.
```
[xUnit.net 00:00:00.6413380]       Assert.Equal() Failure
[xUnit.net 00:00:00.6414400]       Expected: 63403007
[xUnit.net 00:00:00.6415010]       Actual:   4916187 
```

"The rules are different for a struct but I'm not even going to cover that... 8 years using C# professionally and I don't recall creating my own struct once. If you're interested though, you can find more info in the C# guide."

Hmm maybe I should make the FaceBoundingBox a struct? Sure, why not. Seems like it lends well to it then I can write custom equations against it more simply. Yup changing the FaceBoundingBox and it's PixelCoordinateVertext properties to structs from classes allowed my Assert.Equal(a, b); statment to pass like I expected.

Test is passing however it's throwing an exception when actually run in the docker-compose project.

Hmm I think it's an issue with looking at the snapshots being created from the images in the known/ dir that don't have people? Idk why but I need to identify why any would not have people and make sure it's not something I'm looking for to assign bounding boxes against as it will throw an exception. Also I still don't know why the unit test isn't throwing an exception...

Got it working in the docker-compose and outputing the snapshots with the coordinates on the people populated properly. I also have all of the tests passing too however it's concerning that the tests didn't reflect the exception being thrown when actually running the docker-compose project... Also I think it's time to configure the docker compose with vscode to stop using vs4mac.

This throws an exception in the docker runtime however passes all of the tests
```
        private void HandleBoundingBoxes()
        {
            var lines = face_detectionLines;
            // var snaps = Results.Where(x => x.Status != SnapshotStatus.no_persons_found).ToList();
            // var snapsWithPeople = snaps.Where(x => x.People.Any()).ToList();

            //FOR DEBUGGING DOCKER RUN TIME EXCEPTION VS UNIT TEST
            var snapsWithPeople = Results.Where(x => x.Status != SnapshotStatus.no_persons_found).ToList();
```

This also passes all of the tests however fixes the exception in the docker runtime.
```
        private void HandleBoundingBoxes()
        {
            var lines = face_detectionLines;
            var snaps = Results.Where(x => x.Status != SnapshotStatus.no_persons_found).ToList();
            var snapsWithPeople = snaps.Where(x => x.People.Any()).ToList();

            // //FOR DEBUGGING DOCKER RUN TIME EXCEPTION VS UNIT TEST
            // var snapsWithPeople = Results.Where(x => x.Status != SnapshotStatus.no_persons_found).ToList();
```

Make sure that there's a test to reflect this failure

Alright to do this it's time to get the docker-compose.dcsproj running in vscode. So that probably means I'll finally figure out why vs4mac can run it however doing `docker-compose up` doesn't work.
```
Step 12/21 : RUN dotnet restore -nowarn:msb3202,nu1503
 ---> Running in c0fe86f9318e
/usr/share/dotnet/sdk/2.1.302/NuGet.targets(239,5): error MSB3202: The project file "/src/Lazztech.ObsidianPresenses.Vision.Microservice.Tests/Lazztech.ObsidianPresenses.Vision.Microservice.Tests.csproj" was not found. [/src/Lazztech.ObsidianPresences.sln]
/usr/share/dotnet/sdk/2.1.302/NuGet.targets(239,5): error MSB3202: The project file "/src/docker-compose.dcproj" was not found. [/src/Lazztech.ObsidianPresences.sln]
ERROR: Service 'lazztech.obsidianpresenses.vision.microservice.cli' failed to build: The command '/bin/sh -c dotnet restore -nowarn:msb3202,nu1503' returned a non-zero code: 1
```
Hmm it also looks like vs4mac can't run it in release either, only debug. I wonder if that could be related? Yup it looks like it is... vs4mac build error output says:
```
Step 12/21 : RUN dotnet restore -nowarn:msb3202,nu1503
     ---> Running in 03ee26a83aea
    /usr/share/dotnet/sdk/2.1.302/NuGet.targets(239,5): error MSB3202: The project file "/src/Lazztech.ObsidianPresenses.Vision.Microservice.Tests/Lazztech.ObsidianPresenses.Vision.Microservice.Tests.csproj" was not found. [/src/Lazztech.ObsidianPresences.sln]
    /usr/share/dotnet/sdk/2.1.302/NuGet.targets(239,5): error MSB3202: The project file "/src/docker-compose.dcproj" was not found. [/src/Lazztech.ObsidianPresences.sln]
    Service 'lazztech.obsidianpresenses.vision.microservice.cli' failed to build: The command '/bin/sh -c dotnet restore -nowarn:msb3202,nu1503' returned a non-zero code: 1
    /Applications/Visual Studio.app/Contents/Resources/lib/monodevelop/AddIns/docker/MonoDevelop.Docker/MSbuild/Sdks/Microsoft.Docker.Sdk/build/Microsoft.Docker.targets(111,5): error : Building lazztech.obsidianpresenses.vision.microservice.cli
    /Applications/Visual Studio.app/Contents/Resources/lib/monodevelop/AddIns/docker/MonoDevelop.Docker/MSbuild/Sdks/Microsoft.Docker.Sdk/build/Microsoft.Docker.targets(111,5): error : Service 'lazztech.obsidianpresenses.vision.microservice.cli' failed to build: The command '/bin/sh -c dotnet restore -nowarn:msb3202,nu1503' returned a non-zero code: 1.
    /Applications/Visual Studio.app/Contents/Resources/lib/monodevelop/AddIns/docker/MonoDevelop.Docker/MSbuild/Sdks/Microsoft.Docker.Sdk/build/Microsoft.Docker.targets(111,5): error : 
    /Applications/Visual Studio.app/Contents/Resources/lib/monodevelop/AddIns/docker/MonoDevelop.Docker/MSbuild/Sdks/Microsoft.Docker.Sdk/build/Microsoft.Docker.targets(111,5): error : For more troubleshooting information, go to http://aka.ms/DockerToolsTroubleshooting
Done building target "DockerComposeBuild" in project "docker-compose.dcproj" -- FAILED.
```

vs4mac outputs this which gives a clue to what's happening when it's running the docker-compose project in debug.
```
Starting: "docker" exec -i 2d9d413a4180 "/remote_debugger/vsdbg" --interpreter=vscode
```

https://code.visualstudio.com/docs/azure/docker
https://marketplace.visualstudio.com/items?itemName=formulahendry.docker-explorer

https://developercommunity.visualstudio.com/content/problem/216100/build-docker-compose-on-release-fails-on-new-proje.html
https://github.com/dotnet/dotnet-docker/pull/430
Okay so it failing when running `docker-compose up` could be related to the version of the base image.

No actually it looks like it's just throwing an error because it can't find the .Tests project? Oh okay after adding this line below it resolved that issue and now is back to a familiar one.
```COPY Lazztech.ObsidianPresenses.Vision.Microservice.Tests/Lazztech.ObsidianPresenses.Vision.Microservice.Tests.csproj Lazztech.ObsidianPresenses.Vision.Microservice.Tests/```
Now it says this which looks to me like I need to make sure that docker file copies over every project in the solution file? I'm not sure it would sense for one file to have the docker-compose but I'll give it a try.
```/usr/share/dotnet/sdk/2.1.302/NuGet.targets(239,5): error MSB3202: The project file "/src/docker-compose.dcproj" was not found. [/src/Lazztech.ObsidianPresences.sln]```

Maybe I need to have the sln clarify when it needs the docker-compose.dcsproj?
Okay now, after adding the docker-compose proj that the solution needs it says:
```
MSBUILD : error MSB1011: Specify which project or solution file to use because this folder contains more than one project or solution file.
```

I still need to figure out how to run the docker-compose.dcproj in vscode so that I can debug it in vscode.
I think I may need to experiment with seeing how vscode configures the docker files for my solution with the `Docker: Add Docker files to Workspace` command from the command pallete. Also it looks like I need a file called `docker-compose.debug.yml` which I do not have from vs4mac.
https://code.visualstudio.com/docs/azure/docker#_generating-docker-files

Running `Docker: Add Docker files to Workspace` didn't make the `docker-compose.debug.yml` like I wanted. The dockerfile it generates does however look better configured in that it just builds/restores against the specific .proj instead of bundling the entire solution. I may look into that further.

It looks like I'm going to need to configure some launch configurations with remote debugging for the docker container.
https://code.visualstudio.com/docs/editor/tasks
https://code.visualstudio.com/docs/editor/debugging

https://techblog.dorogin.com/running-and-debugging-net-core-unit-tests-inside-docker-containers-48476eda2d2a
https://github.com/OmniSharp/omnisharp-vscode/wiki/Attaching-to-remote-processes
https://github.com/Microsoft/MIEngine/wiki/Offroad-Debugging-of-.NET-Core-on-Linux---OSX-from-Visual-Studio
https://github.com/sleemer/docker.dotnet.debug

https://stackoverflow.com/questions/46500639/how-do-i-use-docker-compose-debug-yml-to-debug-my-node-running-in-docker

https://github.com/VSChina/debug-dotnetcore-in-docker
https://stackoverflow.com/questions/48837155/possible-remote-debugging-net-core-linux-docker-container-with-vscode-from-mac
https://github.com/OmniSharp/omnisharp-vscode/wiki/Attaching-to-remote-processes
https://stackoverflow.com/questions/48596181/how-to-debug-net-core-project-with-docker-support-using-visual-studio-on-a-rem
https://stackoverflow.com/questions/48381127/debugging-already-running-docker-linux-net-core-container-with-visual-studio-20
http://thepassingthoughts.blogspot.com/2018/01/debugging-net-core-application-running.html
http://blog.jonathanchannon.com/2017/06/07/debugging-netcore-docker/
https://developers.redhat.com/blog/2018/06/13/remotely-debug-asp-net-core-container-pod-on-openshift-with-visual-studio/
https://blog.quickbird.uk/debug-netcore-containers-remotely-9a103060b2ff
https://developers.redhat.com/blog/2017/10/23/remote-debug-asp-net-core-container-openshift-visual-studio-code/
https://dzone.com/articles/remotely-debugging-aspnet-core-containers-on-opens

This remote debugging setup could actually let me debug or profile the code when it runs on my raspberry pi cluster too.

Also how do I profile performance with .net core code in vscode? In visual studio it's straight forward with the built in profiler. And would this work remotely in a docker container?

Found this interesting documentation on building software-as-a-service apps: 
"The format is inspired by Martin Fowler’s books Patterns of Enterprise Application Architecture and Refactoring."
https://github.com/docker/labs/blob/master/12factor/README.md
https://12factor.net

https://stackoverflow.com/questions/46382883/shortcut-to-push-code-to-git-in-vscode

## Tuesday, August 7, 2018

Familiarize youreslf with `docker-compose.debug.yml` as that seems to be an important missing file to do the remote debugging with vscode like I need and am doing with vs4mac easily. Once I have the rest of the setup on par with vs4mac then I can continue to identify why the docker run-time throws that exception that the unit tests aren't catching. It could be an issue with the test data not accurately reflecting the run time or possibly something to do with how I've configured the mocks. Either way I can't move forward until I'm able to test and solve that and my goal is to move away from using anything but vscode.

http://www.clearpeople.com/insights/blog/2018/june/finding-docker-compose-vs-debug-yml

Okay so it looks like in regular visual studio/vs4mac the equivalent is generated automatically from building the `docker-compose.dcproj` which looks at the docker-compose.yml then builds `docker-compose.vs.debug.g.yml` and `docker-compose.vs.release.g.yml` which is in the untracked binary output from the projects in the `./obj/Docker`. I wonder if creating a launch action for the `docker-compose.dcproj` in vscode would work?

How much do I really want/need whatever vs2017/vs4mac is automatically doing? It could be better just to manage it all myself via the manual `docker-compose.debug.yml` file way that vscode does and the blog post above illustrates also seems to override the automatic generation in vs2017/vs4mac.

Also I forgot about the `docker-compose.override.yml` that vs4mac created and caused issues for me. Idk what the point of that actually was in the first place?

I wonder if deleting all of the docker related files in the workspace then running `Docker: Add Docker files to Workspace` will add the files like I wanted?

I'm not sure that the docker-compose.dcproj actually makes a dll.

`dotnet sln remove docker-compose.dcproj`

Okay so creating an entirely new solution still doesn't generate all the docker related files like the documentation says. And this is with the Docker 0.1.0 extension...
I did this or something like it:
```
mkdir newsolutionfolder
cd newsolutionfolder
dotnet new sln
mkdir exampleproject
cd exampleproject
dotnet new console
cd ..
dotnet sln add exampleproject/exampleproject.csproj
dotnet build newsolutionfolder.sln
cmd + shift + p : Docker: Add Docker files to Workspace
```

I could probably raise an issue about this on https://github.com/Microsoft/vscode-docker as there it says it only makes the two files.

This linke raises the fact that the docker-compose.dcproj causes issues with vs code:
https://softwareengineering.stackexchange.com/questions/369429/net-core-using-visual-studio-and-keeping-it-cross-platform
Then  this person replies:
"Thank you for the answer. It looks like they're aware of this issue and have a solution: https://github.com/dotnet/cli/issues/6178"

I don't have issues building like in that github issue though so that must've been solved by now. Lol and there's even a comment by Scott Hanselman in that issue!
https://github.com/dotnet/cli/issues/6178#issuecomment-348852554
It was fixed with: https://github.com/dotnet/cli/pull/8416

Maybe I just need to make the `docker-compose.debug.yml` and `docker-compose.release.yml` manually.

This link seems to illustrate my use case exactly. It talks about having multiple containers and configuring everything to work across regular vs on windows and vscode on mac/linux.
https://www.richard-banks.org/2018/07/debugging-core-in-docker.html

https://www.youtube.com/watch?v=D75NBrjRZzs

Okay so I keep on seeing that I'm going to need to instal vsdebugger in the container: curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

## Wednesday, Augsut 8, 2018

- Ran `docker system prune -a`
- Continueing to follow along with https://techblog.dorogin.com/running-and-debugging-net-core-unit-tests-inside-docker-containers-48476eda2d2a
Making the preLaunchTask “preDockerDebug”.
- Then ran `docker compose up`
dockerfile build output:
```
Downloading https://vsdebugger.azureedge.net/vsdbg-15-7-20425-2/vsdbg-linux-x64.zip

ERROR: Command 'unzip' not found. Install 'unzip' for this script to work.
ERROR: Service 'lazztech.obsidianpresenses.vision.microservice.cli' failed to build: The command '/bin/sh -c curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l ~/vsdbg' returned a non-zero code: 1
```
The launch configuration seems to be coming along however the container runs and then closes on completion. I believe from the documentation that I need to add a command to keep it running.
I think that it's just missing the build task.

Theres many mentions about this being unusually manual or "off road" from the conventional path as there's lots of manual configuration they don't seem to expect the typical .net developer to be familiar with.