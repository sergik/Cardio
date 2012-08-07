
using System.Collections.Generic;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Scripts;
using Cardio.Phone.Shared.Scripts;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Core
{
    public abstract class GameEntity : IGameEntity
    {
        private readonly IList<GameEntityScript> _scripts = new List<GameEntityScript>();
        private readonly IList<GameEntityScript> _scriptsAddedOnThisUpdate = new List<GameEntityScript>();

        private bool _isUpdatingScripts;

        public void AddScript(GameEntityScript script)
        {
            (_isUpdatingScripts ? _scriptsAddedOnThisUpdate : _scripts).Add(script);
        }

        public virtual void Update(GameState gameState, GameTime gameTime)
        {
            _isUpdatingScripts = true;

            if (_scripts != null)
            {
                for (int index = 0; index < _scripts.Count; index++)
                {
                    var script = _scripts[index];
                    if (!script.IsStarted)
                    {
                        script.Start(gameState);
                    }

                    script.Update(gameState, gameTime);
                }
            }

            for (int i = _scripts.Count - 1; i >= 0; i-- )
            {
                if (_scripts[i].IsFinished)
                {
                    _scripts.RemoveAt(i);
                }
            }

            for (int index = 0; index < _scriptsAddedOnThisUpdate.Count; index++)
            {
                var script = _scriptsAddedOnThisUpdate[index];
                _scripts.Add(script);
            }

            _scriptsAddedOnThisUpdate.Clear();
            _isUpdatingScripts = false;
        }
    }
}