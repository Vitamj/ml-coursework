using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence.Queries;

public record AccountQuery(AccountId? Id = null, string? AccountNumber = null);