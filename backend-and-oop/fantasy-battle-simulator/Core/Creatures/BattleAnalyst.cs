namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

public class BattleAnalyst : Creature
{
    public BattleAnalyst()
        : base(new AttackValue(2), new HealthValue(4))
    {
    }

    public override void PerformAttack(ICreature target)
    {
        Attack = new AttackValue(Attack.Value + 2);
        base.PerformAttack(target);
    }
}