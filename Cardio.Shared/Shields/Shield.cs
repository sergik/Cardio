using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.UI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Shields
{
    public class Shield : AttachableGameObject
    {
        public Texture2D ShieldTexture
        { 
            get; set;
        }

        public bool IsActivated
        {
            get; set;
        }

        public float Radius { get; private set; }

        public float MaxShieldRadius { get; set; }

        public float Efficiency { get; set; }

        public Rectangle DrawRectangle
        {
            get; set;
        }

        private bool _isShieldCollapsing;
        private bool _isShieldShrinking;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var diff = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            if (_isShieldShrinking)
            {
                Radius += diff;
                if (Radius >= MaxShieldRadius)
                {
                    Radius = MaxShieldRadius;
                    _isShieldShrinking = false;
                }
            }
            else if (_isShieldCollapsing)
            {
                Radius -= diff;
                if (Radius <= 0)
                {
                    Radius = 0;
                    _isShieldCollapsing = false;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            
            if (Radius > 0)
            {
                var rect = new Rectangle(DrawRectangle.Center.X - (int)Radius,
                    DrawRectangle.Center.Y - (int)Radius, 2 * (int)Radius, 2 * (int)Radius);

                SpriteBatch.Begin();
                if (Camera != null)
                {
                    SpriteBatch.Draw(ShieldTexture, Camera.GetScreenRectangle(rect), Color.White);
                }
                else
                {
                    SpriteBatch.Draw(ShieldTexture, rect, Color.White);
                }
                SpriteBatch.End();
            }
        }

        public void Activate()
        {
            if (IsActivated)
            {
                return;
            }

            Radius = 0;
            IsActivated = true;
            _isShieldCollapsing = false;
            _isShieldShrinking = true;
        }

        public void Deactivate()
        {
            if (!IsActivated)
            {
                return;
            }

            IsActivated = false;
            _isShieldCollapsing = true;
            _isShieldShrinking = false;
        }

        public static Shield FromMetadata(ShieldMetadata metadata, ContentManager content)
        {
            var shield = new Shield();
            shield.ShieldTexture = content.Load<Texture2D>(metadata.ShieldTexturePath);
            shield.MaxShieldRadius = metadata.MaxShieldRadius;
            shield.Efficiency = metadata.ShieldEfficiency;

            return shield;
        }
    }
}
