using MobileNetworkFramework.Common.Geometry;
using MobileNetworkFramework.LandscapeModule.Obstacles;

namespace MobileNetworkFramework.LandscapeModule;

public static class TerrainConstructor
{
    public static Terrain Empty(Quad terrainQuad) => new Terrain(terrainQuad);

    public static Terrain CreateNew(TerrainInitializationData data)
    {
        var system = new ObstaclesSystem();
        var terrainQuad = CreateTerrainQuad(data);
        
        if (data.CylinderObstaclesNumber != -1) CreateCylinderObstacles(data, ref system, terrainQuad);
        if (data.SharpObstaclesNumber != -1) CreateSharpObstacles(data, ref system, terrainQuad);
        if (data.TransparentObstaclesNumber != -1) CreateTransparentObstacles(data, ref system, terrainQuad);
        if (data.CustomObstacles.Count > 0)
        {
            foreach (var obstacle in data.CustomObstacles) system.AddCustomObstacle(obstacle);
        }

        system.SearchForComplexObstacles();

        return new Terrain(system, terrainQuad, data);
    }


    private static Quad CreateTerrainQuad(TerrainInitializationData initData)
    {
        if ((initData.QuadWidth == -1) || (initData.QuadLength == -1))
            throw new Exception("Terrain Quad parameters are not defined");
        return new Quad(
            new Point(0, 0), 
            new Point(initData.QuadWidth, 0),
            new Point(initData.QuadLength, 0), 
            new Point(initData.QuadWidth, initData.QuadLength));
    }

    private static void CreateCylinderObstacles(TerrainInitializationData initData, ref ObstaclesSystem system, 
        Quad terrainQuad)
    {
        if (initData.CylinderObstaclesNumber != -1)
        {
            system.CreateObstacles(
                ObstaclesSystem.ObstacleType.Cylinder, 
                initData.CylinderObstaclesNumber,
                initData.ObstacleRange,
                terrainQuad);
        }
    }

    private static void CreateSharpObstacles(TerrainInitializationData initData, ref ObstaclesSystem system,
        Quad terrainQuad)
    {
        if (initData.MinimumVerticesForSharpObstacle == -1 ||
            initData.MaximumVerticesForSharpObstacle == -1)
            throw new Exception("Additional data for SharpObstacle is not defined");
        SharpObstacle.SetMinMaxVertices(
            initData.MinimumVerticesForSharpObstacle,
            initData.MaximumVerticesForSharpObstacle);
                
        system.CreateObstacles(
            ObstaclesSystem.ObstacleType.Sharp,
            initData.SharpObstaclesNumber,
            initData.ObstacleRange,
            terrainQuad);
    }

    private static void CreateTransparentObstacles(TerrainInitializationData initData, ref ObstaclesSystem system,
        Quad terrainQuad)
    {
        if (initData.MinimumVerticesForSharpObstacle == -1 ||
            initData.MaximumVerticesForSharpObstacle == -1)
            throw new Exception("Additional data for TransparentObstacle is not defined");
        if (!SharpObstacle.ValuesWereInitialized)
        {
            SharpObstacle.SetMinMaxVertices(
                initData.MinimumVerticesForSharpObstacle,
                initData.MaximumVerticesForSharpObstacle);
        }

        system.CreateObstacles(
            ObstaclesSystem.ObstacleType.Transparent,
            initData.TransparentObstaclesNumber,
            initData.ObstacleRange,
            terrainQuad);
    }
}