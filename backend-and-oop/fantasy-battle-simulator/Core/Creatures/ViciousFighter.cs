namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

public class ViciousFighter : Creature
{
    public ViciousFighter()
        : base(new AttackValue(1), new HealthValue(6))
    {
    }

    public override void ReceiveDamage(int damage)
    {
        int healthBefore = Health.Value;
        base.ReceiveDamage(damage);

        if (Health.Value > 0 && healthBefore > Health.Value)
        {
            Attack = new AttackValue(Attack.Value * 2);
        }
    }
}