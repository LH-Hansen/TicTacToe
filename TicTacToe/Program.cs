using System;
using System.Threading;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

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

            string[] field = new string[9];

            SetFields(field);

            while (round <= 10 && resetGame == false)
            {
                FormatText(bannerPrint, Banner(), field);
                FormatText(gameboardPrint, GameBoard(), field);
                if (PlayerMove(field, PlayerInput(DetermineSign(round)), DetermineSign(round)) != false)
                    round++;

                Console.Clear();
            }
        }

        static void SetFields(string[] field)
        {
            for (int i = 0; i < field.Length; i++)
            {
                field[i] = Convert.ToString(i + 1);
            }
        }

        static void FormatText(int definePrint, string text, string[] field)
        {
            string separator = "\r\n";

            int count = 100;

            string [] strlist = text.Split(separator, count,
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

        static void PrintBoard(string text, string[] field)
        {
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
            Console.WriteLine(text, field[0], field[1], field[2], field[3], field[4], field[5], field[6], field[7], field[8]);
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
            string gameBoard = "           _           _           \r\n" +
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

        static bool PlayerMove(string[] field, int playerInput, string determineSign)
        {
            bool validInput = false;

            if (playerInput >= 1 && playerInput <= 10 && field[playerInput - 1] != "O" && field[playerInput - 1] != "X")
            {
                field[playerInput - 1] = determineSign;

                validInput = true;
            }
            else
            {
                PrintTextCenter("Invalid input. Try again.");

                Thread.Sleep(1000);
                Console.Clear();
            }

            return validInput;
        }

        static int PlayerInput(string player)
        {
            int move;

            Console.Write("\n\t\t\t\t\t  {0}'s move: ", player);

            move = Convert.ToInt32(Console.ReadLine());

            return move;
        }

        static string DetermineSign(int round)
        {
            int player;

            string sign;

            player = round % 2;

            if (player == 1)
                sign = "O";
            else
                sign = "X";

            return sign;
        }
    }
}
