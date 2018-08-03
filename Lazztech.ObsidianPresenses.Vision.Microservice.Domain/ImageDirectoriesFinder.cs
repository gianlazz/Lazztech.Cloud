namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain
{
    public class ImageDirectoriesFinder : IImageDirectoryFinder
    {
        public string KnownImageDir { get; set; }
        public string UnknownImageDir { get; set; }
        public string KnownUnknownImagDir { get; set; }
        public string NoPersonsFoundDir { get; set; }

        private static string knownPath = @"/face/known/";
        private static string unknownPath = @"/face/unknown/";
        private static string knownUnknownPath = @"/face/known_unknown/";
        private static string noPersonsFoundPath = @"/face/no_persons_found/";

        public ImageDirectoriesFinder()
        {
            KnownImageDir = knownPath;
            UnknownImageDir = unknownPath;
            KnownUnknownImagDir = knownUnknownPath;
            NoPersonsFoundDir = noPersonsFoundPath;
        }

        public string[] GetAllKnownImageDirs()
        {
            throw new System.NotImplementedException();
        }

        public string[] GetAllKnownUnknownImageDirs()
        {
            throw new System.NotImplementedException();
        }

        public string[] GetAllUnknownImageDirs()
        {
            throw new System.NotImplementedException();
        }

        public string[] GetAllNoPersonsFoundImageDirs()
        {
            throw new System.NotImplementedException();
        }
    }
}