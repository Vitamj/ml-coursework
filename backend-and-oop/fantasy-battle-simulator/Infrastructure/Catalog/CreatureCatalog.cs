using System;
using System.Collections.Generic;
using System.Linq;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Infrastructure.Factories;

namespace Itmo.ObjectOrientedProgramming.Lab3.Infrastructure.Catalog;

public class CreatureCatalog
{
    public CreatureCatalog()
    {
        Entries = new[]
        {
            new CreatureCatalogEntry(
                name: "BattleAnalyst",
                builder: new CreatureBuilder()
                    .WithFactory(new BattleAnalystFactory())
                    .WithAttack(new AttackValue(2))
                    .WithHealth(new HealthValue(4))),

            new CreatureCatalogEntry(
                name: "ViciousFighter",
                builder: new CreatureBuilder()
                    .WithFactory(new ViciousFighterFactory())
                    .WithAttack(new AttackValue(1))
                    .WithHealth(new HealthValue(6))),

            new CreatureCatalogEntry(
                name: "MimicChest",
                builder: new CreatureBuilder()
                    .WithFactory(new MimicChestFactory())
                    .WithAttack(new AttackValue(1))
                    .WithHealth(new HealthValue(1))),

            new CreatureCatalogEntry(
                name: "ImmortalHorror",
                builder: new CreatureBuilder()
                    .WithFactory(new ImmortalHorrorFactory())
                    .WithAttack(new AttackValue(4))
                    .WithHealth(new HealthValue(4))),

            new CreatureCatalogEntry(
                name: "AmuletMaster",
                builder: new CreatureBuilder()
                    .WithFactory(new AmuletMasterFactory())
                    .WithAttack(new AttackValue(5))
                    .WithHealth(new HealthValue(2))
                    .WithModifier(new MagicShieldModifier())
                    .WithModifier(new AttackMasteryModifier())),
        };
    }

    public IReadOnlyCollection<CreatureCatalogEntry> Entries { get; }

    public CreatureCatalogEntry? FindByName(string name)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        return Entries.FirstOrDefault(entry => entry.Name == name);
    }
}
