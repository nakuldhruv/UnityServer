namespace Modules;

internal class GlobalLockExample
{
    private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

    public async Task StartAsync()
    {
/*        var tasks = new[]
        {
            DoWorkAsync("任务A"),
            DoWorkAsync("任务B"),
            DoWorkAsync("任务C")
        };*/

        var tasks = new[]
{
            DoWorkAsyncWithScope("任务A"),
            DoWorkAsyncWithScope("任务B"),
            DoWorkAsyncWithScope("任务C")
        };

        await Task.WhenAll(tasks);
        Console.WriteLine("所有任务执行完毕。");
    }

    private async Task DoWorkAsync(string taskName)
    {
        Console.WriteLine($"{taskName} 等待获取锁...");
        await _lock.WaitAsync(); // 异步等待锁
        try
        {
            Console.WriteLine($"{taskName} 进入锁，开始工作...");
            await Task.Delay(2000); // 模拟耗时操作
            Console.WriteLine($"{taskName} 工作完成。");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{taskName} 出现异常：{ex.Message}");
        }
        finally
        {
            _lock.Release();
            Console.WriteLine($"{taskName} 释放锁。");
        }
    }

    private async Task DoWorkAsyncWithScope(string taskName)
    {
        Console.WriteLine($"{taskName} 等待获取锁...");
        using var scope = new SemaphoreSlimScope(_lock);
        await scope.WaitAsync();
        Console.WriteLine($"{taskName} 进入锁，开始工作...");
        await Task.Delay(2000); // 模拟耗时操作
        Console.WriteLine($"{taskName} 工作完成。");
        Console.WriteLine($"{taskName} 释放锁。");
    }
}