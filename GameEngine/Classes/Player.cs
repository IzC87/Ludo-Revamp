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

        internal List<Token> GetMovableTokens()
        {
            List<Token> moveableTokens = new List<Token>();
            foreach (var token in Tokens)
            {
                if (CanTokenMove(token))
                {
                    moveableTokens.Add(token);
                }
            }
            return moveableTokens;
        }

        internal bool CanTokenMove(Token mainToken)
        {
            // Token is locked in start
            if (mainToken.Position == null && DieRoll != 6)
            {
                return false;
            }
            // Token will move too far
            else if (mainToken.MovedSteps + DieRoll > mainToken.MaximumSteps)
            {
                return false;
            }
            else if (AmIBlockingMySelf(mainToken))
            {
                return false;
            }
            return true;
        }

        public bool AmIBlockingMySelf(Token mainToken)
        {
            foreach (var token in Tokens)
            {
                if (mainToken.Position == null && token.MovedSteps == 1 && DieRoll == 6)
                {
                    return true;
                }
                else if (token.Position != null && token != mainToken && mainToken.Position + DieRoll == token.Position)
                {
                    return true;
                }
            }
            return false;
        }

        internal bool IsStartLocked()
        {
            foreach (var token in Tokens)
            {
                if (token.Position == null && DieRoll != 6)
                {
                    return true;
                }
            }
            return false;
        }

        public Token GetSelectedToken()
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
    }
}