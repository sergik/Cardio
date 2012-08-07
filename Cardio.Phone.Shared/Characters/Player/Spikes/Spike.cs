using Cardio.Phone.Shared.Animations;
using Cardio.Phone.Shared.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Cardio.Phone.Shared.Characters.Player.Spikes
{
    public class Spike : AttachableGameObject
    {
        private bool _activated;
        private int _activeTime;
        private int _usedTime;

        public bool Activated
        {
            get { return _activated; }
        }

        public bool ActivatedInShop { get; set; }

        public int ActiveTime
        {
            get { return _activeTime; }
        }

        public static Spike FromMetadata(SpikeMetadata metadata, ContentManager content)
         {
             var spike = new Spike();
             spike.TargetOffset = metadata.Position;
             spike.Content = AnimatedObject.FromMetadata(content.Load<AnimatedObjectMetadata>(metadata.AssetName), content);
             spike._activeTime = metadata.ActiveTime;

             return spike;
         }

        public void Activate()
        {
            _usedTime = 0;
            _activated = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(!Activated)
            {
                return;
            }
            if(_usedTime >= _activeTime)
            {
                _usedTime = 0;
                _activated = false;
                return;
            }
            _usedTime += gameTime.ElapsedGameTime.Milliseconds;
        }

        public override void Draw(GameTime gameTime)
        {
            if(_activated || ActivatedInShop)
            {
                 base.Draw(gameTime);
            }
        }
    }
}