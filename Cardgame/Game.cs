using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame
{
    internal class Game
    {
        //Private Varriables

        private Deck player1Deck;//redplayer's deck
        private Deck player2Deck;//blueplayer's deck
        private Hand player1Hand;//redplayer's hand
        private Hand player2Hand;//blueplayer's hand

        //--------------------------------------------------------------------
        //Properties
        //The Score of the red player
        public sbyte player1Score { get; private set; }

        //The score of the blue player
        public sbyte player2Score { get; private set; }

        //-------------------------------------------------------------------------------
        //Constructor
        public Game()
        {
            NewGame();
        }

        //*****************************************************************************
        //Private Methods
        //Creates the decks
        private void CreateDecks()
        {
            player1Deck = new Deck("..\\..\\Decks\\player1Deck.json");
            player2Deck = new Deck("..\\..\\Decks\\player2Deck.json");
        }

        //Creates the hand (currently handsize doesn't work yet)
        private void Createhands(sbyte handsize = 5)
        {
            player1Hand = new Hand(player1Deck.Draw(handsize), false);
            player2Hand = new Hand(player2Deck.Draw(handsize), true);
        }

        //********************************************************************************
        //Public Methods
        //Starts a new game resets everything
        public void NewGame()
        {
            CreateDecks();//Creates the decks
            Createhands();//Creates the hands
            player1Score = player1Hand.Size;//Updates the red player's score to default
            player2Score = player2Hand.Size;//Updates the blue player's score to default
        }
    }
}