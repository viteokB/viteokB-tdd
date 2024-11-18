using System.Drawing;
using System.Runtime.CompilerServices;
using TagsCloudVisualization.CloudClasses.Interfaces;

namespace TagsCloudVisualization.CloudClasses
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public readonly List<Rectangle> Rectangles = new List<Rectangle>();

        public readonly ISpiralRayMover RayMover;

        public CircularCloudLayouter(ISpiralRayMover rayMover)
        {
            if (rayMover.Center.X <= 0 || rayMover.Center.Y <= 0)
                throw new ArgumentException("IRayMover center Point should have positive X and Y");

            if (rayMover.RadiusStep <= 0 || rayMover.AngleStep <= 0)
                throw new ArgumentException("radiusStep and angleStep should be positive");

            RayMover = rayMover;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("The height and width of the Rectangle must be greater than 0");

            var rectangle = Rectangle.Empty;

            foreach (var point in RayMover.MoveRay())
            {
                var location = new Point(point.X - rectangleSize.Width / 2,
                    point.Y - rectangleSize.Height / 2);

                rectangle = new Rectangle(location, rectangleSize);

                // Проверяем, пересекается ли новый прямоугольник с уже существующими
                if (!Rectangles.Any(r => r.IntersectsWith(rectangle)))
                {
                    Rectangles.Add(rectangle);
                    return rectangle;
                }
            }

            throw new InvalidOperationException("No suitable location found for the rectangle.");
        }
    }
}
