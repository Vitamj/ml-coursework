namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Battle;

public abstract record BattleResult
{
    private BattleResult() { }

    public sealed record Player1Wins : BattleResult;
    public sealed record Player2Wins : BattleResult;
    public sealed record Draw : BattleResult;
}