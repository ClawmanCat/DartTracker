using DartTracker.Commands;
using DartTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DartTracker.ViewModels
{
    class GameLegViewModel
    {
        private Queue<Player> players;
        private GameLeg _gameLeg;
        public GameLeg gameLeg => _gameLeg;

        public GameLegViewModel(List<Player> participatingPlayers, GameLeg leg)
        {
            registerShotCommand = new RegisterShotCommand(this);
            players = new Queue<Player>(participatingPlayers);
            _gameLeg = leg;
            _gameLeg.CurrentTurn = NextPlayer();
        }
        
        public ICommand registerShotCommand
        {
            get;
            private set;
        }
        
        
        public void RegisterShot()
        {
            _gameLeg.CurrentTurn = NextPlayer();
        }
        public Player NextPlayer()
        {
            Player current_player = players.Dequeue();
            players.Enqueue(current_player);
            return current_player;
        }
    }
}
