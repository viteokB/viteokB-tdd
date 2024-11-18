using FluentAssertions;
using NUnit.Framework.Interfaces;
using System.Drawing;
using TagsCloudVisualization.CloudClasses;
using TagsCloudVisualization.Visualisation;

namespace TagsCloudVisualization.Tests
{
    public class CircularCloudLayouterTests
    {
        private const int WIDTH = 1000;

        private const int HEIGHT = 1000;

        private CircularCloudLayouter _layouter;

        private Point _center;

        private IVisualiser _visualiser;

        private TagCloudImageGenerator _tagCloudImageGenerator;

        [SetUp]
        public void Setup()
        {
            _center = new Point(WIDTH / 2, HEIGHT / 2);

            var mover = new SpiralRayMover(_center);

            _layouter = new CircularCloudLayouter(mover);

            _visualiser = new Visualiser(Color.Black, 1);
            
            _tagCloudImageGenerator = new TagCloudImageGenerator(_visualiser);
        }

        [TearDown]
        public void Teardown()
        {
            _visualiser.Dispose();
            _tagCloudImageGenerator.Dispose();
        }

        [Test]
        public void CircularCloudLayouter_WhenCreated_RectanglesShoudBeEmpty()
        {
            _layouter.Rectangles.Should().BeEmpty();
        }

        [Test]
        public void CircularCloudLayouter_WhenCreated_FirstPointEqualsCenter()
        {
            var firstPoint = _layouter.RayMover.MoveRay().First();

            firstPoint.Should().BeEquivalentTo(_layouter.RayMover.Center);
        }

        [Test]
        public void CircularCloudLayouter_WhenAddFirstRectangle_ContainOneAndSameRectangle()
        {
            var rectangle = _layouter.PutNextRectangle(new Size(20, 10));

            _layouter.Rectangles
                .Select(r => r).Should().HaveCount(1).And.Contain(rectangle);
        }

        [Test]
        public void CircularCloudLayouter_WhenAddRectangle_ReturnsRectangleWithSameSize()
        {
            var givenSize = new Size(20, 20);

            var rectangle = _layouter.PutNextRectangle(givenSize);

            rectangle.Size.Should().BeEquivalentTo(givenSize);
        }

        [TestCase(0, 0)]
        [TestCase(-2, 2)]
        [TestCase(2, -2)]
        [TestCase(-2, -2)]
        public void CircularCloudLayouter_WhenWrongSize_ThrowArgumentException(int width, int height)
        {
            Assert.Throws<ArgumentException>(() => _layouter.PutNextRectangle(new Size(width, height)),
                "Not valid size should be positive");
        }

        [TestCase(0, 0)]
        [TestCase(-2, 2)]
        [TestCase(2, -2)]
        [TestCase(-2, -2)]
        public void CircularCloudLayouter_WhenCenterWrong_ThrowArgumentException(int x, int y)
        {
            var rayMover = new SpiralRayMover(new Point(x, y), 1, 5);

            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(rayMover),
                "Center Point should have positive X and Y");
        }

        [TestCase(0, 0)]
        [TestCase(-2, 2)]
        [TestCase(2, -2)]
        [TestCase(-2, -2)]
        public void CircularCloudLayouter_WhenNotValidSteps_ThrowArgumentException(int radiusStep, int angleSter)
        {
            var rayMover = new SpiralRayMover(new Point(10, 10), radiusStep, angleSter);

            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(rayMover), 
                "radiusStep and angleStep should be positive");
        }

        [Test]
        public void CircularCloudLayouter_WhenAddFew_ShouldHaveSameCount()
        {
            var currentBitmap = _tagCloudImageGenerator.CreateNewBitmap(new Size(WIDTH, HEIGHT),
                _layouter,
                () =>
                {
                    return SizeBuilder.Configure()
                        .SetCount(5)
                        .SetWidth(60, 80)
                        .SetHeight(60, 80)
                        .Generate();
                });

            try
            {
                _layouter.Rectangles.Should().HaveCount(5);
            }
            catch (Exception e)
            {
                var fileName = $"{TestContext.CurrentContext.Test.MethodName + Guid.NewGuid()}.png";

                BitmapSaver.SaveToFail(currentBitmap, fileName);
                currentBitmap.Dispose();

                throw;
            }
        }

        [TestCase(1, 2, 3)]
        public void CircularCloudLayouter_ShouldIncreaseRectangleCountCorrectly(int countBefore, 
            int add, int countAfter)
        {
            //Создали состояние countBefore
            var currentBitmap = _tagCloudImageGenerator.CreateNewBitmap(new Size(WIDTH, HEIGHT),
                _layouter,
                () =>
                {
                    return SizeBuilder.Configure()
                        .SetCount(countBefore)
                        .SetWidth(60, 80)
                        .SetHeight(60, 80)
                        .Generate();
                });

            //Создали состояние countAfter
            _tagCloudImageGenerator.AddToCurrentImage(currentBitmap,
                _layouter,
                () =>
                {
                    return SizeBuilder.Configure()
                        .SetCount(add)
                        .SetWidth(60, 80)
                        .SetHeight(60, 80)
                        .Generate();
                });

            try
            {
                _layouter.Rectangles.Should().HaveCount(countAfter);
            }
            catch (Exception e)
            {
                var fileName = $"{TestContext.CurrentContext.Test.MethodName + Guid.NewGuid()}.png";

                BitmapSaver.SaveToFail(currentBitmap, fileName);
                currentBitmap.Dispose();

                throw;
            }
        }

        [TestCase(2)]
        [TestCase(30)]
        public void CircularCloudLayouter_WhenAddFew_RectangleNotIntersectsWithOtherRectangles(int count)
        {
            //Создали состояние countBefore
            var currentBitmap = _tagCloudImageGenerator.CreateNewBitmap(new Size(WIDTH, HEIGHT), _layouter,
                () =>
                {
                    return SizeBuilder.Configure()
                        .SetCount(count)
                        .SetWidth(60, 80)
                        .SetHeight(60, 80)
                        .Generate();
                });

            try
            {
                foreach (var rectangle in _layouter.Rectangles)
                {
                    _layouter.Rectangles
                        //Не забываем исключить самого себя.....
                        .Any(r => r.IntersectsWith(rectangle) && rectangle != r)
                        .Should().BeFalse();
                }
            }
            catch (Exception e)
            {
                var fileName = $"{TestContext.CurrentContext.Test.MethodName + Guid.NewGuid()}.png";

                BitmapSaver.SaveToFail(currentBitmap, fileName);
                currentBitmap.Dispose();

                throw;
            }
        }
    }
}