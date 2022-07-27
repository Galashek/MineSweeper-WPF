using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MineSweeper;

namespace MineSweeperWPF
{
    public partial class MainWindow : Window
    {
        private Game game;
        private Settings currentSettings;

        private Tile[,] tiles;
        public Style empty, opened, flagged, failed;

        //public Timer timer;

        public MainWindow()
        {
            InitializeComponent();

            empty = Resources["empty"] as Style;
            opened = Resources["opened"] as Style;
            flagged = Resources["flagged"] as Style;
            failed = Resources["failed"] as Style;

            CreateGame(Settings.Easy);
        }

        private void CreateGame(Settings settings)
        {
            currentSettings = settings;
            var (rows, columns, mines) = settings;
            ResizeWindow(rows, columns);

            minesCountLabel.Text = mines.ToString();
            game = new Game(currentSettings);
            
            tiles = new Tile[rows, columns];

            field.Children.Clear();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var tile = new Tile()
                    {
                        Position = new Position(i, j)
                    };
                    tile.ChangeImage(empty);
                    tile.PreviewMouseDown += Tile_Click;
                    //tile.PreviewMouseDown += (s,e) => MessageBox.Show("Click");
                    tiles[i, j] = tile;
                    field.Children.Add(tile);
                }
            }
            game.Start += StartGame;
            game.Win += Win;
            game.Lose += Lose;
            game.FlagStateChanged += (x, y) => minesCountLabel.Text = game.MinesLeft.ToString();
            game.CellOpened += OpenTile;
            game.FlagStateChanged += FlagTile;
            game.Win += DisableAllTiles;
        }

        private void Tile_Click(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is not Tile tile) return;
            if (e.ChangedButton == MouseButton.Left)
            {
                game.HandleOpen(tile.Position);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                game.Flag(tile.Position);
            }
        }

        private void StartGame()
        {
            //timer.StartTimer();            
        }

        private void Win()
        {
            //timer.StopTimer();
            MessageBox.Show("You win");            
        }

        private void Lose()
        {
            //timer.StopTimer();
            Explode(game.MinePositions);
        }

        private void Restart(object s, RoutedEventArgs e)
            => CreateGame(currentSettings);
        
        private void StartEasyGame(object s, RoutedEventArgs e)
            => CreateGame(Settings.Easy);

        private void StartMediumGame(object s, RoutedEventArgs e)
            => CreateGame(Settings.Medium);

        private void StartHardGame(object s, RoutedEventArgs e)
            => CreateGame(Settings.Hard);

        private void StartCustomGame(object s, RoutedEventArgs e)
        {
            var window = new CustomSettingsWindow { Owner = this };
            if (window.ShowDialog() == true)
                CreateGame(window.Settings);
        }

        public void ResizeWindow(int rows, int columns)
        {
            field.Children.Clear();
            field.Rows = rows;
            field.Columns = columns;
            var tileSize = 40d;
            window.Height = (field.Rows * tileSize) + menu.Height + gamePanel.Height;
            window.Width = field.Columns * tileSize;
        }

        private void OpenTile(Position pos, int mineCount)
        {
            tiles[pos.Row, pos.Column].ChangeImage(opened);
            if (mineCount > 0)
                tiles[pos.Row, pos.Column].Number = mineCount.ToString();
            else
                tiles[pos.Row, pos.Column].IsHitTestVisible = false;
        }
        
        private void FlagTile(Position pos, bool state)
        {
            tiles[pos.Row, pos.Column].ChangeImage(state ? flagged : empty);
        }

        private void Explode(IEnumerable<Position> mines)
        {
            DisableAllTiles();
            foreach (var pos in mines)
                tiles[pos.Row, pos.Column].ChangeImage(failed);
        }

        private void DisableAllTiles()
        {
            foreach (var tile in tiles)
                tile.IsHitTestVisible = false;
        }

        private void Exit(object s, RoutedEventArgs e) 
            => Application.Current.Shutdown();
    }
}
