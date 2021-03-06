﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfDumper.Helpers
{
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {

        public NotifyPropertyChanged()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                return;
            }

        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected virtual void SetProperty<T>(ref T member,T val,[CallerMemberName] string caller = "")
        {
            if (object.Equals(member, val)) return;
            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
        virtual public void RegisterWithMessenger() { }
        virtual public void UnregisterWithMessenger() { }
    }
}
