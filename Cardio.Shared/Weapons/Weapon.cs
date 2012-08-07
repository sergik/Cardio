using System;
using Cardio.UI.Animations;
using Cardio.UI.Core;
using Cardio.UI.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Cardio.UI.Weapons
{
    public class Weapon : AttachableGameObject
    {
        public bool IsShooting
        {
            get; set;
        }

        public Weapon(string assetName)
        {
            AssetName = assetName;
        }

        public int Damage
        {
            get; set;
        }

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

        public override void Initialize(Game game, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Components.ICamera2D camera)
        {
            base.Initialize(game, spriteBatch, camera);

            Content.AddAnimationRule("Flash", () => IsShooting);
            Content.AddAnimationRule("botHandDeactivated", () => !IsShooting);
            Content.AddAnimationRule("botHandActivated", () => IsShooting);
        }

        public static Weapon FromMetadata(WeaponMetadata metadata, ContentManager content)
        {
            var weapon = new Weapon(metadata.AssetName);
            weapon.Damage = metadata.Damage;
            weapon.TargetOffset = metadata.TargetOffset;
            if (!String.IsNullOrEmpty(metadata.AssetName))
            {
                weapon.Content = AnimatedObject.FromMetadata(content.Load<AnimatedObjectMetadata>(metadata.AssetName), content);
            }
            return weapon;
        }
    }
}
