using System;
using System.Collections.Generic;
using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Animations;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Projectiles;
using Cardio.Phone.Shared.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Cardio.Phone.Shared.Weapons
{
    public class Weapon : AttachableGameObject
    {
        private readonly List<BaseBullet> _activeBullets;
        private int _atackDelay;
        private Game _game;

        public bool IsShooting
        {
            get; set;
        }

        public bool NeedGenerateBullets { get; set; }

        public String BulletAssetName { get; set; }

        public Weapon(String assetName) : this()
        {
            AssetName = assetName;
        }

        public Weapon()
        {
            _activeBullets = new List<BaseBullet>();
        }

        public int Damage
        {
            get; set;
        }

        public int BulletSpeed { get; set; }

        public int AtackRate { get; set; }

        public Vector2 BulletGenerationPosition { get; set; }

        public virtual void StartShooting()
        {
            IsShooting = true;
            SoundManager.BotAttack.Play();
        }

        public void StopSound()
        {
            SoundManager.BotAttack.Stop();
        }

        public virtual void StopShooting()
        {
            IsShooting = false;
            SoundManager.BotAttack.Stop();
        }

        public override void Initialize(Game game, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, ICamera2D camera)
        {
            base.Initialize(game, spriteBatch, camera);

            Content.AddAnimationRule("Flash", () => IsShooting);
            Content.AddAnimationRule("botHandDeactivated", () => !IsShooting);
            Content.AddAnimationRule("botHandActivated", () => IsShooting);
            _atackDelay = AtackRate;
            _game = game;
            NeedGenerateBullets = true;
        }

        public static Weapon FromMetadata(WeaponMetadata metadata, ContentManager content)
        {
            var weapon = WeaponsFactory.GetWeapon(metadata.Name);
            weapon.Damage = metadata.Damage;
            weapon.TargetOffset = metadata.TargetOffset;
            weapon.AssetName = metadata.AssetName;
            if (!String.IsNullOrEmpty(metadata.AssetName))
            {
                weapon.Content = AnimatedObject.FromMetadata(content.Load<AnimatedObjectMetadata>(metadata.AssetName), content);
            }
            weapon.AtackRate = metadata.AtackRate;
            weapon.BulletAssetName = metadata.BulletAssetName;
            weapon.BulletSpeed = metadata.BulletSpeed;
            weapon.BulletGenerationPosition = metadata.BulletGenerationPosition;

            return weapon;
        }

        public virtual IEnumerable<BaseBullet> GenerateBullets(Game game)
        {
            var bullets = new List<BaseBullet>();
            var player = game.Services.GetService<GameState>().Player;
            var bullet = new NanobotBullet
            {
                Content = AnimatedObject.FromMetadata(game.Content.Load<AnimatedObjectMetadata>(BulletAssetName), game.Content),
                Speed = BulletSpeed,
                HitDamage = Damage,
                WorldPosition = player.WorldPosition + player.Nanobot.GroupPosition + BulletGenerationPosition
            };
            bullet.Initialize(game, SpriteBatch, game.Services.GetService<GameState>().Camera);
            bullets.Add(bullet);
            return bullets;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ProcessShoot(gameTime, IsShooting, _game);
            var state = _game.Services.GetService(typeof (GameState)) as GameState;
            UpdateBullets(gameTime, state);
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < _activeBullets.Count; i++)
            {
                _activeBullets[i].Draw(gameTime);
            }
            base.Draw(gameTime);
        }

        private void ProcessShoot(GameTime gameTime, bool isShooting, Game game)
        {
            if (!isShooting)
            {
                return;
            }
            
            if (_atackDelay > 0)
            {
                _atackDelay -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
            {
                if (NeedGenerateBullets)
                {
                    _atackDelay = AtackRate;
                    var bullets = GenerateBullets(game);
                    _activeBullets.AddRange(bullets);
                }
            }
        }

        public void RemoveBullet(BaseBullet bullet)
        {
            _activeBullets.Remove(bullet);
        }

        private void UpdateBullets(GameTime gameTime, GameState state)
        {
            for (int i = 0; i < _activeBullets.Count; i++)
            {
                var bullet = _activeBullets[i];
                bullet.Update(state, gameTime);
                if(bullet.WorldPosition.X - state.Player.WorldPosition.X > 1000)
                {
                    _activeBullets.Remove(bullet);
                }
            }
        }
    }
}
