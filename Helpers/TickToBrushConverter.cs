using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using static WpfDumper.Model.Tick;

namespace WpfDumper.Helpers
{
    //[ValueConversion(typeof(TICKDIRECTION), typeof(Brush))]
    class TickToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TICKDIRECTION))
            {
                return Brushes.Transparent;
            }
            TICKDIRECTION tickDirection = (TICKDIRECTION)value;
            switch (tickDirection)
            {
                case TICKDIRECTION.DOWNTICK:
                    {
                        return Brushes.Red;
                    }
                case TICKDIRECTION.UPTICK:
                    {
                        return Brushes.Green;
                    }
                default:
                    {
                        return Brushes.Transparent;
                    }
            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
