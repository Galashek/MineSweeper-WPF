using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeperWPF
{
    class Field
    {
        // -1 - мина
        // 0-8 - количество мин в соседних клетках
        public readonly int Columns;
        public readonly int Rows;
        public readonly int Mines;
        public readonly Position[] MinePositions;
        private readonly int[,] FieldArray;
        //private readonly (int,int)[,][] Neighbors;

        public Field(int rows, int columns, int mines)
        {
            if (mines >= columns * rows)
                throw new ArgumentException();
            if (mines < 0 || columns <= 0 || rows <= 0)
                throw new ArgumentException();
            Columns = columns;
            Rows = rows;
            Mines = mines;
            FieldArray = new int[rows, columns];
            MinePositions = new Position[mines];
            //Neighbors = new (int, int)[rows, columns][];
        }

        public int this[int index1, int index2] => FieldArray[index1, index2];
        public int this[Position position] => FieldArray[position.Row, position.Column];

        public void CreateOnClick(int rowEx, int columnEx)
        {
            if (FieldArray == null)
                throw new Exception("Grid is not initialized!");
            SetMines(rowEx * Columns + columnEx);
            SetNumbers();
        }

        //Reservoir sampling Simple
        private void SetMines(int exclude)
        {
            var rnd = new System.Random();
            var R = new int[Mines]; // Номера ячеек по порядку где стоят мины

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
                FieldArray[pos.Row, pos.Column] = -1;
                MinePositions[i] = pos;
            }
        }

        private void SetNumbers()
        {
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    if (FieldArray[i, j] != -1)
                    {
                        FieldArray[i, j] = GetNearMinesCount(i, j);
                        //Neighbors[i, j] = FindNearby(i, j);
                    }
        }

        private int GetNearMinesCount(int row, int col)
        {
            //int k = 0;
            //for (int i = row - 1; i <= row + 1; i++)
            //    for (int j = col - 1; j <= col + 1; j++)
            //        if (i >= 0 && j >= 0 && i < Rows && j < Columns)
            //            if (GridArray[i, j] == -1) k++;
            //return k;

            int[] d = { -1, 0, 1 };
            return d.SelectMany(x => d.Select(y => (row + x, col + y)))
                .Count(x => x.Item1 >= 0 && x.Item1 < Rows
                       && x.Item2 >= 0 && x.Item2 < Columns && x != (row, col)
                       && FieldArray[x.Item1, x.Item2] == -1);
        }

        //public (int, int)[] FindNearby(int row, int col)
        //{
        //    int[] d = { -1, 0, 1 };
        //    return d.SelectMany(x => d.Select(y => (row + x, col + y)))
        //            .Where(x => x.Item1 >= 0 && x.Item1 < Rows 
        //                     && x.Item2 >= 0 && x.Item2 < Columns
        //                     && x != (row, col))
        //            .ToArray();
        //}

        public Position[] FindNearby(Position pos)
        {
            int[] d = { -1, 0, 1 };
            return d.SelectMany(x => d.Select(y => new Position(pos.Row + x, pos.Column + y)))
                .Where(x => x.Row >= 0 && x.Row < Rows
                                       && x.Column >= 0 && x.Column < Columns
                                       && !x.Equals(pos))
                .ToArray();
        }

        //public (int, int)[] GetNeighbors(int row, int col) => Neighbors[row, col];
    }
}
