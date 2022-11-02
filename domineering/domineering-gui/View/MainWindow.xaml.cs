using domineering_gui.Model;
using domineering_gui.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace domineering_gui
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new DomineeringViewModel();
            InitializeComponent();

        }
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            //var cell = sender as Button;
            //Brush st = cell.Background;
            //st = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            //var grid = cell.Parent as Grid;
            //var domVM = grid.DataContext as DomineeringViewModel;
            //if (Grid.GetRow(cell) != 0)
            //{
            //    Button upCell = grid.Children[domVM.Board.GetCellId(Grid.GetRow(cell) - 1, Grid.GetColumn(cell))] as Button;
            //    upCell.Background = st;
            //}
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {

            //Brush enterBrush = new SolidColorBrush(Color.FromRgb(190, 230, 253));

            Button cell = sender as Button;
            
            //Cell cellData = cell.DataContext as Cell;
            //Button nCell;


            //if (cellData.H > 0)
            //{
            //    nCell = new Button();
            //    cell.Background = enterBrush;
            //    nCell.Background = enterBrush;
            //}

            //var cell = sender as Button;
            //var grid = cell.Parent as Grid;
            //var domVM = grid.DataContext as DomineeringViewModel;
            //if (Grid.GetRow(cell) != 0)
            //{
            //Button upCell = grid.Children[domVM.Board.GetCellId(Grid.GetRow(cell) - 1, Grid.GetColumn(cell))] as Button;
            //upCell.Background = st;
            //}
            //domineeringVM.Board.BoardSize
            //var context = grid.DataContext;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
