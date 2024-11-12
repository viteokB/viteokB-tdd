using System.Drawing;

namespace TagsCloudVisualization.CloudClasses.Interfaces
{
    public interface ICloudLayouter
    {
        public Rectangle PutNextRectangle(Size rectangleSize);
    }
}
