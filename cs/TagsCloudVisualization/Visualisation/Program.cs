using System.Drawing;
using TagsCloudVisualization.CloudClasses;

namespace TagsCloudVisualization.Visualisation
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using var tagCloudGenerator = new TagCloudImageGenerator(new Visualiser(Color.Black, 1));

            using var bitmap1 = tagCloudGenerator.CreateNewBitmap(new Size(1000, 1000),
                new CircularCloudLayouter(new Point(500, 500)),
                () =>
            {
                return SizeBuilder.Configure()
                    .SetCount(40)
                    .SetWidth(60, 80)
                    .SetHeight(60, 80)
                    .Generate();
            });

            using var bitmap2 = tagCloudGenerator.CreateNewBitmap(new Size(1000, 1000),
                new CircularCloudLayouter(new Point(500, 500)),
                () =>
            {
                return SizeBuilder.Configure()
                    .SetCount(100)
                    .SetWidth(10, 40)
                    .SetHeight(20, 30)
                    .Generate();
            });

            using var bitmap3 = tagCloudGenerator.CreateNewBitmap(new Size(1000, 1000),
                new CircularCloudLayouter(new Point(500, 500)),
                () =>
            {
                return SizeBuilder.Configure()
                    .SetCount(30)
                    .SetWidth(80, 130)
                    .SetHeight(80, 130)
                    .Generate();
            });

            BitmapSaver.SaveToCorrect(bitmap1, "bitmap_1.png");
            BitmapSaver.SaveToCorrect(bitmap2, "bitmap_2.png");
            BitmapSaver.SaveToCorrect(bitmap3, "bitmap_3.png");
        }
    }
}
