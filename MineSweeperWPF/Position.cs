using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeperWPF
{
    public class Position
    {
        public readonly int Row;
        public readonly int Column;

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Position)) return false;
            var pos = (Position)obj;
            return Row == pos.Row && Column == pos.Column;
        }

        public override int GetHashCode()
        {
            return (Row * 397) ^ Column;
        }
    }
}
