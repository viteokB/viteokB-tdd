using System.Drawing;

namespace TagsCloudVisualization.CloudClasses.Interfaces
{
    // Интерфейс для класса, который перемещает луч по спирали, генерируя последовательность точек.
    public interface ISpiralRayMover
    {
        IEnumerable<Point> MoveRay();

        public Point Center { get; }

        public double RadiusStep { get; }

        public double AngleStep { get; }
    }
}
