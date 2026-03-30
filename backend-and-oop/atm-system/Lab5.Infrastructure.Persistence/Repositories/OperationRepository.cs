using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence.Queries;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Operations;

namespace Itmo.ObjectOrientedProgramming.Lab5.Infrastructure.Persistence.Repositories;

public class OperationRepository : IOperationRepository
{
    private readonly System.Collections.Generic.List<Operation> _operations = new();

    public System.Collections.Generic.IReadOnlyCollection<Operation> Query(OperationQuery query)
    {
        if (query.AccountId is null)
            return _operations.AsReadOnly();

        var result = new System.Collections.Generic.List<Operation>();
        foreach (Operation operation in _operations)
        {
            if (operation.AccountId.Value == query.AccountId.Value.Value)
                result.Add(operation);
        }

        return result.AsReadOnly();
    }

    public void Add(Operation operation)
    {
        _operations.Add(operation);
    }
}