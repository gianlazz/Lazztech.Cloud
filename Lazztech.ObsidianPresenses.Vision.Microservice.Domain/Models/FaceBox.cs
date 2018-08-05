using System.Collections.Generic;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain.Models
{
    public class FaceBoundingBox
    {
        public PixelCoordinateVertex LeftTopCoordinate { get; set; }
        public PixelCoordinateVertex RightBottomCoordinate { get; set; }
    }
}