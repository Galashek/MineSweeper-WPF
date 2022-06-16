using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeperWPF
{
    public class Settings
    {
        public int Rows { get; }
        public int Columns { get; }
        public int Mines { get; }
        //public static bool NewGame { get; set; } = true;

        public static Settings Easy => new(9, 9, 10);
        public static Settings Medium => new(16, 16, 40);
        public static Settings Hard => new(16, 30, 99);

        public Settings(int rows, int columns, int mines)
        {
            (Rows, Columns, Mines) = (rows, columns, mines);
        }

        internal void Deconstruct(out int rows, out int columns, out int mines)
        {
            (rows, columns, mines) = (Rows, Columns, Mines);
        }
    }
}
