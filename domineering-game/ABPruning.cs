using domineering_game.game;
using System;
using System.Collections.Generic;

namespace domineering_game
{
    public class AlBePruningAi
    {
        readonly int _level;
        readonly IPlayer _currPlayer;
        readonly IPlayer _enemyPlayer;

        private int H = int.MinValue;
        private int W = int.MinValue;

        int Depth => ((int)Math.Pow(2, _level));

        public AlBePruningAi(int level, IPlayer currentPlayer, IPlayer enemyPlayer)
        {
            _level = level;
            _currPlayer = currentPlayer;
            _enemyPlayer = enemyPlayer;
        }

        public int CalcBestMove(Board board, out int h, out int w)
        {
            int result = MiniMax(board, Depth, true, int.MinValue, int.MaxValue);
            h = H;
            w = W;
            return result;
        }

        public int MiniMax(Board board, int depth, bool IsMaxi, int alpha, int beta)
        {
            
            if (depth == 0)
                return Heuristic(board);

            int best;
            if (IsMaxi)
            {
                best = int.MinValue;

                //Формирование возможных ходов
                var moves = board.HaveAvailableCells(_currPlayer.MoveType);
                if (moves.Count == 0)
                    return Heuristic(board);

                //Проход по вариантам с расчётом 
                foreach (var move in moves)
                {   
                    //Создание доски
                    Board childBoard = (Board)board.Clone();

                    //Получение результата хода
                    bool resultMove = childBoard.Move(_currPlayer.Id, move.Item1, move.Item2, _currPlayer.MoveType);
                    if (!resultMove)
                        continue;

                    int value = MiniMax(childBoard, depth - 1, !IsMaxi, alpha, beta);
                    best = Math.Max(best, value);
                    //Определение наибольшего звена
                    if (best > alpha)
                    {
                        H = move.Item1;
                        W = move.Item2;
                        alpha = best;
                    }    
                    if (beta <= alpha)
                        break;
                }
            }
            else
            {
                best = int.MaxValue;

                //Формирование возможных ходов
                var moves = board.HaveAvailableCells(_enemyPlayer.MoveType);
                if (moves.Count == 0)
                    return Heuristic(board);

                //Проход по вариантам с расчётом 
                foreach (var move in moves)
                {   
                    //Создание доски
                    Board childBoard = (Board)board.Clone();

                    //Получение результата хода
                    bool resultMove = childBoard.Move(_enemyPlayer.Id, move.Item1, move.Item2, _enemyPlayer.MoveType);
                    if (resultMove)
                    {
                        int value = MiniMax(childBoard, depth - 1, !IsMaxi, alpha, beta);
                        best = Math.Min(best, value);
                    }
                    else
                        best = int.MaxValue;

                    //Определение наименьшего звена
                    beta = Math.Min(beta, best);
                    if (beta <= alpha)
                        break;
                }
            }

            return best;
        }

        public int Heuristic(Board bState)
        {
            int result = 0;
            int iH, iW;
            (iH, iW) = bState.BoardSize;
            for (int h = 0; h < iH; h++)
                for (int w = 0; w < iW; w++)
                    if (bState.Cells[h][w] ==0)
                    {
                        if ((h != iH - 1) && bState.Cells[h + 1][w] == 0) //MoveType.Vertical
                            result++;
                        if ((w != iW - 1) && bState.Cells[h][w + 1] == 0) //MoveType.Horizontal
                            result--;
                    }

            //Инверсия если целевой игрок устанавливает Horizontal блоки
            return _currPlayer.MoveType == MoveType.Vertical ? result: -result;
        }
    }
}