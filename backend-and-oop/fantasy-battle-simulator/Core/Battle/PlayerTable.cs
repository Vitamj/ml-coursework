using System;
using System.Collections.Generic;
using System.Linq;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Battle;

public class PlayerTable
{
    private const int MaxCreatures = 7;

    private readonly List<ICreature> _creatures = new();

    private int _nextAttackerIndex;
    private int _nextDefenderIndex;

    public IReadOnlyCollection<ICreature> Creatures => _creatures;

    public void AddCreature(ICreature creature)
    {
        if (_creatures.Count >= MaxCreatures)
            return;

        _creatures.Add(creature);
    }

    public IEnumerable<ICreature> GetAttackingCreatures()
    {
        return _creatures.Where(c => c.CanAttack);
    }

    public IEnumerable<ICreature> GetTargetCreatures()
    {
        return _creatures.Where(c => c.CanBeAttacked);
    }

    public bool HasAliveCreatures()
    {
        return _creatures.Any(c => c.Health.Value > 0);
    }

    public PlayerTable Clone()
    {
        var copy = new PlayerTable();
        foreach (ICreature creature in _creatures)
        {
            copy._creatures.Add(creature.Clone());
        }

        return copy;
    }

    public ICreature? GetNextAttacker()
    {
        var alive = _creatures
            .Where(c => c.CanAttack)
            .ToList();

        if (alive.Count == 0)
            return null;

        if (_nextAttackerIndex >= alive.Count)
            _nextAttackerIndex = 0;

        ICreature attacker = alive[_nextAttackerIndex];
        _nextAttackerIndex++;

        return attacker;
    }

    public ICreature? GetNextDefender()
    {
        var alive = _creatures
            .Where(c => c.CanBeAttacked)
            .ToList();

        if (alive.Count == 0)
            return null;

        if (_nextDefenderIndex >= alive.Count)
            _nextDefenderIndex = 0;

        ICreature defender = alive[_nextDefenderIndex];
        _nextDefenderIndex++;

        return defender;
    }
}