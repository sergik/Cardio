using System.Collections.Generic;
using Cardio.UI.Actions;
using Cardio.UI.Input.Touch;

namespace Cardio.UI.Scenes.Actions
{
    public class SceneActionMetadata
    {
        public PlayerAction Action { get; set;}

        public IList<ButtonType> ActivationSequence { get; set; }
    }
}