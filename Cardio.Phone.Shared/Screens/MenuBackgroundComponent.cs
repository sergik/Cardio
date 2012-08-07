using System;
using System.Collections.Generic;
using Cardio.Phone.Shared.Backgrounds;
using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Screens;
using Cardio.Phone.Shared.Backgrounds;
using Cardio.Phone.Shared.Components;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Screens
{
    public sealed class MenuBackgroundComponent: DrawableGameComponent
    {
        private readonly ScreenManager _screenManager;

        private readonly IDictionary<Type, int> _screenPositions = new Dictionary<Type, int>
        {
            {typeof (MainMenuScreen), 0},
            {typeof (ShopScreen), 2000},
            {typeof (LevelMenu.LevelMenuScreen), 2000}
        };

        private BackgroundController _backgroundController;
        private ICamera2D _camera;

        private Vector2 _target;
        private float _cameraMoveSpeed = 3.5f;
        private float _targetCameraMoveTime = 600f;
        private float _currentCameraMoveTime;

        public MenuBackgroundComponent(Game game, ScreenManager screenManager) : base(game)
        {
            _screenManager = screenManager;
        }

        public override void Initialize()
        {
            LoadingScreen.ScreenLoaded += OnScreenLoaded;

            _camera = new Camera2D();
            _camera.Initialize(_screenManager.Game);

            _backgroundController =
                BackgroundController.FromMetadata(
                    _screenManager.Game.Content.Load<BackgroundMetadata>(@"Backgrounds\Default"),
                    _screenManager.Game.Content);
            _backgroundController.Initialize(_screenManager.Game, _screenManager.SpriteBatch, _camera);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (_currentCameraMoveTime >= _targetCameraMoveTime)
            {
                _currentCameraMoveTime %= _targetCameraMoveTime;
            }

            var diff = _target - _camera.Position;
            var timeDiff = _targetCameraMoveTime - _currentCameraMoveTime;
            var delta = diff/timeDiff*(float)gameTime.ElapsedGameTime.TotalMilliseconds * _cameraMoveSpeed;
            _camera.Position += delta;
            
            _camera.Update(gameTime);
            _backgroundController.Position = (int)_camera.Position.X;
            _backgroundController.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _backgroundController.Draw(gameTime);

            base.Draw(gameTime);
        }

        private void OnScreenLoaded(object sender, ScreenLoadedEventArgs e)
        {
            int position;
            Enabled = _screenPositions.TryGetValue(e.Screen.GetType(), out position);
            if (Enabled)
            {
               _target = new Vector2(position, 0f);
            }
            else
            {
                _target = Vector2.Zero;
            }

            _currentCameraMoveTime = 0f;
        }
    }
}
