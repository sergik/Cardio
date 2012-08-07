using System;
using System.Collections.Generic;
using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Particles;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core.Alive;
using Cardio.Phone.Shared.Animations;
using Cardio.Phone.Shared.Characters;
using Cardio.Phone.Shared.Exceptions;
using Cardio.Phone.Shared.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Cardio.Phone.Shared.Particles;

namespace Cardio.Phone.Shared.Core.Alive
{
    public abstract class AliveGameObject: DrawableGameObject, IAlive, IIntelligent
    {
        private float _health;

        public virtual float Health
        {
            get { return _health; }
            set
            {
                if (_health == value)
                {
                    return;
                }

                _health = value;
                IsAlive = value > 0;
            }
        }

        public virtual float MaxHealth { get; set; }

        protected bool _isAlive = true;
        public bool IsAlive
        {
            get { return _isAlive; }
            protected set
            {
                if (_isAlive != value)
                {
                    _isAlive = value;
                    if (!_isAlive)
                    {
                        OnDie(new DieEventArgs(WorldPosition));
                    }
                }
            }
        }

        //private ThinkCloud _thinkCloud;
        //public ThinkCloud ThinkCloud
        //{
        //    get
        //    {
        //        return _thinkCloud;
        //    }
        //    set
        //    {
        //        if (IsInitialized)
        //        {
        //            throw new UnableToModifyStateAfterInitializationException();
        //        }

        //        _thinkCloud = value;
        //    }
        //}

        public event DamageTakenEventHandler DamageTaken;
        public event DieEventHandler Die;

        public BlowEffectMetadata DeathEffectMetadata { get; set; }
        public BlowEffectMetadata DamageEffectMetadata { get; set; }

        protected AliveGameObject()
        {
            _health = 100;
        }

        //public void Confuse()
        //{
        //    ThinkCloud.Animation.SetFrame(0);
        //    ThinkCloud.IsVisible = true;
        //}

        //public override void Initialize(Game game, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, ICamera2D camera)
        //{
        //    if (ThinkCloud != null)
        //    {
        //        ThinkCloud.Initialize(game, spriteBatch, camera);
        //    }
            
        //    base.Initialize(game, spriteBatch, camera);
        //}

        //public override void Update(GameState gameState, GameTime gameTime)
        //{
        //    UpdateThinkCloud(gameTime);

        //    base.Update(gameState, gameTime);
        //}

        //private void UpdateThinkCloud(GameTime gameTime)
        //{
        //    if (ThinkCloud == null)
        //    {
        //        return;
        //    }

        //    if (ThinkCloud.Animation.HasFinishedPlay)
        //    {
        //        ThinkCloud.IsVisible = false;
        //    }

        //    ThinkCloud.Update(gameTime);
        //}

        //public override void Draw(GameTime gameTime)
        //{
        //    if (ThinkCloud != null)
        //    {
        //        ThinkCloud.Draw(gameTime);
        //    }
            
        //    base.Draw(gameTime);
        //}

        public virtual void Damage(DamageTakenEventArgs e)
        {
            Health -= e.Damage;

            var handler = DamageTaken;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public virtual void OnDie(DieEventArgs e)
        {
            var handler = Die;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Heal(float health)
        {
            Health = MathHelper.Min(MaxHealth, Health + health);
        }

        public virtual IList<ParticleEffect> ToDeathAnimation()
        {
            var cache = (GameCache)Game.Services.GetService(typeof(GameCache));
            BlowEffect effect;

            var effectBox = cache.GetWithRemoveBlowByType(DeathEffectMetadata.Type);

            if (effectBox != null)
            {
                effectBox.BlowEffect.WorldPosition = WorldPosition;
                return new List<ParticleEffect> { effectBox.BlowEffect };
            }
                
            effect = BlowEffect.FromMetadata(DeathEffectMetadata, Game.Content);

            effect.WorldPosition = WorldPosition;

            return new List<ParticleEffect> { effect };
        }

        public virtual IList<ParticleEffect> ToDamageAnimation()
        {
            var cache = (GameCache)Game.Services.GetService(typeof(GameCache));
            BlowEffect effect;

            var effectBox = cache.GetWithRemoveBlowByType(DamageEffectMetadata.Type);

            if (effectBox != null)
            {
                effectBox.BlowEffect.WorldPosition = WorldPosition;
                return new List<ParticleEffect> { effectBox.BlowEffect };
            }

            effect = BlowEffect.FromMetadata(DamageEffectMetadata, Game.Content);

            effect.WorldPosition = WorldPosition;

            return new List<ParticleEffect> { effect };
        }

        public static void FillWithMetadata(AliveGameObject target, AliveGameObjectMetadata source, ContentManager content)
        {
            target.Content = AnimatedObject.FromMetadata(content.Load<AnimatedObjectMetadata>(source.ContentPath), content);
            target.MaxHealth = source.MaxHealth;
            target.Health = source.MaxHealth;
            target.Size = (source.Size.X <= 0.001 || source.Size.Y <= 0.001) ? (Vector2?) null : source.Size;
            //target.ThinkCloud = String.IsNullOrWhiteSpace(source.ThinkCloudPath)
            //    ? null
            //    : new ThinkCloud(source.ThinkCloudPath) { Target = target };
            target.Sound = new Sound(source.SoundPath);

            if (!String.IsNullOrEmpty(source.DeathAnimation))
            {
                target.DeathEffectMetadata = content.Load<BlowEffectMetadata>(source.DeathAnimation);
            }
            if (!String.IsNullOrEmpty(source.DamageAnimation))
            {
                target.DamageEffectMetadata = content.Load<BlowEffectMetadata>(source.DamageAnimation);
            }

        }
    }
}