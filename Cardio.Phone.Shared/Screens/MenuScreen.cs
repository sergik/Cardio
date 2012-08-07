using System;
using System.Collections.Generic;
using Cardio.Phone.Shared.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cardio.Phone.Shared.Screens
{
    public abstract class MenuScreen: GameScreen
    {
        private const int MenuEntryPadding = 10;

        private int _selectedMenuEntry;

        protected IList<MenuEntry> MenuEntries
        {
            get; private set;
        }

        protected MenuEntry SelectedEntry { get; set; }

        protected MenuScreen()
        {
            MenuEntries = new List<MenuEntry>();
        }

        public override void LoadContent()
        {
            foreach (var menu in MenuEntries)
            {
                menu.Initialize(ScreenManager.Game);
            }

            base.LoadContent();
        }

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput()
        {
            if (InputManager.IsKeyTriggered(Keys.Up))
            {
                _selectedMenuEntry--;

                if (_selectedMenuEntry < 0)
                {
                    _selectedMenuEntry = MenuEntries.Count - 1;
                }
            }

            if (InputManager.IsKeyTriggered(Keys.Down))
            {
                _selectedMenuEntry++;

                if (_selectedMenuEntry >= MenuEntries.Count)
                {
                    _selectedMenuEntry = 0;
                }
            }

            if (InputManager.IsMouseMoved())
            {
                for (int i = 0; i < MenuEntries.Count; i++)
                {
                    if (MenuEntries[i].EntryArea.Contains(InputManager.CurrentMouseState.X, InputManager.CurrentMouseState.Y))
                    {
                        _selectedMenuEntry = i;
                        break;
                    }
                }
            }

            if (InputManager.IsKeyTriggered(Keys.Enter))
            {
                MenuEntries[_selectedMenuEntry].OnSelectEntry();
            }

            foreach (MenuEntry entry in MenuEntries)
            {
                //var entryRecg = new Rectangle((int) entry.Position.X, (int) entry.Position.Y,
                //    entry.Texture.Width, entry.Texture.Height);

                if (entry.IsClicked(this))
                {
                    entry.OnSelectEntry();
                    return;
                }
            }

            //if (InputManager.IsActionTriggered(InputManager.Action.Back) ||
            //    InputManager.IsActionTriggered(InputManager.Action.ExitGame))
            //{
            //    OnCancel();
            //}
            //else if (selectedEntry != oldSelectedEntry)
            //{
            //    AudioManager.PlayCue("MenuMove");
            //}

        }

        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel()
        {
            ExitScreen();
        }

        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }

        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                var isSelected = IsActive && (i == _selectedMenuEntry);
                MenuEntries[i].Update(this, gameTime);
            }
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();


            for (int i = 0; i < MenuEntries.Count; i++ )
            {
                var isSelected = IsActive && (i == _selectedMenuEntry);
                MenuEntries[i].Draw(this, gameTime);
            }

            spriteBatch.End();
        }
    }
}
