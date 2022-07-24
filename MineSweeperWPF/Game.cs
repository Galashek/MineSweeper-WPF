using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MineSweeperWPF
{
    public class Game
    {
        private readonly int rows, columns, mines;
        private readonly Field grid;
        private readonly bool[,] tileOpened;
        private readonly bool[,] tileFlagged;
        private int openedCount;
        private bool first;
        public int MinesLeft { get; private set; }

        public event Action FirstTileOpen;
        public event Action<Position, int> TileOpen;
        public event Action<Position, bool> FlagSet;
        public event Action AllTilesOpened;
        public event Action<IEnumerable<Position>> MineOpen;
        public event Action Failed;

        public Game(Settings settings)
        {
            (rows, columns, mines) = settings;
            grid = new Field(rows, columns, mines);
            tileOpened = new bool[rows, columns];
            tileFlagged = new bool[rows, columns];
            MinesLeft = mines;
            openedCount = 0;
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
                    grid.Init(pos);
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
                    foreach (var otherPos in grid.NearbyOf(pos))
                    {
                        if (!tileOpened[otherPos.Row, otherPos.Column])
                            SetOpened(otherPos);
                    }
                }

                if (++openedCount == rows * columns - mines)
                {
                    AllTilesOpened?.Invoke();
                }
            }
        }

        private void SetNearOpened(Position pos)
        {
            if (grid[pos] == 0) return;
            var nearby = grid.NearbyOf(pos).ToArray();
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
