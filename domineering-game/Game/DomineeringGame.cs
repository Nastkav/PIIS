using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace domineering_game.game
{
    public class DomineeringGame
    {
        public Board board { get; private set; }
        public IPlayer ActivePlayer { get; private set; }
        public IPlayer PassivePlayer { get; private set; }
        public IPlayer Winner { get; private set; }

        public DomineeringGame(IPlayer firstPlayer, IPlayer secondPlayer, int sizeH, int sizeV)
        {
            board = new Board(sizeH, sizeV);
            ActivePlayer = firstPlayer;
            PassivePlayer = secondPlayer;
        }

        public void StartNewGame()
        {
            int h, w;
            (h, w) = board.BoardSize;
            board = new Board(h, w);
            Winner = null;
        }
        public List<(int,int)> GetAvailableCells(MoveType moveType) => board.HaveAvailableCells(moveType);

        public bool Move(int h, int w)
        {
            bool result = board.Move(ActivePlayer.Id, h, w, ActivePlayer.MoveType);
            if (result)
                PassMove();
            return result;
        }

        private void PassMove()
        {
            //Смена игроков
            IPlayer currPlayer = ActivePlayer;
            ActivePlayer = PassivePlayer;
            PassivePlayer = currPlayer;

            //Проверка существования ходов для игрока
            int countMoves = board.HaveAvailableCells(ActivePlayer.MoveType).Count;
            if (countMoves == 0)
                Winner = PassivePlayer;
        }

        public string PrintBoard() => board.Print();
    }
}
