namespace HouseAuction.Lobby.Domain
{
    public static class FuzzyNameMatch
    {
        public static bool IsCloseMatch(string first, string second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return first.Strip() == second.Strip();
        }

        private static string Strip(this string val)
        {
            return val
                .Replace(" ", "")
                .Replace("_", "")
                .Replace("-", "");
        }
    }
}
