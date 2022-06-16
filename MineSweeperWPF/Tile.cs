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
        public Position Position { get; init; }
        //public Image Image { get; private set; }
        //public TMP_Text Text { get; private set; }
        private bool disabled;

        public event Action<Position> LeftClick;
        public event Action<Position> RightClick;

        public Tile()
        {
            PreviewMouseDown += Tile_MouseUp;
            Background = Brushes.WhiteSmoke;            
        }

        private void Tile_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (disabled) return;
            if (e.ChangedButton == MouseButton.Left)
            {
                LeftClick?.Invoke(Position);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                RightClick?.Invoke(Position);
            }
        }

        public void Disable()
        {
            disabled = true;
        }
    }
}
