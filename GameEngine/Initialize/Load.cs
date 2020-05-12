using GameEngine.Classes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GameEngine.Initialize
{
    public class Load
    {
        internal static Game LoadGame(Game gameHandle)
        {
            using (var context = new MyContext())
            {
                var game = context.Games
                    .Where(g => g == gameHandle)
                    .Include("Players.Tokens")
                    .FirstOrDefault();
                return game;
            }
        }

        internal static ObservableCollection<Game> LoadSavedGames()
        {
            List<Game> games;
            var context = new MyContext();
            games = context.Games
                .OrderBy(g => g.Name)
                .ToList();
            ObservableCollection<Game> returnValue = new ObservableCollection<Game>(games);
            return returnValue;
        }
    }
}
