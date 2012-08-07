using System;
using System.Collections.Generic;
using Cardio.UI.Characters;
using Cardio.UI.Core;
using Cardio.UI.Core.Alive;
using Cardio.UI.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Cardio.UI.Thrombi
{
    public class Thrombus: ObstacleGameObject, IParticleEmitter
    {
        public static readonly Random Random = new Random();

        private float _totalDamage;
        public float DamageEmissionThreshold { get; set; }

        public Func<Particle> DamageParticleGenerator { get; set; }

        public Action<Particle> EmitParticle { get; set; }

        public Thrombus()
        {
            DamageEmissionThreshold = 30;
            _totalDamage = DamageEmissionThreshold;
            DamageTaken += OnDamageTaken;
        }

        private void OnDamageTaken(IAlive source, DamageTakenEventArgs e)
        {
            _totalDamage += e.Damage;
            while (_totalDamage > DamageEmissionThreshold && DamageParticleGenerator != null)
            {
                var particle = DamageParticleGenerator();
                var x = Random.Next(40, WorldCollisionRectangle.Width - 40);
                var y = Random.Next(40, WorldCollisionRectangle.Height - 40);

                particle.WorldPosition = new Vector2(WorldCollisionRectangle.X + x, WorldCollisionRectangle.Y + y);
                EmitParticle(particle);

                _totalDamage -= DamageEmissionThreshold;
            }
        }

        public override IList<ParticleEffect> ToDeathAnimation()
        {
            var result = new List<ParticleEffect>();
            var random = new Random(DateTime.Now.Millisecond);

            for (var i = 1; i <= 10; i++)
            {
                var DeathEffect = BlowEffect.FromMetadata(DeathEffectMetadata, Game.Content);
                DeathEffect.WorldPosition = WorldPosition + new Vector2((random.Next(2) == 1 ? 1 : -1) * random.Next(200) + Content.BoundingRectangle.Center.X, (random.Next(2) == 0 ? 1 : -1) * random.Next(200) + Content.BoundingRectangle.Center.Y);
                result.Add(DeathEffect);
            }
            //DeathEffect.Initialize(Game, SpriteBatch, Camera);

            return result;
        }

        public override IList<ParticleEffect> ToDamageAnimation()
        {
            var result = new List<ParticleEffect>();
            var random = new Random(DateTime.Now.Millisecond);

            for (var i = 1; i <= 2; i++)
            {
                var DamageEffect = BlowEffect.FromMetadata(DamageEffectMetadata, Game.Content);
                DamageEffect.WorldPosition = WorldPosition + new Vector2((random.Next(2) == 1 ? 1 : -1) * random.Next(200) + Content.BoundingRectangle.Center.X, (random.Next(2) == 0 ? 1 : -1) * random.Next(200) + Content.BoundingRectangle.Center.Y);
                result.Add(DamageEffect);
            }
            //DeathEffect.Initialize(Game, SpriteBatch, Camera);

            return result;
        }

        public static Thrombus FromMetadata(ThrombusMetadata metadata, ContentManager contentManager)
        {
            var thrombus = new Thrombus();
            FillWithMetadata(thrombus, metadata, contentManager);
            return thrombus;
        }

        public static void FillWithMetadata(Thrombus thrombus, ThrombusMetadata metadata, ContentManager contentManager)
        {
            ObstacleGameObject.FillWithMetadata(thrombus, metadata, contentManager);
        }

        
       
    }
}
