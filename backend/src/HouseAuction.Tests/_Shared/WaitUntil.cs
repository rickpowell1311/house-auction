namespace HouseAuction.Tests._Shared;

public class WaitUntil
{
    public static async Task IsTrue(
        Func<bool> assertion, 
        int timeout = 5000)
    {
        await Wait(assertion, timeout);
    }

    public static async Task IsNotNull<T>(
        Func<T> obj,
        int timeout = 5000)
    {
        await Wait(() => obj() != null, timeout);
    }

    private static async Task Wait(Func<bool> assertion, int timeout)
    {
        var cancellationToken = new CancellationTokenSource(timeout);

        try
        {
            while (!cancellationToken.IsCancellationRequested || !assertion())
            {
                await Task.Delay(25, cancellationToken.Token);
            }
        }
        catch (TaskCanceledException)
        {
            Assert.Fail($"Couldn't assert the condition within the timeout of {timeout}ms");
        }
    }
}
