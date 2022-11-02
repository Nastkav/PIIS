using domineering_game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using domineering_game.game;
using System.Diagnostics;

namespace domineering_console
{
    internal class Program
    {
        const int MAX_WIDTH = 8;
        const int MAX_HEIGHT = 8;
        static void Main(string[] args)
        {
            //Создание игры
            IPlayer player1 = new Player(1,MoveType.Vertical);
            IPlayer player2 = new Player(2,MoveType.Horizontal);
            DomineeringGame game = new DomineeringGame(player1,player2, MAX_HEIGHT, MAX_WIDTH);
            game.StartNewGame();

            //Установка бота
            int level = 3;//1-3
            AlBePruningAi pruningBot2 = new AlBePruningAi(level, player2, player1);

            //Отображение стартовой доски
            Console.WriteLine("Стартовая доска");
            Console.WriteLine(game.PrintBoard());
            Console.ReadLine();

            //Ход игрока 1
            while (game.Winner == null)
            {
                Console.WriteLine($"Ход игрока {game.ActivePlayer.Id}");
                if (game.ActivePlayer.Id == 1)
                {
                    //HumanMove(game);
                    RandomMove(game);
                }
                else
                {
                    //RandomMove(game);
                    BotMove(game, pruningBot2);
                }
                Console.WriteLine(game.PrintBoard());
                Console.WriteLine("\t-");
            }
            Console.WriteLine($"Игра закончена, победил {game.Winner.Id}");

            Console.ReadLine();
        }

        private static void HumanMove(DomineeringGame game)
        {
            bool moved = false;
            while (!moved)
            {
                int h = int.MinValue, w = int.MinValue;
                while (h == int.MinValue)
                {
                    Console.Write("Введите индекс строки (h:0-7) :");
                    var key = Console.ReadKey().KeyChar.ToString();
                    if(!int.TryParse(key, out h) || h >= MAX_HEIGHT)    //Если значение не совпадает с заявленной высотой
                        h = int.MinValue;
                    Console.WriteLine();
                }
                while (w == int.MinValue)
                {
                    Console.Write("Введите позицию ячейки (w:0-7) :");
                    var key = Console.ReadKey().KeyChar.ToString();
                    if (!int.TryParse(key, out w) || w >= MAX_WIDTH)    //Если значение не совпадает с заявленной шириной
                        w = int.MinValue;
                    Console.WriteLine();
                }
                Console.WriteLine($"RandomMove: h-{h},w-{w}");
                moved = game.Move(h, w);
                Console.WriteLine($"Успешность выполнения шага: {moved}");
            }
        }
        private static void RandomMove(DomineeringGame game)
        {
            bool moved = false;
            Random random = new Random();
            while (!moved)
            {
                int h = random.Next(0, 8);
                int w = random.Next(0, 8);
                Console.WriteLine($"RandomMove: h-{h},w-{w}");
                moved = game.Move(h, w);
                Console.WriteLine($"Успешность выполнения шага: {moved}");
            }
        }

        private static void BotMove(DomineeringGame game, AlBePruningAi bot)
        {
            bool moved = false;
            while (!moved)
            {
                int heurValue = bot.CalcBestMove(game.Board,out int h, out int w);

                Console.WriteLine($"BotMove: h={h},w={w}, heurValue = {heurValue}");
                moved = game.Move(h, w);
                Console.WriteLine($"Успешность выполнения шага: {moved}");
            }
        }
    }
}
