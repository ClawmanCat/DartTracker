using DartTracker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartTracker.Models
{
    public abstract class GameModel : CompareFields
    {
    }


    public class Score
    {
        private int _score = 501;
        public void SetScore(GameType value) => _score = (int) value;
        public static explicit operator int(Score s) => s._score;

        public static Score operator -(Score s, int amount)
        {
            Score result = new Score();
            result._score = s._score -= amount;
            return result;
        }
    }

    public enum SegmentModifier
    {
        SINGLE = 1,
        DOUBLE = 2,
        TRIPLE = 3
    }

    public enum GameType
    {
        SHORT = 301,
        NORMAL = 501,
        LONG = 701
    }

    public enum NamedSegmentType
    {
        OUTER_BULLSEYE = 25,
        INNER_BULLSEYE = 50,
        OUTSIDE_BOARD = 0
    }

    public class Tournament : INotifyPropertyChanged
    {
        private Player _winner;
        private DateTime? _timeAndDate;
        private List<Player> _players;

        public List<Player> Players
        {
            get { return _players; }
            set { _players = value; OnPropertyChanged("Players"); }
        }
        private List<Game> _games;

        public List<Game> Games
        {
            get { return _games; }
            set { _games = value; OnPropertyChanged("Games"); }
        }
        private int _gamesToWin;

        public int GamesToWin
        {
            get { return _gamesToWin; }
            set { _gamesToWin = value; OnPropertyChanged("GamesToWin"); }
        }
        public Player Winner
        {
            get { return _winner; }
            set { _winner = value; OnPropertyChanged("Winner"); }
        }
        public DateTime? TimeAndDate
        {
            get { return _timeAndDate; }
            set { _timeAndDate = value; OnPropertyChanged("TimeAndDate"); }
        }
        private TimeSpan? _time;
        public TimeSpan? Time
        {
            get { return _time; }
            set { _time = value; OnPropertyChanged("Time"); }
        }
        #region PropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }


    ///   [TypeConverter(typeof (PlayerClassConverter))]
    public class Player : INotifyPropertyChanged
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set{
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        private Score _score = new Score();
        public Score score
        {
            get { return _score; }
            set { _score = value; OnPropertyChanged("score"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Game
    {
        [JsonIgnore] public Tournament parent;
        public Player Winner;
        public List<GameSet> gameSets;
        public int? setsAmount;
        public int? legsAmount;
    }

    public class GameSet
    {
        [JsonIgnore] public Game parent;

        public List<GameLeg> legs;
        public Player Winner;
    }

    public class GameLeg : INotifyPropertyChanged
    {
        /*       private Dictionary<Player, ObservableCollection<Triplet>> _history; 
               [JsonProperty(Order = -2)]
               public Dictionary<Player, ObservableCollection<Triplet>> history { get { return _history; } set { _history = value; OnPropertyChanged("history");} }*/

        private Dictionary<string, ObservableCollection<Triplet>> _history;

        [JsonProperty(Order = -2)]
        public Dictionary<string, ObservableCollection<Triplet>> history
        {
            get { return _history; }
            set
            {
                _history = value;
                OnPropertyChanged("history");
            }
        }


        [JsonIgnore]
        public ObservableCollection<Triplet> HistoryPlayerOne => history.Values.ToArray()[0];

        [JsonIgnore]
        public ObservableCollection<Triplet> HistoryPlayerTwo => history.Values.ToArray()[1];

        [JsonIgnore]
        public ObservableCollection<int> ScorePlayerOne => ScoreHistory.Values.ToArray()[0];

        [JsonIgnore]
        public ObservableCollection<int> ScorePlayerTwo => ScoreHistory.Values.ToArray()[1];

        [JsonProperty(Order = -2)]
        public Dictionary<string, ObservableCollection<int>> ScoreHistory { get; set; }

        [JsonIgnore] public GameSet parent;

        public Player Winner;

        private Player _currentTurn;

        public Player CurrentTurn
        {
            get { return _currentTurn; }
            set
            {
                _currentTurn = value;
                OnPropertyChanged("CurrentTurn");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string porprtyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(porprtyName));
        }
    }

    public class Triplet
    {
        public Triplet(Throw first, Throw second, Throw third)
        {
            throws = new ObservableCollection<Throw> {first, second, third};
        }

        public ObservableCollection<Throw> throws { get; set; }
    }

    public class Throw
    {
        public Throw(BoardSegment segment)
        {
            this.segment = segment;
        }

        public BoardSegment segment { get; set; }
    }

    public class BoardSegment : GameModel
    {
        public virtual int Score { get; }
        public virtual string Display { get; }
    }

    public class NormalSegment : BoardSegment
    {
        public NormalSegment(int value, SegmentModifier modifier)
        {
            this.value = value;
            this.modifier = modifier;
        }

        public int value;
        public SegmentModifier modifier;

        public override int Score
        {
            get => value * (int) modifier;
        }

        public override string Display
        {
            get
            {
                string result = value.ToString();

                if (modifier == SegmentModifier.SINGLE) result += "S";
                if (modifier == SegmentModifier.DOUBLE) result += "D";
                if (modifier == SegmentModifier.TRIPLE) result += "T";

                return result;
            }
        }
    }

    public class NamedSegment : BoardSegment
    {
        public NamedSegment(NamedSegmentType type)
        {
            this.segment = type;
        }

        public NamedSegmentType segment;

        public override int Score
        {
            get => (int) segment;
        }


        public override string Display
        {
            get
            {
                if (segment == NamedSegmentType.OUTER_BULLSEYE) return "B";
                if (segment == NamedSegmentType.INNER_BULLSEYE) return "BS";
                if (segment == NamedSegmentType.OUTSIDE_BOARD) return "X";
                return "?";
            }
        }
    }
}