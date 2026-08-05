namespace Modules;

/// <summary>
/// 分片锁（Striped Lock）示例：
/// 将并发访问密集的 key 集合分散到 N 把独立的锁上，
/// 避免全局锁带来的严重串行化，同时保证同一个 key 的访问互斥。
/// </summary>
internal class StripedLockExample
{
    private readonly SemaphoreSlim[] _stripedLocks;
    private readonly int _lockCount;

    private readonly Dictionary<long, string> _passportMap = new Dictionary<long, string>()
    {
        { 1, "Passport1" },
        { 2, "Passport2" },
        { 3, "Passport3" },
        { 4, "Passport4" },
        { 5, "Passport5" }
    };

    public StripedLockExample(int lockCount = 16)
    {
        if (lockCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(lockCount), "分片锁数量必须大于 0");

        _lockCount = lockCount;
        _stripedLocks = new SemaphoreSlim[lockCount];
        for (int i = 0; i < lockCount; i++)
        {
            _stripedLocks[i] = new SemaphoreSlim(1, 1);
        }
    }

    public async Task StartAsync()
    {
        // 并发模拟多个请求访问不同的 passportId
        var tasks = new[]
        {
            GetPassportInfo(1),
            GetPassportInfo(2),
            GetPassportInfo(3),
            GetPassportInfo(4),
            GetPassportInfo(5),
            GetPassportInfo(1), // 同一个 key 并发，验证互斥
            GetPassportInfo(3)  // 同一个 key 并发，验证互斥
        };

        await Task.WhenAll(tasks);
        Console.WriteLine("所有护照信息查询完毕。");
    }

    private async Task GetPassportInfo(long passportId)
    {
        var stripedLock = GetStripedLock(passportId);
        Console.WriteLine($"[护照 {passportId}] 等待获取分片锁...");

        await stripedLock.WaitAsync();
        try
        {
            Console.WriteLine($"[护照 {passportId}] 进入锁，开始查询...");
            // 模拟数据库/缓存查询耗时
            await Task.Delay(1000);

            if (_passportMap.TryGetValue(passportId, out var passport))
            {
                Console.WriteLine($"[护照 {passportId}] 查询成功: {passport}");
            }
            else
            {
                Console.WriteLine($"[护照 {passportId}] 未找到该护照。");
            }
        }
        finally
        {
            stripedLock.Release();
            Console.WriteLine($"[护照 {passportId}] 释放分片锁。");
        }
    }

    /// <summary>
    /// 根据 key 的哈希值选择对应的分片锁。
    /// 使用不带符号的哈希，避免负数取模问题。
    /// </summary>
    private SemaphoreSlim GetStripedLock(long key)
    {
        int hash = key.GetHashCode();
        int index = (hash & int.MaxValue) % _lockCount;
        return _stripedLocks[index];
    }
}