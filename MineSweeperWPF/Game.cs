using System;
using System.Collections.Generic;
using System.Linq;

namespace MineSweeperWPF
{
    public class Game
    {
        private readonly Field field;
        private readonly FieldState state;
        private int openLeft;
        private bool first;

        public event Action Start, Win, Lose;
        public event Action<Position, int> CellOpened;
        public event Action<Position, bool> FlagStateChanged;

        public int MinesLeft { get; private set; }
        public IEnumerable<Position> MinePositions => field.MinePositions();

        public Game(Settings settings)
        {
            var (rows, columns, mines) = settings;
            field = new Field(rows, columns, mines);
            state = new FieldState(rows, columns);
            MinesLeft = mines;
            openLeft = rows * columns - mines;
            first = true;
        }        

        public void HandleOpen(Position pos)
        {
            if (state.IsFlagged(pos)) return;
            if (state.IsOpened(pos)) 
                OpenNearbyOf(pos);
            else
            {
                if (first)
                {
                    field.Init(pos);
                    Start?.Invoke();
                    first = false;
                }
                Open(pos);
            }
        }

        public void Flag(Position pos)
        {
            if (state.IsOpened(pos)) return;
            state.ChangeFlagState(pos);
            var isFlagged = state.IsFlagged(pos);
            MinesLeft += isFlagged ? -1 : 1;
            FlagStateChanged?.Invoke(pos, isFlagged);
        }

        private void Open(Position pos)
        {
            if (field.IsMine(pos))
            {
                Lose?.Invoke();
            }
            else
            {
                state.Open(pos);
                CellOpened?.Invoke(pos, field[pos]);

                if (field[pos] == 0)
                {
                    foreach (var near in field.NearbyOf(pos))
                    {
                        if (!state.IsOpened(near))
                            Open(near);
                    }
                }
                if (--openLeft == 0)
                {
                    Win?.Invoke();
                }
            }
        }

        private void OpenNearbyOf(Position pos)
        {
            if (field[pos] == 0) return;
            var nearby = field.NearbyOf(pos).ToArray();
            if (nearby.Count(p => state.IsFlagged(p)) != field[pos]) return;
            foreach (var p in nearby)
            {
                if (!state.IsOpened(p) && !state.IsFlagged(p))
                    Open(p);
            }
        }
    }
}