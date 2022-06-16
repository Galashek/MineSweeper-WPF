using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MineSweeperWPF
{
    class GameState
    {
        private readonly int rows, columns, mines;
        private readonly Field grid;
        private readonly bool[,] tileOpened;
        private readonly bool[,] tileFlagged;
        private int openedCounter;
        private bool first;
        public int MinesLeft { get; private set; }

        public event Action FirstTileOpen;
        public event Action<Position, int> TileOpen;
        public event Action<Position, bool> FlagSet;
        public event Action AllTilesOpened;
        public event Action<Position[]> MineOpen;
        public event Action Failed;

        public GameState(Settings settings, MainWindow game)
        {
            (rows, columns, mines) = settings;
            grid = new Field(rows, columns, mines);
            tileOpened = new bool[rows, columns];
            tileFlagged = new bool[rows, columns];
            MinesLeft = mines;
            openedCounter = 0;
            first = true;
        }        

        public void HandleClick(Position pos, bool flagging)
        {
            if (flagging)
            {
                SetFlag(pos);
                return;
            }
            if (tileFlagged[pos.Row, pos.Column]) return;
            if (tileOpened[pos.Row, pos.Column]) SetNearOpened(pos);
            else
            {
                if (first)
                {
                    grid.CreateOnClick(pos.Row, pos.Column);
                    FirstTileOpen?.Invoke();
                    first = false;
                }
                SetOpened(pos);
            }
        }

        private void SetOpened(Position pos)
        {
            if (tileFlagged[pos.Row, pos.Column])
                SetFlag(pos);

            tileOpened[pos.Row, pos.Column] = true;

            if (grid[pos] == -1)
            {
                MineOpen?.Invoke(grid.MinePositions);
                Failed?.Invoke();
            }
            else
            {
                TileOpen?.Invoke(pos, grid[pos]);

                if (grid[pos] == 0)
                {
                    foreach (var otherPos in grid.FindNearby(pos))
                    {
                        if (!tileOpened[otherPos.Row, otherPos.Column])
                            SetOpened(otherPos);
                    }
                }

                if (++openedCounter == rows * columns - mines)
                {
                    AllTilesOpened?.Invoke();
                }
            }
        }

        private void SetNearOpened(Position pos)
        {
            if (grid[pos] == 0) return;
            var nearby = grid.FindNearby(pos);
            if (nearby.Count(x => tileFlagged[x.Row, x.Column]) != grid[pos]) return;
            foreach (var p in nearby)
            {
                if (!tileOpened[p.Row, p.Column] && !tileFlagged[p.Row, p.Column])
                    SetOpened(p);
            }
        }

        private void SetFlag(Position pos)
        {
            if (tileOpened[pos.Row, pos.Column]) return;
            var newState = !tileFlagged[pos.Row, pos.Column];
            tileFlagged[pos.Row, pos.Column] = newState;
            MinesLeft += newState ? -1 : 1;
            FlagSet?.Invoke(pos, newState);
        }
    }
}
