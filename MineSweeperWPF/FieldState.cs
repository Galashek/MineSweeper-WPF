namespace MineSweeperWPF
{
    internal class FieldState
    {
        private readonly bool[,] opened;
        private readonly bool[,] flagged;

        public FieldState(int rows, int columns)
        {
            opened = new bool[rows, columns];
            flagged = new bool[rows, columns];
        }

        public bool IsOpened(Position pos)
        {
            return opened[pos.Row, pos.Column];
        }

        public bool IsFlagged(Position pos)
        {
            return flagged[pos.Row, pos.Column];
        }

        public void Open(Position pos)
        {
            opened[pos.Row, pos.Column] = true;
        }

        public void ChangeFlagState(Position pos)
        {
            flagged[pos.Row, pos.Column] = !flagged[pos.Row, pos.Column];
        }
    }
}
