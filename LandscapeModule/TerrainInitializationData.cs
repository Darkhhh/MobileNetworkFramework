using MobileNetworkFramework.LandscapeModule.Obstacles;

namespace MobileNetworkFramework.LandscapeModule;

public class TerrainInitializationData
{
    #region Quad Data

    public int QuadLength { get; set; } = -1;
    public int QuadWidth { get; set; } = -1;

    #endregion


    #region Common Obstacles Data

    public float ObstacleRange { get; set; } = -1.0f;

    public int CylinderObstaclesNumber { get; set; } = -1;

    public int SharpObstaclesNumber { get; set; } = -1;

    public int TransparentObstaclesNumber { get; set; } = -1;

    #region Additional Obstacles Data

    public int MinimumVerticesForSharpObstacle { get; set; } = -1;
    public int MaximumVerticesForSharpObstacle { get; set; } = -1;

    public List<CustomObstacle> CustomObstacles { get; set; } = new List<CustomObstacle>();

    #endregion

    #endregion
    
    public TerrainInitializationData(){}
}