using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Cardgame
{
    internal class Game
    {
        //Private Varriables

        private Deck player1Deck;//redplayer's deck
        private Deck player2Deck;//blueplayer's deck
        private Hand player1Hand;//redplayer's hand
        private Hand player2Hand;//blueplayer's hand
        private Grid FieldGrid;
        private Grid Player1Grid;
        private Grid Player2Grid;
        private Field board;
        private bool CurrentSide = false; //The side which comes next
        private sbyte CurrentSelectedCardIndex = -1; //The current

        //--------------------------------------------------------------------
        //Properties
        //The Score of the red player
        public sbyte player1Score { get; private set; }

        //The score of the blue player
        public sbyte player2Score { get; private set; }

        //-------------------------------------------------------------------------------
        //Constructor
        public Game(Grid FieldGrid, Grid Player1Grid, Grid Player2Grid)
        {
            this.FieldGrid = FieldGrid;
            this.Player1Grid = Player1Grid;
            this.Player2Grid = Player2Grid;
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

        //--------------------------------------------------------------------------------
        //Creates the hand (currently handsize doesn't work yet)
        private void Createhands(sbyte handsize = 5)
        {
            player1Hand = new Hand(player1Deck.Draw(handsize), false);
            player2Hand = new Hand(player2Deck.Draw(handsize), true);
        }

        //-----------------------------------------------------------------------------
        //Makes a border which contains the card's image
        private Border CreateCard(sbyte index, Uri Picture, bool Side)
        {
            Border border = new Border();
            border.BorderThickness = new Thickness(1);
            border.Background = Brushes.Black;
            Image temp = new Image();
            temp.Source = new System.Windows.Media.Imaging.BitmapImage(Picture);
            temp.Name = Side + "R" + index;
            temp.MouseLeftButtonDown += SelectCard;
            temp.Margin = new Thickness(2);
            Grid.SetColumn(border, index % 2);
            Grid.SetRow(border, index / 2);
            border.Child = temp;
            return border;
        }

        //---------------------------------------------------------------------------------
        //When the game starts place the cards for the players
        private void UpdatePlayerGrids()
        {
            Player1Grid.Children.Clear();
            Player2Grid.Children.Clear();

            for (sbyte i = 0; i < player1Hand.Size; i++)
            {
                Player1Grid.Children.Add(CreateCard(i, player1Hand.GetUri(i), player1Hand.Side));
            }//for

            for (sbyte i = 0; i < player2Hand.Size; i++)
            {
                Player2Grid.Children.Add(CreateCard(i, player2Hand.GetUri(i), player2Hand.Side));
            }//for
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
            UpdatePlayerGrids();
            board = new Field(FieldGrid);
        }

        //--------------------------------------------------------------------------------------
        //Places the card on the board
        public void PlaceCard(sbyte index)
        {
            if (CurrentSelectedCardIndex == -1)
            {
                return;
            }//if
            if (!CurrentSide)
            {
                board.PutDownCard(index, player1Hand.GetCardByIndex(CurrentSelectedCardIndex));
                (Player1Grid.Children[CurrentSelectedCardIndex] as Border).Visibility = Visibility.Hidden;
            }//if
            else
            {
                board.PutDownCard(index, player2Hand.GetCardByIndex(CurrentSelectedCardIndex));
                (Player2Grid.Children[CurrentSelectedCardIndex] as Border).Visibility = Visibility.Hidden;
            }//else
            CurrentSelectedCardIndex = -1;
            //Change the current player
            CurrentSide = !CurrentSide;
        }

        //***********************************************************************************
        //Handlers
        //Click Event for the cards
        private void SelectCard(object sender, MouseButtonEventArgs e)
        {
            sbyte index = Convert.ToSByte((sender as Image).Name.Split('R')[1]);

            //So that the other player can interact with the card
            if (CurrentSide != Convert.ToBoolean((sender as Image).Name.Split('R')[0]))
            {
                return;
            }//if
            //So we can deselect
            if (CurrentSelectedCardIndex == index)
            {
                ((!CurrentSide ? Player1Grid : Player2Grid).Children[CurrentSelectedCardIndex] as Border).Background = Brushes.Black;
                CurrentSelectedCardIndex = -1;
                return;
            }//if
            if (CurrentSelectedCardIndex != -1)
            {
                ((!CurrentSide ? Player1Grid : Player2Grid).Children[CurrentSelectedCardIndex] as Border).Background = Brushes.Black;
            }

            ((sender as Image).Parent as Border).Background = Brushes.Green;
            CurrentSelectedCardIndex = index;
        }
    }
}