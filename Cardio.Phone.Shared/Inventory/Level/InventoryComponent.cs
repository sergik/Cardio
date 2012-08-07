using System.Collections.Generic;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Inventory.Level
{
    public class InventoryComponent : DrawableGameComponent
    {
        private readonly Color _disabledTintColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);
        
        /// <summary>
        /// Local inventory service logic handler.
        /// </summary>
        private InventoryService _inventory;

        /// <summary>
        /// Size of one inventory item on the screen
        /// </summary>
        public Point InventoryItemRenderSize { get; set; }

        /// <summary>
        /// The distance between two adjacent items on the screen
        /// </summary>
        public int InventoryItemRenderMargin { get; set; }

        public float ItemCountFontSize { get; set; }

        /// <summary>
        /// Render position of the first inventory item on the screen.
        /// Positions of other items are calculated based on this value and other render properties.
        /// </summary>
        public Point RenderPosition { get; set; }

        private Rectangle[] _renderingRectangles;

        /// <summary>
        /// SpriteBatch used to render inventory items.
        /// </summary>
        private SpriteBatch _spriteBatch;

        private SpriteFont _itemCountFont;
        private GameState _gameState;

        public InventoryComponent(Game game)
            : base(game)
        {
            InventoryItemRenderSize = new Point(64, 64);
            ItemCountFontSize = 20;
            InventoryItemRenderMargin = 10;
        }

        public override void Initialize()
        {
            _inventory = Game.Services.GetService<InventoryService>();
            _spriteBatch = Game.Services.GetService<SpriteBatch>();
            _gameState = Game.Services.GetService<GameState>();

            RenderPosition = new Point(Game.GraphicsDevice.Viewport.Width -
                _inventory.Slots.Count * (InventoryItemRenderSize.X + InventoryItemRenderMargin),
                InventoryItemRenderMargin);

            var renderingRectangles = new List<Rectangle>();
            for (int i = 0; i < _inventory.Slots.Count; i++ )
            {
                var rect = new Rectangle(Game.GraphicsDevice.Viewport.Width -
                    (_inventory.Slots.Count- i) * (InventoryItemRenderSize.X + InventoryItemRenderMargin),
                    InventoryItemRenderMargin, InventoryItemRenderSize.X, InventoryItemRenderSize.Y);
                renderingRectangles.Add(rect);
                if (_inventory.Slots[i] != null)
                {
                    _inventory.Slots[i].Initialize(Game, rect);
                }
            }

            _renderingRectangles = renderingRectangles.ToArray();

            _itemCountFont = Game.Content.Load<SpriteFont>(@"Fonts\MenuFont");

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            for (int index = 0; index < _renderingRectangles.Length; index++)
            {
                if (_inventory.Slots[index] != null)
                {
                    if (InputManager.IsTouched(_renderingRectangles[index]) && _inventory.Slots[index].Base.IsEnabled)
                    {
                        _inventory.UseItemFromSlot(index, _gameState);
                        _gameState.HandledMouseOnThisUpdate = true;
                    }
                }
            }

            for (int i = 0; i < _inventory.Slots.Count; i++)
            {
                if (_inventory.Slots[i] != null)
                {
                    _inventory.Slots[i].Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            for (int i = 0; i < _inventory.Slots.Count; i++)
            {
                var slot = _inventory.Slots[i];
                if (slot != null && (slot.Count > 0 || slot.Base.IsReuseble))
                {
                    var tintColor = slot.Base.IsEnabled ? Color.White : _disabledTintColor;
                    _spriteBatch.Draw(slot.Base.InventoryTexture, _renderingRectangles[i], tintColor);

                    if (!slot.Base.IsReuseble)
                    {
                        if (!string.IsNullOrEmpty(slot.CountText))
                        {
                            var textSize = _itemCountFont.MeasureString(slot.CountText);
                            float scale = ItemCountFontSize/textSize.Y;
                            var textCenterPoint = new Vector2(_renderingRectangles[i].Center.X,
                                                              _renderingRectangles[i].Bottom + ItemCountFontSize/2);

                            Text.Fonts.DrawCenteredText(_spriteBatch, _itemCountFont, slot.CountText,
                                                        textCenterPoint, scale,
                                                        Color.White);
                        }
                    }
                    else
                    {
                        slot.Draw(gameTime);
                    }
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
