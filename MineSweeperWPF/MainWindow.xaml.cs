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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int rows, columns, mines;
        private double tileSize = 40;
        //public GameObject tilePrefab;
        //public Text minesCountLabel;

        private GameState gameState;
        private GameView gameView;

        public event Action<Position> OpenClick;
        public event Action<Position> FlagClick;

        //public Timer timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            rows = Settings.Rows;
            columns = Settings.Columns;
            mines = Settings.Mines;

            field.Rows = rows;
            field.Columns = columns;
            field.Children.Clear();

            ResizeWindow();
            minesCountLabel.Content = mines.ToString();
            gameView = new GameView();

            gameState = new GameState(rows, columns, mines, this);
            gameState.FirstTileOpen += StartGame;
            gameState.AllTilesOpened += Win;
            gameState.Failed += Lose;
            gameState.FlagSet += (x, y) => minesCountLabel.Content = gameState.MinesLeft.ToString();

            var tiles = new Tile[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    //var tile = Instantiate(tilePrefab, transform);
                    //tile.name = $"{i} {j}";
                    //tiles[i, j] = tile.GetComponent<Tile>();
                    //tiles[i, j].Position = new Position(i, j);
                    //tiles[i, j].LeftClick += (pos) => OpenClick?.Invoke(pos);
                    //tiles[i, j].RightClick += (pos) => FlagClick?.Invoke(pos);
                    var tile = new Tile();
                    //tile.Content = $"{i} {j}";
                    //tile.Margin = new Thickness(3, 3, 3, 3);
                    
                    tile.Position = new Position(i, j);
                    tile.LeftClick += (pos) => OpenClick?.Invoke(pos);
                    tile.RightClick += (pos) => FlagClick?.Invoke(pos);

                    tiles[i, j] = tile;
                    
                    field.Children.Add(tile);
                }
            }
            //GetComponent<GameView>().Initialize(tiles, gameState);            
            gameView.Initialize(tiles, gameState);
        }

        private void StartGame()
        {
            //timer.StartTimer();
            //Debug.Log("Game started!");
        }

        private void Win()
        {
            //timer.StopTimer();
            //Debug.Log("You won!");
        }

        private void Lose()
        {
            //timer.StopTimer();
            //Debug.Log("You lose!");
        }

        public void LoadScene(int id)
        {
            //UnityEngine.SceneManagement.SceneManager.LoadScene(id);
        }


        public void ResizeWindow()
        {
            window.Height = (field.Rows * tileSize) + menu.Height + gamePanel.Height;
            window.Width = field.Columns * tileSize;

            //field.Height = field.Rows * tileSize;
            //field.Width = field.Columns * tileSize;
            //window.Width = field.Width;
            //window.Height = field.Height + menu.Height + gamePanel.Height;


            //var rect = GetComponent<RectTransform>();
            //var grid = GetComponent<GridLayoutGroup>();

            //grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            //grid.constraintCount = columns;

            //var width = columns * grid.cellSize.x + (columns - 1) * grid.spacing.x + 10 * grid.padding.left;
            //var height = rows * grid.cellSize.y + (rows - 1) * grid.spacing.y + 10 * grid.padding.top;

            //rect.sizeDelta = new Vector2(width, height);
        }
    }
}
