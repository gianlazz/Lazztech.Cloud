## Docker notes
- **Pull from docker hub** `docker pull gianlazzarini/lazztech_containerizedface_recognition`
- **List all processes** `docker ps`
- **Open interactive terminal** `docker run -it -v "Full path to images directory without quotes" --entrypoint /bin/bash "Image id without quotes"`
- **Open interactive terminal example 2** `docker run -it -v /Users/gianlazzarini/Desktop/face_recognition:/face/ --entrypoint /bin/bash ba34ace8a4cc`
- **Another shorter way to open an interactive terminal** `docker exec -it f1c767df6163 sh`
- **Stop all running containers** `docker stop $(docker ps -aq)`
- **Delete all stopped container** `docker rm $(docker ps -aq)`
- **Delete all stopped containers and images** `docker system prune -a`
- **List docker networks** `docker network ls`
- **Inspect details about a docker process** `docker inspect "ps id"`
- **Copy data from docker container virtual volume** `docker cp $ID:/var/jenkins_home`
- **Automatically restart container if it's not running** `docker run INSERT HERE --restart always` https://docs.docker.com/config/containers/start-containers-automatically/
- **Run in background detateched daemon mode so you don't need to keep the terminal open and print id** `docker run -d or --detatch INSERT HERE` https://docs.docker.com/v1.11/engine/reference/commandline/run/
- **See the container startup stdout in detatched mode** `docker logs CONTAINER_ID`
- **Exit the current container while keeping it running** `Ctrl+p, Ctrl+q`

**Docker links:**
- https://stackoverflow.com/questions/39988844/docker-compose-up-vs-docker-compose-up-build-vs-docker-compose-build-no-cach
- https://www.digitalocean.com/community/tutorials/how-to-remove-docker-images-containers-and-volumes
- http://blog.baudson.de/blog/stop-and-remove-all-docker-containers-and-images


## Friday July, 27, 2018
- [x] Finished getting docker-compose.yml configured to bind mount volumes to services by absolute path.

Why doesn't docker-compose up work? It always says that it can't find the docker-compose.dcsproj. Running the docker-compose.dcsproj from vs4mac doesn't seem to have this issue if I run the command then run it in vs4mac... Weird.

Having issues with the working directory for the Process object running the correct path for the face_recognition arguments.

Run this line while connected to the vpn to move over all of the images from the rpi to my dev machine:

`rsync -a --progress --remove-source-files pi@pi1:/var/lib/motion Desktop/face_recognition/`

Also I may want to make `/var/lib/motion` a bind mount path volume in the container?

## Saturday July, 28, 2018
Using details like the image capture date from the metadata to populate the Snapshot object's properties would likely be a
good idea. Here's some links I found on getting these values.
https://medium.com/@dannyc/get-image-file-metadata-in-c-using-net-88603e6da63f
https://docs.microsoft.com/en-us/dotnet/framework/winforms/advanced/how-to-read-image-metadata
https://stackoverflow.com/questions/2280948/reading-data-metadata-from-jpeg-xmp-or-exif-in-c-sharp
https://www.dreamincode.net/forums/topic/231165-how-to-read-image-metadata-in-c%23/

Write if else logic in FacialRecognitionManager.CheckAllAssetsValid();

## Monday July, 30, 2018
## Sprint 0: Unit Tests & Snapshot Coordinates
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

## Tuesday July, 31, 2018
## Sprint 0: Unit Tests & Snapshot Coordinates
Project still builds though it's throwing warnings. Setting up CI could be a good idea though I want to keep the repo private. Maybe a heroku jenkins instance? Or azure jenkins? I do have credits. It's probably too soon to worry about that though. However since the project is dockerized it could be more simple when the time comes.

Setup interface for FacialRecognitionManager and a unit test against the resulting Snapshot collection.

Adding reference to domain to the testing layer requires a kind of long chaned command:

`dotnet add Lazztech.ObsidianPresences.Vision.Microservice.Tests/Lazztech.ObsidianPresences.Vision.Microservice.Tests.csproj reference Lazztech.ObsidianPresences.Vision.Microservice.Domain/Lazztech.ObsidianPresences.Vision.Microservice.Domain.csproj`

`dotnet add [Project to be added] reference [project to get reference to other project]`

References are all setup and I've gotten together a very simple unit test though I've got issues with the Nunit3TestAdapter.

Oh this is slightly more complicated. I need to make sure I'm testing against how this class processes the lines output from the cmd line tools not that those perform. Otherwise I'd have to run the docker-compose project to get the domain tested and that would make it more of an integration test instead of a unit test.

- [x] Setup test data for realistic output of cmdline process stdout that I'm parsing into a desired valid collection of Snapshots.

- [ ] Break out responsabilities for the process execution so that it's an interface that returns the List<string> stdout from the respective processes that I'm testing against for the facial recognitiona and facial coordinates.

- [ ] resolve "Some projects have trouble loading. Please review the output for more details" eventually.

- [x] Created mock for IFacialIdentityHandler using the static stdout test data string

Now I need to setup dependency injection into the FacialRecognitionManager in the ctor so that I can inject the mock or real interface implementation for the handling of the process.

Leaving the project in a non-building state to be resolved in the future.

## Wednesday August, 1, 2018
## Sprint 0: Unit Tests & Snapshot Coordinates
- [x] dotnet build Lazztech.ObsidianPresences.sln fails: There is no argument given that corresponds to the required formal parameter 'facialIdentityHandler'
Solution builds again: `dotnet build Lazztech.ObsidianPresences.sln`
Maybe it would be easier to just use xunit instead? I'm having some issues debugging and running the NUnit tests fixture.
Yeah it looks like I'm going to have a smoother development experience if I transistion to using xunit instead of NUnit with with dotnet core.
https://xunit.github.io/docs/why-did-we-build-xunit-1.0.html
```
dotnet sln remove Lazztech.ObsidianPresences.Vision.Microservice.Tests/Lazztech.ObsidianPresences.Vision.Microservice.Tests.csproj
rm -r Lazztech.ObsidianPresences.Vision.Microservice.Tests/
mkdir Lazztech.ObsidianPresences.Vision.Microservice.Tests/
cd Lazztech.ObsidianPresences.Vision.Microservice.Tests
dotnet new xunit
cd ..
dotnet sln add Lazztech.ObsidianPresences.Vision.Microservice.Tests/Lazztech.ObsidianPresences.Vision.Microservice.Tests.csproj
dotnet add Lazztech.ObsidianPresences.Vision.Microservice.Tests/Lazztech.ObsidianPresences.Vision.Microservice.Tests.csproj reference Lazztech.ObsidianPresences.Vision.Microservice.Domain/Lazztech.ObsidianPresences.Vision.Microservice.Domain.csproj
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
Trying adding `"dotnet-test-explorer.testProjectPath": "Lazztech.ObsidianPresences.Vision.Microservice.Tests/Lazztech.ObsidianPresences.Vision.Microservice.Tests.csproj",` to the workspace settings.

## Thursday August 2, 2018
## Sprint 0: Unit Tests & Snapshot Coordinates
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
## Sprint 0: Unit Tests & Snapshot Coordinates
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
## Sprint 0: Unit Tests & Snapshot Coordinates
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
## Sprint 0: Unit Tests & Snapshot Coordinates
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
/usr/share/dotnet/sdk/2.1.302/NuGet.targets(239,5): error MSB3202: The project file "/src/Lazztech.ObsidianPresences.Vision.Microservice.Tests/Lazztech.ObsidianPresences.Vision.Microservice.Tests.csproj" was not found. [/src/Lazztech.ObsidianPresences.sln]
/usr/share/dotnet/sdk/2.1.302/NuGet.targets(239,5): error MSB3202: The project file "/src/docker-compose.dcproj" was not found. [/src/Lazztech.ObsidianPresences.sln]
ERROR: Service 'lazztech.ObsidianPresences.vision.microservice.cli' failed to build: The command '/bin/sh -c dotnet restore -nowarn:msb3202,nu1503' returned a non-zero code: 1
```
Hmm it also looks like vs4mac can't run it in release either, only debug. I wonder if that could be related? Yup it looks like it is... vs4mac build error output says:
```
Step 12/21 : RUN dotnet restore -nowarn:msb3202,nu1503
     ---> Running in 03ee26a83aea
    /usr/share/dotnet/sdk/2.1.302/NuGet.targets(239,5): error MSB3202: The project file "/src/Lazztech.ObsidianPresences.Vision.Microservice.Tests/Lazztech.ObsidianPresences.Vision.Microservice.Tests.csproj" was not found. [/src/Lazztech.ObsidianPresences.sln]
    /usr/share/dotnet/sdk/2.1.302/NuGet.targets(239,5): error MSB3202: The project file "/src/docker-compose.dcproj" was not found. [/src/Lazztech.ObsidianPresences.sln]
    Service 'lazztech.ObsidianPresences.vision.microservice.cli' failed to build: The command '/bin/sh -c dotnet restore -nowarn:msb3202,nu1503' returned a non-zero code: 1
    /Applications/Visual Studio.app/Contents/Resources/lib/monodevelop/AddIns/docker/MonoDevelop.Docker/MSbuild/Sdks/Microsoft.Docker.Sdk/build/Microsoft.Docker.targets(111,5): error : Building lazztech.ObsidianPresences.vision.microservice.cli
    /Applications/Visual Studio.app/Contents/Resources/lib/monodevelop/AddIns/docker/MonoDevelop.Docker/MSbuild/Sdks/Microsoft.Docker.Sdk/build/Microsoft.Docker.targets(111,5): error : Service 'lazztech.ObsidianPresences.vision.microservice.cli' failed to build: The command '/bin/sh -c dotnet restore -nowarn:msb3202,nu1503' returned a non-zero code: 1.
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
```COPY Lazztech.ObsidianPresences.Vision.Microservice.Tests/Lazztech.ObsidianPresences.Vision.Microservice.Tests.csproj Lazztech.ObsidianPresences.Vision.Microservice.Tests/```
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
## Sprint 1: Remote Docker Debugging in vscode

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
## Sprint 1: Remote Docker Debugging in vscode

- Ran `docker system prune -a`
- Continueing to follow along with https://techblog.dorogin.com/running-and-debugging-net-core-unit-tests-inside-docker-containers-48476eda2d2a
Making the preLaunchTask “preDockerDebug”.
- Then ran `docker-compose up`
dockerfile build output:
```
Downloading https://vsdebugger.azureedge.net/vsdbg-15-7-20425-2/vsdbg-linux-x64.zip

