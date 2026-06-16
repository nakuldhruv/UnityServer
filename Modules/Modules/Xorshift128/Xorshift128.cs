namespace Modules;

/// <summary>
/// Xorshift 128 位伪随机数生成器。
/// 算法作者：George Marsaglia (2003)
/// 特点：速度快（只用异或和移位），周期长（2^128 - 1），状态可完全保存/还原。
/// </summary>
public class Xorshift128
{
    // 内部状态：4 个 32 位无符号整数，总共 128 位
    private uint _x, _y, _z, _w;

    // 可选：记录已生成的随机数个数（用于确定性验证）
    private int _usedCount;

    /// <summary>
    /// 获取已经产生的随机数个数（每次调用 Next 都会增加）。
    /// </summary>
    public int UsedCount => _usedCount;

    /// <summary>
    /// 检查状态是否全零（全零状态会导致算法永远输出 0，应避免）。
    /// </summary>
    public bool IsInvalid => _x == 0 && _y == 0 && _z == 0 && _w == 0;

    /// <summary>
    /// 构造函数，使用默认种子 1。
    /// </summary>
    public Xorshift128() : this(1) { }

    /// <summary>
    /// 构造函数，使用一个 32 位整数作为种子。
    /// 种子通过 LCG（线性同余生成器）扩展为 4 个初始状态，确保非全零。
    /// </summary>
    /// <param name="seed">任意整数（0 会自动转为 1，避免全零）</param>
    public Xorshift128(int seed)
    {
        SetSeed(seed);
    }

    /// <summary>
    /// 构造函数，直接指定四个状态值（用于从保存的状态恢复）。
    /// </summary>
    public Xorshift128(uint x, uint y, uint z, uint w)
    {
        _x = x;
        _y = y;
        _z = z;
        _w = w;
        _usedCount = 0;
    }

    /// <summary>
    /// 重新设置种子，重置使用计数。
    /// </summary>
    public void SetSeed(int seed)
    {
        // 避免种子为 0 导致后续全零状态
        if (seed == 0) seed = 1;

        uint s = (uint)seed;
        // 使用 LCG 常数 1812433253 将单个种子扩散到四个状态
        _x = s;
        _y = _x * 1812433253u + 1;
        _z = _y * 1812433253u + 1;
        _w = _z * 1812433253u + 1;
        _usedCount = 0;
    }

    /// <summary>
    /// 核心随机数生成方法：产生一个 32 位无符号整数。
    /// 算法：Xorshift128（t = x^(x<<11); x=y; y=z; z=w; w = (w^(w>>19))^(t^(t>>8))）
    /// </summary>
    private uint NextUInt()
    {
        uint t = _x ^ (_x << 11);
        _x = _y;
        _y = _z;
        _z = _w;
        _w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8));
        _usedCount++;
        return _w;
    }

    /// <summary>
    /// 返回 [0, 1) 范围内的单精度浮点数。
    /// 实现方式：取随机数的高 23 位，除以 2^23-1，保证均匀且避免浮点精度问题。
    /// </summary>
    public float NextFloat()
    {
        // 0x7FFFFF = 8388607，是 2^23 - 1
        return (NextUInt() & 0x7FFFFF) / 8388607.0f;
    }

    /// <summary>
    /// 返回一个随机字节 (0-255)。
    /// </summary>
    public byte NextByte()
    {
        // 取随机数的高 8 位（第 24~31 位）
        return (byte)(NextUInt() >> 24);
    }

    /// <summary>
    /// 返回 [min, max) 范围内的随机浮点数（max 可能无法取到，取决于浮点精度）。
    /// </summary>
    public float Range(float min, float max)
    {
        float t = NextFloat();
        return min * t + max * (1 - t); // 等价于 min + (max-min)*t，但数值更稳定
    }

    /// <summary>
    /// 返回 [min, max] 范围内的随机整数，包含两端。
    /// 注意：由于使用取模，分布可能略有偏斜（但通常可接受）。
    /// </summary>
    public int Range(int min, int max)
    {
        if (min == max) return min;
        if (min > max)
        {
            // 交换，使 min <= max
            int tmp = min;
            min = max;
            max = tmp;
        }
        uint range = (uint)(max - min + 1); // 包含 max 所以要 +1
        uint value = NextUInt() % range;
        return min + (int)value;
    }

    /// <summary>
    /// 克隆当前生成器（包括状态和已使用计数），用于分支预测或保存快照。
    /// </summary>
    public Xorshift128 Clone()
    {
        var clone = new Xorshift128(_x, _y, _z, _w);
        clone._usedCount = this._usedCount;
        return clone;
    }

    /// <summary>
    /// 重置使用计数（注意：这不会改变随机数序列，只是计数归零）。
    /// 谨慎使用，因为计数通常用于同步验证，随意重置可能导致不一致。
    /// </summary>
    public void ResetUsedCount()
    {
        _usedCount = 0;
    }
}

public class Xorshift128Demo
{
    public void Start()
    {
        // 1. 用种子创建生成器
        Xorshift128 rng = new Xorshift128(12345);
        Console.WriteLine("=== 使用种子 12345 的随机序列 ===");
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"NextFloat: {rng.NextFloat():F6}, NextByte: {rng.NextByte()}, UsedCount: {rng.UsedCount}");
        }

        // 2. 演示整数范围随机 (包含两端)
        Console.WriteLine("\n=== 整数范围 [5,10] 随机 ===");
        for (int i = 0; i < 8; i++)
        {
            Console.Write(rng.Range(5, 10) + " ");
        }
        Console.WriteLine();

        // 3. 演示浮点数范围随机
        Console.WriteLine("\n=== 浮点数范围 [-1.0f, 1.0f] 随机 ===");
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine(rng.Range(-1.0f, 1.0f).ToString("F4"));
        }

        // 4. 克隆和状态恢复示例
        Console.WriteLine("\n=== 克隆当前状态并继续生成 ===");
        var snapshot = rng.Clone();            // 保存当前状态
        Console.WriteLine($"原生成器下一个值: {rng.NextFloat():F6}");
        Console.WriteLine($"克隆生成器下一个值: {snapshot.NextFloat():F6} (应该和原生成器相同)");

        // 5. 验证确定性：相同种子必定产生相同序列
        Xorshift128 another = new Xorshift128(12345);
        Console.WriteLine("\n=== 验证确定性 ===");
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"原始: {rng.NextFloat():F6} , 新种子: {another.NextFloat():F6}");
        }

        Console.WriteLine("\n按任意键退出...");
        Console.ReadKey();
    }
}