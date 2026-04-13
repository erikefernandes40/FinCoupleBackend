using System.Text.Json;
using FinCouple.Application.Services.Interfaces;
using StackExchange.Redis;

namespace FinCouple.Infrastructure.Services;

public class RedisEventPublisher : IEventPublisher
{
    private readonly IConnectionMultiplexer _redis;

    public RedisEventPublisher(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task PublishExpenseCreatedAsync(Guid coupleId, object payload, CancellationToken cancellationToken = default)
    {
        var db = _redis.GetSubscriber();
        var channel = $"couple:{coupleId}";
        var json = JsonSerializer.Serialize(payload);
        await db.PublishAsync(RedisChannel.Literal(channel), json);
    }
}
