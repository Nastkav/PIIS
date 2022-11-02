using domineering_gui.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Input;

namespace domineering_gui.ViewModel
{
    public class StartCommand : ICommand
    {
        protected DomineeringViewModel _dvm;
        public StartCommand(DomineeringViewModel dvm)
        {
            _dvm = dvm;
        }

        public event EventHandler CanExecuteChanged;

        //Обновление параметров
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter)
        {
            _dvm.StatusOutput = "Create Board";
            _dvm.Board.CellsReset();
            _dvm.OnPropertyChanged(nameof(_dvm.Map));
            _dvm.StatusOutput = "Game Started";
            ABDistance abd = new ABDistance(3);
            abd.MinMax(_dvm.Board, 3, true);
        }
    }
}
