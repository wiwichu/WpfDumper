using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfDumper.Helpers;

namespace WpfDumper.ViewModel
{
    public class TradeViewModel : ValidatorBase
    {
        private string portfolio;
        private string instrument;
        private string counterparty;
        private DateTime tradeDate;
        private DateTime valueDate;
        private double amount;
        private double price;
        [Required]
        [MinLength(5)]
        [MaxLength(5)]
        public string Portfolio
        {
            get
            {
                return portfolio;
            }
            set
            {
                SetProperty(ref portfolio, value);
            }
        }
        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Instrument
        {
            get
            {
                return instrument;
            }
            set
            {
                SetProperty(ref instrument, value);
            }
        }
        public string Counterparty
        {
            get
            {
                return counterparty;
            }
            set
            {
                SetProperty(ref counterparty, value);
            }
        }
        [Required]
        public DateTime TradeDate
        {
            get
            {
                return tradeDate;
            }
            set
            {
                SetProperty(ref tradeDate, value);
            }
        }
        [Required]
        public DateTime ValueDate
        {
            get
            {
                return valueDate;
            }
            set
            {
                SetProperty(ref valueDate, value);
            }
        }
        [Required]
        public double Amount
        {
            get
            {
                return amount;
            }
            set
            {
                SetProperty(ref amount, value);
            }
        }
        [Required]
        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                SetProperty(ref price, value);
                GetPriceErrors(price).ContinueWith((errorsTask) =>
                {
                    TryAddPropertyError(errorsTask.Result);
                });
            }
        }
        private Action<object, ValidationErrorEventArgs > priceError = (o,e)=> 
        {
            Debug.Print(e.Error.ToString());
        };
        public Action<object , ValidationErrorEventArgs > PriceError
        {
            get{return priceError;}
        }
        private async Task<List<string>> GetPriceErrors(double value)
        {
            return await Task.Run(() => { return ValidatePrice(value); });
        }

        private List<string> ValidatePrice(double value)
        {
            Task.Delay(5000).Wait();
            List<string> result = null;
            if(value<0)
            {
                result = new List<string>();
                result.Add("Price cannot be negative");
            }
            return result;
        }
    }
}
