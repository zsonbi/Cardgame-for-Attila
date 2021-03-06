﻿using System;
using System.Collections.Generic;
using System.Linq;

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
        public void RemoveCard(Card Element)
        {
            cards.Remove(Element);
        }

        //--------------------------------------------------------------------------------
        //Gets the card's Uri
        public Uri GetUri(sbyte index)
        {
            return cards[index].PictureUri;
        }

        //--------------------------------------------------------------------------------
        //Search for the card with the specified index
        public Card GetCardByIndex(sbyte index)
        {
            return cards.Find(x => x.index == index);
        }

        //--------------------------------------------------------------------------
        //Get the cards indexes on the board
        public sbyte[] GetIndexes()
        {
            sbyte[] output = new sbyte[Size];
            for (int i = 0; i < Size; i++)
            {
                output[i] = cards[i].index;
            }
            return output;
        }
    }
}