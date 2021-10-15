﻿using DartTracker.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartTracker.Models
{
    public abstract class GameModel : CompareFields { }


    public class Score : GameModel
    {
        private int _score = 501;
        public void SetScore(GameType value) => _score = (int)value;
        public static explicit operator int(Score s) => s._score;

        public static Score operator -(Score s, int amount) {
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

    public class Tournament : GameModel
    {
        public List<Player> Players;
        public List<Game> Games;
        public int GamesToWin;
        public Player Winner;
        public DateTime TimeAndDate;
    }

    public class Player : GameModel, INotifyPropertyChanged
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged("Name"); }
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
    }

    public class Game : GameModel
    {
        public Tournament parent;
        public Player Winner;
        public List<GameSet> gameSets;
        public int setsAmount;
        public int legsAmount;
    }

    public class GameSet : GameModel
    {
        public Game parent;

        public List<GameLeg> legs;
        public Player Winner;
    }

    public class GameLeg : GameModel, INotifyPropertyChanged
    {
        public ObservableCollection<Triplet> p1History
        {
            get
            {
                return history.Where(kv => kv.Key == parent.parent.parent.Players[0]).Select(kv => kv.Value).First();
            }
        }

        public ObservableCollection<Triplet> p2History
        {
            get
            {
                return history.Where(kv => kv.Key == parent.parent.parent.Players[1]).Select(kv => kv.Value).First();
            }
        }

        public Dictionary<Player, ObservableCollection<Triplet>> history { get; set; }


        public GameSet parent;

        public Dictionary<Player, int> Scores;
        public Player Winner;

        private Player _currentTurn;
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

    public class Triplet : GameModel
    {
        public Triplet(Throw first, Throw second, Throw third) { throws = new ObservableCollection<Throw> { first, second, third }; }
        public ObservableCollection<Throw> throws { get; set; }
    }
    
    public class Throw : GameModel
    {
        public Throw(BoardSegment segment) { this.segment = segment; }
        public BoardSegment segment { get; set; }
    }

    public class BoardSegment : GameModel
    {
        public virtual int Score { get; }
        public virtual string Display { get;  }
    }
    
    public class NormalSegment : BoardSegment
    {
        public NormalSegment(int value, SegmentModifier modifier) { this.value = value; this.modifier = modifier; }

        public int value;
        public SegmentModifier modifier;

        public override int Score { get => value * (int) modifier; }

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
        public NamedSegment(NamedSegmentType type) { this.segment = type; }

        public NamedSegmentType segment;

        public override int Score { get => (int) segment; }


        public override string Display
        {
            get
            {
                if (segment == NamedSegmentType.OUTER_BULLSEYE) return "B";
                if (segment == NamedSegmentType.INNER_BULLSEYE) return "BS";
                if (segment == NamedSegmentType.OUTSIDE_BOARD)  return "X";
                return "?";
            }
        }
    }
}
