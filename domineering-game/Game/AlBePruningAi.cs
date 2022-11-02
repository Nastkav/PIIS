using domineering_game.game;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace domineering_game.game
{
    public class AlBePruningAi
    {
        int _level;
        readonly IPlayer _currPlayer;
        readonly IPlayer _enemyPlayer;
        public int Level { get => _level; set { _level = value; } }

        public AlBePruningAi(int level, IPlayer currentPlayer, IPlayer enemyPlayer)
        {
            _level = level;
            _currPlayer = currentPlayer;
            _enemyPlayer = enemyPlayer;
        }

        public int CalcBestMove(Board board, out int h, out int w)
        {
            //int result = MiniMaxWithout(board, _level, out (int, int) pos);
            //int result = MiniMax(board, _level, int.MinValue, int.MaxValue,out (int,int) pos);
            int turnMultiplier = 1;
            int result = NegaMaxWithout(board, _level, out (int, int) pos, turnMultiplier);
            //int result = NegaMax(board, _level, int.MinValue, int.MaxValue, out (int, int) pos, turnMultiplier);
            //int result = NegaScout(board, _level, int.MinValue, int.MaxValue, out (int, int) pos, turnMultiplier);
            h = pos.Item1;
            w = pos.Item2;
            return result;
        }

        public int NegaScout(Board board, int depth, int alpha, int beta, out (int, int) pos, int turnMultiplier)
        {
            pos = (int.MinValue, int.MinValue);
            if (depth == 0)
                return turnMultiplier * Heuristic(board, _currPlayer.MoveType);

            int best = int.MinValue; //+

            int n = beta;

            //Формирование возможных ходов
            var moves = board.HaveAvailableCells(_currPlayer.MoveType);
            if (moves.Count == 0)
                return Heuristic(board, _currPlayer.MoveType);

            foreach (var move in moves)
            {
                Board tmpchildBoard = (Board)board.Clone();

                //Получение результата хода
                tmpchildBoard.Move(_currPlayer.Id, move.Item1, move.Item2, _currPlayer.MoveType);

                int value = -NegaScout(tmpchildBoard, depth - 1, -alpha, -n, out (int, int) tmpchildPos, -turnMultiplier);


                if (value > best)
                {
                    if (alpha < value && value < beta)
                    {
                        pos.Item1 = move.Item1;
                        pos.Item2 = move.Item2;
                        best = Math.Max(value, best);
                    } 
                    else
                    {
                        Board childBoard = (Board)tmpchildBoard.Clone();

                        //Получение результата хода
                        childBoard.Move(_currPlayer.Id, move.Item1, move.Item2, _currPlayer.MoveType);
                        best = -NegaScout(tmpchildBoard, depth - 1, -value, -beta, out (int, int) childPos, turnMultiplier);
                    }
                }


                alpha = Math.Max(alpha, best);

                // Отсечение
                if (beta <= alpha)
                    break;

                n = alpha + 1;
            }
            return best;
        }



            public int NegaMax(Board board, int depth, int alpha, int beta, out (int,int) pos, int turnMultiplier)
        {
            pos = (int.MinValue, int.MinValue);
            if (depth == 0)
                return turnMultiplier * Heuristic(board, _currPlayer.MoveType);

            int best = int.MinValue; //+

            //Формирование возможных ходов
            var moves = board.HaveAvailableCells(_currPlayer.MoveType);
            if (moves.Count == 0)
                return Heuristic(board, _currPlayer.MoveType);

            foreach(var move in moves)
            {
                Board childBoard = (Board)board.Clone();

                //Получение результата хода
                childBoard.Move(_currPlayer.Id, move.Item1, move.Item2, _currPlayer.MoveType);

                int value = -NegaMax(childBoard, depth - 1, -alpha, -beta, out (int, int) childPos, -turnMultiplier);

                if (value > best)
                {
                    pos.Item1 = move.Item1;
                    pos.Item2 = move.Item2;
                    best = value;
                }
                alpha = Math.Max(alpha, best);

                // Отсечение
                if (beta <= alpha)
                    break;
            }
            return best;

        }

        public int NegaMaxWithout(Board board, int depth, out (int, int) pos, int turnMultiplier)
        {
            pos = (int.MinValue, int.MinValue);
            if (depth == 0)
                return turnMultiplier * Heuristic(board, _currPlayer.MoveType);

            int best = int.MinValue; //+

            //Формирование возможных ходов
            var moves = board.HaveAvailableCells(_currPlayer.MoveType);
            if (moves.Count == 0)
                return Heuristic(board, _currPlayer.MoveType);

            foreach (var move in moves)
            {
                Board childBoard = (Board)board.Clone();

                //Получение результата хода
                childBoard.Move(_currPlayer.Id, move.Item1, move.Item2, _currPlayer.MoveType);

                int value = -NegaMaxWithout(childBoard, depth - 1, out (int, int) childPos, -turnMultiplier);

                if (value > best)
                {
                    pos.Item1 = move.Item1;
                    pos.Item2 = move.Item2;
                    best = value;
                }
            }
            return best;

        }



        public int MiniMax(Board board, int depth, int alpha, int beta, out (int, int) pos)
        {
            pos = (int.MinValue, int.MinValue);
            if (depth == 0)
                return Heuristic(board, _currPlayer.MoveType);

            int best = int.MinValue;

                //Формирование возможных ходов
                var moves = board.HaveAvailableCells(_currPlayer.MoveType);
                if (moves.Count == 0)
                    return Heuristic(board, _currPlayer.MoveType);

                //Проход по вариантам с расчётом 
                foreach (var move in moves)
                {
                    //Создание доски
                    Board childBoard = (Board)board.Clone();

                    //Получение результата хода
                    childBoard.Move(_currPlayer.Id, move.Item1, move.Item2, _currPlayer.MoveType);

                    int value = MaxiMin(childBoard, depth - 1, alpha, beta, out (int, int) childPos);

                    //Определение наибольшего звена
                    if (value > best)
                    {
                        pos.Item1 = move.Item1;
                        pos.Item2 = move.Item2;
                        best = value;
                    }
                    alpha = Math.Max(alpha, best);

                    // Отсечение
                    if (beta <= alpha)
                        break;
                }
            return best;
        }
        public int MaxiMin(Board board, int depth, int alpha, int beta, out (int, int) pos)
        {
            pos = (int.MinValue, int.MinValue);
            if (depth == 0)
                return Heuristic(board, _currPlayer.MoveType);

            int best = int.MaxValue;

                //Формирование возможных ходов
                var moves = board.HaveAvailableCells(_enemyPlayer.MoveType);
                if (moves.Count == 0)
                    return Heuristic(board, _currPlayer.MoveType);

                //Проход по вариантам с расчётом 
                foreach (var move in moves)
                {
                    //Создание доски
                    Board childBoard = (Board)board.Clone();

                    //Получение результата хода
                    childBoard.Move(_enemyPlayer.Id, move.Item1, move.Item2, _enemyPlayer.MoveType);

                    int value = MiniMax(childBoard, depth - 1, alpha, beta, out (int, int) childPos);

                    //Определение наименьшего звена
                    if (value < best)
                    {
                        pos.Item1 = move.Item1;
                        pos.Item2 = move.Item2;
                        best = value;
                    }
                    beta = Math.Min(beta, best);
                    if (beta <= alpha)
                        break;
                }
            return best;
        }

        public int MiniMaxWithout(Board board, int depth, out (int, int) pos)
        {
            pos = (int.MinValue, int.MinValue);
            if (depth == 0)
                return Heuristic(board, _currPlayer.MoveType);
            int best = int.MinValue;

            //Формирование возможных ходов
            var moves = board.HaveAvailableCells(_currPlayer.MoveType);
            if (moves.Count == 0)
                return Heuristic(board, _currPlayer.MoveType);

            //Проход по вариантам с расчётом 
            foreach (var move in moves)
            {
                //Создание доски
                Board childBoard = (Board)board.Clone();

                //Получение результата хода
                childBoard.Move(_currPlayer.Id, move.Item1, move.Item2, _currPlayer.MoveType);

                int value = MaxMinWithout(childBoard, depth - 1, out (int, int) childPos);

                if (value > best)
                {
                    pos.Item1 = move.Item1;
                    pos.Item2 = move.Item2;
                    best = value;
                }
            }
            return best;
        }

        public int MaxMinWithout(Board board, int depth, out (int, int) pos)
        {
            pos = (int.MinValue, int.MinValue);
            if (depth == 0)
                return Heuristic(board, _currPlayer.MoveType);

            int best = int.MaxValue;

            //Формирование возможных ходов
            var moves = board.HaveAvailableCells(_enemyPlayer.MoveType);
            if (moves.Count == 0)
                return Heuristic(board, _currPlayer.MoveType);

            //Проход по вариантам с расчётом 
            foreach (var move in moves)
            {
                //Создание доски
                Board childBoard = (Board)board.Clone();

                //Получение результата хода
                childBoard.Move(_enemyPlayer.Id, move.Item1, move.Item2, _enemyPlayer.MoveType);

                int value = MiniMaxWithout(childBoard, depth - 1, out (int, int) childPos);

                //Определение наименьшего звена
                if (value < best)
                {
                    pos.Item1 = move.Item1;
                    pos.Item2 = move.Item2;
                    best = value;
                }
            }
            return best;
        }

        public static int Heuristic(Board bState, MoveType move)
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
            return move == MoveType.Vertical ? result: -result;
        }
    }
}