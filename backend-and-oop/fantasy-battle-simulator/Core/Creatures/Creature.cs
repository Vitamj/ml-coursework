using System;
using System.Collections.Generic;
using System.Linq;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

public abstract class Creature : ICreature
{
    public AttackValue Attack { get; internal set; }
    public HealthValue Health { get; internal set; }

    private List<CreatureModifier> _modifiers = new();

    protected Creature(AttackValue attack, HealthValue health)
    {
        Attack = attack ?? throw new ArgumentNullException(nameof(attack));
        Health = health ?? throw new ArgumentNullException(nameof(health));
    }

    public bool CanAttack => Attack.Value > 0 && Health.Value > 0;
    public bool CanBeAttacked => Health.Value > 0;

    public virtual ICreature Clone()
    {
        var copy = (Creature)MemberwiseClone();

        copy.Attack = new AttackValue(Attack.Value);
        copy.Health = new HealthValue(Health.Value);

        copy._modifiers = _modifiers
            .Select(m => m.Clone())
            .ToList();

        return copy;
    }

    public virtual void PerformAttack(ICreature target)
    {
        if (target is null || !CanAttack || !target.CanBeAttacked)
            return;

        target.ReceiveDamage(Attack.Value);

        foreach (CreatureModifier modifier in _modifiers.ToList())
        {
            modifier.OnAfterAttack(this, target);
        }

        RemoveExpiredModifiers();
    }

    public virtual void ReceiveDamage(int damage)
    {
        if (damage <= 0) return;

        int actualDamage = damage;

        foreach (CreatureModifier modifier in _modifiers.ToList())
        {
            modifier.OnBeforeReceiveDamage(this, ref actualDamage);
        }

        RemoveExpiredModifiers();

        if (actualDamage > 0)
        {
            Health = new HealthValue(Health.Value - actualDamage);
        }

        RemoveExpiredModifiers();
    }

    protected internal void AddModifier(CreatureModifier modifier)
    {
        if (modifier is null) throw new ArgumentNullException(nameof(modifier));
        _modifiers.Add(modifier);
    }

    private void RemoveExpiredModifiers()
    {
        _modifiers.RemoveAll(m => m.IsExpired);
    }
}