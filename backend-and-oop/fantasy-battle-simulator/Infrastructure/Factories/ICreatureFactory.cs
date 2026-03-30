using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Infrastructure.Factories;

public interface ICreatureFactory
{
    ICreature Create();
}