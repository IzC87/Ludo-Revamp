using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ludo_Revamp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Engine Engine = new Engine();

        public MainWindow()
        {
            // Initialize the GUI
            InitializeComponent();

            // Launch configuration
            LaunchInitialization();

            // Event to close the window with escape for speed
            PreviewKeyDown += new KeyEventHandler(HandleEscapeKeyPress);
        }

        private void LaunchInitialization()
        {
            // InitializeEngine
            Engine.LaunchConfiguration();
        }

        public void RollDie_Click(object sender, RoutedEventArgs e)
        {

        }
        private void PlayerToken_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        public void NewGame_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HandleEscapeKeyPress(object sender, KeyEventArgs e)
        {
            // Close the window with the Escape key
            if (e.Key == Key.Escape) Close();
        }
    }
}
