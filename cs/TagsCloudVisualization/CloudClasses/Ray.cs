using System.Drawing;

namespace TagsCloudVisualization.CloudClasses
{
    public class Ray
    {
        public double Radius { get; private set; }

        //В радианах
        public double Angle { get; private set; }

        public readonly Point RayStart;

        public Ray(Point rayStart, int radius, int angle)
        {
            RayStart = rayStart;

            Radius = radius;

            Angle = angle;
        }

        public void Update(double deltaRadius, double deltaAngle)
        {
            Radius += deltaRadius;
            Angle += deltaAngle;
        }

        public Point GetEndPoint => new Point
        {
            X = RayStart.X + (int)(Radius * Math.Cos(Angle)),
            Y = RayStart.Y + (int)(Radius * Math.Sin(Angle))
        };
    }
}
