using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartTracker
{
    /// <summary>
    /// This is the main model
    /// </summary>
    public class MatchModel
    {
        public Match match { get; set; }
    }

    public class Match
    {
        public int total9darters { get; set; }
        public int total180s { get; set; }
        public Speler player1 { get; set; }
        public Speler player2 { get; set; }
    }

    public class Speler
    {
        public bool IsStartingPlayer { get; set; }
        public string PlayerName { get; set; }
        public string OpponentName { get; set; }
        public List<Set> Sets { get; set; }
    }

    public class Set
    {
        public bool SetWon { get; set; }
        public List<Legs> legs { get; set; }
    }

    public class Legs
    {
        public float AverageThrown { get; set; }
        public List<Turn> Turns { get; set; }
    }

    public class Turn
    {
        public int TurnNumber { get; set; }
        public string ThrowOne { get; set; }
        public string ThrowTwo { get; set; }
        public string ThrowThree { get; set; }
        public float AverageThrownInTurn { get; set; }
    }
    
}

