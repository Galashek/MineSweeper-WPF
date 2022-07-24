namespace MineSweeperWPF
{
    public struct Position
    {
        public readonly int Row;
        public readonly int Column;

        public Position(int row, int column) => (Row, Column) = (row, column);

        public override bool Equals(object obj) => obj is Position pos && Row == pos.Row && Column == pos.Column;

        public override int GetHashCode() => (Row * 397) ^ Column;

        public static bool operator ==(Position left, Position right) => left.Equals(right);

        public static bool operator !=(Position left, Position right) => !(left == right);
    }
}