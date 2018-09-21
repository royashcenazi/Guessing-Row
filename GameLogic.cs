using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B17_Ex02
{
    class GameLogic
    {
        private const short k_MinRangeForGuesses = 1;
        private const short k_MaxRangeForGuesses = 8;
        private const short k_MaxGuessLen = 4;
        private const short k_ConvertToLetter = 64;
        private Board m_GameBoard;
        private short m_HowManyGuesses;
        private short[] m_TheRandomGuess;
        private short m_NumberOfX, m_NumberOfV;
        private short m_CurrentGameRound;

        public GameLogic(short i_NumberOfGuesses)
        {
            this.m_CurrentGameRound = 1;
            this.m_HowManyGuesses = i_NumberOfGuesses;
            this.m_TheRandomGuess = new short[k_MaxGuessLen];
            this.m_GameBoard = new Board(this.m_HowManyGuesses);
            makeNewGuess();
        }

        public short MaxGuessLen
        {
            get
            {
                return k_MaxGuessLen;
            }
        }

        public char GetMinRangeForGuessesAsLetter()
        {
            char theLetter = (char)(k_ConvertToLetter + k_MinRangeForGuesses);

            return theLetter;
        }

        public char GetMaxRangeForGuessesAsLetter()
        {
            char theLetter = (char)(k_ConvertToLetter + k_MaxRangeForGuesses);

            return theLetter;
        }

        private void makeNewGuess()
        {
            byte[] bucketForNotRepeatingNumbers = new byte[k_MaxRangeForGuesses];
            short indexToInsertNewRandedNumber = 0;
            System.Random numbersGenerator = new Random();

            while (indexToInsertNewRandedNumber < k_MaxGuessLen)
            {
                short numberToInsert = (short)numbersGenerator.Next(k_MinRangeForGuesses, k_MaxRangeForGuesses);

                if (bucketForNotRepeatingNumbers[numberToInsert - 1] == 0)
                {
                    bucketForNotRepeatingNumbers[numberToInsert - 1] = 1;
                    m_TheRandomGuess[indexToInsertNewRandedNumber] = numberToInsert;
                    indexToInsertNewRandedNumber++;
                }
            }
        }

        public bool RunRound(short[] i_GuessFromUser, ref bool o_PlayerWinOrLoose)
        {
            this.m_CurrentGameRound++;
            bool continueGame = true;

            if (m_CurrentGameRound <= m_HowManyGuesses)
            {
                o_PlayerWinOrLoose = checkGamePlayGuessFromUserAndCheckVictory(i_GuessFromUser);
            }
            else //no more guesses for player
            {
                continueGame = false;
                o_PlayerWinOrLoose = false;
            }

            if (o_PlayerWinOrLoose == true) //player won, stop game
            {
                continueGame = false;
            }

            return continueGame;
        }

        public short HowManyVinGuess()
        {
            short boardLineIndex = m_GameBoard.currentBoardIndex;
            boardLineIndex--;

            return m_GameBoard.getHowManyVperLine(boardLineIndex);
        }

        public short HowManyXinGuess()
        {
            short boardLineIndex = m_GameBoard.currentBoardIndex;
            boardLineIndex--;

            return m_GameBoard.getHowManyXperLine(boardLineIndex);
        }

        private bool checkGamePlayGuessFromUserAndCheckVictory(short[] i_GuessFromUser) //return true if player wins
        {
            bool gameResault = false;
            checkHowManyVAndXinGuess(i_GuessFromUser);
            m_GameBoard.addGuessToBoard(i_GuessFromUser, m_NumberOfV, m_NumberOfX);

            if (m_NumberOfV == k_MaxGuessLen)
            {
                gameResault = true;
            }

            resetNumberOfVAndXAfterRound();

            return gameResault;
        }

        private void resetNumberOfVAndXAfterRound()
        {
            this.m_NumberOfX = 0;
            this.m_NumberOfV = 0;
        }

        private void checkHowManyVAndXinGuess(short[] i_GuessFromUser)
        {
            for (short numberInUserGuess = 0; numberInUserGuess < k_MaxGuessLen; numberInUserGuess++)
            {
                for (short numberInRandedGuess = 0; numberInRandedGuess < k_MaxGuessLen; numberInRandedGuess++)
                {
                    if (i_GuessFromUser[numberInUserGuess] == m_TheRandomGuess[numberInRandedGuess])
                    {
                        if (numberInUserGuess == numberInRandedGuess)
                        {
                            this.m_NumberOfV++;
                        }
                        else
                        {
                            this.m_NumberOfX++;
                        }
                    }
                }
            }
        }

        public bool checkValidityOfGuessRange(string i_InputFromUser)
        {
            bool invalidInput = false;

            foreach (char letter in i_InputFromUser)
            {
                if (letter != ' ')
                {
                    if (letter < (char)(k_MinRangeForGuesses + k_ConvertToLetter) || letter > (char)(k_MaxRangeForGuesses + k_ConvertToLetter))
                    {
                        invalidInput = true;
                        break;
                    }
                }
            }

            return invalidInput;
        }
    }
}
