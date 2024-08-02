namespace HouseAuction.Tests._Shared
{
    public static class WaitFor
    {
        public static async Task Condition(Func<bool> condition, int timeout = 5000)
        {
            var cts = new CancellationTokenSource(timeout);
            var start = DateTime.UtcNow;

            while (!condition() && DateTime.UtcNow < start.AddMilliseconds(timeout))
            {
                await Task.Delay(100);
            }
        }
    }
}
