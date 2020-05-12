using GameEngine.Classes;
using GameEngine.Initialize;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace GameEngine
{
    public partial class Engine
    {
        readonly Random randomSeed = new Random();

        public ObservableCollection<string> HistoryList = new ObservableCollection<string>();
        public List<ObservableCollection<string>> PlayersScore = new List<ObservableCollection<string>>();
        public ObservableCollection<Game> SavedGames = new ObservableCollection<Game>();

        public Game Game;
        public DbContext Context = new MyContext();

        public List<Token> PlayGame()
        {
            List<Token> tokens = new List<Token>();
            var player = Game.WhoseTurnIsIt();

            // Player is Computer
            if (player.Computer)
            {
                // Computer needs to roll the die
                if (player.DieRoll == 0)
                {
                    player.DieRoll = RollDie();
                }

                // Make move for computer
                var moveableTokens = player.GetMovableTokens();
                if (moveableTokens.Count == 1)
                {
                    tokens = MoveToken(moveableTokens[0]);
                }

                // computer can move more than one token so we select a random movable token to move
                else if (moveableTokens.Count > 1)
                {
                    var token = moveableTokens[randomSeed.Next(0, moveableTokens.Count)];
                    tokens = MoveToken(token);
                }
                else if (player.IsStartLocked())
                {
                    AddMessageToHistoryList($"P{player.PlayerNumber + 1} needs a 6 to start! ({player.DieRoll})");
                    player.HasMoved = true;
                    player.NumberOfRolls = 3;
                }
                else
                {
                    player.HasMoved = true;
                }
            }
            // Player is a real person
            else
            {
                // Player needs to roll the die
                if (player.DieRoll == 0)
                {
                    // Tell the user that it's their turn
                    AddMessageToHistoryList($"It's your turn P{player.PlayerNumber + 1}!");
                }
                // Make move for player
                else
                {
                    var moveableTokens = player.GetMovableTokens();
                    if (moveableTokens.Count == 1)
                    {
                        tokens = MoveToken(moveableTokens[0]);
                    }
                    // Player can move more than one token
                    else if (moveableTokens.Count > 1)
                    {
                        var token = player.GetSelectedToken();
                        // No Token selected
                        if (token == null)
                        {
                            AddMessageToHistoryList("Select a token to move");
                        }
                        // If token can be moved then we move it
                        else if(player.CanTokenMove(token))
                        {
                            tokens = MoveToken(token);
                        }
                        else
                        {
                            AddMessageToHistoryList("Unable to move that token");
                        }
                    }
                    else if (player.IsStartLocked())
                    {
                        AddMessageToHistoryList($"P{player.PlayerNumber + 1} needs a 6 to start! ({player.DieRoll})");
                        player.HasMoved = true;
                        player.NumberOfRolls = 3;
                    }
                }
            }

            return tokens;
        }

        private List<Token> MoveToken(Token token)
        {
            List<Token> tokens = new List<Token>();
            var player = Game.WhoseTurnIsIt();
            var newPosition = GetNewPosition(token, player.DieRoll);

            // Token har just left the starting zone if Position == null
            if (token.Position == null)
            {
                token.MovedSteps = 1;
                AddMessageToHistoryList($"P{player.PlayerNumber + 1} moved 1 steps. ({player.DieRoll})");
            }
            else
            {
                token.MovedSteps += player.DieRoll;
                AddMessageToHistoryList($"P{player.PlayerNumber + 1} moved {player.DieRoll} steps.");
            }

            // Update the token pos and that the player has moved
            token.Position = newPosition;
            player.HasMoved = true;

            // Add the token to list to move it in the GUI later
            tokens.Add(token);

            // Do some checks to see where the token is
            token.CheckIfTokenHasFinished();
            token.CheckIfTokenIsOnFinishLine();

            // Check if the token will bump another player back to start
            var tokenToPushBack = Game.GetOccupyingToken(token);
            if (tokenToPushBack != null)
            {
                tokens.Add(tokenToPushBack);
            }

            return tokens;
        }

        private int GetNewPosition(Token token, int dieRoll)
        {
            int newPosition;

            // Token will start
            if (token.Position == null)
            {
                newPosition = GetStartPosition(token.PlayerNumber);
            }
            else
            {
                newPosition = int.Parse(token.Position.ToString()) + dieRoll;
            }

            // Check if we need to restart postion
            if (newPosition >= token.MaximumMainBoardSteps && token.MovedSteps <= token.MaximumMainBoardSteps)
            {
                newPosition -= token.MaximumMainBoardSteps;
            }

            return newPosition;
        }

        private int GetStartPosition(int playerNumber)
        {
            return playerNumber * 13 + 1;
        }

        public void EndTurn()
        {
            var player = Game.WhoseTurnIsIt();

            if (player.HasFinished == false && player.HasPlayerFinished())
            {
                player.FinishPosition = Game.GetFinishPosition();
                player.HasFinished = true;

                // Congratulate Player
                MessageBox.Show($"Congratulations player {player.PlayerNumber + 1}! You finished {player.FinishPosition}");
            }

            if (CanPlayerGoAgain(player))
            {
                AddMessageToHistoryList($"P{player.PlayerNumber + 1} can go again!");
                ++player.NumberOfRolls;
            }
            else
            {
                player.NumberOfRolls = 0;
                Game.NextPlayerTurn();
            }

            player.HasMoved = false;
            player.DieRoll = 0;

            Context.SaveChanges();
        }

        private bool CanPlayerGoAgain(Player player)
        {
            if (player.DieRoll == 6 && player.HasFinished == false && player.NumberOfRolls < 2)
            {
                return true;
            }
            return false;
        }

        public void AddMessageToHistoryList(string message)
        {
            HistoryList.Insert(0, message);
        }

        public void UpdatePlayerScore(Token token)
        {
            // Remove the list item in order to replace it
            PlayersScore[token.PlayerNumber].RemoveAt(token.TokenNumber + 1);

            // If the token has finished write that it has finished
            if (Game.Players[token.PlayerNumber].Tokens[token.TokenNumber].HasFinished)
            {
                PlayersScore[token.PlayerNumber].Insert(token.TokenNumber + 1, $"T{token.TokenNumber + 1}: has finished!");
            }

            // If the token is locked in start, write that
            else if (Game.Players[token.PlayerNumber].Tokens[token.TokenNumber].Position == null)
            {
                PlayersScore[token.PlayerNumber].Insert(token.TokenNumber + 1, $"T{token.TokenNumber + 1}: is in start");
            }
            else
            {
                PlayersScore[token.PlayerNumber].Insert(token.TokenNumber + 1, $"T{token.TokenNumber + 1}: " + Game.Players[token.PlayerNumber].Tokens[token.TokenNumber].MovedSteps);
            }
        }

        public int RollDie()
        {
            var roll = randomSeed.Next(1, 7);
            return roll;
        }

        public void LaunchConfiguration()
        {
            PlayersScore = StartUp.CreatePlayersScoreList();

            SavedGames = Load.LoadSavedGames();
        }

        public List<Token> LoadGame(Game game)
        {
            List<Token> tokens = new List<Token>();
            Game = Load.LoadGame(game);

            Context = new MyContext();
            Context.Attach(Game);

            foreach (var player in Game.Players)
            {
                foreach (var token in player.Tokens)
                {
                    tokens.Add(token);
                }
            }

            return tokens;
        }

        public void InitializeNewGame(int numberOfPlayers, int numberOfComputers, string gameName)
        {
            Game = StartUp.CreatePlayers();

            NewGame.SetupPlayers(numberOfPlayers, numberOfComputers, ref Game);

            // Clear the Listview and write that a new game has started to the user.
            HistoryList.Clear();
            AddMessageToHistoryList("New Game Started!");

            Game.Name = gameName;
            Game.SetPlayerTurn(0);

            Context.Add(Game);
            Context.SaveChanges();

            SavedGames = Load.LoadSavedGames();
        }
    }
}
