using Cardio.Phone.Shared.Core.Alive;
using Cardio.Phone.Shared.Projectiles;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core.Alive;

namespace Cardio.Phone.Shared.Characters.Ranged
{
    public interface IRangeEnemy : IAlive, ICollidable, IObstacle, IAttacker
    {
        float AttackInterval { get; set; }

        float BulletSpeed { get; set; }

        Bullet GenerateBullet();
    }
}
