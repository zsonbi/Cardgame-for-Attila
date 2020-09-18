using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame
{
    internal class Field
    {
        //The cards which are placed down
        private List<Card> cards = new List<Card>();

        //private Methods
        //Check if the Neighbours have smaller attacks than the card
        private void CheckNeighbours(sbyte index)
        {
        }

        //**************************************************************************
        //Public Methods
        //Puts down a card on the board
        public void PutDownCard(sbyte index, Card thePlacedCard)
        {
            thePlacedCard.index = index;
            cards.Add(thePlacedCard);
            CheckNeighbours(index);
        }
    }
}