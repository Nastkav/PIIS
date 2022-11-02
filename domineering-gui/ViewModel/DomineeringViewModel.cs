using domineering_game;
using domineering_game.game;
using domineering_gui.Model;
using domineering_gui.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using System.Windows.Data;
using System.Windows.Input;

namespace domineering_gui.ViewModel
{
    public class DomineeringViewModel : INotifyPropertyChanged
    {
        const int MAX_WIDTH = 8;
        const int MAX_HEIGHT = 8;
        readonly AlBePruningAi pruningAi;
        bool boardActivated;
        string _statusOutput;
        public string StatusOutput
        {
            set
            {
                _statusOutput = value;
                OnPropertyChanged(nameof(StatusOutput));

            }
            get => _statusOutput;
        }

        //Level params
        private VmLevel selectedLevel;
        public VmLevel SelectedLevel
        {
            get { return selectedLevel; }
            set {
                selectedLevel = value;
                pruningAi.Level = value.Id;
                StatusOutput = "Рівень складності оновлено на " + value.Name;
            }
        }
        public ObservableCollection<VmLevel> AllLevels { get; set; }

        //Game controls
        public DomineeringGame Game { get; private set; }
        public DomineeringViewModel()
        {
            boardActivated = false;
            IPlayer player1 = new Player(1, MoveType.Vertical);
            IPlayer player2 = new Player(2, MoveType.Horizontal);
            Game = new DomineeringGame(player1, player2, MAX_HEIGHT, MAX_WIDTH);
            pruningAi = new AlBePruningAi(0, player2, player1);

            AllLevels = new ObservableCollection<VmLevel>()
            {
                new VmLevel(1,"Легкий"),
                new VmLevel(2,"Середній"),
                new VmLevel(3,"Складний")
            };
            SelectedLevel = AllLevels[0];
            StatusOutput = "Виберіть параметри гри";

            //Назначение действий для команд
        }

        public VmCell[][] Map
        {
            get
            {
                VmCell[][] result = new VmCell[Game.Board.Cells.Length][];
                int sizeH = Game.Board.Cells.Length;
                for (int h = 0; h < sizeH; h++)
                {
                    int sizeW = Game.Board.Cells[h].Length;
                    result[h] = new VmCell[sizeW];
                    for (int w = 0; w < sizeW; w++)
                        result[h][w] = new VmCell(h, w, Game.Board.Cells[h][w], boardActivated, this);

                }
                return result;
            }
        }

        internal void SelectCell(int h, int w)
        {
            bool moved = Game.Move(h, w);
            StatusOutput = $"Успешность выполнения шага: {moved}";
            if (!moved)
                return;

            //BotMove
            moved = false;
            while (!moved)
            {

                int heurValue = pruningAi.CalcBestMove(Game.Board, out int h2, out int w2);
                if (h2 == int.MinValue & w2 == int.MinValue)
                    break;

                moved = Game.Move(h2, w2);
                StatusOutput = $"BotMove: h={h2},w={w2}, heurValue = {heurValue} | Успешность выполнения шага: {moved}";

                if (Game.Winner != null)
                    break;
            }

            if (Game.Winner != null)
            {
                //Запись завершения игры
                StatusOutput = $"Игра закончена,  {(Game.Winner.Id > 0 ? Game.Winner.Id.ToString("Победил 0") : "Ничья")}";
                boardActivated = false;
            }

            OnPropertyChanged(nameof(Map));
        }


        #region EventH
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion EventH

        //Commands
        private RelayCommand startGame;
        public ICommand StartGame
        {
            get
            {
                if (startGame == null)
                {
                    startGame = new RelayCommand(PerformStartGame);
                }

                return startGame;
            }
        }
     
        private void PerformStartGame(object commandParameter)
        {
            Game.StartNewGame();
            StatusOutput = "Нова гра починаеться";
            boardActivated = true;
            OnPropertyChanged(nameof(Map));
        }
    }

    public class RelayCommand : ICommand
    {
        #region Fields 
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        #endregion // Fields 
        #region Constructors 
        public RelayCommand(Action<object> execute) : this(execute, null) { }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute"); _canExecute = canExecute;
        }
        #endregion // Constructors 
        #region ICommand Members 
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter) { _execute(parameter); }
        #endregion // ICommand Members 
    }
}
