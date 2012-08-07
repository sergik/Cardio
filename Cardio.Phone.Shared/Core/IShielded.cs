using Cardio.Phone.Shared.Characters.Player.Shields;
using Cardio.Phone.Shared.Shields;

namespace Cardio.Phone.Shared.Core
{
    public interface IShielded
    {
        Shield Shield { get; set; }

        bool IsShieldActive { get; set; }

        void ActivateShield();

        void DeactivateShields();
    }
}
