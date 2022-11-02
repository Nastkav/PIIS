using domineering_gui.ViewModel;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;

namespace domineering_gui.Model
{
    public class VmCell : INotifyPropertyChanged
    {
        readonly DomineeringViewModel parent;
        public bool Active { get; set; }
        public int Value
        {
            get
            {
                int result = 0;
                if (H < parent.Map.Length - 1)
                    if (parent.Map[H + 1][W].Color == 0)
                        result++;
                if (W < parent.Map[H].Length - 1)
                    if (parent.Map[H][W + 1].Color == 0)
                        result++;
                if (H != 0)
                    if (parent.Map[H - 1][W].Color == 0)
                        result++;
                if (W != 0)
                    if (parent.Map[H][W - 1].Color == 0)
                        result++;
                return result;
            }
        }
        int H { get; set; }
        int W { get; set; }
        int MaxH { get; set; }

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged(nameof(Selected));
                OnPropertyChanged(nameof(ColorBrush));
            }
        }

        private int _color;
        public int Color
        {
            get { return _color; }
            private set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
                OnPropertyChanged(nameof(ColorBrush));
            }
        }




        //public bool Active => Value == 0;
        public VmCell(int h, int w, int color,bool active, DomineeringViewModel parent)
        {
            H = h;
            W = w;
            Color = color;
            MaxH = parent.Game.Board.Cells.Length;
            this.parent = parent;
            Active = active;
        }

        public int Id => H * MaxH + W;
        public Brush ColorBrush
        {
            get
            {
                switch (Color)
                {
                    case 0: return Brushes.WhiteSmoke;
                    case 1: return Brushes.Blue;    //Black
                    case 2: return Brushes.Red;     //Black
                    default: return Brushes.DeepPink;
                }
            }
        }


        #region EventH
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion EventH 

        private RelayCommand cellClick;
        public ICommand CellClick
        {
            get
            {
                if (cellClick == null)
                    cellClick = new RelayCommand(SelectCell);
                return cellClick;
            }
        }
        void SelectCell(object param) => parent.SelectCell(H, W);
    }
}
