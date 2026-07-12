using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules;

public class Xorshift128Example
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