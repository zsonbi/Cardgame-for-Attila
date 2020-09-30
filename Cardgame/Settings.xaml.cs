using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cardgame
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            //Give the default values to the input fields
            switch (Properties.Settings.Default.gameType)
            {
                case 0:
                    HotSeatRadio.IsChecked = true;
                    break;

                case 1:
                    AIRadio.IsChecked = true;
                    break;

                case 2:
                    ShowBothHandsRadio.IsChecked = true;
                    break;

                default:
                    break;
            }
            Timetbox.Text = Properties.Settings.Default.WaitTime.ToString();
        }

        //*************************************************************************************
        //Private Methods
        //Saves the content of the inputs into the settings
        private void Save()
        {
            Properties.Settings.Default.WaitTime = (float)Convert.ToDouble(Timetbox.Text);
            if ((bool)HotSeatRadio.IsChecked)
            {
                Properties.Settings.Default.gameType = 0;
            }//if
            else if ((bool)AIRadio.IsChecked)
            {
                Properties.Settings.Default.gameType = 1;
            }//else if
            else
            {
                Properties.Settings.Default.gameType = 2;
            }//else
            Properties.Settings.Default.Save();
        }

        //****************************************************************************************
        //Handlers
        //Only accept numbers and 1 comma
        private void OnlyNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            if (e.Text == ",")
            {
                e.Handled = (sender as TextBox).Text.Contains(",");
            }
        }

        //----------------------------------------------------------------------
        //Save Button Click event -,-
        private void Savebtn_Click(object sender, RoutedEventArgs e)
        {
            Save();
            this.Close();
        }

        //-----------------------------------------------------------------------------------------
        //When the user closes the window saves the content of the inputs
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Save();
        }
    }
}