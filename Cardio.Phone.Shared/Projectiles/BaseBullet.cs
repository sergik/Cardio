using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Extensions;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Projectiles
{
    public abstract class BaseBullet : DrawableGameObject
    {
        public float Speed { get; set; }

        public float HitDamage { get; set; }

        public void Unload(Game game)
        {
            var cache = Game.Services.GetService<GameCache>();
            cache.Add(this);
        }
    }
}
