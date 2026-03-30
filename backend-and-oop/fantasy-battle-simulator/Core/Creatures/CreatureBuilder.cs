using System;
using System.Collections.Generic;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Infrastructure.Factories;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

public class CreatureBuilder
{
    private readonly List<CreatureModifier> _modifiers = new();
    private AttackValue? _attack;
    private HealthValue? _health;
    private ICreatureFactory? _factory;

    public CreatureBuilder WithFactory(ICreatureFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        return this;
    }

    public CreatureBuilder WithAttack(AttackValue attack)
    {
        _attack = attack ?? throw new ArgumentNullException(nameof(attack));
        return this;
    }

    public CreatureBuilder WithHealth(HealthValue health)
    {
        _health = health ?? throw new ArgumentNullException(nameof(health));
        return this;
    }

    public CreatureBuilder WithModifier(CreatureModifier modifier)
    {
        if (modifier is null) throw new ArgumentNullException(nameof(modifier));
        _modifiers.Add(modifier);
        return this;
    }

    public ICreature Build()
    {
        ICreature baseCreature = _factory.Create();

        if (baseCreature is not Creature creature)
            return baseCreature;

        if (_attack is not null)
            creature.Attack = _attack;

        if (_health is not null)
            creature.Health = _health;

        foreach (CreatureModifier modifier in _modifiers)
        {
            creature.AddModifier(modifier.Clone());
        }

        return creature;
    }
}