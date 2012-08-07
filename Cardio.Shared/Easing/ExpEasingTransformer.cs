using System;

namespace Cardio.UI.Easing
{
    public class ExpEasingTransformer : IEasingTransformer
    {
        private TimeSpan _length;
        private float _start;
        private float _end;
        private float argEnd;

        public ExpEasingTransformer(float start, float end, TimeSpan length)
        {
            _length = length;
            _start = start;
            _end = end;
            argEnd = (float)Math.Log(_end / _start);
        }

        public float Transform(TimeSpan currentTime)
        {
            float arg = (float)(argEnd/(currentTime.TotalMilliseconds / _length.TotalMilliseconds));

            return (float)(Math.Exp(arg)*_start);
        }
    }
}
