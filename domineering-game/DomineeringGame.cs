using domineering_game.game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace domineering_game
{
    public class DomineeringGame
    {
        private int pasPoint;

        public Board Board { get; private set; }
        public IPlayer ActivePlayer { get; private set; }
        public IPlayer PassivePlayer { get; private set; }
        public IPlayer Winner { get; private set; }

        public DomineeringGame(IPlayer firstPlayer, IPlayer secondPlayer, int sizeH, int sizeV)
        {
            Board = new Board(sizeH, sizeV);
            ActivePlayer = firstPlayer;
            PassivePlayer = secondPlayer;
        }

        public void StartNewGame()
        {
            int h, w;
            (h, w) = Board.BoardSize;
            Board = new Board(h, w);
            Winner = null;

            if (ActivePlayer.Id != 1)
            {
                IPlayer player = ActivePlayer;
                ActivePlayer = PassivePlayer;
                PassivePlayer = player;
            }
        }
        public List<(int,int)> GetAvailableCells(MoveType moveType) => Board.HaveAvailableCells(moveType);

        public bool Move(int h, int w)
        {
            bool result = Board.Move(ActivePlayer.Id, h, w, ActivePlayer.MoveType);
            if (result)
                PassMove();
            return result;
        }

        //Проверка существования ходов для игрока
        private void PassMove()
        {
            int currMoves = Board.HaveAvailableCells(ActivePlayer.MoveType).Count;
            int nextMoves = Board.HaveAvailableCells(PassivePlayer.MoveType).Count;

            if (currMoves == 0) 
                if(nextMoves > 0)
                    Winner = PassivePlayer;
                else
                    Winner = new Player(-1, MoveType.Horizontal);
            else if (nextMoves == 0)
                Winner = ActivePlayer;
            else if(nextMoves > 0) //Следующий игрок имеет ходы
            {
                IPlayer nextPlayer = PassivePlayer;
                PassivePlayer = ActivePlayer;
                ActivePlayer = nextPlayer;

            }
        }
        public string PrintBoard() => Board.Print();
    }
}
