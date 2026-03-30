using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Infrastructure.Factories;

public class ViciousFighterFactory : ICreatureFactory
{
    public ICreature Create()
    {
        return new ViciousFighter();
    }
}