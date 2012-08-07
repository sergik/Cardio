using System.Collections.Generic;
using Cardio.UI.Animations;
using Cardio.UI.Particles;

namespace Cardio.UI.Core.Alive
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
