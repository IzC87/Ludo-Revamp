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

        private void SetPlayerTurn(int playerNumber)
        {
            UnsetPlayersTurn();
            Game.Players[playerNumber].MyTurn = true;
        }

        private void UnsetPlayersTurn()
        {
            foreach (var player in Game.Players)
            {
                player.MyTurn = false;
            }
        }

        public bool IsPlayerComputer()
        {
            if (Game.Players[WhoseTurnIsIt()].Computer == false)
            {
                return false;
            }
            return true;
        }

        public int RollDie()
        {
            var roll = randomSeed.Next(1, 7);
            Game.Players[WhoseTurnIsIt()].DieRoll = roll;
            return roll;
        }

        public int WhoseTurnIsIt()
        {
            foreach (var player in Game.Players)
            {
                if (player.MyTurn)
                {
                    return player.PlayerNumber;
                }
            }
            return 0;
        }

        public void LaunchConfiguration()
        {
            Game = StartUp.CreatePlayers();
            Context.Add(Game);

            SavedGames = Load.LoadSavedGames();
        }

        public void LoadGame(Game game)
        {
            //NewGame.SetupPlayers(0, 4, ref Game);
            Game = Load.LoadGame(game);
        }

        public void InitializeNewGame(int numberOfPlayers, int numberOfComputers)
        {
            AmountOfPlayers = numberOfPlayers + numberOfComputers;

            NewGame.SetupPlayers(numberOfPlayers, numberOfComputers, ref Game);

            // Clear the Listview and write that a new game has started to the user.
            HistoryList.Clear();
            //AddMessageToHistoryList("New Game Started!");

            SetPlayerTurn(0);
        }
    }
}
