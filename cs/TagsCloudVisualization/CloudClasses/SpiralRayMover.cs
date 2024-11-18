using System.Drawing;
using TagsCloudVisualization.CloudClasses.Interfaces;

namespace TagsCloudVisualization.CloudClasses
{
    public class SpiralRayMover : ISpiralRayMover
    {
        private readonly Ray spiralRay;

        private readonly double radiusStep;

        //В радианах
        private readonly double angleStep;

        private const double OneRound = Math.PI * 2;

        public SpiralRayMover(Point center, double radiusStep = 1, double angleStep = 5,
            int startRadius = 0, int startAngle = 0)
        {
            spiralRay = new Ray(center, startRadius, startAngle);
            this.radiusStep = radiusStep;

            //Преобразование из градусов в радианы
            this.angleStep = angleStep * Math.PI / 180;
        }

        public Point Center => spiralRay.StartPoint;

        public double RadiusStep => radiusStep;

        public double AngleStep => angleStep;

        public IEnumerable<Point> MoveRay()
        {
            while (true)
            {
                yield return spiralRay.EndPoint;

                //Радиус увеличивается на 1 только после полного прохождения круга
                spiralRay.Update(radiusStep / OneRound * angleStep, angleStep);
            }
        }
    }
}
