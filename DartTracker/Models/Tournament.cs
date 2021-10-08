using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartTracker.Models
{
    public class Score
    {
        private int _score = 501;
        
        public static explicit operator int(Score s) => s._score;

        public static Score operator -(Score s, int amount) {
            Score result = new Score();
            result._score = s._score = amount;
            return result;
        }
    }

    public enum SegmentModifier
    {
        SINGLE = 1,
        DOUBLE = 2,
        TRIPLE = 3
    }

    public enum NamedSegmentType
    {
        OUTER_BULLSEYE = 25,
        INNER_BULLSEYE = 50,
        OUTSIDE_BOARD = 0
    }

    public class Tournament
    {
        public List<Player> Players;
        public List<Game> Games;
        public int GamesToWin;
        public Player Winner;
        public DateTime TimeAndDate;
    }

    public class Player : INotifyPropertyChanged
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged("Name"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string porprtyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(porprtyName));
        }
    }

    public class Game
    {
        public Player Winner;
        public List<GameSet> gameSets;
        public int setsAmount;
        public int legsAmount;
    }

    public class GameSet 
    {
        public List<GameLeg> legs;
        public Player Winner;
    }

    public class GameLeg : INotifyPropertyChanged
    {
        public Player _currentTurn;
        public List<Dictionary<Player, List<Triplet>>> history;
        public Dictionary<Player, int> Scores;
        public Player Winner;
        public Player CurrentTurn
        {
            get { return _currentTurn; }
            set { _currentTurn = value; OnPropertyChanged("CurrentTurn"); }
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
        public List<Throw> Throws;
    }
    
    public class Throw
    {
        public BoardSegment boardSegment;
    }

    public class BoardSegment
    {
        public virtual int Score { get; }
    }
    
    public class NormalSegment : BoardSegment
    {
        public int value;
        public SegmentModifier modifier;

        public override int Score { get => value * (int) modifier; }
    }
    public class NamedSegment : BoardSegment
    {
        public NamedSegmentType segment;

        public override int Score { get => (int) segment; }
    }
}
