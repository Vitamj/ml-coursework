using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

public class StrengthPotion : ISpell
{
    public void Apply(ICreature creature)
    {
        if (creature is Creature concreteCreature)
        {
            concreteCreature.Attack = new AttackValue(concreteCreature.Attack.Value + 5);
        }
    }
}