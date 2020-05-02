using GameEngine.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GameEngine.Initialize
{
    public class StartUp
    {
        internal static Game CreatePlayers()
        {
            var game = new Game
            {
                GameCreationTime = DateTime.Now
            };

            // Initialize the list of players
            game.Players = new List<Player>();

            // Populate the list of players
            for (int i = 0; i < 4; i++)
            {
                game.Players.Add(new Player
                {
                    PlayerNumber = i,
                    Active = true,
                    Computer = true,
                    HasFinished = false,
                    HasMoved = false,
                    MyTurn = false
                });

                // Initialize the list of tokens for the last added player
                game.Players[game.Players.Count - 1].Tokens = new List<Token>();

                // Populate the list of Tokens
                for (int x = 0; x < 4; x++)
                {
                    game.Players[game.Players.Count - 1].Tokens.Add(new Token
                    {
                        Position = null,
                        TokenNumber = x,
                        MovedSteps = 0,
                        HasFinished = false,
                        IsOnFinishLine = false,
                        PlayerNumber = i
                    });
                }
            }

            return game;
        }

        internal static List<ObservableCollection<string>> CreatePlayersScoreList()
        {
            var playersScore = new List<ObservableCollection<string>>();
            for (int i = 0; i < 4; i++)
            {
                playersScore.Add(new ObservableCollection<string>());
                playersScore[i].Add($"Player {i + 1} score:");
                for (int j = 0; j < 4; j++)
                {
                    playersScore[i].Add($"T{j + 1}: is in start");
                }
            }
            return playersScore;
        }
    }
}
