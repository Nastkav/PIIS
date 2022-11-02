using System;
using System.Windows.Media;

namespace domineering_gui.Model
{
    public class Cell : ICloneable
    {
        public int Index { get; private set; }
        public int H { get; private set; }
        public int W { get; private set; }
        public UsedType CellUsed { get; set; }
        public Cell(int h,int w,int id)
        {
            H=h;
            W=w;
            Index = id;
            CellUsed = UsedType.None;
        }

        public bool Active => CellUsed == UsedType.None;
        //public bool Selected { get; set; }
        public Brush ColorBrush
        {
            get
            {   
                //if (Selected)
                //    return Brushes.LightBlue;
                switch (CellUsed)
                {
                    case UsedType.None: return Brushes.WhiteSmoke;
                    case UsedType.Player1: return Brushes.Blue;
                    case UsedType.Player2: return Brushes.Red;
                    default: return Brushes.Black;
                }
            }
        }

        public object Clone() => new Cell(H, W, Index) { CellUsed = this.CellUsed};
    }
}
