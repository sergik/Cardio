using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.UI.Scenes;
using Cardio.UI.Scenes.Scripts;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Tutorial
{
    public class TutorialScriptActivationScene: Scene
    {
        public IGameScript Script { get; set; }

        public TutorialScriptActivationScene()
        {
            ActivationThreshold = -400;
        }

        public override void Update(GameTime gameTime, Core.GameState gameState)
        {
            if (!IsFinished && gameState.Player.WorldPosition.X >= StartPosition)
            {
                IsFinished = true;
                //gameState.AddScript(Script);
            }

            base.Update(gameTime, gameState);
        }

        public override bool CanMovePlayer(Core.GameState state)
        {
            return true;
        }
    }
}
