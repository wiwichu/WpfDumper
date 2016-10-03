using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfDumper.Helpers;

namespace WpfDumper.Model
{
    public class Tick : NotifyPropertyChanged
    {
        private enum TICKCOLOR
        {
            NONE,
            UPTICK,
            DOWNTICK
        }
        private TICKCOLOR tickColor = TICKCOLOR.NONE;
        private string symbol;
        public string Symbol
        {
            get
            {
                return symbol;
            }
            set
            {
                SetProperty(ref symbol,value);
            }
        }
        private DateTime timeStamp;
        public DateTime TimeStamp
        {
            get
            {
                return timeStamp;
            }
            set
            {
                
                SetProperty(ref timeStamp, value);
            }
        }
        private decimal tickValue;
        public decimal TickValue
        {
            get
            {
                return tickValue;
            }
            set
            {
                if (tickValue != 0)
                {
                    if ((value - tickValue) > 0)
                    {
                        tickColor = TICKCOLOR.UPTICK;
                    }
                    else if ((value - tickValue) < 0)
                    {
                        tickColor = TICKCOLOR.DOWNTICK;
                    }
                    else
                    {
                        tickColor = TICKCOLOR.NONE;
                    }
                }
                SetProperty(ref tickValue,value);
                //OnPropertyChanged("RowBrush");
                OnPropertyChanged("TickBrush");
                //Task.Run(async () =>
                //{
                //    await Task.Delay(5000);
                //    TICKCOLOR tcSave = tickColor;
                //    tickColor = TICKCOLOR.NONE;
                //    //OnPropertyChanged("RowBrush");
                //    //await Task.Delay(100);
                //    //tickColor = tcSave;
                //    //OnPropertyChanged("TickBrush");
                //});
            }
        }
        public Brush RowBrush
        {
            get
            {
                return TickBrush;
            }
        }
        public Brush TickBrush
        {
            get
            {
                switch(tickColor)
                {
                    case TICKCOLOR.DOWNTICK:
                        {
                            return Brushes.Red;
                        }
                    case TICKCOLOR.UPTICK:
                        {
                            return Brushes.Green;
                        }
                    default:
                        {
                            break;
                        }
                        }
                return Brushes.Transparent;
            }
        }
    }
}
