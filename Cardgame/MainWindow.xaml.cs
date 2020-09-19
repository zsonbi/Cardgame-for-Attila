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
        private Settings settingsWindow;

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

        //------------------------------------------------------------------------------------------
        //Brings forth the settingsWindow
        private void Settings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            settingsWindow = new Settings();
            settingsWindow.Show();
            settingsWindow.NewGamebtn.Click += NewGamebtn_Click;
        }

        //-----------------------------------------------------------------------------------------
        //So the user can't close the game and the settings remain bugged
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            settingsWindow.Close();
        }

        //----------------------------------------------------------------------
        //Save Button Click event -,-
        private void NewGamebtn_Click(object sender, RoutedEventArgs e)
        {
            settingsWindow.Close();
            game = new Game(Playfield, Player1Grid, Player2Grid);
        }
    }
}