using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame
{
    internal class AI
    {
        private Field board;
        private Hand botHand;
        private List<float[]> values = new List<float[]>();

        //-----------------------------------------------------------------------------
        //Constructor
        public AI(Field board, Hand botHand)
        {
            this.board = board;
            this.botHand = botHand;
        }

        //******************************************************************************
        //Private Methods

        //-------------------------------------------------------------------------------------
        //Finds the first index of the values array which has the biggest sum of it's 4 values
        private sbyte FindBiggestValueOfCard(sbyte index)
        {
            sbyte biggestIndex = 0;
            for (sbyte i = 0; i < 9; i++)
            {
                if (values[index][i] > values[index][biggestIndex])
                {
                    biggestIndex = i;
                }//if
            }//for

            return biggestIndex;
        }

        //----------------------------------------------------------
        private sbyte TheHighestValueCard()
        {
            float biggestvalue = -10000;
            sbyte biggestindex = 0;
            for (sbyte i = 0; i < botHand.Size; i++)
            {
                float temp = values[i][FindBiggestValueOfCard(i)];
                if (biggestvalue < temp)
                {
                    biggestindex = i;
                    biggestvalue = temp;
                }
            }
            return biggestindex;
        }

        //-------------------------------------------------------------------------------------------
        //Fills the Values with the apropiate starter data
        private void FillValues()
        {
            values.Clear();
            for (sbyte i = 0; i < botHand.Size; i++)
            {
                float[] temp = new float[9];
                for (sbyte j = 0; j < 9; j++)
                {
                    temp[j] = board.IsOccupied(j) ? -100 : 0;
                }
                values.Add(temp);
            }//for
        }

        //---------------------------------------------------------------------------------------------------
        /// <summary>
        /// uses f(t)=1.5^(t-3) for true values
        /// uses f(x)=-1/(x-3.1) for false values
        /// </summary>
        /// <param name="whichSidesAreFree"> The Sides which are free to attack</param>
        /// <returns>The Sum value of the card there</returns>
        private float CalculateValue(Card input, bool[] whichSidesAreFree)
        {
            float sum = 0;
            byte[] attacks = new byte[] { input.LeftAttack, input.UpAttack, input.RightAttack, input.DownAttack };

            for (int i = 0; i < 4; i++)
            {
                if (whichSidesAreFree[i])
                    sum += TrueFunction(attacks[i]);
                else
                    sum += FalseFunction(attacks[i]);
            }

            return sum;
        }

        //-------------------------------------------------------------------------------------
        //returns f(x)=-1/(x-3.1)
        private float FalseFunction(byte attackValue)
        {
            return -10 / (attackValue - 3.1f);
        }

        //-----------------------------------------------------------------------------------
        //returns f(x)=1.5^(x-3)
        private float TrueFunction(byte attackValue)
        {
            return (float)Math.Pow(1.5, attackValue - 3);
        }

        private void GoThroughTheCards()
        {
            for (sbyte i = 0; i < botHand.Size; i++)
            {
                Card temp = botHand.GetCardByIndex(botHand.GetIndexes()[i]);
                //The scores the card will get at different places
                for (sbyte j = 0; j < 3; j++)
                {
                    values[i][j] += CalculateValue(temp, new bool[] { false, j != 0, true, j != 2 });
                    values[i][j + 1] += CalculateValue(temp, new bool[] { true, j != 0, true, j != 2 });
                    values[i][j + 2] += CalculateValue(temp, new bool[] { true, j != 0, false, j != 2 });
                }//for
                //Check for if it can take over a card
                for (sbyte j = 0; j < 9; j++)
                {
                    if (!board.IsOccupied(j))
                    {
                        values[i][j] += board.CanTakeOver(j, temp) * 1000;
                    }
                }//for
            }//for
        }

        //**************************************************************************
        //Public Methods
        public sbyte[] Next()
        {
            FillValues();
            GoThroughTheCards();
            sbyte theCard = botHand.GetIndexes()[TheHighestValueCard()];
            sbyte where = FindBiggestValueOfCard(TheHighestValueCard());

            return new sbyte[] { where, theCard };
        }
    }
}