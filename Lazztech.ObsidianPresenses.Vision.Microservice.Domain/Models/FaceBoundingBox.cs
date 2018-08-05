using System.Collections.Generic;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain.Models
{
    public struct FaceBoundingBox
    {
        public PixelCoordinateVertex LeftTopCoordinate { get; set; }
        public PixelCoordinateVertex RightBottomCoordinate { get; set; }
    }
}