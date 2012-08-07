using System;
using Cardio.UI.Particles;

namespace Cardio.UI.Core
{
	public interface IParticleEmitter
	{
		Func<Particle> DamageParticleGenerator { get; set; }

		Action<Particle> EmitParticle { get; set; }
	}
}