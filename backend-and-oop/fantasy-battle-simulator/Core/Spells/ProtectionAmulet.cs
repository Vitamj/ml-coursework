using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

public class ProtectionAmulet : ISpell
{
    public void Apply(ICreature creature)
    {
        if (creature is Creature concreteCreature)
        {
            concreteCreature.AddModifier(new MagicShieldModifier());
        }
    }
}