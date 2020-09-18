using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame
{
    internal class Field
    {
        private List<Card> cards = new List<Card>();

        public void OutDownCard(byte index, Card thePlacedCard)
        {
            cards.Add(thePlacedCard);
        }
    }
}