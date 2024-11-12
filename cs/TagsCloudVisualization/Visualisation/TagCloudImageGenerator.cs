using System.Drawing;
using TagsCloudVisualization.CloudClasses;

namespace TagsCloudVisualization.Visualisation
{
    public class TagCloudImageGenerator : IDisposable
    {
        private readonly IVisualiser Visualiser;

        private bool isDisposed = false;

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

        ~TagCloudImageGenerator()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool fromMethod)
        {
            if (isDisposed)
                return;

            if (fromMethod)
            {
                Visualiser.Dispose();
            }

            isDisposed = true;
        }
    }
}
