using Cardio.UI.Shields;

namespace Cardio.UI.Core
{
    public interface IShielded
    {
        Shield Shield { get; set; }

        bool IsShieldActive { get; set; }

        void ActivateShield();

        void DeactivateShields();
    }
}
