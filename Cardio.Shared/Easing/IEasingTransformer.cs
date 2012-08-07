using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Easing
{
    public interface IEasingTransformer
    {
        float Transform(TimeSpan currentTime);
    }
}
