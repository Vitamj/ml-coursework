using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

public class MagicMirror : ISpell
{
    public void Apply(ICreature creature)
    {
        if (creature is Creature concreteCreature)
        {
            int oldAttack = concreteCreature.Attack.Value;
            int oldHealth = concreteCreature.Health.Value;

            concreteCreature.Attack = new AttackValue(oldHealth);
            concreteCreature.Health = new HealthValue(oldAttack);
        }
    }
}