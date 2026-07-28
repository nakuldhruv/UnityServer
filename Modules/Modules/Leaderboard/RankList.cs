using System.Collections;

namespace Modules;

public class RankItem
{
    public int Rank;
    public long Value;
}

public class RankList<T> : IEnumerable<T> where T : RankItem
{
    public readonly List<T> RankItemDataList = new();

    public T this[int index]
    {
        get => this.RankItemDataList[index];
        set => this.RankItemDataList[index] = value;
    }

    private void SortNewItem(T rItem)
    {
        if (RankItemDataList.Count == 1)
        {
            rItem.Rank = 1;
            return;
        }

        rItem.Rank = RankItemDataList.Count;
        for (int i = RankItemDataList.Count - 1; i >= 1; i--)
        {
            var rankItemData = RankItemDataList[i];
            var nextRankItemData = RankItemDataList[i - 1];
            if (rankItemData.Value > nextRankItemData.Value)
            {
                RankItemDataList[i] = nextRankItemData;
                RankItemDataList[i - 1] = rankItemData;
                rankItemData.Rank--;
                nextRankItemData.Rank++;
            }
            else
                break;
        }
    }

    public void SortChangeItem(T rItem, bool bUpSearch)
    {
        int nStartIndex = rItem.Rank - 1;
        if (bUpSearch)
        {
            for (var i = nStartIndex; i >= 1; i--)
            {
                var rankItemData = RankItemDataList[i];
                var nextRankItemData = RankItemDataList[i - 1];
                if (rankItemData.Value > nextRankItemData.Value)
                {
                    RankItemDataList[i] = nextRankItemData;
                    RankItemDataList[i - 1] = rankItemData;
                    (nextRankItemData.Rank, rankItemData.Rank) = (rankItemData.Rank, nextRankItemData.Rank);
                }
                else
                    break;
            }
        }
        else
        {
            for (var i = nStartIndex; i < this.RankItemDataList.Count - 1; i++)
            {
                var rankItemData = RankItemDataList[i];
                var nextRankItemData = RankItemDataList[i + 1];
                if (nextRankItemData.Value > rankItemData.Value)
                {
                    RankItemDataList[i] = nextRankItemData;
                    RankItemDataList[i + 1] = rankItemData;
                    (nextRankItemData.Rank, rankItemData.Rank) = (rankItemData.Rank, nextRankItemData.Rank);
                }
                else
                    break;
            }
        }
    }

    public void Add(T rItem)
    {
        this.RankItemDataList.Add(rItem);
        this.SortNewItem(rItem);
    }

    public void Clear() => this.RankItemDataList.Clear();

    public List<T> Take(int nTakeCount) => this.RankItemDataList.Take(nTakeCount).ToList();

    public int Count => this.RankItemDataList.Count;

    public IEnumerator<T> GetEnumerator() => this.RankItemDataList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}