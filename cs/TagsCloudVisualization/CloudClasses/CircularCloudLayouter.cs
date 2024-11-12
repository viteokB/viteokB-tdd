using System.Drawing;
using TagsCloudVisualization.CloudClasses.Interfaces;

namespace TagsCloudVisualization.CloudClasses
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public readonly List<Rectangle> Rectangles;

        public readonly Point Center;

        public readonly IRayMover RayMover;

        public CircularCloudLayouter(Point center, double radiusStep = 1, double angleStep = 5)
        {
            if (center.X <= 0 || center.Y <= 0)
                throw new ArgumentException("Center Point should have positive X and Y");

            if (radiusStep <= 0 || angleStep <= 0)
                throw new ArgumentException("radiusStep and angleStep should be positive");

            Rectangles = new List<Rectangle>();
            Center = center;

            RayMover = new SpiralMover(center, radiusStep, angleStep);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Not valid size should be positive");

            foreach (var point in RayMover.MoveRay())
            {
                var location = new Point(point.X - rectangleSize.Width / 2,
                    point.Y - rectangleSize.Height / 2);

                var rectangle = new Rectangle(location, rectangleSize);

                // Проверяем, пересекается ли новый прямоугольник с уже существующими
                if (!Rectangles.Any(r => r.IntersectsWith(rectangle)))
                {
                    Rectangles.Add(rectangle);
                    return rectangle;
                }
            }

            return new Rectangle();
        }
    }
}
