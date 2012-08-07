using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cardio.Phone.Shared.Particles
{
    public enum BlowType
    {
        None,
        TrombDeathBlow,
        TrombDamageBlow,
        PlayerDeathBlow,
        PlayerDamageBlow,
        EnemyDeathBlow,
        EnemyGenerationBlow,
        EnemyDamageBlow,
        BossDamageBlow
    }

    public class BlowEffectBox
    {
        public BlowEffect BlowEffect
        {
            get;
            set;
        }

        public BlowType BlowType
        {
            get;
            set;
        }
    }
}
