using MobileNetworkFramework.Common.Geometry;

namespace MobileNetworkFramework.LandscapeModule.Obstacles
{
    public interface IObstacle
    {
        public bool IsPointBelongTo(Point point);
        public bool DoesItIntersect(Point start, Point end);
    }
}
