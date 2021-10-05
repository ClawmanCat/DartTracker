using System;
using System.Collections.Generic;
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
        public Player[] Players;
        public List<Game> Games;
        public int GamesToWin;
        public Player Winner;
    }

    public class Player
    {
        public string Name;
    }

    public abstract class Game
    {
        public Player Winner;
        public List<GameSet> gameSets;
    }

    public class GameSet 
    {
        public List<GameLeg> legs;
        public Player Winner;
    }

    public class GameLeg
    {
        public List<Dictionary<Player, List<Triplet>>> history;
        public Dictionary<Player, int> Scores;
        public Player Winner;
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
