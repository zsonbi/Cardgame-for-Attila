using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame
{
    internal class Hand
    {
        //private varriables
        private List<Card> cards = new List<Card>();

        //-----------------------------------------------------------------------------------
        //Properties
        //So we can ask for the size of the list
        public sbyte Size { get => (sbyte)cards.Count(); }

        //The Side of the hand (red: false blue: true)
        public bool Side { get; private set; }

        //--------------------------------------------------------------------------------------
        //Constructor
        public Hand(Card[] cardsToFillTheHand, bool Side)
        {
            this.Side = Side;
            //Adding the indexes to the cards where they will be
            for (sbyte i = 0; i < cardsToFillTheHand.Length; i++)
            {
                cardsToFillTheHand[i].index = i;
                cardsToFillTheHand[i].Side = Side;
            }
            //Adds the cards to the List
            cards.AddRange(cardsToFillTheHand);
            //So the List doesn't wast memory
            cards.TrimExcess();
            //Changes every card to the Side of the hand
            cards.ForEach(x => x.Side = Side);
        }

        //**********************************************************************************
        //Public Methods
        //Remove a card from the hand
        public void RemoveCard(byte index)
        {
            cards.RemoveAt(index);
        }
    }
}