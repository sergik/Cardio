using Cardio.Phone.Shared.Core.Alive;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core.Alive;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Cardio.Phone.Shared.Characters
{
    public class ObstacleGameObject: AliveGameObject, IObstacle, IPlayerAttackTarget
    {
        public float StopDistance { get; set; }

        public int PlayerAttackPriority { get; protected set; }

        public int BiomaerialGeneratedMin { get; protected set; }

        public int BiomaerialGeneratedMax { get; protected set; }

        public static ObstacleGameObject FromMetadata(ObstacleMetadata metadata, ContentManager contentManager)
        {
            return FillWithMetadata(new ObstacleGameObject(), metadata, contentManager);
        }

        public static ObstacleGameObject FillWithMetadata(ObstacleGameObject obstacleGameObject, ObstacleMetadata metadata, ContentManager contentManager)
        {
            AliveGameObject.FillWithMetadata(obstacleGameObject, metadata, contentManager);
            obstacleGameObject.StopDistance = metadata.StopDistance;
            obstacleGameObject.PlayerAttackPriority = metadata.PlayerAttackPriority;
            obstacleGameObject.BiomaerialGeneratedMin = (int) metadata.BiomaterialGeneratedInterval.X;
            obstacleGameObject.BiomaerialGeneratedMax = (int) metadata.BiomaterialGeneratedInterval.Y;

            return obstacleGameObject;
        }
    }
}
