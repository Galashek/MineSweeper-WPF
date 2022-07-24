namespace MineSweeperWPF
{
    public class Settings
    {
        public int Rows { get; }
        public int Columns { get; }
        public int Mines { get; }
                
        public Settings(int rows, int columns, int mines)
        {
            if (rows <= 0) rows = 1;
            if (columns <= 0) columns = 1;
            if (mines <= 0) mines = 1;
            if (mines > rows * columns - 1) mines = rows * columns - 1;
            (Rows, Columns, Mines) = (rows, columns, mines);
        }

        public void Deconstruct(out int rows, out int columns, out int mines)
        {
            (rows, columns, mines) = (Rows, Columns, Mines);
        }

        public static Settings Easy => new(9, 9, 12);
        public static Settings Medium => new(16, 16, 45);
        public static Settings Hard => new(16, 30, 99);
    }
}
