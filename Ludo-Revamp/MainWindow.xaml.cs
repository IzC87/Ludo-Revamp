using GameEngine;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using GameEngine.Classes;
using Ludo_Revamp.Classes;

namespace Ludo_Revamp
{
    public partial class MainWindow : Window
    {
        public Engine Engine = new Engine();

        public List<BoardPositionsGUI> BoardPositionsGUI = new List<BoardPositionsGUI>();
        public List<StartingPositionsGUI> StartingPositionsGUI = new List<StartingPositionsGUI>();
        public List<FinishPositionsGUI> FinishPositionsGUI = new List<FinishPositionsGUI>();

        public List<Ellipse> Ellipses = new List<Ellipse>();
        public List<TextBlock> TextBlocks = new List<TextBlock>();

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
            var player = Engine.Game.WhoseTurnIsIt();

            // If it's a real persons turn, we enable the die button.
            if (!player.Computer && player.DieRoll == 0)
            {
                Diebutton.IsEnabled = true;
            }

            // Play the game, this method returns a list of tokens to move on the board.
            var tokensToMove = Engine.PlayGame();
            if (tokensToMove != null)
            {
                foreach (var token in tokensToMove)
                {
                    if (token != null)
                    {
                        Engine.UpdatePlayerScore(token);
                        MoveToken(token);
                    }
                }
            }

            if (player.HasFinished)
            {
                TextBlocks[player.PlayerNumber].Text = "Finish position: " + player.FinishPosition;
            }

            if (player.HasMoved && Engine.Game.HasGameFinished() == false)
            {
                Engine.EndTurn();
                PlayGame();
            }
        }

        public void RollDie_Click(object sender, RoutedEventArgs e)
        {
            int dieRoll = Engine.RollDie();
            Diebutton.Content = dieRoll;
            Diebutton.IsEnabled = false;
            Engine.Game.DeselectTokens();
            Engine.Game.WhoseTurnIsIt().DieRoll = dieRoll;
            PlayGame();
        }

        private void PlayerToken_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = (Ellipse)sender;
            Engine.Game.DeselectTokens();
            ellipse.StrokeThickness = 2;
            if (!Diebutton.IsEnabled)
            {
                PlayGame();
            }
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
                            MoveToken(token);
                        }
                    }
                }

                // Tell the user that the game has finished loading
                Engine.HistoryList.Clear();
                Engine.AddMessageToHistoryList("Game loaded");
            }
            else
            {
                Engine.AddMessageToHistoryList("Unable to load game.");
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
            PlayGame();
        }

        private void MoveToken(Token token)
        {
            // Position is null so move it back to starting position
            if (token.Position == null)
            {
                // Token was pushed back to start, so we add it to the history list
                Engine.AddMessageToHistoryList($"P{token.PlayerNumber + 1} was pushed to start");

                Grid.SetColumn(token.Ellipse, StartingPositionsGUI[token.PlayerNumber].Start[token.TokenNumber].Column);
                Grid.SetRow(token.Ellipse, StartingPositionsGUI[token.PlayerNumber].Start[token.TokenNumber].Row);
            }
            else if (token.HasFinished)
            {
                Engine.AddMessageToHistoryList($"P{token.PlayerNumber + 1} finished with T{token.TokenNumber + 1}");

                Grid.SetColumn(token.Ellipse, FinishPositionsGUI[token.PlayerNumber].Finish[5].Column);
                Grid.SetRow(token.Ellipse, FinishPositionsGUI[token.PlayerNumber].Finish[5].Row);
            }
            else if (token.IsOnFinishLine)
            {
                int maxSteps = Engine.Game.Players[0].Tokens[0].MaximumMainBoardSteps;
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
