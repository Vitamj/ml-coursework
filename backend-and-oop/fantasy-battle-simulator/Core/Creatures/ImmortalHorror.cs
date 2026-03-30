namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

public class ImmortalHorror : Creature
{
    private bool _hasRevived;

    public ImmortalHorror()
        : base(new AttackValue(4), new HealthValue(4))
    {
    }

    public override void ReceiveDamage(int damage)
    {
        base.ReceiveDamage(damage);

        if (Health.Value <= 0 && !_hasRevived)
        {
            _hasRevived = true;
            Health = new HealthValue(1);
        }
    }

    public override ICreature Clone()
    {
        var copy = (ImmortalHorror)base.Clone();
        copy._hasRevived = false;
        return copy;
    }
}