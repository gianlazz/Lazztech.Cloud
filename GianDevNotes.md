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
dotnet add Lazztech.ObsidianPresenses.Vision.Microservice.Tests/Lazztech.ObsidianPresenses.Vision.Microservice.Tests.csproj reference Lazztech.ObsidianPresenses.Vision.Microservice.Domain/Lazztech.ObsidianPresenses.Vision.Microservice.Domain.csproj
```
I then clicked restore on the vscode prompt.

All switched to xUnit now however the test failes because "Access to the path '/face/known/' is denied." so I'll need to make sure that all is handled by the interface implementation that I'm mocking not the FacialRecognitionManager.cs that depends on them and handles their stdout.

