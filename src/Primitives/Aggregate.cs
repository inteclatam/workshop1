

namespace Intec.Workshop1.Customers.Primitives;

public class Aggregate<TId> : Entity<TId>, IAggregate<TId>, IHaveDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Add a domain event to the aggregate
    /// </summary>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Does the aggregate have change that have not been committed to storage
    /// </summary>
    public bool HasUncommittedDomainEvents()
    {
        return _domainEvents.Any();
    }

    /// <summary>
    /// Gets a list of uncommitted events for this aggregate.
    /// </summary>
    public IReadOnlyList<IDomainEvent> GetUncommittedDomainEvents()
    {
        return _domainEvents.AsReadOnly();
    }

    /// <summary>
    /// Gets a list of uncommitted events for this aggregate, mark all events as committed.
    /// </summary>
    public IReadOnlyList<IDomainEvent> DequeueUncommittedDomainEvents()
    {
        var events = _domainEvents.ToList().AsReadOnly();
        _domainEvents.Clear();
        return events;
    }

    /// <summary>
    /// Mark all changes (events) as committed, clears uncommitted changes.
    /// </summary>
    public void MarkUncommittedDomainEventAsCommitted()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// Check specific rule for aggregate and throw an exception if rule is not satisfied.
    /// </summary>
    public void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new BusinessRuleValidationException(rule);
        }
    }
}