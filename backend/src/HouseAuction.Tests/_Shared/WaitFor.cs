namespace HouseAuction.Tests._Shared
{
    public static class WaitFor
    {
        public static async Task Condition(Func<bool> condition, int timeout = 5000)
        {
            var cts = new CancellationTokenSource(timeout);

            while (cts.IsCancellationRequested)
            {
                if (condition())
                {
                    return;
                }

                await Task.Delay(100);
            }
        }
    }
}
