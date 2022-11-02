using domineering_game.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace domineering_game.game
{
    public class Board : ICloneable
    {
        int boardH = 8;
        int boardW = 8;

        public int[][] Cells { get; 
             set; }

        public Board(int boardH, int boardW)
        {
            this.boardH = boardH;
            this.boardW = boardW;
            //Создание чистой доски
            Cells = ExtensionMethods.GetDimArray(boardH, boardW);
        }
        public (int, int) BoardSize
        {
            get => (boardH, boardW);
            set
            {
                boardH = value.Item1;
                boardW = value.Item2;
            }
        }


        public bool CanMove(int h, int w, MoveType moveType)
        {
            bool result;
            if(0 > h || h > boardH-1 || 0 > w || w > boardW-1 || Cells[h][w] != 0) //Ячейка использована
                return false;

            if (moveType == MoveType.Vertical && h != Cells.Length - 1 && Cells[h + 1][w] == 0)
                result = true;//ячейка ниже доступна
            else if (moveType == MoveType.Horizontal && w != Cells.Length - 1 && Cells[h][w + 1] == 0)
                result = true;//ячейка справа доступна
            else
                result = false;

            return result;
        }
        public List<(int, int)> HaveAvailableCells(MoveType moveType)
        {
            List<(int, int)> moves = new List<(int, int)>();
            for (int h = 0; h < Cells.Length; h++)
                for (int w = 0; w < Cells[h].Length; w++)
                    if (CanMove(h, w, moveType))//Возврат 1 есть возможно сделать ход
                        moves.Add((h, w));
            return moves;
        }

        public bool Move(int number, int h, int w, MoveType moveType)
        {
            bool result = false;
            if(CanMove(h,w,moveType))
            {
                Cells[h][w] = number;
                if (moveType == MoveType.Vertical)
                    Cells[h + 1][w] = number;
                if (moveType == MoveType.Horizontal)
                    Cells[h][w + 1] = number;
                result = true;
            }

            return result;
        }

        public object Clone()
        {
            Board board = new Board(boardH, boardW)
            {
                Cells = ExtensionMethods.CloneDimArray(Cells)
            };
            return board;
        }
        public string Print()
        {
            StringBuilder builder = new StringBuilder();
            for (int h = 0; h < Cells.Length; h++)
            {
                for (int w = 0; w < Cells[h].Length; w++)
                    builder.Append(Cells[h][w] + " ");
                builder.AppendLine();
            }
            return builder.ToString();
        }


    }
}