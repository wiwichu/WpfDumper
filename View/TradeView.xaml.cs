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
    /// Interaction logic for ValidationView.xaml
    /// </summary>
    public partial class TradeView : Window
    {
        public TradeView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // Load data by setting the CollectionViewSource.Source property:
            // tradeViewModelViewSource.Source = [generic data source]
        }

        private void tbPrice_Error(object sender, ValidationErrorEventArgs e)
        {

        }


        public string StringDP
        {
            get { return (string)GetValue(StringDPProperty); }
            set { SetValue(StringDPProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StringDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StringDPProperty =
            DependencyProperty.Register("StringDP", typeof(string), typeof(TradeView), new PropertyMetadata("Hi!",stringChanged));

        private static void stringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
