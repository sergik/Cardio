using System.Collections.Generic;
using System.Linq;
using Cardio.Phone.Shared.Levels;
using Microsoft.Xna.Framework.Content;

namespace Cardio.Phone.Shared.Levels
{
    public sealed class GameStory
    {
        public int NextLevelIndex { get; set; }

        public GameStoryEntry NextLevel
        {
            get
            {
                return NextLevelIndex < Levels.Count ? Levels[NextLevelIndex] : null;
            }
        }

        public List<GameStoryEntry> Levels
        {
            get; 
            private set;
        }

        public void MarkLevelAsPassed(string levelName)
        {
            for (int i = 0; i < Levels.Count; i++)
            {
                if (Levels[i].Name == levelName && NextLevelIndex <= i)
                {
                    if (i == Levels.Count - 1)
                    {
                    }
                    else
                    {
                        NextLevelIndex++;
                    }
                    return;
                }
            }
        }

        public bool IsLevelAvailable(GameStoryEntry level)
        {
            int levelIndex = Levels.IndexOf(level);
            return levelIndex <= NextLevelIndex;
        }

        public void Reset()
        {
            NextLevelIndex = 0;
        }

        public GameStoryEntry GetStoryEntry(string levelName)
        {
            return Levels.Single(storyEntry => storyEntry.Name == levelName);
        }

        private GameStory(IEnumerable<GameStoryEntry> levels)
        {
            Levels = levels.ToList();
        }

        public static GameStory FromMetadata(GameStoryMetadata metadata, ContentManager contentManager)
        {
            var levels =
                metadata.Levels.Select(entry => new GameStoryEntry
                                                    {
                                                        Name = entry.Name,
                                                        GetLevel = () => Level.FromMetadata(contentManager.Load<LevelMetadata>(entry.LevelAsset), contentManager)
                                                    });
            return new GameStory(levels);
        }
    }
}
