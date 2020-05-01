using System.Collections.Generic;

namespace Ludo.Classes
{
    public class BoardPositionsGUI
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public int? PlayerStart { get; set; }
        public int? PlayerFinish { get; set; }
    }

    public class Positions
    {
        public int Column { get; set; }
        public int Row { get; set; }
    }
    public class StartingPositionsGUI
    {
        public List<Positions> Start { get; set; }
    }

    public class FinishPositionsGUI
    {
        public List<Positions> Finish { get; set; }
    }
}
