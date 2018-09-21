using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B17_Ex02
{
    class Board
    {
        private const int k_MaxGuessLength = 4;
        private short[,] m_BoardData;
        private short m_SizeOfBoard;
        private short m_CurrentGuessIndex;
        private struct m_VAndX
        {
            public short howManyVinGuess;
            public short howManyXinGuess;
        }
        private m_VAndX[] m_VandXinLine;

        public Board(short i_SizeOfBoard)
        {
            this.m_BoardData = new short[i_SizeOfBoard, k_MaxGuessLength];
            this.m_SizeOfBoard = i_SizeOfBoard;
            this.m_CurrentGuessIndex = 0;
            this.m_VandXinLine = new m_VAndX[i_SizeOfBoard];
        }

        public short currentBoardIndex
        {
            get { return m_CurrentGuessIndex; }
        }

        public short getHowManyXperLine(short i_LineIndex)
        {
            return m_VandXinLine[i_LineIndex].howManyXinGuess;
        }

        public short getHowManyVperLine(short i_LineIndex)
        {
            return m_VandXinLine[i_LineIndex].howManyVinGuess;
        }

        public void addGuessToBoard(short[] i_Guesses, short i_HowManyVinLine, short i_HowManyXinLine)
        {
            for (int i = 0; i < k_MaxGuessLength; i++)
            {
                m_BoardData[m_CurrentGuessIndex, i] = i_Guesses[i];
            }

            this.m_VandXinLine[m_CurrentGuessIndex].howManyVinGuess = i_HowManyVinLine;
            this.m_VandXinLine[m_CurrentGuessIndex].howManyXinGuess = i_HowManyXinLine;
            this.m_CurrentGuessIndex++;
        }

        public void printCheck()
        {
            foreach (short number in m_BoardData)
            {
                Console.WriteLine(number);
            }
        }
    }
}
