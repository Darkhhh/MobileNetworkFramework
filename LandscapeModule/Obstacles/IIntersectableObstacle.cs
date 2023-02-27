using MobileNetworkFramework.Common.Geometry;

namespace MobileNetworkFramework.LandscapeModule.Obstacles;

public interface IIntersectableObstacle
{
    public List<Point> GetIntersectionPoints(Point start, Point end, out int numOfPoints);
}