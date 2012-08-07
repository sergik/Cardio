using System;
using Cardio.Phone.Shared.Actions;
using Cardio.Phone.Shared.Actions;

namespace Cardio.Shared.Scenes.Actions
{
    public class ReactionInvokedEventArgs : EventArgs
    {
        public ActionReaction ReactionInvoked { get; set; }
    }
}