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
using GameEngine.Classes;
using Newtonsoft.Json;
using Ludo_Revamp.Classes;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Ludo_Revamp
{
    public partial class MainWindow : Window
    {
        public Engine Engine = new Engine();

        public List<BoardPositionsGUI> BoardPositionsGUI = new List<BoardPositionsGUI>();
        public List<StartingPositionsGUI> StartingPositionsGUI = new List<StartingPositionsGUI>();
        public List<FinishPositionsGUI> FinishPositionsGUI = new List<FinishPositionsGUI>();

        public MainWindow()
        {
            // Initialize the GUI
            InitializeComponent();

            // Launch configuration
            LaunchInitialization();

            // Event to close the window with escape for speed
            PreviewKeyDown += new KeyEventHandler(HandleEscapeKeyPress);
        }

        public void RollDie_Click(object sender, RoutedEventArgs e)
        {

        }
        private void PlayerToken_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        public void NewGame_Click(object sender, RoutedEventArgs e)
        {
            // Initialize new game by setting up the GUI
            InitializeNewGameGUI(NumberOfPlayersList.SelectedIndex + NumberOfComputersList.SelectedIndex);

            // Initialize the engine for a new game
            Engine.InitializeNewGame(NumberOfPlayersList.SelectedIndex, NumberOfComputersList.SelectedIndex);

            // Name the new game
            Engine.Game.Name = GameNameBox.Text;

            // Save the newly started game
            SaveGame();

            // Start the new game
            Diebutton.IsEnabled = true;
            //PlayGame();
        }

        private void SaveGame()
        {
            Engine.Context.SaveChanges();
        }

        private void HandleEscapeKeyPress(object sender, KeyEventArgs e)
        {
            // Close the window with the Escape key
            if (e.Key == Key.Escape) Close();
        }
    }
}
