using DartTracker.Models;
using DartTracker.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DartTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ExpectedObjects;

namespace UnitTests
{
    [TestClass]
    public class TestTournamentSerializer
    {
        private Tournament context;
        private TournamentViewModel viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            var tournament = new Tournament
            {
                GamesToWin = 1,
                Players = new List<Player> {new Player {Name = "Henk"}, new Player {Name = "Piet"}},
                Winner = new Player {Name = "Henk"},
                Games = new List<Game>
                {
                    new Game()
                    {
                        Winner = new Player {Name = "Henk"},
                        setsAmount = 1,
                        legsAmount = 1,
                        gameSets = new List<GameSet>()
                        {
                            new GameSet()
                            {
                                legs = new List<GameLeg>()
                                {
                                    new GameLeg()
                                    {
                                        Winner = new Player {Name = "Henk"},
                                        CurrentTurn = new Player {Name = "Henk"},
                                        history = new Dictionary<Player, ObservableCollection<Triplet>>()
                                        {
                                            {
                                                "Henk",
                                                new ObservableCollection<Triplet>
                                                {
                                                    new Triplet(
                                                        new Throw(SegmentParser.parse("10T")),
                                                        new Throw(SegmentParser.parse("10D")),
                                                        new Throw(SegmentParser.parse("10"))
                                                    )
                                                }
                                            },
                                            {
                                                "Piet",
                                                new ObservableCollection<Triplet>
                                                {
                                                    new Triplet(
                                                        new Throw(SegmentParser.parse("20T")),
                                                        new Throw(SegmentParser.parse("20D")),
                                                        new Throw(SegmentParser.parse("20"))
                                                    )
                                                }
                                            }
                                        },
                                        ScoreHistory = new Dictionary<string, ObservableCollection<int>>()
                                        {
                                            {
                                                "Henk",
                                                new ObservableCollection<int>()
                                                {
                                                    10,
                                                    20,
                                                    30,
                                                }
                                            },
                                            {
                                                "Piet",
                                                new ObservableCollection<int>()
                                                {
                                                    10,
                                                    20,
                                                    30,
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // mabey make a factory so that you automaticaly generate a correct tournament but that is a bit overkill for the size of this project
            context = tournament;
            viewModel = new TournamentViewModel(tournament);
        }

        [TestMethod]
        public void SerializeTournament()
        {
            var jsonString = viewModel.Serialize();
            Assert.IsNotNull(jsonString);
            Assert.IsTrue(IsValidJson(jsonString));
        }

        private static bool IsValidJson(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            input = input.Trim();
            if ((input.StartsWith("{") && input.EndsWith("}")) || //For object
                (input.StartsWith("[") && input.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(input);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
                return false;
        }


        [TestMethod]
        public void DeserializeTournament()
        {
            var jsonString = viewModel.Serialize();
            var tournament = LoadTournamentJson.LoadTournament(jsonString).ToExpectedObject();
            tournament.ShouldEqual(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }
    }
}