using System;
using System.Collections.Generic;
using Cardio.UI.Bodies;
using Cardio.UI.Core;
using Cardio.UI.Core.Alive;
using Cardio.UI.Inventory;
using Cardio.UI.Inventory.Level;
using Cardio.UI.Particles;
using Cardio.UI.Scripts;
using Cardio.UI.Shields;
using Cardio.UI.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cardio.UI.Extensions;

namespace Cardio.UI.Characters.Player
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
                    if (_isShooting)
                    {
                        Gun.StartShooting();
                    }
                    else
                    {
                        Gun.StopShooting();
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

        public override float Health
        {
            get
            {
                return Body.Health;
            }
            set
            {
                Body.Health = value;
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

        public Func<Particle> DamageParticleGenerator { get; set; }

        public Action<Particle> EmitParticle { get; set; }

        public Vector2 GroupPosition { get; set; }

        public Nanobot()
        {
            AssetName = @"Animations\Player\Nanobot";

            ThinkCloud = new ThinkCloud(@"Core\SpeechCloud")
            {
                Target = this,
                TargetOffset = new Vector2(40, 40)
            };
        }

        public override void Initialize(Game game, SpriteBatch spriteBatch, Components.ICamera2D camera)
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

            base.Initialize(game, spriteBatch, camera);
              
            Gun.Initialize(game, spriteBatch, camera);
            Gun.IsShooting = _isShooting;

            Shield.Initialize(game, spriteBatch, camera);
            Shield.IsActivated = _isShieldActivated;

            Shield.DrawRectangle = WorldCollisionRectangle;
            Body.Initialize(game, spriteBatch, camera);
            //AddScript(new CircleMovementScript(30, 200) { Target = this });
        }

        protected override void LoadContent()
        {
            Shield = Shield.FromMetadata(Game.Content.Load<ShieldMetadata>(ShieldAssetName), Game.Content);
            Gun = Weapon.FromMetadata(Game.Content.Load<WeaponMetadata>(GunAssetName), Game.Content);
            Body = Body.FromMetadata(Game.Content.Load<BodyMetadata>(BodyAssetName), Game.Content);
            
            base.LoadContent();
            Content.AddAnimationRule("Bubbles", () => IsMoving);
        }

        public override void Update(GameState gameState, GameTime gameTime)
        {
            Gun.Update(gameTime);
            Shield.Update(gameTime);
            Shield.DrawRectangle = WorldCollisionRectangle;

            Body.Update(gameTime);

            base.Update(gameState, gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Body.Draw(gameTime);
            Gun.Draw(gameTime);
            Shield.Draw(gameTime);
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
            return new List<ParticleEffect>(){ BlowEffect.FromMetadata(Game.Content.Load<BlowEffectMetadata>(@"Effects\Blows\PlayerDamageBlow"), Game.Content) };
        }

        public override void Damage(DamageTakenEventArgs e)
        {
            var realDamage = IsShieldActive ? e.Damage * Shield.Efficiency : e.Damage;
            
            base.Damage(new DamageTakenEventArgs(realDamage, e.DamageTakenPoint));
        }
    }
}
