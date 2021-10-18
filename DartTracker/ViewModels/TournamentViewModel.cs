using DartTracker.Models;
using System.Windows.Input;
using DartTracker.Commands;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DartTracker.Utility;
using System;

namespace DartTracker.ViewModels
{
    class TournamentViewModel
    {
        private Tournament _tournament;

        public Tournament Tournament => _tournament;

        public TournamentViewModel(Tournament tournament)
        {
            _tournament = tournament;
            SerializeTournamentCommand = new SerializeTournamentCommand(this);
        }


        public ICommand SerializeTournamentCommand
        {
            get;
            private set;
        }
        public ICommand LoadTournamentCommand
        {
            get;
            private set;
        }




        public void Serialize()
        {
           /* var t = new Tournament
            {
                GamesToWin = 1,
                Players = new List<Player> { new Player { Name = "Henk" }, new Player { Name = "Piet" } },
                Winner = new Player { Name = "Henk" },
                Games = new List<Game> { new Game() {
                        Winner = new Player{Name = "Henk"},
                        setsAmount = 1,
                        legsAmount = 1,
                        gameSets = new List<GameSet>() { new GameSet() { legs = new List<GameLeg>() {
                        new GameLeg() {
                           Winner= new Player{Name = "Henk"},
                           CurrentTurn= new Player{Name = "Henk"},
                           history=new Dictionary<Player, ObservableCollection<Triplet>>(){
                               {
                                   new Player{Name = "Henk"},
                                   new ObservableCollection<Triplet>{ new Triplet(
                                    new Throw(SegmentParser.parse("10")),
                                    new Throw(SegmentParser.parse("10")),
                                    new Throw(SegmentParser.parse("10"))
                                    )}
                               },
                               {
                                   new Player{Name = "Piet"},
                                   new ObservableCollection<Triplet>{ new Triplet(
                                    new Throw(SegmentParser.parse("10")),
                                    new Throw(SegmentParser.parse("10")),
                                    new Throw(SegmentParser.parse("10"))
                                    )}
                               }
                           }
                        }
                    }
                }
                }
                }
                }*/
            //};

            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.Auto;
            string jsonString = JsonConvert.SerializeObject(_tournament, Formatting.Indented, new JsonSerializerSettings() {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            });

            File.WriteAllText("..\\..\\..\\tournament.json", jsonString); // add file explorer later for ease of use 
        }

        public bool CanSerializeTournament()
        {
            if (Tournament.Players[0] == Tournament.Players[1])
                return false;
            return true;
        }

    }
}