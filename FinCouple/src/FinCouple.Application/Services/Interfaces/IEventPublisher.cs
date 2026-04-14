namespace FinCouple.Application.Services.Interfaces;

public interface IEventPublisher
{
    Task PublishExpenseCreatedAsync(Guid coupleId, object payload, CancellationToken cancellationToken = default);
}
