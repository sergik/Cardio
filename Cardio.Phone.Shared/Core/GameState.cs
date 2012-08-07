using System;
using System.Collections.Generic;
using System.Linq;
using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Scripts;
using Cardio.Phone.Shared.Actions;
using Cardio.Phone.Shared.Characters.Player;
using Cardio.Phone.Shared.Inventory.Level;
using Cardio.Phone.Shared.Levels;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Core
{
    public class GameState
    {
        public const float ScreenLength = 800;
        public Game Game { get; set; }

        public ICamera2D Camera { get; set; }

        public IPlayer Player { get; set; }

        public Level Level { get; set; }

        public InventoryService Inventory { get; set; }

        public ComboController Combo { get; set; }

        public GameStory GameStory { get; set; }

        public ReactionProgressComponent ReactionProgress { get; set; }

        public bool IsInventoryEnabled { get; set; }

        public bool AreControlsEnabled { get; set; }

        public bool IsHeartbeatEnabled { get; set; }

        public bool IsHealthBarEnabled { get; set; }

        public bool IsGodModeEnabled { get; set; }

        public bool IsGameOver { get; set; }

        public bool HandledMouseOnThisUpdate { get; set; }

        public bool IsHandlingSequence
        {
            get
            { 
                return BlockingHandlers.Any();
            }
        }

        public GameStoryEntry CurrentGameStoryEntry { get; set; }

        public GameServiceContainer ServiceProvider { get; set; }

        public List<GameEntityScript> ActiveScripts { get; private set; }

        private readonly IList<GameEntityScript> _scriptsAddedOnThisUpdate = new List<GameEntityScript>();

        public List<Object> BlockingHandlers { get; private set; }

        private bool _isUpdating;

        private bool _shouldClearAllScripts;

        public GameState(Game game)
        {
            Game = game;
            ActiveScripts = new List<GameEntityScript>();
            BlockingHandlers = new List<object>();
            Camera = new Camera2D();
            Player = new Player();
            Level = new Level();
            Inventory = new InventoryService();
            ReactionProgress = new ReactionProgressComponent();
        }

        public void AddScript(GameEntityScript script)
        {
            (_isUpdating ? _scriptsAddedOnThisUpdate : ActiveScripts).Add(script);
            script.Start(this);
        }

        public void UpdateScripts(GameTime gameTime)
        {
            _isUpdating = true;

            for (int index = 0; index < ActiveScripts.Count; index++)
            {
                var script = ActiveScripts[index];
                script.Update(this, gameTime);
            }

            if (_shouldClearAllScripts)
            {
                _shouldClearAllScripts = false;
                ActiveScripts.Clear();
            }

            for (int i = ActiveScripts.Count - 1; i >= 0; i-- )
            {
                if (ActiveScripts[i].IsFinished)
                {
                    ActiveScripts.RemoveAt(i);
                }
            }

            for (int index = 0; index < _scriptsAddedOnThisUpdate.Count; index++)
            {
                var script = _scriptsAddedOnThisUpdate[index];
                ActiveScripts.Add(script);
            }
            _scriptsAddedOnThisUpdate.Clear();

            _isUpdating = false;
        }

        public void ClearScripts()
        {
            _shouldClearAllScripts = true;
        }

        public void RegisterBlockingHandler(Object token)
        {
            BlockingHandlers.Add(token);
        }

        public void ReleaseBlockingHandler(Object token)
        {
            BlockingHandlers.Remove(token);
            if (BlockingHandlers.Count == 0)
            {
                ReactionProgress.Reset();
            }
        }

        public void Reset()
        {
            Inventory.Reset();
            GameStory.Reset();
        }
    }
}
