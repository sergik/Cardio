using System;
using Cardio.UI.Core;
using Cardio.UI.Input;
using Cardio.UI.Extensions;
using Cardio.UI.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Levels
{
    public class LevelThumbnail
    {
        private readonly GrowingMenuEntry _menuEntry;

        private readonly Color _disabledTintColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);

        private bool _isSelected;
        
        public Vector2 Position
        {
            get; set;
        }

        public String Text
        {
            get
            {
                return _menuEntry.Text;
            }
            set
            {
                _menuEntry.Text = value;
            }
        }

        public Vector2 TextOffset
        {
            get; set;
        }

        public Texture2D Texture
        {
            get; set;
        }

        public Texture2D Border
        {
            get;
            set;
        }

        public GameStoryEntry StoryEntry { get; private set; }

        public bool IsDisabled { get; set; }

        /// <summary>
        /// Path to the thumbnail texture.
        /// </summary>
        public String ThumbnailFileName { get; private set; }

        private Rectangle _thumbnailRenderRectangle;
        /// <summary>
        /// Screen render position and size.
        /// </summary>
        public Rectangle ThumbnailRenderRectangle
        {
            get { return _thumbnailRenderRectangle; }
            set
            {
                _thumbnailRenderRectangle = value;
                Position = new Vector2(value.X, value.Y);
            }
        }

        /// <summary>
        /// Information about the corresponding level state.
        /// </summary>
        public LevelState LevelState { get; set; }

        public string LevelName
        {
            get; set;
        }

        public event EventHandler<EventArgs> Selected;

        public LevelThumbnail()
        {
            _menuEntry = new GrowingMenuEntry("");
            TextOffset = new Vector2(0, 5);
        }

        public void Initialize(Game game, MenuScreen host)
        {
            _menuEntry.Initialize(game);
            var gameState = game.Services.GetService<GameState>();
            StoryEntry = gameState.GameStory.GetStoryEntry(LevelName);
            if  (gameState.GameStory.IsLevelAvailable(StoryEntry))
            {
                Selected += (o, e) => InformativeLoadingScreen.Load(host.ScreenManager, true,
                                           TestData.BuildLevelScreen(game, StoryEntry.GetLevel));
            }
            else
            {
                IsDisabled = true;
                _menuEntry.IsDisabled = true;
            }
            Border = game.Content.Load<Texture2D>(@"Textures\Borders\Border1");
        }

        

        public virtual void Update(MenuScreen screen, GameTime gameTime)
        {
            _isSelected = _menuEntry.EntryArea.Contains(InputManager.CurrentMouseState.X,
                                                        InputManager.CurrentMouseState.Y) && !IsDisabled;

            _menuEntry.Position = new Vector2(ThumbnailRenderRectangle.X,
                                              ThumbnailRenderRectangle.Y + ThumbnailRenderRectangle.Height + 5);
            _menuEntry.EntryArea = new Rectangle((int) Position.X, (int) Position.Y, Texture.Width,
                                                 (int)
                                                 (Texture.Height + _menuEntry.Font.MeasureString(_menuEntry.Text).Y +
                                                  TextOffset.Y));
            _menuEntry.Update(screen, gameTime);
        }

        public void Draw(MenuScreen screen, GameTime gameTime)
        {
            SpriteBatch spriteBatch = screen.ScreenManager.SpriteBatch;

            if (Texture != null)
            {
                spriteBatch.Draw(Texture, ThumbnailRenderRectangle, IsDisabled ? _disabledTintColor : Color.White);
            }

            if (Border != null)
            {
                spriteBatch.Draw(Border, new Rectangle(_thumbnailRenderRectangle.X - 4, _thumbnailRenderRectangle.Y - 4, _thumbnailRenderRectangle.Width + 8, _thumbnailRenderRectangle.Height + 8), null, Color.White);
            }


            _menuEntry.Draw(screen, gameTime);
        }

        public bool IsClicked(MenuScreen host)
        {
            return InputManager.IsButtonClicked(ThumbnailRenderRectangle) || _menuEntry.IsClicked(host);
        }

        public static LevelThumbnail FromMetadata(LevelThumbailMetadata metadata, ContentManager content)
        {
            var lt = new LevelThumbnail();
            lt.Text = metadata.LevelName;
            lt.LevelName = metadata.LevelName;
            lt.Texture = content.Load<Texture2D>(metadata.LevelPreviewImage);
            return lt;
        }

        public void OnSelected(object sender, EventArgs e)
        {
            if (Selected != null)
            {
                Selected(sender, e);
            }
        }

    }
}
