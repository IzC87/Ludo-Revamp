using GameEngine.Classes;
using GameEngine.Initialize;
using System;
using System.Windows;

namespace GameEngine
{
    public class Engine
    {
        public readonly int MaximumSteps = 57;
        public readonly int MaximumMainBoardSteps = 52;
        public Game Game;

        public void LaunchConfiguration()
        {
            Game = StartUp.CreatePlayers();
        }
    }
}
