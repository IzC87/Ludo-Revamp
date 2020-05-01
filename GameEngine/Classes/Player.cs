using System.Collections.Generic;

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
    }
}