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
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Queue<Player> players;
        
        private GameLeg _gameLeg;
        private GameSet _gameSet;
        private Game _game;
        private int _legsAmount;
        private int _setsAmount;

        //public GameLeg gameLeg => _gameLeg;
        public GameLeg gameLeg
        {
            get => _gameLeg;
            set
            {
                _gameLeg = value;
                OnPropertyChanged("gameLeg");
            }
        }
        public GameSet gameSet
        {
            get => _gameSet;
            set
            {
                _gameSet = value;
                OnPropertyChanged("gameSet");
            }
        }

        private string[] throw_inputs = { "", "", "" };
        public string first { get => throw_inputs[0]; set { throw_inputs[0] = value; OnPropertyChanged("first"); } }
        public string second { get => throw_inputs[1]; set { throw_inputs[1] = value; OnPropertyChanged("second"); } }
        public string third { get => throw_inputs[2]; set { throw_inputs[2] = value; OnPropertyChanged("third"); } }

        public ICommand NextTurnCommand
        {
            get;
            private set;
        }

        private List<Player> _participatingPlayers;
        public List<Player> participatingPlayers { get => _participatingPlayers; set { _participatingPlayers = value; OnPropertyChanged("participatingPlayers"); } }

        public MainWindowViewModel(List<Player> participatingPlayers, GameLeg leg, GameSet set, Game game)
        {
            _participatingPlayers = participatingPlayers;
            players = new Queue<Player>(_participatingPlayers);
            _gameLeg = leg;
            _gameSet = set;
            _game = game;
            _legsAmount = game.legsAmount;
            _setsAmount = game.setsAmount;
            _gameLeg.CurrentTurn = NextPlayer();
            NextTurnCommand = new NextTurnCommand(o => RegisterShot());


            participatingPlayers[0].legsWon = 44;
        }

        public void checkIfLegWinner(Player player)
        {
            if (((int)player.score) == 0)
            {
                gameLeg.Winner = player;
                player.legsWon = gameSet.legs.Count(x => x.Winner == _gameLeg.CurrentTurn);

                // ga naar volgende leg of set..
                if (checkIfSetWinner(player, player.legsWon))
                {
                    List<GameLeg> legs = gameSet.legs;
                    gameSet = new GameSet();
                    gameSet.parent = this._game;
                    gameSet.legs = legs;
                }

                var history = new Dictionary<string, ObservableCollection<Triplet>>();
                var scoreHistory = new Dictionary<string, ObservableCollection<int>>();

                foreach (Player p in _participatingPlayers)
                {
                    p.score = new Score();
                    history.Add(p.Name, new ObservableCollection<Triplet>());
                    scoreHistory.Add(p.Name, new ObservableCollection<int>());
                }

                gameLeg = new GameLeg()
                {
                    parent = gameSet,
                    history = history,
                    ScoreHistory = scoreHistory,
                    Winner = null,
                    CurrentTurn = null
                };

                gameSet.legs.Add(gameLeg);
            }
        }

        public bool checkIfSetWinner(Player player, int legsWon)
        {
            if (legsWon > (_legsAmount / 2))
            {
                gameSet.Winner = player;

                // reset legs to zero
                foreach (Player p in participatingPlayers)
                {
                    p.legsWon = 0;
                }
                gameLeg.CurrentTurn.setsWon = _game.gameSets.Count(x => x.Winner == _gameLeg.CurrentTurn);

                return true;
            }
            else
            {
                return false;
            }
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
            _gameLeg.ScoreHistory[_gameLeg.CurrentTurn.Name].Add((int)_gameLeg.CurrentTurn.score);

            first = second = third = "";

            checkIfLegWinner(_gameLeg.CurrentTurn);

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
