using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Battle;

public class BattleEngine
{
    private const int MaxRounds = 100;

    public BattleResult ConductBattle(PlayerTable firstTable, PlayerTable secondTable)
    {
        PlayerTable combatTable1 = firstTable.Clone();
        PlayerTable combatTable2 = secondTable.Clone();

        for (int round = 0; round < MaxRounds; round++)
        {
            ICreature? attacker1 = combatTable1.GetNextAttacker();
            ICreature? target1 = combatTable2.GetNextDefender();

            if (attacker1 is not null && target1 is not null)
            {
                attacker1.PerformAttack(target1);
            }
            else if (attacker1 is not null && target1 is null)
            {
                return new BattleResult.Player1Wins();
            }

            ICreature? attacker2 = combatTable2.GetNextAttacker();
            ICreature? target2 = combatTable1.GetNextDefender();

            if (attacker2 is not null && target2 is not null)
            {
                attacker2.PerformAttack(target2);
            }
            else if (attacker2 is not null && target2 is null)
            {
                return new BattleResult.Player2Wins();
            }

            if (!combatTable1.HasAliveCreatures() && !combatTable2.HasAliveCreatures())
                return new BattleResult.Draw();

            if (!combatTable1.HasAliveCreatures())
                return new BattleResult.Player2Wins();

            if (!combatTable2.HasAliveCreatures())
                return new BattleResult.Player1Wins();
        }

        return new BattleResult.Draw();
    }
}