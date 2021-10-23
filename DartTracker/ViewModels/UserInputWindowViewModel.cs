using DartTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DartTracker.Commands;
using System.ComponentModel.DataAnnotations;

namespace DartTracker.ViewModels
{
    public class UserInputWindowViewModel
    {
        #region Global object & command
        // This is the command when the user clicks on the 'OK' Button.
        public ICommand setGameCommand { get; set; }
        // Calling the current app to access the tournament object globally
        private Tournament _tournament;
        private Score _score;
        //public App currentApp = Application.Current as App;
        #endregion
        #region ComboBox Filler
        public IEnumerable<GameType> TournamentGameType
        {
            get
            {
                return Enum.GetValues(typeof(GameType)).Cast<GameType>();
            }
        }
        #endregion
        #region Binding ComboBox SelectedItem
        private GameType _gameType = GameType.NORMAL;
        public GameType NewGameType
        {
            get { return _gameType; }
            set { _gameType = value; }
        }
        #endregion
        #region Players object
        private List<Player> _players;
        public List<Player> Players
        {
            get { return _players; }
            set { _players = value;}
        }
        #endregion
        #region Date object
        private DateTime? _dateTime = DateTime.Now;
        public DateTime? TournamentDateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; updateModelDate(_tournament, _dateTime); }
        }
        #endregion
        #region Amount of Sets
        private int? _amountofsets;
        public int? AmountOfSets
        {
            get { return _amountofsets; }
            set { _amountofsets = value; }
        }
        #endregion
        #region Amount os Legs
        private int? _amountoflegs;
        public int? AmountOfLegs
        {
            get { return _amountoflegs; }
            set { _amountoflegs = value; }
        }
        #endregion
        #region The Time Object
        private TimeSpan _tournamentTime;
        public TimeSpan TournamentTime
        {
            get { return _tournamentTime; }
            set { _tournamentTime = value; updateModelTime(_tournament, _tournamentTime); }
        }
        #endregion
        #region Constructor
        public UserInputWindowViewModel(Tournament tournament, Score score)
        {
            _tournament = tournament;
            _score = score;
            setGameCommand = new CreateGameObjectCommand(new Action<object>((o) => setGamesets()));
            Players = _tournament.Players;
            _tournament.TimeAndDate = TournamentDateTime;

        }
        #endregion
        #region setup Game
        public void setGamesets()
        {
            
            _score.SetScore(NewGameType);
            IEnumerable<GameType> test = TournamentGameType;
            var gameSets = new List<GameSet>() { new GameSet() { legs = new List<GameLeg>() {
                new GameLeg() {
                    history=new Dictionary<string, ObservableCollection<Triplet>>(),
                    ScoreHistory=new Dictionary<string, ObservableCollection<int>>(),
                    Winner=null,
                    CurrentTurn=null
                } } } };

            foreach (Player p in _tournament.Players)
            {
                gameSets.Last().legs.Last().history.Add(p.Name, new ObservableCollection<Triplet>());
                gameSets.Last().legs.Last().ScoreHistory.Add(p.Name, new ObservableCollection<int>());
            }
            Game game = new Game()
            {
                Winner = null,
                setsAmount = AmountOfSets,
                legsAmount = AmountOfLegs,
                gameSets = gameSets
            };
            foreach (GameSet set in game.gameSets)
            {
                foreach (GameLeg leg in set.legs)
                {
                    leg.parent = set;
                }

                set.parent = game;
            }
            _tournament.Games.Add(game);
        }
        #endregion
        #region Update Functions
        /// <summary>
        /// Updates the DateTime after the user sets another date.
        /// </summary>
        /// <param name="s"></param>
        public void updateModelDate(Tournament tournament, DateTime? newDateTime)
        {
            tournament.TimeAndDate = newDateTime;
        }
        /// <summary>
        /// Update the Time after the user sets the Time
        /// </summary>
        /// <param name="newTime"></param>
        public void updateModelTime(Tournament tournament, TimeSpan? newTime)
        {
            tournament.Time = newTime;
        }
        #endregion
    }
}
