using System;
using System.Threading;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Numerics;
using Microsoft.VisualBasic;
using System.Diagnostics.Tracing;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;
using System.Net.Mail;
using System.Linq.Expressions;

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
            int bannerPrint = 0,
                gameboardPrint = 1,
                round = 1,
                player1;

            bool resetGame = false,
                 validInput;

            string[,] field = new string[3, 3];

            SetFields(field);

            player1 = StartChoice(field, bannerPrint, gameboardPrint);

            while (round < 10 && resetGame == false)
            {
                validInput= false;

                FormatText(bannerPrint, Banner(), field);
                FormatText(gameboardPrint, GameBoard(), field);

                switch (round % 2)
                {
                    case 0:

                        if (player1 == 2)
                        {
                            do
                            {
                                if (PlayerMove(field, PlayerInput(DetermineSign(round)), DetermineSign(round)) == true)
                                    validInput = true;

                                FormatText(bannerPrint, Banner(), field);
                                FormatText(gameboardPrint, GameBoard(), field);

                            } while (validInput == false);
                        }
                        else if (player1 == 1)
                        {
                            ComputerMove(field, DetermineSign(round));
                            Thread.Sleep(1000);
                        }

                        break;

                    case 1:
                        if (player1 == 1)
                        {
                            do
                            {
                                if (PlayerMove(field, PlayerInput(DetermineSign(round)), DetermineSign(round)) == true)
                                    validInput = true;

                                FormatText(bannerPrint, Banner(), field);
                                FormatText(gameboardPrint, GameBoard(), field);

                            } while (validInput == false);
                        }
                        else if (player1 == 2)
                        {
                            ComputerMove(field, DetermineSign(round));

                            Thread.Sleep(500);
                        }

                        break;

                    default:
                        break;
                }

                if (DetermineWinner(field, DetermineSign(round)) == true)
                {
                    DisplayMessage("CONGRATULATIONS, YOU'VE WON THE GAME", bannerPrint, gameboardPrint, field);

                    break;
                }
                else if (DetermineWinner(field, DetermineSign(round)) == false && round >= 9)
                {
                    DisplayMessage("IT'S A DRAW!", bannerPrint, gameboardPrint, field);

                    break;
                }

                round++;

                Console.Clear();
            }
        }

        static int StartChoice(string[,] field, int bannerPrint, int gameboardPrint)
        {

            int firstPlayer = -1;

            while (true)
            {
                FormatText(bannerPrint, Banner(), field);

                Console.WriteLine();
                PrintTextCenter("Choose who should start");
                Console.Write("\n\t\t\t\t\t\t1. You\n\t\t\t\t\t\t2. Computer\n\t\t\t\t\t\t");

                ConsoleKeyInfo UserInput = Console.ReadKey();

                if (char.IsDigit(UserInput.KeyChar))
                    firstPlayer = int.Parse(UserInput.KeyChar.ToString());
                else
                    firstPlayer = -1;

                if (firstPlayer == 1 || firstPlayer == 2)
                {
                    Console.Clear();
                    break;
                }
                else
                { 
                    InvalidInputMessage();
                    Console.Clear();
                }
            }

            return firstPlayer;
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

        static void DisplayMessage(string message, int bannerPrint, int gameboardPrint, string[,] field)
        {
            Console.Clear();

            FormatText(bannerPrint, Banner(), field);
            FormatText(gameboardPrint, GameBoard(), field);

            PrintTextCenter(message);
            PrintTextCenter("Press enter to play again");

            Thread.Sleep(1000);

            Console.ReadKey();
            Console.Clear();
        }

        static void InvalidInputMessage()
        {
            PrintTextCenter("      Invalid input. Try again.      ");

            Thread.Sleep(1000);
            Console.Clear();
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

        static bool PlayerMove(string[,] field, int playerInput, string determineSign)
        {
            bool validInput = false;

            int inputConditionMin = 1,
                inputConditionMax = 3;

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

        static bool ComputerMove(string[,] field, string sign)
        {
            Random rnd = new Random();

            bool moveControl = false;

            int[] xArray = new int[3],
                  yArray = new int[3],
                  xyArray = new int[1],
                  yxArray = new int[1];

            int xyValue = 0,
                yxValue = 0;

            int line = 0;

            bool isCircle;

            if (sign == "O")
                isCircle = true;
            else
                isCircle = false;

            do
            {
                int xValue = 0,
                    yValue = 0;

                for (int i = 0; i < 3; i++)
                {
                    switch (isCircle)
                    {
                        case true:
                            yValue += CalculateAxisValue(field, sign, "O", "X", line, i, yValue);

                            xValue += CalculateAxisValue(field, sign, "O", "X", i, line, xValue);

                            xyValue += CalculateXY(field, sign, "O", "X", line, i, xyValue);

                            yxValue = CalculateYX(field, sign, "O", "X", line, i, yxValue);

                            break;

                        case false:
                            yValue += CalculateAxisValue(field, sign, "X", "O", line, i, yValue);

                            xValue += CalculateAxisValue(field, sign, "X", "O", i, line, xValue);

                            xyValue += CalculateXY(field, sign, "X", "O", line, i, xyValue);

                            yxValue = CalculateYX(field, sign, "X", "O", line, i, yxValue);

                            break;
                    }
                }

                yArray[line] = yValue;
                xArray[line] = xValue;
                xyArray[0] = xyValue;
                yxArray[0] = yxValue;

                line++;

            } while (line < 3);

                line = 0;

            while (moveControl == false && line < 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    moveControl = AttackOrDefend(field, sign, xArray, yArray, xyArray, yxArray, moveControl, i, 2);
                }

                if (moveControl == false)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        moveControl = AttackOrDefend(field, sign, xArray, yArray, xyArray, yxArray, moveControl, i, -2);
                    }
                }
                line++;
            }

            line = 0;

            while (moveControl == false || line == 9)
            {
                moveControl = RndStrat(field, sign, rnd, moveControl);

                line++;
            }

            if (moveControl == false)
                for (int i = 1; i < 9; i++)
                {
                    moveControl = PlayerMove(field, i, sign);
                }
            
            return moveControl;
        }

        static int CalculateAxisValue(string[,] field, string sign, string mySign, string opponentSign, int x, int y, int value)
        {
            value = 0;

            if (sign == mySign && field[x, y] == mySign)
                value++;
            else if (sign == mySign && field[x, y] == opponentSign)
                value--;

            return value;
        }

        static int CalculateXY(string[,] field, string sign, string mySign, string opponentSign, int line, int i, int value)
        {
            value = 0;

            if (line == 0)
            {
                if (sign == mySign && field[i, i] == mySign)
                    value++;
                else if (sign == mySign && field[i, i] == opponentSign)
                    value--;
            }

            return value;
        }

        static int CalculateYX(string[,] field, string sign, string mySign, string opponentSign, int line, int i, int yxValue)
        {
            if (line == 0 && i == 0)
            {
                yxValue += YXValue(field, sign, mySign, opponentSign, 0, 2);

                yxValue += YXValue(field, sign, mySign, opponentSign, 1, 1);

                yxValue += YXValue(field, sign, mySign, opponentSign, 2, 0);
            }

            return yxValue;
        }

        static int YXValue(string[,] field, string sign, string mySign, string opponentSign, int cond1, int cond2)
        {
            int yxValue = 0;

            if (sign == mySign && field[cond1, cond2] == mySign)
                yxValue++;
            else if (sign == mySign && field[cond1, cond2] == opponentSign)
                yxValue--;

            return yxValue;
        }

        static bool AttackOrDefend(string[,] field, string sign, int[] xArray, int[] yArray, int[] xyArray, int[] yxArray, bool moveControl, int line, int playcondition)
        {
            if (xArray[line] == playcondition && moveControl == false)
            {
                switch (line)
                {
                    case 0:
                        moveControl = XArrayMove(field, sign, 1, 7);

                        break;

                    case 1:
                        moveControl = XArrayMove(field, sign, 2, 8);

                        break;

                    case 2:
                        moveControl = XArrayMove(field, sign, 3, 9);

                        break;
                }
            }

            if (yArray[line] == playcondition && moveControl == false)
            {
                switch (line)
                {
                    case 0:
                        moveControl = YArrayMove(field, sign, 1, 3);

                        break;

                    case 1:
                        moveControl = YArrayMove(field, sign, 4, 6);

                        break;

                    case 2:
                        moveControl = YArrayMove(field, sign, 7, 9);

                        break;
                }
            }

            if (xyArray[0] == playcondition && moveControl == false)
            {
                for (int i = 1; i <= 9; i += 4)
                {
                    moveControl = PlayerMove(field, i, sign);

                    if (moveControl == true)
                        break;
                }
            }

            if (yxArray[0] == playcondition && moveControl == false)
            {
                for (int i = 3; i <= 9; i += 2)
                {
                    moveControl = PlayerMove(field, i, sign);

                    if (moveControl == true)
                        break;
                }
            }

            return moveControl;

        }

        static bool XArrayMove(string[,] field, string sign, int cond1, int cond2)
        {
            bool moveControl = false;

            for (int i = cond1; i <= cond2; i += 3)
            {
                moveControl = PlayerMove(field, i, sign);

                if (moveControl == true)
                    break;
            }

            return moveControl;
        }

        static bool YArrayMove(string[,] field, string sign, int cond1, int cond2)
        {
            bool moveControl = false;

            for (int i = cond1; i <= cond2; i++)
            {
                moveControl = PlayerMove(field, i, sign);

                if (moveControl == true)
                    break;
            }

            return moveControl;
        }

        static bool RndStrat(string[,] field, string sign, Random rnd, bool moveControl)
        { 
            int computerInput = -1;

            do
            {
                int initialInput = rnd.Next(1, 10);

                if (initialInput % 2 != 0)
                {
                    computerInput = initialInput;
                }

                moveControl = PlayerMove(field, computerInput, sign);

            } while (moveControl == false);

            return moveControl;
        }
    }
}