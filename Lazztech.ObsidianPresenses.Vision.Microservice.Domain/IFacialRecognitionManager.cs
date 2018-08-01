using System.Collections.Generic;
using Lazztech.ObsidianPresenses.Vision.Microservice.Domain.Models;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain
{
    public interface IFacialRecognitionManager
    {
        List<Snapshot> Results { get; set; }     
        void Process();
    }
}