using System;
using System.Collections.Generic;
using System.Linq;
using Cardio.UI.Levels;
using Cardio.UI.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace Cardio.UI.Components
{
    public class LevelSelector
    {
        private Game _game;
        private MenuScreen _host;

        public Vector2 Position
        {
            get; set;
        }

        public int ColumnsCount
        {
            get; set;
        }

        public int ThumbnailWidth { get; set; }

        public int ThumbnailHeight { get; set; }

        public Rectangle? Size
        {
            get; set;
        }

        public List<LevelThumbnail> Thumbnails { get; set; }

        public LevelSelector()
        {
            ThumbnailWidth = 180;
            ThumbnailHeight = 120;
            Thumbnails = new List<LevelThumbnail>();
        }

        public void Initialize(Game game, MenuScreen host)
        {
            _game = game;
            _host = host;
            
            int count = 0;
            var currentThumbnailY = (int)Position.Y;
            while (count < Thumbnails.Count)
            {
                var currentThumbnailX = (int)Position.X;

                for (int i = 0; i < ColumnsCount; i++ )
                {
                    var thumbnail = Thumbnails[count];
                    thumbnail.Initialize(game, host);
                    thumbnail.ThumbnailRenderRectangle = new Rectangle(currentThumbnailX, currentThumbnailY,
                                                                       ThumbnailWidth, ThumbnailHeight);
                    count++;
                    if (count >= Thumbnails.Count)
                    {
                        break;
                    }
                    currentThumbnailX += ThumbnailWidth + 40;
                }

                currentThumbnailY += ThumbnailHeight + 50;
            }
        }

        
        public void Update(GameTime gameTime)
        {
            for (int index = 0; index < Thumbnails.Count; index++)
            {
                var levelThumbnail = Thumbnails[index];
                levelThumbnail.Update(_host, gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            _host.ScreenManager.SpriteBatch.Begin();
            for (int index = 0; index < Thumbnails.Count; index++)
            {
                var levelThumbnail = Thumbnails[index];
                levelThumbnail.Draw(_host, gameTime);
            }
            _host.ScreenManager.SpriteBatch.End();
        }

        public void HandleInput()
        {
            var levelThumbnail = Thumbnails.FirstOrDefault(t => t.IsClicked(_host));
            if (levelThumbnail != null)
            {
                levelThumbnail.OnSelected(levelThumbnail, null);
            }
        }

        public static LevelSelector FromMetadata(LevelSelectorMetadata metadata, ContentManager content)
        {
            var levelSelector = new LevelSelector();
            levelSelector.ThumbnailWidth = metadata.ThumbnailWidth;
            levelSelector.ThumbnailHeight = metadata.ThumbnailHeight;
            levelSelector.ColumnsCount = metadata.ColumnsCount;

            for (int i = 0; i < metadata.Levels.Count; i++)
            {
                LevelThumbnail thumbnail = LevelThumbnail.FromMetadata(metadata.Levels[i], content);
                thumbnail.Text = String.Format("{0}. {1}", i + 1, thumbnail.Text);
                levelSelector.Thumbnails.Add(thumbnail);
            }
            return levelSelector;
        }
    }
}
