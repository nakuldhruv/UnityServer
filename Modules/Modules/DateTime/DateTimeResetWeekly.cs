namespace Modules;

public partial class DateTimeResetWeekly
{
    private long _lastMondayResetTimestamp;

    /// <summary>
    /// 每周重置逻辑：基于当前时间，计算出本周一的零点（00:00:00）对应的 Unix 时间戳。
    /// 如果 <see cref="_lastMondayResetTimestamp"/> 小于该时间戳，说明尚未在本周进行重置，
    /// 则将其更新为本周一的零点时间戳，从而标记本周已重置。
    /// </summary>
    public bool ResetIfNeeded()
    {
        // 1. 获取当前系统时间（本地时间，注意时区问题）
        var nowDate = System.DateTime.Now;
        // 2. 获取当前是星期几（Sunday=0, Monday=1, ..., Saturday=6）
        var currentDayOfWeek = (int)nowDate.DayOfWeek;
        // 3. 计算需要回退的天数，使得结果日期是本周一
        //    公式: (7 + currentDayOfWeek - (int)DayOfWeek.Monday) % 7
        //    其中 (int)DayOfWeek.Monday = 1
        //    示例：若今天是周一（1），则 daysToSubtract = (7+1-1)%7 = 0，无需回退
        //          若今天是周日（0），则 daysToSubtract = (7+0-1)%7 = 6，回退6天到周一
        var daysToSubtract = (7 + currentDayOfWeek - (int)DayOfWeek.Monday) % 7;
        // 4. 得到本周一的日期（时间部分与 nowDate 相同）
        var mondayZeroDate = nowDate.AddDays(-daysToSubtract);
        var mondayZeroTime = mondayZeroDate.ToMidnightUnixTimeSeconds();
        if (_lastMondayResetTimestamp < mondayZeroTime)
        {
            _lastMondayResetTimestamp = mondayZeroTime;
            return true;
        }

        return false;
    }
}