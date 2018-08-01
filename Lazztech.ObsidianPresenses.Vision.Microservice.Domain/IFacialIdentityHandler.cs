using System.Collections.Generic;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain
{
    public interface IFacialIdentityHandler
    {
         List<string> FaceRecognition();
    }
}