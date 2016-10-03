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
    class ValidatorBase : NotifyPropertyChanged, INotifyDataErrorInfo
    {
        private ConcurrentDictionary<string, List<string>> errors = new ConcurrentDictionary<string, List<string>>();
        public bool HasErrors
        {
            get
            {
                return errors.Count > 0;
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> result = null;
            errors.TryGetValue(propertyName, out result);
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
                errors[caller] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                List<string> removed = null;
                errors.TryRemove(caller, out removed);
            }
            ErrorsChanged(this, new DataErrorsChangedEventArgs(caller));
        }
    }
}
