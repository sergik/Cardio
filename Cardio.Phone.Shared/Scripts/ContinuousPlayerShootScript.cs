using System.Collections.Generic;
using Cardio.Phone.Shared.Core.Alive;
using Cardio.Phone.Shared.Scripts;
using Cardio.Phone.Shared.Core.Alive;

namespace Cardio.Phone.Shared.Scripts
{
    public class ContinuousPlayerShootScript : PlayerShootScript
    {
        public ContinuousPlayerShootScript(IEnumerable<IPlayerAttackTarget> targets) : base(targets){}

        public void ContinueFor(float additionalShootingTime)
        {
            ShootingTime = additionalShootingTime;
            CurrentShootingTime = 0;
        }
    }
}