using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MineSweeperWPF
{
    public partial class CustomSettingsWindow : Window
    {
        public Settings Settings => new(
            int.Parse(rowsBox.Text), 
            int.Parse(columnsBox.Text), 
            int.Parse(minesBox.Text)
            );
        //public int Rows => int.Parse(rowsBox.Text);
        //public int Columns => int.Parse(columnsBox.Text);
        //public int Mines => int.Parse(minesBox.Text);

        public CustomSettingsWindow()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
