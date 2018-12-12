using System;
using System.Collections.Generic;
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

// MVVM in depth
// https://github.com/YvesGingras/Course-WpfMvvmInDepth.git

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataBindingObject dbo;

        public MainWindow()
        {
            InitializeComponent();

            // controls
            dbo = new DataBindingObject()
            {
                Name = "FIRST",
                StartDate = DateTime.Now              
            };
            DataContext = dbo;

            // combobox
            dCombo.ItemsSource = DataBindingObject.GetDataBindingObjects();
            dCombo.SelectedIndex = 0;

            // order book
            dataGrid.ItemsSource = DataBindingObject.GetDataGridObjects();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            mybrowser.Navigate("https://charting-library.tradingview.com/?lang=en");

            // CHANGED, should fire an event OnPropertyChanged
            dbo.Name = "SECOND";
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Help");
        }
    }
}
