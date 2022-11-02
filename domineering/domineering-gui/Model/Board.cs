using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace domineering_gui.Model
{

    public class Board : ICloneable
    {
        private int boardH = 8;
        private int boardW = 8;
        public Cell[][] Cells { get; private set; }

        public Board(int h, int w)
        {
            boardH = h;
            boardW = w;
            CellsReset();
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

        //Методы получения ячейки
        public Cell GetCell(int h, int w) => Cells[h][w];
        public Cell GetCell(int index)
        {
            PositionFromIndex(index,out int h,out int w);
            return Cells[h][w];
        }

        //Функции работы с индексами
        public int GetCellId(int h, int w) => h * boardW + w;
        public void PositionFromIndex(int index, out int h, out int w)
        { 
            h = index/boardW;
            w = index % boardW;
        }

        //Метод использования ячейки пользователем
        public void SetCell(UsedType player,int h,int w) => Cells[h][w].CellUsed = player;
        public void SetCell(UsedType player, int index) {
            Cell cell = GetCell(index);
            cell.CellUsed = player; 
        }

        internal List<Board> GetAllChild(bool vertical)
        {
            List<Board> children = new List<Board>();

            for (int h = 0; h < boardH; h++)
                for (int w = 0; w < boardW; w++)
                {
                    Cell cell = Cells[h][w];
                    Board board;
                    if (!cell.Active)
                        break;

                    if (vertical && cell.H > 0 && Cells[cell.H - 1][cell.W].Active)
                    {
                        board = this.Clone() as Board;
                        board.SelectCell(cell,UsedType.Player1);
                    }
                    else if (!vertical && cell.W > 0 && Cells[cell.H][cell.W - 1].Active)
                    {
                        board = this.Clone() as Board;
                        board.SelectCell(cell, UsedType.Player2);
                    }
                    else
                        continue;

                    children.Add(board);
                }

            return children;
        }

        public void CellsReset()
        {
            Cells = new Cell[boardH][];
            for (int i = 0; i < boardH; i++)
            {
                Cells[i] = new Cell[boardW];
                for (int j = 0; j < boardW; j++)
                    Cells[i][j] = new Cell(i, j, GetCellId(i, j));
            }
        }
        public List<int> AvailableCells()
        {
            List<int> boardCells = new List<int>();
            for (int i = 0; i < boardH; i++)
                for (int j = 0; j < boardW; j++)
                    if (Cells[i][j].Active)
                        boardCells.Add(GetCellId(i,j));

            return boardCells;
        }

        public bool SelectCell(Cell cell, UsedType player)
        {
            //Проверка на наличие ячейки
            if (cell != null)
                return false;

            //Проверка ячейки на соответствия
            Cell nCell;
            cell = Cells[cell.H][cell.W];
            if (player == UsedType.Player1 && cell.H > 0 && Cells[cell.H - 1][cell.W].Active) // up cell
                nCell = Cells[cell.H - 1][cell.W];
            else if (player == UsedType.Player2 && cell.W > 0 && Cells[cell.H][cell.W - 1].Active) // left cell
                nCell = Cells[cell.H][cell.W - 1];
            else
                return false;
            
            //Блокирование ячеек выбранным игроком
            cell.CellUsed = player;
            nCell.CellUsed = player;
            return true;
        }

        public object Clone()
        {
            Board board = new Board(boardH, boardW);
            for (int h = 0; h < boardH; h++)
                for (int w = 0; w < boardW; w++)
                    board.Cells[h][w] = this.Cells[h][w].Clone() as Cell;
                return board;
        }
    }
}
