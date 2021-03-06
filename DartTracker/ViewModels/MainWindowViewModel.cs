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
    public class MainWindowViewModel :  INotifyPropertyChanged
    {
        private Queue<Player> players;
        private Queue<GameLeg> legs;
        private Queue<GameSet> sets;
        private Game _game;
        private Score _standardScore;

        private string[] throw_inputs = { "", "", "" };
        public string first { get => throw_inputs[0]; set { throw_inputs[0] = value; OnPropertyChanged("first"); } }
        public string second { get => throw_inputs[1]; set { throw_inputs[1] = value; OnPropertyChanged("second"); } }
        public string third { get => throw_inputs[2]; set { throw_inputs[2] = value; OnPropertyChanged("third"); } }

        private GameLeg _gameLeg;
        private GameSet _gameSet;

        public GameLeg gameLeg { get => _gameLeg; set { _gameLeg = value; OnPropertyChanged("gameLeg"); } }
        public GameSet gameSet { get => _gameSet; set { _gameSet = value; OnPropertyChanged("gameSet"); } }


        private double _averageInGamePlayerOne;
        public double AverageInGamePlayerOne
        {
            get => _averageInGamePlayerOne;
            set { _averageInGamePlayerOne = value; OnPropertyChanged("AverageInGamePlayerOne"); }
        }
        private double _averageInGamePlayerTwo;
        public double AverageInGamePlayerTwo
        {
            get => _averageInGamePlayerTwo;
            set { _averageInGamePlayerTwo = value; OnPropertyChanged("AverageInGamePlayerTwo"); }
        }

        public ICommand registerShotCommand
        {
            get;
            private set;
        }
        public ICommand undoShotCommand
        {
            get;
            private set;
        }

        private List<Player> _participatingPlayers;
        public List<Player> participatingPlayers { get => _participatingPlayers; set { _participatingPlayers = value; OnPropertyChanged("participatingPlayers"); } }

        public MainWindowViewModel(List<Player> participatingPlayers, Game game, Score score)
        {

            _participatingPlayers = participatingPlayers;
            players = new Queue<Player>(participatingPlayers);
            sets = new Queue<GameSet>(game.gameSets);
            legs = new Queue<GameLeg>(sets.Peek().legs);

            _standardScore = new Score(score);

            _gameLeg = NextLeg();
            _gameSet = NextSet();
            _game = game;
            _gameLeg.CurrentTurn = NextPlayer();
            registerShotCommand = new RegisterShotCommand(this);
            undoShotCommand = new UndoShotCommand(this);
            UpdateAverages();

            if (_gameLeg.HistoryPlayerOne.Count > 0)
            {
                participatingPlayers[0].score.SetScore(_gameLeg.ScorePlayerOne.Last());
            }
            if (_gameLeg.HistoryPlayerTwo.Count > 0)
            {
                participatingPlayers[1].score.SetScore(_gameLeg.ScorePlayerTwo.Last());
            }
        }

        private void UpdateAverages()
        {
            var averages = AveragesScores.Instantance.CalculateAverageScoreInGame(_game);
            AverageInGamePlayerOne = averages.ToArray()[0].Value.Item1;
            AverageInGamePlayerTwo = averages.ToArray()[1].Value.Item1;
        }

        public bool CheckWinner(Player player)
        {
            if (((int)player.score) == 0)
            {
                gameLeg.Winner = player;
                player.TotalLegsWon++;
                player.legsWon = gameSet.legs.Count(x => x.Winner == gameLeg.CurrentTurn);

                participatingPlayers[0].score = new Score(_standardScore);
                participatingPlayers[1].score = new Score(_standardScore);

                // ga naar volgende leg of set..
                if (CheckSetWinner(player, player.legsWon))
                {
                    if(CheckGameWinner(player,player.setsWon)) return true;
                    gameSet = NextSet();
                    legs = new Queue<GameLeg>(gameSet.legs);
                }

                gameLeg = NextLeg();
            }

            return false;
        }

        public bool CheckGameWinner(Player player, int playerSetsWon)
        {
            if (playerSetsWon > (_game.setsAmount / 2))
            {
                _game.Winner = player;
                return true;
            }
            return false;
        }

        public bool CheckSetWinner(Player player, int legsWon)
        {
            if (legsWon > (_game.legsAmount / 2))
            {
                gameSet.Winner = player;
                player.TotalSetsWon++;

                foreach(Player p in _participatingPlayers)
                {
                    p.legsWon = 0;
                }

                gameLeg.CurrentTurn.setsWon = _game.gameSets.Count(x => x.Winner == gameLeg.CurrentTurn);

                return true;
            }
            return false;
        }

        public void RegisterShot()
        {
            _gameLeg.history[_gameLeg.CurrentTurn.Id].Add(new Triplet(

                new Throw(SegmentParser.parse(first)),
                new Throw(SegmentParser.parse(second)),
                new Throw(SegmentParser.parse(third))
            ));

            int totalScore = SegmentParser.parse(first).Score + SegmentParser.parse(second).Score + SegmentParser.parse(third).Score;

            CheckScore(totalScore);
            UpdateAverages();
            if(CheckWinner(_gameLeg.CurrentTurn)) return;

            first = second = third = "";

            _gameLeg.CurrentTurn = NextPlayer();
        }

        public bool CheckHistorySize()
        {
            bool returnValue =  _gameLeg.history[participatingPlayers[0].Id].Count > 0;
            return returnValue;
        }
        public void UndoShot()
        {
            _gameLeg.CurrentTurn = NextPlayer();
            int lastItem = _gameLeg.history[_gameLeg.CurrentTurn.Id].Count - 1;
            //int lastScore = _gameLeg.ScoreHistory[gameLeg.CurrentTurn.Id][lastItem];
            try
            { 
                Triplet lastTriplet = _gameLeg.history[_gameLeg.CurrentTurn.Id][lastItem];
                int lastScore = 0;
                foreach (Throw t in lastTriplet.throws)
                {
                    lastScore += t.segment.Score;
                }

                _gameLeg.CurrentTurn.score -= -1 * lastScore;



                _gameLeg.history[_gameLeg.CurrentTurn.Id].RemoveAt(lastItem);
                _gameLeg.ScoreHistory[_gameLeg.CurrentTurn.Id].RemoveAt(lastItem);
                UpdateAverages();
            }
            catch (ArgumentOutOfRangeException e)
            {
                _gameLeg.CurrentTurn = NextPlayer();
                return;
            }

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
                _gameLeg.ScoreHistory[_gameLeg.CurrentTurn.Id].Add((int)_gameLeg.CurrentTurn.score);
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
                            _gameLeg.ScoreHistory[gameLeg.CurrentTurn.Id].Add((int)_gameLeg.CurrentTurn.score);
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

        public void CheckIfNineDarter(Player player)
        {
            int dartsThrown = _gameLeg.history[player.Id].Count();

            if(dartsThrown != 0 && dartsThrown == 3)
            {
                System.Windows.MessageBox.Show("9 dart leg gegooid door " + player.Name);
                player.nineDarts++;
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
