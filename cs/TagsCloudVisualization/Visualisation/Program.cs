using System.Drawing;
using TagsCloudVisualization.CloudClasses;

namespace TagsCloudVisualization.Visualisation
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var visualiser = new Visualiser(Color.Black, 1);

            var tagCloundGenerator = new TagCloudImageGenerator(visualiser);

            using var bitmap1 = tagCloundGenerator.CreateNewBitmap(new Size(1000, 1000),
                new CircularCloudLayouter(new Point(500, 500)),
                () =>
            {
                return SizeBuilder.Configure()
                    .SetCount(40)
                    .SetWidth(60, 80)
                    .SetHeight(60, 80)
                    .Generate();
            });

            using var bitmap2 = tagCloundGenerator.CreateNewBitmap(new Size(1000, 1000),
                new CircularCloudLayouter(new Point(500, 500)),
                () =>
            {
                return SizeBuilder.Configure()
                    .SetCount(100)
                    .SetWidth(10, 40)
                    .SetHeight(20, 30)
                    .Generate();
            });

            using var bitmap3 = tagCloundGenerator.CreateNewBitmap(new Size(1000, 1000),
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
