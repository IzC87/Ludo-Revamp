using Ludo_Revamp.Classes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Ludo_Revamp
{
    public partial class MainWindow : Window
    {
        public void InitializeNewGameGUI(int numberOfPlayers)
        {
            // Hide Tokens if the total player count is below 4(max)
            if (numberOfPlayers < 4) HideStartingTokens();

            // Show and place tokens on their start position
            PlaceStartingTokens(numberOfPlayers);
        }

        private void HideStartingTokens()
        {
            foreach (var player in Engine.Game.Players)
            {
                foreach (var token in player.Tokens)
                {
                    token.Ellipse.Visibility = Visibility.Hidden;
                }
            }
        }

        public void PlaceStartingTokens(int numberOfPlayers)
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Grid.SetColumn(Engine.Game.Players[i].Tokens[j].Ellipse, StartingPositionsGUI[i].Start[j].Column);
                    Grid.SetRow(Engine.Game.Players[i].Tokens[j].Ellipse, StartingPositionsGUI[i].Start[j].Row);
                    Engine.Game.Players[i].Tokens[j].Ellipse.Visibility = Visibility.Visible;
                }
            }
        }

        private void LaunchInitialization()
        {
            // InitializeEngine
            Engine.LaunchConfiguration();

            // InitializeGUI
            AddPlayerTokensGUI();

            // Load positions on board from JSON
            BoardPositionsGUI = JsonConvert.DeserializeObject<List<BoardPositionsGUI>>(File.ReadAllText(@".\JSON\BoardPositions.json"));
            StartingPositionsGUI = JsonConvert.DeserializeObject<List<StartingPositionsGUI>>(File.ReadAllText(@".\JSON\StartingPositions.json"));
            FinishPositionsGUI = JsonConvert.DeserializeObject<List<FinishPositionsGUI>>(File.ReadAllText(@".\JSON\FinishPositions.json"));

            // Add ItemsSources to display info about the game to the user
            SavedGamesList.ItemsSource = Engine.SavedGames;
            HistoryListview.ItemsSource = Engine.HistoryList;
            Player1Scores.ItemsSource = Engine.PlayersScore[0];
            Player2Scores.ItemsSource = Engine.PlayersScore[1];
            Player3Scores.ItemsSource = Engine.PlayersScore[2];
            Player4Scores.ItemsSource = Engine.PlayersScore[3];
        }

        private void LoadPlayerTokensGUI()
        {
            var colors = new List<SolidColorBrush>() { Brushes.Green, Brushes.Purple, Brushes.Red, Brushes.Blue };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Engine.Game.Players[i].Tokens[j].Ellipse = Ellipses[i * 4 + j];
                    Engine.Game.Players[i].Tokens[j].Ellipse.Fill = colors[Engine.Game.Players[i].PlayerNumber];
                }
            }
        }

        private void AddPlayerTokensGUI()
        {
            var colors = new List<SolidColorBrush>() { Brushes.Green, Brushes.Purple, Brushes.Red, Brushes.Blue };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Ellipse playerToken = new Ellipse
                    {
                        Visibility = Visibility.Hidden,
                        Margin = new Thickness(4),
                        Fill = colors[i],
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = 0
                    };
                    //Engine.Game.Players[i].Tokens[j].Ellipse = playerToken;
                    Ellipses.Add(playerToken);
                    Board.Children.Add(playerToken);
                    playerToken.MouseDown += PlayerToken_MouseDown;
                }
            }
        }
    }
}
