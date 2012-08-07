using System;
using System.Collections.Generic;
using System.Linq;
using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Backgrounds;
using Cardio.Phone.Shared.Backgrounds.Parallax;
using Cardio.Phone.Shared.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Backgrounds
{
    public class BackgroundController
    {
        private bool _isInitialized;

        private int _position;

        public int Position 
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                for (int index = 0; index < _layers.Count; index++)
                {
                    _layers[index].Position = _position;
                }
            }
        }

        private List<ScrollingBackground> _layers = new List<ScrollingBackground>();

        public List<ScrollingBackground> Layers
        {
            get
            {
                return _layers;
            }
            set
            {
                if (_isInitialized)
                {
                    throw new InvalidOperationException("Unable to change the state after Initialize");
                }

                _layers = value;
            }
        }

        private BackgroundController(IEnumerable<ScrollingBackground> layers)
        {
            _layers = new List<ScrollingBackground>(layers);
        }

        public static BackgroundController FromMetadata(BackgroundMetadata metadata, ContentManager content)
        {
            var controller =
                new BackgroundController(
                    metadata.Layers.Select<ParallaxLayerMetadata, ScrollingBackground>(
                        x => ParallaxLayer.FromMetadata(x, content)));
            return controller;
        }

        public void Initialize(Game game, SpriteBatch spriteBatch, ICamera2D camera)
        {
            foreach (ScrollingBackground t in _layers)
            {
                t.Initialize(game, spriteBatch, camera);
            }

            _isInitialized = true;
        }

        public void Update(GameTime gameTime)
        {
            for (int index = 0; index < _layers.Count; index++)
            {
                _layers[index].Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            for (int index = 0; index < _layers.Count; index++)
            {
                _layers[index].Draw(gameTime);
            }
        }
    }
}
