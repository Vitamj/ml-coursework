using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence.Queries;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Operations;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence.Repositories;

public interface IOperationRepository
{
    System.Collections.Generic.IReadOnlyCollection<Operation> Query(OperationQuery query);

    void Add(Operation operation);
}