namespace Modules;

internal class DateTimeResetWeeklyExample
{
    public void Start()
    {
        DateTimeResetWeekly resetWeekly = new DateTimeResetWeekly();
        while (true)
        {
            if (resetWeekly.ResetIfNeeded())
            {
                Console.WriteLine("Weekly reset performed.");
            }

            Task.Delay(1000).Wait(); // Wait for 1 second before checking again
        }
    }
}
