using DartTracker.Models;
using System.Windows.Input;
using DartTracker.Commands;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DartTracker.Utility;
using System;
using System.Text;
using Microsoft.Win32;

namespace DartTracker.ViewModels
{
    public class TournamentViewModel
    {
        private Tournament _tournament;

        public Tournament Tournament => _tournament;

        public TournamentViewModel(Tournament tournament)
        {
            _tournament = tournament;
            SerializeTournamentCommand = new SerializeTournamentCommand(this);
        }


        public ICommand SerializeTournamentCommand { get; private set; }

        public void SaveJson()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JSON-Formatted Data (*json)|*.json";

            if (dialog.ShowDialog() ?? false)
            {
                string jsonString = Serialize();
                File.WriteAllText(dialog.FileName, jsonString, Encoding.UTF8);
            }
        }

        public string Serialize()
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.Auto;
            string jsonString = JsonConvert.SerializeObject(_tournament, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                    TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
                });
            return jsonString;
        }

        public bool CanSerializeTournament()
        {
            if (Tournament.Players[0] == Tournament.Players[1])
                return false;
            return true;
        }
    }
}