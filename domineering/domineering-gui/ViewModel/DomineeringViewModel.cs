using domineering_gui.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;

namespace domineering_gui.ViewModel
{
    public class DomineeringViewModel : INotifyPropertyChanged
    {
        private Level _selectedLevel;
        public Level SelectedLevel
        {
            get => _selectedLevel;
            set
            {
                _selectedLevel = value;
                OnPropertyChanged("StartGame");
            }

        }
        public ObservableCollection<Level> AllLevels { get; set; }
        Board _board;
        public Board Board { get => _board; }
        string _statusOutput;
        private ICommand _startCommand;
        public DomineeringViewModel()
        {
            AllLevels = new ObservableCollection<Level>()
            {
                new Level(1,"Легкий"),
                new Level(2,"Середній"),
                new Level(3,"Складний")
            };
            SelectedLevel = AllLevels[0];
            _board = new Board(8, 8);
            StatusOutput = "Выберите размер доски и параметры для игры";
        }
        public string StatusOutput
        {
            set
            {
                _statusOutput = value;
                OnPropertyChanged(nameof(StatusOutput));

            }
            get => _statusOutput;
        }
        public ICommand StartGame
        {
            get
            {
                if (_startCommand == null)
                    _startCommand = new StartCommand(this);
                return _startCommand;
            }
        }
        public Cell[][] Map { get => Board.Cells; }

        #region EventH
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion EventH
    }
}
