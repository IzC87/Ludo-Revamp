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
    }
}
