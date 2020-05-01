using GameEngine.Classes;
using System.Collections.Generic;
using System.Windows;

namespace GameEngine.Initialize
{
    public class NewGame
    {
        internal static void SetupPlayers(int numberOfPlayers, int numberOfComputers, ref Game game)
        {
            for (int i = 0; i < 4; i++)
            {
                game.Players[i].HasFinished = false;
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