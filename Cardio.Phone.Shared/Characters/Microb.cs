using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.Phone.Shared.Characters.Ranged;
using Microsoft.Xna.Framework.Content;

namespace Cardio.Phone.Shared.Characters
{
    public class Microb: RangeEnemy
    {
        protected Microb() {}

        public static new Microb FromMetadata(RangeEnemyMetadata metadata, ContentManager contentManager)
        {
            var microb = new Microb();
            FillWithMetadata(microb, metadata, contentManager);

            microb.Content.AddAnimationRule("Default", () => microb.IsAlive);
            microb.Content.AddAnimationRule("Blow", () => !microb.IsAlive);

            return microb;
        }
    }
}
