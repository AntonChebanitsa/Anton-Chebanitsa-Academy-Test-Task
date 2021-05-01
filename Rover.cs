using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace Anton_Chebanitsa_Academy_Test_Task
{
    public class MyPoint
    {
        public Point Position { get; set; }

        public int FullPathLength { get; set; }

        public MyPoint PrewPoint { get; set; }

        public int HeuristicPathLength { get; set; }

        public int EstimatePath => FullPathLength + HeuristicPathLength;
    }

    public class Rover
    {
        private static void Main()
        {
            //CalculateRoverPath();
        }

        public static void CalculateRoverPath(int[,] map)
        {

        }

        public static List<Point> FindPath(int[,] map, Point start, Point target)
        {
            var closedPoints = new Collection<MyPoint>();
            var openPoints = new Collection<MyPoint>();

            var startPoint = new MyPoint()
            {
                Position = start,
                PrewPoint = null,
                FullPathLength = 0,
                HeuristicPathLength = GetHeuristicPathLength(start, target)
            };
            openPoints.Add(startPoint);

            while (openPoints.Count > 0)
            {
                var currentPoint = openPoints.OrderBy(point =>
                    point.EstimatePath).First();

                if (currentPoint.Position == target)
                    return GetPath(currentPoint);

                openPoints.Remove(currentPoint);
                closedPoints.Add(currentPoint);

                foreach (var neighbourPoint in GetNeighbours(currentPoint, target, map))
                {
                    if (closedPoints.Count(currPoint => currPoint.Position == neighbourPoint.Position) > 0)
                        continue;
                    var openNode = openPoints.FirstOrDefault(node =>
                        node.Position == neighbourPoint.Position);

                    if (openNode == null)
                        openPoints.Add(neighbourPoint);
                    else
                    if (openNode.FullPathLength > neighbourPoint.FullPathLength)
                    {
                        openNode.PrewPoint = currentPoint;
                        openNode.FullPathLength = neighbourPoint.FullPathLength;
                    }
                }
            }
            return null;
        }

        private static List<MyPoint> GetNeighbours(MyPoint currPoint,
            Point target,
            int[,] field)
        {
            var result = new List<MyPoint>();

            Point[] neighbours = new Point[4];
            neighbours[0] = new Point(currPoint.Position.X - 1, currPoint.Position.Y);
            neighbours[1] = new Point(currPoint.Position.X, currPoint.Position.Y - 1);
            neighbours[2] = new Point(currPoint.Position.X + 1, currPoint.Position.Y);
            neighbours[3] = new Point(currPoint.Position.X, currPoint.Position.Y + 1);

            foreach (var neighbour in neighbours)
            {
                if (neighbour.X < 0 || neighbour.X >= field.GetLength(0) &&
                    (neighbour.Y < 0 || neighbour.Y >= field.GetLength(1)))
                    continue;
                var newPoint = new MyPoint()
                {
                    Position = neighbour,
                    PrewPoint = currPoint,
                    FullPathLength = currPoint.FullPathLength +
                                     GetCostTransitionToNeighbours(field, currPoint, neighbour),
                    HeuristicPathLength = GetHeuristicPathLength(neighbour, target)
                };
                result.Add(newPoint);
            }
            return result;
        }

        private static int GetHeuristicPathLength(Point pointFrom, Point pointTo)
        {
            return Math.Abs(pointFrom.X - pointTo.X) + Math.Abs(pointFrom.Y - pointTo.Y);
        }

        private static List<Point> GetPath(MyPoint path)
        {
            throw new NotImplementedException();
            //todo calculation of the optimal route
        }

        private static int GetCostTransitionToNeighbours(int[,] field, MyPoint pointFrom, Point pointTo)
        {
            return 1 + Math.Abs(field[pointFrom.Position.X, pointFrom.Position.Y] - field[pointTo.X, pointTo.Y]);
        }

    }
}