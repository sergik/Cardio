using System;
using System.Collections.Generic;
using Cardio.UI.Characters.Ranged;
using Cardio.UI.Core;
using Cardio.UI.Core.Alive;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Characters.Player
{
    public interface IPlayer: IGameEntity, IPositioned, IIntelligent, IAttacker
    {
        event EventHandler Confused;

        float Health { get; set; }

        List<Nanobot> Nanobots { get; }

        bool IsMoving { get; set; }

        bool IsShooting { get; set; }

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
    }
}
