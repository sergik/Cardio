namespace Cardio.Phone.Shared.Characters.Ranged
{
    public interface IAttacker
    {
        float AttackRange { get; set; }

        float AttackDamage { get; set; }
    }
}