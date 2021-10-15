using DartTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DartTracker.ViewModels
{
    class GameSetViewModel : INotifyPropertyChanged
    {
        private int _legsAmount;
        private GameSet _set;
        private Player _player;
        public ICommand SaveTurnCommand;

        public GameSetViewModel(int amountOfLegs, GameSet set, Player player)
        {
            _legsAmount = amountOfLegs;
            _player = player;
            SaveTurnCommand = new CheckSetWinnerCommand(o => checkIfSetWinner(_set, _player), o => checkIfSetWinner(_set, _player));
        }

        public bool loadTestData(int amountOfLegs)
        {
            _legsAmount = amountOfLegs;

            // Test legs
            GameLeg leg1 = new GameLeg()
            {
                history = null,
                scoreHistory = null,
                Winner = _player
            };

            GameLeg leg2 = new GameLeg()
            {
                history = null,
                scoreHistory = null,
                Winner = _player
            };

            GameLeg leg3 = new GameLeg()
            {
                history = null,
                scoreHistory = null,
                Winner = _player
            };

            List<GameLeg> legs = new List<GameLeg>();

            legs.Add(leg1);
            legs.Add(leg2);
            legs.Add(leg3);

            _set = new GameSet()
            {
                legs = legs,
                Winner = null
            };

            return checkIfSetWinner(_set, _player);
        }

        public bool checkIfSetWinner(GameSet set, Player player)
        {
            int timesWon = set.legs.Count(x => x.Winner == player);

            if (timesWon > (_legsAmount / 2))
            {
                set.Winner = player;
                return true;
            }
            else
            {
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