ERROR: Command 'unzip' not found. Install 'unzip' for this script to work.
ERROR: Service 'lazztech.ObsidianPresences.vision.microservice.cli' failed to build: The command '/bin/sh -c curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l ~/vsdbg' returned a non-zero code: 1
```
The launch configuration seems to be coming along however the container runs and then closes on completion. I believe from the documentation that I need to add a command to keep it running.
I think that it's just missing the build task.

Theres many mentions about this being unusually manual or "off road" from the conventional path as there's lots of manual configuration they don't seem to expect the typical .net developer to be familiar with.

## Thursday, August 9, 2018
## Sprint 1: Remote Docker Debugging in vscode


# Project Managment Ideas:

- Markdown for documentation
- Json based Kanban Board
- Json based Pomodoro based time tracking
- Must be free, non hosted and git backed

I'll just log and track all of my project managment with those approaches and have it all git backed so it's portable. Also I want a report generated and embeded into the README.md with statistics.

It could be really useful for freelance project managment because literally everything would be right in the git repository. I could work offline, it would be free, everything would be located in one place, I would actually be independent from any service or third party hosted solution.

I really want the readme to have a report of:
- Time spent in the last 3 days, 10 days & month along with the calculated quote based on an hourly consulting rate
- Kanban completed taskes, added tasks, moved tasks for the past two weeks or something. I guess it could be new and completed for two weeks.
- I'd also want a report on days that I had logged entries in the documentation and preview of the "DevNotes" work documentaion log.
- Details about current and sprint

Maybe the report shoult just be about the current and just completed weekly or bi-weekly sprint?

Then a client or myself would see a report, updated from every single commit where any of the details changed.

That would allow me to focus on work and maintain the KISS principale(Keep It Simple Stupid) by taking what I've found to be the most helpful project managment techniques while keeping everything really minimalist and non-distracting.

I guess the report may need to be client side on my dev machine as some kind of statically generated html,css,js and whatever from the json files. Or maybe it could just be a jekyll based report? Either way I really want it be seemlessly self updating from every commit or as close as possible. I don't want it to make more work for me.

Here's the link for the json based pomodoro technique time tracker I found:
https://github.com/luckyshot/freelance-timetracker

## Markdown documentation Tips and tricks
- https://stackoverflow.com/questions/15764242/is-it-possible-to-make-relative-link-to-image-in-a-markdown-file-in-a-gist

Markdown based wiki
- http://dynalon.github.io/mdwiki/#!index.md
- https://wiki.js.org
- https://github.com/gollum/gollum - A simple, Git-powered wiki with a sweet API and local frontend.

## Json Based Pomodoro Time Trackers
- https://github.com/luckyshot/freelance-timetracker
- https://github.com/hangxingliu/vscode-coding-tracker
- https://marketplace.visualstudio.com/items?itemName=EastonLee.todopomo
- https://github.com/brunobord/pomodorock - A single webpage pomodoro timer / tracker that keeps ZERO KNOWLEDGE of your data on the server-side.
    -  http://brunobord.github.io/pomodorock
- https://www.tyme-app.com/mac-2/ - cost money and is mac only
- http://www.lukefabish.com/turned-react-tutorial-complete-application/
- https://github.com/nerab/paradeiser - Command-line tool for the Pomodoro Technique https://github.com/nerab/paradeiser

This one is command line based and can export to json. It could work well in vscode and could even work as a vscode task with a shortcut.
https://github.com/nerab/paradeiser

Goodtime is the android based one that I've used the most and like:
https://github.com/goodtime-productivity/Goodtime

https://marketplace.visualstudio.com/items?itemName=yahya-gilany.vscode-pomodoro
https://marketplace.visualstudio.com/items?itemName=lkytal.pomodoro
https://marketplace.visualstudio.com/items?itemName=odonno.pomodoro-code
https://marketplace.visualstudio.com/items?itemName=brandonsoto.pomodoro-timer
https://github.com/brandonsoto/pomodoro-timer

https://eastonlee.com/blog/2017/06/01/todopomo/

## Json Based Git Backed Vscode Kanban extension
- https://marketplace.visualstudio.com/items?itemName=mkloubert.vscode-kanban
---

Also I've decided to just use json persistance for the vision microservice at least until it ends up not being managable. I'll bind mount to a a usb drive on my cluster so that I can just unplug and inspect the results. Each microservice can have it's own usb flash drive. The read right performance will probably not be that good but it should be enough for my needs. I can name the json files after the date and time and a dash to seperate the hash value of the snapshot objects.

That way I can just iterate though them by either date or compare the hash to a set of deserialized snapshots for sorting. So I could just iterate through sets of jsons.

Also I want to set a limit on when to set down the effort on getting the remote debugging for the docker containers working in vscode and get some more development progress made as it's been stalled for too long. I do still intend to get the vscode docker remote debugging setup however it's really not a blocking issue and more of just an inconvenience.

I'd like to start doing weekly sprints with objectives for my project and tracking that too.

For this one I'll just go ahead and say that this weeks sprint has been #1 and was for vscode remote docker debugging.

I also need to go ahead and add in the devnotes from setting up the raspberry pi cluster too as that's definitly part of this project too. I've added it in as lazztech_cloud_notes.txt

**Next weeks sprint will be**
#### Sprint 2, Json Snapshot Persistance & Multiple Subjects 

## Saturday, August 11, 2018
## Sprint 1: Remote Docker Debugging in vscode
## Poms: 2

cli image classifier
https://www.npmjs.com/package/puddlenuts
http://dlib.net/train_object_detector.cpp.html
https://www.tensorflow.org/tutorials/images/image_recognition
https://github.com/TensorPy/TensorPy - This one seems really easy
http://tensorpy.com
https://robopress.robotsandpencils.com/build-your-own-image-classifier-in-less-time-than-it-takes-to-bake-a-pizza-9a7b898264de
http://androidkt.com/train-image-classifier/

The first few pomodoro extensions don't seem to save to json.
Yeah none of them seem to do what I want. Coding tracker seems promising though.

Yeah coding tracker is great but it might be overkill. Also it only tracks time from vscode usage. I don't really want or need that much specificity and would rather have just pomodoro time tracking.

Maybe I should just make my own cli time tracking program? I wonder what would be involved in making a vscode extension with c#? Also it wouldn't have to be a command line tool and instead could just be a cli. Then I could use it anywhere and just set the output path for all of the json results. I could also add a build step to run some kind of report generation script. Or maybe have a vscode task that could execute it. Yeah maybe I should make my own pomodoro time tracking cli.

I could eventually add in a kanban feature to and build it out more with time? 

https://code.visualstudio.com/docs/extensions/example-hello-world
It looks like they're typically done in javascript which makes sense. Yeah a cli may be best.

Okay so I went ahead and made a new project for the pomodoro technique cli however I was unable to get it to help me with the "Add Using Statment" shortcut. It looks like I had to add it to the solution first as vscode is only going to give full intelisense support to the project/solution listed in the bottom left of the vscode window.

https://github.com/OmniSharp/omnisharp-vscode/issues/8

https://www.slant.co/topics/4542/viewpoints/2/~open-source-multi-platform-time-tracking-apps~spacemacs
http://orgmode.org

It looks like this one that I originally found may be the best:
https://github.com/luckyshot/freelance-timetracker
I could fork and improve it if I need.

Oh actually the same developer made this too which seems better and also supports json exporting:
https://github.com/luckyshot/twentyfive

Looks like just sshing into docker containers is a viable option too after installing the command line debugger. I wonder if that would end up being simpler to manage.

Okay so I've decided to end this sprint early. I don't like how much time I've spent on setting up remote debugging in vscode for docker containers. I've learned a lot and documented it all but it's time to continue. Debugging with vs4mac or regular vs works fine enough for now.

#### Sprint 2, Json Snapshot Persistance & Multiple Subjects

So I'm comparing the docker debugging and the unit test debugging and one thing I see that is different that would make them similar is to have the sdtout lines as a string array in the tests instead of just removing the return carriage on each as that doesn't show up in the container.

Also in the unit test the snapsWithPeople has 8 but in the container it has 7.

The container crashes on this line: `/face/unknown/Chad Peterson.jpg,113,328,328,113`

Okay so in the tests there's a person object on the snapshot but not in the actual container... In the tests the name is "unknown_person".

I need to find in the tests when that person object for that snapshot is being newed up and see why. It's probably an issue with the test data. That and I should count the snaps between the test and container enviroments.

I guess in the test enviroment there's one more snapshot that hasn't geen set as no_persons_found than in the container.

In test Chad Peterson is known and there's a person object on the snapshot. In the container it's unknown_person. Which is correct. I wonder if I figure out why that snapshot has a person if that will solve the issue.

Okay so I found the issue. In the test there's an extra snapshot object for the "/face/unkown/webcam.jpg" again that has the status for the image set as the name... It's 3rd in the collection.

The other messed up one is the snapshot for "/face/unkown/Chad Peterson.jpg" where the name on the person is "unknown_person". Which for this one there shouldn't even be person.

Okay so yeah the status was getting left as the default .known since it didn't match the other status settings due to still having the "\r" on the end of the line from the test data stdout not mocking the container data properly.

Splitting the test stdout lines properly with the return carriage trimmed off seems to have unified the debugging values in the test env with the container env. There is now the same amout of snapshot objects as there's no return carriage characters at the end of the string to mess up the assignment of the snapshot status that the method throwing the error in the container depends on.

The tests now match in passing or failing.

For tomorrow I'll probably switch to testing against known and a single unknown image as apposed to the entire collection of unknown. Then I think I could parallelize the image processing.

## Sunday August 12, 2018
## Sprint 2, Json Snapshot Persistance & Multiple Subjects

Okay so it outputs a line for each face in an image with multiple faces. That should be easy enough. I wonder if it starts from left to right or how to know which face goes to which bounding box? That'll definitly be something to get some unit tests for. Here's an example of the output.
```
/face/unknown/harry-meghan-15.jpg,unknown_person
/face/unknown/harry-meghan-15.jpg,unknown_person
```
```
/face/unknown/harry-meghan-15.jpg,294,792,443,642
/face/unknown/harry-meghan-15.jpg,154,652,333,473
```

After adding in images for both the prince and his wife the output looks like this which seems to suggest that parser may go form right to left
```
/face/unknown/harry-meghan-15.jpg,Meghan Markle
/face/unknown/harry-meghan-15.jpg,Prince Harry
```

## Monday August 13, 2018
## Sprint 2, Json Snapshot Persistance & Multiple Subjects

Improved all of the test mocks to accept ctor parameters and refactored all of the test cases over. I then added in stdout data for multiperson image output.

I just relised I don't have all of the mock data setup yet.

## Tuesday August 14, 2018
## Sprint 2, Json Snapshot Persistance & Multiple Subjects

After adding in the multiperson face_recognition and face_detection stdout test data the test passed in finding two people.
It does however have the facebounding box values not assigned for the second person. I think I may remember leaving code in there specifying only the first person.

I also removed all of the results where the ImageDir contains "/known/" since that's not as constructive to output for now. I may want to handle the known people differently.

I'll just do something like foreach where imageDir ==. Wait maybe I should persist in the processed snapshots not only the snapshot being processed but also in the people object there should be the name of the image used to match them? No because that's already used as their name.

```
    //THIS IS THE LINE CAUSING THE ISSUE WITH MULTIPERSON SNAPSHOTS
    snap.People.First().FaceBoundingBox = bb;
