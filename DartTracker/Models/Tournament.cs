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
        SINGLE,
        DOUBLE,
        TRIPLE
    }

    public enum NamedSegmentType
    {
        OUTER_BULLSEYE = 25,
        INNER_BULLSEYE = 50,
        OUTSIDE_BOARD = 0
    }

    public class Tournament
    {
        public Player[] Players { get; set; }
        public Game[] Games { get; set; }
        public int GamesToWin { get; set; }
        public Player? Winner { get; set; }
    }

    public class Player
    {
        public string Name { get; set; }
    }

    public class Game
    {
        public Player? Winner { get; set; }
    }

    public class GameSet : Game
    {
        public Leg[] legs { get; set; }
        public int LegsToWin { get; set; }
    }

    public class Leg : Game
    {
        public Tuple<Player, Triplet[]>[] history { get; set; }
        public Tuple<Player, int> Scores { get; set; }
        public Player turn { get; set; }
    }

    public class Triplet
    {
        public Throw[] Throws { get; set; }
    }
    /// <summary>
    /// TODO: maak virtuele classes
    /// </summary>
    public class Throw
    {
        public BoardSegment boardSegment { get; set; }
    }

    public class BoardSegment
    {
        public bool normalSegment { get; set; }
    }
    
    public class NormalSegment : BoardSegment
    {
        public int value { get; set; }
        public SegmentModifier modifier { get; set; }
    }
    public class NamedSegment : BoardSegment
    {
        public int value { get; set; }
        public NamedSegmentType segment { get; set; }
    }
}
