using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
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
            var pathToWriteFile = @"C:\Users\USER\source\repos\TextFileForIntership\";
            var testVariable = new int[,]
            {
                {0, 4},
                {1, 3}
            };

            CalculateRoverPath(testVariable, pathToWriteFile);
        }

        private static void CalculateRoverPath(int[,] map, string path)
        {
            {
                var start = new Point(0, 0);
                var pointTo = new Point(map.GetUpperBound(0), map.GetUpperBound(1));
                var closedPoints = new Collection<MyPoint>();
                var openPoints = new Collection<MyPoint>();

                var startPoint = new MyPoint()
                {
                    Position = start,
                    PrewPoint = null,
                    FullPathLength = 0,
                    HeuristicPathLength = GetHeuristicPathLength(start, pointTo)
                };
                openPoints.Add(startPoint);

                while (openPoints.Count > 0)
                {
                    var prewPoint = openPoints.OrderBy(point =>
                        point.EstimatePath).First();

                    if (prewPoint.Position == pointTo)
                    {
                        var pathPointList = GetPath(prewPoint);

                        PrintPathToFile(pathPointList, path);
                    }

                    openPoints.Remove(prewPoint);
                    closedPoints.Add(prewPoint);

                    foreach (var neighbourPoint in GetNeighbours(prewPoint, pointTo, map))
                    {
                        if (closedPoints.Count(currPoint => currPoint.Position == neighbourPoint.Position) > 0)
                            continue;
                        var openNode = openPoints.FirstOrDefault(node =>
                            node.Position == neighbourPoint.Position);

                        if (openNode == null)
                            openPoints.Add(neighbourPoint);
                        else if (openNode.FullPathLength > neighbourPoint.FullPathLength)
                        {
                            openNode.PrewPoint = prewPoint;
                            openNode.FullPathLength = neighbourPoint.FullPathLength;
                        }
                    }
                }
            }
        }

        private static IEnumerable<MyPoint> GetNeighbours(MyPoint currPoint,
            Point target,
            int[,] field)
        {
            var result = new List<MyPoint>();

            var neighbours = new Point[4];
            neighbours[0] = new Point(currPoint.Position.X - 1, currPoint.Position.Y);
            neighbours[1] = new Point(currPoint.Position.X, currPoint.Position.Y - 1);
            neighbours[2] = new Point(currPoint.Position.X + 1, currPoint.Position.Y);
            neighbours[3] = new Point(currPoint.Position.X, currPoint.Position.Y + 1);

            foreach (var neighbour in neighbours)
            {
                if (neighbour.X < 0 || neighbour.X >= field.GetLength(0) ||
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

        private static List<MyPoint> GetPath(MyPoint path)
        {
            var result = new List<MyPoint>();
            var current = path;

            while (current != null)
            {
                result.Add(current);
                current = current.PrewPoint;
            }

            result.Reverse();
            return result;
        }

        private static int GetCostTransitionToNeighbours(int[,] field, MyPoint pointFrom, Point pointTo)
        {
            return 1 + Math.Abs(field[pointFrom.Position.X, pointFrom.Position.Y] - field[pointTo.X, pointTo.Y]);
        }

        private static void PrintPathToFile(List<MyPoint> points, string path)
        {
            using var sw = new StreamWriter(path + "path-plan.txt", false);
            {
                var fuel = 0;
                foreach (var point in points)
                {
                    sw.Write($"[{point.Position.X}][{point.Position.Y}]");
                    fuel = point.FullPathLength;
                    if (points.Last().Position != point.Position)
                    {
                        sw.Write("->");
                    }
                }

                sw.WriteLine($"\nsteps: {points.Count - 1}");
                sw.WriteLine($"fuel: {fuel}");
            }
        }
    }
}