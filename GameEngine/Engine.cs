using GameEngine.Classes;
using GameEngine.Initialize;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            List<Token> tokens = new List<Token>();
            var player = Game.WhoseTurnIsIt();

            // Current player is a Computer
            if (player.Computer)
            {
                RollDie();
                if (CanPlayerMove(player))
                {
                    do
                    {
                        tokens = MakeMoveForComputer(player);
                    } while (!player.HasMoved);
                }
                else
                {
                    EndTurn();
                }
            }
            // Current Player is a real person
            else
            {
                // Player hasn't rolled the die yet.
                if (player.DieRoll == 0)
                {
                    // Tell the user that it's their turn
                    AddMessageToHistoryList($"It's your turn P{player.PlayerNumber + 1}!");
                }
                else if (CanPlayerMove(player))
                {
                    var token = player.GetSelectedToken();
                    if (token == null)
                    {
                        AddMessageToHistoryList("Select a token to move");
                    }
                    else if (player.AmIBlocking(token))
                    {
                        AddMessageToHistoryList("You are blocking yourself");
                    }
                    else if (player.CanTokenMove(token))
                    {
                        MoveSelectedToken();
                    }
                }
                else
                {
                    EndTurn();
                }
                
            }

            return tokens;
        }

        private List<Token> MakeMoveForComputer(Player player)
        {
            List<Token> tokens;
            if (player.SelectTokenThatCanFinish())
            {
                tokens = MoveSelectedToken();
            }
            else
            {
                player.SelectRandomTokenForComputer(randomSeed);
                tokens = MoveSelectedToken();
            }

            return tokens;
        }

        private List<Token> MoveSelectedToken()
        {
            List<Token> tokens = new List<Token>();
            var player = Game.WhoseTurnIsIt();
            var token = player.GetSelectedToken();


            int position = GetNewPosition(token, player.DieRoll);

            if (token.Position == null)
            {
                token.MovedSteps = 1;
            }
            else
            {
                token.MovedSteps += player.DieRoll;
            }

            token.Position = position;

            if (token.MovedSteps > 51)
            {
                token.IsOnFinishLine = true;
            }

            // See if the space is already occupied, if it is get that Token# to push it back to start
            var tokenToPushBack = GetOccupyingToken(position, token);
            if (tokenToPushBack != null)
            {
                tokens.Add(tokenToPushBack);
            }

            CheckIfTokenFinished(token);

            AddMessageToHistoryList($"P{player.PlayerNumber + 1} moved {player.DieRoll} steps.");
            tokens.Add(token);
            player.HasMoved = true;
            return tokens;
        }

        private Token GetOccupyingToken(int position, Token movedToken)
        {
            foreach (var player in Game.Players)
            {
                if (player.PlayerNumber != movedToken.PlayerNumber)
                {
                    foreach (var token in player.Tokens)
                    {
                        if (token.Position == position && token.IsOnFinishLine == false && movedToken != token && movedToken.IsOnFinishLine == false)
                        {
                            token.Position = null;
                            token.MovedSteps = 0;
                            return token;
                        }
                    }
                }
            }
            return null;
        }

        private void CheckIfTokenFinished(Token token)
        {
            if (token.MovedSteps == token.MaximumSteps)
            {
                AddMessageToHistoryList($"P{Game.WhoseTurnIsIt().PlayerNumber + 1} finished with token {token.TokenNumber + 1}");
                token.HasFinished = true;
            }
        }

        private int GetNewPosition(Token token, int dieRoll)
        {
            int position;

            if (token.Position == null)
            {
                position = GetStartPosition(token.PlayerNumber);
            }
            else
            {
                position = int.Parse(token.Position.ToString());
                position += dieRoll;
            }
            if (position > token.MaximumMainBoardSteps)
            {
                position -= token.MaximumMainBoardSteps;
            }

            return position;
        }

        public int GetStartPosition(int playerNumber)
        {
            return playerNumber * 13 + 1;
        }

        internal bool CanPlayerMove(Player player)
        {
            // Check if the player is locked in start and needs a 6
            if (IsPlayerLockedInStart(player))
            {
                return false;
            }
            else if (IsPlayerFinishLocked(player))
            {
                return false;
            }
            return true;
        }

        private bool IsPlayerFinishLocked(Player player)
        {
            foreach (var token in player.Tokens)
            {
                if (token.Position == null || token.MovedSteps + player.DieRoll <= token.MaximumSteps)
                {
                    return false;
                }
                else if (token.Position != null && token.MovedSteps + player.DieRoll > token.MaximumSteps)
                {
                    AddMessageToHistoryList($"P{player.PlayerNumber + 1} T{token.TokenNumber + 1} needs a {token.MaximumSteps - token.Position} to finish!");
                }
            }
            return true;
        }

        private bool IsPlayerLockedInStart(Player player)
        {
            if (player.DieRoll == 6)
            {
                return false;
            }
            foreach (var token in player.Tokens)
            {
                if (token.Position != null)
                {
                    return false;
                }
            }
            AddMessageToHistoryList($"P{player.PlayerNumber + 1} needs a 6 to start ({player.DieRoll})");
            return true;
        }

        public void EndTurn()
        {
            var player = Game.WhoseTurnIsIt();

            player.HasPlayerFinished();

            player.HasMoved = false;
            player.DieRoll = 0;

            if (CanPlayerGoAgain(player))
            {
                ++player.NumberOfRolls;
            }
            else
            {
                player.NumberOfRolls = 0;
                Game.NextPlayerTurn();
            }


            Context.SaveChanges();
        }

        private bool CanPlayerGoAgain(Player player)
        {
            if (player.DieRoll == 6 && player.HasFinished == false && player.NumberOfRolls < 3)
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
