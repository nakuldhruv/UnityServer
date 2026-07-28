using System.Diagnostics;

namespace Modules;

public class RateLimitingPeriodicTimerExample
{
    public async Task StartAsync()
    {
        await RateLimiting();
    }

    private async Task RateLimiting()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        // 创建一个每 100ms 触发一次的定时器
        using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(100));

        for (var i = 0; i < 20; i++)
        {
            // 耗时操作 (模拟耗时 50ms)
            await Task.Delay(50);

            // 自动等待直到下一个 100ms 周期。如果耗时操作超过100ms，它会立即继续而不再等待。
            await timer.WaitForNextTickAsync();
        }

        stopwatch.Stop();
        // 预期输出将会非常接近 2000ms
        Console.WriteLine($"总耗时: {stopwatch.ElapsedMilliseconds} ms");
    }
}

// 一秒10个
// 每个100ms
// 超过100ms不管，小于100ms等待到100ms

// 进入->小于100ms等待到100ms->继续执行
// 进入->超过100ms->放行，立即进入下一次循环，并瞄准时刻表上的下一个整点。
/*【100 ms】：后台闹钟响了，门打开。
    【150 ms】：你第一次干完活，走到 wait。
看到门开着，瞬间通过。
通过的瞬间，门被关闭（150ms - 200ms 之间被彻底关闭）。
第一次循环结束，进入第二次循环。
    【150ms ~ 200ms 之间】（比如 170ms）：
你第二次干活干得很快，170ms 就干完了，再次来到 wait。
此时看门，门是关闭的！（因为刚才 150ms 的时候被关上了）。
你必须在门外等待 30ms（从 170ms 等到 200ms）。
    【200 ms】：
后台闹钟再次响起，门再次打开！第二次放行！*/