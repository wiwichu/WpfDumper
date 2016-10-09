using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfDumper.Helpers;

namespace WpfDumper.Model
{
    public class Tick : ValidatorBase
    {
        public enum TICKDIRECTION
        {
            NONE,
            UPTICK,
            DOWNTICK
        }
        private TICKDIRECTION tickDirection = TICKDIRECTION.NONE;
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
                timeStamp = value;
            }
        }
        public TICKDIRECTION TickDirection
        {
            get { return tickDirection; }
            set
            {
                SetProperty(ref tickDirection, value);
            }
        }
        private object refreshLock = new object();
        private decimal tickValue;
        public decimal TickValue
        {
            get
            {
                lock (refreshLock)
                {
                    return tickValue;
                }
            }
            set
            {
                lock (refreshLock)
                {
                    if (tickValue != 0)
                    {
                        if ((value - tickValue) > 0)
                        {
                            tickDirection = TICKDIRECTION.UPTICK;
                        }
                        else if ((value - tickValue) < 0)
                        {
                            tickDirection = TICKDIRECTION.DOWNTICK;
                        }
                        else
                        {
                            tickDirection = TICKDIRECTION.NONE;
                        }
                    }
                    tickValue = value;
                    //SetProperty(ref tickValue, value);
                    //OnPropertyChanged("TickDirection");
                }
            }
        }
        public Brush RowBrush
        {
            get
            {
                return TickBrush;
            }
        }
        private Brush tickBrush = Brushes.Transparent;
        public Brush TickBrush
        {
            get
            {
                lock (refreshLock)
                {
                    return tickBrush;
                }
            }
        }
    }
}