```

I also now need to make sure it can handle a known and unknown. When there is a "known_unknown" it should still have a person object with it's bounding box set.
Yup so adding an image with two known people and a 3rd unkonw person in it causes ArgumentOutOfRangeException in the HandleBoundingBoxes() private method because it's trying to assign the bounding box to the "unknown_person" however it has not had a person object instantiated on the People property of the snapshot.

I need to make sure that "unknown_person" still instantiates a person so that it can recieve the bounding box. Also I think I'm going to do away with the "known_unknown" status and just consolidate that down to "unknown_person" since that's the same thing. Also I may move the status to the person object.

## Wednesday August 15, 2018
## Sprint 2, Json Snapshot Persistance & Multiple Subjects

New test: _3PersonSnap1UnkownShouldStillHavePeopleForeachWithBB()
Now I need to make sure that in the status setting method that if it returns "unknown_person" to still makes a person. I'll just have their name be "unknown_person" or empty.

Making progress on _3PersonSnap1UnkownShouldStillHavePeopleForeachWithBB() test however it's failing because there's 1 extra test empty snapshot. I think I've seen this once in the debugger. Idk why it's happening.

Okay this comes from the mock saying that there is an array element for the knownUnknownImageDirs when I pass in an empty string to be split into the paths. I need to ensure that if it's an empty string it doesn't result in any, and is null or Count == 0. 
Yup so after just setting that test data to string.Empty instead of just "" it worked as I expected. I guess that's why to use string.Empty instead... And setting it to null caused an exception because a null value can't be passed in as a param for .Split(). I guess string.Empty really is what I'll want to use in that kind of situation.

Also now after this design decision change the test SnapshotWithStatusOfno_persons_found_ShouldHaveNoPeople() fails however that is no longer a fact that I want to assert so I'll delete it. I've changed it over to reflect that a "unknown_person" should still have a non empty bb.

Oddly now however, the _3PersonSnap1UnkownShouldStillHavePeopleForeachWithBB() test case is failing again with an empty snapshot resulting in the count assertion being off. It appears to be taken from the KnownUnknown collection which I thought I'd just solved...

I swear I saw it pass so that's really confusing. Maybe I was just confused. Anyways, in the mocks where it's splitting the mock stdout test data I added the .Split() enum param StringSplitOptions.RemoveEmptyEntries which fixed it.

I should really probably just remove the whole notion of a "known_unknown" dir or snapshot enum status entirely. It's not helping and didn't end up fitting the design.

I also need to refactor the project code name over to there correct spelling where it's misspelled.

I ran into issues getting the solution to run and build again after fixing the mispelling due to untracked files. After cloning the repo it regenerated all of the docker compose dcproj related files and then it all worked again as expected.

## Friday August 17, 2018
## Sprint 2, Json Snapshot Persistance & Multiple Subjects

### ***Project Planning:***
I think that next I want to make a web front end for this project. I'm thinking of trying out razor pages. I also would really like to try out blazor eventually. Also I've been thinking about the microservice architecture and wondering how to handle the vision data which now is in the form of json files to start with and then also the images. How are binary files like images and such usually handled? Does that count as "blob storage"? 

I supose to start with I could make the aspnet core razor pages/mvc in a docker container and give it access to the same volume/bind path with the images and the data however knowing that eventually I'll be switching to interacting with the vision service through a REST Api so that I can set it up for the cluster with eventual elastic scaling and dns handling for the requests however I may still want to have a volume that both the front end and the vision layer use to handle the images.

Yeah so I'll do it in this order I think unless I find out something better from more research:
1. Make a web front end in a docker container with the same volume acess to the vision with the jsons and images
2. Setup that container along with the vision Cli conatainer in the docker-compose
3. Transition to using a new vision REST Api container from the Cli container, that will serve all of the json data and all clients will also have volume access to the images as well

Also eventually I'm excited to setup the authentication microservice. I'll be using the aspnet core authentication classes and that will probably be easy enough with the WebApi REST Api project. However that will come later.

Also after I get step 1. and step 2. done from that list above I but before getting it setup as an actual independent microservice administrating it's own data I would like to look into how I could do object detection and classification on cars and cats. I would like to be able to tell what individual cars are outside the camera, query about them to display statistics like who's there based on who owns a certain type of car. I would also like to be able to ask questions like how many times a certain type of car has been by in the last 2 weeks etc. like "How many times has the silver 2002 honda been seen outside?" Or follow up context aware questions like "How many times has that car been seen for around for longer that 20 seconds at a time?"; all of these questions I'm imagining a mix of voice and web front end interaction and presentation.

I want to be able to ask it either spoken or ui based queries like: "When was the last time you saw Waldo?", "Have you seen Osma outside?", "Where's Waldo?" and the like.

### ***Finishing this sprint by persisting to Json***
For now I'm going to use Json serialized output as a basic form of persistence then I'll make a non microservice though still docker containerized front end that just shares the same bind mount volume as the vision cli container that will also be in the docker-compose. This will not count as an actual microservice as since interacting with the output of the vision cli container in such a way wouldn't work as well in a decoupled eleastically scaling cluster which requires some kind of networked rpc.

First step to finish the sprint is to write all of the Json files to disk with the images, for convenience, which will be done in the Cli program.cs class in it's container. I'll handle replacing jsons if one already exists too.

## Saturday, August 18, 2018
## Sprint 2, Json Snapshot Persistance & Multiple Subjects

I'll continue the philosophy of microservices in allowing them to marshal the data. In the intereum I'll be using the container with the cli and just reference that instead of the vision domain directly. I'll have the cli responsable for the initial deserialization of the snapshot jsons, processing of any unprocessed images and writing the new jsons. I'll make a public field or property in the Cli program that I'll reference in the web page but eventually will just get the data from the rest api instead of the cli. I will probably need to change the references to the domain at that point so that I can still have access to the vision domain models, right? Or will that eventually cause a coupling issue with the microservice architecture? Idk I'll deal with that when I get there. 

Also after finishing with the json related stuff when I'm making the aspnet core razer pages web app front end I'll just make it as the general **Lazztech Cloud** front end. I'll work my towards having a page for each of the microservices or something like that, idk.

To add the datetime to the names of the snapshot jsons like I want to I'll have to format it to use dashes instead of the default forward slashes as that will result in deeply nested folders instead of file names. Or it just won't write at all because it will think that they're paths.

https://stackoverflow.com/questions/4158868/get-date-without-slashes

The current naming convention for the json files is: `dd-MM-yyyy-hh-mm-ss-tt_ImageName_SnapshotObjectHashcode.json`
The date is day-month-year-hour-minute-seccond-am/pm and that's for the datetime of the creation of the original image that the snapshot object/json was processed from.
This should allow me to look through them all by date range, and also with the hashcode I can use that for object comparisons against the jsons.
All of these values can then be accessed by spliting the json file name by '_'

Okay so I made the new web app front end project and then in vs4mac added docker support however it's throwing this issue: "failed to build: manifest for microsoft/aspnetcore:2.1 not found."
https://stackoverflow.com/questions/49798012/aspnetcore2-1-not-found

I'm updating vs4mac to see if that fixes this since I think I've run into this issue before earlier when I was first learning how to work with docker but before I was documenting my project.

This problem can be fixed by following along with that documentation and changing the base images to the new ones so `microsoft/aspnetcore:2.1` => `microsoft/dotnet:2.1-aspnetcore-runtime` and `microsoft/aspnetcore-build:2.1` => `microsoft/dotnet:2.1-sdk`.

So I can build and launch the docker-compose.dcproj in debug which includes the containerized dotnet core razor project however idk how to connect to it. I thought it would automatically launch it in a browser but it just says: "Now listening on: http://[::]:80". 
https://www.google.com/search?client=safari&rls=en&q=Now+listening+on:+http://%5B::%5D:80&ie=UTF-8&oe=UTF-8

Okay so I added the port to the docker-compose.yml for the webapp and also in the dockerfile for it set the port to 5000 now it launches the app. I learned how to set the interanal port to 5000 with this link: https://stackoverflow.com/questions/48669548/why-does-aspnet-core-start-on-port-80-from-within-docker

And I learned how to configure the port with the docker-compose.yml with this link: https://stackoverflow.com/questions/51033062/docker-container-listening-on-http-80

Also idk when it happened but now the .NET TEST EXPLORER is now having issues finding the tests and I've got a warning from it saying something about the settings.json "This setting cannot be applied now. It will be applied when you open this folder directly."

Also I need to update the WebApp project to use bootstrap 4 which is always a hassle with these. Or at least it was with the old .Net Framework MVC project. I'll document the process.
All of the tests are still passing though so that's good. I just ran them in vs4mac which works fine. I think this is because I added the "face_detection" folder in and that made it a vscode multi-root workspace which confused the test explorer on where to look.

Hmm it also looks like these aspnet core web projects have npm intalled even with the mvc/razor pages projects. Do I update it with nuget like before or with one of the other package managers? Maybe I should watch a primer video on aspnet core?

Okay yeah so it doesn't come with npm but here's a video on how to set that up:
Adding node package manager in asp net core mvc project
https://www.youtube.com/watch?v=dRGkUiQ1Kto

I really want to avoid having to configure any kind of webpack build configuration scripts or anything like that though... So it looks like it does however come with bower package manager out of the box which is too bad...
Here's a guide on how to update with bower for aspnet core which is how the project ships by default. Though I may have to transistion to npm since there's going to be less and less support for bower.
https://medium.com/@ashwinigupta/how-to-migrate-from-bootstrap-3-template-to-4-in-asp-net-core-application-7da01a1acf99

Okay so I've got bootstrap 4 based on the above blog post but the nav bar is floating below where it should be... It's also not rendering the body content with the suggested cshtml. Maybe it's because it's a razor pages project instead of mvc.

Also here's the link for the watcher details so that it will recompile as I make changes:
https://stackoverflow.com/questions/46584740/watching-an-asp-net-core-2-0-app

Why the fuck is the nav bar floating too low?... Okay after commenting out everthing else out in the site.css it now is in the right place.
Okay so it was this that was causing the problem which was fixed after commenting it out:
```css
body {
    padding-top: 50px;
    padding-bottom: 20px;
}
```

I should setup the docker container to run the webapp with watch while in debug. Here's more documentation on the watcher: https://docs.microsoft.com/en-us/aspnet/core/tutorials/dotnet-watch?view=aspnetcore-2.1

## Sunday, August 19, 2018
## Sprint 2, Json Snapshot Persistance & Multiple Subjects
=> *Transitionsing To Next Sprint*
## Sprint 3, Web Front-end then decoupling into Microservices with containerized vision REST Api

I'm planning the next sprint which I've named above. I'm going to go ahead and throw together the front end then once it presents the basics nicely then I'll go ahead pull it apart from the vision service with it's current containerized Cli execution to a containerized REST Api as a aspnet core webapi project.

Well actually this is already tricky since only the vision cli container has the dependencies to be able to execute the vision services... I do know I'll need to give the front end container access to the same volume so that it can access the images. I could start by having the front end deserialize the jsons into snapshots with the cli container creating them then when I decouple into microservices I'll have the new Vision Microservice REST Api responsible for marshaling the snapshot data.

So yeah to get it working roughly to build out the front end I'm going to create a lot of coupling issues which I'll then have to pull apart.
### ***Coupling Issues To Resolve Going Forward:***
- Frontend reference to vision domain for models: ***should be dynamic view models, with no direct reference to the domain***
- Frontend deserializing the Snapshot jsons: ***this should be served by Vision Microservice REST Api***
- 

Created temporary coupling issue with:
```
dotnet add Lazztech.ObsidianPresences.CloudWebApp/Lazztech.ObsidianPresences.CloudWebApp.csproj reference Lazztech.ObsidianPresences.Vision.Microservice.Domain/Lazztech.ObsidianPresences.Vision.Microservice.Domain.csproj
```

Hmm now it could be convenient to move the serialization and deserialization code to the domain... But I want to keep it pure. idk. Doesn't aspnet core mvc/razor projects have built in serialization/deserialization by convention?

- [ ] Look into aspnet core controller serialization conventions

Also I think the next microservice I'll be making is a web crawler/scraping service. I could use it to look up images for people to be recognized by scraping facebook, google images etc. That will be a really fun project challenge... Maybe after that I can start looking into either marketing or a software defined radio microservice?

Okay with razor pages projects where do I do the stuff I would usually do in the controller? Is it all in the page view too sorta like a react jsx file? But doesn't it also have mvc as well built in under the razor pages? Tbd. Oh I see it's got something like codebehind with each pages cshtml being accompanied by a cshtml.cs following the naming convention of the pages that I think by convention makes the url routing. So for this I'll be working in the `Index.cshtml` and I'll do the Snapshot model preperation I was talking about in it's `Index.cshtml.cs`.

Also in the next couple of sprints I'm going to really be wanting to get together some continous integration testing for all of the microservies working together then that will set me up for continous deployment with ARM compilation for the Single Board Computer cluster I've setup.

Okay so it looks like the `cshtml.cs` which inherits from `PageModel` forwhich the class is name: `IndexModel` or whatever the page is; is both the controller and the model... It looks like the `cshtml` view binds to the model with `@model IndexModel` and accesses the view models properties like this: `<h3>@Model.Message</h3>`.

## Monday, August 20, 2018
## Sprint 3, Web Front-end then decoupling into Microservices with containerized vision REST Api

Saw this new syntax last night on properties and wtf it's awesome...
```C#
public List<Snapshot> Snapshots => new List<Snapshot>();
```

Here's the video where I saw it @23:00 he called it a derived or calculated property.
https://www.youtube.com/watch?v=yyBijyCI5Sk

Okay so it doesn't looked like a derived property but that also looks cool as hell:
https://codereview.stackexchange.com/questions/28609/when-one-property-is-calculated-from-another

Okay I think this is a calculated property which is seems to be a C# 7 language feature:
https://stackoverflow.com/questions/36372457/lambda-for-getter-and-setter-of-property/36372531

Or maybe it's the C# 6 Expression bodied members?:
https://davefancher.com/2014/08/25/c-6-0-expression-bodied-members/

Woah... Is ther no Intellisense for aspnet core razor pages in vscode? 
https://github.com/OmniSharp/omnisharp-vscode/issues/168

Wow... yeah vscode Intellisense really doesn't support razor syntax in .cshtml files, though at least vs4mac does.

Having an issue casting the deserialized json object into a Snapshot... Okay so the syntax is `var snapshot = JsonConvert.DeserializeObject<Snapshot>(json);` not `var snapshot = (Snapshot)JsonConvert.DeserializeObject(json);`

Weird it's not able to find the directories for the images however it can find the directories to the jsons no problem...

I thought initially that it may be an issue with not having a correct aspnet `app.UseStaticFiles()` configuration based on this link. https://github.com/aspnet/Home/issues/575

Okay so I got it working through a kind of hack since it seems to be being prevented by some kind of aspnet configuration so what I did for now to get going was just read the byte array in the page code behind `.cshtml.cs` then converted it to base64 and did string enterpolation to configure the output to html friendly base64 which got the images displaying. It's super hacky though and I'm pretty sure that it's not how I'm going to want to actually deploy it... However for stubbing out the front end it's fine.

Used this as reference to see how to setup string interpolation for a valid base64 value in an html source.

## Tuesday, August 21, 2018
## Sprint 3, Lazztech Cloud Razor Pages Web Frontend

I've desided to hone in the objective of this weeks sprint just to design the web page. I'm doing this to set healthier expectations with a better work life balance. I'll continue as I have now, with the website deserializing the json snapshots and save the REST Api webapi project for the next sprint.

Also I'm growing really fed up with the uncomplete support for vscode or vs4mac. I also really don't want to have to reboot to get to full vs2017 so I'm going to eventually, wipe away bootcamp and just use VMWare Fusion to run vs2017 in my mac os.

Today while I was walking in the botanical garden I remembered my desire to farm plants indoor with some kind of software monitoring so I'm going to go ahead and stub out a Plants page in the Lazztech Cloud Web Frontend. And I'm going to take the work I've done for the Snapshot visualization and move it to it's own Vision page leaving the index as just a homepage with a dashboard.

I need to get the Bootstrap 4 Nav bar in the _Layout.cshtml to highlight the correct link instead of always just the Home link.

https://asp.net-hacker.rocks/2017/03/03/taghelper-to-show-or-hide-on-route-conditions.html
https://stackoverflow.com/questions/50342390/how-can-i-retrieve-the-current-route-from-the-layout-cshtml-file-in-my-asp-net

Here's a link about making bootstrap 4 cards dynamically resize in a grid layout:
https://stackoverflow.com/questions/35868756/how-to-make-bootstrap-4-cards-the-same-height-in-card-columns

Add Side bar menu or nav bar drop down for the Vision page link to sort between the Known and Processed.

Make the background for the unknown_persons red to alert that they stand out. Or just make the card outline red with the "unknown_person" name red too. Remove buttons from cards if possible and just have the image on the card be the link with hover over text.

## Wednesday, August 22, 2018
## Sprint 3, Lazztech Cloud Razor Pages Web Frontend

Yeah I'm sick of this shit. I'm deleting bootcamp windows 10 and installing parallels desktop for regular vs2017 in mac os. Also for the rest of my sprint my objective is to get a little bit more improvment to the frontend and get the backend/jsons to also support known people.

Also I need to update my mac but it caused me to have to do an emergency reinstall of mac os as it panicked during the update... I may have to wipe my mac and start clean... That would be a pain and take some time. I suspect it may be because of the modifications I tried to make to the terminal.

I need to backup the files from my macbook pro then I'll reinstall mac os. I'll do that later for now I'll just start with a fresh install of windows for the parallels install and delete bootcamp.

I restored my mac os command line with this: https://apple.stackexchange.com/questions/100468/how-to-uninstall-zsh

https://www.cnet.com/how-to/how-to-delete-a-hard-drive-partition-on-a-mac/

Well I restored the partition size, maybe I don't need to reinstall mac os?

Okay so I need to setup windows 10 pro on the parallels desktop first to be able to be able to install docker.

## Thursday, August 23, 2018
## Sprint 3, Lazztech Cloud Razor Pages Web Frontend
***Parallels Windows 10 Experience***
So it looks like I need both windows pro with hyper v and also parallels pro to be able to run docker in the windows machine. That sucks because that all will be very expensive however I think I can still write all the code in vs2017 in parallels and then just run it on my mac with vs4mac? I'll give it a try and see how far that gets me... That is the point of the containers after all isn't it though that I should be able to launch it in any machine with docker installed?

Added vision nav dropdown and navigation breadcrumbs. I need to figure out how to do pages groups or nested pages with aspnet core razor pages. I think I saw something about it supporting grouped pages in a folder from one of the videos I watched.

Also I'm unsure about the performance of doing development with mac os, the virtualizing linux in mac for docker then windows 10 in parallels all with 8gb of ram... It seems okay but we'll see how it goes.

https://natemcmaster.com/blog/2017/11/13/dotnet-watch-and-docker/

Added watch to web app frontend docker file entrypoint command.
Okay so I need to setup windows 10 pro on the parallels desktop first to be able to be able to install docker.

## Sunday, August 26, 2018
## Sprint 3, Lazztech Cloud Razor Pages Web Frontend
***vs2017 fresh setup and install for this project***

I've refunded parallels desktop home for mac as it doesn't support nested vms that I need for docker. I'm also now thinking that I'll most likely be selling
my mac and purchasing a more powerful pc laptop instead as I really am tired of fighting against not having proper vs2017 among other reasons. I'm looking at the
thinkpad x1 carbon.

I'm going through the process now of setting up this project for development on a windows machine again with vs2017 after having had installed bootcamp again
and perchasing a windows 10 pro license for docker support.

I've gone ahead and accepted the vs2017 community prompts to install docker after opening the solution however those were unsuccesful and I ended up installing 
docker ce for windows from their site directly. Then upon setup I signed in to the docker desktop app giving it my gianlazzarini@gmail.com username which I recall making this mistake
before... Docker ce desktop app accepts your email as the username but it causes build issues as the username and email are actually treated as two seperate things which the cli doesn't
interchange the same way so build will fail. To get/confirm your actual user name sign in to docker's website with your email address where you'll be able to find your username on the ui.
Sign into the docker desktop app with your username not your email. This applies for both mac and windows.

Actually, apperantly just signing out will also allow the docker build to succeed too. That's kinda weird.

Here's the github issues link talking about this issue:
https://github.com/docker/hub-feedback/issues/935

I was able to get the docker-compose.dsproj building the images after installing windows pro, docker ce for windows, signing out after signing in and launching the 
.dsproj which has the dlib cpp dependencies compiling however it seems to have stalled at 96%.

Oh also I had to add volume support:
https://docs.microsoft.com/en-us/azure/vs-azure-tools-docker-troubleshooting-docker-errors

Also vs2017 doesn't seem to be tracking this file properly as it doesn't always see changes.

You'll also get an exception thrown by vs2017 about docker if you open vs right after boot up before docker has finished starting up.

Running this to delete the the image that stalled.
```
docker rmi 43354180703a
```

I then signed into the docker ce windows desktop with the username as appossed to the email.

I've been blocked by some kind of compilation error:
```
2>[ 96%] Building CXX object CMakeFiles/dlib_python.dir/src/face_recognition.cpp.o
2>[91mc++: internal compiler error: Killed (program cc1plus)
2>Please submit a full bug report,
2>with preprocessed source if appropriate.
2>See <file:///usr/share/doc/gcc-6/README.Bugs> for instructions.
2>[0m[91mmake[2]: *** [CMakeFiles/dlib_python.dir/src/face_recognition.cpp.o] Error 4
2>[0mCMakeFiles/dlib_python.dir/build.make:518: recipe for target 'CMakeFiles/dlib_python.dir/src/face_recognition.cpp.o' failed
2>CMakeFiles/Makefile2:67: recipe for target 'CMakeFiles/dlib_python.dir/all' failed
2>[91mmake[1]: *** [CMakeFiles/dlib_python.dir/all] Error 2
2>[0mMakefile:83: recipe for target 'all' failed
2>[91mmake: *** [all] Error 2
2>[0m[91mTraceback (most recent call last):
2>  File "setup.py", line 257, in <module>
2>    'Topic :: Software Development',
2>  File "/usr/lib/python3.5/distutils/core.py", line 148, in setup
2>    dist.run_commands()
2>  File "/usr/lib/python3.5/distutils/dist.py", line 955, in run_commands
2>    self.run_command(cmd)
2>  File "/usr/lib/python3.5/distutils/dist.py", line 974, in run_command
2>    cmd_obj.run()
2>  File "/usr/lib/python3/dist-packages/setuptools/command/install.py", line 67, in run
2>    self.do_egg_install()
2>  File "/usr/lib/python3/dist-packages/setuptools/command/install.py", line 109, in do_egg_install
2>    self.run_command('bdist_egg')
2>  File "/usr/lib/python3.5/distutils/cmd.py", line 313, in run_command
2>    self.distribution.run_command(command)
2>  File "/usr/lib/python3.5/distutils/dist.py", line 974, in run_command
2>    cmd_obj.run()
2>  File "/usr/lib/python3/dist-packages/setuptools/command/bdist_egg.py", line 161, in run
2>    cmd = self.call_command('install_lib', warn_dir=0)
2>  File "/usr/lib/python3/dist-packages/setuptools/command/bdist_egg.py", line 147, in call_command
2>    self.run_command(cmdname)
2>  File "/usr/lib/python3.5/distutils/cmd.py", line 313, in run_command
2>    self.distribution.run_command(command)
2>  File "/usr/lib/python3.5/distutils/dist.py", line 974, in run_command
2>    cmd_obj.run()
2>  File "/usr/lib/python3/dist-packages/setuptools/command/install_lib.py", line 24, in run
2>    self.build()
2>  File "/usr/lib/python3.5/distutils/command/install_lib.py", line 109, in build
2>    self.run_command('build_ext')
2>  File "/usr/lib/python3.5/distutils/cmd.py", line 313, in run_command
2>    self.distribution.run_command(command)
2>  File "/usr/lib/python3.5/distutils/dist.py", line 974, in run_command
2>    cmd_obj.run()
2>  File "setup.py", line 133, in run
2>    self.build_extension(ext)
2>  File "setup.py", line 173, in build_extension
2>    subprocess.check_call(cmake_build, cwd=build_folder)
2>  File "/usr/lib/python3.5/subprocess.py", line 271, in check_call
2>    raise CalledProcessError(retcode, cmd)
2>subprocess.CalledProcessError: Command '['cmake', '--build', '.', '--config', 'Release', '--', '-j1']' returned non-zero exit status 2
2>Service 'lazztech.ObsidianPresences.vision.microservice.cli' failed to build: The command '/bin/sh -c apt-get update -y &&    apt-get install -y python3 &&    apt-get install -y python3-setuptools &&    apt-get install -y python3-dev &&    apt-get install -y build-essential cmake &&    apt-get install -y libopenblas-dev liblapack-dev &&    apt-get install -y git &&    git clone https://github.com/davisking/dlib.git &&    cd dlib && ls &&    python3 setup.py install --yes USE_AVX_INSTRUCTIONS --no DLIB_USE_CUDA &&    apt-get install -y python3-pip &&    pip3 install face_recognition' returned a non-zero code: 1
2>[0m
2>C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\Sdks\Microsoft.Docker.Sdk\build\Microsoft.VisualStudio.Docker.Compose.targets(365,5): error : Building lazztech.ObsidianPresences.vision.microservice.cli
2>C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\Sdks\Microsoft.Docker.Sdk\build\Microsoft.VisualStudio.Docker.Compose.targets(365,5): error : Service 'lazztech.ObsidianPresences.vision.microservice.cli' failed to build: The command '/bin/sh -c apt-get update -y &&    apt-get install -y python3 &&    apt-get install -y python3-setuptools &&    apt-get install -y python3-dev &&    apt-get install -y build-essential cmake &&    apt-get install -y libopenblas-dev liblapack-dev &&    apt-get install -y git &&    git clone https://github.com/davisking/dlib.git &&    cd dlib && ls &&    python3 setup.py install --yes USE_AVX_INSTRUCTIONS --no DLIB_USE_CUDA &&    apt-get install -y python3-pip &&    pip3 install face_recognition' returned a non-zero code: 1.
2>C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\Sdks\Microsoft.Docker.Sdk\build\Microsoft.VisualStudio.Docker.Compose.targets(365,5): error : 
2>C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\Sdks\Microsoft.Docker.Sdk\build\Microsoft.VisualStudio.Docker.Compose.targets(365,5): error : For more troubleshooting information, go to http://aka.ms/DockerToolsTroubleshooting
2>Done building project "docker-compose.dcproj" -- FAILED.
========== Build: 1 succeeded, 1 failed, 3 up-to-date, 0 skipped ==========
```

Notes for potential leads:
https://github.com/Kurento/bugtracker/issues/166

https://www.google.com/search?q=91mc%2B%2B%3A+internal+compiler+error%3A+Killed+(program+cc1plus)&rlz=1C1CHBF_enUS811US811&oq=91mc%2B%2B%3A+internal+compiler+error%3A+Killed+(program+cc1plus)&aqs=chrome..69i57.370j0j7&sourceid=chrome&ie=UTF-8

## Monday, August 27, 2018
## Sprint 4, vs2017 & Vision Microservice REST API
**First time working in the local Bellevue community library to focus.**

vs2017 was able to launch the docker-compose.dcproj today despite the failure last night. I'm unable to get the vision processing as it's looking for the volume that doesn't
yet have the images. It may fail anyways since the dlib compilation failed. I'm also noticing that the web frontend doesn't have offline development support for bootstrap and only
seems to be working via the cdn which isn't what I want. That and it's also rendering slightly weird.

However it launches and all of the unit tests pass so that's a start. I'm also installing CodeMaid to do some MS StyleCop static code analysis and refactoring.

Also I've decided when I get the vs2017 bits sorted and am working on the REST API I'm going to go ahead and just Base64 encode all of the images instead of giving the
web frontend access to the volume with images. That way I can work towards using docker virtual volumes instead of bind mounting explicit paths to folders on my desktop.

The rest api should be able to:
- Register new known people
- Submit an image to be processed with returned result response
- Return all snapshots by person name or unkown w/ pagination and total
- Return all by date range

Interesting note about windows/mac desktop explicit volume path bind mount with docker; it seems that docker is doing something to resolve discrepencies and is able to use the windows path on mac os too... Or maybe the image was already built on my mac with the other volume path set? Idk.

I've gone ahead and transfered over the face_detection folder with the images and jsons from macOS to continue testing with vs2017.

I'm getting an exception about the path being stale that I've never seen:
`System.IO.IOException: 'Stale file handle'`
https://stackoverflow.com/questions/20105260/what-does-stale-file-handle-in-linux-means

Running docker stop and then docker rm on the related processes to see if that fixes the issue with the path? That would explain why the volume still worked in mac despite it changing in the docker-compose if it retains the volume.

Okay so just stopping the docker container processes seemed to be enough to fix the stale path problem however I have a couple of new ones. 

1. fac_recognition doesn't seem to be working as the contents of the resulting jsons are empty of people
2. There seems to be some new problem with image paths

`Newtonsoft.Json.JsonReaderException: 'Unexpected character encountered while parsing value: . Path '', line 1, position 1.'`
This is being cause by an image path like this:
`"/face/results/._10-07-2018-03-22-46-AM_webcam.jpg_16495015.json"`

Forwhich when I read all the text from that file it looks like:
```
"\0\u0005\u0016\a\0\u0002\0\0Mac OS X        \0\u0002\0\0\0\t\0\0\02\0\0\u000e�\0\0\0\u0002\0\0\u000e�\0\0\u0001\u001e\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0ATTR;���\0\0\u000e�\0\0\0�\0\0\0\u0010\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0001\0\0\0�\0\0\0\u0010\0\0\u001acom.apple.lastuseddate#PS\0\0\0\0ă{[\0\0\0\0�4�\u001d\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0001\0\0\0\u0001\0\0\0\0\0\0\0\0\u001eThis resource fork intentionally left blank   \0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0001\0\0\0\u0001\0\0\0\0\0\0\0\0\u001e\0\0\0\0\0\0\0\0\0\u001c\0\u001e��"
```

However on a positive note I think the path having a period in the beginning is actually from the string enterpolation for the name since there's no face_detection that's what's causing it to break. I'll se if I can get dlib to compile as I need then continue on.

I'll try `docker system prune -a` then see if I can get it to compile the container again while looking up the exception it threw.

Weird yeah after stoping all running docker ps then running `docker system prune -a` it seems to have succesfully compile face_recognition and dlib where it failed before. However the resulting jsons are still empty. I'll have to walk through. I imagine it's just something about it looking for a path with a `/` where windows uses `\` or something like that. All the unit tests still pass so that's good. 

I'll have to look at how I'm naming the jsons which I documented then walk through where the problem is. It's possible that it's somehow configured in a way that assumes macOS or maybe face_detection still isn't working? I'll open a bash terminal to the container and test it.

I talk about the naming convention for the jsons on **Saturday, August 18, 2018**

Okay so the dot in the the begining of the name isn't due to the naming convention as it still has the details it's supposed to. It just also has a prefixing `._` to the name...

Also the the aspnet page is throwing this error:
```
Development Mode
Swapping to Development environment will display more detailed information about the error that occurred.

Development environment should not be enabled in deployed applications, as it can result in sensitive information from exceptions being displayed to end users. For local debugging, development environment can be enabled by setting the ASPNETCORE_ENVIRONMENT environment variable to Development, and restarting the application.
```

FaceRecognitionProcess is throwing this exception:
`OSError: cannot identify image file '/face/known/._Gian Lazzarini.jpeg'`

Again with the `._` causing problems. I have to open a bash terminal with the container just to be sure face_recognition is compiled since I think it is... Idk what's causing the `._`

The command to open up the image with volume access and interactive bash is as follows but with actual path and image id value: **old example**
```
docker run -it -v /Users/gianlazzarini/Desktop/face_recognition:/face/ --entrypoint /bin/bash ba34ace8a4cc
```

Running the following gives a clue that I may have the volume path for the docker-compose messed up:
```
PS C:\Users\Gian Lazzarini> docker run -it -v C:\Users\Gian Lazzarini\Desktop\face_recognition:/face/ --entrypoint /bin/bash 41296ecb09db
C:\Program Files\Docker\Docker\Resources\bin\docker.exe: invalid reference format.
See 'C:\Program Files\Docker\Docker\Resources\bin\docker.exe run --help'.
```

However I was still able to open up an interactive bash and confirm the both `face_recognition` and `face_detection` are responding correctly:
```
PS C:\Users\Gian Lazzarini> docker run -it --entrypoint /bin/bash 41296ecb09db
```

Okay it I think it was the space in the path that was messing it up as this doesn't work:
```
docker run -it -v C:\Users\Gian Lazzarini\Desktop\face_recognition:/face/ --entrypoint /bin/bash 41296ecb09db
```

But this does:
```
docker run -it -v C:\face_recognition:/face/ --entrypoint /bin/bash 41296ecb09db
```

Oddly enough however it still has the exact same problem even when I execute it in the container myself with the corrected volume path:
```
OSError: cannot identify image file 'known/._Gian Lazzarini.jpeg'
```

Interesting I seem to have found the issue shown in this output:
```
root@d032f645d436:/face/known# ls -a
.  ..  .DS_Store  ._.DS_Store  ._Gian Lazzarini.jpeg  ._Meghan Markle.jpeg  ._Prince Harry.jpg  ._Scott Hanselman.png  Gian Lazzarini.jpeg  Meghan Markle.jpeg  Prince Harry.jpg  Scott Hanselman.png
```

in powershell you can't run `ls -a` but can run `ls -h` however I've also just enabled viewing hidden files etc:
https://support.microsoft.com/en-us/help/4028316/windows-view-hidden-files-and-folders-in-windows-10

I've just deleted all of the files that start with `.` including the	`.DS_Store`.

https://apple.stackexchange.com/questions/69467/consequences-of-deleting-ds-store

Okay yeah that fixed it. I moved it back to the desktop but I'll move the docker-compose.dsproj volume to the C:\ so that it's not specific to my machine only.

I added a 1.6MB zip of the face_recognition folder of example images that I've been using. Hopefully adding that binary won't cause problems for me later...

I've also fixe that weird partially loaded button by commenting out the reference to the _CookieConsentPartial in the _Layout. Idk why that's happening in the first place but it's ugly and I don't really need the cookie consent right now.

## Tuesday, August 28, 2018
## Sprint 4, vs2017 & Vision Microservice REST API
**At the local library to focus again**

Hmm when I created the webapi project in vs2017 and selected the add docker support check box that's only in vs2017 it didn't add my project to the docker-compose.yml I wonder if this is because something is missconfigured for vs2017? Or maybe it would do this but only if I created the project then after the facted added docker support? Idk I may experiment later to find out but for now I just went ahead and added it in myself.

***Important Microservices Architecture Resource:***
- https://microservices.io/
- https://microservices.io/patterns/microservices.html
- https://microservices.io/patterns/data/database-per-service.html
- https://microservices.io/patterns/data/saga.html
- https://microservices.io/patterns/data/api-composition.html
- https://www.manning.com/books/microservices-patterns

Asp.net core web api resources:
- https://www.youtube.com/watch?v=aIkpVzqLuhA
- https://www.youtube.com/watch?v=ARIsfkhoRts

Is there a way to run/debug individual docker containers in vs2017 as apposed the the docker-compose.dcproj?

Also why is my web app not launching on it's own?
- http://iamnotmyself.com/2017/05/07/simplest-possible-asp-net-core-web-application-in-docker-for-windows/
- https://stackoverflow.com/questions/40221787/asp-net-core-on-docker
- https://stackoverflow.com/questions/50591908/how-do-i-run-a-visual-studio-docker-container-from-the-command-line-works-in-vs
- https://stackoverflow.com/questions/40221787/asp-net-core-on-docker
- https://github.com/aspnet/KestrelHttpServer/issues/2174

I think the reason the web page isn't launching on it's own is most likely the same reason the console output says `Now listening on: http://[::]:80	`.

Anyways that's beside the point of this sprint so I'll set it down for now as I've already spent too much of todays time on it.

Yes so I was testing to see if making a webapi project then adding docker support after the fact would preconfigure the docker-compose.yml like it should and some times did in vs4mac?

Yeah so adding docker after the fact didin't configure the docker-compose like I'm used to either... Idk whatever.

I'm having trouble connecting to the webapi project from the docker container. I think it may be something about the ports being exposed in the dockerfile, docker-compose & or the env aspnet core variable for the port.

I wonder if having had selected yes to use https by default for this webapi project is going to make development difficult in the container?

***Webapi Docker Container & Https***
- https://stackoverflow.com/questions/43338665/this-site-can-t-provide-a-secure-connection
- https://stackoverflow.com/questions/44186860/net-core-webapi-refuses-connection-in-docker-container
- https://carlos.mendible.com/2016/11/06/step-by-step-expose-asp-net-core-over-https-with-docker/
- https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-2.1&tabs=visual-studio

So it looks like the two different ports are one for http and the other for https which is pre-configured in the Properties > launchSettings.json file of the project.

The Output in vs2017 is showing:
```
fail: Microsoft.AspNetCore.Server.IISIntegration.IISMiddleware[0]
Lazztech.ObsidianPresences.Vision.Microservice.WebAPI>       'MS-ASPNETCORE-TOKEN' does not match the expected pairing token '421f95e9-791b-4d67-a848-b49bef5050fc', request rejected.
```

Maybe I should make a new project for now without https added during the project creation? After all I'm ultimately probably going to only expose an application facade/api gateway/composition api or what ever the patterns called. I could just defer concerns about https for now while I'm learning? 

It feels like bad practice though but this is primarily and educational project after all though...

I'm just going to watch more of this video then tomorrow I'll evaluate whether I should make a project without https just to move on for now.

**Building Web APIs with ASP.NET Core 2.0** https://www.youtube.com/watch?v=aIkpVzqLuhA

## Wednesday, August 29, 2018
## Sprint 4, vs2017 & Vision Microservice REST API

Finished watching Building Web APIs with ASP.NET Core 2.0 video and learned about these resources:
- https://github.com/RSuter/NSwag
- https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.1&tabs=visual-studio%2Cvisual-studio-xml
- https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-2.1

I learned a log more about aspnet, webapi's, and even a bit about entity framework core at the end too. It didn't however touch on https at all however there seems to be another video that does which I'll link below and am going to watch to see if it talks about https.

Swagger seems really cool and I keep on hearing about it. If I understand correctly it will help you automatically generate documentation on you webapi's and even stub out a boilerplate UI based on assesing what kind of REST Api routes and options they have. Super cool, I think Sameeze was telling me about it too that they use it at T-Mobile.

- https://swagger.io/
- https://en.wikipedia.org/wiki/Swagger_(software)
- https://github.com/swagger-api/swagger-ui

**Building Secure Web APIs with ASP.NET Core** https://www.youtube.com/watch?v=e2qZvabmSvo

Resources from the above video:
- https://github.com/OmniSharp/generator-aspnet Offers other project templates than what come out of the box. This project seems to only support Core 1.1 though. I think it's based on http://yeoman.io/ which could let me make custom templates which could be hugely useful!

**ASP.NET Core 2.0 Security** https://www.youtube.com/watch?v=6LzmEOvzt1A

## Friday, August 31, 2018
## Sprint 4, vs2017 & Vision Microservice REST API

Yeah whatever I'm just going to remake the project without.

## Sunday, August 2, 2018
## Sprint 4, vs2017 & Vision Microservice REST API

I've removed the https webapi project and after some configuring was able to access the webapi from the container by hitting http://localhost:50199/api/values

I added some EXPOSE statments to the Dockerfile and also exposed ports on the docker-compose.

I however wasn't able access it at http://localhost:50199/ I have to give the full path to a controller to get any response. This makes me think that maybe I did have the https version working in the docker container and just wasn't hitting the full route?

Now I'm having issues with aspnet webapi not having access to the files  "Could not find a part of the path"
- https://stackoverflow.com/questions/45600943/azure-web-deploy-could-not-find-a-part-of-the-path-d-home-site-wwwroot-bin-ro

It looks like it may actually be some kind of an issue with the tooling? That's what stack overflow is suggesting and that configuring the project to use a different version of the compiler can fix it?

Weird it works now after stoping and removing the processes. Also it's now at http://localhost:8080/api/values Idk exactly what happened there?..

## Monday, August 3, 2018
### Sprint 4, vs2017 & Vision Microservice REST API

***Extending the sprint for laborday.***

I've setup rest api endpoint calling from within the aspnet pages OnGet for the webapi endpoint. I'm having trouble with getting the address correct for the internal ip address.

https://stackoverflow.com/questions/17157721/how-to-get-a-docker-containers-ip-address-from-the-host

I looked at the ip address of the running webapi container with `docker inspect 506da009d30f`.
It returned:
```
"Networks": {
                "dockercompose18306792969269339587_default": {
                    "IPAMConfig": null,
                    "Links": null,
                    "Aliases": [
                        "lazztech.ObsidianPresences.vision.microservice.webapi",
                        "506da009d30f"
                    ],
                    "NetworkID": "69c6ec3b66c7ea271db9155defa2c57c7defdc61bfcaf8e010e098ab64ab9d59",
                    "EndpointID": "cb9380463b3e06ff019ab0907a26e1ecb862ed6847ce82a46c6957c6a08f6c11",
                    "Gateway": "172.20.0.1",
                    "IPAddress": "172.20.0.3",
                    "IPPrefixLen": 16,
                    "IPv6Gateway": "",
                    "GlobalIPv6Address": "",
                    "GlobalIPv6PrefixLen": 0,
                    "MacAddress": "02:42:ac:14:00:03",
                    "DriverOpts": null
                }
            }
```

Trying to solve HttpRequestException: Connection refused

- https://github.com/aspnet/Home/issues/1975
- https://stackoverflow.com/questions/42915782/httpclient-request-on-networked-docker-container
- https://forums.docker.com/t/connection-refused-when-you-try-to-connect-to-a-service-port-started-on-host/11508/6

I'm trying adding to docker-compose:
```
    networks:
      - api
```
This however doesn't seem to be enough alone as it throw this build error: `Error		Service "lazztech.ObsidianPresences.vision.microservice.webapi" uses an undefined network "api".`

docker-compose networking:
- https://docs.docker.com/compose/networking/
- https://stackoverflow.com/questions/35708873/how-do-you-define-a-network-in-a-version-2-docker-compose-definition-file
- https://www.youtube.com/watch?v=rFQqiuFIjms
- https://www.youtube.com/watch?v=RCG-5N41FpQ

Later I'll probably have a, or a few sprints related just to security but that'll be much later. During that time I'll setup https for the webapi(s) and an aspnet core authentication microservice that will probably use jwt for authentication.

Here's a good open source project example of using multiple languages in microservices: https://github.com/dockersamples/example-voting-app

I'm still having issues with the cross container networking. What I've seen says I should just be able to use the services name I've defined in the docker-compose however that hasn't worked yet. I wonder if it could be from the periods in the name?

I've tried all of these so far:
```C#
        //Hosted web API REST Service base url  
        //string Baseurl = "http://localhost:8080/";
        //string Baseurl = "http://localhost:50199/";
        //string Baseurl = "http://lazztechobsidianpresensevisionmicroservicewebapi:50199/";
        string Baseurl = "http://lazztech.ObsidianPresences.vision.microservice.webapi:8080/";
        //string Baseurl = "http://172.20.0.3:5000/";
        //string Baseurl = "http://dockercompose18306792969269339587_lazztech.ObsidianPresences.vision.microservice.webapi_1:8080";
```

**Define Docker Container Networking so Containers can Communicate** - https://www.youtube.com/watch?v=RCG-5N41FpQ

They're both in the same docker network and the aliases look alright.
```
"Networks": {
                "dockercompose18306792969269339587_default": {
                    "IPAMConfig": null,
                    "Links": null,
                    "Aliases": [
                        "c29edb6f84c8",
                        "lazztech.ObsidianPresences.vision.microservice.webapi"
                    ],
                    "NetworkID": "69c6ec3b66c7ea271db9155defa2c57c7defdc61bfcaf8e010e098ab64ab9d59",
                    "EndpointID": "47cd3916cd72c33dca8b6a8587e9cad229aced327a87defbf1952bee62da9b74",
                    "Gateway": "172.20.0.1",
                    "IPAddress": "172.20.0.2",
                    "IPPrefixLen": 16,
                    "IPv6Gateway": "",
                    "GlobalIPv6Address": "",
                    "GlobalIPv6PrefixLen": 0,
                    "MacAddress": "02:42:ac:14:00:02",
                    "DriverOpts": null
                }
            }
```
```
      "Networks": {
                "dockercompose18306792969269339587_default": {
                    "IPAMConfig": null,
                    "Links": null,
                    "Aliases": [
                        "lazztech.obsidianpresences.cloudwebapp",
                        "f1c767df6163"
                    ],
                    "NetworkID": "69c6ec3b66c7ea271db9155defa2c57c7defdc61bfcaf8e010e098ab64ab9d59",
                    "EndpointID": "34524330b35867e82afce5b4284c3c3b3b93de5bb945d63c58a56b686f06f538",
                    "Gateway": "172.20.0.1",
                    "IPAddress": "172.20.0.4",
                    "IPPrefixLen": 16,
                    "IPv6Gateway": "",
                    "GlobalIPv6Address": "",
                    "GlobalIPv6PrefixLen": 0,
                    "MacAddress": "02:42:ac:14:00:04",
                    "DriverOpts": null
                }
            }
```

Okay so opening an interactive terminal in the webfrontend container with `docker exec -it f1c767df6163 sh` and running `curl c29edb6f84c8/api/values` returns the results from the api in the other contianer. `c29edb6f84c8` is one of the aliases in the network above. Running `curl lazztech.ObsidianPresences.vision.microservice.webapi/api/values` also works! I wonder if the problem is that I was specifying the ports wrong? Because I think it just uses port 80 in the curl statment since port 80 is the default. Yeah if I leave it without a port it works or if I specify port 80 it works but that's it.

Yay! `string Baseurl = "http://c29edb6f84c8/";` worked!

I messed with the `depends_on` value in the docker-compose by commenting it out to see if it would still work however I also changed the Baseurl to the service name alias and unfortunately after doing both of those it stopped working and I'm not exactly certain. However I do know that I should probably use the service name alias for the networking url instead of using the container id since that can change from run to run.

Here's some more info about the `depends_on` paramter for docker-compose: https://docs.docker.com/compose/compose-file/#depends_on

I wonder if it doesn't work with `lazztech.ObsidianPresences.vision.microservice.webapi` as the network baseurl despite it working with curl from the container terminal because of some aspnet issue with capitalization? Because this network alias does seem to be case sensistive.

Yup so case sensistivity in urls seems to be the issue! When I put a breakpoint in the pages code making a new Uri from the base url it doesn't retain the case sensitivity in most of the fields I saw when I moused over it. And the casing matters.

- https://stackoverflow.com/questions/7996919/should-url-be-case-sensitive
- https://stackoverflow.com/questions/21001455/should-a-rest-api-be-case-sensitive-or-non-case-sensitive
- https://stackoverflow.com/questions/11726635/net-uri-case-sensitivity

Okay, so I fixed this by simply making all of the services names in the docker-compose lowercase. Idk if this will always be the best seeming option but it works and is the best I can think of for now. I should run reliably now since it doesn't depend on the transient container id as the network alias. I do still wonder why making a .Net Uri object automatically lowercases the value despite it showing the property for the original as being case sensitive... It must be some kind of implicit convention...

## Tuesday, August 4, 2018
#### Sprint 5, Frontend & Webapi Improvements/Integrations

Earlier I said:

The rest api should be able to:
- Register new known people
- Submit an image to be processed with returned result response
- Return all snapshots by person name or unkown w/ pagination and total
- Return all by date range

I also want to sort out Page groups in folders with the aspnet core razor pages routing thing. I need a vision route for both known and processed images along with a way to upload new images to either process or recognize.

Made a bunch of improvments to the frontend including getting the breadcrumbs working, organizing pages in folders and adding new page routes for features I would like to add.

Here's an important detail I learned while working on the breadcrumbs, that in aspnet core you use Context.Request instead of Request to get access to the static methods for getting the current url. It's mentioned in a comment on this link: 
https://stackoverflow.com/questions/43392179/the-name-request-does-not-exist-in-the-current-context

Now I want to draw the bounding box around the persons face from the Snapshot objects with html 5 canvas.
- http://jsbin.com/ejeyef/1/edit?html,css,js,output
- https://stackoverflow.com/questions/46528123/how-to-get-bounding-box-coordinates-for-canvas-content
- https://www.w3schools.com/graphics/canvas_coordinates.asp
- https://stackoverflow.com/questions/23973932/responsive-canvas-in-bootstrap-column

I'm having issues with `Uncaught ReferenceError: $ is not defined` for my javascript canvas bounding box rendering logic and I think it's because it's trying to execute before jquery is loaded...
- https://stackoverflow.com/questions/23973932/responsive-canvas-in-bootstrap-column
- https://stackoverflow.com/questions/2075337/uncaught-referenceerror-is-not-defined

## Thursday, August 6, 2018
#### Sprint 5, Frontend & Webapi Improvements/Integrations
***CI/CD Thoughts & VSTS***

Not getting as much done this week since I needed a down week. Also I'm working on the rfid chip implant push notification project. However while working on that it got me thinking about VSTS and how it could be helpfull in a quick solution for continous integration and hopefully also continous delevery too. 
I think I'll have a CI/CD sprint week just for setting up VSTS CI & CD to Azure initially... I'm ideally going to still want a pipeline for continous deployment to my ARM cluster but Idk how much longer that will take then deploying to azure. It may be a sprint or more to get it deploying to my arm cluster let alone setting up a continous delivery pipeline for it. Idk though maybe it will just be easy?

VSTS is the only free continous integration service that lets you use private repos that I can think of... I like TravisCI but it's pretty expensive. Ideally I'd like the entirety of my CI/CD devops architecture and scripts to be open source and maintained as part of this git repo but that can be something to work towards in the future.

Oh also I could look into GitLabs CI/CD options to see if that might be nice but I shouldn't spend to much time as I'll probably get further in a sprint if I just go with VSTS.

**Sprint 5, Frontend & Webapi Improvements/Integrations**
As for this sprint the next thing I want to do is persist the Known images and expose that data from the web api for which I'll then display on the Vision/Known page. After that I can start implementing uploading of known and unknown snapshots. That all would be plenty for the week.

## Friday, August 7, 2018
#### Sprint 5, Frontend & Webapi Improvements/Integrations
Up to today all of the processed snapshots were being returned by the FaceRecognitionManager class where they were being serialized by the cli into a folder called `/results`. However I want to have seperated results for processed and known. I'm going to remove the results folder and instead just write out the jsons to the `/known	` and `/unknown` directories respectively with their images. I'll then take from those two and return the results respectively in the web api so that I can render or interact with that from whatever client. This will also make uploading/processing new images simpler from the Webapi.

***Future Deployment Considerations***
I was thinking about this and the original idea I was having was to deffer any kind of authentication and instead just hide the whole service behind a vpn, however that posses complications for sending images for processing to the Webapi. It will also complicate the deployment a lot to deploy locally behind a firewall... Idk how I would handle the vpn credentials for the devices sending the images for processing?

I've introduced some bug where the people on the unknown path snapshots aren't having their people instantiated... It however doesn't show up on the unit tests which is concerning. Or possibly I just pasted over the jsons and never actually got dlib working in the container on this machine when I thought I had?

I'll have to solve this.

Okay so I debugged it and both the face_detectionLines and face_recognitionLines are returninging properly during the docker runtime so idk what's happening to the null people property on the snapshots...

Okay I found the issue sort of and as it turns out the unit tests were correct it's still working however something seems to be causing intermitent issues with the output json. Sometimes there fine and sometimes there not, having empty people properties. I wonder if this is from multiple services trying to read and write from them? When I put a break point on the part where the people are being instantiated in the snapshot it ends up writing them out properly. I wonder if this happens from me calling the webapi before their done being written? Idk this is more the domain of an integration test though.

However I'm going to try adding the depends_on in the docker-compose for the cli to see if that fixes it.

Also I realised I need to be able to have a way to view and return just snapshots where the enum is unknown_person. Do these snapshots have people instantiated too? Because it's not showing the "webcam.jpg" with an enum of 3, which with actually is no_persons_found so I guess that makes sense actually.

Yeah so it looks like if I go to the vision page, which calls the webapi, which deserializes the jsons and do that all before the cli finishes processing the results and writing to them then somehow it erases the people properties... What is happening?

I think I actually may have seen face_recognitionLines null which could explain it? I set it to check both of the process outputs and throw exceptions if their empty.

I'm able to get it working right now with a combination of a breakpoint(which I'm not sure if makes a difference or not) and having both all of the known and unknown jsons deleted before starting the docker-compose(which I'm also not sure if matters).
After re-running the docker-compose it thre an exception for face_recognitionLines being at a count of 0.

Adding `depends_on - lazztech.ObsidianPresences.vision.microservice.cli` didn't fix it.

However I wonder if maybe the directories it sees is possibly passing a json into the face_recognition causing it to crash? I should have stderr handeling on the processes. That way I can know if the face_recognition process fails instead of just returning null since it's only returning the stdout not the stderr, right? This would require me to setup event handeling so that I don't have the deadlock issue between the stdout and stderr streams that I solved the other day.

Okay so I don't think I'm passing in the jsons directly however by the fact that I'm passing in the known and unknown paths forwhich I'm also writing out the jsons I bet that's what's causing the issue...

## Saturday, August 8, 2018
#### Sprint 5, Frontend & Webapi Improvements/Integrations

Okay so I opened an interactive terminal with the cli container and confirmed that the face_recognition process is throwing an error when there's jsons in either the /known or /unknown. I tried to get both the stdout and stderr working asynchronously with events to avoid the deadlock but didn't quite get it working. I would still like to do this so that I can catch any face_recognition process failures however to resolve this issue I should probably just have the jsons written out to a seperate directory. 

I will probably also want to be able to pass in a single image directory for the face_recognition so that I can process just single image instead of re-processing all of the images again.

Okay so I solved the face_recognition & face_detection failure issue in identifying that it was from the jsons in the folders and I fixed the event driven process stdout & stderr handeling by making some tweaks.

**How to do non-deadlocking stdout & stderr handeling on a process with System.Diagnostic.Process**

1. Enable redirectstdout & redirectstderr in the startinfo object for the process object.
2. in the startinfo set the filename property to the name of the prcess your calling
3. Startinfo useshellexecute property should be false
4. pass args using Arguments property on the Startinfo
5. Subscribe to the process.OutputDataRecieved & process.ErrorDataRecieved with new DataRecievedEventHandlers
6. process.BeginOutputReadline() process.BeginErrorReadline() to start the async streams
7. process.WaitForExit()

You can also enable RedirectStandardInput in the ProcessStartInfo too which does not need event handeling or anything special to do `process.StandardInput.WriteLine()`.

Anyways back to solving the issue that face_recognition was failing on where there's jsons in the /known and /unknown. I want to setup a public static string with the output paths so that refactoring isn't a pain of hunting through the projects to rename strings.

## Sunday, August 9, 2018
#### Sprint 5, Frontend & Webapi Improvements/Integrations

I need to setup viewing for the unidentified people with the rest api and json persistence.

Also I'm thinking more and more about deployment. I need to do it soon. I'm considering just setting it up on the pc I have first as that would probably be easier than the cluster without the recurring cost. However I'll still need authentication before it can be publically exposed. 

For authentication I'm going to setup and application facade webapi project with the optional authentication.

Also I wonder if it could be more flexible to use ngrok to tunnel to the service locally as apposed to dynamic dns like noip or dyndns? It could be more flexible for early use and since it's mostly just going to be for personal use it may work fine. The advantage I see with this is that it would get around the 48hr time to repopulate dns.

**ngrok localhost tunneling vs dynamic dns for private cloud**
- https://medium.com/@milanaleksic/ngrok-vs-dynamic-dns-for-remote-linux-home-server-access-1486299502f2
- https://houtianze.github.io/ddns/ngrok/localtunnel/2017/01/22/ddns-alternative.html
- https://news.ycombinator.com/item?id=14278703
- https://ngrok.com/pricing
- https://discuss.fsftn.org/t/is-it-reliable-to-run-a-service-behind-ngrok/621
- https://localtunnel.github.io/www/
- https://www.noip.com/remote-access
- https://api.slack.com/tutorials/tunneling-with-ngrok

I want to figure out some infrastructer as code, scriptable provisioning deployment and configuring for my lazz.tech domain name. Should I use teraform for this?

## Monday, August 10, 2018
#### Sprint 6, CI/CD

I have to deploy my app. As it is right now, authentication is not really very urgent as there's not really any way to do anything but get the data, there's no creating, deleting or updating. There's also not really any super private data yet either. So now would be an okay time to deploy and setup my CI/CD pipeline. After that I'll make a Webapi project with the included optional authentication which will serve as the application facade rest api for all consuming clients. At that point I can add the rest of the data modifying operations to the rest api behind authentication.

Out of the given options I could use:
- Jenkins
- TravisCI
- Azure DevOps(VsTs)

Or any of the other myriad of options. First and formost I do actually want to get this project deployed and some degree of completion so shooting to get everything perfect with a self rolled open source solution may hinder that. And out of the practical solutions Azure DevOps, which was just announced as the successor of VsTs offers everything I need and is the only one that does it all free of charge for private repos. So that's what I'm going to start with. I can always make it an effort to strive towards a more proprietery solution later as I hate vendor lock in but for now it seems perfect as I do want to deliver after all.

I don't really care where it's hosted to start with. Initially it would be okay to use azure as long as I can still lift and shift. Ideally I want to self host and be able to switch between hosting solutions without too much friction.

I think I may still be able to setup my continous deployment pipeline with Azure DevOps to self host but I'm not certain. That may have to be a sprint of it's own. I do see they have a discounted self hosting option which would be a good option however the first project is free anyways so it doesn't matter.

- https://docs.microsoft.com/en-us/azure/devops-project/
- https://docs.microsoft.com/en-us/azure/devops-project/azure-devops-project-aspnet-core
- https://channel9.msdn.com/Events/Connect/2017/T174/player/

I'm using gianlazzarini@srnd.org account. I need to speak with the team about my confusions surrounding my gianlazzarini@gmail.com msdn account since I think I can use those resources however now I'm not 100% certain so just to be safe for now I'll use gianlazzarini@srnd.org.

I want the master banch to deploy and staging to build. Actually after looking at it more it looks like the srnd account is pretty heavily used and maybe I should just make a new lazz.tech account. I could setup an email address for gian@lazz.tech. Then maybe some day I could use that for other more serious projects too.

I just setup email forwarding only settings for gian@lazz.tech to gianlazzarini@gmail.com with my get.tech account. I should probably actually configure it as a full portal so I can send emails from my gmail with that account too but I'll deal with that later.

Azure DevOps failed to build this project due to:
```

COPY failed: stat /var/lib/docker/tmp/docker-builder483335454/Lazztech.ObsidianPresences.sln: no such file or directory

/usr/local/bin/docker failed with return code: 1
```

Hmm yeah and if I just run `docker-compose up` it fails locally too. I thought I fixed this...

Looking back I seemed to have solved this issue before by modifying the dockerfiles to fix the copy lines or something.

Checkout this days entry for the docker-compose up failure solution:
Sunday, August 5, 2018
Sprint 0: Unit Tests & Snapshot Coordinates

Yeah if I look at the cli project dockerfile which is the oldest I can take from that. It has copy statments for each of the projects that the solution needs to build so I'll have to model the other ones after that and I think that will fix it.

## Tuesday, August 11, 2018
#### Sprint 6, CI/CD

***Switching to self owned Containerized Jenkins Blue Ocean CI/CD***

Okay I'm changing the plan. I don't like the licensing concerns and vendor lock in with Azure DevOps. I want to have full ownership of my CI/CD Pipeline and do that I'll have to use open source software. I'm going to maintian a dockerized jenkins container in this repo. I want to setup YML configurations for the pipelines and try out the new Blue Ocean UI. This way I won't even have to worry about hosting. I can maintain my CI/CD server locally on my laptop with docker. That way I'll just spin up the container when I want or need and can switch later to self hosting the container however I see fit.

- https://jenkins.io/projects/blueocean/
- https://jenkins.io/doc/tutorials/create-a-pipeline-in-blue-ocean/
- https://jenkins.io/doc/book/blueocean/getting-started/
- https://jenkins.io/blog/2017/04/05/say-hello-blueocean-1-0/
- https://hub.docker.com/r/jenkinsci/blueocean/

`docker run -p 8080:8080 jenkinsci/blueocean`
`http://localhost:8080/`

Jenkins Pipelines as YAML:
- https://jenkins.io/blog/2018/07/17/simple-pull-request-plugin/
- https://jenkins.io/blog/2018/04/25/configuring-jenkins-pipeline-with-yaml-file/
- https://news.ycombinator.com/item?id=17558611
- https://github.com/Jenkinsci/travis-yml-plugin - Designed to run .travis.yml as Jenkins pipeline job.
"everyone has to continue using Groovy DSL and/or JJB to reinstantiate parameterized jobs or handle jobs that deal with multiple Jenkinsfiles in a project."

"**Jenkins configuration as code lets you define the entire Jenkins configuration in YAML and launch Jenkins as a docker container to do immutable infra. Jenkins Pipeline lets you define your pipeline in your Git repo, so that's the other part of immutable infra**, and between modern pipeline and efforts liek this one, there's no need to write Groovy per se. It's just a configuration syntax based on brackets like nginx, which happens to conform to Groovy syntax, so that when you need to do a little bit of complicated stuff you can, but you don't need to"

- https://docs.openstack.org/infra/jenkins-job-builder/ Jenkins Job Builder takes simple descriptions of Jenkins jobs in YAML or JSON format and uses them to configure Jenkins. You can keep your job descriptions in human readable text format in a version control system to make changes and auditing easier. It also has a flexible template system, so creating many similarly configured jobs is easy.

"Agreed, I love the groovy jenkins job DSL. It's so awesome and simple. Easy to put everything on github and forget about it."

I may need ngrok to tunnel the locally hosted instance of docker on my machine so that the gitlab webhooks can access it. Below is a link that gave me this idea.
http://thesociablegeek.com/node/github-continuous-deployment-to-a-raspberry-pi/

Also theres this:
https://pulumi.io/

**Gitlab CI/CD**
Okay so it looks like the other good free/open source CI/CD software is actually gitlab. Gitlab uses yml files at the root like TravisCI and CircleCI. I'll stick with jenkins until it doesn't seem to suite my desired needs. It looks very much like TravicCI actually.
- https://www.youtube.com/watch?v=1iXFbchozdY Demo: CI/CD with GitLab
- https://docs.gitlab.com/omnibus/docker/ GitLab Docker images

Gitlab CI/CD maybe a much simpler answer.

---
Hmm so **I seem to have gotten the docker-compose up working** by adding the missing copy statments to all of the dockerfiles foreach project in the solution then also clearing out all of the existing cached images. If I run docker-compose up it works now with minimal warnings.

One of those warnings is:
`WARNING: Image for service lazztech.obsidianpresences.cloudwebapp was built because it did not already exist. To rebuild this image you must use `docker-compose build` or `docker-compose up --build`.`

I think this could be resolved by modifying this line:
`RUN dotnet restore Lazztech.ObsidianPresences.sln -nowarn:msb3202,nu1503`

**Jenkins Configuration As Code**
- https://wiki.jenkins.io/display/JENKINS/Configuration+as+Code+Plugin
- https://github.com/jenkinsci/configuration-as-code-plugin/blob/master/README.md
- https://www.praqma.com/stories/jenkins-configuration-as-code/

## Wednesday, August 12, 2018
#### Sprint 6, CI/CD

- https://www.youtube.com/watch?v=GkGXAPj8wSI Setup Jenkins Blue Ocean with Docker (With .Net Core!)
- https://github.com/jenkinsci/docker/blob/master/README.md Contains documentation on setting up a conatienr explicit path to save the jenkins configurations. This seems to be the same documentation in the video above.
- https://hub.docker.com/r/_/jenkins/ This is exaclty the same link.
- https://github.com/boxboat/jenkins-demo

Here's the command line argument from the youtube video:
`docker run -d -p 8080:8080 -p 5000:5000 -v $(pwd):/var/jenkins_home --restart always jenkins:alpine`
`docker logs -f CONTAINER ID` This is how they seem to get the initial jenkins password in a detatched container
`ifconfig -a`
`sudo vi /etc/hosts`

I've added a number of commands to the docker notes section above based on this argument.

***Saving Jenkins Docker Container Configuration State***
"NOTE: Avoid using a bind mount from a folder on the host machine into /var/jenkins_home" So it looks like I should not use docker bind mounted explicit paths and instead use virtual volumes? The two options seem to be documented in the links above.

"If your volume is inside a container - you can use `docker cp $ID:/var/jenkins_home` command to extract the data, or other options to find where the volume data is. Note that some symlinks on some OSes may be converted to copies (this can confuse jenkins with lastStableBuild links etc)"

https://www.quora.com/What-does-pwd-mean pwd is a system program which just prints current directory, so echo $(pwd) prints that.

Should I set the jenkins container volume to be bind mounted to the git repo? Or will it save a bunch of large plugin binaries? Or maybe it shouldn't even actually be a bind mounted volume at all and a docker virtual volume instead?

Should I expose my locally hosted dockerized jenkins container on the internet? Or just use a vpn? Again with this question, vpn, dynamic dns or localhost tunneling?

- http://get.docker.com
- http://plugins.jenkins.io/swarm
- https://wiki.jenkins.io/display/JENKINS/Swarm+Plugin

This video doesn't seem to take advantage of stateless jenkins configuration as code. He installs plugins before the Jenkinsfile can run. Hmm maybe I should still make my own custom Dockerfile to setup my own Jenkins container with any dependencies I may need? Idk or maybe I could script the provisioning of a vm with docker on it? Later possibly.  

- https://www.youtube.com/watch?v=xqxoR7UzF4A Jenkins CD to Docker Swarm (Pt 2 from video above!)

## Thursday, August 13, 2018
#### Sprint 6, CI/CD

"Ensure that /your/home is accessible by the jenkins user in container (jenkins user - uid 1000) or use -u some_other_user parameter with docker run"

In the same updated jenkins docker README it says: "NOTE: Avoid using a bind mount from a folder on the host machine" then "If you bind mount in a volume - you can simply back up that directory (which is jenkins_home) at any time. This is highly recommended."...

I'll look at the jenkins blue ocean documentation and see if there's another take on which to do... I want to bind mount it. Also how can I configure the jenkins_home with the yml I was reading about? I want to check it into my repo.

**This has more documentation on how to run the jenkins blue ocean docker container.**
- https://jenkins.io/doc/book/installing/#docker ***VERY HELPFUL AND THOROUGH***
```
docker run \
  -u root \
  --rm \
  -d \
  -p 8080:8080 \
  -p 50000:50000 \
  -v jenkins-data:/var/jenkins_home \
  -v /var/run/docker.sock:/var/run/docker.sock \
  jenkinsci/blueocean
```
**or in one line:**
- `docker run -u root --rm -d -p 8888:8080 -p 50000:50000 -v jenkins-data:/var/jenkins_home -v /var/run/docker.sock:/var/run/docker.sock jenkinsci/blueocean`

**or my own spin with auto restart:**
- `docker run -u root -d -p 8888:8080 -p 50000:50000 -v jenkins-data:/var/jenkins_home -v /var/run/docker.sock:/var/run/docker.sock --restart always jenkinsci/blueocean`

#### My own custom docker jenkins blue ocean command to run with bind mount volume to home and always auto restart so that it's always running when you boot up your pc
---
- `docker run -u root -d -p 8888:8080 -p 50000:50000 -v $HOME/jenkins:/var/jenkins_home -v /var/run/docker.sock:/var/run/docker.sock --restart always jenkinsci/blueocean`

OR WITHOUT BIND MOUNT:
- `docker run -u root -d -p 8888:8080 -p 50000:50000 -v jenkins-data:/var/jenkins_home -v /var/run/docker.sock:/var/run/docker.sock --restart always jenkinsci/blueocean`

OR FOR WINDOWS:
- `docker run -u root -d -p 8888:8080 -p 50000:50000 -v C:\jenkins:/var/jenkins_home -v /var/run/docker.sock:/var/run/docker.sock --restart always jenkinsci/blueocean`

Do this to get the jenkins startup password:
- `docker logs CONTAINER_ID`

Set Localhost dns mapping host route on linux/unix:
- `ifconfig -a`
- `sudo vi /etc/hosts`

For windows host file:
- https://gist.github.com/zenorocha/18b10a14b2deb214dc4ce43a2d2e2992
---

**or multiline for windows:**
```
docker run ^
  -u root ^
  --rm ^
  -d ^
  -p 8080:8080 ^
  -p 50000:50000 ^
  -v jenkins-data:/var/jenkins_home ^
  -v /var/run/docker.sock:/var/run/docker.sock ^
  jenkinsci/blueocean
```

`-p 5000` is optional for if you want to setup other build agents like on a cluster.

"Instead of mapping the `/var/jenkins_home` directory to a Docker volume, you could also map this directory to one on your machine’s local file system. For example, specifying the option
`-v $HOME/jenkins:/var/jenkins_home` would map the container’s `/var/jenkins_home` directory to the jenkins subdirectory within the $HOME directory on your local machine, which would typically be `/Users/<your-username>/jenkins or /home/<your-username>/jenkins`."

"`/var/run/docker.sock`` represents the Unix-based socket through which the Docker daemon listens on. This mapping allows the jenkinsci/blueocean container to communicate with the Docker daemon, which is required if the `jenkinsci/blueocean` container needs to instantiate other Docker containers. This option is necessary if you run declarative Pipelines whose syntax contains the agent section with the docker parameter - i.e.
agent { docker { …​ } }. Read more about this on the Pipeline Syntax page."

**Accessing the docker jenkins container through terminal:**
- `docker exec -it jenkins-blueocean bash`

Here's some documentation on setting up a .gitignore to check in your jenkins_home into git:
- https://gist.github.com/samrocketman/9391439
- https://gist.github.com/cenkalti/5089392 I think this one is related too
- https://wiki.jenkins.io/display/JENKINS/SCM+Sync+configuration+plugin

So the auto restart parameter for the jenkins container seems to work just fine however it doesn't seem to maintain the bind mount to the volume on my host machine as it didn't maintain the state after startup.

I think I may need to make a dockerfile with all of the configured build dependencies for the project I'm working on. It'll probably need to have docker installed in it? Or maybe also just .net core and msbuild?
Isn't there a way to open an interactive terminal with a docker contianer, execute commands then save the changes into a dockerfile? I need to sort the jenkins dependencies install for building my projects then also learn more about deploying to docker-swarm.

## Friday, August 13, 2018
#### Sprint 6, CI/CD

Setting up .net core dependencies in the Jenkins container:
- https://docs.docker.com/engine/reference/commandline/commit/
- https://stackoverflow.com/questions/19585028/i-lose-my-data-when-the-container-exits
- https://hackernoon.com/to-commit-or-not-to-commit-5ab72f9a466e

If I commit changes in a container does it save a dockerfile somewhere? Or can I generate a dockerfile from my commited changes? If so that could be really practical for setting up containers instead of just retrying with new changes in a dockerfile everytime.

I see nothing on being able to save commited container changes to a dockerfile so it looks like the only way is to not commit container changes and just use a dockerfile in the first place.

It should be avoided but you can export or save a container or image binary for re-runnig:
- https://tuhrig.de/difference-between-save-and-export-in-docker/

However continue using dockerfiles so that you can check in your containers state and just rebuild from each change that way your container is repeatable.

So I should probably make my own jenkins dockerfile to have it pre-configured with all of my build dependencies? Or do I just use another container image from dockerhub with all of the build dependencies and specify that in my pipeline?

Containerized Jenkins Blue Ocean Pipeline for .NET Core related links:
- https://stackoverflow.com/questions/48104954/adding-net-core-to-docker-container-with-jenkins

However installing ngrok inside the jenkins dockerfile container would let me setup webhooks from my git repo right?

Or I can setup the pipeline to poll the repo for changes hourly.

Also should I be writing my build steps as shell scripts? That seems to be what everyone else is doing.

Here's an example build shell script: https://www.pgs-soft.com/blog/cross-platform-application-using-net-core-jenkins-docker/
```
dotnet restore
dotnet publish -c release
docker build -t dockerhubuser/simplecoreapp:v0.${BUILD_NUMBER} .
docker login -u dockerhubuser -p dockerhubpassword -e user@domain.com
docker push dockerhubuser/simplecoreapp:v0.${BUILD_NUMBER}
```

Here's an example of the official dotnetcore 2.0 dockerfile:
https://github.com/dotnet/dotnet-docker/blob/1e0a8502922a4a836558e08df1379ce0032988f6/2.0/sdk/stretch/amd64/Dockerfile

Here's a dotnetcore install shell script:
https://github.com/dotnet/cli/blob/master/scripts/obtain/dotnet-install.sh

Here's a set of commands to run that dotnetcore install shell script:
Install the latest .NET Core 2.0:
```
sudo apt install libunwind8 gettext apt-transport-https
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 2.0
```
Or LTS version of .NET Core
```
sudo apt install libunwind8 gettext apt-transport-https
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel LTS
```

However I should probably just use the official microsoft dotnetcore docker image to build the project right? But what if I want both regular dotnet and aspnet core projects in the solution? Because their's the two different images? Which if either do I use to build the solution? 
`microsoft/aspnetcore-build`/`microsoft/dotnet:2.1-sdk` vs `microsoft/dotnet:2.1-sdk`

https://hub.docker.com/r/microsoft/aspnetcore-build/

Well it looks like the aspnetcore-build image uses the microsoft/dotnet as the base however it looks like everything builds with `microsoft/dotnet:2.1-sdk`. Also I think the `microsoft/aspnetcore-build` image may be depricated which I learned about when vs4mac's add docker dockerfile wouldn't work. Yeah it's from the change from 2.0 to 2.1. 

So I use `microsoft/dotnet:2.1-sdk` to build my solution in jenkins pipeline. I think so. 

Also I see there's a docker plugin for jenkins?

Jenkins CI part of the pipeline should:
- restore packages
- build solution
- run unit tests
- run docker-compose up
- delete everything for the next build

Jenkins CD part of the pipeline should?:
- publish the docker-compose up images?
- build the docker-compose for an arm processor?
- ssh into my rpi cluster and deploy?

**The official dotnetcore 2.1 docker image has notes about which tag to use for arm processors:**
Looks like: `microsoft/dotnet:2.1.402-sdk-stretch-arm32v7`
- https://hub.docker.com/r/microsoft/dotnet/
**Official documentation on dotnetcore image on Raspberry Pi:**
https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/dotnet-docker-arm32.md

https://youtu.be/GkGXAPj8wSI?t=1003 This is the part of the video from before where he starts talking about the .netcore build pipeline in blue ocean. He has shell scripts for each build step in the pipeline including:
= Build docker-compose
- Run Unit tests in docker
- Run functional tests in docker
- Run stress test
- Tear down docker and cleanup

The Continous Integrations shell scripts can be found here:
- https://github.com/boxboat/jenkins-demo/tree/develop/ci
- https://github.com/boxboat/jenkins-demo/tree/develop/ci/test

**For example:**
```
#!/usr/bin/env bash
cd $(dirname $0)

set -e

docker exec app-dev-dotnet app-test-unit
```

The Continous Deployment shell scripts are here:
- https://github.com/boxboat/jenkins-demo/tree/develop/cd

It uses a tool called Vegeta for load stress testing which could be useful (HTTP load testing tool and library. It's over 9000!) It should be easy enough to implement since I have a good example right here.:
- https://github.com/tsenart/vegeta
- https://medium.com/@carlosaugustosouzalima/do-you-need-to-run-load-tests-vegeta-to-the-rescue-7e8818127a65
- https://thisdata.com/blog/load-testing-api-interfaces-with-go-and-vegeta/

It could be nice to include this kind of stress testing and it's plotting output as a report on builds to see what effect my changes may make on the load tolerance of my software infrastructure. It would clue me in right away.

I wonder if for my project I should have a full second set of docker-compose and dockerfiles just for arm cluster deployment? Like docker-compose.rpi.yml and dockerfile.rpi for example? I think that would still work but I also wonder if having the naming for these files changed would cause problems; that or possibly having duplicates could be an issue?

## Saturday, August 14, 2018
#### Sprint 6, CI/CD
***Got the nfc implant this day.***

Just opened up an interactive terminal with the jenkins container and confirmed that it does have docker installed in it so I ca go ahead and just right my shell build scripts to take advantage of that without any extra work.

## Sunday, August 15, 2018
#### Sprint 6, CI/CD

Setting up the build shell scripts.
- https://www.youtube.com/watch?v=v-F3YLd6oMw Shell Scripting Crash Course - Beginner Level

Running dotnet core xunit tests in command line:
- https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test?tabs=netcore21
- https://xunit.github.io/docs/getting-started-dotnet-core
- https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test

I"m going to enable the linux subsystem for windows so that I can run these shell scripts while I develop them.
- https://docs.microsoft.com/en-us/windows/wsl/install-win10

Installed Ubuntu.

Installed Hyper Terminal because I like it.
- https://hyper.is/

Configured with this:
- https://daverupert.com/2017/03/my-bash-on-windows-developer-environment/

Used this to customize it:
- http://ezprompt.net/

Install .NET Core SDK on Linux Ubuntu 16.04
- https://www.microsoft.com/net/download/linux-package-manager/ubuntu16-04/sdk-current

Installed dotnet core on linux with:
```
wget -q https://dot.net/v1/dotnet-install.sh
./dotnet-install.sh -c Current
rm dotnet-install.sh
```

Having issues installing dotnetcore on the linux subsystem for windows:
- https://github.com/dotnet/core-setup/issues/4049

Running this from the github issues link above fixed it:
```
curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg
sudo mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg
sudo sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-bionic-prod bionic main" > /etc/apt/sources.list.d/dotnetdev.list'

sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install dotnet-sdk-2.1.105
```

My build-restore.sh script is coming along but I've got an error from running dotnet restore: `/mnt/c/Users/Gian Lazzarini/source/repos/Lazztech.ObsidianPresences/docker-compose.dcproj : error MSB4236: The SDK 'Microsoft.Docker.Sdk' specified could not be found.`

https://github.com/dotnet/cli/issues/6178

"This is an SDK that ships only with VS. It is not supported in the CLI"
Wow, so yeah this is a real issue because windows hasn't opensourced it's dotnet docker support so the decided way for now is to copy and paste the dependency from a pc with vs2017 installed... That's even how mono did it... Wtf microsoft.

Here's the fix:
- https://github.com/dotnet/cli/issues/6178#issuecomment-330864290
"Just to summarize the steps I had to perform to make it work on Windows and Linux (Ubuntu):

Copy the Microsoft.Docker.Sdk folder from C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\Sdks (Only the Sdk subfolder. Do not copy build and tools subfolders).
Then paste it on C:\Program Files\dotnet\sdk\2.0.0\Sdks on Windows or /usr/share/dotnet/sdk/2.0.0./Sdks on Linux (Ubuntu)
Then dotnet build SolutionName.sln will work fine.
These steps will fix both errors:

error MSB4236: The SDK 'Microsoft.Docker.Sdk' specified could not be found.
error MSB4022: The result "" of evaluating the value "$(DockerBuildTasksAssembly)" of the "AssemblyFile" attribute in element is not valid."

Or, you can run `dotnet sln MySolution.sln remove docker-compose.dcproj` for the ci-cd build. Stupid but that should work fine too. I don't think that will cause any issue with the `docker-compose up` part of the ci-cd pipeline either so I'm going to go ahead and do that. I'll have to be sure to re-add it though.

So just adding a shell script command to remove the .dcproj from the solution seems to work. 

Now I'm runnin into this error:
`error : The current .NET SDK does not support targeting .NET Core 2.1.`

Also something seems to have happened to my jenkins docker container's volume? It's lost all of it's configurations... wtf. Hmm I see multiple jenkins docker container processes and somehow it's been misconfigured to use the docker `jenkins-data` volume; which is still there after checking with `docker volume ls`.

I was able to fix the jenkins container by stopping all jenkins containers and removing them. After that I re-ran
```
docker run -u root -d -p 8888:8080 -p 50000:50000 -v jenkins-data:/var/jenkins_home -v /var/run/docker.sock:/var/run/docker.sock --restart always jenkinsci/blueocean
```
This worked since the jenkins-data volume is still there.

BTW, here's the command example to make a shell script executable:
```
chmod +x prebuild-remove-dcproj.sh
```

I've hit some kind of permissions error with my Jenkins build pipeline:
```
./ci-cd/prebuild-remove-dcproj.sh: Permission denied

script returned exit code 126
```

## Monday, August 16, 2018
#### Sprint 7, CI/CD Shell Scripts

Resolving jenkins shell script permissions error:
- https://stackoverflow.com/questions/26858599/build-failure-while-running-shell-command-from-jenkins
- https://stackoverflow.com/questions/47191469/jenkinsfile-permission-denied-when-running-sh-step-in-docker-container
- https://stackoverflow.com/questions/46766121/permission-denied-error-jenkins-shell-script
- https://aggarwalarpit.wordpress.com/2017/01/18/permission-denied-executing-shell-script-on-remote-host-using-ssh-jenkins/

Recursively make the entire directory executable:
- `chmod --recursive a+rwx /ci-cd/`

Do I have to make the scripts executable every time as part of the build process or can I do it then push the changes to the repo? Or is this executable permission only applicable to local machine users?
- https://stackoverflow.com/questions/3207728/retaining-file-permissions-with-git
- https://stackoverflow.com/questions/40978921/how-to-add-chmod-permissions-to-file-in-git/40979016

It looks like you can get git file permission changes checked into git with this following command:
- `git update-index --chmod=+x check_services.sh`

This can also be maintained with another tool called `git-cache-meta`.

So could I use a combination of the two commands to check into git recursive execution permissions for a whole folder?
- `git update-index --chmod --recursive a+rwx /ci-cd/`

No that didn't seem to work. I'll just run the individual git update-index permission modifier above.
