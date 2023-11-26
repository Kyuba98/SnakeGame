using System;
using System.Collections.Generic;
using System.Windows;

namespace SnakeGame
{
    public class Apple
    {
        public Point Position { get; private set; }

        private readonly Random _random;

        public Apple()
        {
            _random = new Random();
        }

        public void GenerateNewPosition(List<Point> snakeBody)
        {
            bool validPosition = false;

            // Keep generating new positions until a valid position is found
            while (!validPosition)
            {
                int x = _random.Next(0, 25); // Range: 0-24 (25x25 grid)
                int y = _random.Next(0, 25);
                Position = new Point(x, y);

                // Check if the new apple position doesn't overlap with the snake's body
                validPosition = true;
                foreach (var segment in snakeBody)
                {
                    if (Position == segment)
                    {
                        validPosition = false;
                        break;
                    }
                }
            }
        }
    }
}
