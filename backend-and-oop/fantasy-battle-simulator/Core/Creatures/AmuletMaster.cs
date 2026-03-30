namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

public class AmuletMaster : Creature
{
    public AmuletMaster()
        : this(new AttackValue(5), new HealthValue(2))
    {
    }

    public AmuletMaster(AttackValue attack, HealthValue health)
        : base(attack, health)
    {
    }
}