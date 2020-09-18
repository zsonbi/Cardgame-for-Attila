using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame
{
    internal class Deck
    {
        private readonly Random rnd = new Random(); //A simple random
        private List<Card> cards = new List<Card>(); //A List which stores the cards

        //Properties
        //Gets the Size of the deck
        public int Size { get; private set; }

        //Constructor
        public Deck(string DeckFileName)
        {
            string json;
            try
            {
                using (StreamReader r = new StreamReader(DeckFileName))
                {
                    json = r.ReadToEnd();
                    cards = JsonConvert.DeserializeObject<List<Card>>(json);
                }
            }
            catch (Exception)
            {
                throw new Exception("Deck doesn't exist");
            }
            Size = cards.Count();
        }

        //Draws a card from the deck
        public Card Draw()
        {
            Card output = cards[rnd.Next(0, cards.Count)];
            cards.Remove(output);
            return output;
        }
    }
}