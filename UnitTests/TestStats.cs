using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartTracker.Models;
using DartTracker.Utility;
using DartTracker.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestStats
    {
        private Tournament context;
        private StatsWindowViewModel viewModel;
        private AveragesScores averagesScores;

        [TestInitialize]
        public void TestInitialize()
        {
            var tournament = new Tournament
            {
                GamesToWin = 1,
                Players = new List<Player> { new Player { Name = "Henk" }, new Player { Name = "Piet" } },
                Winner = new Player { Name = "Henk" },
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
                                        history = new Dictionary<int, ObservableCollection<Triplet>>()
                                        {
                                            {
                                                0,
                                                new ObservableCollection<Triplet>
                                                {
                                                    new Triplet(
                                                        new Throw(SegmentParser.parse("10T")),
                                                        new Throw(SegmentParser.parse("10D")),
                                                        new Throw(SegmentParser.parse("10"))
                                                    )
                                                    // average in leg = 20
                                                }
                                            },
                                            {
                                                1,
                                                new ObservableCollection<Triplet>
                                                {
                                                    new Triplet(
                                                        new Throw(SegmentParser.parse("20T")),
                                                        new Throw(SegmentParser.parse("20D")),
                                                        new Throw(SegmentParser.parse("20"))
                                                    ),
                                                    new Triplet(
                                                        new Throw(SegmentParser.parse("10D")),
                                                        new Throw(SegmentParser.parse("20D")),
                                                        new Throw(SegmentParser.parse("X"))
                                                    )
                                                    // average in leg = 30

                                                }
                                            }
                                        },
                                        ScoreHistory = new Dictionary<int, ObservableCollection<int>>()
                                        {
                                            {
                                                0,
                                                new ObservableCollection<int>()
                                                {
                                                    10,
                                                    20,
                                                    30,
                                                }
                                            },
                                            {
                                                1,
                                                new ObservableCollection<int>()
                                                {
                                                    10,
                                                    20,
                                                    30,
                                                }
                                            }
                                        }
                                    },
                                    new GameLeg()
                                    {
                                        Winner = new Player {Name = "Henk"},
                                        CurrentTurn = new Player {Name = "Henk"},
                                        history = new Dictionary<int, ObservableCollection<Triplet>>()
                                        {
                                            {
                                                0,
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
                                                1,
                                                new ObservableCollection<Triplet>
                                                {
                                                    new Triplet(
                                                        new Throw(SegmentParser.parse("20T")),
                                                        new Throw(SegmentParser.parse("20D")),
                                                        new Throw(SegmentParser.parse("20"))
                                                    ),
                                                    new Triplet(
                                                        new Throw(SegmentParser.parse("10D")),
                                                        new Throw(SegmentParser.parse("20D")),
                                                        new Throw(SegmentParser.parse("X"))
                                                    )
                                                    // average = 30
                                                }
                                            }
                                        },
                                        ScoreHistory = new Dictionary<int, ObservableCollection<int>>()
                                        {
                                            {
                                                0,
                                                new ObservableCollection<int>()
                                                {
                                                    10,
                                                    20,
                                                    30,
                                                }
                                            },
                                            {
                                                1,
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
                            },
                            new GameSet()
                            {
                                legs = new List<GameLeg>()
                                {
                                    new GameLeg()
                                    {
                                        Winner = new Player {Name = "Henk"},
                                        CurrentTurn = new Player {Name = "Henk"},
                                        history = new Dictionary<int, ObservableCollection<Triplet>>()
                                        {
                                            {
                                                0,
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
                                                1,
                                                new ObservableCollection<Triplet>
                                                {
                                                    new Triplet(
                                                        new Throw(SegmentParser.parse("20T")),
                                                        new Throw(SegmentParser.parse("20D")),
                                                        new Throw(SegmentParser.parse("20"))
                                                    ),
                                                    new Triplet(
                                                        new Throw(SegmentParser.parse("10D")),
                                                        new Throw(SegmentParser.parse("20D")),
                                                        new Throw(SegmentParser.parse("X"))
                                                    )
                                                }
                                            }
                                        },
                                        ScoreHistory = new Dictionary<int, ObservableCollection<int>>()
                                        {
                                            {
                                                0,
                                                new ObservableCollection<int>()
                                                {
                                                    10,
                                                    20,
                                                    30,
                                                }
                                            },
                                            {
                                                1,
                                                new ObservableCollection<int>()
                                                {
                                                    10,
                                                    20,
                                                    30,
                                                }
                                            }
                                        }
                                    },
                                    new GameLeg()
                                    {
                                        Winner = new Player {Name = "Henk"},
                                        CurrentTurn = new Player {Name = "Henk"},
                                        history = new Dictionary<int, ObservableCollection<Triplet>>()
                                        {
                                            {
                                                0,
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
                                                1,
                                                new ObservableCollection<Triplet>
                                                {
                                                    new Triplet(
                                                        new Throw(SegmentParser.parse("20T")),
                                                        new Throw(SegmentParser.parse("20D")),
                                                        new Throw(SegmentParser.parse("20"))
                                                    ),
                                                    new Triplet(
                                                        new Throw(SegmentParser.parse("10D")),
                                                        new Throw(SegmentParser.parse("20D")),
                                                        new Throw(SegmentParser.parse("X"))
                                                    )
                                                }
                                            }
                                        },
                                        ScoreHistory = new Dictionary<int, ObservableCollection<int>>()
                                        {
                                            {
                                                0,
                                                new ObservableCollection<int>()
                                                {
                                                    10,
                                                    20,
                                                    30,
                                                }
                                            },
                                            {
                                                1,
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
            context = tournament;
            viewModel = new StatsWindowViewModel(tournament);
            averagesScores = AveragesScores.Instantance;
        }
        [TestMethod()]
        public void TestAverageInLeg()
        {
            var leg = context.Games.First().gameSets.First().legs.First();
            var result = averagesScores.CalculateAverageScoreInLeg(leg.history);
            Assert.AreEqual(60, result[0].Item1);
            Assert.AreEqual(90, result[1].Item1);
        }
        [TestMethod]
        public void TestAverageInSet()
        {
            var set = context.Games.First().gameSets.First();
            var result = averagesScores.CalculateAverageScoreInSet(set);


        }

        [TestMethod]
        public void TestAverageInGame()
        {
            var game = context.Games.First();
            var result = averagesScores.CalculateAverageScoreInGame(game);
            Assert.AreEqual(60, result[0].Item1);
            Assert.AreEqual(90, result[1].Item1);
            Assert.AreEqual(12, result[0].Item2);
            Assert.AreEqual(24, result[1].Item2);
        }

        [TestMethod]
        public void Test180SInGame()
        {
            var game = new Game()
            {
                Winner = new Player { Name = "Henk" },
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
                                history = new Dictionary<int, ObservableCollection<Triplet>>()
                                {
                                    {
                                        0,
                                        new ObservableCollection<Triplet>
                                        {
                                            new Triplet(
                                                new Throw(SegmentParser.parse("20T")),
                                                new Throw(SegmentParser.parse("20T")),
                                                new Throw(SegmentParser.parse("20T"))
                                            )
                                            // average in leg = 20
                                        }
                                    },
                                    {
                                        1,
                                        new ObservableCollection<Triplet>
                                        {
                                            new Triplet(
                                                new Throw(SegmentParser.parse("20T")),
                                                new Throw(SegmentParser.parse("20T")),
                                                new Throw(SegmentParser.parse("20T"))
                                            ),
                                            new Triplet(
                                                new Throw(SegmentParser.parse("10D")),
                                                new Throw(SegmentParser.parse("20D")),
                                                new Throw(SegmentParser.parse("X"))
                                            )
                                            // average in leg = 30

                                        }
                                    }
                                },
                                ScoreHistory = new Dictionary<int, ObservableCollection<int>>()
                                {
                                    {
                                        0,
                                        new ObservableCollection<int>()
                                        {
                                            10,
                                            20,
                                            30,
                                        }
                                    },
                                    {
                                        1,
                                        new ObservableCollection<int>()
                                        {
                                            10,
                                            20,
                                            30,
                                        }
                                    }
                                }
                            },
                            new GameLeg()
                            {
                                Winner = new Player {Name = "Henk"},
                                CurrentTurn = new Player {Name = "Henk"},
                                history = new Dictionary<int, ObservableCollection<Triplet>>()
                                {
                                    {
                                        0,
                                        new ObservableCollection<Triplet>
                                        {
                                            new Triplet(
                                                new Throw(SegmentParser.parse("20T")),
                                                new Throw(SegmentParser.parse("20T")),
                                                new Throw(SegmentParser.parse("20T"))
                                            )
                                        }
                                    },
                                    {
                                        1,
                                        new ObservableCollection<Triplet>
                                        {
                                            new Triplet(
                                                new Throw(SegmentParser.parse("20T")),
                                                new Throw(SegmentParser.parse("20T")),
                                                new Throw(SegmentParser.parse("20T"))
                                            ),
                                            new Triplet(
                                                new Throw(SegmentParser.parse("20T")),
                                                new Throw(SegmentParser.parse("20T")),
                                                new Throw(SegmentParser.parse("20T"))
                                            )
                                            // average = 30
                                        }
                                    }
                                },
                                ScoreHistory = new Dictionary<int, ObservableCollection<int>>()
                                {
                                    {
                                        0,
                                        new ObservableCollection<int>()
                                        {
                                            10,
                                            20,
                                            30,
                                        }
                                    },
                                    {
                                        1,
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
                    },
                    new GameSet()
                    {
                        legs = new List<GameLeg>()
                        {
                            new GameLeg()
                            {
                                Winner = new Player {Name = "Henk"},
                                CurrentTurn = new Player {Name = "Henk"},
                                history = new Dictionary<int, ObservableCollection<Triplet>>()
                                {
                                    {
                                        0,
                                        new ObservableCollection<Triplet>
                                        {
                                            new Triplet(
                                                new Throw(SegmentParser.parse("20T")),
                                                new Throw(SegmentParser.parse("20T")),
                                                new Throw(SegmentParser.parse("20T"))
                                            )
                                        }
                                    },
                                    {
                                        1,
                                        new ObservableCollection<Triplet>
                                        {
                                            new Triplet(
                                                new Throw(SegmentParser.parse("20T")),
                                                new Throw(SegmentParser.parse("20T")),
                                                new Throw(SegmentParser.parse("20T"))
                                            ),
                                            new Triplet(
                                                new Throw(SegmentParser.parse("10D")),
                                                new Throw(SegmentParser.parse("20D")),
                                                new Throw(SegmentParser.parse("X"))
                                            )
                                        }
                                    }
                                },
                                ScoreHistory = new Dictionary<int, ObservableCollection<int>>()
                                {
                                    {
                                        0,
                                        new ObservableCollection<int>()
                                        {
                                            10,
                                            20,
                                            30,
                                        }
                                    },
                                    {
                                        1,
                                        new ObservableCollection<int>()
                                        {
                                            10,
                                            20,
                                            30,
                                        }
                                    }
                                }
                            },
                            new GameLeg()
                            {
                                Winner = new Player {Name = "Henk"},
                                CurrentTurn = new Player {Name = "Henk"},
                                history = new Dictionary<int, ObservableCollection<Triplet>>()
                                {
                                    {
                                        0,
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
                                        1,
                                        new ObservableCollection<Triplet>
                                        {
                                            new Triplet(
                                                new Throw(SegmentParser.parse("20T")),
                                                new Throw(SegmentParser.parse("20D")),
                                                new Throw(SegmentParser.parse("20"))
                                            ),
                                            new Triplet(
                                                new Throw(SegmentParser.parse("10D")),
                                                new Throw(SegmentParser.parse("20D")),
                                                new Throw(SegmentParser.parse("X"))
                                            )
                                        }
                                    }
                                },
                                ScoreHistory = new Dictionary<int, ObservableCollection<int>>()
                                {
                                    {
                                        0,
                                        new ObservableCollection<int>()
                                        {
                                            10,
                                            20,
                                            30,
                                        }
                                    },
                                    {
                                        1,
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
            };
            var result = viewModel.GetNumberOf180SInGame(game);

            Assert.AreEqual(3, result[0]);
            Assert.AreEqual(4, result[1]);
        }


    }
}
