using System;
using System.Threading;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Numerics;

namespace TicTacToe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                TicTacToe();
            }
        }
        
        static void TicTacToe()
        {   
            int bannerPrint = 0, gameboardPrint = 1;
            int round = 1;

            bool resetGame = false;

            string[,] field = new string[9, 3];

            SetFields(field);

            while (round < 10 && resetGame == false)
            {
                FormatText(bannerPrint, Banner(), field);
                FormatText(gameboardPrint, GameBoard(), field);

                switch (round % 2)
                {
                    case 0:
                        ComputerMove(field, DetermineSign(round));

                        break;

                    case 1:
                        //if (PlayerMove(field, PlayerInput(DetermineSign(round)), DetermineSign(round)) == false)
                        //    InvalidInputMessage();

                        ComputerMove(field, DetermineSign(round));

                        break;

                    default:
                        break;
                }

                if (DetermineWinner(field, DetermineSign(round)) == true)
                {
                    DisplayMessage("CONGRATULATIONS, YOU'VE WON THE GAME", bannerPrint, gameboardPrint, field);

                    break;
                }
                else if (DetermineWinner(field, DetermineSign(round)) == true)
                {
                    DisplayMessage("YOU'VE LOST!", bannerPrint, gameboardPrint, field);

                    break;
                }
                else if (DetermineWinner(field, DetermineSign(round)) == false && round == 10)
                {
                    DisplayMessage("IT'S A DRAW!", bannerPrint, gameboardPrint, field);

                    break;
                }

                round++;

                Console.Clear();
            }
        }

        static void SetFields(string[,] field)
        {
            for (int i = 0; i < 3; i++)
            {
                field[0, i] = Convert.ToString(i + 1);
                field[1, i] = Convert.ToString(i + 4);
                field[2, i] = Convert.ToString(i + 7);
            }
        }

        static void FormatText(int definePrint, string text, string[,] field)
        {
            string separator = "\r\n";

            int count = 100;

            string[] strlist = text.Split(separator, count,
                   StringSplitOptions.RemoveEmptyEntries);

            if (definePrint == 0)
            {
                for (int i = 0; i < strlist.Length; i++)
                {
                    PrintTextCenter(strlist[i]);
                }
            }
            else
            {
                for (int i = 0; i < strlist.Length; i++)
                {
                    PrintBoard(strlist[i], field);
                }
            }
        }

        static void PrintTextCenter(string text)
        {
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
            Console.WriteLine(text);
        }

        static void PrintBoard(string text, string[,] field)
        {
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
            Console.WriteLine(text, field[0, 0], field[0, 1], field[0, 2], field[1, 0], field[1, 1], field[1, 2], field[2, 0], field[2, 1], field[2, 2]);
        }

        static string Banner()
        {
            string welcomeMessage = "  __          __  _                              _          _______ _        _______           _______         " + "\r\n" +
                                    @" \ \        / / | |                            | |        |__   __(_)      |__   __|         |__   __|        " + "\r\n" +
                                    @"  \ \  /\  / /__| | ___ ___  _ __ ___   ___    | |_ ___      | |   _  ___     | | __ _  ___     | | ___   ___ " + "\r\n" +
                                    @"   \ \/  \/ / _ \ |/ __/ _ \| '_ ` _ \ / _ \   | __/ _ \     | |  | |/ __|    | |/ _` |/ __|    | |/ _ \ / _ \" + "\r\n" +
                                    @"    \  /\  /  __/ | (_| (_) | | | | | |  __/   | || (_) |    | |  | | (__     | | (_| | (__     | | (_) |  __/" + "\r\n" +
                                    @"     \/  \/ \___|_|\___\___/|_| |_| |_|\___|    \__\___/     |_|  |_|\___|    |_|\__,_|\___|    |_|\___/ \___|" + "\r\n";

            return welcomeMessage;
        }

        static string GameBoard()
        {
            string gameBoard =   "           _           _           \r\n" +
                                 "          | |         | |          \r\n" +
                                 "          | |         | |          \r\n" +
                               "     {0}    | |    {1}    | |    {2}\r\n" +
                                 "          | |         | |          \r\n" +
                                 " _________| |_________| |_________ \r\n" +
                                 "|_________| |_________| |_________|\r\n" +
                                 "          | |         | |          \r\n" +
                                 "          | |         | |          \r\n" +
                               "     {3}    | |    {4}    | |    {5}\r\n" +
                                 "          | |         | |          \r\n" +
                                 " _________| |_________| |_________ \r\n" +
                                 "|_________| |_________| |_________|\r\n" +
                                 "          | |         | |          \r\n" +
                                 "          | |         | |          \r\n" +
                               "     {6}    | |    {7}    | |    {8}\r\n" +
                                 "          | |         | |          \r\n" +
                                 "          |_|         |_|          \r\n" +
                                 "\r\n";

            return gameBoard;
        }

        static bool PlayerMove(string[,] field, int playerInput, string determineSign)
        {
            bool validInput = false;

            int inputConditionMin = 1;
            int inputConditionMax = 3;

            for (int i = 0; i < 3; i++)
            {
                if (playerInput >= inputConditionMin && playerInput <= inputConditionMax && field[i, playerInput - inputConditionMin] != "O" && field[i, playerInput - inputConditionMin] != "X")
                {
                    field[i, playerInput - inputConditionMin] = determineSign;

                    validInput = true;

                    break;
                }

                inputConditionMin = inputConditionMin + 3;
                inputConditionMax = inputConditionMax + 3;
            }

            return validInput;
        }

        static int PlayerInput(string player)
        {
            int move = -1;

            Console.Write("\n\t\t\t\t\t  {0}'s move: ", player);

            ConsoleKeyInfo userInput = Console.ReadKey(true);

            if (char.IsDigit(userInput.KeyChar))
                move = int.Parse(userInput.KeyChar.ToString());

            else
                InvalidInputMessage();

            return move;
        }

        static string DetermineSign(int round)
        {
            string sign;

            int player = round % 2;

            if (player == 1)
                sign = "O";
            else
                sign = "X";

            return sign;
        }

        static bool DetermineWinner(string[,] field, string sign)
        {
            bool winner = false;

            for (int i = 0; i <= 2; i++)
            {
                if (field[i, 0] == sign && field[i, 1] == sign && field[i, 2] == sign || field[0, i] == sign && field[1, i] == sign && field[2, i] == sign)
                    winner = true;

                else if (field[0, 0] == sign && field[1, 1] == sign && field[2, 2] == sign || field[0, 2] == sign && field[1, 1] == sign && field[2, 0] == sign)
                    winner = true;
            }

            return winner;
        }

        static void DisplayMessage(string message, int bannerPrint, int gameboardPrint, string[,] field)
        {
            Console.Clear();

            FormatText(bannerPrint, Banner(), field);
            FormatText(gameboardPrint, GameBoard(), field);

            PrintTextCenter(message);
            PrintTextCenter("Press enter to play again");

            Console.ReadKey();
            Console.Clear();
        }

        static void InvalidInputMessage()
        {
            PrintTextCenter("      Invalid input. Try again.      ");

            Thread.Sleep(1000);
            Console.Clear();
        }

        static bool ComputerMove(string[,] field, string determineSign)
        {
            Random rnd = new Random();

            bool moveControl = false;

            while (moveControl == false)
            {
                int computerInput = rnd.Next(1, 10);

                moveControl = PlayerMove(field, computerInput, determineSign);
            }

            return moveControl;
        }
    }
}


/*

Mangler:
 * Lav en pre-game menu hvor spiller(re) kan indtaste navn ( dont wanna do this)
 * Lav single-player mode mod computer

*/