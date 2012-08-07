using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.UI.Animations;
using Cardio.UI.Core;
using Microsoft.Xna.Framework.Content;

namespace Cardio.UI.Bodies
{
    public class Body : AttachableGameObject
    {
        public float Health
        {
            get; set;
        }

        public float MaxHealth
        {
            get; set;
        }

        public static Body FromMetadata(BodyMetadata metadata, ContentManager content)
        {
            var body = new Body();
            body.Content = AnimatedObject.FromMetadata(content.Load<AnimatedObjectMetadata>(metadata.AssetName), content);
            body.MaxHealth = metadata.MaxHealth;
            body.Health = metadata.MaxHealth;
            return body;
        }
    }
}
