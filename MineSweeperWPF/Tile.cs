using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MineSweeperWPF
{
    class Tile : Button
    {
        public Position Position { get; }

        public Tile(Position position)
        {
            Position = position;
            Background = Brushes.WhiteSmoke;            
        }
    }
}
