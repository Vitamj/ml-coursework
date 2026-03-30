using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;

public abstract class CreatureModifier
{
    public virtual void OnAfterAttack(ICreature attacker, ICreature target)
    {
    }

    public virtual void OnBeforeReceiveDamage(ICreature creature, ref int damage)
    {
    }

    public bool IsExpired { get; protected set; }

    public virtual CreatureModifier Clone()
    {
        return (CreatureModifier)MemberwiseClone();
    }
}