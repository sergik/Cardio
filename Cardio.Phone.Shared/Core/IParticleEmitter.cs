using System;
using Cardio.Phone.Shared.Particles;
using Cardio.Phone.Shared.Particles;

namespace Cardio.Phone.Shared.Core
{
	public interface IParticleEmitter
	{
		Func<Particle> DamageParticleGenerator { get; set; }

		Action<Particle> EmitParticle { get; set; }
	}
}