using GameEngine.Classes;
using GameEngine.Initialize;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GameEngine
{
    public partial class Engine
    {
        readonly Random randomSeed = new Random();

        public ObservableCollection<string> HistoryList = new ObservableCollection<string>();
        public List<ObservableCollection<string>> PlayersScore = new List<ObservableCollection<string>>();
        public ObservableCollection<Game> SavedGames = new ObservableCollection<Game>();

        public int AmountOfPlayers = 0;

        public Game Game;
        public DbContext Context = new MyContext();

        public List<Token> PlayGame()
        {

        }

        public void EndTurn()
        {

        }

        public int RollDie()
        {
            var roll = randomSeed.Next(1, 7);
            Game.WhoseTurnIsIt().DieRoll = roll;
            return roll;
        }

        public void DeselectSelectedTokens()
        {
            foreach (var player in Game.Players)
            {
                foreach (var token in player.Tokens)
                {
                    token.Ellipse.StrokeThickness = 0;
                }
            }
        }

        public void LaunchConfiguration()
        {
            Game = StartUp.CreatePlayers();
            Context.Add(Game);

            SavedGames = Load.LoadSavedGames();
        }

        public List<Token> LoadGame(Game game)
        {
            List<Token> tokens = new List<Token>();

            Game = Load.LoadGame(game);

            foreach (var player in Game.Players)
            {
                foreach (var token in player.Tokens)
                {
                    tokens.Add(token);
                }
            }

            return tokens;
        }

        public void InitializeNewGame(int numberOfPlayers, int numberOfComputers)
        {
            AmountOfPlayers = numberOfPlayers + numberOfComputers;

            NewGame.SetupPlayers(numberOfPlayers, numberOfComputers, ref Game);

            // Clear the Listview and write that a new game has started to the user.
            HistoryList.Clear();
            //AddMessageToHistoryList("New Game Started!");

            Game.SetPlayerTurn(0);
        }
    }
}
