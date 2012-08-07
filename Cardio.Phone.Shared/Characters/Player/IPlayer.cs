using System;
using System.Collections.Generic;
using Cardio.Phone.Shared.Characters.Ranged;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core.Alive;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Characters.Player
{
    public interface IPlayer: IGameEntity, IPositioned, IIntelligent, IAttacker
    {
        float Health { get; set; }

        Nanobot Nanobot { get; }

        bool IsMoving { get; set; }

        bool IsShooting { get; set; }

        bool Handled { get; set; }

        /// <summary>
        /// If true - bots will evate the next boss attack
        /// </summary>
        bool CanEvadeAttack { get; set; }

        /// <summary>
        /// This property was introduced to prevent unexpected nanobots moving when they try to evade and swith simultaneously
        /// </summary>
        bool CanSwitch { get; set; }

        Vector2 BoundingSize { get; }

        float MaxHealth { get; set; }

        void Initialize(Game game, SpriteBatch spriteBatch);

        void Draw(GameTime gameTime);

        IList<DrawableGameObject> Effects { get; set; }
    }
}
