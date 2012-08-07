using System;
using System.IO;
using System.IO.IsolatedStorage;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core;

namespace Cardio.Phone.Shared.Persistance
{
    public class SavedGame
    {
        public const string DefaultSaveGameFile = "save.dat";

        public int Money { get; set; }

        public string Gun { get; set; }

        public string Shield { get; set; }

        public string Body { get; set; }

        public int NextLevelIndex { get; set; }

        public string Plazma { get; set; }

        public string Medcit { get; set; }

        public string Spikes { get; set; }

        public string Tablet { get; set; }

        private static SavedGame MapToSavedGame(GameState gameState)
        {
            var savedGame = new SavedGame
                                {
                                    Body = gameState.Inventory.Body,
                                    Gun = gameState.Inventory.Gun,
                                    Shield = gameState.Inventory.Shield,
                                    Money = gameState.Inventory.Money,
                                    NextLevelIndex = gameState.GameStory.NextLevelIndex,
                                    Plazma = gameState.Inventory.FrontWeapon,
                                    Medcit = gameState.Inventory.Medcit,
                                    Spikes = gameState.Inventory.Spike,
                                    Tablet = gameState.Inventory.Tablet
                                };
            return savedGame;
        }

        private static void PopulateWithLoadedData(GameState sourceState, SavedGame loadedGame)
        {
            sourceState.Inventory.Body = loadedGame.Body;
            sourceState.Inventory.Gun = loadedGame.Gun;
            sourceState.Inventory.Shield = loadedGame.Shield;
            sourceState.Inventory.Money = loadedGame.Money;
            sourceState.GameStory.NextLevelIndex = loadedGame.NextLevelIndex;
            sourceState.Inventory.FrontWeapon = loadedGame.Plazma;
            sourceState.Inventory.Medcit = loadedGame.Medcit;
            sourceState.Inventory.Spike = loadedGame.Spikes;
            sourceState.Inventory.Tablet = loadedGame.Tablet;

            return;
        }

        public static void Save(GameState gameState)
        {
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication(); // grab the storage
            var game = MapToSavedGame(gameState);
            using (var stream = store.OpenFile(DefaultSaveGameFile, FileMode.Create))
            {
                var writer = new BinaryWriter(stream);
                writer.Write(game.Money);
                writer.Write(game.Gun);
                writer.Write(game.Shield);
                writer.Write(game.Body);
                writer.Write(game.NextLevelIndex);
                writer.Write(game.Plazma ?? String.Empty);
                writer.Write(game.Medcit ?? String.Empty);
                writer.Write(game.Spikes ?? String.Empty);
                writer.Write(game.Tablet ?? String.Empty);
                writer.Close();
            }
        }
        
        public static GameState Load(string filename, GameState state)
        {
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();

            if (store.FileExists(DefaultSaveGameFile)) // Check if file exists
            {
                var save = new IsolatedStorageFileStream(DefaultSaveGameFile, FileMode.Open, store);
                var reader = new BinaryReader(save);
                var game = new SavedGame();
                game.Money = reader.ReadInt32();
                game.Gun = reader.ReadString();
                game.Shield = reader.ReadString();
                game.Body = reader.ReadString();
                game.NextLevelIndex = reader.ReadInt32();
                game.Plazma = reader.ReadString();
                game.Medcit = reader.ReadString();
                game.Spikes = reader.ReadString();
                game.Tablet = reader.ReadString();
                reader.Close();
                PopulateWithLoadedData(state, game);
            }
            return state;
        }
    }
}
