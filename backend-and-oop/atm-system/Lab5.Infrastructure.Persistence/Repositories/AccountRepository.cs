using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence.Queries;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;

namespace Itmo.ObjectOrientedProgramming.Lab5.Infrastructure.Persistence.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly System.Collections.Generic.Dictionary<long, Account> _accounts = new();

    public Account? Query(AccountQuery query)
    {
        if (query.Id is not null)
        {
            return _accounts.TryGetValue(query.Id.Value.Value, out Account? account) ? account : null;
        }

        if (query.AccountNumber is not null)
        {
            foreach (Account account in _accounts.Values)
            {
                if (string.Equals(account.AccountNumber, query.AccountNumber, System.StringComparison.Ordinal))
                    return account;
            }
        }

        return null;
    }

    public void Add(Account account)
    {
        _accounts[account.Id.Value] = account;
    }

    public void Update(Account account)
    {
        _accounts[account.Id.Value] = account;
    }
}