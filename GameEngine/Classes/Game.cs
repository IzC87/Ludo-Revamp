using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace GameEngine.Classes
{
    public class Game
    {
        public int GameID { get; set; }
        public List<Player> Players { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public DateTime GameCreationTime { get; set; }


        public bool HasGameFinished()
        {
            int finishedPlayers = 0;
            foreach (var player in Players)
            {
                if (player.HasFinished)
                {
                    ++finishedPlayers;
                }
            }
            if (finishedPlayers >= GetNumberOfPlayers() - 1)
            {
                return true;
            }
            return false;
        }

        public void DeselectTokens()
        {
            foreach (var player in Players)
            {
                foreach (var token in player.Tokens)
                {
                    token.Ellipse.StrokeThickness = 0;
                }
            }
        }

        public Player WhoseTurnIsIt()
        {
            foreach (var player in Players)
            {
                if (player.MyTurn)
                {
                    return player;
                }
            }
            return null;
        }

        public void SetPlayerTurn(int playerNumber)
        {
            UnsetPlayersTurn();
            Players[playerNumber].MyTurn = true;
        }

        public void UnsetPlayersTurn()
        {
            foreach (var player in Players)
            {
                player.MyTurn = false;
            }
        }

        public void NextPlayerTurn()
        {
            int currentPlayer = 0;
            foreach (var player in Players)
            {
                if (player.MyTurn)
                {
                    currentPlayer = player.PlayerNumber;
                }
            }

            ++currentPlayer;
            if (currentPlayer > GetNumberOfPlayers() - 1)
            {
                currentPlayer = 0;
            }
            if (Players[currentPlayer].HasFinished)
            {
                SetPlayerTurn(currentPlayer);
                NextPlayerTurn();
            }
            else
            {
                SetPlayerTurn(currentPlayer);
            }
        }

        public int GetNumberOfPlayers()
        {
            int numberOfPlayers = 0;
            foreach (var player in Players)
            {
                if (player.Active)
                {
                    ++numberOfPlayers;
                }
            }
            return numberOfPlayers;
        }

        internal Token GetOccupyingToken(Token movedToken)
        {
            foreach (var player in Players)
            {
                foreach (var token in player.Tokens)
                {
                    if (token.Position == movedToken.Position && token.IsOnFinishLine == false && movedToken != token && movedToken.IsOnFinishLine == false)
                    {
                        token.Position = null;
                        token.MovedSteps = 0;
                        return token;
                    }
                }
            }
            return null;
        }

        internal int GetFinishPosition()
        {
            int position = 0;
            foreach (var player in Players)
            {
                if (player.FinishPosition >= position)
                {
                    position = player.FinishPosition + 1;
                }
            }
            return position;
        }
    }
}
