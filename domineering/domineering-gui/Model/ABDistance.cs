using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domineering_gui.Model
{
    public class ABDistance
    {
        public Board boardState { get; set; }

        int _depth;

        public int H { get; private set; }
        public int W { get; private set; }

        public ABDistance(int depth) => _depth = depth;
        public int Heuristic(Board boardState)
        {
            int result = 0;
            int iH, iW;
            (iH, iW) = boardState.BoardSize;
            for (int h = 0; h < iH; h++)
                for (int w = 0; w < iW; w++)
                {
                    if (boardState.Cells[h][w].Active)
                    {
                        if ((h != iH - 1) && boardState.Cells[h + 1][w].Active)
                            result++;
                        if ((w != iW - 1) && boardState.Cells[h][w + 1].Active)
                            result--;
                    }
                }
            return result;
        }

        public int MinMax(Board boardState, int depth, bool maximizing)
        {
            int value = int.MaxValue;

            if (depth == 0 || Heuristic(boardState) == 0)
                return Heuristic(boardState);

            var neig = boardState.GetAllChild(maximizing);
            foreach(var child in neig)
                value = Math.Min(value + 1, MaxMin(child, depth - 1, !maximizing));

            return value;
        }

        private int MaxMin(Board boardState, int depth,bool maximizing)
        {
            int value = int.MinValue;
            if (depth == 0 || Heuristic(boardState) == 0)
                return Heuristic(boardState);

            var neig = boardState.GetAllChild(maximizing);
            foreach (var child in neig)
                value = Math.Max(value + 1, MinMax(child, depth - 1,!maximizing));
            return value;

        }

        // https://github.com/Flyboy1010/ChessAI/blob/main/Scripts/AI.cs
        //                depth--;
        //maximizing = !maximizing;

        //return matrixState[];

        //if (maximizing)
        //{
        //    int best = MIN;

        //    Recur for left and

        //    right children
        //                for (int i = 0; i < 2; i++)
        //        {
        //            int val = minimax(depth + 1, nodeIndex * 2 + i,
        //                            false, values, alpha, beta);
        //            best = Math.Max(best, val);

        //        }
        //    return best;
        //}
        //else
        //{
        //    int best = MAX;

        //    Recur for left and

        //    right children
        //                for (int i = 0; i < 2; i++)
        //        {

        //            int val = minimax(depth + 1, nodeIndex * 2 + i,
        //                            true, values, alpha, beta);
        //            best = Math.Min(best, val);
        //            beta = Math.Min(beta, best);

        //            Alpha Beta Pruning
        //                    if (beta <= alpha)
        //                break;
        //        }
        //    return best;
        //}
        //alpha = Math.Max(alpha, best);

        //// Alpha Beta Pruning
        //if (beta <= alpha)
        //    break;
        //int best = int.MinValue;
        //for (int i = 0; i < 2; i++)
        //{
        //    int val = Calculate(depth + 1, nodeIndex * 2 + i,
        //                    maximizing, matrixState, a, b);
        //    best = Math.Max(best, val);

        //}
        //return best;
        //            }




        //static int minimax(int depth, int nodeIndex,
        //         bool maximizing,
        //         int[] v, int alpha,
        //         int beta)
        //{
        //    // Terminating condition. i.e
        //    // leaf node is reached
        //    if (depth == 3)
        //        return values[nodeIndex];

        //    if (maximizing)
        //    {
        //        int best = MIN;

        //        // Recur for left and
        //        // right children
        //        for (int i = 0; i < 2; i++)
        //        {
        //            int val = minimax(depth + 1, nodeIndex * 2 + i,
        //                            false, values, alpha, beta);
        //            best = Math.Max(best, val);

        //        }
        //        return best;
        //    }
        //    else
        //    {
        //        int best = MAX;

        //        // Recur for left and
        //        // right children
        //        for (int i = 0; i < 2; i++)
        //        {

        //            int val = minimax(depth + 1, nodeIndex * 2 + i,
        //                            true, values, alpha, beta);
        //            best = Math.Min(best, val);
        //            beta = Math.Min(beta, best);

        //            // Alpha Beta Pruning
        //            if (beta <= alpha)
        //                break;
        //        }
        //        return best;
        //    }
        //}
    }
}

