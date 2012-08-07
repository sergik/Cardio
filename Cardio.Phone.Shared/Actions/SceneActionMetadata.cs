using System.Collections.Generic;
using Cardio.Phone.Shared.Actions;
using Cardio.Phone.Shared.Input.Touch;

namespace Cardio.Phone.Shared.Scenes.Actions
{
    public class SceneActionMetadata
    {
        public PlayerAction Action { get; set;}

        public IList<ButtonType> ActivationSequence { get; set; }
    }
}