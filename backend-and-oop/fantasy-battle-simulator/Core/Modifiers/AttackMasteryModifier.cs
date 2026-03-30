using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;

public class AttackMasteryModifier : CreatureModifier
{
    private bool _hasBonusAttack;

    public override void OnAfterAttack(ICreature attacker, ICreature target)
    {
        if (_hasBonusAttack) return;
        if (attacker.Health.Value <= 0) return;

        _hasBonusAttack = true;
        attacker.PerformAttack(target);
        IsExpired = true;
    }

    public override CreatureModifier Clone()
    {
        return new AttackMasteryModifier();
    }
}