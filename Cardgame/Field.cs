using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cardgame
{
    internal class Field
    {
        private sbyte numberOfPlacedCards = 0;//The number of cards played
        private Card[] cards = new Card[9];//The cards which are placed down
        private bool[] occupied = new bool[9];//The value is false if the index is empty
        private Border[] CardObjects = new Border[9];
        private Grid FieldGrid;

        //Properties
        public sbyte RedScore { get; private set; } // Score of RedPlayer

        public sbyte BlueScore { get; private set; } // Score of BluePlayer

        //Constructor
        public Field(Grid FieldGrid, sbyte handsize)
        {
            this.FieldGrid = FieldGrid;
            RedScore = 0;
            BlueScore = 0;
            ResetGrid();
        }

        //**********************************************************************
        //private Methods
        //Check if the Neighbours have smaller attacks than the card
        private void CheckNeighbours(sbyte index)
        {
            //So the computer doesn't do useless stuff
            if (numberOfPlacedCards == 0)
            {
                return;
            }//if

            //Left check
            if (index - 1 >= 0 && occupied[index - 1] && index % 3 != 0)
            {
                if (cards[index].LeftAttack > cards[index - 1].RightAttack)
                {
                    ChangeSide((sbyte)(index - 1), cards[index].Side);
                }//if
            }//if
            //Up check
            if (index - 3 >= 0 && occupied[index - 3])
            {
                if (cards[index].UpAttack > cards[index - 3].DownAttack)
                {
                    ChangeSide((sbyte)(index - 3), cards[index].Side);
                }//if
            }//if
            //Right check
            if (index + 1 < 9 && occupied[index + 1] && index % 3 != 2)
            {
                if (cards[index].RightAttack > cards[index + 1].LeftAttack)
                {
                    ChangeSide((sbyte)(index + 1), cards[index].Side);
                }//if
            }//if
            //Down check
            if (index + 3 < 9 && occupied[index + 3])
            {
                if (cards[index].DownAttack > cards[index + 3].UpAttack)
                {
                    ChangeSide((sbyte)(index + 3), cards[index].Side);
                }//if
            }//if
        }

        //-----------------------------------------------------------------------
        //Change the card's owner
        private void ChangeSide(sbyte index, bool Side)
        {
            cards[index].Side = Side;
            CardObjects[index].Background = !Side ? Brushes.Red : Brushes.Blue;
            if (!Side)
            {
                RedScore++;
                BlueScore--;
            }//if
            else
            {
                BlueScore++;
                RedScore--;
            }//else
        }

        //-----------------------------------------------------------------------
        //Places a card at the specified index
        private void PlaceCardInGrid(sbyte index, Uri input, bool Side)
        {
            Border border = new Border();
            border.BorderThickness = new Thickness(1);
            border.Background = !Side ? Brushes.Red : Brushes.Blue;
            Image temp = new Image();
            temp.Source = new System.Windows.Media.Imaging.BitmapImage(input);
            temp.Name = Side + "R" + index;
            temp.Margin = new Thickness(2);
            Grid.SetColumn(border, index % 3);
            Grid.SetRow(border, index / 3);
            border.Child = temp;
            Grid.SetZIndex(border, 20);
            CardObjects[index] = border;
            occupied[index] = true;
            FieldGrid.Children.Add(border);
        }

        //------------------------------------------------------------------------
        //Resets the grid deletes the cards in it
        private void ResetGrid()
        {
            for (int i = 0; i < FieldGrid.Children.Count; i++)
            {
                //If the element is a Border (card) remove it
                if (FieldGrid.Children[i].GetType().Name == "Border")
                {
                    FieldGrid.Children.Remove(FieldGrid.Children[i] as Border);
                    i--;
                }//if
            }//for
        }

        //**************************************************************************
        //Public Methods
        //Puts down a card on the board
        public void PutDownCard(sbyte index, Card thePlacedCard)
        {
            thePlacedCard.index = index;
            cards[index] = thePlacedCard;
            CheckNeighbours(index);
            PlaceCardInGrid(index, thePlacedCard.PictureUri, thePlacedCard.Side);
            numberOfPlacedCards++;
            if (!thePlacedCard.Side)
                RedScore++;
            else
                BlueScore++;
        }

        //--------------------------------------------------------------------------
        //Checks if the board is full
        public bool IsOver()
        {
            return numberOfPlacedCards == 9;
        }

        //Returns if the indexed place is occupied or not
        public bool IsOccupied(sbyte index)
        {
            return occupied[index];
        }

        public sbyte CanTakeOver(sbyte index, Card input)
        {
            sbyte output = 0;
            //So the computer doesn't do useless stuff
            if (numberOfPlacedCards == 0)
            {
                return 0;
            }//if

            //Left check
            if (index - 1 >= 0 && occupied[index - 1] && index % 3 != 0)
            {
                if (input.LeftAttack > cards[index - 1].RightAttack)
                {
                    if (cards[index - 1].Side != input.Side)
                    {
                        output++;
                    }
                }//if
            }//if
            //Up check
            if (index - 3 >= 0 && occupied[index - 3])
            {
                if (input.UpAttack > cards[index - 3].DownAttack)
                {
                    if (cards[index - 3].Side != input.Side)
                    {
                        output++;
                    }
                }//if
            }//if
            //Right check
            if (index + 1 < 9 && occupied[index + 1] && index % 3 != 2)
            {
                if (input.RightAttack > cards[index + 1].LeftAttack)
                {
                    if (cards[index + 1].Side != input.Side)
                    {
                        output++;
                    }
                }//if
            }//if
            //Down check
            if (index + 3 < 9 && occupied[index + 3])
            {
                if (input.DownAttack > cards[index + 3].UpAttack)
                {
                    if (cards[index + 3].Side != input.Side)
                    {
                        output++;
                    }
                }//if
            }//if
            return output;
        }
    }
}