using System;
using System.Collections.Generic;
using System.Linq;

namespace MineSweeperWPF
{
    internal class Field
    {
        private readonly int[,] field;
        public readonly int Rows, Columns, Mines;
        public IEnumerable<Position> MinePositions() => Positions.Where(IsMine);

        private IEnumerable<Position> Positions
        {
            get
            {
                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Columns; j++)
                        yield return new Position(i, j);
            }
        }

        public Field(int rows, int columns, int mines)
        {            
            (Rows, Columns, Mines) = (rows, columns, mines);
            field = new int[rows, columns];
        }

        public int this[Position pos]
        {
            get => field[pos.Row, pos.Column];
            private set => field[pos.Row, pos.Column] = value;
        }

        public void Init(Position startPoint)
        {
            SetMines(startPoint);
            SetNumbers();
        }

        //Reservoir sampling - Algorithm R
        private void SetMines(Position start)
        {
            var rnd = new Random();
            var R = new int[Mines];
            var exclude = start.Row * Columns + start.Column;

            for (int i = 0; i < Mines; i++)
                R[i] = i;

            if (exclude < Mines)
            {
                R[exclude] = Mines;
                for (int i = Mines + 1; i < Columns * Rows; i++)
                {
                    var j = rnd.Next(0, i + 1);
                    if (j <= Mines - 1)
                        R[j] = i;
                }
            }
            else
            {
                for (int i = Mines; i < Columns * Rows; i++)
                {
                    var j = rnd.Next(0, i + 1);
                    if (j <= Mines - 1 && i != exclude)
                        R[j] = i;
                }
            }
            for (int i = 0; i < Mines; i++)
            {
                var pos = new Position(R[i] / Columns, R[i] % Columns);
                this[pos] = -1; // -1 = Mine
            }
        }

        private void SetNumbers()
        {
            foreach (var pos in Positions)
            {
                if (!IsMine(pos))
                    this[pos] = NearbyOf(pos).Count(IsMine);
            }
        }

        public bool IsMine(Position pos) => this[pos] == -1;

        private bool InBounds(Position pos)
        {
            return pos.Row >= 0 && pos.Row < Rows &&
                pos.Column >= 0 && pos.Column < Columns;
        }

        public IEnumerable<Position> NearbyOf(Position pos)
        {
            int row = pos.Row;
            int col = pos.Column;
            for (int i = row - 1; i <= row + 1; i++)
                for (int j = col - 1; j <= col + 1; j++)
                {
                    var p = new Position(i, j);
                    if (InBounds(p) && p != pos)
                        yield return p;
                }
        }
    }
}