namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain
{
    public interface IImageDirectoryFinder
    {
        string KnownImageDir { get; set; } 
        string UnknownImageDir { get; set; }
        string KnownUnknownImagDir { get; set; }
        string NoPersonsFoundDir { get; set; }

        string[] GetAllKnownImageDirs();
        string[] GetAllUnknownImageDirs();
        string[] GetAllKnownUnknownImageDirs();
        string[] GetAllNoPersonsFoundImageDirs();
    }
}