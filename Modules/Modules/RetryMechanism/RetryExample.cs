using System;
using System.Threading.Tasks;

namespace Modules;

public class RetryMechanismExample
{
    private int _attemptCount = 0;

    public async Task StartAsync()
    {
        Console.WriteLine("=== 开始重试机制演示 ===");

        try
        {
            // 调用封装好的重试方法：最大重试 3 次，初始等待 1000 毫秒
            await ExecuteWithRetryAsync(FlakyOperationAsync, maxRetries: 3, initialDelayMs: 1000);
            
            Console.WriteLine("=== 操作最终成功！ ===");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"=== 操作最终失败，已放弃: {ex.Message} ===");
        }
    }

    /// <summary>
    /// 模拟一个不稳定的耗时操作（前2次必定报错，第3次才会成功）
    /// </summary>
    private async Task FlakyOperationAsync()
    {
        _attemptCount++;
        Console.WriteLine($"[操作] 正在进行第 {_attemptCount} 次尝试...");
        
        await Task.Delay(200); // 模拟网络请求耗时

        if (_attemptCount <= 2)
        {
            Console.WriteLine($"[操作] 第 {_attemptCount} 次尝试失败，抛出异常！");
            throw new InvalidOperationException("网络连接超时或服务器无响应");
        }
        
        Console.WriteLine($"[操作] 第 {_attemptCount} 次尝试完美成功！");
    }

    /// <summary>
    /// 通用重试机制执行器
    /// </summary>
    /// <param name="operation">要执行的任务</param>
    /// <param name="maxRetries">最大重试次数（不包含首次执行）</param>
    /// <param name="initialDelayMs">初始等待时间（毫秒）</param>
    private async Task ExecuteWithRetryAsync(Func<Task> operation, int maxRetries, int initialDelayMs)
    {
        int currentDelay = initialDelayMs;

        // 循环次数 = 1次正常执行 + maxRetries次重试
        for (int i = 0; i <= maxRetries; i++)
        {
            try
            {
                await operation(); // 尝试执行任务
                return; // 如果没报错，直接 return 结束方法，跳出循环
            }
            catch (Exception ex)
            {
                // 如果当前已经是最后一次重试了，就不要再吞掉异常了，直接抛出给外层
                if (i == maxRetries)
                {
                    Console.WriteLine($"[重试器] 达到最大重试次数 ({maxRetries}次)，不再重试。");
                    throw; 
                }

                // 还没到最大重试次数，等待一段时间后继续循环
                Console.WriteLine($"[重试器] 捕获到异常 '{ex.Message}'");
                Console.WriteLine($"[重试器] 等待 {currentDelay}ms 后进行第 {i + 1} 次重试...\n");
                
                await Task.Delay(currentDelay);
                
                // 指数退避：每次重试的等待时间翻倍 (1s -> 2s -> 4s)
                currentDelay *= 2; 
            }
        }
    }
}