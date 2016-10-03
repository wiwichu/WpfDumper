using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
        }

    }
}
