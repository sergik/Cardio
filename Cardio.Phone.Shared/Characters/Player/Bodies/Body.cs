using Cardio.Phone.Shared.Bodies;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Animations;
using Microsoft.Xna.Framework.Content;

namespace Cardio.Phone.Shared.Characters.Player.Bodies
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
