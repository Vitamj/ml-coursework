using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence.Repositories;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence;

public interface IPersistenceContext
{
    IAccountRepository Accounts { get; }

    ISessionRepository Sessions { get; }

    IOperationRepository Operations { get; }
}