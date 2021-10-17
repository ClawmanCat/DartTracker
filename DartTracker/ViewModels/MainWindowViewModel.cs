using DartTracker.Models;
using DartTracker.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DartTracker.ViewModels 
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private Player currentPlayer;

        private Queue<Player> players;
        private Game _currentGame;
        private GameLeg _currentLeg;
        private GameSet _currentSet;
        public GameLeg currentLeg => _currentLeg;
        public GameSet currentSet => _currentSet;


        private string[] throw_inputs = { "", "", "" };
        public string first { get => throw_inputs[0]; set { throw_inputs[0] = value; OnPropertyChanged("first"); } }
        public string second { get => throw_inputs[1]; set { throw_inputs[1] = value; OnPropertyChanged("second"); } }
        public string third { get => throw_inputs[2]; set { throw_inputs[2] = value; OnPropertyChanged("third"); } }

        public ICommand NextTurnCommand
        {
            get;
            private set;
        }

        public MainWindowViewModel(Game currentGame, List<Player> participatingPlayers, GameLeg currentLeg, GameSet currentSet)
        {
            players = new Queue<Player>(participatingPlayers);
            _currentLeg = currentLeg;
            _currentLeg.CurrentTurn = NextPlayer();
            _currentSet = currentSet;
            _currentGame = currentGame;

            // command
            NextTurnCommand = new NextTurnCommand(o => RegisterShot());
        }

        public void checkIfLegWinner()
        {
            var currentScore = _currentLeg.Scores[currentPlayer];

            if (currentScore == 0)
            {
                _currentLeg.Winner = currentPlayer;
                checkIfSetWinner();
                // ga naar volgende leg
            }
        }

        public void checkIfSetWinner()
        {
            int timesWon = _currentSet.legs.Count(x => x.Winner == currentPlayer);

            if (timesWon > (_currentGame.legsAmount / 2))
            {
                _currentSet.Winner = currentPlayer;
                // ga naar volgende set
            }
        }


        public void RegisterShot()
        {
            _currentLeg.history[currentLeg.CurrentTurn].Add(new Triplet(
                new Throw(SegmentParser.parse(first)),
                new Throw(SegmentParser.parse(second)),
                new Throw(SegmentParser.parse(third))
            ));

            first = second = third = "";

            _currentLeg.CurrentTurn = NextPlayer();
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
