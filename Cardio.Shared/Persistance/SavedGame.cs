using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Cardio.UI.Core;

namespace Cardio.UI.Persistance
{
    public class SavedGame
    {
        public const string DefaultSaveGameFile = "save.dat";

        public int Money { get; set; }

        public string Gun { get; set; }

        public string Shield { get; set; }

        public string Body { get; set; }

        public int NextLevelIndex { get; set; }

        private static SavedGame MapToSavedGame(GameState gameState)
        {
            var savedGame = new SavedGame
                                {
                                    Body = gameState.Inventory.Body,
                                    Gun = gameState.Inventory.Gun,
                                    Shield = gameState.Inventory.Shield,
                                    Money = gameState.Inventory.Money,
                                    NextLevelIndex = gameState.GameStory.NextLevelIndex
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

            return;
        }

        public static void Save(string filename, GameState gameState)
        {
            var game = MapToSavedGame(gameState);
            var formatter = new BinaryFormatter();
            using (var stream = new FileStream(filename, FileMode.Create))
            {
                formatter.Serialize(stream, game);
            }
        }
        
        public static GameState Load(string filename, GameState state)
        {
            if (File.Exists(filename))
            {
                try
                {
                    var formater = new BinaryFormatter();
                    using (var stream = new FileStream(filename, FileMode.Open))
                    {
                        var game = (SavedGame)formater.Deserialize(stream);
                        if (game != null)
                        {
                            PopulateWithLoadedData(state, game);
                        }
                    }
                }
                catch
                {
                    return state;
                }
                
            }
            return null;
        }
    }
}
