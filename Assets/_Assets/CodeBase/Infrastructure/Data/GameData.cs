namespace countMastersTest.infrastructure.data
{
    public class GameData
    {
        private int _coins;
        private int _level;

        public GameData()
        {
            _coins = 0;
            _level = 1;
        }

        public int coins { get => _coins; set => _coins = value; }
        public int level { get => _level; set => _level = value; }
    }
}
