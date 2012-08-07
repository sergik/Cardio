using System;
using System.Collections.Generic;
using Cardio.Shared.Scenes.Actions;
using Cardio.UI.Core;
using Cardio.UI.Extensions;
using Cardio.UI.Scenes.Actions;

namespace Cardio.UI.Actions
{
    public class ReactionsCatalog
    {
        private readonly Dictionary<PlayerAction, ActionReaction> _reactions = new Dictionary<PlayerAction, ActionReaction>();

        public event EventHandler<ReactionInvokedEventArgs> ReactionInvoked;

        public void RegisterReaction(PlayerAction action, ActionReaction reaction)
        {
            _reactions.Add(action, reaction);
        }

        public bool TryInvokeReaction(PlayerAction action, GameState state)
        {
            var reaction = _reactions[action];
            if (reaction.CanBeIvoked(state))
            {
                ReactionInvoked.Fire(this, () => new ReactionInvokedEventArgs{ReactionInvoked = reaction});
                reaction.Invoke(state);
                return true;
            }
            state.Player.Confuse();
            return false;
        }

        public bool ContainsReaction(PlayerAction action)
        {
            return _reactions.ContainsKey(action);
        }

        public void ClearReactions()
        {
            _reactions.Clear();
        }
    }
}
