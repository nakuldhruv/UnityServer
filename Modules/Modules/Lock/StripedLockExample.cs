namespace Modules.Lock;

internal class StripedLockExample
{
    private SemaphoreSlim[] _stripedLocks;

    private Dictionary<long, string> _passportMap = new Dictionary<long, string>()
    {
        { 1, "Passport1" },
        { 2, "Passport2" },
        { 3, "Passport3" },
        { 4, "Passport4" },
        { 5, "Passport5" }
    };

    public void StartAsync()
    {
    }

    private async Task GetPassportInfo(long passportId)
    {
    }

    private SemaphoreSlim GetStripedLock()
    {
        return _stripedLocks[0];
    }
}
