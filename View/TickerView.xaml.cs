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
using System.Windows.Shapes;

namespace WpfDumper.View
{
    /// <summary>
    /// Interaction logic for TickerView.xaml
    /// </summary>
    public partial class TickerView : Window
    {
        public TickerView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // Load data by setting the CollectionViewSource.Source property:
            // tickViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource tickViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tickViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // tickViewSource.Source = [generic data source]
        }

        private void tickDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
