using System;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Infrastructure.Catalog;

public class CreatureCatalogEntry
{
    private readonly CreatureBuilder _builder;

    public CreatureCatalogEntry(
        string name,
        CreatureBuilder builder)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        _builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }

    public string Name { get; }

    public ICreature Create()
    {
        return _builder.Build();
    }
}