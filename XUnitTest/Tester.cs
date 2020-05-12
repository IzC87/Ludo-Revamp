using GameEngine.Classes;
using GameEngine.Initialize;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTest
{
    public class Tester
    {
        [Fact]
        public void GetFinishPosition_TwoPlayersHaveFinished_3()
        {
            // Arrange
            var game = SetupNewGameForTesting(1, 3);
            game.Players[0].FinishPosition = 1;
            game.Players[3].FinishPosition = 2;

            // Act
            var finishPosition = game.GetFinishPosition();

            // Assert
            Assert.Equal(3, finishPosition);
        }

        [Fact]
        public void GetMovableTokens_StartLockedDieRoll6_4()
        {
            // Arrange
            var game = SetupNewGameForTesting(1, 3);
            game.Players[0].DieRoll = 6;

            // Act
            var moveableTokens = game.Players[0].GetMovableTokens();

            // Assert
            Assert.Equal(4, moveableTokens.Count);
        }

        [Fact]
        public void GetMovableTokens_BlockingMyselfInStart_1()
        {
            // Arrange
            var game = SetupNewGameForTesting(1, 3);
            game.Players[0].Tokens[0].Position = 1;
            game.Players[0].Tokens[0].MovedSteps = 1;
            game.Players[0].DieRoll = 6;

            // Act
            var moveableTokens = game.Players[0].GetMovableTokens();

            // Assert
            Assert.Single(moveableTokens);
        }

        [Fact]
        public void HasGameFinished_TwoOutOfThreeFinished_False()
        {
            // Arrange
            var game = SetupNewGameForTesting(1, 2);
            game.Players[0].HasFinished = true;
            game.Players[1].HasFinished = true;
            game.Players[2].HasFinished = false;
            game.Players[3].HasFinished = false;

            // Act
            bool result = game.HasGameFinished();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasGameFinished_EveryoneFinishedWithThreePlayers_True()
        {
            // Arrange
            var game = SetupNewGameForTesting(1, 2);
            game.Players[0].HasFinished = true;
            game.Players[1].HasFinished = true;
            game.Players[2].HasFinished = true;
            game.Players[3].HasFinished = false;

            // Act
            bool result = game.HasGameFinished();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasGameFinished_EveryoneFinishedWithFourPlayers_True()
        {
            // Arrange
            var game = SetupNewGameForTesting(1, 3);
            game.Players[0].HasFinished = true;
            game.Players[1].HasFinished = true;
            game.Players[2].HasFinished = true;
            game.Players[3].HasFinished = true;

            // Act
            bool result = game.HasGameFinished();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetNumberOfPlayers_Three_True()
        {
            // Arrange
            var game = SetupNewGameForTesting(1, 2);

            // Act
            int numberOfPlayers = game.GetNumberOfPlayers();

            // Assert
            Assert.Equal(3, numberOfPlayers);
        }

        [Fact]
        public void GetNumberOfPlayers_Four_True()
        {
            // Arrange
            var game = SetupNewGameForTesting(1, 3);

            // Act
            int numberOfPlayers = game.GetNumberOfPlayers();

            // Assert
            Assert.Equal(4, numberOfPlayers);
        }

        [Fact]
        public void SetPlayerTurn_PlayerTurn2_Equal()
        {
            // Arrange
            var game = SetupNewGameForTesting(1, 3);
            var playerTurn = 2;

            // Act
            game.SetPlayerTurn(playerTurn);

            // Assert
            Assert.Equal(game.Players[playerTurn], game.WhoseTurnIsIt());
        }

        [Fact]
        public void NextPlayerTurn_PlayerTurn2_PlayerTurn0()
        {
            // Arrange
            var game = SetupNewGameForTesting(1, 2);

            // Act
            game.SetPlayerTurn(2);
            game.NextPlayerTurn();

            // Assert
            Assert.Equal(game.Players[0], game.WhoseTurnIsIt());
        }

        private Game SetupNewGameForTesting(int numberOfPeople, int numberOfComputers)
        {
            var game = StartUp.CreatePlayers();
            NewGame.SetupPlayers(numberOfPeople, numberOfComputers, ref game);
            return game;
        }
    }
}
