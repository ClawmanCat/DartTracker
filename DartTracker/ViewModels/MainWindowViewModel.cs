﻿using DartTracker.Commands;
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
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Queue<Player> players;
        private Queue<GameLeg> legs;
        private Queue<GameSet> sets;
        private Game _game;

        private string[] throw_inputs = { "", "", "" };
        public string first { get => throw_inputs[0]; set { throw_inputs[0] = value; OnPropertyChanged("first"); } }
        public string second { get => throw_inputs[1]; set { throw_inputs[1] = value; OnPropertyChanged("second"); } }
        public string third { get => throw_inputs[2]; set { throw_inputs[2] = value; OnPropertyChanged("third"); } }

        private GameLeg _gameLeg;
        private GameSet _gameSet;

        public GameLeg gameLeg => _gameLeg;
        public GameSet gameSet => _gameSet;

        public ICommand registerShotCommand
        {
            get;
            private set;
        }

        private List<Player> _participatingPlayers;
        public List<Player> participatingPlayers { get => _participatingPlayers; set { _participatingPlayers = value; OnPropertyChanged("participatingPlayers"); } }

        public MainWindowViewModel(List<Player> participatingPlayers,/* GameLeg leg, GameSet set,*/ Game game)
        {
            _participatingPlayers = participatingPlayers;
            players = new Queue<Player>(participatingPlayers);
            sets = new Queue<GameSet>(game.gameSets);
            legs = new Queue<GameLeg>(sets.Peek().legs);

            _gameLeg = NextLeg();
            _gameSet = NextSet();
            _game = game;
            _gameLeg.CurrentTurn = NextPlayer();
            registerShotCommand = new RegisterShotCommand(this);
        }

        public void checkWinner(Player player)
        {
            if (((int)player.score) == 0)
            {
                _gameLeg.Winner = player;
                player.legsWon = _gameSet.legs.Count(x => x.Winner == _gameLeg.CurrentTurn);

                // ga naar volgende leg of set..
                if (checkSetWinner(player, player.legsWon))
                {
                    NextSet();
                }

                NextLeg();
            }
        }

        public bool checkSetWinner(Player player, int legsWon)
        {
            if (legsWon > (_game.legsAmount / 2))
            {
                _gameSet.Winner = player;

                foreach(Player p in participatingPlayers)
                {
                    p.legsWon = 0;
                }

                _gameLeg.CurrentTurn.setsWon = _game.gameSets.Count(x => x.Winner == _gameLeg.CurrentTurn);

                return true;
            }
            else
            {
                return false;
            }
        }

        public void RegisterShot()
        {
            _gameLeg.history[_gameLeg.CurrentTurn.Name].Add(new Triplet(

                new Throw(SegmentParser.parse(first)),
                new Throw(SegmentParser.parse(second)),
                new Throw(SegmentParser.parse(third))
            ));

            int totalScore = SegmentParser.parse(first).Score + SegmentParser.parse(second).Score + SegmentParser.parse(third).Score;

            CheckScore(totalScore);

            checkWinner(_gameLeg.CurrentTurn);

            first = second = third = "";

            _gameLeg.CurrentTurn = NextPlayer();
        }

        public Player NextPlayer()
        {
            Player currentPlayer = players.Dequeue();
            players.Enqueue(currentPlayer);
            return currentPlayer;
        }

        public GameSet NextSet()
        {
            GameSet currentSet = sets.Dequeue();
            sets.Enqueue(currentSet);
            return currentSet;
        }

        public GameLeg NextLeg()
        {
            GameLeg currentLeg = legs.Dequeue();
            legs.Enqueue(currentLeg);
            return currentLeg;
        }

        public void CheckScore(int totalScore)
        {
            int futureScore = (int)_gameLeg.CurrentTurn.score - totalScore;
            if (futureScore < 0 || futureScore == 1)
            {
                _gameLeg.ScoreHistory[_gameLeg.CurrentTurn.Name].Add((int)_gameLeg.CurrentTurn.score);
            }
            else if (futureScore == 0)
            {
                foreach (var t in throw_inputs.Reverse())
                {
                    BoardSegment segment = SegmentParser.parse(t);

                    if ((segment is not NamedSegment) || ((NamedSegment)segment).segment != NamedSegmentType.OUTSIDE_BOARD)
                    {
                        if (segment is NamedSegment && ((NamedSegment)segment).segment == NamedSegmentType.INNER_BULLSEYE
                            || segment is NormalSegment && ((NormalSegment)segment).modifier == SegmentModifier.DOUBLE)
                        {
                            // Player won
                            _gameLeg.CurrentTurn.score -= totalScore;
                            _gameLeg.ScoreHistory[_gameLeg.CurrentTurn.Name].Add((int)_gameLeg.CurrentTurn.score);
                            return;
                        }
                        else
                        {
                            _gameLeg.ScoreHistory[_gameLeg.CurrentTurn.Name].Add((int)_gameLeg.CurrentTurn.score);
                            return;
                        }
                    }

                }
            }
            else
            {
                _gameLeg.CurrentTurn.score -= totalScore;
                _gameLeg.ScoreHistory[_gameLeg.CurrentTurn.Name].Add((int)_gameLeg.CurrentTurn.score);
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
