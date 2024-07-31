namespace HouseAuction.Lobby.Domain
{
    public class GameId
    {
        public static string NewGameId()
        {
            var chars = new[] { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Y', 'Z' };
            var picks = new List<char>();

            for (var i = 0; i < 5; i++)
            {
                picks.Add(chars[new Random().Next(0, chars.Length)]);
            }

            return new string(picks.ToArray());
        }
    }
}
