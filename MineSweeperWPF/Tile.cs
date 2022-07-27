using System.Windows;
using System.Windows.Controls;
using MineSweeper;

namespace MineSweeperWPF
{
    public class Tile : UserControl
    {
        public Position Position { get; init; }

        public void ChangeStyle(Style style)
        {
            Style = style;
        }

        public static readonly DependencyProperty NumberProperty =
        DependencyProperty.Register("Number", typeof(string),
        typeof(Tile), new UIPropertyMetadata(""));

        public string Number
        {
            get => (string)GetValue(NumberProperty);
            set => SetValue(NumberProperty, value);
        }
    }
}
