using System.Collections.Generic;
using Cardio.Phone.Shared.Input.Touch;
using Cardio.Phone.Shared.Scenes.Actions;

namespace Cardio.Phone.Shared.Actions
{
    public static class ActionCatalog
    {
        private static readonly IList<SceneActionMetadata> _actions = new List<SceneActionMetadata>();

        public static void AddAction(SceneActionMetadata action)
        {
            _actions.Add(action);
        }

        public static void AddAction(PlayerAction action, IList<ButtonType> sequence)
        {
            AddAction(new SceneActionMetadata{Action = action, ActivationSequence = sequence});
        }

        public static PlayerAction? CheckSequence(IList<ButtonTouch> sequence)
        {
            for (int i = 0; i < _actions.Count; i++ )
            {
                var action = _actions[i];
                if (CheckSequencesForEquality(sequence, action.ActivationSequence))
                {
                    return action.Action;
                }
            }

            return null;
        }

        private static bool CheckSequencesForEquality(IList<ButtonTouch> touches, IList<ButtonType> activationSequences)
        {
            if (touches.Count != activationSequences.Count)
            {
                return false;
            }

            for (int i = 0; i < touches.Count; i++)
            {
                if (touches[i].Button != activationSequences[i])
                {
                    return false;
                }
            }

            return true;
        }
    } 
}
