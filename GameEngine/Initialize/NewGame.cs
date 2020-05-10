using GameEngine.Classes;

namespace GameEngine.Initialize
{
    public class NewGame
    {
        internal static void SetupPlayers(int numberOfPlayers, int numberOfComputers, ref Game game)
        {
            for (int i = 0; i < 4; i++)
            {
                game.Players[i].HasFinished = false;
                game.Players[i].HasMoved = false;
                game.Players[i].FinishPosition = 0;
                game.Players[i].DieRoll = 0;
                game.Players[i].NumberOfRolls = 0;
                game.Players[i].MyTurn = false;

                foreach (var token in game.Players[i].Tokens)
                {
                    token.HasFinished = false;
                    token.IsOnFinishLine = false;
                    token.MovedSteps = 0;
                    token.Position = null;
                }

                if (i < numberOfPlayers)
                {
                    game.Players[i].Active = true;
                    game.Players[i].Computer = false;
                }
                else if (i < numberOfPlayers + numberOfComputers)
                {
                    game.Players[i].Active = true;
                    game.Players[i].Computer = true;
                }
                else
                {
                    game.Players[i].Active = false;
                }
            }
        }
    }
}