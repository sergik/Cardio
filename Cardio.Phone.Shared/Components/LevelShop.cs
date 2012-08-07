using System;
using System.Collections.Generic;
using System.Linq;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Input;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Inventory.Level;
using Cardio.Phone.Shared.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Components
{
    public class LevelShop
    {
        private const int OffsetLeft = 12;
        private int _money;
        private SpriteBatch _spriteBatch;

        private GrowingMenuEntry _selectedTab;

        public Game Game { get; private set; }

        public Texture2D Biomaterial { get; set; }

        public int? SelectedTabIndex { get; set; }

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

        public List<SpikeShopItem> Spikes { get; set; }

        public List<LevelShopItem> Medcits { get; set; }

        public List<LevelShopItem> Tablets { get; set; }

        public List<LevelShopItem> FrontWeapons { get; set; }

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
            Medcits = new List<LevelShopItem>();
            Tablets = new List<LevelShopItem>();
            Spikes = new List<SpikeShopItem>();
            FrontWeapons = new List<LevelShopItem>();
        }

        private void CreateTabs()
        {
            var inventory = Game.Services.GetService<InventoryService>();
            var guns = new GrowingMenuEntry("GUNS");
            guns.Position = new Vector2(Position.X - 20, Position.Y + 60);
            EventHandler<EventArgs> gunsSelected = (s, e) =>
            {
                ItemsSetToDisplay.Clear();
                foreach (var gun in Guns)
                {
                    ItemsSetToDisplay.Add(gun);
                }
                InitializeItemsPositions();
                SetAvailibility(ItemsSetToDisplay, inventory.Gun);
            };
            guns.Selected += gunsSelected;
            Tabs.Add(guns);
            gunsSelected(null, null);

            var shields = new GrowingMenuEntry("SHIELDS");
            shields.Position = new Vector2(Position.X - 20, Position.Y + 100);
            shields.Selected += (s, e) =>
                                    {
                                        ItemsSetToDisplay.Clear();
                                        foreach (var shield in Shields)
                                        {
                                            ItemsSetToDisplay.Add(shield);
                                        }
                                        InitializeItemsPositions();
                                        SetAvailibility(ItemsSetToDisplay, inventory.Shield);
                                    };
            Tabs.Add(shields);

            var bodies = new GrowingMenuEntry("BODIES");
            bodies.Position = new Vector2(Position.X - 20, Position.Y + 140);
            bodies.Selected += (s, e) =>
                                    {
                                        ItemsSetToDisplay.Clear();
                                        foreach (var body in Bodies)
                                        {
                                            ItemsSetToDisplay.Add(body);
                                        }
                                        InitializeItemsPositions();
                                        SetAvailibility(ItemsSetToDisplay, inventory.Body);
                                    };
            Tabs.Add(bodies);

            var medcits = new GrowingMenuEntry("MEDCITS");
            medcits.Position = new Vector2(Position.X - 20, Position.Y + 220);
            medcits.Selected += (s, e) =>
                                    {
                                        ItemsSetToDisplay.Clear();
                                        foreach (var medcit in Medcits)
                                        {
                                            ItemsSetToDisplay.Add(medcit);
                                        }
                                        InitializeItemsPositions();
                                        SetAvailibility(ItemsSetToDisplay, inventory.Medcit);
                                    };
            Tabs.Add(medcits);

            var tablets = new GrowingMenuEntry("TABLETS");
            tablets.Position = new Vector2(Position.X - 20, Position.Y + 260);
            tablets.Selected += (s, e) =>
            {
                ItemsSetToDisplay.Clear();
                foreach (var tablet in Tablets)
                {
                    ItemsSetToDisplay.Add(tablet);
                }
                InitializeItemsPositions();
                SetAvailibility(ItemsSetToDisplay, inventory.Tablet);
            };
            Tabs.Add(tablets);

            var spikes = new GrowingMenuEntry("SPIKES");
            spikes.Position = new Vector2(Position.X - 20, Position.Y + 300);
            spikes.Selected += (s, e) =>
                                   {
                                       ItemsSetToDisplay.Clear();
                                       foreach (var spike in Spikes)
                                       {
                                           ItemsSetToDisplay.Add(spike);
                                       }
                                       InitializeItemsPositions();
                                       SetAvailibility(ItemsSetToDisplay, inventory.Spike);
                                   };
            Tabs.Add(spikes);

            var plazma = new GrowingMenuEntry("PLAZMA");
            plazma.Position = new Vector2(Position.X - 20, Position.Y + 180);
            plazma.Selected += (s, e) =>
            {
                ItemsSetToDisplay.Clear();
                foreach (var pazma in FrontWeapons)
                {
                    ItemsSetToDisplay.Add(pazma);
                }
                InitializeItemsPositions();
                SetAvailibility(ItemsSetToDisplay, inventory.FrontWeapon);
            };
            Tabs.Add(plazma);
            
            _selectedTab = Tabs[0];
        }

        private void SetAvailibility(List<LevelShopItem> items, string installedUpgrade)
        {
            int installedIndex = items.IndexOf(items.FirstOrDefault(i => i.UpgradeName == installedUpgrade));
            for (int i = 0; i <= installedIndex; i++)
            {
                items[i].Installed = true;
            }
            for(int i = installedIndex + 2; i < items.Count; i++)
            {
                items[i].Desabled = true;
            }
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

            foreach (var frontWeapon in FrontWeapons)
            {
                frontWeapon.Initialize(game);
            }

            CreateTabs();
            
            foreach (var tab in Tabs)
            {
                tab.Initialize(game);
            }

            if (SelectedTabIndex.HasValue)
            {
                Tabs[SelectedTabIndex.Value].OnSelectEntry();
                _selectedTab = Tabs[SelectedTabIndex.Value];
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
                ItemsSetToDisplay[i].Position = new Vector2(Position.X, Position.Y - 60 + i * 105);
                ItemsSetToDisplay[i].Initialize(Game);
            }
        }

        public void Update(GameTime gameTime)
        {
            _money = Game.Services.GetService<GameState>().Inventory.Money;
            int selectedTabIndex = Tabs.IndexOf(_selectedTab);

            for (int index = 0; index < ItemsSetToDisplay.Count; index++)
            {
                var levelShopItem = ItemsSetToDisplay[index];
                levelShopItem.ActiveTabIndex = selectedTabIndex;
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
            _spriteBatch.Draw(Biomaterial, Position - new Vector2(0, 50), Color.White);
            _spriteBatch.DrawString(Fonts.Default, string.Format("x{0}", _money), Position + new Vector2(OffsetLeft + 3, -50), Color.White);
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

            for (int i = 0; i < metadata.Medcits.Count; i++)
            {
                shop.Medcits.Add(MedcitShopItem.FromMetadata(metadata.Medcits[i]));
            }

            for (int i = 0; i < metadata.Tablets.Count; i++)
            {
                shop.Tablets.Add(TabletShopItem.FromMetadata(metadata.Tablets[i]));
            }

            for (int i = 0; i < metadata.Spikes.Count; i++)
            {
                shop.Spikes.Add(SpikeShopItem.FromMetadata(metadata.Spikes[i]));
            }

            for (int i = 0; i < metadata.FrontWeapons.Count; i++)
            {
                shop.FrontWeapons.Add(FrontWeaponShopItem.FromMetadata(metadata.FrontWeapons[i]));
            }

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
