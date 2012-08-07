using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Characters.Player
{
    public class NanobotGroup
    {
        private const int NanobotsCount = 4;

        private List<Nanobot> _nanobots;

        public Vector2 Position { get; set; }

        public bool IsMoving { get; set; }

        public bool IsShooting { get; set; }

        public NanobotGroup()
        {
            CreateNanobots(NanobotsCount);
        }

        private void CreateNanobots(int nanobotsCount)
        {
            _nanobots = new List<Nanobot>();
            for (int i = 0; i < nanobotsCount; i++)
            {
                _nanobots.Add(new Nanobot());
            }
        }

        public void Initialize(Game game, SpriteBatch spriteBatch, ICamera2D camera)
        {
           for (int i = 0; i < _nanobots.Count; i++)
           {
               _nanobots[i].Initialize(game, spriteBatch, camera);
           }
        }
    }
}
