using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Actions
{
    public class ReactionProgressComponent
    {
        private int _reactionDuration;

        public float ReactionInertia { get; set; }

        public const float InitialProgressValue = 100;

        public bool IsReactionInProgress { get; private set; }

        public float RemainingReactionProgress { get; private set; }

        public void StartProgress(int duration)
        {
            IsReactionInProgress = true;
            _reactionDuration = duration;
            RemainingReactionProgress = InitialProgressValue;
        }

        public void Update(GameTime gameTime)
        {
            if (IsReactionInProgress)
            {
                RemainingReactionProgress -= (float) (InitialProgressValue / _reactionDuration * gameTime.ElapsedGameTime.TotalMilliseconds);
                if (RemainingReactionProgress <= 0)
                {
                    IsReactionInProgress = false;
                }
            }
        }

        public void Reset()
        {
            RemainingReactionProgress = 0;
            IsReactionInProgress = false;
        }
    }
}
