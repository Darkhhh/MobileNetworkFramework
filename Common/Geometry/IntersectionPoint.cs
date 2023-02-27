namespace MobileNetworkFramework.Common.Geometry;

public class IntersectionPoint :Point, IComparable<IntersectionPoint>
{
    public float Length { get; set; }

    public IntersectionPoint(float length, float xValue, float yValue) : base(xValue, yValue)
    {
        Length = length;
    }

    public int CompareTo(IntersectionPoint comparePart)
    {
        return this.Length.CompareTo(comparePart.Length);
    }
}