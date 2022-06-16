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
        public Position Position { get; set; }
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

        //void Awake()
        //{
        //    Image = GetComponent<Image>();
        //    Text = GetComponentInChildren<TMP_Text>();
        //}

        public void Disable()
        {
            disabled = true;
        }

        //public void OnPointerClick(PointerEventData eventData)
        //{
        //    if (disabled) return;
        //    if (eventData.button == PointerEventData.InputButton.Left)
        //        LeftClick?.Invoke(Position);
        //    else if (eventData.button == PointerEventData.InputButton.Right)
        //        RightClick?.Invoke(Position);
        //}
    }
}
