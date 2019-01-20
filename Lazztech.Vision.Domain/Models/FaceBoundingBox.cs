namespace Lazztech.Cloud.Vision.Domain.Models
{
    public struct FaceBoundingBox
    {
        public PixelCoordinateVertex LeftTopCoordinate { get; set; }
        public PixelCoordinateVertex RightBottomCoordinate { get; set; }
    }
}