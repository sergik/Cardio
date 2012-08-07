using System.Collections.Generic;
using Cardio.UI.Core.Alive;

namespace Cardio.UI.Scripts
{
    public class ContinuousPlayerShootScript : PlayerShootScript
    {
        public ContinuousPlayerShootScript(IEnumerable<IAlive> targets) : base(targets){}

        public void ContinueFor(float additionalShootingTime)
        {
            ShootingTime = additionalShootingTime;
            CurrentShootingTime = 0;
        }
    }
}