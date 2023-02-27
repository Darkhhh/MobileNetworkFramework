namespace MobileNetworkFramework.LandscapeModule;

public class LandscapeData
{
    #region Generic Data

    public float Area { get; set; }
    public int ObstaclesNumber { get; set; }
    public float ObstaclesRange { get; set; }

    #endregion

    #region Obstacles Number

    public int CylinderObstaclesNumber { get; set; }
    public int SharpObstaclesNumber { get; set; }
    public int TransparentObstaclesNumber { get; set; }
    public int CustomObstaclesNumber { get; set; }
    public int ComplexObstaclesNumber { get; set; }

    #endregion

    #region Data

    public float FreeSpace { get; set; }
    public float AverageLengthOfFreeTrail { get; set; }

    #endregion
}