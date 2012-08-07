using Cardio.UI.Core;
using Cardio.UI.Core.Alive;
using Cardio.UI.Projectiles;

namespace Cardio.UI.Characters.Ranged
{
    public interface IRangeEnemy : IAlive, ICollidable, IObstacle, IAttacker
    {
        float AttackInterval { get; set; }

        float BulletSpeed { get; set; }

        Bullet GenerateBullet();
    }
}
