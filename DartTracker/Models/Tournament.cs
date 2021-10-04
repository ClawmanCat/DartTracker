using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartTracker.Models
{
    /// <summary>
    /// Unused code
    /// </summary>
    public class Score
    {
        private int _score = 501;
        public int getScore()
        {
            return _score;
        }

        public void substractFromScore(int value)
        {
            _score = _score - value;
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
        public Player[] Players;
        public Game[] Games;
        public int GamesToWin;
        public Player Winner;
    }

    public class Player
    {
        public string Name;
    }

    public class Game
    {
        public Player Winner;
    }

    public class GameSet : Game
    {
        public GameLeg[] legs;
        public int LegsToWin;
    }

    public class GameLeg : Game
    {
        public Dictionary<Player, Triplet[]>[] history;
        public Dictionary<Player, int> Scores;
        public Player turn;
    }

    public class Triplet
    {
        public Throw[] Throws;
    }
    
    public class Throw
    {
        public BoardSegment boardSegment;
    }

    public class BoardSegment
    {
        public virtual int Score
        {
            get;set;
        }
    }
    
    public class NormalSegment : BoardSegment
    {
        public int value;
        public SegmentModifier modifier;
        public override int Score { get => base.Score; set => base.Score = value; }
    }
    public class NamedSegment : BoardSegment
    {
        public NamedSegmentType segment;
        public override int Score { get => base.Score; set => base.Score = value; }
    }
}
