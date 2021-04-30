using System.Drawing;

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
}