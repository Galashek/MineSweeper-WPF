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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MineSweeperWPF
{
    class GameView
    {
        private Tile[,] tiles;
        //public Sprite empty, opened, flagged, failed;
        public Brush empty = Brushes.White, 
            opened = Brushes.SlateGray, flagged = Brushes.DarkBlue, failed = Brushes.Red;

        public void Initialize(Tile[,] tiles, GameState gameState)
        {
            this.tiles = tiles;
            gameState.TileOpen += OpenTile;
            gameState.FlagSet += FlagTile;
            gameState.MineOpen += Explode;
            gameState.AllTilesOpened += DisableAllTiles;
        }

        private void OpenTile(Position pos, int mineCount)
        {
            tiles[pos.Row, pos.Column].Background = opened;
            if (mineCount > 0)
                tiles[pos.Row, pos.Column].Content = mineCount.ToString();
            else
                tiles[pos.Row, pos.Column].Disable();
        }

        private void FlagTile(Position pos, bool state)
        {
            tiles[pos.Row, pos.Column].Background = state ? flagged : empty;
        }

        private void Explode(Position[] mines)
        {
            DisableAllTiles();
            foreach (var pos in mines)
                tiles[pos.Row, pos.Column].Background = failed;

            // TODO+: сделать взрыв мин в рандомном порядке и с небольшой задержкой
        }

        private void DisableAllTiles()
        {
            foreach (var tile in tiles)
                tile.Disable();
        }
    }
}
