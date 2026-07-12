namespace Modules
{
    internal class SemaphoreSlimScope : IDisposable
    {
        private readonly SemaphoreSlim _semaphoreSlim;
        private bool _disposed;

        public SemaphoreSlimScope(SemaphoreSlim semaphoreSlim)
        {
            _semaphoreSlim = semaphoreSlim;
        }

        public async Task WaitAsync()
        {
            await _semaphoreSlim.WaitAsync();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                Console.WriteLine("锁被错误重复释放，请检查。");
                return;
            }

            _disposed = true;
            _semaphoreSlim.Release();
        }
    }
}
