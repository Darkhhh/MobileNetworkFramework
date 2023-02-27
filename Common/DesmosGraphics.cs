using MobileNetworkFramework.LandscapeModule.Obstacles;

namespace MobileNetworkFramework.Common;

public static class DesmosGraphics
{

    public static void DrawCylinderObstacle(CylinderObstacle obstacle)
    {
        var str =
            $"(x-{Math.Round(obstacle.Center.X, 3)})^2+(y-{Math.Round(obstacle.Center.Y, 3)})^2={Math.Round(obstacle.Range, 3)}^2";
        Console.WriteLine("Cylinder Obstacle: ");
        Console.WriteLine(str.Replace(',','.'));
        Console.WriteLine();
    }

    private static void DrawSharpOrTransparentObstacle(SharpObstacle obstacle, string type)
    {
        Console.WriteLine($"{type} Obstacle: ");
        var str2 =
            $"(x-{Math.Round(obstacle.Center.X, 3)})^2+(y-{Math.Round(obstacle.Center.Y, 3)})^2={Math.Round(obstacle.Range, 3)}^2";
        Console.WriteLine(str2.Replace(',','.'));
        foreach (var vertex in obstacle.Vertices)
        {
            var str =
                $"{Math.Round(vertex.X, 3).ToString().Replace(',', '.')}, {Math.Round(vertex.Y, 3).ToString().Replace(',', '.')}";
            Console.WriteLine(str);
        }
        Console.WriteLine();
    }
    public static void DrawSharpObstacle(SharpObstacle obstacle)
    {
        DrawSharpOrTransparentObstacle(obstacle, "Sharp");
    }

    public static void DrawTransparentObstacle(TransparentObstacle obstacle)
    {
        DrawSharpOrTransparentObstacle(obstacle, "Transparent");
    }
}