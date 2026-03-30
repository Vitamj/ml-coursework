using Itmo.ObjectOrientedProgramming.Lab3.Core.Abilities;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

public interface ICreature
{
    AttackValue Attack { get; }
    HealthValue Health { get; }
    bool CanAttack { get; }
    bool CanBeAttacked { get; }

    ICreature Clone();
    void PerformAttack(ICreature target);
    void ReceiveDamage(int damage);
}