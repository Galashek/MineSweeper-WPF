using MineSweeper;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MineSweeperWPF
{
    public partial class TileGrid : UserControl
    {
        private Tile[,] tiles;
        public Style empty, opened, flagged, mined, failed, pressed;

        public event Action<Position> TileOpen;
        public event Action<Position> TileFlag;

        private int rows, columns;

        public TileGrid()
        {
            InitializeComponent();
            empty = Resources["empty"] as Style;
            opened = Resources["opened"] as Style;
            flagged = Resources["flagged"] as Style;
            mined = Resources["mined"] as Style;
            failed = Resources["failed"] as Style;
            pressed = Resources["pressed"] as Style;
        }

        public void Initialize(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            grid.Rows = rows;
            grid.Columns = columns;
            tiles = new Tile[rows, columns];
            grid.Children.Clear();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var tile = new Tile()
                    {
                        Position = new Position(i, j)
                    };                    
                    tile.Style = empty;
                    tile.MouseLeftButtonUp += Tile_MouseLeftButtonUp;
                    tile.MouseRightButtonDown += Tile_MouseRightButtonDown;
                    tile.MouseEnter += Tile_MouseEnter;
                    tile.MouseLeave += Tile_MouseLeave;
                    tile.MouseLeftButtonDown += Tile_MouseLeftButtonDown;
                    tiles[i, j] = tile;
                    grid.Children.Add(tile);
                }
            }
        }

        private void Tile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is not Tile tile) return;
            if (tile.Style == empty)
                tile.Style = pressed;
            if (tile.Style == opened)
                MarkNearTilesToOpen(tile.Position);
        }

        private IEnumerable<Tile> Nearby(Position pos)
        {
            for (int i = pos.Row - 1; i <= pos.Row + 1; i++)
            {
                if (i < 0 || i >= rows) continue;
                for (int j = pos.Column - 1; j <= pos.Column + 1; j++)
                {
                    if (j < 0 || j >= columns) continue;
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
                    t.Style = pressed;
            }
        }

        private void UnmarkNearTilesToOpen(Position pos)
        {
            foreach (var t in Nearby(pos))
            {
                if (t.Style == pressed)
                    t.Style = empty;
            }
        }

        private void Tile_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.Source is not Tile tile) return;
            if (tile.Style == pressed)
                tile.Style = empty;
            if (tile.Style == opened)
                UnmarkNearTilesToOpen(tile.Position);

        }

        private void Tile_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.Source is not Tile tile) return;
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (tile.Style == empty)
                    tile.Style = pressed;
                if (tile.Style == opened)
                    MarkNearTilesToOpen(tile.Position);
            }
        }

        private void Tile_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is not Tile tile) return;
            TileFlag.Invoke(tile.Position);
        }

        private void Tile_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is not Tile tile) return;
            TileOpen.Invoke(tile.Position);
            if (tile.Style == opened)
                UnmarkNearTilesToOpen(tile.Position);
        }

        public void OpenTile(Position pos, int mineCount)
        {
            var tile = tiles[pos.Row, pos.Column];
            tile.Style = opened;
            if (mineCount > 0)
                tile.Number = mineCount.ToString();
            else
                tile.IsEnabled = false;
        }

        public void FlagTile(Position pos, bool state)
        {
            tiles[pos.Row, pos.Column].Style = state ? flagged : empty;
        }

        public void MarkMines(IEnumerable<Position> mines, Position failedPos)
        {
            foreach (var pos in mines)
            {
                var t = tiles[pos.Row, pos.Column];
                if (pos == failedPos)
                    t.Style = failed;
                else if (t.Style != flagged)
                    t.Style = mined;
            }
        }

        public void MarkFlags(IEnumerable<Position> mines)
        {
            foreach (var mine in mines)
                tiles[mine.Row, mine.Column].Style = flagged;
        }

        public void DisableAllTiles()
        {
            foreach (var tile in tiles)
                tile.IsEnabled = false;
        }
    }
}
