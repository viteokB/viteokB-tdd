using FluentAssertions;
using NUnit.Framework.Interfaces;
using System.Drawing;
using TagsCloudVisualization.CloudClasses;
using TagsCloudVisualization.Visualisation;

namespace TagsCloudVisualization.Tests
{
    public class Tests
    {
        private const int WIDTH = 1000;

        private const int HEIGHT = 1000;

        private CircularCloudLayouter _layouter;

        private Point _center;

        private IVisualiser _visualiser;

        private Bitmap _currentBitmap = null;

        private TagCloudImageGenerator _tagCloudImageGenerator;

        [SetUp]
        public void Setup()
        {
            _center = new Point(WIDTH / 2, HEIGHT / 2);
            _layouter = new CircularCloudLayouter(_center);

            _visualiser = new Visualiser(Color.Black, 1);
            _tagCloudImageGenerator = new TagCloudImageGenerator(_visualiser);
        }

        [TearDown]
        public void Teardown()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure && _currentBitmap != null)
            {
                var fileName = $"{TestContext.CurrentContext.Test.MethodName + Guid.NewGuid()}.png";

                BitmapSaver.SaveToFail(_currentBitmap, fileName);
                _currentBitmap.Dispose();
            }

            _visualiser.Dispose();
            _tagCloudImageGenerator.Dispose();
        }

        [Test]
        public void CircularCloudLayouter_WhenCreated_RectanglesShoudBeEmpty()
        {
            _layouter.Rectangles.Should().BeEmpty();
        }

        [Test]
        public void CircularCloudLayouter_WhenCreated_FirstPointEqualsLayouterCenter()
        {
            var firstPoint = _layouter.RayMover.MoveRay().First();

            firstPoint.Should().BeEquivalentTo(_layouter.Center);
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
            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(new Point(x, y)),
                "Center Point should have positive X and Y");
        }

        [TestCase(0, 0)]
        [TestCase(-2, 2)]
        [TestCase(2, -2)]
        [TestCase(-2, -2)]
        public void CircularCloudLayouter_WhenNotValidSteps_ThrowArgumentException(int radiusStep, int angleSter)
        {
            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(new Point(10, 10), radiusStep, angleSter),
                "radiusStep and angleStep should be positive");
        }

        [TestCase(5)]
        public void CircularCloudLayouter_WhenAddFew_ShouldHaveSameCount(int add)
        {
            _currentBitmap = _tagCloudImageGenerator.CreateNewBitmap(new Size(WIDTH, HEIGHT),
                _layouter,
                () =>
                {
                    return SizeBuilder.Configure()
                        .SetCount(add)
                        .SetWidth(60, 80)
                        .SetHeight(60, 80)
                        .Generate();
                });

            _layouter.Rectangles.Should().HaveCount(add);
        }

        [TestCase(1, 2, 3)]
        public void CircularCloudLayouter_WhenAddFewButWasBefore_ShouldHaveCorrectCount(int countBefore, int add, int countAfter)
        {
            //Создали состояние countBefore
            _currentBitmap = _tagCloudImageGenerator.CreateNewBitmap(new Size(WIDTH, HEIGHT),
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
            _tagCloudImageGenerator.AddToCurrentImage(_currentBitmap,
                _layouter,
                () =>
                {
                    return SizeBuilder.Configure()
                        .SetCount(add)
                        .SetWidth(60, 80)
                        .SetHeight(60, 80)
                        .Generate();
                });

            _layouter.Rectangles.Should().HaveCount(countAfter);
        }

        [TestCase(2)]
        [TestCase(30)]
        public void CircularCloudLayouter_WhenAddFew_RectangleNotIntersectsWithOtherRectangles(int count)
        {
            //Создали состояние countBefore
            _currentBitmap = _tagCloudImageGenerator.CreateNewBitmap(new Size(WIDTH, HEIGHT),
                _layouter,
                () =>
                {
                    return SizeBuilder.Configure()
                        .SetCount(count)
                        .SetWidth(60, 80)
                        .SetHeight(60, 80)
                        .Generate();
                });

            foreach (var rectangle in _layouter.Rectangles)
            {
                _layouter.Rectangles
                    //Не забываем исключить самого себя.....
                    .Any(r => r.IntersectsWith(rectangle) && rectangle != r)
                    .Should().BeFalse();
            }
        }
    }
}