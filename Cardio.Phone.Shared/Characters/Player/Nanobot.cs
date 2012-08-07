using System;
using System.Collections.Generic;
using Cardio.Phone.Shared.Bodies;
using Cardio.Phone.Shared.Characters.Player.Bodies;
using Cardio.Phone.Shared.Characters.Player.Shields;
using Cardio.Phone.Shared.Characters.Player.Spikes;
using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core.Alive;
using Cardio.Phone.Shared.Particles;
using Cardio.Phone.Shared.Projectiles;
using Cardio.Phone.Shared.Weapons;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core.Alive;
using Cardio.Phone.Shared.Inventory;
using Cardio.Phone.Shared.Inventory.Level;
using Cardio.Phone.Shared.Particles;
using Cardio.Phone.Shared.Scripts;
using Cardio.Phone.Shared.Shields;
using Cardio.Phone.Shared.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cardio.Phone.Shared.Extensions;

namespace Cardio.Phone.Shared.Characters.Player
{
    public class Nanobot: AliveGameObject, IShielded, IParticleEmitter
    {
        public bool IsMoving { get; set; }

        private bool _isShooting;
        public bool IsShooting 
        { 
            get
            {
                return _isShooting;
            }
            set
            {
                _isShooting = value;
                if (Gun != null)
                {
                    if (_isShooting && _isAlive)
                    {
                        Gun.StartShooting();
                        if (FrontWeapon != null)
                        {
                            FrontWeapon.StartShooting();
                        }
                    }
                    else
                    {
                        Gun.StopShooting();
                        if (FrontWeapon != null)
                        {
                            FrontWeapon.StopShooting();
                        }
                    }
                }
            }
        }

        private bool _isShieldActivated;
        public bool IsShieldActive 
        {
            get
            {
                return Shield.IsActivated;
            }
            set
            {
                if (Shield != null)
                {
                    Shield.IsActivated = value;
                }
                _isShieldActivated = value;
            }
        }

        public string ShieldAssetName
        {
            get; set;
        }

        public string BodyAssetName
        {
            get; set;
        }

        public string SpikesAssetName { get; set; }

        public string FrontWeaponAssetName { get; set; }

        public override float Health
        {
            get
            {
                return Body.Health;
            }
            set
            {
                Body.Health = value;
                IsAlive = value > 0;
            }
        }

        public override float MaxHealth
        {
            get
            {
                return Body.MaxHealth;
            }
            set
            {
                Body.MaxHealth = value;
            }
        }

        public string GunAssetName
        {
            get; set;
        }

        private Shield _shield;
        public Shield Shield 
        {
            get { return _shield; }
            set 
            { 
                _shield = value;
                _shield.Target = this;
            }
        }

