using Kregle;

namespace KregleTests
{
    public class FrameTests
    {
        [Fact]

        public void MissFrameTest()
        {
            IFrame missFrame = new MissFrame();
            Assert.Equal(0, missFrame.FirstThrowPoints);
            Assert.Equal(0, missFrame.NextThrowPoints);
            Assert.Equal(0, missFrame.GetFramePoints());
            Assert.Equal(0, missFrame.GetGamePoints());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(10)]
        public void HalfFrameTest(int points)
        {
            IFrame halfFrame = new HalfFrame(points);
            Assert.Equal(points, halfFrame.FirstThrowPoints);
            Assert.Equal(0, halfFrame.NextThrowPoints);
            Assert.Equal(points, halfFrame.GetFramePoints());
            Assert.Equal(points, halfFrame.GetGamePoints());
        }

        [Fact]
        public void StartFrameTest()
        {
            IFrame startFrame = new StartFrame();
            Assert.Equal(0, startFrame.FirstThrowPoints);
            Assert.Equal(0, startFrame.NextThrowPoints);
            Assert.Equal(0, startFrame.GetFramePoints());
            Assert.Equal(0, startFrame.GetGamePoints());
        }

        [Fact]
        public void AddingMissFrame()
        {
            IFrame startFrame = new StartFrame();
            startFrame.AddNextThrow(0);
            startFrame.AddNextThrow(0);
            MissFrame missFrame = Assert.IsType<MissFrame>(startFrame.GetFrame(1));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(9)]
        public void AddingHalfFrame(int points)
        {
            IFrame startFrame = new StartFrame();
            startFrame.AddNextThrow(points);
            HalfFrame halfFrame = Assert.IsType<HalfFrame>(startFrame.GetFrame(1));
            Assert.Equal(points, halfFrame.FirstThrowPoints);
            Assert.Equal(0, halfFrame.NextThrowPoints);
            Assert.Equal(points, halfFrame.GetFramePoints());
            Assert.Equal(points, halfFrame.GetGamePoints());
            Assert.Equal(points, startFrame.GetGamePoints());
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(5, 5)]
        [InlineData(1, 9)]
        [InlineData(9, 1)]
        public void AddingSpareFrame(int throw1, int throw2)
        {
            IFrame startFrame = new StartFrame();
            startFrame.AddNextThrow(throw1);
            startFrame.AddNextThrow(throw2);
            SpareFrame spareFrame = Assert.IsType<SpareFrame>(startFrame.GetFrame(1));
            Assert.Equal(10, spareFrame.GetFramePoints());
            Assert.Equal(10, spareFrame.GetGamePoints());
            Assert.Equal(10, startFrame.GetGamePoints());
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(1, 1)]
        public void AddingOpenFrame(int throw1, int throw2)
        {
            IFrame startFrame = new StartFrame();
            startFrame.AddNextThrow(throw1);
            startFrame.AddNextThrow(throw2);
            OpenFrame openFrame = Assert.IsType<OpenFrame>(startFrame.GetFrame(1));
            Assert.Equal(throw1 + throw2, openFrame.GetFramePoints());
            Assert.Equal(throw1 + throw2, openFrame.GetGamePoints());
            Assert.Equal(throw1 + throw2, startFrame.GetGamePoints());
        }

        [Fact]
        public void AddingStrikeFrame()
        {
            IFrame startFrame = new StartFrame();
            startFrame.AddNextThrow(10);
            StrikeFrame strikeFrame = Assert.IsType<StrikeFrame>(startFrame.GetFrame(1));
            Assert.Equal(10, strikeFrame.FirstThrowPoints);
            Assert.Equal(0, strikeFrame.NextThrowPoints);
            Assert.Equal(10, strikeFrame.GetFramePoints());
            Assert.Equal(10, strikeFrame.GetGamePoints());
            Assert.Equal(10, startFrame.GetGamePoints());
        }

        [Fact]
        public void ThreeStrikeFrames()
        {
            IFrame startFrame = new StartFrame();
            startFrame.AddNextThrow(10);
            startFrame.AddNextThrow(10);
            startFrame.AddNextThrow(10);
            StrikeFrame strikeFrame1 = Assert.IsType<StrikeFrame>(startFrame.GetFrame(1));
            StrikeFrame strikeFrame2 = Assert.IsType<StrikeFrame>(startFrame.GetFrame(2));
            StrikeFrame strikeFrame3 = Assert.IsType<StrikeFrame>(startFrame.GetFrame(3));
            Assert.Equal(30, strikeFrame1.GetFramePoints());
            Assert.Equal(20, strikeFrame2.GetFramePoints());
            Assert.Equal(10, strikeFrame3.GetFramePoints());
            Assert.Equal(60, startFrame.GetGamePoints());
        }

