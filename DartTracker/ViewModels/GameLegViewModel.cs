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
        public string first { get => throw_inputs[0]; set { throw_inputs[0] = value; OnPropertyChanged("first"); } }
        public string second { get => throw_inputs[1]; set { throw_inputs[1] = value; OnPropertyChanged("second"); } }
        public string third { get => throw_inputs[2]; set { throw_inputs[2] = value; OnPropertyChanged("third"); } }


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
            _gameLeg.history[gameLeg.CurrentTurn.Id].Add(new Triplet(

                new Throw(SegmentParser.parse(first)),
                new Throw(SegmentParser.parse(second)),
                new Throw(SegmentParser.parse(third))
            ));

            int totalScore = SegmentParser.parse(first).Score + SegmentParser.parse(second).Score + SegmentParser.parse(third).Score;

            CheckScore(totalScore);

            first = second = third = "";

            _gameLeg.CurrentTurn = NextPlayer();
        }

        public Player NextPlayer()
        {
            Player currentPlayer = players.Dequeue();
            players.Enqueue(currentPlayer);
            return currentPlayer;
        }

        public void CheckScore(int totalScore)
        {
            int futureScore = (int)_gameLeg.CurrentTurn.score - totalScore;
            if (futureScore < 0 || futureScore == 1)
            {
                _gameLeg.ScoreHistory[_gameLeg.CurrentTurn.Id].Add((int)_gameLeg.CurrentTurn.score);
            }
            else if (futureScore == 0)
            {
                foreach (var t in throw_inputs.Reverse())
                {
                    BoardSegment segment = SegmentParser.parse(t);

                    if ( (segment is not NamedSegment) || ((NamedSegment)segment).segment != NamedSegmentType.OUTSIDE_BOARD )
                    {
                        if (segment is NamedSegment && ((NamedSegment)segment).segment == NamedSegmentType.INNER_BULLSEYE 
                            || segment is NormalSegment && ((NormalSegment)segment).modifier == SegmentModifier.DOUBLE)
                        {
                            // Player won
                            _gameLeg.CurrentTurn.score -= totalScore;
                            _gameLeg.ScoreHistory[_gameLeg.CurrentTurn.Id].Add((int)_gameLeg.CurrentTurn.score);
                            return;
                        }
                        else
                        {
                            _gameLeg.ScoreHistory[_gameLeg.CurrentTurn.Id].Add((int)_gameLeg.CurrentTurn.score);
                            return;
                        }
                    }

                }   
            }
            else
            {
                _gameLeg.CurrentTurn.score -= totalScore;
                _gameLeg.ScoreHistory[_gameLeg.CurrentTurn.Id].Add((int)_gameLeg.CurrentTurn.score);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
