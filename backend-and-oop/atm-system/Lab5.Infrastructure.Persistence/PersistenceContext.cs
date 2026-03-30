using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence.Repositories;

namespace Itmo.ObjectOrientedProgramming.Lab5.Infrastructure.Persistence;

public class PersistenceContext : IPersistenceContext
{
    public PersistenceContext(
        IAccountRepository accounts,
        ISessionRepository sessions,
        IOperationRepository operations)
    {
        Accounts = accounts;
        Sessions = sessions;
        Operations = operations;
    }

    public IAccountRepository Accounts { get; }

    public ISessionRepository Sessions { get; }

    public IOperationRepository Operations { get; }
}