using System.Drawing;
using TagsCloudVisualization.CloudClasses;

namespace TagsCloudVisualization.Visualisation
{
    public class TagCloudImageGenerator
    {
        public readonly IVisualiser Visualiser;

        public TagCloudImageGenerator(IVisualiser visualiser)
        {
            Visualiser = visualiser;
        }

        public Bitmap CreateNewBitmap(Size bitmapSize, CircularCloudLayouter layouter,
            Func<IEnumerable<Size>> configurationFunc)
        {
            var newBitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);

            foreach (var size in configurationFunc())
            {
                Visualiser.DrawRectangle(newBitmap, layouter.PutNextRectangle(size));
            }

            return newBitmap;
        }

        public void AddToCurrentImage(Bitmap bitmap, CircularCloudLayouter layouter,
            Func<IEnumerable<Size>> configurationFunc)
        {
            foreach (var size in configurationFunc())
            {
                Visualiser.DrawRectangle(bitmap, layouter.PutNextRectangle(size));
            }
        }
    }
}
