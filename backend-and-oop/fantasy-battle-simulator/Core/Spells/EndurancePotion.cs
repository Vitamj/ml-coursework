using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

public class EndurancePotion : ISpell
{
    public void Apply(ICreature creature)
    {
        if (creature is Creature concreteCreature)
        {
            concreteCreature.Health = new HealthValue(concreteCreature.Health.Value + 5);
        }
    }
}