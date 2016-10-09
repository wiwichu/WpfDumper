using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfDumper.Helpers
{
    public class ValidatorBase : NotifyPropertyChanged, INotifyDataErrorInfo
    {
        private ConcurrentDictionary<string, List<string>> propertyErrors = new ConcurrentDictionary<string, List<string>>();
        protected bool TryAddPropertyError(List<string> errorList, [CallerMemberName] string propertyName = "")
        {
            bool result = false;
            List<string> currentList = null;
            if (propertyErrors.TryGetValue(propertyName, out currentList))
            {
                List<string> newList = new List<string>();
                if (currentList!=null)
                {
                    newList.AddRange(currentList);
                }
                newList.AddRange(errorList);
                result = propertyErrors.TryUpdate(propertyName, currentList, newList);
            } 
            else
            {
                result = propertyErrors.TryAdd(propertyName, errorList);
            }
            if (result)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
            return result;
        }
        public bool HasErrors
        {
            get
            {
                return PropertyErrorsPresent();
            }
        }

        private bool PropertyErrorsPresent()
        {
            return propertyErrors.Values.Any((v) => v != null);
            //bool result = false;
            //foreach (var key in errors.Keys)
            //{
            //    if(errors[key] != null)
            //    {
            //        result = true;
            //        break;
            //    }
            //}
            //return result;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> result = null;
            propertyErrors.TryGetValue(propertyName, out result);
            return result;
        }
        protected override void SetProperty<T>(ref T member, T val, [CallerMemberName] string caller = "")
        {
            base.SetProperty(ref member, val, caller);
            ValidateProperty(caller, val);
        }

        private void ValidateProperty<T>(string caller, T val)
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext context = 
                new System.ComponentModel.DataAnnotations.ValidationContext(this);
            context.MemberName = caller;
            System.ComponentModel.DataAnnotations.Validator.TryValidateProperty(val, context, results);
            if (results.Any())
            {
                propertyErrors[caller] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                List<string> removed = null;
                propertyErrors.TryRemove(caller, out removed);
            }
            ErrorsChanged(this, new DataErrorsChangedEventArgs(caller));
        }
    }
}