        private Body _body;
        public Body Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
                _body.Target = this;
            }
        }

        public InventoryItem CarriedItem
        {
            get; set;
        }

        private Weapon  _gun;
        public Weapon Gun
        {
            get
            {
                return _gun;
            }
            set 
            { 
                _gun = value;
                _gun.Target = this;
            }
        }

        public Weapon FrontWeapon { get; set; }

        public Spike Spikes { get; set; }

        public Func<Particle> DamageParticleGenerator { get; set; }

        public Action<Particle> EmitParticle { get; set; }

        public Vector2 GroupPosition { get; set; }

        public Nanobot()
        {
            AssetName = @"Animations\Player\Nanobot";

            //ThinkCloud = new ThinkCloud(@"Core\SpeechCloud")
            //{
            //    Target = this,
            //    TargetOffset = new Vector2(40, 40)
            //};
        }

        public override void Initialize(Game game, SpriteBatch spriteBatch, ICamera2D camera)
        {
            var inventory = game.Services.GetService<InventoryService>();
            if (String.IsNullOrEmpty(ShieldAssetName))
            {
                ShieldAssetName = inventory.Shield;
            }
            if (String.IsNullOrEmpty(GunAssetName))
            {
                GunAssetName = inventory.Gun;
            }
            if (String.IsNullOrEmpty(BodyAssetName))
            {
                BodyAssetName = inventory.Body;
            }
            if (String.IsNullOrEmpty(SpikesAssetName))
            {
                SpikesAssetName = inventory.Spike;
            }
            if(String.IsNullOrEmpty(FrontWeaponAssetName))
            {
                FrontWeaponAssetName = inventory.FrontWeapon;
            }

            base.Initialize(game, spriteBatch, camera);
              
            Gun.Initialize(game, spriteBatch, camera);
            Gun.IsShooting = _isShooting;

            Shield.Initialize(game, spriteBatch, camera);
            Shield.IsActivated = _isShieldActivated;

            Shield.DrawRectangle = WorldCollisionRectangle;
            Body.Initialize(game, spriteBatch, camera);
            if (Spikes != null)
            {
                Spikes.Initialize(game, spriteBatch, camera);
            }
            if (FrontWeapon != null)
            {
                FrontWeapon.Initialize(game, spriteBatch, camera);
            }
        }

        protected override void LoadContent()
        {
            Shield = Shield.FromMetadata(Game.Content.Load<ShieldMetadata>(ShieldAssetName), Game.Content);
            Gun = Weapon.FromMetadata(Game.Content.Load<WeaponMetadata>(GunAssetName), Game.Content);
            Body = Body.FromMetadata(Game.Content.Load<BodyMetadata>(BodyAssetName), Game.Content);

            if(!String.IsNullOrEmpty(SpikesAssetName))
            {
                Spikes = Spike.FromMetadata(Game.Content.Load<SpikeMetadata>(SpikesAssetName), Game.Content);
                Spikes.Target = this;
            }

            if(!String.IsNullOrEmpty(FrontWeaponAssetName))
            {
                FrontWeapon = Weapon.FromMetadata(Game.Content.Load<WeaponMetadata>(FrontWeaponAssetName), Game.Content);
                FrontWeapon.Target = this;
            }

            base.LoadContent();
            Content.AddAnimationRule("Bubbles", () => IsMoving);



        }

        public override void Update(GameState gameState, GameTime gameTime)
        {
            if (!_isAlive)
            {
                return;
            }

            Gun.Update(gameTime);
            Shield.Update(gameTime);
            Shield.DrawRectangle = WorldCollisionRectangle;
            Body.Update(gameTime);
            if (Spikes != null)
            {
                Spikes.Update(gameTime);
            }
            if (FrontWeapon != null)
            {
                FrontWeapon.Update(gameTime);
            }

            base.Update(gameState, gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!_isAlive)
            {
                return;
            }

            if (Spikes != null)
            {
                Spikes.Draw(gameTime);
            }
            base.Draw(gameTime);
            Body.Draw(gameTime);
            Gun.Draw(gameTime);
            Shield.Draw(gameTime);
            if (FrontWeapon != null)
            {
                FrontWeapon.Draw(gameTime);
            }
        }

        public void ActivateShield()
        {
            if (IsShieldActive)
            {
                return;
            }
            Shield.Activate();
           
        }

        public void DeactivateShields()
        {
            if (!IsShieldActive)
            {
                return;
            }

            Shield.Deactivate();
        }

        public override IList<ParticleEffect> ToDamageAnimation()
        {
            var cache = Game.Services.GetService<GameCache>();

            var effectBox = cache.GetWithRemoveBlowByType(Particles.BlowType.PlayerDamageBlow);

            if (effectBox != null)
            {
                effectBox.BlowEffect.WorldPosition = WorldPosition;
                return new List<ParticleEffect> {  effectBox.BlowEffect };
            }

            return new List<ParticleEffect> { BlowEffect.FromMetadata(Game.Content.Load<BlowEffectMetadata>(@"Effects\Blows\PlayerDamageBlow"), Game.Content) };
        }

        public override void Damage(DamageTakenEventArgs e)
        {
            var realDamage = IsShieldActive ? e.Damage * Shield.Efficiency : e.Damage;
            
            base.Damage(new DamageTakenEventArgs(realDamage, e.DamageTakenPoint));
        }

        public override IList<Particles.ParticleEffect> ToDeathAnimation()
        {
            var cache = Game.Services.GetService<GameCache>();

            var effectBox = cache.GetWithRemoveBlowByType(Particles.BlowType.PlayerDeathBlow);

            if (effectBox != null)
            {
                effectBox.BlowEffect.WorldPosition = WorldPosition;
                return new List<ParticleEffect> { effectBox.BlowEffect };
            }

            return new List<ParticleEffect> { BlowEffect.FromMetadata(Game.Content.Load<BlowEffectMetadata>(@"Effects\Blows\PlayerDeathBlow"), Game.Content) };
        }
    }
}
