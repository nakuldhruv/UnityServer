namespace Modules;

public partial class Reset
{
    // 私有字段：记录上一次每周重置的时间戳（Unix 秒数，对应本周一的零点）
    // 用于判断是否已经在本周内执行过重置操作，避免重复重置
    private long _lastMondayResetTimestamp;

    /// <summary>
    /// 每周重置逻辑：基于当前时间，计算出本周一的零点（00:00:00）对应的 Unix 时间戳。
    /// 如果 <see cref="_lastMondayResetTimestamp"/> 小于该时间戳，说明尚未在本周进行重置，
    /// 则将其更新为本周一的零点时间戳，从而标记本周已重置。
    /// </summary>
    public void ResetWeekly()
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

        // 5. 将本周一的日期转换为当天的零点（00:00:00）对应的 Unix 时间戳（秒）
        //    注意：ToMidnightUnixTimeSeconds 是一个自定义扩展方法，
        //    其作用为：将 DateTime 对象转换为当天 00:00:00 的 Unix 时间戳（自 1970-01-01 以来的秒数）
        var mondayZeroTime = mondayZeroDate.ToMidnightUnixTimeSeconds();

        // 6. 如果上次记录的重置时间戳小于本周一的零点时间戳，
        //    说明（可能由于跨周或首次调用）本周尚未执行过重置，
        //    则更新字段值，表示本周已完成重置
        if (_lastMondayResetTimestamp < mondayZeroTime)
        {
            _lastMondayResetTimestamp = mondayZeroTime;
        }
    }
}

public static class DateTimeUtil
{
    public static long ToMidnightUnixTimeSeconds(this System.DateTime dateTime)
    {
        var midnight = new System.DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        var epoch = new System.DateTime(1970, 1, 1, 0, 0, 0);
        return (long)(midnight - epoch).TotalSeconds;
    }
    
    public static long ToUnixTimeSeconds(this System.DateTime dateTime)
    {
        var epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var seconds = (long)(dateTime - epoch).TotalSeconds;
        return seconds;
    }
}

/*
 * Unix 时间戳（Unix timestamp）定义为 从 1970 年 1 月 1 日 00:00:00 UTC 开始经过的秒数（不含闰秒）。
 * 这个起始时间正是 Unix 系统的纪元（Epoch），最初由早期的 Unix 开发者选定，并沿用至今，成为很多计算机系统的时间表示基准。
*/