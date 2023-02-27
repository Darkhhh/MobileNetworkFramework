namespace MobileNetworkFramework.Common.Geometry
{
    public class Quad
    {
        #region Properties

        public float Area => _area;
        public Point Center => _center;

        public Point LeftDownCorner { get; private set; }
        public Point LeftUpCorner { get; private set; }
        public Point RightDownCorner { get; private set; }
        public Point RightUpCorner { get; private set; }

        public double MinX => LeftDownCorner.X;
        public double MaxX => RightDownCorner.X;
        public double MinY => LeftDownCorner.Y;
        public double MaxY => LeftUpCorner.Y;

        #endregion


        #region Private Values

        private float _area;

        private Point _center;

        #endregion


        #region Constructors

        public Quad(): 
            this(new Point(), new Point(), new Point(), new Point())
        {
            
        }

        public Quad(Point leftDownCorner, Point rightUpCorner): 
            this(leftDownCorner, new Point(rightUpCorner.X, leftDownCorner.Y),
            new Point(leftDownCorner.X, rightUpCorner.Y), rightUpCorner)
        {
            
        }

        public Quad(Point leftDownCorner, Point rightDownCorner, Point leftUpCorner, Point rightUpCorner)
        {
            LeftDownCorner = leftDownCorner;
            RightDownCorner = rightDownCorner;
            LeftUpCorner = leftUpCorner;
            RightUpCorner = rightUpCorner;

            _area = CountArea();
            _center = GetCenter();
        }

        #endregion


        #region Private Methods

        private float CountArea()
        {
            float width = (float)(MaxX - MinX), height = (float)(MaxY - MinY);
            return width * height;
        }

        private Point GetCenter()
        {
            var xLength = Math.Abs(MaxX - MinX);
            var yLength = Math.Abs(MaxY - MinY);
            return new Point(MinX + xLength / 2, MinY + yLength / 2);
        }

        #endregion


        #region Public Methods

        public bool IsPointBelongTo(Point coordinates)
        {
            return coordinates.X > MinX && coordinates.X < MaxX &&
                   coordinates.Y > MinY && coordinates.Y < MaxY;
        }

        #endregion
    }
}
