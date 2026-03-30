using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Infrastructure.Factories;

public class BattleAnalystFactory : ICreatureFactory
{
    public ICreature Create()
    {
        return new BattleAnalyst();
    }
}