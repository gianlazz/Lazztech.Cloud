namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain
{
    public interface IImageDirectoriesFinder
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