//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MineSweeperWPF
//{
//    class Game
//    {
//        private int rows, columns, mines;
//        //public GameObject tilePrefab;
//        //public Text minesCountLabel;

//        private GameState gameState;

//        public event Action<Position> OpenClick;
//        public event Action<Position> FlagClick;

//        //public Timer timer;

//        public Game()
//        {
//            rows = Settings.Rows;
//            columns = Settings.Columns;
//            mines = Settings.Mines;
            
//            //ScaleGrid();
//            //minesCountLabel.text = mines.ToString();

//            gameState = new GameState(rows, columns, mines, this);
//            gameState.FirstTileOpen += StartGame;
//            gameState.AllTilesOpened += Win;
//            gameState.Failed += Lose;
//            gameState.FlagSet += (x, y) => minesCountLabel.text = gameModel.MinesLeft.ToString();

//            var tiles = new Tile[rows, columns];
//            for (int i = 0; i < rows; i++)
//            {
//                for (int j = 0; j < columns; j++)
//                {
//                    //var tile = Instantiate(tilePrefab, transform);
//                    //tile.name = $"{i} {j}";
//                    //tiles[i, j] = tile.GetComponent<Tile>();
//                    //tiles[i, j].Position = new Position(i, j);
//                    //tiles[i, j].LeftClick += (pos) => OpenClick?.Invoke(pos);
//                    //tiles[i, j].RightClick += (pos) => FlagClick?.Invoke(pos);
//                    var tile = new Tile();
//                    tile.Content = $"{i} {j}";
//                    //field.Children.Add(tile);
//                }
//            }

//            //GetComponent<GameView>().Initialize(tiles, gameState);            
//        }

//        private void StartGame()
//        {
//            //timer.StartTimer();
//            //Debug.Log("Game started!");
//        }

//        private void Win()
//        {
//            //timer.StopTimer();
//            //Debug.Log("You won!");
//        }

//        private void Lose()
//        {
//            //timer.StopTimer();
//            //Debug.Log("You lose!");
//        }

//        public void LoadScene(int id)
//        {
//            //UnityEngine.SceneManagement.SceneManager.LoadScene(id);
//        }


//        public void ScaleGrid()
//        {
//            //var rect = GetComponent<RectTransform>();
//            //var grid = GetComponent<GridLayoutGroup>();

//            //grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
//            //grid.constraintCount = columns;

//            //var width = columns * grid.cellSize.x + (columns - 1) * grid.spacing.x + 10 * grid.padding.left;
//            //var height = rows * grid.cellSize.y + (rows - 1) * grid.spacing.y + 10 * grid.padding.top;

//            //rect.sizeDelta = new Vector2(width, height);
//        }
//    }
//}
