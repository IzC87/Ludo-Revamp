using GameEngine.Classes;
using GameEngine.Initialize;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace GameEngine
{
    public class Engine
    {
        public ObservableCollection<string> HistoryList = new ObservableCollection<string>();
        public List<ObservableCollection<string>> PlayersScore = new List<ObservableCollection<string>>();

        public readonly int MaximumSteps = 57;
        public readonly int MaximumMainBoardSteps = 52;
        public int AmountOfPlayers = 0;

        public Game Game;

        public DbContext Context = new MyContext();

        public void LaunchConfiguration()
        {
            Game = StartUp.CreatePlayers();
            Context.Add(Game);
        }

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
