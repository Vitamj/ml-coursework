using System;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

public class MimicChest : Creature
{
    public MimicChest()
        : base(new AttackValue(1), new HealthValue(1))
    {
    }

    public override void PerformAttack(ICreature target)
    {
        if (target is not null)
        {
            Attack = new AttackValue(Math.Max(Attack.Value, target.Attack.Value));
            Health = new HealthValue(Math.Max(Health.Value, target.Health.Value));
        }

        base.PerformAttack(target);
    }
}