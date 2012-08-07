using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Easing
{
    public interface IEasingTransformer
    {
        float Transform(TimeSpan currentTime);
    }
}
