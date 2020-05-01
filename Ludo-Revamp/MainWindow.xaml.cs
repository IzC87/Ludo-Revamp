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
using Ludo.Classes;
using System.IO;

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

            //HistoryListview.ItemsSource = HistoryList;
            HistoryListview.ItemsSource = Engine.HistoryList;
            //Player1Scores.ItemsSource = Engine.PlayersScore[0];
            //Player2Scores.ItemsSource = Engine.PlayersScore[1];
            //Player3Scores.ItemsSource = Engine.PlayersScore[2];
            //Player4Scores.ItemsSource = Engine.PlayersScore[3];
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
                    Engine.Game.Players[i].Tokens[j].Ellipse = playerToken;
                    Board.Children.Add(playerToken);
                    playerToken.MouseDown += PlayerToken_MouseDown;
                }
            }
        }

        private void HandleEscapeKeyPress(object sender, KeyEventArgs e)
        {
            // Close the window with the Escape key
            if (e.Key == Key.Escape) Close();
        }
    }
}
