using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace FinCouple.API.Controllers;

[ApiController]
[Route("api/sse")]
[Authorize]
public class SseController : ControllerBase
{
    private readonly IConnectionMultiplexer _redis;

    public SseController(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    [HttpGet("stream")]
    public async Task Stream([FromQuery] Guid coupleId, CancellationToken cancellationToken)
    {
        Response.Headers["Content-Type"] = "text/event-stream";
        Response.Headers["Cache-Control"] = "no-cache";
        Response.Headers["Connection"] = "keep-alive";

        var channel = $"couple:{coupleId}";
        var subscriber = _redis.GetSubscriber();

        var tcs = new TaskCompletionSource<bool>();
        cancellationToken.Register(() => tcs.TrySetResult(true));

        await subscriber.SubscribeAsync(RedisChannel.Literal(channel), async (ch, message) =>
        {
            if (cancellationToken.IsCancellationRequested) return;
            var data = $"data: {message}\n\n";
            await Response.WriteAsync(data, cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        });

        // Keep the connection open until client disconnects
        await tcs.Task;
        await subscriber.UnsubscribeAsync(RedisChannel.Literal(channel));
    }
}
