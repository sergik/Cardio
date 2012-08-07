using System;
using Cardio.UI.Characters.Player;
using Cardio.UI.Components;
using Cardio.UI.Core;
using Cardio.UI.Inventory;
using Cardio.UI.Levels;
using Cardio.UI.Particles;
using Cardio.UI.Screens;
using Cardio.UI.Scripts;
using Microsoft.Xna.Framework;
using Cardio.UI.Extensions;

namespace Cardio.UI
{
    public static class TestData
    {
        public static Level BuildTestLevel2(Game game)
        {
            var metadata = game.Content.Load<LevelMetadata>(@"Levels\Level00");
            return Level.FromMetadata(metadata, game.Content);
        }

        private static GameStory BuildGameStory(Game game)
        {
            var metadata = game.Content.Load<GameStoryMetadata>(@"Story\GameStory");
            return GameStory.FromMetadata(metadata, game.Content);
        }

        public static void EmitParticle(Particle x, Level level)
        {
            x.Died += (s, e) => level.RemoveLevelObject(s as Particle);
            level.AddLevelObject(x);
        }

        public static LevelScreen BuildTestLevelScreen2(Game game)
        {
            return BuildLevelScreen(game, () => BuildTestLevel2(game));
        }

        public static GameState CreateDefaultGameState(Game game)
        {
            var gameState = new GameState(game);

            gameState.Camera = new Camera2D
                                   {
                                       Scale = 1f,
                                       FocusedAt = gameState.Player
                                   };

            gameState.Camera.FocusedAtOffset = new Vector2(370, 70) / gameState.Camera.Scale;
            gameState.IsHealthBarEnabled = true;
            gameState.IsInventoryEnabled = true;
            gameState.IsHeartbeatEnabled = true;
            gameState.IsGodModeEnabled = false;
            gameState.AreControlsEnabled = true;

            gameState.Inventory.Add(Items.BloodElement, 800);

            gameState.Inventory.ToActive(0, 0);
            gameState.Inventory.ToActive(1, 1);

            gameState.ServiceProvider = game.Services;

            gameState.GameStory = BuildGameStory(game);

            return gameState;
        }

        public static LevelScreen BuildLevelScreen(Game game, Func<Level> createLevel)
        {
            var screen = new LevelScreen();
            var gameState = game.Services.GetService<GameState>();

            gameState.Player = new Player
            {
                WorldPosition = new Vector2(0, 0),
                AttackDamage = 60f,
                AttackRange = 1280
            };

            gameState.Camera.Position = Vector2.Zero;
            gameState.Camera.FocusedAt = gameState.Player;
            gameState.AreControlsEnabled = true;
            gameState.IsGameOver = false;

            gameState.Level = createLevel();

            gameState.Inventory.Set(Items.SmallShield, 10);
            gameState.Inventory.Set(Items.SmallMedkit, 10);

            foreach (var bot in gameState.Player.Nanobots)
            {
                bot.EmitParticle = x => EmitParticle(x, screen.Level);
            }

            return screen;
        }
    }
}
