using GameEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;

namespace GameEngine
{
    public partial class Engine
    {
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
    }
}
