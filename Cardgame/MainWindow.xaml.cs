using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Cardgame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;

        public MainWindow()
        {
            InitializeComponent();
            game = new Game(Playfield, Player1Grid, Player2Grid);
        }

        //***********************************************************************************
        //Handlers
        //Click Event for the cards
        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            game.PlaceCard(Convert.ToSByte((sender as Rectangle).Name.Split('t')[1]));
        }
    }
}