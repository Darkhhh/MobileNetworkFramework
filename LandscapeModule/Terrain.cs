using MobileNetworkFramework.Common.Geometry;
using MobileNetworkFramework.LandscapeModule.Obstacles;

namespace MobileNetworkFramework.LandscapeModule
{
    public class Terrain
    {
        public ObstaclesSystem ObstaclesSystem { get; set; } = new ObstaclesSystem();
        public Quad TerrainQuad { get; set; }
        public TerrainInitializationData InitData { get; set; }

        public Terrain(TerrainInitializationData initData)
        {
            if ((initData.QuadWidth == -1) || (initData.QuadLength == -1))
                throw new Exception("Terrain Quad parameters are not defined");
            TerrainQuad = new Quad(
                new Point(0, 0), 
                new Point(initData.QuadWidth, 0),
                new Point(initData.QuadLength, 0), 
                new Point(initData.QuadWidth, initData.QuadLength));

            if((initData.CylinderObstaclesNumber != -1 || initData.SharpObstaclesNumber != -1 ||
               initData.TransparentObstaclesNumber != -1) && Math.Abs(initData.ObstacleRange - (-1.0f)) < float.Epsilon)
                throw new Exception("Obstacles Range is not defined");
            
            if (initData.CylinderObstaclesNumber != -1)
            {
                ObstaclesSystem.CreateObstacles(
                    ObstaclesSystem.ObstacleType.Cylinder, 
                    initData.CylinderObstaclesNumber,
                    initData.ObstacleRange,
                    TerrainQuad);
            }

            if (initData.SharpObstaclesNumber != -1)
            {
                if (initData.MinimumVerticesForSharpObstacle == -1 ||
                    initData.MaximumVerticesForSharpObstacle == -1)
                    throw new Exception("Additional data for SharpObstacle is not defined");
                SharpObstacle.SetMinMaxVertices(
                    initData.MinimumVerticesForSharpObstacle,
                    initData.MaximumVerticesForSharpObstacle);
                
                ObstaclesSystem.CreateObstacles(
                    ObstaclesSystem.ObstacleType.Sharp,
                    initData.SharpObstaclesNumber,
                    initData.ObstacleRange,
                    TerrainQuad);
            }
            
            if (initData.TransparentObstaclesNumber != -1)
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

                ObstaclesSystem.CreateObstacles(
                    ObstaclesSystem.ObstacleType.Transparent,
                    initData.TransparentObstaclesNumber,
                    initData.ObstacleRange,
                    TerrainQuad);
            }

            if (initData.CustomObstacles.Count > 0)
            {
                foreach (var obstacle in initData.CustomObstacles) ObstaclesSystem.AddCustomObstacle(obstacle);
            }

            InitData = initData;

            ObstaclesSystem.SearchForComplexObstacles();
        }

        private Terrain(Quad terrainQuad)
        {
            TerrainQuad = terrainQuad;
            InitData = new TerrainInitializationData();
        }

        public static Terrain Empty(Quad terrainQuad) => new Terrain(terrainQuad);
    }
}

