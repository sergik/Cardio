using System;
using Cardio.UI.Actions;

namespace Cardio.Shared.Scenes.Actions
{
    public class ReactionInvokedEventArgs : EventArgs
    {
        public ActionReaction ReactionInvoked { get; set; }
    }
}