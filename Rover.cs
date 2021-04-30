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

        private static int GetHeuristicPathLength(Point from, Point to)
        {
            throw new NotImplementedException();
            //todo approximate distance estimate
        }

        private static List<Point> GetPath(MyPoint path)
        {
            throw new NotImplementedException();
            //todo calculation of the optimal route
        }

        private static List<MyPoint> GetNeighbours(MyPoint pathNode,
            Point goal,
            int[,] field)
        {
            throw new NotImplementedException();
            //todo look around current point and getting list of neighbours

        }
        private static int GetCostTransitionToNeighbours()
        {
            return 1;//todo calculate the cost of the transition
        }

    }
}