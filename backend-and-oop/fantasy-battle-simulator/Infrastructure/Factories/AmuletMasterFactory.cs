using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Infrastructure.Factories;

public class AmuletMasterFactory : ICreatureFactory
{
    public ICreature Create()
    {
        return new AmuletMaster();
    }
}