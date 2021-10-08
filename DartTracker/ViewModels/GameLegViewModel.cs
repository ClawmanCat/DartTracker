using DartTracker.Commands;
using DartTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private List<Throw> _currentThrows;
        public GameLeg gameLeg => _gameLeg;
        private int _dartCounter = 0;

        public GameLegViewModel(List<Player> participatingPlayers, GameLeg leg)
        {
            registerShotCommand = new RegisterShotCommand(this);
            players = new Queue<Player>(participatingPlayers);
            _gameLeg = leg;
            _gameLeg.CurrentTurn = NextPlayer();
            _currentThrows = new List<Throw>();
        }
        
        public ICommand registerShotCommand
        {
            get;
            private set;
        }
        
        
        public void RegisterShot()
        {
            // make this dynamic of course 
            Throw tesThrow = new Throw(new NormalSegment(10, SegmentModifier.SINGLE));
            _currentThrows.Add(tesThrow);
            // add score to history
            _dartCounter++;
            if (_dartCounter == 3)
            {
                _dartCounter = 0;
                var currentPlayer = NextPlayer();
                _gameLeg.CurrentTurn = currentPlayer;
                var currentHistory= _gameLeg.history[currentPlayer];
                currentHistory.Add(new Triplet(_currentThrows[0], _currentThrows[1], _currentThrows[2]));
                _gameLeg.history[currentPlayer] = currentHistory;
                _currentThrows = new List<Throw>();
            }
            
        }
        public Player NextPlayer()
        {
            Player currentPlayer = players.Dequeue();
            players.Enqueue(currentPlayer);
            return currentPlayer;
        }
    }
}
