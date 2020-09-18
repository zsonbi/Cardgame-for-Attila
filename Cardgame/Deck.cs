using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cardgame
{
    internal class Deck
    {
        private readonly Random rnd = new Random(); //A simple random
        private List<Card> cards = new List<Card>(); //A List which stores the cards

        //Properties
        //Gets the current Size of the deck
        public int Size { get => cards.Count(); }

        //Constructor
        public Deck(string DeckFileName)
        {
            string json;

            using (StreamReader r = new StreamReader(DeckFileName))
            {
                json = r.ReadToEnd();
                cards = JsonConvert.DeserializeObject<List<Card>>(json);
            }
        }

        //**************************************************************************
        //Public Methods
        //Draws an array of cards
        public Card[] Draw(sbyte numberOfCardsToDraw)
        {
            Card[] output = new Card[5];
            for (int i = 0; i < numberOfCardsToDraw; i++)
            {
                Card temp = cards[rnd.Next(0, Size)];
                cards.Remove(temp);
                output[i] = temp;
            }
            return output;
        }
    }
}