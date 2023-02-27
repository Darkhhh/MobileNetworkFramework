using MobileNetworkFramework.Common.Geometry;

namespace MobileNetworkFramework.LandscapeModule.Obstacles
{
    public interface IConnectableObstacle: IObstacle
    {
        public Point Center { get; }
        public float Range { get; }
        
        public static bool AreObstaclesIntersect(IConnectableObstacle first, IConnectableObstacle second)
        {
            var distanceBetweenCenters = Point.GetDistanceBetweenPoints(first.Center, second.Center);
            return distanceBetweenCenters <= 2 * first.Range || distanceBetweenCenters <= 2 * second.Range;
        }
    }
}