        [Theory]
        [InlineData(1,9,8,1)]
        [InlineData(0,10,3,3)]
        [InlineData(5,5,0,1)]
        [InlineData(9,1,1,8)]
        public void StrikeSpareOpenFrames(int throw2, int throw3, int throw4, int throw5)
        {
            IFrame startFrame = new StartFrame();
            startFrame.AddNextThrow(10);
            startFrame.AddNextThrow(throw2);
            startFrame.AddNextThrow(throw3);
            startFrame.AddNextThrow(throw4);
            startFrame.AddNextThrow(throw5);
            StrikeFrame strikeFrame = Assert.IsType<StrikeFrame>(startFrame.GetFrame(1));
            SpareFrame spareFrame = Assert.IsType<SpareFrame>(startFrame.GetFrame(2));
            OpenFrame openFrame = Assert.IsType<OpenFrame>(startFrame.GetFrame(3));
            Assert.Equal(throw4 + throw5, openFrame.GetFramePoints());
            Assert.Equal(throw2 + throw3 + throw4, spareFrame.GetFramePoints());
            Assert.Equal(throw2 + throw3 + 2 * throw4 + throw5, spareFrame.GetGamePoints());
            Assert.Equal(10 + throw2 + throw3, strikeFrame.GetFramePoints());
            Assert.Equal(10 + 2 * throw2 + 2 * throw3 + 2 * throw4 + throw5, startFrame.GetGamePoints());
        }

        [Fact]
        public void StrikeMissStrikeFrames()
        {
            IFrame startFrame = new StartFrame();
            startFrame.AddNextThrow(10);
            startFrame.AddNextThrow(0);
            startFrame.AddNextThrow(0);
            startFrame.AddNextThrow(10);
            StrikeFrame strikeFrame1 = Assert.IsType<StrikeFrame>(startFrame.GetFrame(1));
            MissFrame missFrame = Assert.IsType<MissFrame>(startFrame.GetFrame(2));
            StrikeFrame strikeFrame2 = Assert.IsType<StrikeFrame>(startFrame.GetFrame(3));
            Assert.Equal(10, strikeFrame1.GetFramePoints());
            Assert.Equal(0, missFrame.GetFramePoints());
            Assert.Equal(10, strikeFrame2.GetFramePoints());
            Assert.Equal(20, startFrame.GetGamePoints());
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(1, 1)]
        public void StrikeOpenStrikeFrames(int throw2, int throw3)
        {
            IFrame startFrame = new StartFrame();
            startFrame.AddNextThrow(10);
            startFrame.AddNextThrow(throw2);
            startFrame.AddNextThrow(throw3);
            startFrame.AddNextThrow(10);
            StrikeFrame strikeFrame1 = Assert.IsType<StrikeFrame>(startFrame.GetFrame(1));
            OpenFrame openFrame = Assert.IsType<OpenFrame>(startFrame.GetFrame(2));
            StrikeFrame strikeFrame2 = Assert.IsType<StrikeFrame>(startFrame.GetFrame(3));
            Assert.Equal(10 + throw2 + throw3, strikeFrame1.GetFramePoints());
            Assert.Equal(throw2 + throw3, openFrame.GetFramePoints());
            Assert.Equal(10, strikeFrame2.GetFramePoints());
            Assert.Equal(20 + 2 * (throw2 + throw3), startFrame.GetGamePoints());
        }

        [Theory]
        [InlineData(0, 10, 0, 10, 0, 10)]
        [InlineData(5, 5, 5, 5, 5, 5)]
        [InlineData(9, 1, 9, 1, 9, 1)]
        public void SpareSpareSpareFrames(int throw1, int throw2, int throw3, int throw4, int throw5, int throw6)
        {
            IFrame startFrame = new StartFrame();
            startFrame.AddNextThrow(throw1);
            startFrame.AddNextThrow(throw2);
            startFrame.AddNextThrow(throw3);
            startFrame.AddNextThrow(throw4);
            startFrame.AddNextThrow(throw5);
            startFrame.AddNextThrow(throw6);
            SpareFrame spareFrame1 = Assert.IsType<SpareFrame>(startFrame.GetFrame(1));
            SpareFrame spareFrame2 = Assert.IsType<SpareFrame>(startFrame.GetFrame(2));
            SpareFrame spareFrame3 = Assert.IsType<SpareFrame>(startFrame.GetFrame(3));
            Assert.Equal(throw1 + throw2 + throw3, spareFrame1.GetFramePoints());
            Assert.Equal(throw3 + throw4 + throw5, spareFrame2.GetFramePoints());
            Assert.Equal(throw5 + throw6, spareFrame3.GetFramePoints());
            Assert.Equal(throw1 + throw2 + 2 * throw3 + throw4 + 2 * throw5 + throw6, startFrame.GetGamePoints());
        }
    }
}