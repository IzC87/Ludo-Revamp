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

        public List<Ellipse> Ellipses = new List<Ellipse>();

        public MainWindow()
        {
            // Initialize the GUI
            InitializeComponent();

            // Launch configuration
            LaunchInitialization();

            // Event to close the window with escape for speed
            PreviewKeyDown += new KeyEventHandler(HandleEscapeKeyPress);
        }

        private void PlayGame()
        {
            var tokensToMove = Engine.PlayGame();
            if (tokensToMove != null)
            {
                foreach (var token in tokensToMove)
                {
                    if (token != null)
                    {
                        //Engine.UpdatePlayerScore(token);
                        MoveToken(token);
                    }
                }
            }
        }

        public void RollDie_Click(object sender, RoutedEventArgs e)
        {
            int dieRoll = Engine.RollDie();
            Diebutton.Content = dieRoll;
            Diebutton.IsEnabled = false;
            Engine.DeselectSelectedTokens();
            PlayGame();
        }

        private void PlayerToken_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        public void LoadGame_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected game
            var game = (Game)SavedGamesList.SelectedItem;
            if (game != null)
            {
                // Update the game name textbox
                GameNameBox.Text = game.Name;

                // Load the game
                var tokensToMove = Engine.LoadGame(game);

                // Re-add the Ellipses to the corresponding players
                LoadPlayerTokensGUI();
                InitializeNewGameGUI(Engine.Game.GetNumberOfPlayers());

                // Move tokens to their saved positions
                if (tokensToMove != null)
                {
                    foreach (var token in tokensToMove)
                    {
                        if (token != null)
                        {
                            //Engine.UpdatePlayerScore(token);
                            MoveToken(token);
                        }
                    }
                }
            }
        }

        public void NewGame_Click(object sender, RoutedEventArgs e)
        {
            // Initialize new game by setting up the GUI
            InitializeNewGameGUI(NumberOfPlayersList.SelectedIndex + NumberOfComputersList.SelectedIndex);

            // Initialize the engine for a new game
            Engine.InitializeNewGame(NumberOfPlayersList.SelectedIndex, NumberOfComputersList.SelectedIndex);

            // Give the new game some properties
            Engine.Game.Name = GameNameBox.Text;

            // Save the newly started game
            SaveGame();

            // Start the new game
            Diebutton.IsEnabled = true;
            //PlayGame();
        }

        private void MoveToken(Token token)
        {
            // Position is null so move it back to starting position
            if (token.Position == null)
            {
                // Token was pushed back to start, so we add it to the history list
                //Engine.AddMessageToHistoryList($"P{token.PlayerNumber + 1} was pushed to start");

                Grid.SetColumn(token.Ellipse, StartingPositionsGUI[token.PlayerNumber].Start[token.TokenNumber].Column);
                Grid.SetRow(token.Ellipse, StartingPositionsGUI[token.PlayerNumber].Start[token.TokenNumber].Row);
            }
            else if (token.HasFinished)
            {
                Grid.SetColumn(token.Ellipse, FinishPositionsGUI[token.PlayerNumber].Finish[5].Column);
                Grid.SetRow(token.Ellipse, FinishPositionsGUI[token.PlayerNumber].Finish[5].Row);
            }
            else if (token.IsOnFinishLine)
            {
                int maxSteps = Engine.Game.WhoseTurnIsIt().MaximumMainBoardSteps;
                Grid.SetColumn(token.Ellipse, FinishPositionsGUI[token.PlayerNumber].Finish[token.MovedSteps - maxSteps].Column);
                Grid.SetRow(token.Ellipse, FinishPositionsGUI[token.PlayerNumber].Finish[token.MovedSteps - maxSteps].Row);
            }
            // Regular movement of the token
            else
            {
                Grid.SetColumn(token.Ellipse, BoardPositionsGUI[int.Parse(token.Position.ToString())].Column);
                Grid.SetRow(token.Ellipse, BoardPositionsGUI[int.Parse(token.Position.ToString())].Row);
            }
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
