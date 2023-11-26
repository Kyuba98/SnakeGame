using System.Collections.Generic;
using System.Windows;

namespace SnakeGame
{
    public class Snake
    {
        public List<Point> Body { get; private set; }
        public Direction Direction { get; set; }

        public Snake()
        {
            Body = new List<Point>
            {
                new Point(10, 10), // Initial position
                new Point(10, 11),
                new Point(10, 12)
            };
            Direction = Direction.Up; // Initial direction
        }

        public void Move(int grindSize)
        {
            Point head = Body[0];
            Point newHead = CalculateNewHeadPosition(head, grindSize);

            Body.Insert(0, newHead);
            Body.RemoveAt(Body.Count - 1);
        }

        private Point CalculateNewHeadPosition(Point currentHead, int gridSize)
        {
            Point newHead = new Point(currentHead.X, currentHead.Y);

            switch (Direction)
            {
                case Direction.Up:
                    newHead.Y = (currentHead.Y - 1 + gridSize) % gridSize;
                    break;
                case Direction.Down:
                    newHead.Y = (currentHead.Y + 1) % gridSize;
                    break;
                case Direction.Left:
                    newHead.X = (currentHead.X - 1 + gridSize) % gridSize;
                    break;
                case Direction.Right:
                    newHead.X = (currentHead.X + 1) % gridSize;
                    break;
            }

            return newHead;
        }

        public void Grow()
        {
            Point tail = Body[Body.Count - 1];
            Point newTail = CalculateNewTailPosition(tail);
            Body.Add(newTail);
        }

        private Point CalculateNewTailPosition(Point currentTail)
        {
            // You can define the logic for the new tail position here if needed
            return currentTail;
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
