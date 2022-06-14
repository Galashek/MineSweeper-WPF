using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeperWPF
{
    public static class Settings
    {
        public static int Rows { get; set; } = 9;
        public static int Columns { get; set; } = 9;
        public static int Mines { get; set; } = 10;
        public static bool NewGame { get; set; } = true;
    }
}
