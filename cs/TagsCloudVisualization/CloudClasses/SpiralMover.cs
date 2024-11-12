using System.Drawing;
using TagsCloudVisualization.CloudClasses.Interfaces;

namespace TagsCloudVisualization.CloudClasses
{
    public class SpiralMover : IRayMover
    {
        public readonly Ray SpiralRay;

        public readonly double RadiusStep;

        //В радианах
        public readonly double AngleStep;

        public const double OneRound = Math.PI * 2;

        public SpiralMover(Point center, double radiusStep = 1, double angleStep = 5,
            int startRadius = 0, int startAngle = 0)
        {
            SpiralRay = new Ray(center, startRadius, startAngle);
            RadiusStep = radiusStep;

            //Преобразование из градусов в радианы
            AngleStep = angleStep * Math.PI / 180;
        }

        public IEnumerable<Point> MoveRay()
        {
            while (true)
            {
                yield return SpiralRay.GetEndPoint;

                //Радиус увеличивается на 1 только после полного прохождения круга
                SpiralRay.Update(RadiusStep / OneRound * AngleStep, AngleStep);
            }
        }
    }
}
