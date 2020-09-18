using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Cardgame
{
    internal class Field
    {
        //The cards which are placed down
        private List<Card> cards = new List<Card>();

        private Grid FieldGrid;

        //Constructor
        public Field(Grid FieldGrid)
        {
            this.FieldGrid = FieldGrid;
        }

        //private Methods
        //Check if the Neighbours have smaller attacks than the card
        private void CheckNeighbours(sbyte index)
        {
        }

        private void PlaceCardInGrid(sbyte index, Uri input)
        {
            Image temp = new Image();

            temp.Source = new System.Windows.Media.Imaging.BitmapImage(input);
            temp.Name = "Field" + "R" + index;
            Grid.SetZIndex(temp, 20);
            Grid.SetColumn(temp, index % 3);
            Grid.SetRow(temp, index / 3);
            FieldGrid.Children.Add(temp);
        }

        //**************************************************************************
        //Public Methods
        //Puts down a card on the board
        public void PutDownCard(sbyte index, Card thePlacedCard)
        {
            thePlacedCard.index = index;
            cards.Add(thePlacedCard);
            CheckNeighbours(index);
            PlaceCardInGrid(index, thePlacedCard.PictureUri);
        }
    }
}