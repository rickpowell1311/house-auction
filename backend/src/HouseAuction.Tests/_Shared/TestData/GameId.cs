namespace HouseAuction.Tests._Shared.TestData
{
    public static class GameId
    {
        public static string Generate()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
