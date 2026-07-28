using System.Diagnostics;

namespace Modules;

public class RankListBenchmark
{
    private RankList<RankItemExample> _rankList = new RankList<RankItemExample>();
    private List<QuickSortItemExample> _quickSortList = new List<QuickSortItemExample>();

    public void Start()
    {
        Stopwatch rankListWatch = new Stopwatch();
        rankListWatch.Start();
        for (int i = 0; i < 2000; i++)
        {
            _rankList.Add(new RankItemExample() { Value = i });
        }

        rankListWatch.Stop();
        Console.WriteLine($"RankList:{rankListWatch.ElapsedMilliseconds}ms");

        Stopwatch quickSortWatch = new Stopwatch();
        quickSortWatch.Start();
        for (int i = 0; i < 1000; i++)
        {
            _quickSortList.Add(new QuickSortItemExample() { Value = i });
            _quickSortList.Sort();
        }

        quickSortWatch.Stop();
        Console.WriteLine($"QuickSort:{quickSortWatch.ElapsedMilliseconds}ms");
    }
}

public class RankItemExample : RankItem
{
}

public class QuickSortItemExample : RankItem, IComparable<QuickSortItemExample>
{
    public int CompareTo(QuickSortItemExample? other)
    {
        if (other == null) return 1;
        return other.Value.CompareTo(this.Value);
    }
}