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
using System.Text;

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

        #region ------------------------------ Initialize game ------------------------------
        /// <summary>
        /// Initializes the game
        /// </summary>
        static void TicTacToe()
        {       //Banner- & gameboard prin are used to choose how the respective string are printed
            int bannerPrint = 0,
                gameboardPrint = 1,
                turn = 1,
                player1;

            bool resetGame = false;

            string[,] field = new string[3, 3];

            //Sets the gamefield to defualt values
            SetFields(field);

            //Lets the player select the gamemode
            player1 = StartChoice(field, bannerPrint);

            while (turn < 10 && resetGame == false)
            {
                bool playerTurn = false;

                //Prints the UI
                FormatText(bannerPrint, Banner(), field);
                FormatText(gameboardPrint, GameBoard(), field);


                //Keeps track of who is playing each turn depending on gamemode
                switch (turn % 2)
                {
                    case 1:
                        //Player VS Computer
                        if (player1 == 1)
                            playerTurn = InititalaizePlayer(field, turn, bannerPrint, gameboardPrint);

                        //Computer VS Player
                        else if (player1 == 2)
                        {
                            ComputerMove(field, DetermineSign(turn));
                            Thread.Sleep(500);
                        }


                        //Player VS Player
                        else if (player1 == 3)
                            playerTurn = InititalaizePlayer(field, turn, bannerPrint, gameboardPrint);
                        break;

                    case 0:
                        if (player1 == 1)
                        {
                            ComputerMove(field, DetermineSign(turn));
                            Thread.Sleep(500);
                        }

                        else if (player1 == 2)
                            playerTurn = InititalaizePlayer(field, turn, bannerPrint, gameboardPrint);

                        else if (player1 == 3)
                            playerTurn = InititalaizePlayer(field, turn, bannerPrint, gameboardPrint);

                        break;
                }

                //Displays winning message depending on outcome
                if (DetermineWinner(field, DetermineSign(turn)) == true && playerTurn == true)
                    resetGame = DisplayEndMessage("CONGRATULATIONS, YOU WON!", bannerPrint, gameboardPrint, field);

                //Displays losing message depending on outcome
                else if (DetermineWinner(field, DetermineSign(turn)) == true && playerTurn == false)
                    resetGame = DisplayEndMessage("HOW UNFORTUNATE, YOU LOST!", bannerPrint, gameboardPrint, field);

                //Displays draw message depending on outcome
                else if (DetermineWinner(field, DetermineSign(turn)) == false && turn >= 9)
                    resetGame = DisplayEndMessage("IT'S A DRAW!", bannerPrint, gameboardPrint, field);

                turn++;

                Console.Clear();
            }

            //Makes system reclaim memory, as the program keept using more for each turn prior
            GC.Collect();
        }

        /// <summary>
        /// Determines the sign of the current player
        /// </summary>
        /// <param name="turn"></param>
        /// <returns>"O" or "X"</returns>
        static string DetermineSign(int turn)
        {
            //Keeps track of what sign the current player is using
            string sign;

            int player = turn % 2;

            if (player == 1)
                sign = "O";

            else
                sign = "X";

            return sign;
        }
        #endregion

        #region ------------------------------ UI ------------------------------
        /// <summary>
        /// Sets the fields to defualt
        /// </summary>
        /// <param name="field"></param>
        static void SetFields(string[,] field)
        {
            for (int i = 0; i < 3; i++)
            {
                field[0, i] = Convert.ToString(i + 1);
                field[1, i] = Convert.ToString(i + 4);
                field[2, i] = Convert.ToString(i + 7);
            }
        }

        /// <summary>
        /// Formats the banner and gameboard to display in the center of the console
        /// </summary>
        /// <param name="definePrint"></param>
        /// <param name="text"></param>
        /// <param name="field"></param>
        static void FormatText(int definePrint, string text, string[,] field)
        {
            string separator = "\r\n";

            int count = 100;

            //Takes string and splits it into an array with a seperator
            string[] strlist = text.Split(separator, count,
                   StringSplitOptions.RemoveEmptyEntries);

            //Uses banner- or gamemodeprint to determine what method should be used to print 
            if (definePrint == 0)
            {
                for (int i = 0; i < strlist.Length; i++)
                {
                    //Prints strings in array in the center of the console
                    PrintTextCenter(strlist[i]);
                }
            }
            else
            {
                for (int i = 0; i < strlist.Length; i++)
                {
                    //Prints strings in array in the center of the console with the field array and values
                    PrintBoard(strlist[i], field);
                }
            }
        }

        /// <summary>
        /// Prints strings in the center of the console
        /// </summary>
        /// <param name="text"></param>
        static void PrintTextCenter(string text)
        {
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
            Console.WriteLine(text);
        }

        /// <summary>
        /// Prints strings in the center of the console, with field array values
        /// </summary>
        /// <param name="text"></param>
        /// <param name="field"></param>
        static void PrintBoard(string text, string[,] field)
        {
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
            Console.WriteLine(text, field[0, 0], field[0, 1], field[0, 2], field[1, 0], field[1, 1], field[1, 2], field[2, 0], field[2, 1], field[2, 2]);
        }

        /// <summary>
        /// Prints end message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="bannerPrint"></param>
        /// <param name="gameboardPrint"></param>
        /// <param name="field"></param>
        /// <returns>Bool reset promt</returns>
        static bool DisplayEndMessage(string message, int bannerPrint, int gameboardPrint, string[,] field)
        {
            bool resetGame = true;

            Console.Clear();

            FormatText(bannerPrint, Banner(), field);
            FormatText(gameboardPrint, GameBoard(), field);

            PrintTextCenter(message);
            PrintTextCenter("Press enter to play again");

            Thread.Sleep(1000);
            Console.ReadKey();
            Console.Clear();

            return resetGame;
        }

        /// <summary>
        /// Print error message
        /// </summary>
        static void InvalidInputMessage()
        {
            PrintTextCenter("      Invalid input. Try again.      ");

            Thread.Sleep(1000);
            Console.Clear();
        }

        /// <summary>
        /// Sets banner string
        /// </summary>
        /// <returns>Banner string</returns>
        static string Banner()
        {
            //The default banner to be printed
            string banner = "  __          __  _                              _          _______ _        _______           _______         " + "\r\n" +
                                    @" \ \        / / | |                            | |        |__   __(_)      |__   __|         |__   __|        " + "\r\n" +
                                    @"  \ \  /\  / /__| | ___ ___  _ __ ___   ___    | |_ ___      | |   _  ___     | | __ _  ___     | | ___   ___ " + "\r\n" +
                                    @"   \ \/  \/ / _ \ |/ __/ _ \| '_ ` _ \ / _ \   | __/ _ \     | |  | |/ __|    | |/ _` |/ __|    | |/ _ \ / _ \" + "\r\n" +
                                    @"    \  /\  /  __/ | (_| (_) | | | | | |  __/   | || (_) |    | |  | | (__     | | (_| | (__     | | (_) |  __/" + "\r\n" +
                                    @"     \/  \/ \___|_|\___\___/|_| |_| |_|\___|    \__\___/     |_|  |_|\___|    |_|\__,_|\___|    |_|\___/ \___|" + "\r\n";

            return banner;
        }

        /// <summary>
        /// Sets gameboard string
        /// </summary>
        /// <returns>Gameboard string</returns>
        static string GameBoard()
        {
            //Default gameboard to be printed
            string gameBoard = "           _           _           \r\n" +
                                 "          | |         | |          \r\n" +
                                 "          | |         | |          \r\n" +
                               "      {0}    | |    {1}    | |    {2}\r\n" +
                                 "          | |         | |          \r\n" +
                                 " _________| |_________| |_________ \r\n" +
                                 "|_________| |_________| |_________|\r\n" +
                                 "          | |         | |          \r\n" +
                                 "          | |         | |          \r\n" +
                               "      {3}    | |    {4}    | |    {5}\r\n" +
                                 "          | |         | |          \r\n" +
                                 " _________| |_________| |_________ \r\n" +
                                 "|_________| |_________| |_________|\r\n" +
                                 "          | |         | |          \r\n" +
                                 "          | |         | |          \r\n" +
                               "      {6}    | |    {7}    | |    {8}\r\n" +
                                 "          | |         | |          \r\n" +
                                 "          |_|         |_|          \r\n" +
                                 "\r\n";

            return gameBoard;
        }

        /// <summary>
        /// Checks if a winner is declared
        /// </summary>
        /// <param name="field"></param>
        /// <param name="sign"></param>
        /// <returns>A bool to see if you win or not</returns>
        static bool DetermineWinner(string[,] field, string sign)
        {
            bool winner = false;

            for (int i = 0; i <= 2; i++)
            {
                //Sees if there is any current-player sign in a row on x and y axis
                if (field[i, 0] == sign && field[i, 1] == sign && field[i, 2] == sign || field[0, i] == sign && field[1, i] == sign && field[2, i] == sign)
                    winner = true;

                //Sees if there are any player signs going across on both sides
                else if (field[0, 0] == sign && field[1, 1] == sign && field[2, 2] == sign || field[0, 2] == sign && field[1, 1] == sign && field[2, 0] == sign)
                    winner = true;
            }

            return winner;
        }
        #endregion

        #region ------------------------------ Player ------------------------------
        /// <summary>
        /// Takes player input and saves it if it is valid
        /// </summary>
        /// <param name="player"></param>
        /// <returns>Int with the valid move</returns>
        static int PlayerInput(string player)
        {
            int move = -1;

            Console.Write("\n\t\t\t\t\t  {0}'s move: ", player);

            //Takes the input without needing to press enter
            ConsoleKeyInfo userInput = Console.ReadKey(true);

            //makes sure it's a valid move
            if (char.IsDigit(userInput.KeyChar))
                move = int.Parse(userInput.KeyChar.ToString());

            else
                InvalidInputMessage();

            return move;
        }

        /// <summary>
        /// Gives user ability to choose what gamemode to play
        /// </summary>
        /// <param name="field"></param>
        /// <param name="bannerPrint"></param>
        /// <returns>Int; value for the Gamemode</returns>
        static int StartChoice(string[,] field, int bannerPrint)
        {
            int firstPlayer;

            //Loops until a valid choice has been made
            while (true)
            {
                FormatText(bannerPrint, Banner(), field);

                Console.WriteLine();
                PrintTextCenter("Choose what gamemode to play");
                Console.Write("\n\t\t\t\t\t\t    1. You   VS  Bot" +
                              "\n\t\t\t\t\t\t    2. Bot   VS  You" +
                              "\n\t\t\t\t\t\t    3. User  VS  User" +
                              "\n\t\t\t\t\t\t    ");

                //Takes the input
                ConsoleKeyInfo UserInput = Console.ReadKey();

                //Makes sure its valid
                if (char.IsDigit(UserInput.KeyChar))
                    firstPlayer = int.Parse(UserInput.KeyChar.ToString());
                else
                    firstPlayer = -1;

                //Throws error if false
                if (firstPlayer >= 1 && firstPlayer <= 3)
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

        /// <summary>
        /// validates input, calculates its position, applies the sign to the correct place and makes sure it can be placed there
        /// </summary>
        /// <param name="field"></param>
        /// <param name="playerInput"></param>
        /// <param name="determineSign"></param>
        /// <returns>Bool for valid input</returns>
        static bool PlayerMove(string[,] field, int playerInput, string determineSign)
        {
            bool validInput = false;

            int inputConditionMin = 1,
                inputConditionMax = 3;

            //Loop is used to determines what line is being looked at [i, x]
            for (int i = 0; i < 3; i++)
            {
                //Takes the player input and converts it into the multidimensional array
                //looks if it fits on the line dictated by for for-loop and sees if it is already used on the designated spot
                if (playerInput >= inputConditionMin && playerInput <= inputConditionMax && field[i, playerInput - inputConditionMin] != "O" && field[i, playerInput - inputConditionMin] != "X")
                {
                    //Writes the players sign to the designated place on the board/array 
                    field[i, playerInput - inputConditionMin] = determineSign;

                    //Sets bool to true
                    validInput = true;

                    break;
                }

                //Adds 3 to the conditions to look at the next line the next line
                inputConditionMin = inputConditionMin + 3;
                inputConditionMax = inputConditionMax + 3;
            }

            return validInput;
        }
        #endregion

        #region ------------------------------ Computer ------------------------------
        /// <summary>
        /// Determines computer move
        /// </summary>
        /// <param name="field"></param>
        /// <param name="sign"></param>
        static void ComputerMove(string[,] field, string sign)
        {
            Random rnd = new();

            bool moveControl = false;

            //Arrays, used to store and updae values from calculations
            int[] xArray = new int[3],
                  yArray = new int[3],
                  xyArray = new int[1],
                  yxArray = new int[1];

            int xyValue = 0,
                yxValue = 0,
                line = 0;

            bool isCircle;

            //Looks at what the computers sign is for the game
            if (sign == "O")
                isCircle = true;
            else
                isCircle = false;

            do
            {
                int xValue = 0,
                    yValue = 0;

                //Loops 3 times, to go through each line through each axis
                for (int i = 0; i < 3; i++)
                {
                    //Changes calculations based on sign
                    switch (isCircle)
                    {
                        //Calculates values for the axis'
                        case true:
                            yValue += CalculateAxisValue(field, sign, "O", "X", line, i);

                            xValue += CalculateAxisValue(field, sign, "O", "X", i, line);

                            xyValue += CalculateXY(field, sign, "O", "X", line, i);

                            yxValue = CalculateYX(field, sign, "O", "X", line, i, yxValue);

                            break;

                        case false:
                            yValue += CalculateAxisValue(field, sign, "X", "O", line, i);

                            xValue += CalculateAxisValue(field, sign, "X", "O", i, line);

                            xyValue += CalculateXY(field, sign, "X", "O", line, i);

                            yxValue = CalculateYX(field, sign, "X", "O", line, i, yxValue);

                            break;
                    }
                }

                //Adds the values to the arrays
                yArray[line] = yValue;
                xArray[line] = xValue;
                xyArray[0] = xyValue;
                yxArray[0] = yxValue;

                line++;

                //Loops 3 times to go through all lines on the axis
            } while (line < 3);

            //Resets line to 0 to re-use
            line = 0;

            //Determines if computer should attack of defend based on the array values.
            while (moveControl == false && line < 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    //If it can make a winning move, it will do so.
                    moveControl = AttackOrDefend(field, sign, xArray, yArray, xyArray, yxArray, moveControl, i, 2);
                }

                if (moveControl == false)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        //If it can't make a winning move, and the opponent is able to, it will defend 
                        moveControl = AttackOrDefend(field, sign, xArray, yArray, xyArray, yxArray, moveControl, i, -2);
                    }
                }
                line++;
            }

            //If it cant win nor see a reason to defend, it will place its sign on a strategic place.
            moveControl = RndStrat(field, sign, rnd, moveControl);

            //Should it not be able to place it strategically it means the game will tie and it'll fill in wherever it can
            if (moveControl == false)
            {
                for (int i = 1; i < 9; i++)
                {
                    moveControl = PlayerMove(field, i, sign);

                    if (moveControl == true)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Calculates values on x and y axis
        /// </summary>
        /// <param name="field"></param>
        /// <param name="sign"></param>
        /// <param name="mySign"></param>
        /// <param name="opponentSign"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Int; value for the position to axis value</returns>
        static int CalculateAxisValue(string[,] field, string sign, string mySign, string opponentSign, int x, int y)
        {
            int value = 0;

            //Every time the computer sees it owwn sign, it adds a point for the axis value
            if (sign == mySign && field[x, y] == mySign)
                value++;

            //Every time the computer finds opponent sign, it subtracts a point from the axis value
            else if (sign == mySign && field[x, y] == opponentSign)
                value--;

            return value;
        }

        /// <summary>
        /// Calculates value for the first cross line (1, 5, 9)
        /// </summary>
        /// <param name="field"></param>
        /// <param name="sign"></param>
        /// <param name="mySign"></param>
        /// <param name="opponentSign"></param>
        /// <param name="line"></param>
        /// <param name="i"></param>
        /// <returns>Int; value for the line</returns>
        static int CalculateXY(string[,] field, string sign, string mySign, string opponentSign, int line, int i)
        {
            int value = 0;

            //The positions are easily found by using a for-loop
            if (line == 0)
            {
                //Will add point for every own sign found
                if (sign == mySign && field[i, i] == mySign)
                    value++;

                //Will subtract for every opponent sign found
                else if (sign == mySign && field[i, i] == opponentSign)
                    value--;
            }

            return value;
        }

        /// <summary>
        /// Adds value for the second cross line (3, 5, 7)
        /// </summary>
        /// <param name="field"></param>
        /// <param name="sign"></param>
        /// <param name="mySign"></param>
        /// <param name="opponentSign"></param>
        /// <param name="line"></param>
        /// <param name="i"></param>
        /// <param name="yxValue"></param>
        /// <returns>Returns value for the line</returns>
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

        /// <summary>
        /// Calculates XYValue
        /// </summary>
        /// <param name="field"></param>
        /// <param name="sign"></param>
        /// <param name="mySign"></param>
        /// <param name="opponentSign"></param>
        /// <param name="cond1"></param>
        /// <param name="cond2"></param>
        /// <returns>Returns int vlaue for the specific cordinate</returns>
        static int YXValue(string[,] field, string sign, string mySign, string opponentSign, int cond1, int cond2)
        {
            //Made to save space and reuse code
            int yxValue = 0;

            //Takes conditions for the positions that need to be checked
            //Adds point if own sign found
            if (sign == mySign && field[cond1, cond2] == mySign)
                yxValue++;

            //Substracts if oponnent sign found
            else if (sign == mySign && field[cond1, cond2] == opponentSign)
                yxValue--;

            return yxValue;
        }

        /// <summary>
        /// Initializes the player
        /// </summary>
        /// <param name="field"></param>
        /// <param name="turn"></param>
        /// <param name="bannerPrint"></param>
        /// <param name="gameboardPrint"></param>
        /// <returns>Bool for valid input</returns>
        static bool InititalaizePlayer(string[,] field, int turn, int bannerPrint, int gameboardPrint)
        {
            bool validInput = false;

            //Loops the players turn until a valid input has been used
            do
            {
                //Sets validInput to true when the player makes a valid move
                if (PlayerMove(field, PlayerInput(DetermineSign(turn)), DetermineSign(turn)) == true)
                    validInput = true;

                //Prints banner and gameboard
                FormatText(bannerPrint, Banner(), field);
                FormatText(gameboardPrint, GameBoard(), field);

            } while (validInput == false);
 
            return validInput;
        }

        /// <summary>
        /// Uses calculated move values to determine whether to place a winning move or defensive move
        /// </summary>
        /// <param name="field"></param>
        /// <param name="sign"></param>
        /// <param name="xArray"></param>
        /// <param name="yArray"></param>
        /// <param name="xyArray"></param>
        /// <param name="yxArray"></param>
        /// <param name="moveControl"></param>
        /// <param name="line"></param>
        /// <param name="playcondition"></param>
        /// <returns>Bool to see if it did a move</returns>
        static bool AttackOrDefend(string[,] field, string sign, int[] xArray, int[] yArray, int[] xyArray, int[] yxArray, bool moveControl, int line, int playcondition)
        {
            //Uses line to determine which line in the board it should play when it finds a matching condition
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

            //Controls the cross-board lines
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

        /// <summary>
        /// Makes a move on X-axis
        /// </summary>
        /// <param name="field"></param>
        /// <param name="sign"></param>
        /// <param name="cond1"></param>
        /// <param name="cond2"></param>
        /// <returns>Whether a move has been made</returns>
        static bool XArrayMove(string[,] field, string sign, int cond1, int cond2)
        {
            //Makes a move on the X-array
            bool moveControl = false;

            for (int i = cond1; i <= cond2; i += 3)
            {
                moveControl = PlayerMove(field, i, sign);

                if (moveControl == true)
                    break;
            }

            return moveControl;
        }
        
        /// <summary>
        /// Makes a move on Y-array
        /// </summary>
        /// <param name="field"></param>
        /// <param name="sign"></param>
        /// <param name="cond1"></param>
        /// <param name="cond2"></param>
        /// <returns>Weather a move has been made</returns>
        static bool YArrayMove(string[,] field, string sign, int cond1, int cond2)
        {
            //Makes a move on the Y-array
            bool moveControl = false;

            for (int i = cond1; i <= cond2; i++)
            {
                moveControl = PlayerMove(field, i, sign);

                if (moveControl == true)
                    break;
            }

            return moveControl;
        }

        /// <summary>
        /// Makes a random move on startegic places (1, 3, 5, 7, 9)
        /// </summary>
        /// <param name="field"></param>
        /// <param name="sign"></param>
        /// <param name="rnd"></param>
        /// <param name="moveControl"></param>
        /// <returns>Whether a move has been made</returns>
        static bool RndStrat(string[,] field, string sign, Random rnd, bool moveControl)
        {
            int computerInput = -1,
                line = 0;

            while (moveControl == false && line <= 9)
            {
                int initialInput = rnd.Next(1, 10);

                if (initialInput % 2 != 0)
                {
                    computerInput = initialInput;
                }

                moveControl = PlayerMove(field, computerInput, sign);

                line++;
            }

            return moveControl;
        }
        #endregion
    }
}