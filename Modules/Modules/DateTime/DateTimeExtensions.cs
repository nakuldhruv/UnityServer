namespace Modules;

public static class DateTimeExtensions
{
    /// <summary>
    /// 计算该日期所在"当天零点（本地时间）"对应的 Unix 时间戳（UTC 秒）。
    /// </summary>
    public static long ToMidnightUnixTimeSeconds(this System.DateTime dateTime)
    {
        var localMidnight = new System.DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, System.DateTimeKind.Local);
        return new System.DateTimeOffset(localMidnight).ToUnixTimeSeconds();
    }

    /// <summary>
    /// 计算该时间对应的 Unix 时间戳（UTC 秒）。
    /// 注意：若 <see cref="System.DateTime.Kind"/> 为 Unspecified，将被视为本地时间；
    /// 若为 Utc 则按 UTC 计算。
    /// </summary>
    public static long ToUnixTimeSeconds(this System.DateTime dateTime)
    {
        return new System.DateTimeOffset(dateTime).ToUnixTimeSeconds();
    }
}

/*
 * 全用 UTC；仅在面向用户展示的那一刻，才转为 Local。
 * Unix 时间戳（Unix timestamp）定义为 从 1970 年 1 月 1 日 00:00:00 UTC 开始经过的秒数（不含闰秒）。
 * 这个起始时间正是 Unix 系统的纪元（Epoch），最初由早期的 Unix 开发者选定，并沿用至今，成为很多计算机系统的时间表示基准。
 */