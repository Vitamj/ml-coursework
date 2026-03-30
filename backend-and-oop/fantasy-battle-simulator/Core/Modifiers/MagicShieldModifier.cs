using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;

public class MagicShieldModifier : CreatureModifier
{
    private bool _isUsed;

    public override void OnBeforeReceiveDamage(ICreature creature, ref int damage)
    {
        if (_isUsed) return;

        damage = 0;
        _isUsed = true;
        IsExpired = true;
    }

    public override CreatureModifier Clone()
    {
        return new MagicShieldModifier();
    }
}