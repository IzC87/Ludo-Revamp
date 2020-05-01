using GameEngine.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace GameEngine.Initialize
{
    public class Load
    {
        internal static Game LoadGame(Game gameHandle)
        {
            Game game;
            using (var context = new MyContext())
            {
                //game = context.Games.Where(g => g.Name == gameName).FirstOrDefault();
                game = context.Games
                    .Where(g => (g.Name == gameHandle.Name && g.GameCreationTime == gameHandle.GameCreationTime))
                    .Include("Players.Tokens")
                    .FirstOrDefault();
            }
            return game;
        }

        internal static ObservableCollection<Game> LoadSavedGames()
        {
            List<Game> games;
            using (var context = new MyContext())
            {
                games = context.Games.ToList();
            }
            ObservableCollection<Game> returnValue = new ObservableCollection<Game>(games);
            return returnValue;
        }
    }
}
