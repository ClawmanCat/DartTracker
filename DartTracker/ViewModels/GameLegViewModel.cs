using DartTracker.Commands;
using DartTracker.Models;
using DartTracker.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DartTracker.ViewModels
{
    class GameLegViewModel : INotifyPropertyChanged
    {
        private Queue<Player> players;
        private GameLeg _gameLeg;

        public GameLeg gameLeg => _gameLeg;

        private string[] throw_inputs = { "", "", "" };
        public string first  { get => throw_inputs[0]; set { throw_inputs[0] = value; OnPropertyChanged("first"); } }
        public string second { get => throw_inputs[1]; set { throw_inputs[1] = value; OnPropertyChanged("second"); } }
        public string third  { get => throw_inputs[2]; set { throw_inputs[2] = value; OnPropertyChanged("third"); } }


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
            _gameLeg.history[gameLeg.CurrentTurn.Name].Add(new Triplet(
                new Throw(SegmentParser.parse(first)),
                new Throw(SegmentParser.parse(second)),
                new Throw(SegmentParser.parse(third))
            ));

            int totalScore = SegmentParser.parse(first).Score + SegmentParser.parse(second).Score + SegmentParser.parse(third).Score;
            _gameLeg.CurrentTurn.score -= totalScore;
            _gameLeg.scoreHistory[_gameLeg.CurrentTurn].Add((int)_gameLeg.CurrentTurn.score);

            first = second = third = "";

            _gameLeg.CurrentTurn = NextPlayer();
        }
        
        public Player NextPlayer()
        {
            Player currentPlayer = players.Dequeue();
            players.Enqueue(currentPlayer);
            return currentPlayer;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
