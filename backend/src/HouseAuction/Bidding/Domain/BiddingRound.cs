namespace HouseAuction.Bidding.Domain
{
    public class BiddingRound
    {
        private readonly PlayerCycle _playerCycle;
        private readonly Dictionary<string, Play> _plays;
        
        public bool HasFinished => _plays.Count == _playerCycle.TotalPlayers
            && _plays.Values.Where(x => !x.IsPass).Count() <= 1;

        public BiddingRound(PlayerCycle playerCycle)
        {
            _playerCycle = playerCycle;
            _plays = [];
        }

        public void MakePlay(string player, Play play)
        {
            if (HasFinished)
            {
                throw new InvalidOperationException("Round has finished");
            }

            if (_playerCycle.CurrentPlayer != player)
            {
                throw new InvalidOperationException($"It's not {player}'s turn");
            }

            var highestBid = _plays.Values
                .Select(x => x.Amount)
                .DefaultIfEmpty(0)
                .Max();

            if (play.Amount <= highestBid)
            {
                throw new InvalidOperationException($"Bid must be higher than {highestBid}");
            }

            _plays[player] = play;

            if (!HasFinished)
            {
                _playerCycle.Next();

                var nextPlayer = _playerCycle.CurrentPlayer;
                while (_plays.ContainsKey(nextPlayer) && _plays[nextPlayer].IsPass)
                {
                    _playerCycle.Next();
                    nextPlayer = _playerCycle.CurrentPlayer;
                }
            }
        }
    }
}
