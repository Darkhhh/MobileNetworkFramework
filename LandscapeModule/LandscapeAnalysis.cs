using MobileNetworkFramework.Common;
using MobileNetworkFramework.Common.Geometry;

namespace MobileNetworkFramework.LandscapeModule;

public class LandscapeAnalysis
{
    #region Properties

    private readonly Terrain _terrain;
    public LandscapeData Data { get; private set; }
    
    #endregion


    #region Constructors

    public LandscapeAnalysis(Terrain terrain)
    {
        _terrain = terrain;
        Data = new LandscapeData();
    }

    #endregion


    #region Generic Data

    public void CountArea() => Data.Area = _terrain.TerrainQuad.Area;
    public void CountObstaclesNumber() => Data.ObstaclesNumber = _terrain.ObstaclesSystem.ObstaclesNumber;
    public void CountObstaclesRange() => Data.ObstaclesRange = _terrain.InitData.ObstacleRange;
    public void CountCylinderObstaclesNumber() => Data.CylinderObstaclesNumber = _terrain.InitData.CylinderObstaclesNumber;
    public void CountSharpObstaclesNumber() => Data.SharpObstaclesNumber = _terrain.InitData.SharpObstaclesNumber;
    public void CountTransparentObstaclesNumber() => Data.TransparentObstaclesNumber = _terrain.InitData.TransparentObstaclesNumber;
    public void CountCustomObstaclesNumber() => Data.CustomObstaclesNumber = _terrain.InitData.CustomObstacles.Count;
    public void CountComplexObstaclesNumber() => Data.ComplexObstaclesNumber = _terrain.ObstaclesSystem.ComplexObstaclesNumber;

    #endregion


    #region Additional Data

    public void CountFreeSpace(int randomGeneratedPointsNumber)
    {
        var freePoints = 0;
        var p = new Point();
        for (var i = 0; i < randomGeneratedPointsNumber; i++)
        {
            Randomizer.RandomPointInQuad(_terrain.TerrainQuad, ref p);
            if (!_terrain.ObstaclesSystem.IsPointBelongTo(p)) freePoints++;
        }
        Data.FreeSpace = (float)freePoints / randomGeneratedPointsNumber;
    }

    public void CountAverageLengthOfFreeTrail(int numOfTestSegments)
    {
        /*
         * Генерируем 500 отрезков с концами на границе области
         * Находим все точки пересечений с препятствиями
         * Сортируем их так, чтобы все точки были последовательны (по увеличению расстояния от одной из точек отрезка
         * Берем каждые две точки i и i+1
         * Если середина отрезка не принадлежит препятствию значит отрезок свободен, добавляем его длину в общую сумму
         * и увеличиваем количество свободных отрезков
         */
        Point first = new Point(), second = new Point(), middlePoint = new Point();
        var intersectionPoints = new List<Point>();
        var obstacles = _terrain.ObstaclesSystem.IntersectableObstacles;
        var points = new List<IntersectionPoint>();
    
        var segmentsAmount = 0f;
        var averageLength = 0f;
        for (var i = 0; i < numOfTestSegments; i++)
        {
            Randomizer.RandomTwoPointsOnQuad(_terrain.TerrainQuad, ref first, ref second);
            intersectionPoints.Clear();
            points.Clear();
    
            foreach (var obstacle in obstacles)
            {
                var list = obstacle.GetIntersectionPoints(first, second, out var numOfPoints);
                if(numOfPoints > 0) intersectionPoints.AddRange(list);
            }

            foreach (var point in intersectionPoints)
            {
                points.Add(new IntersectionPoint(
                    Point.GetDistanceBetweenPoints(first, point), 
                    (float)point.X,
                    (float)point.Y));
            }
                
            points.Sort();
            
            for (var j = 0; j < points.Count - 1; j++)
            {
                Segment.GetMiddlePoint(points[j], points[j + 1], ref middlePoint);
                if (!_terrain.ObstaclesSystem.IsPointBelongTo(middlePoint)) continue;
                segmentsAmount++;
                averageLength += points[j + 1].Length - points[j].Length;
            }
        }
        
        Data.AverageLengthOfFreeTrail = averageLength / segmentsAmount;
    }

    #endregion
}