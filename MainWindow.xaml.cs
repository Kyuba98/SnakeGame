using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeGame
{
    public partial class MainWindow : Window
    {
        private Snake _snake;
        private Apple _apple;
        private DispatcherTimer _gameTimer;
        private bool _isGameOver;
        private int gameSpeed = 200;

        public MainWindow()
        {
            InitializeComponent();
            BackgroundMusic.Volume = 0.2;
            BackgroundMusic.Play();

            _snake = new Snake();
            _apple = new Apple();
            _apple.GenerateNewPosition(_snake.Body);

            Loaded += MainWindow_Loaded;
            this.KeyDown += MainWindow_KeyDown;
            BackgroundMusic.MediaEnded += BackgroundMusic_MediaEnded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Calculate the window size based on the grid size (25x25)
            const int gridSize = 25;
            const int cellSize = 20; // Adjust cell size as needed

            int windowWidth = gridSize * cellSize;
            int windowHeight = gridSize * cellSize;

            // Set the window size
            Width = windowWidth;
            Height = windowHeight;

            GameCanvas.Width = windowWidth;
            GameCanvas.Height = windowHeight;

            // Ensure that the cells fit within the window size
            AdjustCellSizes(cellSize);
            StartGame();
        }

        private void StartGame()
        {
            gameSpeed = 200;
            _isGameOver = false;
            _gameTimer = new DispatcherTimer();
            _gameTimer.Tick += GameTick;
            _gameTimer.Interval = new System.TimeSpan(0, 0, 0, 0, 200); // Adjust speed here
            _gameTimer.Start();
        }

        private void AdjustCellSizes(int cellSize)
        {
            foreach (var element in GameCanvas.Children)
            {
                if (element is Rectangle)
                {
                    Rectangle rectangle = element as Rectangle;
                    rectangle.Width = cellSize;
                    rectangle.Height = cellSize;
                }
            }
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    ChangeDirection(Direction.Up);
                    break;
                case Key.S:
                    ChangeDirection(Direction.Down);
                    break;
                case Key.A:
                    ChangeDirection(Direction.Left);
                    break;
                case Key.D:
                    ChangeDirection(Direction.Right);
                    break;
                default:
                    break;
            }
        }
        private void ChangeDirection(Direction newDirection)
        {
            // Change the snake's direction if the new direction is valid
            // For example, prevent the snake from immediately turning back on itself
            if (IsOppositeDirection(newDirection, _snake.Direction))
                return;

            _snake.Direction = newDirection;
        }
        private bool IsOppositeDirection(Direction newDirection, Direction currentDirection)
        {
            return (newDirection == Direction.Up && currentDirection == Direction.Down) ||
                   (newDirection == Direction.Down && currentDirection == Direction.Up) ||
                   (newDirection == Direction.Left && currentDirection == Direction.Right) ||
                   (newDirection == Direction.Right && currentDirection == Direction.Left);
        }

        private void GameTick(object sender, System.EventArgs e)
        {
            if (!_isGameOver)
            {
                // Logic for moving the snake, checking collisions, etc.
                _snake.Move(25);
                CheckCollisions();
                UpdateUI();
            }            
        }

        private void CheckCollisions()
        {
            // Logic to check if the snake collides with the apple
            // If collision happens, grow the snake and generate a new apple position
            if (_snake.Body[0] == _apple.Position)
            {
                if (gameSpeed > 100) gameSpeed -= 5;
                _snake.Grow();
                _apple.GenerateNewPosition(_snake.Body);
                EatSound.Position = TimeSpan.Zero;
                EatSound.Play();

                // Draw the new apple after it's generated
                DrawElement(_apple.Position, Brushes.Red);
                _gameTimer.Interval = TimeSpan.FromMilliseconds(gameSpeed);
            }

            // Check collision with itself
            for (int i = 1; i < _snake.Body.Count; i++)
            {
                if (_snake.Body[i] == _snake.Body[0])
                {
                    GameOver();
                    break;
                }
            }
        }

        private void GameOver()
        {
            _isGameOver = true;
            _gameTimer.Stop();

            GameOverSound.Position = TimeSpan.Zero;
            GameOverSound.Play();

            GameOverText.Text = $"Game Over! Apples eaten: {_snake.Body.Count - 3}";
            GameOverText.Visibility = Visibility.Visible;
            // Show the buttons after game over
            ButtonsPanel.Visibility = Visibility.Visible;
        }

        private void UpdateUI()
        {

            // Clear previous UI elements
            GameCanvas.Children.Clear();


            // Redraw the regular apple
            DrawElement(_apple.Position, Brushes.Red);

            // Redraw the snake
            foreach (var segment in _snake.Body)
            {
                DrawElement(segment, Brushes.Green);
            }
        }



        private void DrawElement(Point position, Brush color)
        {
            if (color == Brushes.Red) // Check for the apple color
            {
                var rectangle = new Rectangle
                {
                    Width = 20,
                    Height = 20,
                };

                ImageBrush appleBrush = new ImageBrush();
                appleBrush.ImageSource = new BitmapImage(new Uri(@"Assets\apple.png", UriKind.Relative));

                rectangle.Fill = appleBrush;
                Canvas.SetLeft(rectangle, position.X * 20); // Scale the position to fit the grid
                Canvas.SetTop(rectangle, position.Y * 20); // Scale the position to fit the grid

                GameCanvas.Children.Add(rectangle);
            }
            else // For other elements (like the snake), keep the existing rectangle creation
            {
                var rectangle = new Rectangle
                {
                    Width = 20,
                    Height = 20,
                    Fill = color
                };

                Canvas.SetLeft(rectangle, position.X * 20); // Scale the position to fit the grid
                Canvas.SetTop(rectangle, position.Y * 20); // Scale the position to fit the grid

                GameCanvas.Children.Add(rectangle);
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset the game
            // Implement the logic to reset the game (reinitialize snake, apple, etc.)
            // For example:
            _snake = new Snake();
            _apple = new Apple();
            _apple.GenerateNewPosition(_snake.Body);

            // Hide buttons and reset game over text
            ButtonsPanel.Visibility = Visibility.Hidden;
            GameOverText.Visibility = Visibility.Hidden;

            // Restart the game loop
            StartGame();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the game window
            this.Close();
        }
        private void BackgroundMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Reset the position of the MediaElement to the beginning and play it again
            BackgroundMusic.Position = TimeSpan.Zero;
            BackgroundMusic.Play();
        }

    }
}
