using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B17_Ex02
{
    class UserInterface
    {
        private int m_NumOfGuesses;
        private string m_UserNextGuess;
        private readonly byte r_MinNumOfGuesses = 4;
        private readonly byte r_MaxNumOfGuesses = 10;
        private bool m_WinOrLoose = false; // win the game -> true
        private bool m_ContinueGame = true;
        private int m_CurrentIndexToinsertGuessToPrintBoard;
        private List<StringBuilder> m_PrintingBoard;
        private GameLogic m_GameLogic;

        public void StartUserInterFace()
        {
            int stepsCounter = 0;
            bool playerDecideToQuit = false;

            while (!playerDecideToQuit)
            {
                getNumOfGuessesFromUser();
                m_GameLogic = new GameLogic((short)m_NumOfGuesses);
                initPrintingBoard();
                printPrintingBoard();

                while (m_ContinueGame)
                {
                    stepsCounter++;
                    getGuessAndStartNewRound();
                    insertGuessToPrintingBoard();
                    printPrintingBoard();
                }

                if (m_WinOrLoose)
                {
                    Console.WriteLine(@"
you guessed after {0} steps!
would you like to start a new game? <Y/N>", stepsCounter);
                }
                else
                {
                    Console.WriteLine(@"
no more guesses allowed, you lost. 
would you like to start a new game? <Y/N>");
                }

                string inputFromUser = Console.ReadLine();
                if (inputFromUser.ToUpper() == "N")
                {
                    playerDecideToQuit = true;
                }

                Ex02.ConsoleUtils.Screen.Clear();
                m_ContinueGame = true;

                stepsCounter = 0;
            }

            Console.WriteLine("Bye bye");
        }

        private void initPrintingBoard()
        {
            StringBuilder firstDefaultLine = new StringBuilder("|Pins:    |Result:|");
            StringBuilder spacesLineForLaterGuess = new StringBuilder("|         |       |");
            StringBuilder secondDefaultLine = new StringBuilder("| # # # # |       |");
            StringBuilder regularPrintingLine = new StringBuilder("|=========|=======|");
            m_PrintingBoard = new List<StringBuilder>(2 * m_NumOfGuesses);
            m_PrintingBoard.Add(firstDefaultLine);
            m_PrintingBoard.Add(regularPrintingLine);
            m_PrintingBoard.Add(secondDefaultLine);
            m_PrintingBoard.Add(regularPrintingLine);

            for (int i = 0; i < 2 * m_NumOfGuesses; i++)
            {
                if (i % 2 != 0)
                {
                    StringBuilder addRegularPrintingLine = new StringBuilder("|=========|=======|");
                    m_PrintingBoard.Add(addRegularPrintingLine);
                }
                else
                {
                    StringBuilder addSpacesPrintingLine = new StringBuilder("|         |       |");
                    m_PrintingBoard.Add(addSpacesPrintingLine);
                }
            }

            m_CurrentIndexToinsertGuessToPrintBoard = 4;
        }

        private void printPrintingBoard()
        {
            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine("current board status:{0}", Environment.NewLine);

            foreach (StringBuilder line in m_PrintingBoard)
            {
                Console.WriteLine(line);
            }
        }

        private void insertGuessToPrintingBoard()
        {
            int indexForLatters = 0;
            int indexForXAndY = 11;

            for (int i = 2; i < 10; i += 2)
            {
                m_PrintingBoard[m_CurrentIndexToinsertGuessToPrintBoard][i] = m_UserNextGuess[indexForLatters];
                indexForLatters++;
            }
            for (int i = 0; i < m_GameLogic.HowManyVinGuess(); i++)
            {
                m_PrintingBoard[m_CurrentIndexToinsertGuessToPrintBoard][indexForXAndY] = 'V';
                indexForXAndY += 2;
            }
            for (int i = 0; i < m_GameLogic.HowManyXinGuess(); i++)
            {
                m_PrintingBoard[m_CurrentIndexToinsertGuessToPrintBoard][indexForXAndY] = 'X';
                indexForXAndY += 2;
            }

            m_CurrentIndexToinsertGuessToPrintBoard += 2;
        }

        private void getNumOfGuessesFromUser()
        {
            bool validInput = false;
            string inputFromUser;
            string msgToUser = string.Format("Hello! please enter maximum number of guesses, between {0} - {1}: ", r_MinNumOfGuesses, r_MaxNumOfGuesses);
            string msgTryAgain = string.Format(@"Pay attention, you need to enter a number between {0} - {1}.
Please try again:", r_MinNumOfGuesses, r_MaxNumOfGuesses);
            string msgInvalidInput = string.Format("You entered a invalid input!");
            string msgOutOfRangeInput = string.Format("You entered number out of range!");
            Console.WriteLine(msgToUser);
            inputFromUser = Console.ReadLine();
            validInput = int.TryParse(inputFromUser, out m_NumOfGuesses);

            while (!validInput || m_NumOfGuesses < 4 || m_NumOfGuesses > 10)
            {
                if (!validInput)
                {
                    Console.WriteLine(msgInvalidInput);
                    Console.WriteLine(msgTryAgain);
                    inputFromUser = Console.ReadLine();
                    validInput = int.TryParse(inputFromUser, out m_NumOfGuesses);
                }
                else
                {
                    Console.WriteLine(msgOutOfRangeInput);
                    Console.WriteLine(msgTryAgain);
                    inputFromUser = Console.ReadLine();
                    validInput = int.TryParse(inputFromUser, out m_NumOfGuesses);
                }
            }

        }

        private void getGuessAndStartNewRound()
        {
            string inputFromUser = getNextGuessFromUser();
            runGameLogic(inputFromUser);
        }

        private string getNextGuessFromUser()
        {
            bool syntaxInvalidInput = false, rangeInvalidInput = false, quitTheGame = false;
            string msgToUser = string.Format("{0}Please type your next guess <A B C D> or 'Q' to quit", Environment.NewLine);
            string msgTryAgain = string.Format(@"Pay attention, you need to enter 4 letters between {0} - {1}.
Please try again:", m_GameLogic.GetMinRangeForGuessesAsLetter(), m_GameLogic.GetMaxRangeForGuessesAsLetter());
            string msgSyntaxInvalidInput = string.Format("You entered a invalid input!");
            string msgOutOfRangeInput = string.Format("You entered a letter out of range!");

            Console.WriteLine(msgToUser);
            string inputFromUser = Console.ReadLine();
            string inputFromUserUpper = inputFromUser.ToUpper();
            quitTheGame = checkIfUserWantToQuit(inputFromUserUpper);
            syntaxInvalidInput = checkValidityOfGuessSyntax(inputFromUserUpper);// syntax-> UI check
            rangeInvalidInput = m_GameLogic.checkValidityOfGuessRange(inputFromUserUpper);// range-> logic check

            while ((syntaxInvalidInput || rangeInvalidInput) && !quitTheGame)
            {
                if (syntaxInvalidInput)
                {
                    Console.WriteLine(msgSyntaxInvalidInput);
                }
                else
                {
                    Console.WriteLine(msgOutOfRangeInput);
                }

                Console.WriteLine(msgTryAgain);
                inputFromUser = Console.ReadLine();
                inputFromUserUpper = inputFromUser.ToUpper();
                quitTheGame = checkIfUserWantToQuit(inputFromUserUpper);
                syntaxInvalidInput = checkValidityOfGuessSyntax(inputFromUserUpper);// syntax-> UI check
                rangeInvalidInput = m_GameLogic.checkValidityOfGuessRange(inputFromUserUpper);// range-> logic check
            }

            if (quitTheGame)
            {
                Console.WriteLine("Bye bye");
                Environment.Exit(0);
            }

            return inputFromUserUpper.Replace(" ", "");
        }

        private void runGameLogic(string i_InputFromUser)
        {
            m_UserNextGuess = i_InputFromUser;
            short[] guessForLogic = new short[m_GameLogic.MaxGuessLen];
            int index = 0;

            foreach (char letter in m_UserNextGuess)
            {
                if (letter != ' ')
                {
                    guessForLogic[index] = (short)(letter - 64); // convert letters to numbers;
                    index++;
                }
            }

            m_ContinueGame = m_GameLogic.RunRound(guessForLogic, ref m_WinOrLoose);
        }

        private bool checkValidityOfGuessSyntax(string i_InputFromUser)
        {
            bool invalidInput = false;
            int countHomManyletters = 0;

            foreach (char letter in i_InputFromUser)
            {
                if (letter != ' ')
                {
                    countHomManyletters++;

                    if (letter > 'Z' || letter < 'A')// not a letter
                    {
                        invalidInput = true;
                        break;
                    }
                }
            }

            if (countHomManyletters != m_GameLogic.MaxGuessLen)
            {
                invalidInput = true;
            }

            return invalidInput;
        }

        private bool checkIfUserWantToQuit(string i_InputFromUser)
        {
            bool quitGame;

            if (i_InputFromUser == "Q")
            {
                quitGame = true;
            }
            else
            {
                quitGame = false;
            }

            return quitGame;
        }
    }
}
