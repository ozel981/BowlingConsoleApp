namespace Kregle
{
    public interface IFrame
    {
        int FirstThrowPoints { get; }
        int NextThrowPoints { get; }
        void AddNextThrow(int points);
        int GetFramePoints();
        int GetGamePoints();
        IFrame GetFrame(int frameNumber);
        IEnumerable<IFrame> GetFramsesEnumerable();
        string ToString();
        bool IsHalfFrame();
    }

    public abstract class Frame : IFrame
    {
        protected IFrame nextFrame = new NullFrame();

        public abstract int FirstThrowPoints { get; }

        public abstract int NextThrowPoints { get; }

        public void AddNextThrow(int points)
        {
            if (points > 10 || points < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (nextFrame is NullFrame)
            {
                if (points == 10)
                {
                    nextFrame = new StrikeFrame();
                } 
                else
                {
                    nextFrame = new HalfFrame(points);
                }
            } 
            else if (nextFrame is HalfFrame)
            {
                if (nextFrame.FirstThrowPoints + points > 10)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else if (nextFrame.FirstThrowPoints + points == 10)
                {
                    nextFrame = new SpareFrame(nextFrame.FirstThrowPoints, points);
                }
                else if (nextFrame.FirstThrowPoints + points == 0)
                {
                    nextFrame = new MissFrame();
                }
                else
                {
                    nextFrame = new OpenFrame(nextFrame.FirstThrowPoints, points);
                }
            }
            else
            {
                nextFrame.AddNextThrow(points);
            }
        }

        public IFrame GetFrame(int frameNumber)
        {
            if (frameNumber == 0)
            {
                return this;
            }
            else
            {
                return nextFrame.GetFrame(frameNumber - 1);
            }
        }

        public abstract int GetFramePoints();

        public IEnumerable<IFrame> GetFramsesEnumerable()
        {
            yield return this;
            foreach (IFrame frame in nextFrame.GetFramsesEnumerable())
            {
                yield return frame;
            }
        }

        public abstract int GetGamePoints();

        public bool IsHalfFrame() => nextFrame.IsHalfFrame();

        public abstract string ToString();
    }

    public class NullFrame : IFrame
    {
        public int FirstThrowPoints => 0;

        public int NextThrowPoints => 0;

        public void AddNextThrow(int points) { }

        public IFrame GetFrame(int frameNumber)
        {
            throw new IndexOutOfRangeException($"Frame with {frameNumber} does not exist");
        }

        public int GetFramePoints() => 0;

        public IEnumerable<IFrame> GetFramsesEnumerable()
        {
            yield break;
        }

        public int GetGamePoints() => 0;

        public bool IsHalfFrame() => false;

        public string ToString()
        {
            return "||";
        }
    }

    public class HalfFrame : IFrame
    {
        private int points = 0;
        public int FirstThrowPoints => points;

        public int NextThrowPoints => 0;

        public HalfFrame(int points = 0)
        {
            this.points = points;
        }

        public void AddNextThrow(int points) { }

        public int GetFramePoints() => points;

        public int GetGamePoints() => points;

        public IFrame GetFrame(int frameNumber)
        {
            if (frameNumber == 0)
            {
                return this;
            }
            else
            {
                throw new IndexOutOfRangeException($"Frame with {frameNumber} does not exist");
            }
        }

        public IEnumerable<IFrame> GetFramsesEnumerable()
        {
            yield return this;
        }

        public bool IsHalfFrame() => true;

        public string ToString()
        {
            return $"|[{FirstThrowPoints}][ ]|";
        }
    }

    public class StartFrame : Frame
    {
        public override int FirstThrowPoints => 0;

        public override int NextThrowPoints => 0;

        public override int GetFramePoints() => 0;

        public override int GetGamePoints() => nextFrame.GetGamePoints();

        public override string ToString()
        {
            return $"";
        }

    }

    public class MissFrame : Frame
    {
        public override int FirstThrowPoints => 0;

        public override int NextThrowPoints => 0;

        public override int GetFramePoints() => 0;

        public override int GetGamePoints() => nextFrame.GetGamePoints();

        public override string ToString()
        {
            return $"|[{FirstThrowPoints}][{NextThrowPoints}]M|";
        }
    }

    public class StrikeFrame : Frame
    {
        public override int FirstThrowPoints => 10;

        public override int NextThrowPoints => nextFrame.FirstThrowPoints;

        public override int GetFramePoints() => FirstThrowPoints + 
            nextFrame.FirstThrowPoints + nextFrame.NextThrowPoints;

        public override int GetGamePoints() => GetFramePoints() +
            nextFrame.GetGamePoints();

        public override string ToString()
        {
            return $"|[{FirstThrowPoints}]X|";
        }
    }

    public class SpareFrame : Frame
    {
        private int firstThrowPoints;
        private int secondThrowPoints;
        public override int FirstThrowPoints => firstThrowPoints;

        public override int NextThrowPoints => secondThrowPoints;
        public SpareFrame(int firstThrowPoints, int secondThrowPoints)
        {
            this.firstThrowPoints = firstThrowPoints;
            this.secondThrowPoints = secondThrowPoints;
        }
        public override int GetFramePoints() => 10 + nextFrame.FirstThrowPoints;

        public override int GetGamePoints() => GetFramePoints() +
            nextFrame.GetGamePoints();

        public override string ToString()
        {
            return $"|[{FirstThrowPoints}][{NextThrowPoints}]/|";
        }
    }

    public class OpenFrame : Frame
    {
        private int firstThrowPoints;
        private int secondThrowPoints;
        public override int FirstThrowPoints => firstThrowPoints;

        public override int NextThrowPoints => secondThrowPoints;

        public OpenFrame(int firstThrowPoints, int secondThrowPoints)
        {
            this.firstThrowPoints = firstThrowPoints;
            this.secondThrowPoints = secondThrowPoints;
        }

        public override int GetFramePoints() => FirstThrowPoints +
            NextThrowPoints;

        public override int GetGamePoints() => GetFramePoints() +
            nextFrame.GetGamePoints();

        public override string ToString()
        {
            return $"|[{FirstThrowPoints}][{NextThrowPoints}]O|";
        }
    }
}
