using System.Collections.Generic;

namespace Cardio.UI.Rhythm
{
    public class BeatPattern
    {
        public IList<float> Beats { get; set; }

        public int BeatCount
        {
            get { return Beats.Count; }
        }

        public BeatPattern(): this(new List<float>())
        {
            // blank
        }

        public BeatPattern(IList<float> beats)
        {
            Beats = beats;
        }
    }
}
