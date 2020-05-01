using GameEngine.Classes;
using GameEngine.Initialize;
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
        public Game Game;

        public void LaunchConfiguration()
        {
            Game = StartUp.CreatePlayers();
        }
    }
}
