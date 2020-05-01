using GameEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;

namespace GameEngine
{
    public partial class Engine
    {
        public void SelectToken(Token token)
        {
            DeselectTokens();
            token.Ellipse.StrokeThickness = 2;
        }

        public void DeselectTokens()
        {
            foreach (var player in Game.Players)
            {
                foreach (var token in player.Tokens)
                {
                    token.Ellipse.StrokeThickness = 0;
                }
            }
        }
        public Token SelectRandomTokenForComputer(int dieRoll)
        {
            var numberOfLockedTokens = GetNumberOfStartLockedTokens();
            if (dieRoll == 6 && numberOfLockedTokens > 0 && numberOfLockedTokens < 4)
            {
                if (randomSeed.Next(1, 3) > 1)
                {
                    return SelectLockedToken();
                }
                else
                {
                    var token = SelectUnlockedToken(dieRoll);
                    if (token == null)
                    {
                        return SelectLockedToken();
                    }
                    return token;
                }
            }
            else if (numberOfLockedTokens == 4)
            {
                return SelectLockedToken();
            }
            else
            {
                return SelectUnlockedToken(dieRoll);
            }
        }

        public int GetNumberOfStartLockedTokens()
        {
            int number = 0;
            foreach (var token in Game.Players[WhoseTurnIsIt()].Tokens)
            {
                if (token.Position == null && token.HasFinished == false)
                {
                    ++number;
                }
            }
            return number;
        }

        private int GetNumberOfMovableTokens(int dieRoll)
        {
            int number = 0;
            if (dieRoll == 6)
            {
                number = GetNumberOfStartLockedTokens();

            }
            foreach (var token in Game.Players[WhoseTurnIsIt()].Tokens)
            {
                if (token.HasFinished == false && token.MovedSteps + dieRoll <= MaximumSteps && token.Position != null)
                {
                    ++number;
                }
            }
            return number;
        }

        private Token SelectUnlockedToken(int dieRoll = 0)
        {
            foreach (var token in Game.Players[WhoseTurnIsIt()].Tokens)
            {
                if (token.Position != null && token.HasFinished == false && token.MovedSteps + dieRoll <= MaximumSteps)
                {
                    SelectToken(token);
                    return token;
                }
            }
            return null;
        }

        private Token SelectLockedToken()
        {
            foreach (var token in Game.Players[WhoseTurnIsIt()].Tokens)
            {
                if (token.Position == null && token.HasFinished == false)
                {
                    SelectToken(token);
                    return token;
                }
            }
            return null;
        }
    }
}
