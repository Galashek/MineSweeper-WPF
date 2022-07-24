using System.Windows;

namespace MineSweeperWPF
{
    public partial class CustomSettingsWindow : Window
    {
        public Settings Settings => new(Rows, Columns, Mines);

        private int Rows => (int)rowsSlider.Value;
        private int Columns => (int)columnsSlider.Value;
        private int Mines => (int)minesSlider.Value;

        public CustomSettingsWindow()
        {
            InitializeComponent();
            rowsSlider.ValueChanged += (s, e) => UpdateMinesMaximum();
            columnsSlider.ValueChanged += (s, e) => UpdateMinesMaximum();
        }

        private void UpdateMinesMaximum() => minesSlider.Maximum = Rows * Columns - 1;

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
