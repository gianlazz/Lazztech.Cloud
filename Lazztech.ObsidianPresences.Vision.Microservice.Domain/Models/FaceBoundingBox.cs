namespace Lazztech.ObsidianPresences.Vision.Microservice.Domain.Models
{
    public struct FaceBoundingBox
    {
        public PixelCoordinateVertex LeftTopCoordinate { get; set; }
        public PixelCoordinateVertex RightBottomCoordinate { get; set; }
    }
}