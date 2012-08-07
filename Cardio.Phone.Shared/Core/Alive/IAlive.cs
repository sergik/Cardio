using System.Collections.Generic;
using Cardio.Phone.Shared.Particles;
using Cardio.Phone.Shared.Core.Alive;
using Cardio.Phone.Shared.Particles;

namespace Cardio.Phone.Shared.Core.Alive
{
    /// <summary>
    /// Represents an object that has 'Health' information.
    /// </summary>
    public interface IAlive
    {
        float Health { get; set; }

        bool IsAlive { get; }

        //ParticleEffect DeathEffect { get; set; }

        event DamageTakenEventHandler DamageTaken;
        event DieEventHandler Die;

        void Damage(DamageTakenEventArgs e);

        void Heal(float addedHealth);

        IList<ParticleEffect> ToDeathAnimation();
        IList<ParticleEffect> ToDamageAnimation();
    }
}
