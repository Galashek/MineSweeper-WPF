using System;
using System.Windows;
using System.Windows.Threading;
using MineSweeper;

namespace MineSweeperWPF
{
    public partial class MainWindow : Window
    {
        private Game game;
        private Settings currentSettings;
        private DispatcherTimer timer;
        private int elapsedTime;
        private Position lastOpened;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => UpdateTimer();

            view.TileFlag += (pos) => game.Flag(pos);
            view.TileOpen += (pos) =>
            {
                lastOpened = pos;
                game.HandleOpen(pos);
            };

            CreateGame(Settings.Easy);
        }

        private void CreateGame(Settings settings)
        {
            currentSettings = settings;
            var (rows, columns, mines) = settings;

            timer.Stop();
            elapsedTime = 0;
            UpdateTimer();
            UpdateMineCounter(mines);
            gamePanel.Style = Resources["defaultPanel"] as Style;

            view.Initialize(rows, columns);
            ResizeWindow(rows, columns);

            game = new Game(currentSettings);
            game.Start += StartGame;
            game.Win += Win;
            game.Lose += Lose;
            game.CellOpened += (p, m) => view.OpenTile(p, m);
            game.FlagStateChanged += (p, s) => UpdateMineCounter(game.MinesLeft);
            game.FlagStateChanged += (p, s) => view.FlagTile(p, s);
        }

        private void StartGame()
        {               
            timer.Start();      
        }

        private void Win()
        {
            timer.Stop();
            view.DisableAllTiles();
            view.MarkFlags(game.MinePositions);
            UpdateMineCounter(0);
            gamePanel.Style = Resources["winPanel"] as Style;
        }

        private void UpdateTimer()
        {
            timerText.Text = elapsedTime > 999 ?
                "999" : elapsedTime.ToString().PadLeft(3, '0');
            elapsedTime++;
        }

        private void UpdateMineCounter(int count)
        {
            minesCount.Text = count < 0 ?
                "000" : count.ToString().PadLeft(3, '0');
        }

        private void Lose()
        {
            timer.Stop();
            view.DisableAllTiles();
            view.MarkMines(game.MinePositions, lastOpened);
            gamePanel.Style = Resources["losePanel"] as Style;
        }

        private void Restart(object s, RoutedEventArgs e)
            => CreateGame(currentSettings);

        private void StartEasyGame(object s, RoutedEventArgs e)
            => CreateGame(Settings.Easy);

        private void StartMediumGame(object s, RoutedEventArgs e)
            => CreateGame(Settings.Medium);

        private void StartHardGame(object s, RoutedEventArgs e)
            => CreateGame(Settings.Hard);

        private void Exit(object s, RoutedEventArgs e)
            => Application.Current.Shutdown();

        private void StartCustomGame(object s, RoutedEventArgs e)
        {
            var settingsWindow = new CustomSettingsWindow { Owner = this };
            if (settingsWindow.ShowDialog() == true)
                CreateGame(settingsWindow.Settings);
        }

        public void ResizeWindow(int rows, int columns)
        {
            var tileSize = 40d;
            window.Height = (rows * tileSize) + menu.Height + gamePanel.Height;
            window.Width = columns * tileSize;
        }
    }
}
