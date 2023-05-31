using MobileNetworkFramework.Common.Geometry;
using MobileNetworkFramework.LandscapeModule.Obstacles;

namespace MobileNetworkFramework.LandscapeModule
{
    public class Terrain
    {
        public ObstaclesSystem ObstaclesSystem { get; set; }
        public Quad TerrainQuad { get; set; }
        public TerrainInitializationData InitData { get; set; }

        public Terrain(ObstaclesSystem obstaclesSystem,Quad terrainQuad, TerrainInitializationData initData)
        {
            ObstaclesSystem = obstaclesSystem;
            TerrainQuad = terrainQuad;
            InitData = initData;
        }

        public Terrain(Quad terrainQuad)
        {
            ObstaclesSystem = new ObstaclesSystem();
            TerrainQuad = terrainQuad;
            InitData = new TerrainInitializationData();
        }
    }
}

