using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameEngine.Classes
{
    public class Player
    {
        public int PlayerID { get; set; }
        public List<Token> Tokens { get; set; }
        public int PlayerNumber { get; set; }
        public bool Computer { get; set; }
        public bool Active { get; set; }
        public bool HasFinished { get; set; }
        public bool MyTurn { get; set; }
        public int DieRoll { get; set; }
        public int NumberOfRolls { get; set; }

        [NotMapped]
        public bool HasMoved { get; set; }
        [NotMapped]
        public readonly int MaximumSteps = 57;
        [NotMapped]
        public readonly int MaximumMainBoardSteps = 52;

        public bool HasPlayerFinished()
        {
            int numberOfFinishedTokens = 0;

            foreach (var token in Tokens)
            {
                if (token.HasFinished)
                {
                    ++numberOfFinishedTokens;
                }
            }
            if (numberOfFinishedTokens == 4)
            {
                HasFinished = true;
                return true;
            }
            return false;
        }

        private Token GetSelectedToken()
        {
            foreach (var token in Tokens)
            {
                if (token.Ellipse.StrokeThickness == 2)
                {
                    return token;
                }
            }
            return null;
        }

        public Token SelectTokenThatCanFinish()
        {
            foreach (var token in Tokens)
            {
                if (token.MovedSteps + this.DieRoll == MaximumSteps)
                {
                    SelectToken(token);
                    return token;
                }
            }
            return null;
        }

        public int GetNumberOfStartLockedTokens()
        {
            int number = 0;
            foreach (var token in Tokens)
            {
                if (token.Position == null && token.HasFinished == false)
                {
                    ++number;
                }
            }
            return number;
        }

        public int GetNumberOfMovableTokens()
        {
            int number = 0;
            if (DieRoll == 6)
            {
                number = GetNumberOfStartLockedTokens();
            }

            foreach (var token in Tokens)
            {
                if (token.HasFinished == false && token.MovedSteps + DieRoll <= MaximumSteps && token.Position != null && !AmIBlocking(token))
                {
                    ++number;
                }
            }
            return number;
        }

        public bool AmIBlocking(Token mainToken)
        {
            foreach (var token in Tokens)
            {
                if (token.Position != null && token != mainToken && token.Position + DieRoll == mainToken.Position)
                {
                    return true;
                }
            }
            return false;
        }

        public Token SelectRandomTokenForComputer(Random random)
        {
            var numberOfLockedTokens = GetNumberOfStartLockedTokens();
            if (DieRoll == 6 && numberOfLockedTokens > 0 && numberOfLockedTokens < 4)
            {
                if (random.Next(1, 3) > 1)
                {
                    return SelectLockedToken();
                }
                else
                {
                    var token = SelectUnlockedToken();
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
                return SelectUnlockedToken();
            }
        }

        public Token SelectUnlockedToken()
        {
            foreach (var token in Tokens)
            {
                if (token.Position != null && token.MovedSteps + DieRoll <= MaximumSteps)
                {
                    SelectToken(token);
                    return token;
                }
            }
            return null;
        }

        public Token SelectLockedToken()
        {
            foreach (var token in Tokens)
            {
                if (token.Position == null)
                {
                    SelectToken(token);
                    return token;
                }
            }
            return null;
        }

        public void SelectToken(Token token)
        {
            token.Ellipse.StrokeThickness = 2;
        }
    }
}