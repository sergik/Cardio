using System.Collections.Generic;
using System.Linq;
using Cardio.UI.Core;
using Cardio.UI.Input;
using Cardio.UI.Screens;
using Microsoft.Xna.Framework;
using Cardio.UI.Extensions;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Components
{
    public class LevelShop
    {
        private int _money;
        private SpriteBatch _spriteBatch;

        private GrowingMenuEntry _selectedTab;

        public Game Game
        {
            get; private set;
        }

        public Texture2D Biomaterial
        {
            get;
            set;
        }

        public List<GrowingMenuEntry> Tabs
        {
            get; set;
        }

        public List<GunShopItem> Guns
        {
            get; set;
        }

        public List<ShieldShopItem> Shields
        {
            get; set;
        }

        public List<BodyShopItem> Bodies
        {
            get; set;
        }

        public List<LevelShopItem> ItemsSetToDisplay
        {
            get; set;
        }

        public Vector2 Position
        {
            get; set;
        }

        public LevelShop()
        {
            ItemsSetToDisplay = new List<LevelShopItem>();
            Tabs = new List<GrowingMenuEntry>();
            Guns = new List<GunShopItem>();
            Shields = new List<ShieldShopItem>();
            Bodies = new List<BodyShopItem>();
        }

        private void CreateTabs()
        {
            var guns = new GrowingMenuEntry("GUNS");
            guns.Position = new Vector2(Position.X, Position.Y + 100);
            guns.Selected += (s, e) =>
                                 {
                                     ItemsSetToDisplay.Clear();
                                     ItemsSetToDisplay.AddRange(Guns);
                                     InitializeItemsPositions();
                                 };
            Tabs.Add(guns);

            var shields = new GrowingMenuEntry("SHIELDS");
            shields.Position = new Vector2(Position.X, Position.Y + 140);
            shields.Selected += (s, e) =>
                                    {
                                        ItemsSetToDisplay.Clear();
                                        ItemsSetToDisplay.AddRange(Shields);
                                        InitializeItemsPositions();
                                    };
            Tabs.Add(shields);

            var bodies = new GrowingMenuEntry("BODIES");
            bodies.Position = new Vector2(Position.X, Position.Y + 180);
            bodies.Selected += (s, e) =>
                                    {
                                        ItemsSetToDisplay.Clear();
                                        ItemsSetToDisplay.AddRange(Bodies);
                                        InitializeItemsPositions();
                                    };
            Tabs.Add(bodies);
            
            _selectedTab = Tabs[0];
        }

        public void Initialize(Game game)
        {
            Game = game;
            _spriteBatch = game.Services.GetService<SpriteBatch>();

            foreach (var gun in Guns)
            {
                gun.Initialize(game);
            }
            foreach (var shield in Shields)
            {
                shield.Initialize(game);
            }
            foreach (var body in Bodies)
            {
                body.Initialize(game);
            }

            CreateTabs();
            InitializeItemsPositions();
            
            foreach (var tab in Tabs)
            {
                tab.Initialize(game);
            }

            LoadContent();
        }

        public void LoadContent()
        {
            Biomaterial = Game.Content.Load<Texture2D>(@"Textures\Inventory\BiomaterialInventory");
        }

        private void InitializeItemsPositions()
        {
            for (int i = 0; i < ItemsSetToDisplay.Count; i++)
            {
                ItemsSetToDisplay[i].Position = new Vector2(Position.X, Position.Y - 20 + i * 150);
                ItemsSetToDisplay[i].Initialize(Game);
            }
        }

        public void Update(GameTime gameTime)
        {
            _money = Game.Services.GetService<GameState>().Inventory.Money;

            for (int index = 0; index < ItemsSetToDisplay.Count; index++)
            {
                var levelShopItem = ItemsSetToDisplay[index];
                levelShopItem.Update(gameTime);
            }

            for (int index = 0; index < Tabs.Count; index++)
            {
                var tab = Tabs[index];
                tab.Update(null, gameTime);
                tab.IsSelected = tab.IsSelected || tab == _selectedTab;
            }
        }

        public void Draw(GameTime gameTime)
        {
            for (int index = 0; index < ItemsSetToDisplay.Count; index++)
            {
                var levelShopItem = ItemsSetToDisplay[index];
                levelShopItem.Draw(gameTime);
            }

            for (int index = 0; index < Tabs.Count; index++)
            {
                var tab = Tabs[index];
                tab.Draw(null, gameTime);
            }

            DrawMoney();
        }

        private void DrawMoney()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(Biomaterial, Position, Color.White);
            _spriteBatch.DrawString(Fonts.Default, string.Format("x{0}", _money), Position + new Vector2(30, 25), Color.White);
            _spriteBatch.End();
        }

        public static LevelShop FromMetadata(LevelShopMetadata metadata)
        {
            var shop = new LevelShop();
            
            foreach(var gun in metadata.Guns)
            {
                shop.Guns.Add(GunShopItem.FromMetadata(gun));
            }

            foreach (var shield in metadata.Shields)
            {
                shop.Shields.Add(ShieldShopItem.FromMetadata(shield));
            }

            foreach (var body in metadata.Bodies)
            {
                shop.Bodies.Add(BodyShopItem.FromMetadata(body));
            }

            shop.ItemsSetToDisplay.AddRange(shop.Guns);

            return shop;
        }

        public void HandleInput()
        {
            if (InputManager.IsMouseButtonTriggered(x => x.LeftButton))
             {
                 var tab = Tabs.FirstOrDefault(
                     t => t.EntryArea.Contains(InputManager.CurrentMouseState.X, InputManager.CurrentMouseState.Y));
                 if (tab != null)
                 {
                     tab.OnSelectEntry();
                     _selectedTab = tab;
                 }
             }

            for (int index = 0; index < ItemsSetToDisplay.Count; index++)
            {
                var item = ItemsSetToDisplay[index];
                item.HandleInput();
            }
        }
    }
}
