using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Infrastructure.Factories;

public class MimicChestFactory : ICreatureFactory
{
    public ICreature Create()
    {
        return new MimicChest();
    }
}