using System;
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
        private Grid FieldGrid; //The Grid where the players will place their cards
        private Grid Player1Grid; //red player's hand
        private Grid Player2Grid; //blue player's hand
        private Field board; //The Board where the players will place their cards
        private bool CurrentSide; //The side which comes next
        private sbyte CurrentSelectedCardIndex = -1; //The current selected card
        private TimeSpan HotSeatTime; //The time it waits during the hotswap
        private byte gameType; //The type of game (0 hotseat, 1 AI, 2 Show Both hands ) 1 and 2 are unimplemented as of right now
        private Label[] scoreLabels = new Label[2]; //The labels where the scores are
        private AI bot;

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
            Properties.Settings.Default.Reload();
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

        //-------------------------------------------------------------------
        //Determines who won
        private string WhoWon()
        {
            if (player1Score > player2Score)
            {
                return "Red Wins";
            }
            else if (player1Score < player2Score)
            {
                return "Blue Wins";
            }
            else
            {
                return "Draw";
            }
        }

        //------------------------------------------------------------------
        //Creates a StackPanel which contains two labels
        private StackPanel CreateScoreLabels(bool Side)
        {
            StackPanel output = new StackPanel();
            Grid.SetColumn(output, Player1Grid.ColumnDefinitions.Count - 1);
            Grid.SetRow(output, Player1Grid.RowDefinitions.Count - 1);
            output.Orientation = Orientation.Horizontal;
            output.HorizontalAlignment = HorizontalAlignment.Right;
            Label scorelabel = new Label();
            scorelabel.Content = "Score";
            scorelabel.Foreground = !Side ? Brushes.Red : Brushes.Blue;
            scorelabel.FontSize = 25;
            scorelabel.VerticalAlignment = VerticalAlignment.Bottom;
            Label CounterLabel = new Label();
            CounterLabel.Content = "";
            CounterLabel.Foreground = !Side ? Brushes.Red : Brushes.Blue;
            CounterLabel.FontSize = 25;
            CounterLabel.VerticalAlignment = VerticalAlignment.Bottom;
            CounterLabel.Name = (!Side ? "Red" : "Blue") + "Score";

            output.Children.Add(scorelabel);
            output.Children.Add(CounterLabel);
            scoreLabels[!Side ? 0 : 1] = CounterLabel;

            return output;
        }

        //---------------------------------------------------------------------------
        //Updates the scores
        private void UpdateScore()
        {
            player1Score = (sbyte)(player1Hand.Size + board.RedScore);
            player2Score = (sbyte)(player2Hand.Size + board.BlueScore);
            scoreLabels[0].Content = player1Score;
            scoreLabels[1].Content = player2Score;
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
            Player1Grid.Children.Add(CreateScoreLabels(false));
            Player2Grid.Children.Add(CreateScoreLabels(true));
        }

        //-----------------------------------------------------------------------------
        //Hides the specified player's cards
        private void HidePlayersCards(bool Side)
        {
            sbyte[] player1Indexes = player1Hand.GetIndexes();
            sbyte[] player2Indexes = player2Hand.GetIndexes();
            for (int i = 0; i < player1Hand.Size; i++)
            {
                Player1Grid.Children[player1Indexes[i]].Visibility = !Side ? Visibility.Hidden : Visibility.Visible;
            }
            for (int i = 0; i < player2Hand.Size; i++)
            {
                Player2Grid.Children[player2Indexes[i]].Visibility = !Side ? Visibility.Visible : Visibility.Hidden;
            }
        }

        //---------------------------------------------------------------------------
        //Hotswap (hides both player's card for the specified time)
        private async Task HotSeat()
        {
            for (int i = 0; i < 5; i++)
            {
                Player1Grid.Children[i].Visibility = Visibility.Hidden;
                Player2Grid.Children[i].Visibility = Visibility.Hidden;
            }

            //The time to wait till the cards are shown again
            await Task.Delay(HotSeatTime);
            HidePlayersCards(!CurrentSide);
        }

        //********************************************************************************
        //Public Methods
        //Starts a new game resets everything
        public void NewGame()
        {
            CurrentSide = false;
            HotSeatTime = TimeSpan.FromSeconds(Properties.Settings.Default.WaitTime);
            gameType = Properties.Settings.Default.gameType;
            CreateDecks();//Creates the decks
            Createhands();//Creates the hands
            player1Score = player1Hand.Size;//Updates the red player's score to default
            player2Score = player2Hand.Size;//Updates the blue player's score to default
            UpdatePlayerGrids();
            board = new Field(FieldGrid, player1Hand.Size);
            if (gameType == 0 || gameType == 1)
                HidePlayersCards(true);
            UpdateScore();
            bot = new AI(board, player2Hand);
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
                player1Hand.RemoveCard(player1Hand.GetCardByIndex(CurrentSelectedCardIndex));
            }//if
            else
            {
                board.PutDownCard(index, player2Hand.GetCardByIndex(CurrentSelectedCardIndex));
                (Player2Grid.Children[CurrentSelectedCardIndex] as Border).Visibility = Visibility.Hidden;
                player2Hand.RemoveCard(player2Hand.GetCardByIndex(CurrentSelectedCardIndex));
            }//else
            CurrentSelectedCardIndex = -1;
            //Change the current player
            if (gameType != 1)
                CurrentSide = !CurrentSide;

            //UpdateScore
            UpdateScore();

            //When the board is full
            if (board.IsOver())
            {
                //Select a winner
                MessageBox.Show(WhoWon());
                return;
            }

            if (gameType == 1)
            {
                sbyte[] temp = bot.Next();

                board.PutDownCard(temp[0], player2Hand.GetCardByIndex(temp[1]));
                (Player2Grid.Children[temp[1]] as Border).Visibility = Visibility.Hidden;
                player2Hand.RemoveCard(player2Hand.GetCardByIndex(temp[1]));
            }

            if (gameType == 0)
                HotSeat();
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