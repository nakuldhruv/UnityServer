using System.Diagnostics;

namespace Modules;

public class RateLimitingExample
{
    public async Task StartAsync()
    {
        await RateLimiting();
    }

    private async Task RateLimiting()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        long intervalMs = 1000L / 10; // 100ms
        for (var i = 0; i < 20; i++)
        {
            long startMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // 耗时操作
            await Task.Delay(100);

            long endMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long costMs = endMs - startMs; // 50ms
            long remainMs = intervalMs - costMs; // 50ms
            if (remainMs > 0)
                await Task.Delay((int)remainMs);
        }

        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
    }
}