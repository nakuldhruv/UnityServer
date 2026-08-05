namespace Modules;

public class DateTimeResetWeekly
{
    private long _lastMondayResetTimestamp;

    /// <summary>
    /// 每周重置逻辑：基于当前 UTC 时间，计算出本周一的零点（00:00:00 UTC）对应的 Unix 时间戳。
    /// 如果 <see cref="_lastMondayResetTimestamp"/> 小于该时间戳，说明尚未在本周进行重置，
    /// 则将其更新为本周一的零点时间戳，从而标记本周已重置。
    ///
    /// 使用 UTC 时间计算，保证服务器部署在任意时区时每周重置点一致。
    /// </summary>
    /// <param name="initialLastResetTimestamp">初始的上次重置时间戳（可用于从持久化状态恢复），默认 0 表示从未重置。</param>
    public DateTimeResetWeekly(long initialLastResetTimestamp = 0)
    {
        _lastMondayResetTimestamp = initialLastResetTimestamp;
    }

    /// <summary>
    /// 返回本周一零点（UTC）对应的 Unix 时间戳。
    /// </summary>
    public static long GetThisMondayUtcTimestamp(System.DateTime nowUtc)
    {
        // 1. 获取当前是星期几（Sunday=0, Monday=1, ..., Saturday=6）
        var currentDayOfWeek = (int)nowUtc.DayOfWeek;
        // 2. 计算需要回退的天数，使得结果日期是本周一
        //    公式: (7 + currentDayOfWeek - (int)DayOfWeek.Monday) % 7
        //    其中 (int)DayOfWeek.Monday = 1
        //    示例：若今天是周一（1），则 daysToSubtract = (7+1-1)%7 = 0，无需回退
        //          若今天是周日（0），则 daysToSubtract = (7+0-1)%7 = 6，回退6天到周一
        var daysToSubtract = (7 + currentDayOfWeek - (int)DayOfWeek.Monday) % 7;
        // 3. 得到本周一的日期（UTC，时间归零）
        var mondayDate = nowUtc.Date.AddDays(-daysToSubtract);
        var mondayUtc = System.DateTime.SpecifyKind(mondayDate, System.DateTimeKind.Utc);
        return new System.DateTimeOffset(mondayUtc).ToUnixTimeSeconds();
    }

    public bool ResetIfNeeded()
    {
        var nowUtc = System.DateTime.UtcNow;
        var mondayZeroTime = GetThisMondayUtcTimestamp(nowUtc);
        if (_lastMondayResetTimestamp < mondayZeroTime)
        {
            _lastMondayResetTimestamp = mondayZeroTime;
            return true;
        }

        return false;
    }
}