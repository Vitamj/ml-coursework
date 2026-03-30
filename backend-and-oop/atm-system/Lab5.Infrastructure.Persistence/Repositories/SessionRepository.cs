using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence.Queries;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;

namespace Itmo.ObjectOrientedProgramming.Lab5.Infrastructure.Persistence.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly System.Collections.Generic.Dictionary<System.Guid, Session> _sessions = new();

    public Session? Query(SessionQuery query)
    {
        if (query.Id is not null)
        {
            return _sessions.TryGetValue(query.Id.Value.Value, out Session? session) ? session : null;
        }

        return null;
    }

    public void Add(Session session)
    {
        _sessions[session.Id.Value] = session;
    }

    public void Remove(SessionId id)
    {
        _sessions.Remove(id.Value);
    }
}