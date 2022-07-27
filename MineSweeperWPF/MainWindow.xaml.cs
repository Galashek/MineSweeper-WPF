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
        public Style empty, opened, flagged, failed, pressed;

        //public Timer timer;

        public MainWindow()
        {
            InitializeComponent();

            empty = Resources["empty"] as Style;
            opened = Resources["opened"] as Style;
            flagged = Resources["flagged"] as Style;
            failed = Resources["failed"] as Style;
            pressed = Resources["pressed"] as Style;

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
                    tile.ChangeStyle(empty);
                    tile.MouseLeftButtonUp += Tile_MouseLeftButtonUp;
                    tile.MouseRightButtonDown += Tile_MouseRightButtonDown;
                    tile.MouseEnter += Tile_MouseEnter;
                    tile.MouseLeave += Tile_MouseLeave;
                    tile.MouseLeftButtonDown += Tile_MouseLeftButtonDown;
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

        private void Tile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is not Tile tile) return;
            if (tile.Style == empty)
                tile.ChangeStyle(pressed);
            if (tile.Style == opened)
                MarkNearTilesToOpen(tile.Position);
        }

        private IEnumerable<Tile> Nearby(Position pos)
        {
            for (int i = pos.Row - 1; i <= pos.Row + 1; i++)
            {
                if (i < 0 || i >= currentSettings.Rows) continue;
                for (int j = pos.Column - 1; j <= pos.Column + 1; j++)
                {
                    if (j < 0 || j >= currentSettings.Columns) continue;
                    if (i == pos.Row && j == pos.Column) continue;
                    yield return tiles[i, j];
                }
            }
        }

        private void MarkNearTilesToOpen(Position pos)
        {
            foreach (var t in Nearby(pos))
            {
                if (t.Style == empty)
                    t.ChangeStyle(pressed);
            }
        }

        private void UnmarkNearTilesToOpen(Position pos)
        {
            foreach (var t in Nearby(pos))
            {
                if (t.Style == pressed)
                    t.ChangeStyle(empty);
            }
        }

        private void Tile_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.Source is not Tile tile) return;
            if (tile.Style == pressed)
                tile.ChangeStyle(empty);
            if (tile.Style == opened)
                UnmarkNearTilesToOpen(tile.Position);

        }

        private void Tile_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.Source is not Tile tile) return;
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (tile.Style == empty)
                    tile.ChangeStyle(pressed);
                if (tile.Style == opened)
                    MarkNearTilesToOpen(tile.Position);
            }
        }

        private void Tile_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is not Tile tile) return;
            game.Flag(tile.Position);            
        }

        private void Tile_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is not Tile tile) return;
            game.HandleOpen(tile.Position);
            if (tile.Style == opened)
                UnmarkNearTilesToOpen(tile.Position);
        }

        private void StartGame()
        {
            //timer.StartTimer();            
        }

        private void Win()
        {
            //timer.StopTimer();
            foreach (var mine in game.MinePositions)
                tiles[mine.Row, mine.Column].ChangeStyle(flagged);

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
            tiles[pos.Row, pos.Column].ChangeStyle(opened);
            if (mineCount > 0)
                tiles[pos.Row, pos.Column].Number = mineCount.ToString();
            else
                tiles[pos.Row, pos.Column].IsHitTestVisible = false;
        }
        
        private void FlagTile(Position pos, bool state)
        {
            tiles[pos.Row, pos.Column].ChangeStyle(state ? flagged : empty);
        }

        private void Explode(IEnumerable<Position> mines)
        {
            DisableAllTiles();
            foreach (var pos in mines)
                tiles[pos.Row, pos.Column].ChangeStyle(failed);
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
